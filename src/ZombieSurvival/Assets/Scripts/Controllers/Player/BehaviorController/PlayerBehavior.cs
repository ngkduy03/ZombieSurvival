using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Standard player behavior controller that handles player other controllers.
/// </summary>
public class PlayerBehavior : ControllerBase, IPlayerBehavior
{
    private IMovementController movementController;
    private IAttackController attackController;

    public PlayerBehavior(
        IMovementController movementController,
        IAttackController attackController)
    {
        this.movementController = movementController;
        this.attackController = attackController;
    }

    /// <inheritdoc />
    public void Update()
    {
        movementController.Move();
        movementController.Look();
    }
}
