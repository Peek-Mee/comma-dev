using System;
using Comma.Global.SaveLoad;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI
{
    public class VideoSettingUI : MonoBehaviour
    {
        [SerializeField] private VideoSaveData _videoSaveData;
        [SerializeField] private VideoResolutionType _videoResolutionType;
        private VideoResolutionType[] _resolutionTypes;
        private int _currentResolutionIndex;
        
        [Header("Resolution")]
        [SerializeField] private Button _nextResolutionButton;
        [SerializeField] private Button _previousResolutionButton;
        [SerializeField] private TMP_Text _resolutionText;
        
        [Header("Windowed")]
        [SerializeField] private Toggle _windowedToggle;
        
        [Header("Brightness")]
        [SerializeField] private Slider _brightnessSlider;
        
        private void OnEnable()
        {
            RemoveAllListeners();
            InitVideoSettings();
            OnWindowedToggle();
            _nextResolutionButton.onClick.AddListener(OnNextResolutionButton);
            _previousResolutionButton.onClick.AddListener(OnPreviousResolutionButton);
        }

        private void OnDisable()
        {
            RemoveAllListeners();
        }
        private void InitVideoSettings()
        {
            _videoSaveData = SaveSystem.GetVideoSetting();
            _resolutionTypes = (VideoResolutionType[]) Enum.GetValues(typeof(VideoResolutionType));
            _resolutionText.text = _videoSaveData.GetDisplayResolution().width + "x" + _videoSaveData.GetDisplayResolution().height;
        }
        
        private void OnNextResolutionButton()
        {
            _currentResolutionIndex++;
            if (_currentResolutionIndex >= _resolutionTypes.Length)
            {
                _currentResolutionIndex = 0;
            }
            ChangeResolution(_currentResolutionIndex);
            Screen.SetResolution(_videoSaveData.GetDisplayResolution().width, _videoSaveData.GetDisplayResolution().height,!_videoSaveData.IsFullScreen(), _videoSaveData.GetDisplayResolution().refreshRate);
            _resolutionText.text = _videoSaveData.GetDisplayResolution().width + " x " + _videoSaveData.GetDisplayResolution().height;
        }
        private void OnPreviousResolutionButton()
        {
            _currentResolutionIndex--;
            if (_currentResolutionIndex < 0)
            {
                _currentResolutionIndex = _resolutionTypes.Length - 1;
            }
            ChangeResolution(_currentResolutionIndex);
            Screen.SetResolution(_videoSaveData.GetDisplayResolution().width, _videoSaveData.GetDisplayResolution().height,!_videoSaveData.IsFullScreen(), _videoSaveData.GetDisplayResolution().refreshRate);
            _resolutionText.text = _videoSaveData.GetDisplayResolution().width + " x " + _videoSaveData.GetDisplayResolution().height;
        }
        private void ChangeResolution(int index)
        {
            switch (index)
            {
                case 0:
                    _videoSaveData.SetDisplayResolution(new Resolution()
                    {
                        width = 480,
                        height = 272,
                        refreshRate = 60
                    });
                    break;
                case 1:
                    _videoSaveData.SetDisplayResolution(new Resolution()
                    {
                        width = 720,
                        height = 480,
                        refreshRate = 60
                    });
                    break;
                case 2:
                    _videoSaveData.SetDisplayResolution(new Resolution()
                    {
                        width = 1080,
                        height = 720,
                        refreshRate = 60
                    });
                    break;
                case 3:
                    _videoSaveData.SetDisplayResolution(new Resolution()
                    {
                        width = 1920,
                        height = 1080,
                        refreshRate = 60
                    });
                    break;
                case 4:
                    _videoSaveData.SetDisplayResolution(new Resolution()
                    {
                        width = 2560,
                        height = 1440,
                        refreshRate = 60
                    });
                    break;
            }
        }
        private void OnWindowedToggle()
        {
            _windowedToggle.isOn = _videoSaveData.IsFullScreen();
            Screen.fullScreen = !_videoSaveData.IsFullScreen();
            
            _windowedToggle.onValueChanged.AddListener(isOn =>
            {
                _videoSaveData.SetFullScreen(isOn);
                Screen.fullScreen = !isOn;
            });
        }
        private void RemoveAllListeners()
        {
            _nextResolutionButton.onClick.RemoveAllListeners();
            _previousResolutionButton.onClick.RemoveAllListeners();
            _windowedToggle.onValueChanged.RemoveAllListeners();
            _brightnessSlider.onValueChanged.RemoveAllListeners();
        }
    }
}