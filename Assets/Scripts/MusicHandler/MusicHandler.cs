using System;
using GameHandler;
using Ghosts;
using UnityEngine;

namespace MusicHandler
{
    public class MusicHandler : MonoBehaviour
    {
        public static MusicHandler Instance { get; private set; }

        private AudioSource _audioSource;
        public AudioClip pacmanChase;
        public AudioClip ghostChase;
        public AudioClip intermissionClip;

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

        public void PlayGhostChase()
        {
            if (_audioSource.isPlaying) return;
            _audioSource.clip = ghostChase;
            _audioSource.Play();
        }

        public void PlayPacmanChase()
        {
            if (_audioSource.isPlaying) return;
            _audioSource.clip = pacmanChase;
            _audioSource.Play();
        }

        public void StopMusic()
        {
            _audioSource.Stop();
        }

        public void TogglePause()
        {
            if (_audioSource.isPlaying)
                _audioSource.Pause();
            else
                _audioSource.UnPause();
        }

        public void PlayIntermission()
        {
            _audioSource.clip = intermissionClip;
            _audioSource.loop = false;
            _audioSource.Play();
        }
    }
}
