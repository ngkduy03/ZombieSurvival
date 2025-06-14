using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// Controller for player.
/// </summary>
public class PlayerController : ControllerBase
{
    private readonly Transform transform;
    private readonly Animator animator;
    private readonly AudioSource gunAudioSource;
    private readonly AudioSource movementAudioSource;
    private readonly InputActionReference[] inputActions;
    private readonly CharacterController characterController;
    private readonly List<IGunController> gunControllers;
    private readonly float maxHealth;
    private readonly FireButton fireButton;
    private readonly Button switchGunButton;
    private readonly Button reloadButton;
    private readonly Slider healthSlider;
    private IBehavior playerBehavior;

    public PlayerController(
        Transform transform,
        Animator animator,
        InputActionReference[] inputActions,
        CharacterController characterController,
        List<IGunController> gunControllers,
        AudioSource gunAudioSource,
        AudioSource movementAudioSource,
        float maxHealth,
        FireButton fireButton,
        Button switchGunButton,
        Button reloadButton,
        Slider healthSlider)
    {
        this.transform = transform;
        this.animator = animator;
        this.inputActions = inputActions;
        this.characterController = characterController;
        this.gunControllers = gunControllers;
        this.maxHealth = maxHealth;
        this.fireButton = fireButton;
        this.switchGunButton = switchGunButton;
        this.reloadButton = reloadButton;
        this.gunAudioSource = gunAudioSource;
        this.movementAudioSource = movementAudioSource;
        this.healthSlider = healthSlider;
    }

    /// <summary>
    /// Initializes the player controller by setting up movement and attack controllers,
    /// </summary>
    public void Initialize()
    {
        var movementController = new MoveController(transform, animator, inputActions, characterController, movementAudioSource);

        var attackController = new FireBulletController(animator, gunControllers, fireButton, switchGunButton, reloadButton, gunAudioSource);

        var healthController = new PlayerHealthController(characterController, animator, maxHealth, healthSlider);

        // Create the behavior controller with all dependencies.
        playerBehavior = new PlayerBehavior(movementController, attackController,healthController);
    }

    /// <summary>
    /// Handles when the player takes damage. 
    /// </summary>
    /// <param name="damageAmount"></param>
    public void OnTakenDamage(float damageAmount)
    {
        playerBehavior?.OnTakenDamage(damageAmount);
    }

    /// <summary>
    /// Starts the player behavior, which includes initializing movement and attack actions.
    /// </summary>
    public void OnEnable()
    {
        playerBehavior?.OnEnable();
    }
    
    /// <summary>
    /// Disables the player behavior, which includes stopping movement and attack actions.
    /// </summary>
    public void OnDisable()
    {
        playerBehavior?.OnDisable();    
    }

    /// <summary>
    /// Updates the player behavior, which includes movement and attack actions.
    /// </summary>
    public void Update()
    {
        playerBehavior?.Update();
    }
}
