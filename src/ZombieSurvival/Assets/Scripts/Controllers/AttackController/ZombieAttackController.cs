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
    private readonly Animator animator;
    private readonly AudioSource audioSource;
    private ZombieSetting zombieSetting;
    private float attackDamage = 20f;
    private float attackCooldown;
    private bool canAttack = true;
    private const string State = "State";
    private CancellationTokenSource attackCTS = new();

    public ZombieAttackController(
        Animator animator,
        AudioSource audioSource,
        ZombieSetting zombieSetting)
    {
        this.animator = animator;
        this.audioSource = audioSource;
        this.zombieSetting = zombieSetting;
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
        if (!canAttack)
            return;

        if (player != null && player.TryGetComponent<PlayerComponent>(out var playerComponent))
        {
            animator.Play(animator.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, 0f);
            var playerController = playerComponent.playerController;
            playerController.OnTakenDamage(attackDamage);
            StartAttackCooldown(attackCTS.Token).Forget();
            audioSource.Stop();
            audioSource.PlayOneShot(zombieSetting.AttackClip);
        }
        else
        {
            Debug.LogWarning("Player component not found on transform");
        }
    }

    /// <summary>
    /// Starts the cooldown period after an attack.
    /// </summary>
    private async UniTask StartAttackCooldown(CancellationToken cancellationToken)
    {
        canAttack = false;
        await UniTask.Delay((int)(attackCooldown * 1000), cancellationToken: cancellationToken);
        canAttack = true;
        // Reset to idle state after cooldown
        animator.SetInteger(State, (int)ZombieAnimationEnum.Idle);
    }

    protected override void Dispose(bool isDispose)
    {
        canAttack = false;
        audioSource.Stop();
        attackCTS?.Cancel();
        attackCTS?.Dispose();
        attackCTS = null;
    }
}
