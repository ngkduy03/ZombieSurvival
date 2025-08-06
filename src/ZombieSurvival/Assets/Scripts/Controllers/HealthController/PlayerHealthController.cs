using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// PlayerHealthController is a component that manages the player's health in the game.
/// </summary>
public class PlayerHealthController : ControllerBase, IHealthController
{
    private const string State = "State";
    private readonly CharacterController characterController;
    private readonly Animator animator;
    private readonly float maxHealth;
    private readonly Slider healthSlider;
    private float currentHealth;
    private bool isDead = false;
    public bool IsDead => isDead;

    public PlayerHealthController(
        CharacterController characterController,
        Animator animator,
        float maxHealth,
        Slider healthSlider)
    {
        this.characterController = characterController;
        this.animator = animator;
        this.maxHealth = maxHealth;
        this.healthSlider = healthSlider;
    }

    /// <inheritdoc />
    public void Initialize()
    {
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
    }

    /// <inheritdoc />
    public void Die()
    {
        Debug.Log("Player has died!");
        isDead = true;
        characterController.enabled = false;
        animator.SetInteger(State, (int)PlayerAnimationEnum.Death);
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
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthSlider.value = currentHealth;
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
