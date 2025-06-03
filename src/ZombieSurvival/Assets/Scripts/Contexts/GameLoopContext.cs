using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
[DefaultExecutionOrder(-1)]
public class GameLoopContext : BaseContext<ServiceInitializer>
{
    [SerializeField]
    private PlayerComponent playerComponent;

    protected override void Initialize(IServiceContainer serviceResolver)
    {
        playerComponent.CreateController();
    }

    protected override void Deinitialize()
    {
    }
}
