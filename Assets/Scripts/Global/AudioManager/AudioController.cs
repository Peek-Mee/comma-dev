using Comma.Global.SaveLoad;
using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Comma.Global.AudioManager
{
    [Serializable]
    public struct MixerPair
    {
        [SerializeField] private AudioDataType _type;
        [SerializeField] private AudioMixerGroup _audioMixer;
        [SerializeField] private string _leakedVolumeVariable;

        public AudioDataType Type => _type;
        public AudioMixerGroup Mixer => _audioMixer;
        public string Variable => _leakedVolumeVariable;
        
    }

    public class AudioController : MonoBehaviour
    {
        [SerializeField] private MixerPair[] _mixerPairs;
        
        private static AudioController audioControllerInstance;
        private static AudioController _audioController;
        private static AudioController Instance
        {
            get
            {
                if (!_audioController)
                {
                    _audioController = FindObjectOfType<AudioController>();
                    if (_audioController == null)
                    {
                        GameObject temp = new("Audio Controller"); 
                        _audioController = temp.AddComponent<AudioController>();
                    }
                    _audioController.Init();
                }
                return _audioController;
            }
        }

        private void Awake()
        {
            if (audioControllerInstance != null && audioControllerInstance != this)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                audioControllerInstance = this;
                Init();
            }


            DontDestroyOnLoad(gameObject);
        }

        private void Init()
        {
            AudioSaveData _setting = SaveSystem.GetAudioSetting();
            
        }
        
        #region INSTANCE
        private float NormalizeAudioValue(float value)
        {
            return (value * 80f) - 80f;
        }

        private void UpdateMixerVolume(AudioDataType type, float volume)
        {
            if (volume > 1 || volume < 0) volume = NormalizeAudioValue(volume);

            MixerPair controller = Array.Find<MixerPair>(_mixerPairs, t => t.Type == type);
            controller.Mixer.audioMixer.SetFloat(controller.Variable, volume);

        }

        #endregion

        public static void UpdateAudioVolume(AudioDataType type, float volume)
        {
            float normalizedValue = Instance.NormalizeAudioValue(volume);
        }
    }
}