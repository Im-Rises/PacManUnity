using GameHandler;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LevelInputHandler
{
    public class LevelInputHandler : MonoBehaviour
    {
        public PlayerController playerController;
        public GamePauseUiHandler gamePauseUiHandler;

        private void OnCancel()
        {
            if (GameStartHandler.Instance.enabled)
                GameStartHandler.Instance.TogglePause();
            GameHandler.GameHandler.Instance.TogglePause();
            MusicHandler.MusicHandler.Instance.TogglePause();
        }

        private void OnMove(InputValue value)
        {
            Debug.Log("Move");
            var inputDirection = value.Get<Vector2>();

            if (inputDirection.x != 0) inputDirection.y = 0; // Create a priority for x movement

            if (inputDirection != Vector2.zero)
                inputDirection = inputDirection.normalized; // Normalize the output to be 1 or -1 not floating values

            if (gamePauseUiHandler.gameObject.activeSelf)
                gamePauseUiHandler.MoveSelection(inputDirection);
            else
                playerController.LastInputDirection = playerController.LastInputDirection;
        }
    }
}
