using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Interface for zombie movement controllers.
/// </summary>
public interface IZombieMovementController : IController
{
    /// <summary>
    /// Defines the movement method for zombies.
    /// </summary>
    void MoveOnPatrol();

    /// <summary>
    /// Defines the look method for zombies.
    /// </summary>
    /// <param name="playerTransform"></param>
    UniTask ChasePlayer(Transform playerTransform, CancellationToken cancellationToken);
}
