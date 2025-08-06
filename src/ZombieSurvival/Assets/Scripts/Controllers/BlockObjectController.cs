using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BlockObjectController is a controller that manages the block object in the game.
/// It checks if the block object should be destroyed based on the number of zombies remaining.
/// </summary>
public class BlockObjectController : ControllerBase
{
    private readonly Transform transform;
    private int totalZombies;
    public BlockObjectController(
        Transform transform,
        int totalZombies)
    {
        this.transform = transform;
        this.totalZombies = totalZombies;
    }

    /// <summary>
    /// Checks if the block object should be destroyed based on the number of zombies remaining.
    /// </summary>
    public void CheckDestroy()
    {
        totalZombies--;
        if (totalZombies <= 0)
        {
            GameObject.Destroy(transform.gameObject);
        }
    }
}
