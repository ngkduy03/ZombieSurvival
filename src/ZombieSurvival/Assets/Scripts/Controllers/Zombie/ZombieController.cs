using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Controller of the zombie.
public class ZombieController : ControllerBase
{
    private IZombieAttackController attackController;
    private IZombieMovementController movementController;
    private IDetectionController detectionController;
    private IBehavior zombieBehavior;
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private CharacterController characterController;
    private List<Transform> patrol;
    private ZombieSetting zombieSetting;
    private Transform zombieTransform;
    private int health;
    
    public ZombieController(
        Animator animator,
        NavMeshAgent navMeshAgent,
        List<Transform> patrol,
        ZombieSetting zombieSetting,
        Transform zombieTransform,
        CharacterController characterController)
    {
        this.animator = animator;
        this.navMeshAgent = navMeshAgent;
        this.patrol = patrol;
        this.zombieSetting = zombieSetting;
        this.zombieTransform = zombieTransform;
        this.characterController = characterController;
    }
      public void Initialize()
    {
        // First create the attack controller
        attackController = new ZombieAttackController(animator);
        
        // Pass the attack controller to the movement controller
        movementController = new ZombieMovementController(animator, navMeshAgent, patrol, zombieSetting, attackController);
        
        // Create the detection controller
        detectionController = new ZombieFoVController(zombieTransform, zombieSetting);
        
        // Create the behavior controller with all dependencies
        zombieBehavior = new ZombieBehavior(movementController, attackController, detectionController);
    }

    /// <inheritdoc/>
    public void Start()
    {
        zombieBehavior?.Start();
    }

    /// <inheritdoc/>
    public void Update()
    {
        zombieBehavior?.Update();
    }
}
