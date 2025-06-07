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
    private RiffleComponent riffleComponent;

    [SerializeField]
    private ShotgunComponent shotgunComponent;

    private List<IGunController> gunControllers = new List<IGunController>();
    private PlayerController playerController;

    protected override PlayerController CreateControllerImpl()
    {
        var riffleController = riffleComponent.CreateController();
        var shotgunController = shotgunComponent.CreateController();
        gunControllers.Add(riffleController);
        gunControllers.Add(shotgunController);

        playerController = new PlayerController(transform, animator, inputActions, characterController, gunControllers, fireButton);
        return playerController;
    }

    private void Awake()
    {
        playerController = CreateController();
    }

    private void Update()
    {
        playerController?.Update();
    }
}