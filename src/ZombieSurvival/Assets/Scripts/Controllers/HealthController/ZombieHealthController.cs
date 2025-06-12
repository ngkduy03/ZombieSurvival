using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ZombieHealthController is responsible for managing the health of a zombie character.
/// </summary>
public class ZombieHealthController : ControllerBase, IHealthController
{
    private CharacterController characterController;
    private readonly Animator animator;
    private ZombieSetting zombieSettings;
    private bool isDead;
    private float currentHealth;

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
        if (isDead)
            return;

        currentHealth -= damage;
        Debug.Log($"Zombie took {damage} damage. Health: {currentHealth}/{zombieSettings.MaxHealth}");

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }
}
