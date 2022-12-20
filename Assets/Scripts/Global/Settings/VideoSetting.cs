using System;
using Comma.Global.SaveLoad;
using UnityEngine;

namespace Comma.Global.Settings
{
    public class VideoSetting : MonoBehaviour
    {
        [SerializeField] private VideoSaveData _videoSaveData;
        [SerializeField] private VideoResolutionType _videoResolutionType;
        
        public static VideoSetting VideoSettingInstance;
        private static VideoSetting _videoSetting;
        private static VideoSetting Instance
        {
            get
            {
                _ = !_videoSetting ? _videoSetting = FindObjectOfType<VideoSetting>() : null;
                return _videoSetting;
            }
        }

        private void Awake()
        {
            if (VideoSettingInstance != null && VideoSettingInstance != this) Destroy(gameObject);
            else VideoSettingInstance = this;
            
            // Make this object persistent once it's instantiated
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            InitVideoSetting();
        }

        private void InitVideoSetting()
        {
            _videoSaveData = SaveSystem.GetVideoSetting();
            // need set video resolution type data
            ChangeScreenResolution();
            
        }
        private void ChangeScreenResolution()
        {
            Screen.SetResolution(_videoSaveData.GetDisplayResolution().width, _videoSaveData.GetDisplayResolution().height, !_videoSaveData.IsFullScreen());
        }
        public void ChangeDisplayResolution(VideoResolutionType type)
        {
            switch (type)
            {
                case VideoResolutionType.P480:
                    _videoSaveData.SetDisplayResolution(new Resolution()
                    {
                        width = 480,
                        height = 272,
                        refreshRate = 60
                    });
                    break;
                case VideoResolutionType.P720:
                    _videoSaveData.SetDisplayResolution(new Resolution()
                    {
                        width = 720,
                        height = 480,
                        refreshRate = 60
                    });
                    break;
                case VideoResolutionType.P1080:
                    _videoSaveData.SetDisplayResolution(new Resolution()
                    {
                        width = 1080,
                        height = 720,
                        refreshRate = 60
                    });
                    break;
                case VideoResolutionType.P1920:
                    _videoSaveData.SetDisplayResolution(new Resolution()
                    {
                        width = 1920,
                        height = 1080,
                        refreshRate = 60
                    });
                    break;
                case VideoResolutionType.P2080:
                    _videoSaveData.SetDisplayResolution(new Resolution()
                    {
                        width = 2560,
                        height = 1440,
                        refreshRate = 60
                    });
                    break;
            }
            ChangeScreenResolution();
            // need set video resolution type data
        }
        public VideoSaveData GetVideoSaveData()
        {
            return _videoSaveData;
        }
        public Resolution GetDisplayResolution()
        {
            return _videoSaveData.GetDisplayResolution();
        }
    }
}