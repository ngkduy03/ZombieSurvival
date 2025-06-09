using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ShotgunController : ControllerBase, IGunController
{
    private Transform transform;
    private GunSetting gunSetting;
    public GunSetting GunSetting => gunSetting;
    private int currentAmmo;
    private int totalAmmo;
    private Transform pelletSpawnPoint;

    // The angle of spread for the shotgun pellets.
    private float spreadAngle;
    private float radius;
    private LayerMask enemyMask;
    private Collider[] enemyRangeChecks = new Collider[200];
    private bool isFiring = false;
    private bool isReloaded = false;

    public ShotgunController(
        Transform transform,
        GunSetting gunSetting,
        Transform pelletSpawnPoint)
    {
        this.transform = transform;
        this.gunSetting = gunSetting;
        this.pelletSpawnPoint = pelletSpawnPoint;
        currentAmmo = this.gunSetting.MagazineSize;
        totalAmmo = this.gunSetting.TotalAmmo;
        spreadAngle = this.gunSetting.SpreadAngle;
        radius = this.gunSetting.Radius;
        enemyMask = this.gunSetting.EnemyMask;
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
                    break;

                var enemyPos = enemyRangeChecks[0].transform.position;

                // Check if the enemy is within the spread angle of the shotgun
                // This assumes pelletSpawnPoint.forward is the direction the shotgun is facing
                if (Vector3.Angle(pelletSpawnPoint.position, enemyPos) < spreadAngle / 2f)
                {
                    //TODO: Implement damage logic here
                    Debug.Log("Hit enemy");
                }
            }
        }

        Array.Clear(enemyRangeChecks, 0, enemyRangeChecks.Length);
    }

    /// <inheritdoc/>
    public async UniTask FireBullet(CancellationToken cancellationToken)
    {
        Debug.Log("Attempting to fire bullet");
        if (isFiring || totalAmmo <= 0 || isReloaded)
        {
            return;
        }

        isFiring = true;
        Debug.Log("Firing pellet");
        if (currentAmmo < 0)
        {
            await ReloadAsync(cancellationToken);
        }

        currentAmmo--;
        CheckInRange();
        await UniTask.Delay((int)(gunSetting.FireRate * 1000), cancellationToken: cancellationToken);
        isFiring = false;

    }

    /// <inheritdoc/>
    public async UniTask ReloadAsync(CancellationToken cancellationToken)
    {
        Debug.Log("Attempting to reload");
        int needed = gunSetting.MagazineSize - currentAmmo;
        int reloadAmount = System.Math.Min(needed, totalAmmo);

        if (reloadAmount <= 0)
        {
            return;
        }

        await UniTask.Delay((int)(gunSetting.ReloadTime * 1000), cancellationToken: cancellationToken);
        currentAmmo += reloadAmount;
        totalAmmo -= reloadAmount;
        isReloaded = false;
    }

    public void SetActive(bool isActive)
    {
        transform.gameObject.SetActive(isActive);
    }

    public bool CanFire()
    {
        return !(totalAmmo <= 0 || isFiring);
    }

    public bool IsReloaded()
    {
        return isReloaded;
    }
}