using UnityEngine;

namespace GameHandler
{
    public class GameStartHandler : MonoBehaviour
    {
        private static GameStartHandler Instance { get; set; }

        private AudioSource _startAudioSource;

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this);
            else
                Instance = this;
        }

        private void Start()
        {
            _startAudioSource = GetComponent<AudioSource>();
            _startAudioSource.Play();
        }

        private void Update()
        {
            if (_startAudioSource.isPlaying)
                return;

            GameHandler.Instance.ActivateGhostsAndPlayer();

            enabled = false;
        }
    }
}
