using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BlockObjectComponent is a component that represents a block object in the game.
/// </summary>
public class BlockObjectComponent : SceneComponent<BlockObjectController>
{
    private int totalZombies;
    public BlockObjectController BlockObjectController { get; private set; }
    protected override BlockObjectController CreateControllerImpl()
    {
        BlockObjectController = new BlockObjectController(transform, totalZombies);
        return BlockObjectController;
    }

    /// <summary>
    /// Initializes the block object component with the total number of zombies.
    /// </summary>
    /// <param name="totalZombies"></param>
    public void Initialize(int totalZombies)
    {
        this.totalZombies = totalZombies;
    }
}
