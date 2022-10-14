using UnityEngine;

namespace AudioHandler
{
    public class AudioHandler : MonoBehaviour
    {
        private AudioSource _audioSourcePacGum;
        public static AudioHandler Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this);
            else
                Instance = this;

            _audioSourcePacGum = GetComponent<AudioSource>();
        }

        public void PlayAudioPacGumClip(AudioClip clip)
        {
            // _audioSourcePacGum.clip = clip;
            // if (_audioSourcePacGum.isPlaying)
            //     _audioSourcePacGum.loop = true;
            // _audioSourcePacGum.Play();
        }
    }
}
