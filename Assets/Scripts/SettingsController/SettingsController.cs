using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace SettingsController
{
    public class SettingsController : MonoBehaviour
    {
        public Slider mainVolumeSlider;
        public Slider musicVolumeSlider;
        public Slider soundVolumeSlider;

        public AudioMixer audioMixer;

        private void Start()
        {
            mainVolumeSlider.value = PlayerPrefs.GetFloat(AudioMixerConstants.Master, 0.5f);
            musicVolumeSlider.value = PlayerPrefs.GetFloat(AudioMixerConstants.Music, 0.5f);
            soundVolumeSlider.value = PlayerPrefs.GetFloat(AudioMixerConstants.Sound, 0.5f);
        }

        private void Update()
        {
            audioMixer.SetFloat(AudioMixerConstants.Master, Mathf.Log10(mainVolumeSlider.value) * 20);
            audioMixer.SetFloat(AudioMixerConstants.Music, Mathf.Log10(musicVolumeSlider.value) * 20);
            audioMixer.SetFloat(AudioMixerConstants.Sound, Mathf.Log10(soundVolumeSlider.value) * 20);
        }

        private void OnDestroy()
        {
            PlayerPrefs.SetFloat(AudioMixerConstants.Master, mainVolumeSlider.value);
            PlayerPrefs.SetFloat(AudioMixerConstants.Music, musicVolumeSlider.value);
            PlayerPrefs.SetFloat(AudioMixerConstants.Sound, soundVolumeSlider.value);
        }

        // private void OnApplicationQuit()
        // {
        //     PlayerPrefs.SetFloat(AudioMixerConstants.Master, mainVolumeSlider.value);
        //     PlayerPrefs.SetFloat(AudioMixerConstants.Music, musicVolumeSlider.value);
        //     PlayerPrefs.SetFloat(AudioMixerConstants.Sound, soundVolumeSlider.value);
        // }
    }
}
