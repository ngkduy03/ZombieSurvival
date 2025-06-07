using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// RiffleController is responsible for managing the riffle weapon's behavior.
/// </summary>
public class RiffleController : ControllerBase, IGunController
{
    private GunSetting gunSetting;
    private int currentAmmo;
    private int totalAmmo;
    private readonly Transform bulletSpawnPoint;

    // Pool ammo.
    private readonly Transform poolHolder;
    private Stack<BulletComponent> bulletPool;
    private bool isFiring = false;

    public RiffleController(
        GunSetting gunSetting,
        Transform bulletSpawnPoint,
        Transform poolHolder)
    {
        this.gunSetting = gunSetting;
        currentAmmo = this.gunSetting.MagazineSize;
        totalAmmo = this.gunSetting.TotalAmmo;
        this.bulletSpawnPoint = bulletSpawnPoint;
        this.poolHolder = poolHolder;
    }

    public async UniTask FireBullet(CancellationToken cancellationToken)
    {
        if (isFiring)
        {
            return;
        }

        isFiring = true;
        Debug.Log("Firing bullet");
        if (currentAmmo <= 0)
        {
            await ReloadAsync(cancellationToken);
        }

        currentAmmo--;
        PopBulletPool(cancellationToken);
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
        bulletPool.Push(bullet);
    }

    public async UniTask ReloadAsync(CancellationToken cancellationToken)
    {
        int needed = gunSetting.MagazineSize - currentAmmo;
        int reloadAmount = System.Math.Min(needed, totalAmmo);

        if (reloadAmount <= 0)
        {
            return;
        }

        await UniTask.Delay((int)(gunSetting.ReloadTime * 1000), cancellationToken: cancellationToken);
        currentAmmo += reloadAmount;
        totalAmmo -= reloadAmount;
    }
}