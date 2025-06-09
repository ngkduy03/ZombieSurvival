using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ZombieBehavior class implements the IBehavior interface for zombie behavior.
/// </summary>
public class ZombieBehavior : ControllerBase, IBehavior
{
    private IMovementController movementController;
    private IAttackController attackController;
    private IDetectionController detectionController;

    public ZombieBehavior(
        IMovementController movementController,
        IAttackController attackController,
        IDetectionController detectionController)
    {
        this.movementController = movementController;
        this.attackController = attackController;
        this.detectionController = detectionController;
    }

    public void Update()
    {
        movementController.Move();
        movementController.Look();
    }
}
