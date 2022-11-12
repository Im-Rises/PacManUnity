using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LevelSelector
{
    public class LevelSelectorHandler : MonoBehaviour
    {
        // Buttons
        public Button backButton;

        public Button leftArrow;
        public Button rightArrow;


        // TextMeshPro
        public TextMeshProUGUI levelNameText;

        // Level settings
        public Level[] levels;

        [Serializable]
        public struct Level
        {
            public string name;
            public string sceneName;
            public GameObject levelImage;
        }


        // View settings
        public GameObject pivot;
        public float imageDistance = 100f;

        // Move settings
        private float _imageAngleOffset;
        private int _currentLevelIndex;

        private void Start()
        {
            backButton.onClick.AddListener(BackButtonPressed);

            leftArrow.onClick.AddListener(() => RotateLeft());
            rightArrow.onClick.AddListener(() => RotateRight());

            _imageAngleOffset = 360f / levels.Length;
            var pivotPosition = pivot.transform.position;

            for (var i = 0; i < levels.Length; i++)
            {
                // Set initial position and rotation
                levels[i].levelImage.transform.position = pivotPosition + new Vector3(0, 0, -imageDistance);
                levels[i].levelImage.transform.RotateAround(pivotPosition, Vector3.up, _imageAngleOffset * i);
            }

            levelNameText.text = levels[_currentLevelIndex].name;
        }

        private void BackButtonPressed()
        {
            SceneManager.LoadScene(SceneNameConstants.TitleScreen);
        }

        private void RotateLeft()
        {
            // get input
            pivot.transform.Rotate(0, _imageAngleOffset, 0);
            _currentLevelIndex--;
            if (_currentLevelIndex < 0) _currentLevelIndex = levels.Length - 1;
            levelNameText.text = levels[_currentLevelIndex].name;
        }

        private void RotateRight()
        {
            pivot.transform.Rotate(0, -_imageAngleOffset, 0);
            var target = levels[0].levelImage.transform.position;
            _currentLevelIndex = (_currentLevelIndex + 1) % levels.Length;
            levelNameText.text = levels[_currentLevelIndex].name;
        }

        private void OnMove(InputValue value)
        {
            var input = value.Get<Vector2>();
            if (input.x < 0)
                RotateLeft();
            else if (input.x > 0) RotateRight();
        }

        private void OnCancel()
        {
            BackButtonPressed();
        }

        private void OnSelect()
        {
            LoadLevel(levels[_currentLevelIndex].sceneName);
        }

        private void LoadLevel(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
