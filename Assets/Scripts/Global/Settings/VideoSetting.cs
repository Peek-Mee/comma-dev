using System;
using Comma.Global.SaveLoad;
using UnityEngine;

namespace Comma.Global.Settings
{
    public class VideoSetting : MonoBehaviour
    {
        [SerializeField] private VideoSaveData _videoSaveData;
        [SerializeField] private VideoResolutionType _videoResolutionType;
        private Resolution _resolution;
        private bool _isFullScreen;
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
            _videoSaveData = SaveSystem.GetVideoSetting();
            _resolution = _videoSaveData.GetDisplayResolution();
            ChangeScreenResolution(_resolution, _videoSaveData.IsFullScreen());
        }
        public void ChangeDisplayResolution(VideoResolutionType type)
        {
            switch (type)
            {
                case VideoResolutionType.P480:
                    _resolution = new Resolution { width = 480, height = 272, refreshRate = 60 };
                    break;
                
                case VideoResolutionType.P720:
                    _resolution = new Resolution { width = 720, height = 480, refreshRate = 60 };
                    break;
                
                case VideoResolutionType.P1080:
                    _resolution = new Resolution { width = 1080, height = 720, refreshRate = 60 };
                    break;
                
                case VideoResolutionType.P1920:
                    _resolution = new Resolution { width = 1920, height = 1080, refreshRate = 60 };
                    break;
                
                case VideoResolutionType.P2080:
                    _resolution = new Resolution { width = 2080, height = 1080, refreshRate = 60 };
                    break;
            }
            _videoResolutionType = type;
            ChangeScreenResolution(_resolution, _videoSaveData.IsFullScreen());
            // need set video resolution type data
        }
        private void ChangeScreenResolution(Resolution resolution, bool isFullScreen)
        {
            Screen.SetResolution(resolution.width, resolution.height, isFullScreen,60);
        }
        public void ChangeFullScreen(bool isFullScreen)
        {
            _isFullScreen = isFullScreen;
            Screen.fullScreen = isFullScreen;
        }
        public void AcceptVideoSetting()
        {
            _videoSaveData.SetDisplayResolution(_resolution);
            _videoSaveData.SetFullScreen(_isFullScreen);
            SaveSystem.SaveDataToDisk();
        }
        public void CancelVideoSetting()
        {
            InitVideoSetting();
        }
        public Resolution GetCurrentResolution()
        {
            return _resolution;
        }
        public Resolution GetDisplayResolution()
        {
            return _videoSaveData.GetDisplayResolution();
        }
        public bool IsFullScreen()
        {
            return _videoSaveData.IsFullScreen();
        }
        public VideoResolutionType GetVideoResolutionType()
        {
            // will get enum from save data
            return _videoResolutionType;
        }
    }
}