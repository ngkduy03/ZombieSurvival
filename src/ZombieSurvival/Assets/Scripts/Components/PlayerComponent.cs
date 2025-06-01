using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PlayerComponent is a component that manages the player controller in the game.
/// </summary>
public class PlayerComponent : SceneComponent<PlayerController>
{
    protected override PlayerController CreateControllerImpl()
    {
        throw new System.NotImplementedException();
    }
}