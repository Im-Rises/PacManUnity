using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerLife : MonoBehaviour
    {
        private static readonly int IsDead = Animator.StringToHash(AnimationsConstants.PlayerIsDead);

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

        // Death audio
        private AudioSource _deathAudio;

        private void Start()
        {
            // Set respawn point
            _respawnPoint = transform.position;

            // Create hearts UI sprites
            GenerateHearts();

            // Get death audio
            _deathAudio = GetComponent<AudioSource>();
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

        public bool Kill()
        {
            _deathAudio.Play();
            life--;
            _hearts[life].SetActive(false);
            GetComponent<PlayerInput>().enabled = false;
            anim.SetBool(IsDead, true);
            Invoke(nameof(DisableGameObject), 2f);
            return life <= 0;
        }

        private void DisableGameObject()
        {
            gameObject.SetActive(false);
            // gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
