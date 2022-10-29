using Ghosts;
using TMPro;
using UnityEngine;

namespace GameHandler
{
    public enum GameGhostsMode
    {
        Chase,
        Scatter
    }

    public class GameHandler : MonoBehaviour
    {
        // Singleton
        public static GameHandler Instance { get; private set; }

        // Ghosts
        private GhostAiMovement[] _ghosts;

        // Timers for modes
        public uint[] ghostsModeTimes = { 10, 7, 20, 7, 20, 5, 20, 5 };
        private GameGhostsMode _gameGhostsMode;
        private int _ghostsModeTimesIndex;
        private float _frightenTimer;
        private float _switcherModeTimer;
        private bool _switcherModeTimerPaused;

        // Door animator
        public Animator ghostHouseDoor;
        private static readonly int IsOpen = Animator.StringToHash("isOpen");

        // Current Ghost mode text
        public TextMeshProUGUI currentModeText;

        #region Awake Singleton

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this);
            else
                Instance = this;
        }

        #endregion

        #region Start and Update

        private void Start()
        {
            _ghosts = FindObjectsOfType<GhostAiMovement>();
            UpdateGhostsMode();
        }

        private void Update()
        {
            if (!_switcherModeTimerPaused)
            {
                if (_ghostsModeTimesIndex >= ghostsModeTimes.Length)
                {
                    // If the index is out of bounds, we are in the last mode, which is chase mode.
                    if (_gameGhostsMode != GameGhostsMode.Chase) // If we are not in chase mode, switch to chase mode.
                    {
                        SwitchingChaseMode();
                        PrintCurrentMode();
                    }

                    return;
                }

                _switcherModeTimer += Time.deltaTime;
                if (_switcherModeTimer >= ghostsModeTimes[_ghostsModeTimesIndex])
                {
                    _ghostsModeTimesIndex++;
                    _switcherModeTimer = 0;
                    UpdateGhostsMode();
                }
            }
            else
            {
                _frightenTimer += Time.deltaTime;
                if (_frightenTimer >= 7)
                {
                    _frightenTimer = 0;
                    _switcherModeTimerPaused = false;
                    ghostHouseDoor.SetBool(IsOpen, false);
                }
            }

            PrintCurrentMode();
        }

        #endregion

        #region Ghosts Mode

        private void UpdateGhostsMode()
        {
            if (_ghostsModeTimesIndex % 2 == 0)
                SwitchingChaseMode();
            else
                SwitchingScatterMode();
        }

        public void SwitchingChaseMode()
        {
            _gameGhostsMode = GameGhostsMode.Chase;
            foreach (var ghost in _ghosts)
                ghost.SetGhostMode(GhostMode.Chase);
        }

        public void SwitchingScatterMode()
        {
            _gameGhostsMode = GameGhostsMode.Scatter;
            foreach (var ghost in _ghosts)
                ghost.SetGhostMode(GhostMode.Scatter);
        }

        public void SwitchingFrightenedMode()
        {
            _switcherModeTimerPaused = true;
            foreach (var ghost in _ghosts) ghost.SetGhostMode(GhostMode.Frightened);
            ghostHouseDoor.SetBool(IsOpen, true);
        }

        #endregion

        #region Other

        public GameGhostsMode GetGameGhostsMode()
        {
            return _gameGhostsMode;
        }

        private void PrintCurrentMode()
        {
            if (_ghostsModeTimesIndex >= ghostsModeTimes.Length)
            {
                currentModeText.text = "Chase";
                return;
            }

            currentModeText.text =
                $"{_gameGhostsMode} for {ghostsModeTimes[_ghostsModeTimesIndex] - _switcherModeTimer:F2}s";
        }

        #endregion
    }
}
