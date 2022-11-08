using UnityEngine;

namespace GameHandler
{
    public class GameStartHandler : MonoBehaviour
    {
        private static GameStartHandler Instance { get; set; }

        private AudioSource _startAudioSource;

        // private AudioSource _audioSourcePacGum;
        // private AudioSource _audioSourcePacDeath;

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this);
            else
                Instance = this;

            // _audioSourcePacGum = GetComponent<AudioSource>();
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
