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
    /// Updates the player behavior.
    /// </summary>
    void Update();
}
