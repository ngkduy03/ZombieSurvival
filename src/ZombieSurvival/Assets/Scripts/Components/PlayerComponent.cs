using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    private InputActionReference moveInput;

    [SerializeField]
    private InputActionReference lookInput;

    private PlayerController playerController;
    protected override PlayerController CreateControllerImpl()
    {
        playerController = new PlayerController(transform, animator, moveInput, lookInput, characterController);
        return playerController;
    }

    void Awake()
    {
        playerController = CreateController();
    }

    private void Update()
    {
        playerController?.Update();
    }
}