using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Player
{
    public class PlayerLife : MonoBehaviour
    {
        private static readonly int IsDead = Animator.StringToHash("isDead");
        [Header("Player Life")] public int life = 3;

        [Header("Animation")] public Animator anim;

        [Header("Text UI")] public TextMeshProUGUI gameOverText;

        [Header("Point of life")] public Sprite heartTexture;

        public Vector3 heartSpriteScale = new(1f, 1f);
        public GameObject heartsParent;
        public float heartOffset = 1.5f;
        private GameObject[] _hearts;
        private Vector2 _respawnPoint;


        private void Start()
        {
            _respawnPoint = transform.position;
            gameOverText.enabled = false;
            _hearts = new GameObject[life];

            for (var i = 0; i < life; i++)
            {
                var heart = new GameObject();
                heart.AddComponent<SpriteRenderer>().sprite = heartTexture;
                var position = heartsParent.transform.position;
                heart.transform.position =
                    new Vector2(position.x + i * heartOffset, position.y);
                heart.transform.localScale = heartSpriteScale;
                _hearts[i] = heart;
            }
        }


        private void Respawn()
        {
            GetComponent<PlayerController>().Destination = _respawnPoint +
                                                           GetComponent<PlayerController>().initPositionOffset *
                                                           GetComponent<PlayerController>().originalDirection;
            anim.SetBool(IsDead, false);
            transform.position = _respawnPoint;
            GetComponent<PlayerInput>().enabled = true;
            _hearts[life].SetActive(false);
        }

        public void Die()
        {
            life--;
            GetComponent<PlayerInput>().enabled = false;
            anim.SetBool(IsDead, true);
            if (life <= 0)
            {
                gameOverText.enabled = true;
                Invoke(nameof(ReloadScene), 4f);
            }
            else
            {
                Invoke(nameof(Respawn), 2f);
            }
        }

        public void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
