using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Controller of the zombie.
public class ZombieController : ControllerBase
{
    private IAttackController attackController;
    private IZombieMovementController movementController;
    private IDetectionController detectionController;
    private IBehavior zombieBehavior;
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private List<Transform> patrol;
    private ZombieSetting zombieSetting;
    private Transform zombieTransform;   
     private LayerMask playerMask;
    private int health;
    
    public ZombieController(
        Animator animator,
        NavMeshAgent navMeshAgent,
        List<Transform> patrol,
        ZombieSetting zombieSetting,
        Transform zombieTransform,
        LayerMask playerMask)
    {
        this.animator = animator;
        this.navMeshAgent = navMeshAgent;
        this.patrol = patrol;
        this.zombieSetting = zombieSetting;
        this.zombieTransform = zombieTransform;
        this.playerMask = playerMask;        this.health = 100;
    }
    
    public void Initialize()
    {
        movementController = new ZombieMovementController(animator, navMeshAgent, patrol, zombieSetting);
        attackController = new ZombieAttackController();
        detectionController = new ZombieFoVController(zombieTransform, zombieSetting, playerMask);
        zombieBehavior = new ZombieBehavior(movementController, attackController, detectionController);
    }

    public void Update()
    {
        zombieBehavior.Update();
    }
}
