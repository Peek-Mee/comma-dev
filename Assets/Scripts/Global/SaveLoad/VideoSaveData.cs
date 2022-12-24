using System;
using UnityEngine;

namespace Comma.Global.SaveLoad
{
    [Serializable]
    public struct VideoResolution
    {
        [SerializeField] private int _width;
        [SerializeField] private int _height;
        [SerializeField] private int _refreshRate;

        public int Width { get { return _width; } private set { } }
        public int Height { get { return _height;} private set { } }
        public int RefreshRate { get { return _refreshRate;} private set { } }

        public VideoResolution(int width, int height, int refreshRate = 60)
        {
            _width = width;
            _height= height;
            _refreshRate = refreshRate;
        }
        
    }
    public enum VideoResolutionType {P480, P720, P1080, P1440, P2160}
    [Serializable]
    public class VideoSaveData : ICloneable
    {
        [SerializeField] private VideoResolution _displayResolution;
        [SerializeField] private bool _fullScreen;

        public VideoSaveData()
        {
            _displayResolution = new(1920, 1080, 60);
            _fullScreen = true;
        }

        /// <summary>
        /// Get saved display resolution
        /// </summary>
        /// <returns>Resolution</returns>
        public VideoResolution GetDisplayResolution() { return _displayResolution; }
        /// <summary>
        /// Save a new display resolution and set data dirty
        /// </summary>
        /// <param name="value">Resolution</param>
        public void SetDisplayResolution(VideoResolution displayResolution) { _displayResolution = displayResolution; }
        /// <summary>
        /// Is game in full screen?
        /// </summary>
        /// <returns>bool</returns>
        public bool IsFullScreen() { return _fullScreen; }
        /// <summary>
        /// Set new full screen preference
        /// </summary>
        /// <param name="value">bool</param>
        public void SetFullScreen(bool isFullScreen) { _fullScreen = isFullScreen; }

        public object Clone()
        {
            return (VideoSaveData)this.MemberwiseClone();
        }
    }
}