using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// FireButton is a component that handles the fire button input in the UI.
/// It implements the IUpdateSelectedHandler interface to respond to button press events.
/// </summary>
public class FireButton : MonoBehaviour, IUpdateSelectedHandler
{
    /// <summary>
    /// Event that is triggered when the fire button is pressed.
    /// </summary>
    public event Action FireButtonPressed;

    /// <inheritdoc />
    public void OnUpdateSelected(BaseEventData eventData)
    {
        FireButtonPressed?.Invoke();
    }
}
