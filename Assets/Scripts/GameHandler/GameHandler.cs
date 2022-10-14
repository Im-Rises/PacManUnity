using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameHandler
{
    public class GameHandler : MonoBehaviour
    {
        // private Time time;
        // private int state { get; set; }

        private static GameHandler Instance { get; set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this);
            else
                Instance = this;
        }

        public void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
