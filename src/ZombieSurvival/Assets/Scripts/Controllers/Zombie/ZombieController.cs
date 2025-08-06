using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Controller of the zombie.
public class ZombieController : ControllerBase
{
    private Animator animator;
    private AudioSource audioSource;
    private NavMeshAgent navMeshAgent;
    private CharacterController characterController;
    private List<Transform> patrol;
    private ZombieSetting zombieSetting;
    private Transform zombieTransform;
    private DissolverObject dissolverObject;
    private IBehavior zombieBehavior;
    private ParticleSystem bloodParticleSystem;
    private BlockObjectController blockObjectController;

    public ZombieController(
        Animator animator,
        AudioSource audioSource,
        NavMeshAgent navMeshAgent,
        List<Transform> patrol,
        ZombieSetting zombieSetting,
        Transform zombieTransform,
        CharacterController characterController,
        DissolverObject dissolverObject,
        ParticleSystem bloodParticleSystem,
        BlockObjectController blockObjectController)
    {
        this.animator = animator;
        this.audioSource = audioSource;
        this.navMeshAgent = navMeshAgent;
        this.patrol = patrol;
        this.zombieSetting = zombieSetting;
        this.zombieTransform = zombieTransform;
        this.characterController = characterController;
        this.dissolverObject = dissolverObject;
        this.bloodParticleSystem = bloodParticleSystem;
        this.blockObjectController = blockObjectController;
    }
    public void Initialize()
    {
        var attackController = new ZombieAttackController(animator, audioSource, zombieSetting);

        var movementController = new ZombieMovementController(animator, navMeshAgent, patrol, zombieSetting, attackController);

        var detectionController = new ZombieFoVController(zombieTransform, zombieSetting);

        var healthController = new ZombieHealthController(characterController, animator, audioSource, zombieSetting, dissolverObject, bloodParticleSystem, blockObjectController);

        // Create the behavior controller with all dependencies.
        zombieBehavior = new ZombieBehavior(movementController, attackController, detectionController, healthController);
    }

    /// <summary>
    /// Initializes the zombie controller by setting up the movement, attack, detection, and health controllers.
    /// </summary>
    public void OnEnable()
    {
        zombieBehavior?.OnEnable();
    }
    
    /// <summary>
    /// Disables the zombie behavior, cleaning up resources and stopping any ongoing actions.
    /// </summary>
    public void OnDisable()
    {
        zombieBehavior?.OnDisable();
    }

    /// <summary>
    /// Updates the zombie behavior each frame.
    /// </summary>
    public void Update()
    {
        zombieBehavior?.Update();
    }

    /// <summary>
    /// Handles the trigger enter event for the zombie, allowing it to interact with other objects in the game world.
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other)
    {
        zombieBehavior?.OnTriggerEnter(other);
    }

    /// <summary>
    /// Handles when the zombie takes damage.
    /// </summary>
    /// <param name="damageAmount"></param>
    public void OnTakenDamage(float damageAmount)
    {
        zombieBehavior?.OnTakenDamage(damageAmount);
    }

}
