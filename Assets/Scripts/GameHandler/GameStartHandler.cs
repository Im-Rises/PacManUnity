using UnityEngine;

namespace GameHandler
{
    public class GameStartHandler : MonoBehaviour
    {
        public static GameStartHandler Instance { get; private set; }

        public AudioSource StartAudioSource { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this);
            else
                Instance = this;
        }

        private void Start()
        {
            StartAudioSource = GetComponent<AudioSource>();
            StartAudioSource.Play();
        }

        private void Update()
        {
            if (StartAudioSource.isPlaying)
                return;

            GameHandler.Instance.ActivateGhostsAndPlayer();

            enabled = false;
        }
    }
}
