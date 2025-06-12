using System;
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
    private IZombieAttackController attackController;
    private IDetectionController detectionController;
    private CancellationTokenSource movementCTS = new();

    public ZombieBehavior(
        IZombieMovementController movementController,
        IZombieAttackController attackController,
        IDetectionController detectionController)
    {
        this.movementController = movementController;
        this.attackController = attackController;
        this.detectionController = detectionController;
    }

    /// <inheritdoc />
    public void OnTakenDamage(float damageAmount)
    {
        //TODO: Implement damage handling logic
    }

    /// <inheritdoc />
    public void Start()
    {
        movementController.Initialize();
        attackController.Initialize();
    }

    /// <inheritdoc />
    public void Update()
    {
        if (detectionController.CheckInRange())
        {
            Transform playerTransform = detectionController.GetTargetTransform();
            if (playerTransform != null)
            {
                movementController.ChasePlayer(playerTransform, movementCTS.Token);
            }
        }
        else
        {
            movementController.MoveOnPatrol(movementCTS.Token);
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            movementCTS?.Cancel();
            movementCTS?.Dispose();
            GC.SuppressFinalize(this);
        }
        base.Dispose(disposing);
    }
}
