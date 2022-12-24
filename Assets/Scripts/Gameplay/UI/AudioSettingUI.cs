using Comma.Global.SaveLoad;
using Comma.Global.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace Comma.Gameplay.UI
{
    public class AudioSettingUI : MonoBehaviour
    {
        private AudioSetting _audioSetting;

        [Header("Music Setting")]
        [SerializeField] private Toggle _musicToggle;
        [SerializeField] private Slider _musicSlider;
        
        [Header("SFX Setting")]
        [SerializeField] private Toggle sfxToggle;
        [SerializeField] private Slider _sfxSlider;

        private void Awake()
        {
            _audioSetting = AudioSetting.Instance;
        }
        private void OnEnable()
        {
            RemoveAllListener();
            InitAudioSetting();
        }

        private void OnDisable()
        {
            RemoveAllListener();
        }
        
        private void InitAudioSetting()
        {
            _musicToggle.isOn = !_audioSetting.GetCurrentMute(AudioDataType.MASTER);
            _musicSlider.value = _audioSetting.GetCurrentVolume(AudioDataType.MASTER);
            sfxToggle.isOn = !_audioSetting.GetCurrentMute(AudioDataType.SFX);
            _sfxSlider.value = _audioSetting.GetCurrentVolume(AudioDataType.SFX);
            
            OnChangedSettings(AudioDataType.MASTER,_musicToggle,_musicSlider);
            OnChangedSettings(AudioDataType.SFX,sfxToggle,_sfxSlider);
        }

        private void OnChangedSettings(AudioDataType type,Toggle toggle, Slider slider)
        {
            toggle.onValueChanged.AddListener((isOn) =>
            {
                if(isOn)
                    _audioSetting.ChangeUnmuteAudio(type, !isOn);
                else
                    _audioSetting.ChangeMuteAudio(type, !isOn);
                slider.interactable = isOn;
            });
            
            slider.onValueChanged.AddListener((value) =>
            {
                _audioSetting.ChangeAudioVolume(type,value);
            });
        }
        private void RemoveAllListener()
        {
            _musicToggle.onValueChanged.RemoveAllListeners();
            _musicSlider.onValueChanged.RemoveAllListeners();
            sfxToggle.onValueChanged.RemoveAllListeners();
            _sfxSlider.onValueChanged.RemoveAllListeners();
        }
    }
}