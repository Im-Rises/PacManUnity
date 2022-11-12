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
            SelectButton(_mainMenuButton[0].gameObject);
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
            var direction = value.Get<Vector2>();
            if (direction.y > 0.5f)
            {
                DeselectButton(EventSystem.current.currentSelectedGameObject);
                _currentMainMenuButtonIndex--;
                if (_currentMainMenuButtonIndex < 0) _currentMainMenuButtonIndex = _mainMenuButton.Length - 1;
            }
            else if (direction.y < -0.5f)
            {
                DeselectButton(EventSystem.current.currentSelectedGameObject);
                _currentMainMenuButtonIndex = (_currentMainMenuButtonIndex + 1) % _mainMenuButton.Length;
            }

            SelectButton(_mainMenuButton[_currentMainMenuButtonIndex].gameObject);
        }

        private void SelectButton(GameObject button)
        {
            EventSystem.current.SetSelectedGameObject(button);
            button.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        }

        private void DeselectButton(GameObject button)
        {
            button.transform.localScale = new Vector3(1f, 1f, 1f);
        }

        private void OpenCredits()
        {
            SceneManager.LoadScene(SceneNameConstants.Credits);
        }
    }
}
