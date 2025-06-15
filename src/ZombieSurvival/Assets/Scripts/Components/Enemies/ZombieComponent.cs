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
    private AudioSource audioSource;

    [SerializeField]
    private NavMeshAgent navMeshAgent;

    [SerializeField]
    private CharacterController characterController;

    [SerializeField]
    private DissolverObject dissolverObject;

    [SerializeField]
    private ParticleSystem bloodParticleSystem;
    private BlockObjectController blockObjectController;

    /// <summary>
    /// ZombieController is the controller that manages the zombie's behavior, movement, and interactions.
    /// </summary>
    public ZombieController ZombieController { get; private set; }

    protected override ZombieController CreateControllerImpl()
    {
        ZombieController = new ZombieController(
            animator,
            audioSource,
            navMeshAgent,
            patrol,
            zombieSetting,
            transform,
            characterController,
            dissolverObject,
            bloodParticleSystem,
            blockObjectController);

        ZombieController.Initialize();
        return ZombieController;
    }

    private void Awake()
    {
    }

    private void OnEnable()
    {
        ZombieController?.OnEnable();
    }

    private void OnDisable()
    {
        ZombieController?.OnDisable();
    }

    public void Initialize(BlockObjectController blockObjectController)
    {
        this.blockObjectController = blockObjectController;
    }

    private void Update()
    {
        ZombieController?.Update();
    }

    private void OnTriggerEnter(Collider other)
    {
        ZombieController?.OnTriggerEnter(other);
        Debug.Log($"ZombieComponent OnTriggerEnter: {other.name}", gameObject);
    }
}
