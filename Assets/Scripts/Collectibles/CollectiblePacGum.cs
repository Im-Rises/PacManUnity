using UnityEngine;

namespace Collectibles
{
    public class CollectiblePacGum : MonoBehaviour
    {
        public int points = 10;
        private AudioSource _audioSource;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.gameObject.CompareTag(TagsConstants.PlayerTag)) return;
            ScoreHandler.ScoreHandler.Instance.AddScore(points);
            // AudioHandler.AudioHandler.Instance.PlayAudioPacGumClip(_audioSource.clip);
            // if (TryGetComponent(out SpriteRenderer sprite)) sprite.enabled = false;
            // Destroy(gameObject, 1f);
            Destroy(gameObject, 0f);
            GameHandler.GameHandler.Instance.DecrementPacGumNumber();
        }
    }
}
