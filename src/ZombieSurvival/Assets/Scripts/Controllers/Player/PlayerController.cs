using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// Controller for player.
/// </summary>
public class PlayerController : IController
{
    private readonly Transform transform;
    private readonly Animator animator;
    private readonly InputActionReference[] inputActions;
    private readonly CharacterController characterController;
    private readonly List<IGunController> gunControllers;
    private readonly FireButton fireButton;
    private IAttackController attackController;
    private IMovementController movementController;
    private IPlayerBehavior playerBehavior;

    public PlayerController(
        Transform transform,
        Animator animator,
        InputActionReference[] inputActions,
        CharacterController characterController,
        List<IGunController> gunControllers,
        FireButton fireButton)
    {
        this.transform = transform;
        this.animator = animator;
        this.inputActions = inputActions;
        this.characterController = characterController;
        this.gunControllers = gunControllers;
        this.fireButton = fireButton;

        Initialize();
    }

    private void Initialize()
    {
        movementController = new MoveController(transform, animator, inputActions, characterController);
        attackController = new FireBulletController(animator, gunControllers, fireButton);
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
