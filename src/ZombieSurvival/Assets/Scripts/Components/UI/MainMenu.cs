using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject levelSelectPopupRoot;
    [SerializeField] private Button buttonPrefab;
    [SerializeField] private Button openLevelSelectButton;
    [SerializeField] private Button closePopupButton;
    [SerializeField] private Button onQuitButton;
   
    private void OpenLevelSelectPopup()
    {
        mainMenuPanel.SetActive(false);
        levelSelectPopupRoot.SetActive(true);
        PopulateLevelButtons();
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

	public void PopulateLevelButtons()
    {
    }

	private void OnEnable()
    {
        Debug.Log("MainMenu OnEnable called");
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
