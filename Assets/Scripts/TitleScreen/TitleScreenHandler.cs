using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TitleScreen
{
    public class TitleScreenHandler : MonoBehaviour
    {
        public Button newGameButton;
        public Button exitButton;
        public Button settingsButton;
        public Button creditsButton;
        public Button backSettingsButton;

        public GameObject mainMenuPanel;
        public GameObject settingsPanel;

        private Button[] _mainMenuButton;
        private int _currentMainMenuButtonIndex;

        private Vector2 _inputDirection;
        private Vector2 _lastDirection;

        private SettingsController.SettingsController _settingsController;

        private void Start()
        {
            newGameButton.onClick.AddListener(StartNewGame);
            exitButton.onClick.AddListener(ExitGame);
            settingsButton.onClick.AddListener(OpenSettings);
            creditsButton.onClick.AddListener(OpenCredits);
            backSettingsButton.onClick.AddListener(CloseSettings);
            mainMenuPanel.SetActive(true);
            settingsPanel.SetActive(false);

            _mainMenuButton = mainMenuPanel.GetComponentsInChildren<Button>();
            SelectButton(_mainMenuButton[0]);

            _settingsController = settingsPanel.GetComponent<SettingsController.SettingsController>();
        }

        private void Update()
        {
            var direction = _inputDirection;

            // if the direction is the same as the last direction, ignore it
            // or if the panel is not active, ignore it
            if (_lastDirection == direction || !mainMenuPanel.gameObject.activeSelf)
            {
                _settingsController.SetInputDirection(direction);
                return;
            }

            if (direction.y > 0.5f)
            {
                DeselectButton(_mainMenuButton[_currentMainMenuButtonIndex]);
                _currentMainMenuButtonIndex--;
                if (_currentMainMenuButtonIndex < 0) _currentMainMenuButtonIndex = _mainMenuButton.Length - 1;
                SelectButton(_mainMenuButton[_currentMainMenuButtonIndex]);
            }
            else if (direction.y < -0.5f)
            {
                DeselectButton(_mainMenuButton[_currentMainMenuButtonIndex]);
                _currentMainMenuButtonIndex = (_currentMainMenuButtonIndex + 1) % _mainMenuButton.Length;
                SelectButton(_mainMenuButton[_currentMainMenuButtonIndex]);
            }

            _lastDirection = direction;
        }

        private void StartNewGame()
        {
            SceneManager.LoadScene(SceneNameConstants.LevelSelector);
        }

        private void ExitGame()
        {
            Application.Quit();
        }

        private void OpenSettings()
        {
            mainMenuPanel.SetActive(false);
            settingsPanel.SetActive(true);
        }

        private void CloseSettings()
        {
            mainMenuPanel.SetActive(true);
            settingsPanel.SetActive(false);
        }

        private void OnCancel()
        {
            if (settingsPanel.activeSelf) CloseSettings();
        }

        private void OnMove(InputValue value)
        {
            Debug.Log("OnMove");
            _inputDirection = value.Get<Vector2>();

            if (_inputDirection.y != 0) _inputDirection.x = 0; // Create a priority for y movement

            if (_inputDirection != Vector2.zero)
                _inputDirection = _inputDirection.normalized; // Normalize the output to be 1 or -1 not floating values
        }

        private void SelectButton(Button button)
        {
            button.Select();
            // EventSystem.current.SetSelectedGameObject(button.gameObject);
            button.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        }

        private void DeselectButton(Button button)
        {
            button.transform.localScale = new Vector3(1f, 1f, 1f);
        }

        private void OpenCredits()
        {
            SceneManager.LoadScene(SceneNameConstants.Credits);
        }
    }
}
