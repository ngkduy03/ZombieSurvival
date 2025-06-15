using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// ZombieBehavior class implements the IBehavior interface for zombie behavior.
/// </summary>
public class ZombieBehavior : ControllerBase, IBehavior
{
    private IZombieMovementController movementController;
    private IZombieAttackController attackController;
    private IDetectionController detectionController;
    private IHealthController healthController;
    private CancellationTokenSource movementCTS = new();
    private CancellationTokenSource dieCTS = new();
    private bool isDisposed = false;
    private bool isMoveCTSCancel = false;

    public ZombieBehavior(
        IZombieMovementController movementController,
        IZombieAttackController attackController,
        IDetectionController detectionController,
        ZombieHealthController healthController)
    {
        this.movementController = movementController;
        this.attackController = attackController;
        this.detectionController = detectionController;
        this.healthController = healthController;
    }

    /// <inheritdoc />
    public void OnTakenDamage(float damageAmount)
    {
        healthController.TakeDamage(damageAmount);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out BulletComponent bullet))
            return;

        if (bullet != null && !bullet.IsDeactivated)
        {
            healthController.TakeDamage(bullet.Damage);
            bullet.DestroyBullet();
        }
    }

    /// <inheritdoc />
    public void OnEnable()
    {
        movementController.Initialize();
        attackController.Initialize();
        healthController.Initialize();
    }

    /// <inheritdoc />
    public void OnDisable()
    {
        Dispose();
    }

    /// <inheritdoc />
    public void Update()
    {
        if (healthController.IsDead)
        {
            if (!isDisposed)
            {
                isDisposed = true;
                healthController.DestroyObjectAsync(dieCTS.Token, Dispose).Forget();
            }

            return;
        }

        bool isPlayerInRange = detectionController.CheckInRange();

        // Handle state transition between chasing and patrolling
        if (isPlayerInRange && isMoveCTSCancel)
        {
            // Reset token for chase behavior
            movementCTS?.Cancel();
            movementCTS?.Dispose();
            movementCTS = new();
            isMoveCTSCancel = false;
        }
        else if (!isPlayerInRange && !isMoveCTSCancel)
        {
            // Reset token for patrol behavior
            movementCTS?.Cancel();
            movementCTS?.Dispose();
            movementCTS = new();
            isMoveCTSCancel = true;
        }

        if (isPlayerInRange)
        {
            var playerComponent = detectionController.GetPlayerComponent();
            if (playerComponent != null)
            {
                movementController.ChasePlayer(playerComponent.transform, movementCTS.Token);
            }
        }
        else
        {
            movementController.MoveOnPatrol(movementCTS.Token);
        }
    }

    protected override void Dispose(bool disposing)
    {
        movementCTS?.Cancel();
        movementCTS?.Dispose();
        movementCTS = null;
        
        dieCTS?.Cancel();
        dieCTS?.Dispose();
        dieCTS = null;

        healthController?.Dispose();
        movementController?.Dispose();
        attackController?.Dispose();
        detectionController?.Dispose();
    }

    /// <inheritdoc />
    public bool GetAttackStatus()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public bool GetDieStatus()
    {
        throw new NotImplementedException();
    }
}
