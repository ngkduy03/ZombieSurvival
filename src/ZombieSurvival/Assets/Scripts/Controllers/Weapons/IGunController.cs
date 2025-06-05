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
}
