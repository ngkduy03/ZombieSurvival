using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// ZombieAttackController class implements the IZombieAttackController interface for zombie attacks.
/// </summary>
public class ZombieAttackController : ControllerBase, IZombieAttackController
{
    private float attackDamage = 20f;
    private float attackCooldown;
    private bool isAttacking = false;
    private bool canAttack = true;
    private Animator animator; // Add animator field
    private const string State = "State";

    public ZombieAttackController(Animator animator)
    {
        this.animator = animator;
    }

    /// <summary>
    /// Gets the damage amount the zombie inflicts per attack.
    /// </summary>
    public float AttackDamage => attackDamage;

    /// <summary>
    /// Initializes attack controller with default values.
    /// </summary>
    public void Initialize()
    {
        attackCooldown = animator ? animator.GetCurrentAnimatorStateInfo(0).length : 2f;

    }

    /// <summary>
    /// Attacks the specified player, damaging their health component.
    /// </summary>
    /// <param name="player">The player transform to attack</param>
    public void AttackPlayer(Transform player)
    {
        if (!canAttack) return;

        if (player != null && player.TryGetComponent<PlayerComponent>(out var playerComponent))
        {
            // Play the current animation state again
            animator.Play(animator.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, 0f);
            isAttacking = true;

            // Apply damage to player
            var playerController = playerComponent.playerController;
            playerController.OnTakenDamage(attackDamage);
            Debug.Log($"Zombie attacked player for {attackDamage} damage!");

            // Start cooldown
            StartAttackCooldown().Forget();
        }
        else
        {
            Debug.LogWarning("Player component not found on transform");
        }
    }

    /// <summary>
    /// Starts the cooldown period after an attack.
    /// </summary>
    private async UniTaskVoid StartAttackCooldown()
    {
        canAttack = false;
        await UniTask.Delay((int)(attackCooldown * 1000));
        canAttack = true;
        // Reset to idle state after cooldown
        animator.SetInteger(State, (int)ZombieAnimationEnum.Idle);
    }
}
