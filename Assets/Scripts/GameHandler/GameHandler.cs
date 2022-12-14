using System.Collections;
using Door;
using GamePauseUi;
using Ghosts;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace GameHandler
{
    public class GameHandler : MonoBehaviour
    {
        // Singleton
        public static GameHandler Instance { get; private set; }

        // Current Game Mode
        public GhostMode GameGhostsMode { get; private set; }

        // Ghosts
        private GhostAiMovement[] _ghosts;

        // Player
        private PlayerController _player;

        // Pac-gum
        private int _pacGumCount;

        // Timer for scatter and chase
        public uint[] ghostsModeTimes = { 7, 20, 7, 20, 5, 20, 5 };
        private int _ghostsModeTimesIndex;
        private float _switcherModeTimer;
        private bool _allTimersPaused;

        // Timer for ghost frightened
        public uint frightenedTime = 10;
        private float _frightenTimer;
        private bool _switcherModeTimerPaused;

        // Door
        public DoorHandler doorHandler;

        // Current Ghost mode text
        public TextMeshProUGUI currentModeText;

        // UI elements
        public TextMeshProUGUI gameOverText;
        public TextMeshProUGUI winText;
        public TextMeshProUGUI gamePausedText;

        public GameObject pauseMenuUi;

        public GamePauseUiHandler gamePauseUiHandler;

        public int GhostCountEaten { get; set; }


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
            _player = FindObjectOfType<PlayerController>();
            winText.enabled = false;
            gamePausedText.enabled = false;
            _pacGumCount = GameObject.FindGameObjectsWithTag(TagsConstants.PacGumTag).Length;
            UpdateGhostsMode();
            gamePauseUiHandler.Reset();
            pauseMenuUi.SetActive(false);
        }

        private void Update()
        {
            if (GameStartHandler.Instance.enabled || _allTimersPaused)
                return;

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
                if (_frightenTimer >= frightenedTime)
                {
                    _frightenTimer = 0;
                    _switcherModeTimerPaused = false;
                    doorHandler.CloseDoor();
                    UpdateGhostsMode();
                }

                PrintFrightenMode();
            }
        }

        #endregion

        #region Ghosts Mode

        private void UpdateGhostsMode()
        {
            if (_ghostsModeTimesIndex % 2 != 0)
                SwitchingChaseMode();
            else
                SwitchingScatterMode();

            if (!GameStartHandler.Instance.enabled)
            {
                MusicHandler.MusicHandler.Instance.StopMusic();
                MusicHandler.MusicHandler.Instance.PlayGhostChase();
            }
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
            GhostCountEaten = 0;

            // Not changing the game mode for eaten ghosts to be able to switch back to the normal current mode.
            if (_switcherModeTimerPaused)
                _frightenTimer = 0; // Reset the timer if the ghosts are already in frightened mode.

            _switcherModeTimerPaused = true;
            foreach (var ghost in _ghosts) ghost.SetGhostMode(GhostMode.Frightened);
            doorHandler.OpenDoor();

            MusicHandler.MusicHandler.Instance.StopMusic();
            MusicHandler.MusicHandler.Instance.PlayPacmanChase();
        }

        #endregion

        #region Print text for current mode

        private void PrintCurrentMode()
        {
            // If we are in the last mode, which is chase mode.
            // or if the last mode in the array is chase mode then we are in final chase mode.
            if (_ghostsModeTimesIndex >= ghostsModeTimes.Length ||
                (_ghostsModeTimesIndex == ghostsModeTimes.Length - 1 && GameGhostsMode == GhostMode.Chase))
            {
                currentModeText.text = "Chase";
                return;
            }

            // Print the current mode.
            currentModeText.text =
                $"{GameGhostsMode} for {ghostsModeTimes[_ghostsModeTimesIndex] - _switcherModeTimer:F2}s";
        }

        private void PrintFrightenMode()
        {
            currentModeText.text = $"Frighten for {frightenedTime - _frightenTimer:F2}s";
        }

        #endregion

        #region Reset functions

        public void KillPlayer()
        {
            // Stop music
            MusicHandler.MusicHandler.Instance.StopMusic();

            // Reset the player destination
            _player.Immobilize();

            // Reset the ghost mode.
            _allTimersPaused = true;

            // Deactivate the ghosts
            foreach (var ghost in _ghosts)
            {
                ghost.gameObject.SetActive(false);
                ghost.bodyRenderer.enabled = true;
            }

            // Exit frighten timer
            _switcherModeTimerPaused = false;

            // Decrease the lives and handle the player death.
            if (FindObjectOfType<PlayerLife>().Kill())
            {
                ScoreHandler.ScoreHandler.Instance.UpdateHighScore();
                gameOverText.enabled = true;

                // Restart the game after 3 seconds.
                Invoke(nameof(RestartGame), 3);
            }
            else // If the player still has lives.
            {
                Invoke(nameof(ResetGhostsAndPlayer), 3);
            }
        }

        private void ResetGhostsAndPlayer()
        {
            // Reset the ghosts
            foreach (var ghost in _ghosts)
            {
                ghost.gameObject.SetActive(true);
                ghost.Reset();
            }

            // Reset the player
            _player.enabled = true;
            _player.GetComponent<PlayerController>().enabled = true;
            _player.Reset();
            _player.gameObject.SetActive(true);

            // Play the music
            MusicHandler.MusicHandler.Instance.PlayGhostChase();

            // // Reset the ghost mode.
            _allTimersPaused = false;
        }

        public void ActivateGhostsAndPlayer()
        {
            foreach (var ghost in _ghosts)
            {
                ghost.bodyAnimator.enabled = true;
                ghost.enabled = true;
            }

            _player.enabled = true;
            _player.animator.enabled = true;

            doorHandler.CloseDoor();

            MusicHandler.MusicHandler.Instance.PlayGhostChase();
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            _allTimersPaused = false;
            TogglePause();
        }

        public void DecrementPacGumNumber()
        {
            _pacGumCount--;
            if (_pacGumCount <= 0) NextLevel();
        }

        private void NextLevel()
        {
            _allTimersPaused = true;

            winText.enabled = true;

            foreach (var ghost in _ghosts) ghost.gameObject.SetActive(false);

            _player.enabled = false;
            _player.animator.enabled = false;

            MusicHandler.MusicHandler.Instance.StopMusic();
            MusicHandler.MusicHandler.Instance.PlayIntermission();

            Invoke(nameof(RestartGame), 6);
        }

        public void TogglePause()
        {
            if (winText.enabled || gameOverText.enabled) return;

            if (gamePausedText.enabled)
            {
                ResumeGame();
                gamePauseUiHandler.Reset();
            }
            else
            {
                PauseGame();
            }
        }


        private void PauseGame()
        {
            Time.timeScale = 0;
            Time.fixedDeltaTime = 0;
            gamePausedText.enabled = true;
            pauseMenuUi.SetActive(true);
        }

        private void ResumeGame()
        {
            Time.timeScale = TimeConstants.TimeScaleNormal;
            Time.fixedDeltaTime = TimeConstants.FixedDeltaTime;
            gamePausedText.enabled = false;
            pauseMenuUi.SetActive(false);
        }

        public void GoToMainMenu()
        {
            Time.timeScale = TimeConstants.TimeScaleNormal;
            Time.fixedDeltaTime = TimeConstants.FixedDeltaTime;
            SceneManager.LoadScene(SceneNameConstants.TitleScreen);
        }

        #endregion
    }
}
