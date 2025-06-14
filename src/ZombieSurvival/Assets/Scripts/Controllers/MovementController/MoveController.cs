using System;
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
    private readonly AudioSource movementAudioSource;
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
        CharacterController characterController,
        AudioSource movementAudioSource)
    {
        this.transform = transform;
        this.animator = animator;
        this.inputActions = inputActions;
        this.characterController = characterController;
        this.movementAudioSource = movementAudioSource;
    }

    ///<inheritdoc />
    public void Initialize()
    {
        velocity = Animator.StringToHash("Velocity");
        movementAudioSource.Stop();
        movementAudioSource.loop = true;
    }

    ///<inheritdoc />
    public void Move()
    {
        var direction = inputActions[(int)InputActionEnum.Move].action.ReadValue<Vector2>().normalized;
        moveDirection = new Vector3(direction.x, 0, direction.y);

        if (direction.magnitude > 0)
        {
            walkSpeed += walkSpeedAccel;
            walkAnimationSpeed += walkSpeedAnimationAccel * Time.deltaTime;
        }
        else
        {
            walkSpeed -= walkSpeedAccel * 10;
            walkAnimationSpeed -= walkSpeedAnimationAccel * 2 * Time.deltaTime;
        }

        walkSpeed = Mathf.Clamp(walkSpeed, 0, 5);
        walkAnimationSpeed = Mathf.Clamp(walkAnimationSpeed, 0, 1);
        animator.SetFloat(velocity, walkAnimationSpeed);
        characterController.SimpleMove(moveDirection * walkSpeed);

        if (walkSpeed > 0 && !movementAudioSource.isPlaying)
        {
            movementAudioSource.Play();
        }
        else if (walkSpeed <= 0 && movementAudioSource.isPlaying)
        {
            movementAudioSource.Stop();
        }
    }

    ///<inheritdoc />
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

    protected override void Dispose(bool disposing)
    {
        characterController.enabled = false;
        Array.Clear(inputActions, 0, inputActions.Length);
    }
}
