using System;
using UnityEngine;

namespace Comma.Global.SaveLoad
{
    public enum AudioDataType { MASTER, BGM, AMBIANCE, SFX }

    [Serializable]
    public class AudioSaveData 
    {
        [SerializeField] private float _masterVolume;
        [SerializeField] private float _bgmVolume;
        [SerializeField] private float _ambianceVolume;
        [SerializeField] private float _sfxVolume;
        [SerializeField] private bool _masterMute;
        [SerializeField] private bool _bgmMute;
        [SerializeField] private bool _ambianceMute;
        [SerializeField] private bool _sfxMute;

        public AudioSaveData()
        {
            _masterMute = false;
            _bgmMute = false;
            _ambianceMute = false;
            _sfxMute = false;

            _masterVolume = 0f;
            _bgmVolume = 0f;
            _ambianceVolume = 0f;
            _sfxVolume = 0f;
        }
        /// <summary>
        /// Change audio volume setting and set as dirty
        /// </summary>
        /// <param name="type">AudioDataType</param>
        /// <param name="value">float</param>
        public void ChangeAudioSetting(AudioDataType type, float value)
        {
            if (value > 1 || value < 0) return;

            switch(type)
            {
                case AudioDataType.MASTER:
                    _masterVolume = value;
                    break;
                case AudioDataType.BGM:
                    _bgmVolume = value;
                    break;
                case AudioDataType.AMBIANCE:
                    _ambianceVolume = value;
                    break;
                case AudioDataType.SFX:
                    _sfxVolume = value;
                    break;
            }
        }
        /// <summary>
        /// Change audio mute setting and set as dirty
        /// </summary>
        /// <param name="type">AudioDataType</param>
        /// <param name="value">bool</param>
        public void ChangeAudioSetting(AudioDataType type, bool value)
        {
            switch (type)
            {
                case AudioDataType.MASTER:
                    _masterMute = value;
                    break;
                case AudioDataType.BGM:
                    _bgmMute = value;
                    break;
                case AudioDataType.AMBIANCE:
                    _ambianceMute = value;
                    break;
                case AudioDataType.SFX:
                    _sfxMute = value;
                    break;
            }
        }
        /// <summary>
        /// Get the volume level of a specific audio group type
        /// </summary>
        /// <param name="type">AudioDataType</param>
        /// <returns>float</returns>
        public float GetVolumeSetting(AudioDataType type)
        {
            float temp = type switch
            {
                AudioDataType.MASTER => _masterVolume,
                AudioDataType.BGM => _bgmVolume,
                AudioDataType.AMBIANCE => _ambianceVolume,
                AudioDataType.SFX => _sfxVolume,
                _ => 0f
            };
            return temp;
        }
        /// <summary>
        /// Get the mute setting of a specific audio group type
        /// </summary>
        /// <param name="type">AudioDataType</param>
        /// <returns>bool</returns>
        public bool GetMuteSetting(AudioDataType type)
        {
            bool temp = type switch
            {
                AudioDataType.MASTER => _masterMute,
                AudioDataType.BGM => _bgmMute,
                AudioDataType.AMBIANCE => _ambianceMute,
                AudioDataType.SFX => _sfxMute,
                _ => false
            };
            return temp;
        }

    }
}