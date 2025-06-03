using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controller for player.
/// </summary>
public class PlayerController : IController
{
    private readonly Transform transform;
    private readonly Animator animator;
    private readonly InputActionReference moveInput;
    private readonly CharacterController characterController;
    private readonly IAttackController attackController;
    private readonly IMovementController movementController;
    private readonly IPlayerBehavior playerBehavior;

    public PlayerController(
        Transform transform,
        Animator animator,
        InputActionReference moveInput,
        CharacterController characterController)
    {
        this.transform = transform;
        this.animator = animator;
        this.moveInput = moveInput;
        this.characterController = characterController;

        movementController = new MoveController(transform,animator, moveInput, characterController);
        attackController = new ShootController(animator);
        playerBehavior = new PlayerBehavior(movementController, attackController);
    }

    public void Update()
    {
        playerBehavior.Update();
    }

    public void Dispose()
    {
        throw new System.NotImplementedException();
    }
}
