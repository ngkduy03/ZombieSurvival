using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class MoveController : ControllerBase, IMovementController
{
    private readonly Animator animator;

    public MoveController(Animator animator)
    {
        this.animator = animator;
    }
}
