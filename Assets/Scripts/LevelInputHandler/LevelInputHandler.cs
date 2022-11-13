using GameHandler;
using GamePauseUi;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LevelInputHandler
{
    public class LevelInputHandler : MonoBehaviour
    {
        public PlayerController playerController;
        public GamePauseUiHandler gamePauseUiHandler;
        public SettingsController.SettingsController settingsController;

        private void OnCancel()
        {
            if (GameStartHandler.Instance.enabled)
                GameStartHandler.Instance.TogglePause();
            GameHandler.GameHandler.Instance.TogglePause();
            MusicHandler.MusicHandler.Instance.TogglePause();

            gamePauseUiHandler.ResetInitialButton();
            settingsController.ResetSliderSelection();
        }

        private void OnMove(InputValue value)
        {
            var inputDirection = value.Get<Vector2>();

            if (inputDirection.x != 0) inputDirection.y = 0; // Create a priority for x movement

            if (inputDirection != Vector2.zero)
                inputDirection = inputDirection.normalized; // Normalize the output to be 1 or -1 not floating values

            if (gamePauseUiHandler.gameObject.activeSelf)
                gamePauseUiHandler.MoveSelection(inputDirection);
            else if (settingsController.gameObject.activeSelf)
                settingsController.SetInputDirection(inputDirection);
            else
                playerController.LastInputDirection = inputDirection;
        }
    }
}
