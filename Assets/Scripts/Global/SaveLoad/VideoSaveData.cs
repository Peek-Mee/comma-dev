using System;
using UnityEngine;

namespace Comma.Global.SaveLoad
{
    public enum VideoResolutionType {P480, P720, P1080, P1920, P2080}
    [Serializable]
    public class VideoSaveData
    {
        [SerializeField] private Resolution _displayResolution;
        [SerializeField] private bool _fullScreen;

        // public VideoSaveData()
        // {
        //     Display mainDisplay = Display.main;
        //     _displayResolution = new()
        //     {
        //         height = mainDisplay.systemHeight,
        //         width = mainDisplay.systemWidth,
        //         refreshRate = 60
        //     };
        // }

        /// <summary>
        /// Get saved display resolution
        /// </summary>
        /// <returns>Resolution</returns>
        public Resolution GetDisplayResolution() { return _displayResolution; }
        /// <summary>
        /// Save a new display resolution and set data dirty
        /// </summary>
        /// <param name="value">Resolution</param>
        public void SetDisplayResolution(Resolution displayResolution) { _displayResolution = displayResolution; }
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
    }
}