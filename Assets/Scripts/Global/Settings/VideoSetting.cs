using System;
using Comma.Global.SaveLoad;
using UnityEngine;

namespace Comma.Global.Settings
{
    public class VideoSetting : MonoBehaviour
    {
        [SerializeField] private VideoSaveData _videoSaveData;
        [SerializeField] private VideoResolutionType _videoResolutionType;
        private VideoResolution _resolution;
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
            _isFullScreen= _videoSaveData.IsFullScreen();
            ChangeScreenResolution(_resolution, _videoSaveData.IsFullScreen());
        }
        public void ChangeDisplayResolution(VideoResolutionType type)
        {
            _resolution = type switch
            {
                VideoResolutionType.P480 => new VideoResolution(854, 480),
                VideoResolutionType.P720 => new VideoResolution(1280, 720),
                VideoResolutionType.P1080 => new VideoResolution(1920, 1080),
                VideoResolutionType.P1440 => new VideoResolution(2560, 1440),
                VideoResolutionType.P2160 => new VideoResolution(3840, 2160),
                _ => new VideoResolution(1920, 1080)
            };
            _videoResolutionType = type;
            ChangeScreenResolution(_resolution, _videoSaveData.IsFullScreen());
            // need set video resolution type data
        }
        private void ChangeScreenResolution(VideoResolution resolution, bool isFullScreen)
        {
            Screen.SetResolution(resolution.Width, resolution.Height, isFullScreen,60);
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
            print(SaveSystem.GetVideoSetting().GetDisplayResolution());
        }
        public void CancelVideoSetting()
        {
            InitVideoSetting();
        }
        public VideoResolution GetCurrentResolution()
        {
            return _resolution;
        }
        public VideoResolution GetDisplayResolution()
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