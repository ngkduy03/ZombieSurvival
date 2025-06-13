using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// ZombieHealthController is responsible for managing the health of a zombie character.
/// </summary>
public class ZombieHealthController : ControllerBase, IHealthController
{
    private CharacterController characterController;
    private readonly Animator animator;
    private ZombieSetting zombieSettings;
    private float currentHealth;
    private bool isDead;
    public bool IsDead => isDead;
    private const string State = "State";
    private const int ExpiredTime = 2000;

    public ZombieHealthController(
        CharacterController characterController,
        Animator animator,
        ZombieSetting zombieSettings
    )
    {
        this.characterController = characterController;
        this.animator = animator;
        this.zombieSettings = zombieSettings;
    }

    /// <inheritdoc />
    public void Initialize()
    {
        currentHealth = zombieSettings.MaxHealth;
        isDead = false;
    }

    /// <inheritdoc />
    public void Die()
    {
        if (isDead)
            return;

        isDead = true;
        Debug.Log("Zombie has died!");
        characterController.enabled = false;
        animator.SetInteger(State, (int)ZombieAnimationEnum.Dead);
    }

    /// <inheritdoc />
    public float GetHealth()
    {
        return currentHealth;
    }

    /// <inheritdoc />
    public void TakeDamage(float damage)
    {
        if (isDead)
            return;

        currentHealth -= damage;
        Debug.Log($"Zombie took {damage} damage. Health: {currentHealth}/{zombieSettings.MaxHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <inheritdoc />
    public async UniTask DestroyObjectAsync(CancellationToken cancellationToken, Action onDestroyed = null)
    {
        await UniTask.Delay(ExpiredTime, cancellationToken: cancellationToken);
        GameObject.Destroy(characterController.gameObject);
        onDestroyed?.Invoke();
    }

    protected override void Dispose(bool isDispose)
    {
        isDead = true;
        characterController.enabled = false;
    }
}
