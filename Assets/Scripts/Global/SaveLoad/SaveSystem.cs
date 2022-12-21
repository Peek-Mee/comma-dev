using UnityEngine;

namespace Comma.Global.SaveLoad
{
    public class SaveSystem : MonoBehaviour
    {
        private PlayerSaveData _playerData;
        private AudioSaveData _audioSetting;
        private VideoSaveData _videoSetting;
        private InputSaveData _inputSetting;

        public static SaveSystem saveInstance;
        private static SaveSystem _saveSystem;
        private static SaveSystem Instance
        {
            get
            {
                if (!_saveSystem)
                {
                    _saveSystem = FindObjectOfType<SaveSystem>();
                    if (_saveSystem == null)
                    {
                        GameObject temp = new("Save System");
                        _saveSystem = temp.AddComponent<SaveSystem>();
                    }
                    _saveSystem.Init();
                }
                
                return _saveSystem;
            }
        }

        private void Awake()
        {
            if (saveInstance != null && saveInstance != this) Destroy(gameObject);
            else saveInstance = this;

            // Make this object persistent once it's instantiated
            DontDestroyOnLoad(gameObject);
        }

        private void Init()
        {
            _playerData ??= new();
            _audioSetting ??= new();
            _videoSetting ??= new();
            _inputSetting ??= new();

            InitiateData<PlayerSaveData>(ref _playerData, "PlayerData");
            InitiateData<AudioSaveData>(ref _audioSetting, "AudioData");
            InitiateData<VideoSaveData>(ref _videoSetting, "VideoData");
            InitiateData<InputSaveData>(ref _inputSetting, "InputData");
        }

        private void InitiateData<T>(ref T data, string prefsName)
        {
            if(!PlayerPrefs.HasKey(prefsName))
            {
                print($"{prefsName} hasn't been saved");
                SaveData<T>(ref data, prefsName);
                return;
            }
            data = JsonUtility.FromJson<T>(PlayerPrefs.GetString(prefsName));
            print(PlayerPrefs.GetString(prefsName));
        }

        private void SaveData<T>(ref T data, string prefsName)
        {
            PlayerPrefs.SetString(prefsName, JsonUtility.ToJson(data));
            PlayerPrefs.Save();
        }

        private void SaveAllDataToDisk()
        {
            SaveData<PlayerSaveData>(ref _playerData, "PlayerData");
            SaveData<AudioSaveData>(ref _audioSetting, "AudioData");
            SaveData<VideoSaveData>(ref _videoSetting, "VideoData");
            SaveData<InputSaveData>(ref _inputSetting, "InputData");
        }

        /// <summary>
        /// Get saved player data as reference
        /// </summary>
        /// <returns>PlayerSaveData</returns>
        public static PlayerSaveData GetPlayerData()
        {
            return Instance._playerData;
        }
        /// <summary>
        /// Get saved audio settings data as reference
        /// </summary>
        /// <returns>AudioSaveData</returns>
        public static AudioSaveData GetAudioSetting()
        {
            return Instance._audioSetting;
        }
        /// <summary>
        /// Get saved video settings data as reference
        /// </summary>
        /// <returns>VideoSaveData</returns>
        public static VideoSaveData GetVideoSetting()
        {
            return Instance._videoSetting;
        }
        /// <summary>
        /// Get saved input key bindings data from the disk
        /// </summary>
        /// <returns>InptSaveData</returns>
        public static InputSaveData GetInputSetting()
        {
            return Instance._inputSetting;
        }
        /// <summary>
        /// Save all dirty data into disk
        /// </summary>
        public static bool SaveDataToDisk()
        {
            try
            {
                Instance.SaveAllDataToDisk();
                return true;
            }
            catch 
            {
                return false;
            }
        }
    }
}