using System;
using Comma.Global.SaveLoad;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI
{
    public class VideoSettingUI : MonoBehaviour
    {
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
            // get data from video setting global
            _resolutionText.text = "x";
        }
        
        private void OnNextResolutionButton()
        {
            // enum++ from video setting global
            // enum get value = (VideoResolutionType)Enum.GetValues(typeof(VideoResolutionType)).Length - 1;
            // Change Resolution
            _resolutionText.text = " x ";
        }
        private void OnPreviousResolutionButton()
        {
            // enum-- from video setting global
            // enum get value = (VideoResolutionType)Enum.GetValues(typeof(VideoResolutionType)).Length - 1;
            // Change Resolution
            _resolutionText.text = " x ";
        }
        private void OnWindowedToggle()
        {
            _windowedToggle.onValueChanged.AddListener(isOn =>
            {
                // set full screen from video setting global
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