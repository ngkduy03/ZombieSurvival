using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
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

    /// <summary>
    /// 
    /// </summary>
    public void Update()
    {
        movementController.Move();
    }
}
