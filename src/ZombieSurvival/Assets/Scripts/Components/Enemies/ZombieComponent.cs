using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ZombieComponent is a component that manages the zombie controller in the game.
/// </summary>
public class ZombieComponent : SceneComponent<ZombieController>
{
    private ZombieController zombieController;
    protected override ZombieController CreateControllerImpl()
    {
        zombieController = new ZombieController();
        return zombieController;
    }
}
