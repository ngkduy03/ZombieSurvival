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

    /// <inheritdoc />
    protected override void Initialize(IServiceContainer serviceResolver)
    {
        playerComponent.CreateController();
    }

    /// <inheritdoc />
    protected override void Deinitialize()
    {
    }
}
