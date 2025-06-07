using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Controller for shooting behavior of the player.
/// </summary>
public class FireBulletController : ControllerBase, IAttackController
{
    private readonly Animator animator;
    private List<IGunController> gunControllers = new List<IGunController>();
    private FireButton fireButton;
    private Button switchGunButton;
    private Button reloadButton;
    private IGunController currentGunController;
    private int gunIndex = 0;
    private CancellationTokenSource cts = new CancellationTokenSource();

    public FireBulletController(
        Animator animator,
        List<IGunController> gunControllers,
        FireButton fireButton)

    {
        this.animator = animator;
        this.gunControllers = gunControllers;
        this.fireButton = fireButton;
    }

    public void Initialize()
    {
        Subscribe();
        if (gunControllers.Count > 0)
        {
            currentGunController = gunControllers[0];
        }
        else
        {
            Debug.LogError("No gun controllers available.");
        }
    }

    private void Subscribe()
    {
        fireButton.FireButtonPressed += OnAttack;
        switchGunButton.onClick.AddListener(SwitchGun);
        reloadButton.onClick.AddListener(OnReload);
    }

    private void Unsubscribe()
    {
        fireButton.FireButtonPressed -= OnAttack;
        switchGunButton.onClick.RemoveListener(SwitchGun);
        reloadButton.onClick.RemoveListener(OnReload);
    }

    private void SwitchGun()
    {
        gunIndex++;
        currentGunController = gunControllers[gunIndex % gunControllers.Count];
    }

    public void OnAttack()
    {
        currentGunController.FireBullet(cts.Token);
    }

    private void OnReload()
    {
        currentGunController.ReloadAsync(cts.Token);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            Unsubscribe();
            cts?.Cancel();
            cts?.Dispose();
        }
        base.Dispose(disposing);
    }
}
