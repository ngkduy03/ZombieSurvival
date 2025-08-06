using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for detection controllers.
/// </summary>
public interface IDetectionController : IController
{
    /// <summary>
    /// Checks if the target is within the detection range and angle.
    /// </summary>
    /// <returns></returns>
    bool CheckInRange();

    /// <summary>
    /// Gets the component of the player if detected within range.
    /// </summary>
    /// <returns></returns>
    PlayerComponent GetPlayerComponent();

    /// <summary>
    /// Gets the transform of this detection controller's owner.
    /// </summary>
    /// <returns></returns>
    Transform GetTransform();
}
