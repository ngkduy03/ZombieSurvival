using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ShotgunController : ControllerBase, IGunController
{
    private GunSetting gunSetting;
    private int currentAmmo;
    private int totalAmmo;
    private Transform pelletSpawnPoint;
    private float spreadAngle = 15f;
    private float range = 2f;
    private LayerMask enemyMask;
    private Collider[] enemyRangeChecks = new Collider[100];
    private bool isFiring = false;

    public ShotgunController(
        GunSetting gunSetting,
        Transform pelletSpawnPoint,
        LayerMask enemyMask)
    {
        this.gunSetting = gunSetting;
        this.pelletSpawnPoint = pelletSpawnPoint;
        this.enemyMask = enemyMask;
        currentAmmo = this.gunSetting.MagazineSize;
        totalAmmo = this.gunSetting.TotalAmmo;
    }

    public void FireBullet()
    {
        if (currentAmmo <= 0)
            return;
        currentAmmo--;
    }

    private void CheckInRange()
    {
        var enemyAmount = Physics.OverlapSphereNonAlloc(pelletSpawnPoint.position, range, enemyRangeChecks, enemyMask, QueryTriggerInteraction.Collide);

        if (enemyAmount > 0)
        {
            for (int i = 0; i < enemyAmount; i++)
            {
                var enemy = enemyRangeChecks[i];

                if (enemy == null)
                    continue;

                var enemyPos = enemyRangeChecks[0].transform.position;

                // Check if the enemy is within the spread angle of the shotgun
                // This assumes pelletSpawnPoint.forward is the direction the shotgun is facing
                if (Vector3.Angle(pelletSpawnPoint.position, enemyPos) < spreadAngle / 2f)
                {
                    //TODO: Implement damage logic here
                }
            }
        }

        Array.Clear(enemyRangeChecks, 0, enemyRangeChecks.Length);
    }

    public async UniTask FireBullet(CancellationToken cancellationToken)
    {
        if (isFiring)
        {
            return;
        }

        isFiring = true;
        Debug.Log("Firing pellet");
        if (currentAmmo <= 0)
        {
            await ReloadAsync(cancellationToken);
        }

        currentAmmo--;
        CheckInRange();
        await UniTask.Delay((int)(gunSetting.FireRate * 1000), cancellationToken: cancellationToken);
        isFiring = false;

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