﻿using System;
using Comma.Global.SaveLoad;
using Comma.Global.Settings;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI
{
    public class VideoSettingUI : MonoBehaviour
    {
        private VideoSetting _videoSetting;
        
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
        }

        private void OnDisable()
        {
            RemoveAllListeners();
        }
        private void InitVideoSettings()
        {
            _videoSetting = VideoSetting.Instance;
            _windowedToggle.isOn = !_videoSetting.IsFullScreen();
            _brightnessSlider.value = _videoSetting.GetGameBrightness();
            ChangeResolutionText();
            OnWindowedToggle();
            OnBrightnessSlider();
            _nextResolutionButton.onClick.AddListener(OnNextResolutionButton);
            _previousResolutionButton.onClick.AddListener(OnPreviousResolutionButton);
        }
        
        private void OnNextResolutionButton()
        {
            var videoResolutionType = _videoSetting.GetVideoResolutionType();
            videoResolutionType++;
            if (videoResolutionType > (VideoResolutionType)Enum.GetValues(typeof(VideoResolutionType)).Length - 1)
            {
                videoResolutionType = (VideoResolutionType)0;
            }
            _videoSetting.ChangeDisplayResolution(videoResolutionType);
            ChangeResolutionText();
        }
        private void OnPreviousResolutionButton()
        {
            var videoResolutionType = _videoSetting.GetVideoResolutionType();
            videoResolutionType--;
            if (videoResolutionType < 0)
            {
                videoResolutionType = (VideoResolutionType)Enum.GetValues(typeof(VideoResolutionType)).Length - 1;
            }
            _videoSetting.ChangeDisplayResolution(videoResolutionType);
            ChangeResolutionText();
        }
        private void ChangeResolutionText()
        {
            _resolutionText.text = _videoSetting.GetNewResolution().Width + " x " + _videoSetting.GetNewResolution().Height;
        }
        private void OnWindowedToggle()
        {
            _windowedToggle.onValueChanged.AddListener(isOn =>
            {
                _videoSetting.ChangeFullScreen(!isOn);
            });
        }
        private void OnBrightnessSlider()
        {
            _brightnessSlider.onValueChanged.AddListener(value =>
            {
                _videoSetting.ChangeGameBrightness(value);
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