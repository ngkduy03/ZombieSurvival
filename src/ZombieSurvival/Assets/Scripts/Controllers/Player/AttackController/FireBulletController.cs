using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controller for shooting behavior of the player.
/// </summary>
public class FireBulletController : ControllerBase, IAttackController
{
    private readonly Animator animator;
    private Transform bulletSpawnPoint;
    private List<IGunController> gunControllers = new List<IGunController>();
    private Button fireButton;
    private IGunController currentGunController;
    private CancellationTokenSource cts = new CancellationTokenSource();

    public FireBulletController(
        Animator animator,
        Transform bulletSpawnPoint,
        List<IGunController> gunControllers,
        Button fireButton)

    {
        this.animator = animator;
        this.bulletSpawnPoint = bulletSpawnPoint;
        this.gunControllers = gunControllers;
        this.fireButton = fireButton;
    }

    public void Attack()
    {
        currentGunController.FireBullet(cts.Token);
    }
}
