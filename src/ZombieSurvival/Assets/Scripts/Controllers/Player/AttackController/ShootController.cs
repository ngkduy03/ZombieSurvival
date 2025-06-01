using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class ShootController : ControllerBase, IAttackController
{
    private readonly Animator animator;

    public ShootController(Animator animator)
    {
        this.animator = animator;
    }
}
