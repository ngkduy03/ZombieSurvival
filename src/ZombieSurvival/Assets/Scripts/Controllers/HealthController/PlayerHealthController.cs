using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// PlayerHealthController is a component that manages the player's health in the game.
/// </summary>
public class PlayerHealthController : ControllerBase, IHealthController
{
    private readonly CharacterController characterController;
    private readonly Animator animator;
    private readonly float maxHealth;
    private float currentHealth;
    private bool isDead = false;
    public bool IsDead => isDead;

    public PlayerHealthController(
        CharacterController characterController,
        Animator animator,
        float maxHealth)
    {
        this.characterController = characterController;
        this.animator = animator;
        this.maxHealth = maxHealth;
    }

    /// <inheritdoc />
    public void Initialize()
    {
        currentHealth = maxHealth;
    }

    /// <inheritdoc />
    public void Die()
    {
        Debug.Log("Player has died!");
        characterController.enabled = false;
    }


    /// <inheritdoc />
    public float GetHealth()
    {
        return currentHealth;
    }

    /// <inheritdoc />
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"Player took {damage} damage. Health: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    /// <inheritdoc />
    public UniTask DestroyObjectAsync(CancellationToken cancellationToken, Action onDestroyed = null)
    {
        throw new NotImplementedException();
    }

    protected override void Dispose(bool disposing)
    {

    }
}
