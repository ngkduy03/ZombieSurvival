using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// RiffleController is responsible for managing the riffle weapon's behavior.
/// </summary>
public class RiffleController : ControllerBase, IGunController
{
    private readonly Transform bulletSpawnPoint;
    private readonly Transform transform;
    private readonly AudioSource audioSource;
    private int currentAmmo;
    private int totalAmmo;

    // Pool ammo.
    private readonly Transform poolHolder;
    private Stack<BulletComponent> bulletPool = new();
    private bool isFiring = false;
    private bool isReloaded = false;
    private GunSetting gunSetting;

    /// <inheritdoc/>
    public GunSetting GunSetting => gunSetting;

    public RiffleController(
        Transform transform,
        GunSetting gunSetting,
        Transform bulletSpawnPoint,
        Transform poolHolder,
        AudioSource audioSource)
    {
        this.transform = transform;
        this.gunSetting = gunSetting;
        currentAmmo = this.gunSetting.MagazineSize;
        totalAmmo = this.gunSetting.TotalAmmo;
        this.bulletSpawnPoint = bulletSpawnPoint;
        this.poolHolder = poolHolder;
        this.audioSource = audioSource;
    }

    /// <inheritdoc/>
    public async UniTask FireBullet(CancellationToken cancellationToken)
    {
        isFiring = true;
        if (currentAmmo < 0)
        {
            await ReloadAsync(cancellationToken);
        }

        currentAmmo--;
        PopBulletPool(cancellationToken);
        audioSource.Stop();
        audioSource.PlayOneShot(gunSetting.FireClip);

        await UniTask.Delay((int)(gunSetting.FireRate * 1000), cancellationToken: cancellationToken);
        isFiring = false;
    }

    private void PopBulletPool(CancellationToken cancellationToken)
    {
        if (bulletPool.Count > 0)
        {
            var bullet = bulletPool.Pop();
            bullet.transform.position = bulletSpawnPoint.position;
            bullet.transform.rotation = bulletSpawnPoint.rotation;
            bullet.gameObject.SetActive(true);
            ReloadPoolAmmo(bullet, cancellationToken).Forget();
        }
        else
        {
            var bulletInstance = Object.Instantiate(gunSetting.BulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation, poolHolder);
            bulletInstance.gameObject.SetActive(true);
            ReloadPoolAmmo(bulletInstance, cancellationToken).Forget();
        }
    }

    private async UniTask ReloadPoolAmmo(BulletComponent bullet, CancellationToken cancellationToken)
    {
        await UniTask.WaitUntil(() => bullet.IsDeactivated, cancellationToken: cancellationToken);
        bullet.IsDeactivated = false;
        bulletPool.Push(bullet);
    }

    /// <inheritdoc/>
    public async UniTask ReloadAsync(CancellationToken cancellationToken)
    {
        Debug.Log("Attempting to reload");
        int needed = gunSetting.MagazineSize - currentAmmo;
        int reloadAmount = System.Math.Min(needed, totalAmmo);

        if (reloadAmount <= 0)
            return;

        isReloaded = true;
        audioSource.PlayOneShot(gunSetting.ReloadClip);
        await UniTask.Delay((int)(gunSetting.ReloadTime * 1000), cancellationToken: cancellationToken);
        currentAmmo += reloadAmount;
        totalAmmo -= reloadAmount;
        isReloaded = false;
    }

    /// <inheritdoc/>
    public void SetActive(bool isActive)
    {
        transform.gameObject.SetActive(isActive);
    }

    /// <inheritdoc/>
    public bool CanFire()
    {
        return !(totalAmmo <= 0 || isFiring);
    }

    /// <inheritdoc/>
    public bool IsReloaded()
    {
        return isReloaded;
    }

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        audioSource.Stop();
        isFiring = false;
        isReloaded = false;
    }
}