using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IGunController
{
    /// <summary>
    /// Fires the gun.
    /// </summary>
    UniTask FireBullet(CancellationToken cancellationToken);

    /// <summary>
    /// Reloads the gun's ammo.
    /// </summary> 
    UniTask ReloadAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Checks if the gun is ready to fire.
    /// </summary>
    /// <param name="isActive"></param>
    void SetActive(bool isActive);

    /// <summary>
    /// Gets the current gun setting.
    /// </summary>
    GunSetting GunSetting { get; }

    /// <summary>
    /// Checks if the gun can fire.
    /// </summary>
    bool CanFire();

    /// <summary>
    /// Checks if the gun is reloaded.
    /// </summary>
    bool IsReloaded();
}
