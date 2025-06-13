using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Movement controller for zombies.
/// </summary>
public class ZombieMovementController : ControllerBase, IZombieMovementController
{
    private readonly NavMeshAgent agent;
    private readonly Animator animator;
    private readonly List<Transform> patrol;
    private readonly ZombieSetting zombieSetting;
    private readonly IZombieAttackController attackController;
    private Transform currDestination;
    private Transform prevDestination;
    private bool isChasing = false;
    private bool isReached = false;
    private bool isResetUpdatePath = false;
    private const float UpdatePathCoolDown = 0.15f;
    private const string State = "State";

    public ZombieMovementController(
        Animator animator,
        NavMeshAgent agent,
        List<Transform> patrol,
        ZombieSetting zombieSetting,
        IZombieAttackController attackController)
    {
        this.animator = animator;
        this.agent = agent;
        this.patrol = patrol;
        this.zombieSetting = zombieSetting;
        this.attackController = attackController;
    }

    /// <inheritdoc />
    public void Initialize()
    {
        agent.speed = zombieSetting.WalkSpeed;
        agent.acceleration = zombieSetting.WalkAcceleration;
        agent.stoppingDistance = zombieSetting.StopDistance;
        var patrolIndex = Random.Range(0, patrol.Count);
        currDestination = patrol[patrolIndex];
        agent.SetDestination(currDestination.position);
        animator.SetInteger(State, (int)ZombieAnimationEnum.Move);
    }

    /// <inheritdoc />
    public async UniTask MoveOnPatrol(CancellationToken cancellationToken)
    {
        if (isChasing)
        {
            isChasing = false;
            agent.ResetPath();
            if (currDestination != null)
            {
                agent.SetDestination(currDestination.position);
                return;
            }
        }

        if (agent != null && currDestination != null && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!isReached)
            {
                // Set new destination after reaching the current one.
                patrol.Remove(currDestination);
                prevDestination = currDestination;
                isReached = true;
                var patrolIndex = Random.Range(0, patrol.Count);
                currDestination = patrol[patrolIndex];

                animator.SetInteger(State, (int)ZombieAnimationEnum.Idle);
                await UniTask.WaitForSeconds(Random.Range(4, 8), cancellationToken: cancellationToken);
                agent.SetDestination(currDestination.position);
                patrol.Add(prevDestination);
                isReached = false;
                animator.SetInteger(State, (int)ZombieAnimationEnum.Move);
            }
        }
    }

    /// <inheritdoc />
    public async UniTask ChasePlayer(Transform target, CancellationToken cancellationToken)
    {
        // Check if reach to the player.
        var distanceToTarget = Vector3.Distance(agent.transform.position, target.position);
        if (distanceToTarget <= agent.stoppingDistance)
        {
            animator.SetInteger(State, (int)ZombieAnimationEnum.Attack);
            attackController.AttackPlayer(target);
        }
        else
        {
            // If not reached, back on the patrol path.
            if (!isResetUpdatePath)
            {
                isResetUpdatePath = true;
                animator.SetInteger(State, (int)ZombieAnimationEnum.Move);
                agent.SetDestination(target.position);

                await UniTask.WaitForSeconds(UpdatePathCoolDown, cancellationToken: cancellationToken);
                isResetUpdatePath = false;
            }
        }
    }

    /// <inheritdoc />
    public float GetStoppingDistance()
    {
        return agent.stoppingDistance;
    }

    protected override void Dispose(bool isDispose)
    {
        isChasing = false;
        isReached = false;
        isResetUpdatePath = false;
        currDestination = null;
        prevDestination = null;

        if (agent != null)
        {
            agent.ResetPath();
            agent.enabled = false; // Disable the NavMeshAgent to stop movement.
        }
    }
}
