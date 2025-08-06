using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GameLoopContext is a context that initializes the game loop components for the play scene. 
/// </summary>
[DefaultExecutionOrder(-1)]
public class GameLoopContext : BaseContext<ServiceInitializer>
{
    [SerializeField]
    private PlayerComponent playerComponent;
    private PlayerController playerController;

    [SerializeField]
    List<ZombieComponent> zombieComponents;
    List<ZombieController> zombieControllers = new();

    [SerializeField]
    private BlockObjectComponent blockObject;
    private BlockObjectController blockObjectController;
    
    [SerializeField]
    private GameCanvasComponent gameCanvasComponent;
    private ILoadSceneService loadSceneService;

    /// <inheritdoc />
    protected override void Initialize(IServiceContainer serviceResolver)
    {
        loadSceneService = serviceResolver.Resolve<ILoadSceneService>();
        playerController = playerComponent.CreateController();
        blockObject.Initialize(zombieComponents.Count);
        blockObjectController = blockObject.CreateController();
        gameCanvasComponent.Initialize(loadSceneService);
        foreach (var zombieComponent in zombieComponents)
        {
            zombieComponent.Initialize(blockObjectController);
            var zombieController = zombieComponent.CreateController();
            zombieControllers.Add(zombieController);
        }
    }

    /// <inheritdoc />
    protected override void Deinitialize()
    {
        playerController?.Dispose();
        blockObjectController?.Dispose();
        foreach (var zombieController in zombieControllers)
        {
            zombieController?.Dispose();
        }
    }
}
