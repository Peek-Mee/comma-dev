using System;
using Comma.Global.SaveLoad;
using UnityEngine;

namespace Comma.Global.Settings
{
    public class VideoSetting : MonoBehaviour
    {
        private VideoSaveData _currentVideoSaveData;
        private VideoSaveData _newVideoSaveData;
        public static VideoSetting Instance { get; private set; }
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
            InitVideoSetting();
        }

        private void InitVideoSetting()
        {
            _currentVideoSaveData = SaveSystem.GetVideoSetting();
            _newVideoSaveData = (VideoSaveData) _currentVideoSaveData.Clone();
            
            var resolution = _currentVideoSaveData.GetDisplayResolution();
            var isFullScreen= _currentVideoSaveData.IsFullScreen();
            ChangeScreenResolution(resolution, isFullScreen);
            ChangeGameBrightness(_currentVideoSaveData.GetBrightness());
        }
        private void ChangeScreenResolution(VideoResolution resolution, bool isFullScreen)
        {
            Screen.SetResolution(resolution.Width, resolution.Height, isFullScreen,60);
        }
        public void ChangeDisplayResolution(VideoResolutionType type)
        {
            VideoResolution newResolution;
            newResolution = type switch
            {
                VideoResolutionType.P720 => new VideoResolution(1280, 720),
                VideoResolutionType.P768 => new VideoResolution(1366, 768),
                VideoResolutionType.P900 => new VideoResolution(1600, 900),
                VideoResolutionType.P1080 => new VideoResolution(1920, 1080),
                VideoResolutionType.P1440 => new VideoResolution(2560, 1440),
                _ => new VideoResolution(1920, 1080)
            };
            _newVideoSaveData.SetDisplayResolution(newResolution);
            _newVideoSaveData.SetResolutionType(type);
            
            var resolution = _newVideoSaveData.GetDisplayResolution();
            var isFullScreen = _newVideoSaveData.IsFullScreen();
            ChangeScreenResolution(resolution, isFullScreen);
        }
        public void ChangeFullScreen(bool isFullScreen)
        {
            _newVideoSaveData.SetFullScreen(isFullScreen);
            Screen.fullScreen = isFullScreen;
        }
        
        public void ChangeGameBrightness(float newValue)
        {
            _newVideoSaveData.SetBrightness(newValue);
            Screen.brightness = newValue;
        }
        public void AcceptVideoSetting()
        {
            _currentVideoSaveData = (VideoSaveData) _newVideoSaveData.Clone();
            SaveSystem.ChangeDataReference(_currentVideoSaveData);
            SaveSystem.SaveDataToDisk();
        }
        public void CancelVideoSetting()
        {
            InitVideoSetting();
        }
        public VideoResolution GetNewResolution()
        {
            return _newVideoSaveData.GetDisplayResolution();
        }
        public bool IsFullScreen()
        {
            return _currentVideoSaveData.IsFullScreen();
        }
        public VideoResolutionType GetVideoResolutionType()
        {
            return _newVideoSaveData.GetResolutionType();
        }
        public float GetGameBrightness()
        {
            return _newVideoSaveData.GetBrightness();
        }
    }
}