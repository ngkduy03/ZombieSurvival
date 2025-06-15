using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// PlayerComponent is a component that manages the player controller in the game.
/// </summary>
public class PlayerComponent : SceneComponent<PlayerController>
{
    [SerializeField]
    private CharacterController characterController;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private InputActionReference[] inputActions;

    [SerializeField]
    private FireButton fireButton;

    [SerializeField]
    private Button switchGunButton;

    [SerializeField]
    private Button reloadButton;

    [SerializeField]
    private Slider healthSlider;

    [SerializeField]
    private RiffleComponent riffleComponent;

    [SerializeField]
    private ShotgunComponent shotgunComponent;

    [SerializeField]
    private float maxHealth = 100f;

    [SerializeField]
    private AudioSource gunAudioSource;

    [SerializeField]
    private AudioSource movementAudioSource;

    [SerializeField]
    private GameObject endgamePanel;

    [SerializeField]
    private TMP_Text endgameText;
    private const string YouWonText = "You Won";
    private const string YouDieText = "You Die";
    private const int DelayPopUp = 3500;
    private float currentHealth;
    private bool isDead = false;

    private List<IGunController> gunControllers = new List<IGunController>();
    public PlayerController playerController { get; private set; }
    private CancellationTokenSource cts = new();

    protected override PlayerController CreateControllerImpl()
    {
        var riffleController = riffleComponent.CreateController();
        var shotgunController = shotgunComponent.CreateController();
        gunControllers.Add(riffleController);
        gunControllers.Add(shotgunController);

        playerController = new PlayerController(transform, animator, inputActions, characterController, gunControllers, gunAudioSource, movementAudioSource, maxHealth, fireButton, switchGunButton, reloadButton, healthSlider);
        return playerController;
    }

    private void Awake()
    {
        playerController.Initialize();
        currentHealth = maxHealth;
    }

    private void OnEnable()
    {
        playerController?.OnEnable();
    }

    private void OnDisable()
    {
        playerController?.OnDisable();
        cts.Cancel();
        cts.Dispose();
        cts = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out WareHouseComponent wareHouse))
        {
            endgamePanel.SetActive(true);
            endgameText.text = YouWonText;
        }
    }

    private void Update()
    {
        if (playerController.GetDieStatus() && !isDead)
        {
            isDead = true;
            ShowGameOverAsync(cts.Token).Forget();
            return;
        }
        else
        {
            playerController?.Update();
        }
    }

    private async UniTask ShowGameOverAsync(CancellationToken cancellationToken)
    {
        await UniTask.Delay(DelayPopUp, cancellationToken: cancellationToken);
        endgamePanel.SetActive(true);
        endgameText.text = YouDieText;
        playerController.Dispose();
    }
}