using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerLife : MonoBehaviour
    {
        private static readonly int IsDead = Animator.StringToHash(AnimationsConstants.PlayerIsDead);

        // UI elements
        public TextMeshProUGUI gameOverText;

        // Animations and sprites
        public Animator anim;
        public Sprite heartTexture;
        public Vector3 heartSpriteScale = new(1f, 1f);

        // Lives
        public int life = 3;

        // Hearts display properties
        public GameObject heartsParent;
        public float heartOffset = 1.5f;
        private GameObject[] _hearts;

        // Respawn coordinates
        private Vector2 _respawnPoint;


        private void Start()
        {
            // Set respawn point
            _respawnPoint = transform.position;

            // Disable game over text
            gameOverText.enabled = false;

            // Create hearts UI sprites
            GenerateHearts();
        }

        private void GenerateHearts()
        {
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
            // GetComponent<PlayerController>().Destination = _respawnPoint +
            //                                                GetComponent<PlayerController>().initPositionOffset *
            //                                                GetComponent<PlayerController>().originalDirection;
            // anim.SetBool(IsDead, false);
            // transform.position = _respawnPoint;
            // GetComponent<PlayerInput>().enabled = true;
            // _hearts[life].SetActive(false);
        }

        // private void DecreaseLife()
        // {
        //     life--;
        //     anim.SetBool(IsDead, false);
        //     _hearts[life].SetActive(false);
        // }

        public void Kill()
        {
            life--;
            anim.SetBool(IsDead, false);
            _hearts[life].SetActive(false);

            if (life <= 0)
            {
                gameOverText.enabled = true;
                // Invoke(nameof(ReloadScene), 4f);
            }
            else
            {
                // Invoke(nameof(Respawn), 2f);
            }
        }
    }
}
