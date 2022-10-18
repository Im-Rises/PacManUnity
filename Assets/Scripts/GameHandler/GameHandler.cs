using Ghosts;
using UnityEngine;

namespace GameHandler
{
    public class GameHandler : MonoBehaviour
    {
        public int State;

        public GameObject[] ghosts;
        private float _frightenTimer;

        // private Time time;
        private float _modeSwitcherTimer;

        // private int[] _timeToChangeMode = { 7, 20, 7, 20, 5, 20, 5 };
        private bool TimerPaused { get; set; }
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
            if (TimerPaused)
            {
                _frightenTimer += Time.deltaTime;
                if (_frightenTimer >= 7)
                {
                    _frightenTimer = 0;
                    TimerPaused = false;
                }

                return;
            }

            _modeSwitcherTimer += Time.deltaTime;
            if (!(_modeSwitcherTimer >= 7)) return;
            _modeSwitcherTimer = 0;
            if (State > 1)
                State = 0;
            ghosts[0].GetComponent<GhostMovement>().ChangeMode(State++);
        }

        // private void FixedUpdate()
        // {
        // }

        public void SetGhostsEaten()
        {
            TimerPaused = true;
            GhostEatenCount = 0;

            foreach (var ghost in GameObject.FindGameObjectsWithTag("Ghost"))
            {
                var ghostScript = ghost.GetComponent<GhostMovement>();
                ghostScript.bodyRenderer.enabled = true;
                ghostScript.GhostMode =
                    GhostGlobal.GhostMode.Frightened;
            }
        }
    }
}
