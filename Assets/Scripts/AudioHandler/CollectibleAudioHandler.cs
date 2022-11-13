using UnityEngine;

namespace AudioHandler
{
    public class CollectibleAudioHandler : MonoBehaviour
    {
        // Singleton
        public static CollectibleAudioHandler Instance { get; private set; }

        private AudioSource _audioSource;

        private float _timer;

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this);
            else
                Instance = this;
        }

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            _timer += Time.deltaTime;
        }

        public bool PlaySound()
        {
            if (_timer > 0.2f)
            {
                // _audioSource.Play();
                _timer = 0;
                return true;
            }

            return false;
            // if (!_audioSource.isPlaying)
            //     _audioSource.Play();
        }
    }
}
