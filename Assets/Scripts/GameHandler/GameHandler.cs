using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameHandler
{
    public class GameHandler : MonoBehaviour
    {
        // private int[] _timeToChangeMode = { 7, 20, 7, 20, 5, 20, 5 };

        // private Time time;
        private int State { get; set; } = 0;

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
