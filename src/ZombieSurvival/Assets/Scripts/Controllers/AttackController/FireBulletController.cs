using System;
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
    private readonly AudioSource audioSource;
    private List<IGunController> gunControllers = new List<IGunController>();
    private FireButton fireButton;
    private Button switchGunButton;
    private Button reloadButton;
    private IGunController currentGunController;
    private int gunIndex = 0;
    private const int FiredPerSec = 1;
    private const string FireRate = "FireRate";
    private CancellationTokenSource cts = new CancellationTokenSource();

    public FireBulletController(
        Animator animator,
        List<IGunController> gunControllers,
        FireButton fireButton,
        Button switchGunButton,
        Button reloadButton,
        AudioSource audioSource)

    {
        this.animator = animator;
        this.gunControllers = gunControllers;
        this.fireButton = fireButton;
        this.switchGunButton = switchGunButton;
        this.reloadButton = reloadButton;
        this.audioSource = audioSource;
    }

    /// <inheritdoc/>
    public void Initialize()
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
        audioSource.PlayOneShot(currentGunController.GunSetting.SwitchGunClip);
        animator.SetFloat(FireRate, FiredPerSec / currentGunController.GunSetting.FireRate);
    }

    /// <inheritdoc/>
    public void OnAttack(bool isPressed)
    {
        var shootLayer = (int)PlayerAnimationLayerEnum.ShootLayer;

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

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        Unsubscribe();
        currentGunController.SetActive(false);
        animator.SetLayerWeight((int)PlayerAnimationLayerEnum.ShootLayer, 0f);

        foreach (var gunController in gunControllers)
        {
            gunController.SetActive(false);
        }

        cts?.Cancel();
        cts?.Dispose();
        cts = null;
    }
}
