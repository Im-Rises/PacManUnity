using Ghosts;
using TMPro;
using UnityEngine;

namespace GameHandler
{
    // public enum GameGhostsMode
    // {
    //     Chase,
    //     Scatter
    // }

    public class GameHandler : MonoBehaviour
    {
        // Singleton
        public static GameHandler Instance { get; private set; }

        // Current Game Mode
        public GhostMode GameGhostsMode { get; private set; }

        // Ghosts
        private GhostAiMovement[] _ghosts;


        // Timer for scatter and chase
        public uint[] ghostsModeTimes = { 15, 10, 25, 10, 25, 10, 25, 5 };
        private int _ghostsModeTimesIndex;
        private float _switcherModeTimer;

        // Timer for ghost frightened
        public uint frightenedTime = 10;
        private float _frightenTimer;
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
                // If the index is out of bounds, we are in the last mode, which is chase mode.
                if (_ghostsModeTimesIndex >= ghostsModeTimes.Length)
                {
                    if (GameGhostsMode != GhostMode.Chase)
                    {
                        // If we are not in chase mode, switch to chase mode.
                        SwitchingChaseMode();
                        PrintCurrentMode();
                    }

                    return;
                }

                // Update the timer and check if it's time to switch mode.
                _switcherModeTimer += Time.deltaTime;
                if (_switcherModeTimer >= ghostsModeTimes[_ghostsModeTimesIndex])
                {
                    _ghostsModeTimesIndex++;
                    _switcherModeTimer = 0;
                    UpdateGhostsMode();
                }

                // Update the current mode text.
                PrintCurrentMode();
            }
            else
            {
                // Update the timer and check if it's time to end the frighten mode.
                _frightenTimer += Time.deltaTime;
                if (_frightenTimer >= 7)
                {
                    _frightenTimer = 0;
                    _switcherModeTimerPaused = false;
                    ghostHouseDoor.SetBool(IsOpen, false);
                    UpdateGhostsMode();
                }

                PrintFrightenMode();
            }
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

        private void SwitchingChaseMode()
        {
            GameGhostsMode = GhostMode.Chase;
            foreach (var ghost in _ghosts)
                ghost.SetGhostMode(GhostMode.Chase);
        }

        private void SwitchingScatterMode()
        {
            GameGhostsMode = GhostMode.Scatter;
            foreach (var ghost in _ghosts)
                ghost.SetGhostMode(GhostMode.Scatter);
        }

        public void SwitchingFrightenedMode()
        {
            // Not changing the game mode for eaten ghosts to be able to switch back to the normal current mode.
            // GameGhostsMode = GhostMode.Frightened;
            _switcherModeTimerPaused = true;
            foreach (var ghost in _ghosts) ghost.SetGhostMode(GhostMode.Frightened);
            ghostHouseDoor.SetBool(IsOpen, true);
        }

        #endregion

        #region Print text for current mode

        private void PrintCurrentMode()
        {
            if (_ghostsModeTimesIndex >= ghostsModeTimes.Length)
            {
                currentModeText.text = "Chase";
                return;
            }

            currentModeText.text =
                $"{GameGhostsMode} for {ghostsModeTimes[_ghostsModeTimesIndex] - _switcherModeTimer:F2}s";
        }

        private void PrintFrightenMode()
        {
            currentModeText.text = $"Frighten for {frightenedTime - _frightenTimer:F2}s";
        }

        #endregion
    }
}
