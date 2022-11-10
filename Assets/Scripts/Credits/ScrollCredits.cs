using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Credits
{
    public class ScrollCredits : MonoBehaviour
    {
        public Button exitButton;
        public TextMeshProUGUI text;
        private Vector2 _initialPosition;
        public GameObject scrollEnd;

        public float scrollSpeed = 30f;
        // public float scrollDistance = 1000f;

        private void Start()
        {
            exitButton.onClick.AddListener(Exit);
            _initialPosition = text.transform.position;
        }

        private void Update()
        {
            text.transform.position += Vector3.up * (Time.deltaTime * scrollSpeed);

            if (Vector2.Distance(text.transform.position, _initialPosition) >= scrollEnd.transform.position.y)
                enabled = false;


            // if (Vector2.Distance(text.transform.position, _initialPosition) > scrollDistance)
            //     enabled = false;
        }

        private void Exit()
        {
            SceneManager.LoadScene(SceneNameConstants.TitleScreen);
        }
    }
}
