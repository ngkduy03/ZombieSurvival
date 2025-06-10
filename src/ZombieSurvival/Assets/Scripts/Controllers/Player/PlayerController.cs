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
    private readonly Button switchGunButton;
    private readonly Button reloadButton;
    private IAttackController attackController;
    private IMovementController movementController;
    private IBehavior playerBehavior;

    public PlayerController(
        Transform transform,
        Animator animator,
        InputActionReference[] inputActions,
        CharacterController characterController,
        List<IGunController> gunControllers,
        FireButton fireButton,
        Button switchGunButton,
        Button reloadButton)
    {
        this.transform = transform;
        this.animator = animator;
        this.inputActions = inputActions;
        this.characterController = characterController;
        this.gunControllers = gunControllers;
        this.fireButton = fireButton;
        this.switchGunButton = switchGunButton;
        this.reloadButton = reloadButton;
    }

    /// <summary>
    /// Initializes the player controller by setting up movement and attack controllers,
    /// </summary>
    public void Initialize()
    {
        movementController = new MoveController(transform, animator, inputActions, characterController);
        attackController = new FireBulletController(animator, gunControllers, fireButton, switchGunButton, reloadButton);
        playerBehavior = new PlayerBehavior(movementController, attackController);

    }

    /// <summary>
    /// Updates the player behavior, which includes movement and attack actions.
    /// </summary>
    public void Update()
    {
        playerBehavior?.Update();
    }

    public void Dispose()
    {
        throw new System.NotImplementedException();
    }
}
