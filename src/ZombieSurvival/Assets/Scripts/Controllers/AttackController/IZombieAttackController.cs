using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for zombie attack controller.
/// </summary>
public interface IZombieAttackController : IController
{
    /// <summary>
    /// Initializes the zombie attack controller with necessary settings and parameters.
    /// </summary>
    void Initialize();

    /// <summary>
    /// Handles the attack event triggered by the zombie's attack animation.
    /// </summary>
    /// <param name="player">The player transform to attack</param>
    void AttackPlayer(Transform player);

    /// <summary>
    /// Gets the damage amount the zombie inflicts per attack.
    /// </summary>
    float AttackDamage { get; }

    /// <summary>
    /// Gets or sets a value indicating whether the zombie is currently attacking.
    /// </summary>
    bool IsAttacking { get; }
}
