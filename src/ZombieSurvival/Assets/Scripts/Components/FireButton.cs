using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// FireButton is a component that handles the fire button input in the UI.
/// It implements the IUpdateSelectedHandler interface to respond to button press events.
/// </summary>
public class FireButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    /// <summary>
    /// Event that is triggered when the fire button is pressed.
    /// </summary>
    public event Action<bool> FireButtonPressed;
    private bool isHolding = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false;
    }

    void Update()
    {
        FireButtonPressed?.Invoke(isHolding);
    }
}