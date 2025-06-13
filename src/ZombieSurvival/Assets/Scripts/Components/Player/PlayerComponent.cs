using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// PlayerComponent is a component that manages the player controller in the game.
/// </summary>
public class PlayerComponent : SceneComponent<PlayerController>
{
    [SerializeField]
    private CharacterController characterController;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private InputActionReference[] inputActions;

    [SerializeField]
    private FireButton fireButton;

    [SerializeField]
    private Button switchGunButton;

    [SerializeField]
    private Button reloadButton;

    [SerializeField]
    private RiffleComponent riffleComponent;

    [SerializeField]
    private ShotgunComponent shotgunComponent;

    [SerializeField]
    private float maxHealth = 100f;

    [SerializeField]
    private AudioSource audioSource;

    private float currentHealth;
    private bool isDead = false;

    private List<IGunController> gunControllers = new List<IGunController>();
    public PlayerController playerController { get; private set; }

    protected override PlayerController CreateControllerImpl()
    {
        var riffleController = riffleComponent.CreateController();
        var shotgunController = shotgunComponent.CreateController();
        gunControllers.Add(riffleController);
        gunControllers.Add(shotgunController);

        playerController = new PlayerController(transform, animator, inputActions, characterController, gunControllers, audioSource, maxHealth, fireButton, switchGunButton, reloadButton);
        return playerController;
    }

    private void Awake()
    {
        playerController = CreateController();
        playerController.Initialize();
        currentHealth = maxHealth;
    }

    private void OnEnable()
    {
        playerController?.OnEnable();
    }

    private void OnDisable()
    {
        playerController?.OnDisable();
    }

    private void Update()
    {
        playerController?.Update();
    }
}