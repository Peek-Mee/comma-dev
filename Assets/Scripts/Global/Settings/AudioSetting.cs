using System;
using Comma.Global.AudioManager;
using Comma.Global.SaveLoad;
using UnityEngine;

namespace Comma.Global.Settings
{
    public class AudioSetting : MonoBehaviour
    {
        private float _masterVolume;
        private float _bgmVolume;
        private float _ambianceVolume;
        private float _sfxVolume;
        private bool _masterMute;
        private bool _bgmMute;
        private bool _ambianceMute;
        private bool _sfxMute;
        
        private AudioSaveData _audioSaveData;
        public static AudioSetting Instance { get; private set; }
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                Instance = this;
            }
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            InitAudioSetting();
        }
        
        private void InitAudioSetting()
        {
            _audioSaveData = SaveSystem.GetAudioSetting();

            _masterVolume = _audioSaveData.GetVolumeSetting(AudioDataType.MASTER);
            _bgmVolume = _audioSaveData.GetVolumeSetting(AudioDataType.BGM);
            _ambianceVolume = _audioSaveData.GetVolumeSetting(AudioDataType.AMBIANCE);
            _sfxVolume = _audioSaveData.GetVolumeSetting(AudioDataType.SFX);
            
            _masterMute = _audioSaveData.GetMuteSetting(AudioDataType.MASTER);
            _bgmMute = _audioSaveData.GetMuteSetting(AudioDataType.BGM);
            _ambianceMute = _audioSaveData.GetMuteSetting(AudioDataType.AMBIANCE);
            _sfxMute = _audioSaveData.GetMuteSetting(AudioDataType.SFX);
            
            foreach (AudioDataType type in AudioDataType.GetValues(typeof(AudioDataType)))
            {
                AudioController.UpdateAudioVolume(type,_audioSaveData.GetVolumeSetting(type));
                
                if (_audioSaveData.GetMuteSetting(type))
                    AudioController.MuteAudio(type);
                else
                    AudioController.UnmuteAudio(type);
            }
            
        }
        public void ChangeAudioVolume(AudioDataType type, float volume)
        {
            volume = type switch
            {
                AudioDataType.MASTER => _masterVolume = volume,
                AudioDataType.BGM => _bgmVolume = volume,
                AudioDataType.AMBIANCE => _ambianceVolume = volume,
                AudioDataType.SFX => _sfxVolume = volume,
                _ => 0f
            };
            AudioController.UpdateAudioVolume(type,volume);
        }
        public void ChangeMuteAudio(AudioDataType type, bool mute)
        {
            mute = type switch
            {
                AudioDataType.MASTER => _masterMute = mute,
                AudioDataType.BGM => _bgmMute = mute,
                AudioDataType.AMBIANCE => _ambianceMute = mute,
                AudioDataType.SFX => _sfxMute = mute,
                _ => false
            };
            AudioController.MuteAudio(type);
        }
        public void ChangeUnmuteAudio(AudioDataType type, bool unmute)
        {
            unmute = type switch
            {
                AudioDataType.MASTER => _masterMute = unmute,
                AudioDataType.BGM => _bgmMute = unmute,
                AudioDataType.AMBIANCE => _ambianceMute =!unmute,
                AudioDataType.SFX => _sfxMute = unmute,
                _ => false
            };
            AudioController.UnmuteAudio(type);
        }
        public void AcceptAudioSetting()
        {
            foreach (AudioDataType type in AudioDataType.GetValues(typeof(AudioDataType)))
            {
                _audioSaveData.ChangeAudioSetting(type,GetCurrentVolume(type)); //float
                _audioSaveData.ChangeAudioSetting(type,GetCurrentMute(type)); //bool
            }
        }
        public void CancelAudioSetting()
        {
            InitAudioSetting();
        }
        public float GetCurrentVolume(AudioDataType type)
        {
            float volume = type switch
            {
                AudioDataType.MASTER => _masterVolume,
                AudioDataType.BGM => _bgmVolume,
                AudioDataType.AMBIANCE => _ambianceVolume,
                AudioDataType.SFX => _sfxVolume,
                _ => 0f
            };
            return volume;
        }
        public bool GetCurrentMute(AudioDataType type)
        {
            bool mute = type switch
            {
                AudioDataType.MASTER => _masterMute,
                AudioDataType.BGM => _bgmMute,
                AudioDataType.AMBIANCE => _ambianceMute,
                AudioDataType.SFX => _sfxMute,
                _ => false
            };
            return mute;
        }
    }
}