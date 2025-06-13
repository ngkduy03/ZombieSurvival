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
    /// Initializes the zombie movement controller with necessary settings and parameters.
    /// </summary>
    void Initialize();

    /// <summary>
    /// Defines the movement method for zombies.
    /// </summary>
    UniTask MoveOnPatrol(CancellationToken cancellationToken);

    /// <summary>
    /// Defines the chase method for zombies.
    /// </summary>
    /// <param name="playerTransform"></param>
    UniTask ChasePlayer(Transform playerTransform, CancellationToken cancellationToken);
    
    /// <summary>
    /// Gets the stopping distance for the zombie's movement.
    /// </summary>
    /// <returns>The distance at which the zombie stops approaching its target</returns>
    float GetStoppingDistance();
}
