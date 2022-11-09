using AudioHandler;
using UnityEngine;

namespace Collectibles
{
    public class CollectiblePacGum : MonoBehaviour
    {
        public int points = 10;

        private AudioSource _audioSource;
        private SpriteRenderer _spriteRenderer;
        private Collider2D _collider2D;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _collider2D = GetComponent<Collider2D>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.gameObject.CompareTag(TagsConstants.PlayerTag)) return;
            ScoreHandler.ScoreHandler.Instance.AddScore(points);
            _collider2D.enabled = false;
            _spriteRenderer.enabled = false;
            if (CollectibleAudioHandler.Instance.PlaySound()) _audioSource.Play();
            GameHandler.GameHandler.Instance.DecrementPacGumNumber();
            Destroy(gameObject, 0.5f);
        }
    }
}
