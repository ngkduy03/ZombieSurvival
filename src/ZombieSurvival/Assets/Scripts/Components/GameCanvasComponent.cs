using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// GameCanvasComponent is a component that manages the game canvas UI, including buttons for navigating back to the boot scene.
/// </summary>
public class GameCanvasComponent : MonoBehaviour
{
    private const string Boot = "BootScene";

    [SerializeField]
    private Button backToBootSceneButton;

    [SerializeField]
    private Button reloadButton;

    [SerializeField]
    private Button backToBootScene2ndButton;

    [SerializeField]
    private Button reload2ndButton;

    [SerializeField]
    private Button escButton;

    [SerializeField]
    private Button closeButton;

    [SerializeField]
    private GameObject settingsPanel;

    [SerializeField]
    private ILoadSceneService loadSceneService;

    /// <summary>
    /// Initializes the GameCanvasComponent with the provided load scene service. 
    /// </summary>
    public void Initialize(ILoadSceneService loadSceneService)
    {
        this.loadSceneService = loadSceneService;
    }

    private void OnEnable()
    {
        backToBootSceneButton.onClick.AddListener(OnLoadBoot);
        reloadButton.onClick.AddListener(OnSceneReloaded);
        backToBootScene2ndButton.onClick.AddListener(OnLoadBoot);
        reload2ndButton.onClick.AddListener(OnSceneReloaded);
        escButton.onClick.AddListener(OnEscButtonClicked);
        closeButton.onClick.AddListener(OnCloseButtonClicked);
    }

    private void OnDisable()
    {
        backToBootSceneButton.onClick.RemoveListener(OnLoadBoot);
        reloadButton.onClick.RemoveListener(OnSceneReloaded);
        backToBootScene2ndButton.onClick.RemoveListener(OnLoadBoot);
        reload2ndButton.onClick.RemoveListener(OnSceneReloaded);
        escButton.onClick.RemoveListener(OnEscButtonClicked);
        closeButton.onClick.RemoveListener(OnCloseButtonClicked);
    }

    private void OnLoadBoot()
    {
        loadSceneService.LoadSceneAsync(Boot);
    }

    private void OnSceneReloaded()
    {
        loadSceneService.ReloadCurrentScene();
    }

    private void OnEscButtonClicked()
    {
        settingsPanel.SetActive(true);
    }

    private void OnCloseButtonClicked()
    {
        settingsPanel.SetActive(false);
    }
}
