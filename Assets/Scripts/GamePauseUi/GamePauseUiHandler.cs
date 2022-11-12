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

    private int _curentButtonIndex = 0;
    private Vector2 _inputDirection;

    private Vector2 _lastDirection;
    // private Button[] _buttons;

    private void Start()
    {
        resumeButton.onClick.AddListener(ResumeGame);
        quitButton.onClick.AddListener(QuitGame);
        settingsButton.onClick.AddListener(SettingsGame);
        restartButton.onClick.AddListener(RestartGame);
        backSettingsButton.onClick.AddListener(BackSettingsGame);

        // _buttons = GetComponentsInChildren<Button>();

        // SelectButton(_buttons[_curentButtonIndex]);
        settingsPanel.SetActive(false);

        SelectButton(resumeButton);
    }

    public void Reset()
    {
        settingsPanel.SetActive(false);
    }

    public void MoveSelection(Vector2 direction)
    {
        if (_lastDirection == direction || !gameObject.activeSelf)
            // _settingsController.SetInputDirection(direction);
            return;

        if (direction.y > 0.5f)
        {
            _curentButtonIndex--;
            if (_curentButtonIndex < 0) _curentButtonIndex = 4 - 1;
        }
        else if (direction.y < -0.5f)
        {
            _curentButtonIndex = (_curentButtonIndex + 1) % 4;
        }

        DeselectButton(resumeButton);
        DeselectButton(restartButton);
        DeselectButton(settingsButton);
        DeselectButton(quitButton);

        switch (_curentButtonIndex)
        {
            case 0:
                SelectButton(resumeButton);
                break;
            case 1:
                SelectButton(restartButton);
                break;
            case 2:
                SelectButton(settingsButton);
                break;
            case 3:
                SelectButton(quitButton);
                break;
        }

        /*if (direction.y > 0.5f)
        {
            DeselectButton(_buttons[_curentButtonIndex]);
            _curentButtonIndex--;
            if (_curentButtonIndex < 0) _curentButtonIndex = _buttons.Length - 1;
            SelectButton(_buttons[_curentButtonIndex]);
        }
        else if (direction.y < -0.5f)
        {
            DeselectButton(_buttons[_curentButtonIndex]);
            _curentButtonIndex = (_curentButtonIndex + 1) % _buttons.Length;
            SelectButton(_buttons[_curentButtonIndex]);
        }*/

        _lastDirection = direction;
    }

    private void SelectButton(Button button)
    {
        button.Select();
        button.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
    }

    private void DeselectButton(Button button)
    {
        button.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    private void ResumeGame()
    {
        Debug.Log("Resume");
        if (GameStartHandler.Instance.enabled)
            GameStartHandler.Instance.TogglePause();
        GameHandler.GameHandler.Instance.TogglePause();
        MusicHandler.MusicHandler.Instance.TogglePause();
    }

    private void QuitGame()
    {
        Debug.Log("Quit");
        GameHandler.GameHandler.Instance.GoToMainMenu();
        Debug.Log("Go to main menu");
    }

    private void SettingsGame()
    {
        Debug.Log("Settings");
        settingsPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    private void BackSettingsGame()
    {
        Debug.Log("Back");
        settingsPanel.SetActive(false);
        gameObject.SetActive(true);
    }

    private void RestartGame()
    {
        Debug.Log("Restart");
        GameHandler.GameHandler.Instance.RestartGame();
        // Reset();
    }
}
