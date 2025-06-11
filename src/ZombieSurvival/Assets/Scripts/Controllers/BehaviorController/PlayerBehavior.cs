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

    public PlayerBehavior(
        IMovementController movementController,
        IAttackController attackController)
    {
        this.movementController = movementController;
        this.attackController = attackController;
    }

    /// <inheritdoc />
    public void Start()
    {
        attackController?.Initialize();
    }

    /// <inheritdoc />
    public void Update()
    {
        movementController?.Move();
        movementController?.Look();
    }
}
