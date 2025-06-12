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
    public void Start()
    {
        healthController?.Initialize();
        attackController?.Initialize();
    }

    /// <inheritdoc />
    public void Update()
    {
        movementController?.Move();
        movementController?.Look();
    }
}
