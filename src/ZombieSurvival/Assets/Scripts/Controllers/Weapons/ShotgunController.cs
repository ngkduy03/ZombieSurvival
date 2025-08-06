using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Controller for shotgun shooting behavior.
/// </summary>
public class ShotgunController : ControllerBase, IGunController
{
    private readonly Transform transform;
    private readonly AudioSource audioSource;
    private int currentAmmo;
    private int totalAmmo;
    private Transform pelletSpawnPoint;

    // Shotgun range check.
    private float spreadAngle;
    private float radius;
    private LayerMask enemyMask;
    private Collider[] enemyRangeChecks = new Collider[200];
    private bool isFiring = false;
    private bool isReloaded = false;
    private GunSetting gunSetting;

    /// <inheritdoc/>
    public GunSetting GunSetting => gunSetting;

    public ShotgunController(
        Transform transform,
        GunSetting gunSetting,
        Transform pelletSpawnPoint,
        AudioSource audioSource)
    {
        this.transform = transform;
        this.gunSetting = gunSetting;
        this.pelletSpawnPoint = pelletSpawnPoint;
        currentAmmo = this.gunSetting.MagazineSize;
        totalAmmo = this.gunSetting.TotalAmmo;
        spreadAngle = this.gunSetting.SpreadAngle;
        radius = this.gunSetting.Radius;
        enemyMask = this.gunSetting.EnemyMask;
        this.audioSource = audioSource;
    }

    private void CheckInRange()
    {
        var enemyAmount = Physics.OverlapSphereNonAlloc(pelletSpawnPoint.position, radius, enemyRangeChecks, enemyMask, QueryTriggerInteraction.Collide);

        if (enemyAmount > 0)
        {
            for (int i = 0; i < enemyAmount; i++)
            {
                var enemy = enemyRangeChecks[i];

                if (enemy == null)
                    continue;

                var enemyPos = enemyRangeChecks[0].transform.position;
                enemyPos.y = 0;
                var pelletSpawnPos = pelletSpawnPoint.position;
                pelletSpawnPos.y = 0;
                var direction = (enemyPos - pelletSpawnPos).normalized;

                // Check if the enemy is within the spread angle of the shotgun.
                // This assumes pelletSpawnPoint.forward is the direction the shotgun is facing.
                if (Vector3.Angle(pelletSpawnPoint.forward, direction) < spreadAngle / 2f)
                {
                    if (enemy.TryGetComponent<ZombieComponent>(out var zombieComponent))
                    {
                        zombieComponent.ZombieController.OnTakenDamage(gunSetting.Damage);
                    }
                }
            }
        }
        Array.Clear(enemyRangeChecks, 0, enemyRangeChecks.Length);
    }

    /// <inheritdoc/>
    public async UniTask FireBullet(CancellationToken cancellationToken)
    {
        if (isFiring || totalAmmo <= 0 || isReloaded)
        {
            return;
        }

        isFiring = true;
        if (currentAmmo < 0)
        {
            await ReloadAsync(cancellationToken);
        }

        currentAmmo--;
        CheckInRange();
        audioSource.Stop();
        audioSource.PlayOneShot(gunSetting.FireClip);
        await UniTask.Delay((int)(gunSetting.FireRate * 1000), cancellationToken: cancellationToken);
        isFiring = false;

    }

    /// <inheritdoc/>
    public async UniTask ReloadAsync(CancellationToken cancellationToken)
    {
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