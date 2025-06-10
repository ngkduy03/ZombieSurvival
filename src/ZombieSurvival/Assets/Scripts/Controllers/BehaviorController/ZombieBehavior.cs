using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/// <summary>
/// ZombieBehavior class implements the IBehavior interface for zombie behavior.
/// </summary>
public class ZombieBehavior : ControllerBase, IBehavior
{
    private IZombieMovementController movementController;
    private IAttackController attackController;
    private IDetectionController detectionController;
    private CancellationTokenSource chasingCTS;

    public ZombieBehavior(
        IZombieMovementController movementController,
        IAttackController attackController,
        IDetectionController detectionController)
    {
        this.movementController = movementController;
        this.attackController = attackController;
        this.detectionController = detectionController;
    }

    /// <inheritdoc />
    public void Update()
    {
        if (detectionController.CheckInRange())
        {
            Transform playerTransform = detectionController.GetPlayerTransform();
            if (playerTransform != null)
            {
                movementController.ChasePlayer(playerTransform, chasingCTS.Token);
            }
        }
        else
        {
            chasingCTS?.Cancel();
            chasingCTS?.Dispose();
            chasingCTS = new CancellationTokenSource();
            movementController.MoveOnPatrol();
        }
    }
}
