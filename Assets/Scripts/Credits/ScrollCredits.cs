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
        public float scrollSpeed = 10f;
        public float scrollDistance = 1000f;

        private void Start()
        {
            exitButton.onClick.AddListener(Exit);
            _initialPosition = text.transform.position;
        }

        private void Update()
        {
            text.transform.position += Vector3.up * (Time.deltaTime * scrollSpeed);

            if (Vector2.Distance(text.transform.position, _initialPosition) > scrollDistance)
                Exit();
        }

        private void Exit()
        {
            SceneManager.LoadScene(SceneNameConstants.TitleScreen);
        }
    }
}
