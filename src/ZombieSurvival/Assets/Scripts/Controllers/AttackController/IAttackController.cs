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
    /// Handles the attack action.
    /// </summary>
    /// <param name="isAttacking"></param>
    void OnAttack(bool isAttacking);
}
