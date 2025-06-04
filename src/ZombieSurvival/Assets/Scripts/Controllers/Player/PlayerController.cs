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
    private readonly InputActionReference lookInput;
    private readonly CharacterController characterController;
    private IAttackController attackController;
    private IMovementController movementController;
    private IPlayerBehavior playerBehavior;

    public PlayerController(
        Transform transform,
        Animator animator,
        InputActionReference moveInput,
        InputActionReference lookInput,
        CharacterController characterController)
    {
        this.transform = transform;
        this.animator = animator;
        this.moveInput = moveInput;
        this.lookInput = lookInput;
        this.characterController = characterController;

        Initialize();
    }

    private void Initialize()
    {
        movementController = new MoveController(transform, animator, moveInput, lookInput, characterController);
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
