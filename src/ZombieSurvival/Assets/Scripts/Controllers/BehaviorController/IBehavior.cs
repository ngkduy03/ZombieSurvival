using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for player behavior.
/// </summary>
public interface IBehavior : IController
{
    /// <summary>
    /// Starts the player behavior.
    /// </summary>
    void Start();

    /// <summary>
    /// Updates the player behavior.
    /// </summary>
    void Update();
    
    void OnTakenDamage(float damageAmount);
    void OnTriggerEnter(Collider other);
}
