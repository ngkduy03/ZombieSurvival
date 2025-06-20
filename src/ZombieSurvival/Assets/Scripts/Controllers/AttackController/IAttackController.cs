using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Interface for the attack controller.
/// </summary>
public interface IAttackController : IController
{
    /// <summary>
    /// Initializes the attack controller, setting up necessary components or states.
    /// </summary>
    void Initialize();

    /// <summary>
    /// Handles the attack action.
    /// </summary>
    /// <param name="isAttacking"></param>
    void OnAttack(bool isAttacking);

    /// <summary>
    /// Gets or sets a value indicating whether the controller is currently attacking.
    /// </summary>
    bool IsAttacking { get; }
}
