using Comma.Global.SaveLoad;
using UnityEngine;

namespace Comma.Global.Settings
{
    public class VideoSetting : MonoBehaviour
    {
        [SerializeField] private VideoSaveData _videoSaveData;
        [SerializeField] private VideoResolutionType _videoResolutionType;
        
        private void ChangeResolution(VideoResolutionType type)
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
        }
    }
}