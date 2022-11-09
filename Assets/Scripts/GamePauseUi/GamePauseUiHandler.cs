using System;
using System.Collections;
using System.Collections.Generic;
using GameHandler;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUiHandler : MonoBehaviour
{
    public Button resumeButton;
    public Button quitButton;
    public Button settingsButton;
    public Button restartButton;

    public Button backSettingsButton;

    public GameObject settingsPanel;

    private void Start()
    {
        resumeButton.onClick.AddListener(ResumeGame);
        quitButton.onClick.AddListener(QuitGame);
        settingsButton.onClick.AddListener(SettingsGame);
        restartButton.onClick.AddListener(RestartGame);
        backSettingsButton.onClick.AddListener(BackSettingsGame);

        settingsPanel.SetActive(false);
    }

    public void Reset()
    {
        settingsPanel.SetActive(false);
    }

    private void ResumeGame()
    {
        if (GameStartHandler.Instance.enabled)
            GameStartHandler.Instance.TogglePause();
        GameHandler.GameHandler.Instance.TogglePause();
        MusicHandler.MusicHandler.Instance.TogglePause();
    }

    private void QuitGame()
    {
        GameHandler.GameHandler.Instance.GoToMainMenu();
        Debug.Log("Go to main menu");
    }

    private void SettingsGame()
    {
        settingsPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    private void BackSettingsGame()
    {
        settingsPanel.SetActive(false);
        gameObject.SetActive(true);
    }

    private void RestartGame()
    {
        GameHandler.GameHandler.Instance.RestartGame();
        // Reset();
    }
}
