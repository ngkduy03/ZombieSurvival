using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieComponent : SceneComponent<ZombieController>
{
    private ZombieController zombieController;
    protected override ZombieController CreateControllerImpl()
    {
        zombieController = new ZombieController();
        return zombieController;
    }
}
