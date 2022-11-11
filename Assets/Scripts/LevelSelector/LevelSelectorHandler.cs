using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LevelSelector
{
    public class LevelSelectorHandler : MonoBehaviour
    {
        public Button originalLevelButton;
        public Button customLevel1Button;
        public Button customLevel2Button;

        public Button leftArrow;
        public Button rightArrow;

        public GameObject[] levelImages;
        public GameObject pivot;

        private float _imageAngleOffset = 0f;
        public float imageDistance = 100f;

        private void Start()
        {
            originalLevelButton.onClick.AddListener(() => LoadLevel(SceneNameConstants.OriginalLevel));
            customLevel1Button.onClick.AddListener(() => LoadLevel(SceneNameConstants.CustomLevel1));
            customLevel2Button.onClick.AddListener(() => LoadLevel(SceneNameConstants.CustomLevel2));

            leftArrow.onClick.AddListener(() => RotateLeft());
            rightArrow.onClick.AddListener(() => RotateRight());

            _imageAngleOffset = 360f / levelImages.Length;
            var pivotPosition = pivot.transform.position;

            for (var i = 0; i < levelImages.Length; i++)
            {
                // Set initial position and rotation
                levelImages[i].transform.position = pivotPosition + new Vector3(0, 0, -imageDistance);
                levelImages[i].transform.RotateAround(pivotPosition, Vector3.up, _imageAngleOffset * i);
            }
        }

        // private void Update()
        // {
        //     if (Input.GetMouseButtonDown(0))
        //     {
        //         ClickLeft();
        //         Debug.Log("Left");
        //     }
        //     else if (Input.GetMouseButtonDown(1))
        //     {
        //         ClickRight();
        //     }
        // }

        private void RotateLeft()
        {
            // get input
            pivot.transform.Rotate(0, _imageAngleOffset, 0);
        }

        private void RotateRight()
        {
            pivot.transform.Rotate(0, -_imageAngleOffset, 0);
            // or
            // for (var i = 0; i < levelImages.Length; i++)
            // {
            //     levelImages[i].transform.RotateAround(pivot.transform.position, Vector3.up, _imageAngleOffset);
            // }
        }

        private void LoadLevel(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
