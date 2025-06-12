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

    [SerializeField]
    private CharacterController characterController;

    /// <summary>
    /// ZombieController is the controller that manages the zombie's behavior, movement, and interactions.
    /// </summary>
    public ZombieController ZombieController { get; private set; }

    protected override ZombieController CreateControllerImpl()
    {
        ZombieController = new ZombieController(
            animator,
            navMeshAgent,
            patrol,
            zombieSetting,
            transform,
            characterController);

        ZombieController.Initialize();
        return ZombieController;
    }

    private void Awake()
    {
        ZombieController = CreateController();
    }

    private void Start()
    {
        ZombieController?.Start();
    }

    private void Update()
    {
        ZombieController?.Update();
    }

    private void OnTriggerEnter(Collider other)
    {
        ZombieController?.OnTriggerEnter(other);
    }
}
