using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PlayerHealthController is a component that manages the player's health in the game.
/// </summary>
public class PlayerHealthController : ControllerBase, IHealthController
{
    private bool isDead;
    private CharacterController characterController;
    private float currentHealth;
    public float maxHealth { get; private set; }

    public PlayerHealthController(
        CharacterController characterController,
        float maxHealth)
    {
        this.characterController = characterController;
        this.maxHealth = maxHealth;
    }

    /// <inheritdoc />
    public void Initialize()
    {
        currentHealth = maxHealth;
        isDead = false;
    }

    /// <inheritdoc />
    public void Die()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log("Player has died!");

        // Play death animation or handle game over state
        // Disable player control
        characterController.enabled = false;

        // TODO: Add game over UI or restart logic
    }


    /// <inheritdoc />
    public float GetHealth()
    {
        return currentHealth;
    }

    /// <inheritdoc />
    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log($"Player took {damage} damage. Health: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }
}
