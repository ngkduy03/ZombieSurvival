using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// MainMenu is a component that manages the main menu UI, including opening and closing the level select popup,
/// </summary>
[DefaultExecutionOrder(-1)]
public class BootSceneContext : BaseContext<ServiceInitializer>
{
    private const string GameLoop = "GameLoopScene";

    [SerializeField]
    private GameObject mainMenuPanel;

    [SerializeField]
    private GameObject levelSelectPopupRoot;

    [SerializeField]
    private Button openLevelSelectButton;

    [SerializeField]
    private Button closePopupButton;

    [SerializeField]
    private Button onQuitButton;

    [SerializeField]
    private Button levelOneButton;

    private ILoadSceneService loadSceneService;

    protected override void Initialize(IServiceContainer serviceResolver)
    {
        loadSceneService = serviceResolver.Resolve<ILoadSceneService>();
    }

    private void OnLevelSelectPanelOpened()
    {
        mainMenuPanel.SetActive(false);
        levelSelectPopupRoot.SetActive(true);
    }
    private void OnLevelSelectPanelClosed()
    {
        levelSelectPopupRoot.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    private void OnGameQuit()
    {
        loadSceneService.QuitGame();
    }

    private void OnLoadLevelOne()
    {
        loadSceneService.LoadSceneAsync(GameLoop);
    }

    private void OnEnable()
    {
        closePopupButton.onClick.AddListener(OnLevelSelectPanelClosed);
        openLevelSelectButton.onClick.AddListener(OnLevelSelectPanelOpened);
        onQuitButton.onClick.AddListener(OnGameQuit);
        levelOneButton.onClick.AddListener(OnLoadLevelOne);
        mainMenuPanel.SetActive(true);
        levelSelectPopupRoot.SetActive(false);
    }

    private void OnDisable()
    {
        closePopupButton.onClick.RemoveAllListeners();
        openLevelSelectButton.onClick.RemoveAllListeners();
        onQuitButton.onClick.RemoveAllListeners();
        levelOneButton.onClick.RemoveListener(OnLoadLevelOne);
        mainMenuPanel.SetActive(true);
        levelSelectPopupRoot.SetActive(false);
    }


    protected override void Deinitialize()
    {
    }
}