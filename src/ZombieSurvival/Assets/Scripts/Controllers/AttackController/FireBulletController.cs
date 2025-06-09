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
    private const int FiredPerSec = 1;
    private readonly Animator animator; //TODO: 2 layer animator.
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
        FireButton fireButton,
        Button switchGunButton,
        Button reloadButton)

    {
        this.animator = animator;
        this.gunControllers = gunControllers;
        this.fireButton = fireButton;
        this.switchGunButton = switchGunButton;
        this.reloadButton = reloadButton;
        Initialize();
    }

    private void Initialize()
    {
        Subscribe();
        if (gunControllers.Count > 0)
        {
            currentGunController = gunControllers[0];
            currentGunController.SetActive(true);
        }
        else
        {
            Debug.LogError("No gun controllers available.");
        }
    }

    private void Subscribe()
    {
        fireButton.FireButtonPressed += OnAttack;
        switchGunButton.onClick.AddListener(OnGunSwitched);
        reloadButton.onClick.AddListener(OnReload);
    }

    private void Unsubscribe()
    {
        fireButton.FireButtonPressed -= OnAttack;
        switchGunButton.onClick.RemoveListener(OnGunSwitched);
        reloadButton.onClick.RemoveListener(OnReload);
    }

    private void OnGunSwitched()
    {
        gunIndex++;
        currentGunController.SetActive(false);
        currentGunController = gunControllers[gunIndex % gunControllers.Count];
        currentGunController.SetActive(true);
        animator.SetFloat("FireRate", FiredPerSec / currentGunController.GunSetting.FireRate);
    }

    /// <inheritdoc/>
    public void OnAttack(bool isPressed)
    {
        var shootLayer = (int)PlayerAnimationLayerEnum.SHootLayer;

        if (isPressed && !currentGunController.IsReloaded())
        {
            if (currentGunController.CanFire())
            {
                currentGunController.FireBullet(cts.Token);
                animator.SetLayerWeight(shootLayer, 1f);
            }
        }
        else
        {
            animator.SetLayerWeight(shootLayer, 0f);
        }
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
