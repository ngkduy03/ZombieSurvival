using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for health controller.
/// </summary>
public interface IHealthController : IController
{
    /// <summary>
    /// Method to initialize the health controller.
    /// </summary>
    void Initialize();
    
    /// <summary>
    /// Method to take damage from an entity.
    /// </summary>
    /// <param name="damage"></param>
    void TakeDamage(float damage);
    
    /// <summary>
    /// Method to heal the entity.
    /// </summary>
    void Die();
    
    /// <summary>
    /// Method to get the current health of the entity.
    /// </summary>
    float GetHealth();
}
