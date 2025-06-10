using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// ZombieComponent is a component that manages the zombie controller in the game.
/// </summary>
public class ZombieComponent : SceneComponent<ZombieController>
{
    [SerializeField]
    private List<Transform> patrol;

    [SerializeField]
    private ZombieSetting zombieSetting;

    [SerializeField]
    private LayerMask playerMask;
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private Transform zombieTransform;
    private ZombieController zombieController;
    
    protected override ZombieController CreateControllerImpl()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        zombieTransform = transform;
        zombieController = new ZombieController(
            animator,
            navMeshAgent,
            patrol,
            zombieSetting,
            zombieTransform,
            playerMask
        );
        return zombieController;
    }
}
