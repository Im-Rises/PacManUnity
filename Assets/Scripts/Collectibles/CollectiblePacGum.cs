using AudioHandler;
using UnityEngine;

namespace Collectibles
{
    public class CollectiblePacGum : MonoBehaviour
    {
        public int points = 10;

        private AudioSource _audioSource;
        private SpriteRenderer _spriteRenderer;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.gameObject.CompareTag(TagsConstants.PlayerTag)) return;
            ScoreHandler.ScoreHandler.Instance.AddScore(points);
            _spriteRenderer.enabled = false;
            if (CollectibleAudioHandler.Instance.PlaySound()) _audioSource.Play();
            GameHandler.GameHandler.Instance.DecrementPacGumNumber();
            Destroy(gameObject, 1f);
        }
    }
}
