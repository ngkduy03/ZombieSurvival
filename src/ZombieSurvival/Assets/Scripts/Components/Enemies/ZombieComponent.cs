using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
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
    [Expandable]
    private ZombieSetting zombieSetting;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private NavMeshAgent navMeshAgent;

    private ZombieController zombieController;

    protected override ZombieController CreateControllerImpl()
    {
        zombieController = new ZombieController(
            animator,
            navMeshAgent,
            patrol,
            zombieSetting,
            transform);

        zombieController.Initialize();
        return zombieController;
    }

    private void Awake()
    {
        zombieController = CreateController();
    }

    private void Start()
    {
        zombieController?.Start();
    }

    private void Update()
    {
        zombieController?.Update();
    }
}
