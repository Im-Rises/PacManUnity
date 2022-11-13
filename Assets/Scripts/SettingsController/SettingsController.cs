using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace SettingsController
{
    public class SettingsController : MonoBehaviour
    {
        public Slider mainVolumeSlider;
        public Slider musicVolumeSlider;
        public Slider soundVolumeSlider;

        public AudioMixer audioMixer;

        private Vector2 _lastDirection;

        private int _currentSlider;

        private void Start()
        {
            mainVolumeSlider.value = PlayerPrefs.GetFloat(AudioMixerConstants.Master,
                PlayerPrefs.GetFloat(AudioMixerConstants.Master));
            musicVolumeSlider.value = PlayerPrefs.GetFloat(AudioMixerConstants.Music,
                PlayerPrefs.GetFloat(AudioMixerConstants.Music));
            soundVolumeSlider.value = PlayerPrefs.GetFloat(AudioMixerConstants.Sound,
                PlayerPrefs.GetFloat(AudioMixerConstants.Sound));

            ResetSliderSelection();
        }

        public void ResetSliderSelection()
        {
            _currentSlider = 0;
            SelectSlider(mainVolumeSlider);
            DeselectSlider(musicVolumeSlider);
            DeselectSlider(soundVolumeSlider);
        }

        public void SetInputDirection(Vector2 direction)
        {
            if (_lastDirection == direction || !gameObject.activeSelf) return;

            if (direction.y > 0)
                _currentSlider = _currentSlider == 0 ? 2 : _currentSlider - 1;
            else if (direction.y < 0) _currentSlider = _currentSlider == 2 ? 0 : _currentSlider + 1;

            switch (_currentSlider)
            {
                case 0:
                    SelectSlider(mainVolumeSlider);
                    DeselectSlider(musicVolumeSlider);
                    DeselectSlider(soundVolumeSlider);
                    break;
                case 1:
                    DeselectSlider(mainVolumeSlider);
                    SelectSlider(musicVolumeSlider);
                    DeselectSlider(soundVolumeSlider);
                    break;
                case 2:
                    DeselectSlider(mainVolumeSlider);
                    DeselectSlider(musicVolumeSlider);
                    SelectSlider(soundVolumeSlider);
                    break;
            }


            _lastDirection = direction;
        }

        private void Update()
        {
            audioMixer.SetFloat(AudioMixerConstants.Master, Mathf.Log10(mainVolumeSlider.value) * 20);
            audioMixer.SetFloat(AudioMixerConstants.Music, Mathf.Log10(musicVolumeSlider.value) * 20);
            audioMixer.SetFloat(AudioMixerConstants.Sound, Mathf.Log10(soundVolumeSlider.value) * 20);
        }

        private void SelectSlider(Slider slider)
        {
            slider.Select();
            slider.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        }

        private void DeselectSlider(Slider slider)
        {
            slider.transform.localScale = new Vector3(1f, 1f, 1f);
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
