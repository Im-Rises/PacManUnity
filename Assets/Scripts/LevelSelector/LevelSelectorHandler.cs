using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LevelSelector
{
    public class LevelSelectorHandler : MonoBehaviour
    {
        public Button originalLevelButton;
        public Button customLevel1Button;
        public Button customLevel2Button;

        private void Start()
        {
            originalLevelButton.onClick.AddListener(() => LoadLevel(SceneNameConstants.OriginalLevel));
            customLevel1Button.onClick.AddListener(() => LoadLevel(SceneNameConstants.CustomLevel1));
            customLevel2Button.onClick.AddListener(() => LoadLevel(SceneNameConstants.CustomLevel2));
        }

        private void LoadLevel(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
