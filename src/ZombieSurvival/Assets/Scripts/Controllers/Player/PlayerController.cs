using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controller for player.
/// </summary>
public class PlayerController : IController
{
    private readonly Animator animator;
    private readonly IAttackController attackController;
    private readonly IMovementController movementController;
    private readonly IPlayerBehavior playerBehavior;

    public PlayerController()
    {
        attackController = new ShootController(animator);
        movementController = new MoveController(animator);
        playerBehavior = new PlayerBehavior(movementController, attackController);
    }

    public void Dispose()
    {
        throw new System.NotImplementedException();
    }
}
