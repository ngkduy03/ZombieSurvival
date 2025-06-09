using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controller of the zombie.
public class ZombieController : ControllerBase
{
    private IAttackController attackController;
    private IMovementController movementController;
    private IDetectionController detectionController;
    private IBehavior zombieBehavior;

    public ZombieController()
    {
    }

    public void Initialize()
    {
        movementController = new ZombieMovementController();
        attackController = new ZombieAttackController();
        detectionController = new ZombieFoVController();
        zombieBehavior = new ZombieBehavior(movementController, attackController, detectionController);
    }

    public void Update()
    {
        zombieBehavior.Update();
    }
}
