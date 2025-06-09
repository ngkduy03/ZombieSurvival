using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for movement controllers. 
/// </summary>
public interface IMovementController
{
    /// <summary>
    /// Define movement method.
    /// </summary>
    void Move();

    /// <summary>
    /// Define Look method.
    /// </summary>
    void Look();
}
