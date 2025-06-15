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
    private readonly Animator animator;
    private readonly AudioSource audioSource;
    private readonly CharacterController characterController;
    private readonly ZombieSetting zombieSettings;
    private readonly ParticleSystem bloodParticleSystem;
    private readonly BlockObjectController blockObjectController;
    private DissolverObject dissolverObject;
    private float currentHealth;
    private bool isDead;
    public bool IsDead => isDead;
    private const string State = "State";
    private const int ExpiredTime = 2000;

    public ZombieHealthController(
        CharacterController characterController,
        Animator animator,
        AudioSource audioSource,
        ZombieSetting zombieSettings,
        DissolverObject dissolverObject,
        ParticleSystem bloodParticleSystem,
        BlockObjectController blockObjectController)
    {
        this.characterController = characterController;
        this.animator = animator;
        this.audioSource = audioSource;
        this.zombieSettings = zombieSettings;
        this.dissolverObject = dissolverObject;
        this.bloodParticleSystem = bloodParticleSystem;
        this.blockObjectController = blockObjectController;
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
        blockObjectController?.CheckDestroy();
        characterController.enabled = false;
        animator.SetInteger(State, (int)ZombieAnimationEnum.Dead);
        audioSource.Stop();
        audioSource.PlayOneShot(zombieSettings.DieClip);
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
        currentHealth = Mathf.Clamp(currentHealth, 0, zombieSettings.MaxHealth);
        bloodParticleSystem.Play();
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
        await dissolverObject.DissolveAsync(cancellationToken);
        GameObject.Destroy(characterController.gameObject);
        onDestroyed?.Invoke();
    }

    protected override void Dispose(bool isDispose)
    {
        isDead = true;
        characterController.enabled = false;
        audioSource.Stop();
        bloodParticleSystem.Stop();
    }
}
