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
    private Transform currDestination;
    private Transform prevDestination;
    private bool isChasing = false;
    private bool isResetUpdatePath = false;
    private const float UpdatePathCoolDown = 0.3f;

    public ZombieMovementController(
        Animator animator,
        NavMeshAgent agent,
        List<Transform> patrol,
        ZombieSetting zombieSetting)
    {
        this.animator = animator;
        this.agent = agent;
        this.patrol = patrol;
        this.zombieSetting = zombieSetting;
        Initialize();
    }

    private void Initialize()
    {
        agent.speed = zombieSetting.WalkSpeed;
        agent.acceleration = zombieSetting.WalkAcceleration;
        agent.stoppingDistance = zombieSetting.StopDistance;
        var patrolIndex = Random.Range(0, patrol.Count);
        currDestination = patrol.Count > 0 ? patrol[patrolIndex] : null;
        agent.SetDestination(currDestination.position);
    }

    /// <inheritdoc />
    public void MoveOnPatrol()
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
            prevDestination = currDestination;
            patrol.Remove(currDestination);

            if (patrol.Count > 0)
            {
                var patrolIndex = Random.Range(0, patrol.Count);
                currDestination = patrol[patrolIndex];
                agent.SetDestination(currDestination.position);
                patrol.Add(prevDestination);
            }
        }
    }

    /// <inheritdoc />
    public async UniTask ChasePlayer(Transform target, CancellationToken cancellationToken)
    {
        if (isResetUpdatePath || agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance <= agent.stoppingDistance)
            return;

        isResetUpdatePath = true;
        agent.SetDestination(target.position);
        await UniTask.WaitForSeconds(UpdatePathCoolDown, cancellationToken: cancellationToken);
        isResetUpdatePath = false;
    }
}
