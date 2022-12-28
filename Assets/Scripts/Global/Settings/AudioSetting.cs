using Comma.Global.AudioManager;
using Comma.Global.SaveLoad;
using UnityEngine;

namespace Comma.Global.Settings
{
    public class AudioSetting : MonoBehaviour
    {
        private AudioSaveData _currentAudioSaveData;
        private AudioSaveData _newAudioSaveData;
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
            _currentAudioSaveData = SaveSystem.GetAudioSetting();
            _newAudioSaveData = (AudioSaveData)_currentAudioSaveData.Clone();
        }
        public void ChangeAudioVolume(AudioDataType type, float volume)
        {
            _newAudioSaveData.ChangeAudioSetting(type, volume);
            AudioController.UpdateAudioVolume(type, volume);
        }
        public void ChangeMuteAudio(AudioDataType type, bool mute)
        {
            _newAudioSaveData.ChangeAudioSetting(type, mute);
            AudioController.MuteAudio(type);
        }
        public void ChangeUnmuteAudio(AudioDataType type, bool unmute)
        {
            _newAudioSaveData.ChangeAudioSetting(type, unmute);
            AudioController.UnmuteAudio(type, false, _newAudioSaveData.GetVolumeSetting(type));
        }
        public void AcceptAudioSetting()
        {
            _currentAudioSaveData = (AudioSaveData)_newAudioSaveData.Clone();
            SaveSystem.ChangeDataReference(_currentAudioSaveData);
            SaveSystem.SaveDataToDisk();
        }
        public void CancelAudioSetting()
        {
            InitAudioSetting();
            
            foreach (AudioDataType type in AudioDataType.GetValues(typeof(AudioDataType)))
            {
                AudioController.UpdateAudioVolume(type,_currentAudioSaveData.GetVolumeSetting(type));
                
                if (_currentAudioSaveData.GetMuteSetting(type))
                    AudioController.MuteAudio(type);
                else
                    AudioController.UnmuteAudio(type, true);
            }
        }
        public float GetCurrentVolume(AudioDataType type)
        {
            return _currentAudioSaveData.GetVolumeSetting(type);
        }
        public bool GetCurrentMute(AudioDataType type)
        {
            return _currentAudioSaveData.GetMuteSetting(type);
        }
    }
}