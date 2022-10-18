using UnityEngine;

namespace GameHandler
{
    public enum GameGhostMode
    {
        Scatter = 0,
        Chase = 1
    }

    public class GameHandler : MonoBehaviour
    {
        // private int[] _timeToChangeMode = { 7, 20, 7, 20, 5, 20, 5 };
        // private int _currentMode = 0;
        public GameGhostMode State;

        public GameObject[] ghosts;
        private float _frightenTimer;

        private float _modeSwitcherTimer;
        public bool TimerPaused { get; set; }
        public int GhostEatenCount { get; private set; }
        public static GameHandler Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this);
            else
                Instance = this;
        }

        private void Update()
        {
            // if (TimerPaused)
            // {
            //     _frightenTimer += Time.deltaTime;
            //     if (_frightenTimer >= 7)
            //     {
            //         _frightenTimer = 0;
            //         TimerPaused = false;
            //     }
            //
            //     return;
            // }
            //
            // _modeSwitcherTimer += Time.deltaTime;
            // if (!(_modeSwitcherTimer >= 7)) return;
            // _modeSwitcherTimer = 0;
            // if (State > 1)
            //     State = 0;
            // ghosts[0].GetComponent<GhostMovement>().ChangeMode(State++);
        }

        // private void FixedUpdate()
        // {
        // }

        // public void SetGhostsEaten()
        // {
        //     // GhostEatenCount = 0;
        //     // foreach (var ghost in GameObject.FindGameObjectsWithTag("Ghost"))
        //     // {
        //     //     var ghostScript = ghost.GetComponent<GhostMovement>();
        //     //     ghostScript.bodyRenderer.enabled = true;
        //     //     ghostScript.GhostMode =
        //     //         GhostGlobal.GhostMode.Frightened;
        //     // }
        // }
    }
}
