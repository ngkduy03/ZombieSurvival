using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Standard player behavior controller that handles player other controllers.
/// </summary>
public class PlayerBehavior : ControllerBase, IBehavior
{
    private IMovementController movementController;
    private IAttackController attackController;
    private IHealthController healthController;
    private bool isDisposed = false;

    public PlayerBehavior(
        IMovementController movementController,
        IAttackController attackController,
        IHealthController healthController)
    {
        this.movementController = movementController;
        this.attackController = attackController;
        this.healthController = healthController;
    }

    /// <inheritdoc />
    public void OnTakenDamage(float damageAmount)
    {
        healthController?.TakeDamage(damageAmount);
    }

    /// <inheritdoc />
    public void OnTriggerEnter(Collider other)
    {
        
    }

    /// <inheritdoc />
    public void OnEnable()
    {
        healthController?.Initialize();
        attackController?.Initialize();
    }

    public void OnDisable()
    {
        Dispose();
    }


    /// <inheritdoc />
    public void Update()
    {
        if (isDisposed || healthController.IsDead)
        {
            Dispose();
            isDisposed = true;
            return;
        }

        movementController?.Move();
        movementController?.Look();
    }

    protected override void Dispose(bool disposing)
    {
        movementController?.Dispose();
        attackController?.Dispose();
        healthController?.Dispose();
    }
}
