using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controller for player movement and rotation.
/// </summary>
public class MoveController : ControllerBase, IMovementController
{
    private readonly Transform transform;
    private readonly Animator animator;
    private readonly InputActionReference[] inputActions;
    private readonly CharacterController characterController;
    private float walkSpeed = 0;
    private float walkAnimationSpeed = 1f;
    private Vector3 moveDirection;
    private const float walkSpeedAccel = 0.01f;
    private const float walkSpeedAnimationAccel = 1.8f;
    private int velocity;

    public MoveController(
        Transform transform,
        Animator animator,
        InputActionReference[] inputActions,
        CharacterController characterController)
    {
        this.transform = transform;
        this.animator = animator;
        this.inputActions = inputActions;
        this.characterController = characterController;
        velocity = Animator.StringToHash("Velocity");

    }

    /// <summary>
    /// Moves the player in the specified direction.
    /// </summary>
    public void Move()
    {
        var direction = inputActions[(int)InputActionEnum.Move].action.ReadValue<Vector2>().normalized;
        moveDirection = new Vector3(direction.x, 0, direction.y);

        // If the player is moving, set the velocity to the walk speed.
        if (direction.magnitude > 0)
        {
            walkSpeed += walkSpeedAccel;
            walkAnimationSpeed += walkSpeedAnimationAccel * Time.deltaTime;
        }
        else
        {
            // Slow down the player when not pressing the key with double the deceleration.
            walkSpeed -= walkSpeedAccel * 10;
            walkAnimationSpeed -= walkSpeedAnimationAccel * 2 * Time.deltaTime;
        }

        // Clamp the speed and animation speed.
        walkSpeed = Mathf.Clamp(walkSpeed, 0, 5);
        walkAnimationSpeed = Mathf.Clamp(walkAnimationSpeed, 0, 1);

        // Set the animator's velocity parameter.
        animator.SetFloat(velocity, walkAnimationSpeed);

        // Move the character controller.
        characterController.SimpleMove(moveDirection * walkSpeed);
    }

    /// <summary>
    /// Rotates the player to face the direction of movement based on input.
    /// </summary>
    public void Look()
    {
        var lookDirection = inputActions[(int)InputActionEnum.Look].action.ReadValue<Vector2>();
        if (lookDirection.magnitude > 0)
        {
            // Calculate the rotation based on the look direction.
            var rotation = Quaternion.Euler(0, Mathf.Atan2(lookDirection.x, lookDirection.y) * Mathf.Rad2Deg, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10f);
        }
    }
}
