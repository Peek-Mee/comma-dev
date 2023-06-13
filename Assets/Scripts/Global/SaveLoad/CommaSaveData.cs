using System;
using UnityEngine;

namespace Comma.Global.SaveLoad
{
    [Serializable]
    public class CommaSaveData : ICloneable
    {
        [SerializeField]
        private int _version;
        [SerializeField]
        private PlayerSaveData _playerSave;
        [SerializeField]
        private InputSaveData _inputSave;
        [SerializeField]
        private AudioSaveData _audioSave;
        [SerializeField]
        private VideoSaveData _videoSave;

        public CommaSaveData(int version)
        {
            _version = version;
            _playerSave = new();
            _inputSave = new();
            _audioSave = new();
            _videoSave = new();
        }

        /// <summary>
        /// Update all save data at once
        /// </summary>
        /// <param name="playerSave"></param>
        /// <param name="inputSave"></param>
        /// <param name="audioSave"></param>
        /// <param name="videoSave"></param>
        /// <returns></returns>
        public bool UpdateGlobalSave(PlayerSaveData playerSave,
            InputSaveData inputSave, AudioSaveData audioSave, 
            VideoSaveData videoSave)
        {
            try
            {
                _playerSave = playerSave;
                _inputSave = inputSave;
                _audioSave = audioSave;
                _videoSave = videoSave;

                Debug.Log($"Save data updated successfully.");
                return true;
            }
            catch (Exception e)
            {
                Debug.Log($"Failed to Update Global Save. \n{e.Message}");
                return false;
            }
        }
        /// <summary>
        /// Only update player save data
        /// </summary>
        /// <param name="playerSave"></param>
        /// <returns></returns>
        public bool UpdatePlayerSave(PlayerSaveData playerSave)
        {
            try
            {
                _playerSave = playerSave;

                Debug.Log("Player save data updated successfully.");
                return true;
            }
            catch(Exception e)
            {
                Debug.Log($"Failed to Update Player Save. \n{e.Message}");
                return false;
            }
        }
        /// <summary>
        /// Only update input save data
        /// </summary>
        /// <param name="inputSave"></param>
        /// <returns></returns>
        public bool UpdateInputSave(InputSaveData inputSave)
        {
            try
            {
                _inputSave = inputSave;

                Debug.Log("Input save data updated successfully.");
                return true;
            }
            catch (Exception e)
            {
                Debug.Log($"Failed to Update Input Save. \n{e.Message}");
                return false;
            }
        }
        /// <summary>
        /// Only update Audio save data
        /// </summary>
        /// <param name="audioSave"></param>
        /// <returns></returns>
        public bool UpdateAudioSave(AudioSaveData audioSave)
        {
            try
            {
                _audioSave = audioSave;

                Debug.Log("Audio save data updated successfully.");
                return true;
            }
            catch (Exception e)
            {
                Debug.Log($"Failed to Update Audio Save. \n{e.Message}");
                return false;
            }
        }
        /// <summary>
        /// Only update Video save data
        /// </summary>
        /// <param name="videoSave"></param>
        /// <returns></returns>
        public bool UpdateVideoSave(VideoSaveData videoSave)
        {
            try
            {
                _videoSave = videoSave;

                Debug.Log("Player save data updated successfully.");
                return true;
            }
            catch (Exception e)
            {
                Debug.Log($"Failed to Update Player Save. \n{e.Message}");
                return false;
            }
        }
        /// <summary>
        /// Get the latest saved player save data
        /// </summary>
        /// <returns></returns>
        public PlayerSaveData GetPlayerSave()
        {
            return _playerSave;
        }
        /// <summary>
        /// Get the latest saved input save data
        /// </summary>
        /// <returns></returns>
        public InputSaveData GetInputSave() 
        { 
            return _inputSave;
        }
        /// <summary>
        /// Get the latest saved audio save data
        /// </summary>
        /// <returns></returns>
        public AudioSaveData GetAudioSave()
        {
            return _audioSave;
        }
        /// <summary>
        /// Get the latest saved video save data
        /// </summary>
        /// <returns></returns>
        public VideoSaveData GetVideoSave()
        {
            return _videoSave;
        }

        public object Clone()
        {
            return true;
        }
    }
}
