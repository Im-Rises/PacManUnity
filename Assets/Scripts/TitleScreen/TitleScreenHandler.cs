using UnityEngine;
using UnityEngine.Audio;
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

        private void Start()
        {
            newGameButton.onClick.AddListener(StartNewGame);
            exitButton.onClick.AddListener(ExitGame);
            settingsButton.onClick.AddListener(OpenSettings);
            creditsButton.onClick.AddListener(OpenCredits);
            backSettingsButton.onClick.AddListener(CloseSettings);
            mainMenuPanel.SetActive(true);
            settingsPanel.SetActive(false);
        }


        private void StartNewGame()
        {
            SceneManager.LoadScene(SceneNameConstants.Level1);
        }

        private void ExitGame()
        {
            Debug.Log("Exit Game");
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

        private void OpenCredits()
        {
            SceneManager.LoadScene(SceneNameConstants.Credits);
        }
    }
}
