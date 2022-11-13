using GameHandler;
using UnityEngine;
using UnityEngine.UI;

namespace GamePauseUi
{
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

        private void Start()
        {
            resumeButton.onClick.AddListener(ResumeGame);
            quitButton.onClick.AddListener(QuitGame);
            settingsButton.onClick.AddListener(SettingsGame);
            restartButton.onClick.AddListener(RestartGame);
            backSettingsButton.onClick.AddListener(BackSettingsGame);

            settingsPanel.SetActive(false);

            ResetInitialButton();
        }

        public void ResetInitialButton()
        {
            _curentButtonIndex = 0;
            SelectButton(resumeButton);
            DeselectButton(restartButton);
            DeselectButton(settingsButton);
            DeselectButton(quitButton);
        }

        public void Reset()
        {
            settingsPanel.SetActive(false);
        }

        public void MoveSelection(Vector2 direction)
        {
            if (_lastDirection == direction || !gameObject.activeSelf) return;

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
            if (GameStartHandler.Instance.enabled)
                GameStartHandler.Instance.TogglePause();
            GameHandler.GameHandler.Instance.TogglePause();
            MusicHandler.MusicHandler.Instance.TogglePause();
        }

        private void QuitGame()
        {
            GameHandler.GameHandler.Instance.GoToMainMenu();
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
        }
    }
}
