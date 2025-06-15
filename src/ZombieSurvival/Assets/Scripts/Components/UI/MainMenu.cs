using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// MainMenu is a component that manages the main menu UI, including opening and closing the level select popup,
/// </summary>
public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenuPanel;

    [SerializeField]
    private GameObject levelSelectPopupRoot;

    [SerializeField]
    private Button buttonPrefab;

    [SerializeField]
    private Button openLevelSelectButton;

    [SerializeField]
    private Button closePopupButton;

    [SerializeField]
    private Button onQuitButton;

    [SerializeField]
    private GameObject endgamePanel;

    [SerializeField]
    private TMP_Text endgameText;

    private const string YouDIedText = "You Died";
    private const string YouWonText = "You Won";

    private void OpenLevelSelectPopup()
    {
        mainMenuPanel.SetActive(false);
        levelSelectPopupRoot.SetActive(true);
    }
    public void CloseLevelSelectPopup()
    {
        levelSelectPopupRoot.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
        Debug.Log("Quit Game");
        Application.Quit();
#endif
    }

    public void ShowEndGamePanel(bool isWin)
    {
        endgamePanel.SetActive(true);
        endgameText.text = isWin ? YouWonText : YouDIedText;
    }

	private void OnEnable()
    {
        closePopupButton.onClick.AddListener(CloseLevelSelectPopup);
        openLevelSelectButton.onClick.AddListener(OpenLevelSelectPopup);
        onQuitButton.onClick.AddListener(QuitGame);
        mainMenuPanel.SetActive(true);
        levelSelectPopupRoot.SetActive(false);
    }

    private void OnDisable()
    {
        closePopupButton.onClick.RemoveAllListeners();
        openLevelSelectButton.onClick.RemoveAllListeners();
        onQuitButton.onClick.RemoveAllListeners();
        mainMenuPanel.SetActive(true);
        levelSelectPopupRoot.SetActive(false);
    }
}
