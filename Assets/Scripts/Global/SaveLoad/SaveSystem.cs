using Comma.Utility.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Comma.Global.SaveLoad
{
    public class SaveSystem : MonoBehaviour
    {
        private PlayerSaveData _playerData;
        private AudioSaveData _audioSetting;
        private VideoSaveData _videoSetting;
        private InputSaveData _inputSetting;
        private CommaSaveData _globalSave;

        public static SaveSystem saveInstance;
        private static SaveSystem _saveSystem;
        //private bool _isNew;
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
            if (saveInstance != null && saveInstance != this)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                saveInstance = this;
                Init();
            }
            

            // Make this object persistent once it's instantiated
            DontDestroyOnLoad(gameObject);
        }

        private void CreateCommaGameData()
        {
            _globalSave ??= new(1);
            var temp = JsonUtility.ToJson(_globalSave);
            temp = SimpleEncryption.Encrypt(temp);
            BinaryFormatter bf = new();
            FileStream file = File.Create(Application.persistentDataPath + "/Comma.save");
            bf.Serialize(file, temp);
            file.Close();
            Debug.Log("New game data created!");
        }
        private void SaveCommaGameData()
        {
            var path = Application.persistentDataPath + "/Comma.save";

            if (File.Exists(path))
            {
                _globalSave.UpdatePlayerSave(_playerData);
                _globalSave.UpdateVideoSave(_videoSetting);
                _globalSave.UpdateAudioSave(_audioSetting);
                _globalSave.UpdateInputSave(_inputSetting);

                try
                {
                    var temp = JsonUtility.ToJson(_globalSave);
                    temp = SimpleEncryption.Encrypt(temp);
                    BinaryFormatter bf = new();
                    FileStream file = File.OpenWrite(Application.persistentDataPath + "/Comma.save");
                    bf.Serialize(file, temp);
                    file.Close();
                    Debug.Log("Data saved successfully!");
                }
                catch
                {
                    Debug.Log("There is an error occured when trying to save data");
                }
            }
            else
            {
                CreateCommaGameData();
                SaveCommaGameData();
            }
            
        }
        private void LoadCommaGameData()
        {
            var path = Application.persistentDataPath + "/Comma.save";
            //print(path);
            if (File.Exists(path))
            {
                try
                {
                    BinaryFormatter bf = new();
                    FileStream file = File.OpenRead(path);
                    var temp = JsonUtility.FromJson(SimpleEncryption.Decrypt(bf.Deserialize(file).ToString()), typeof(CommaSaveData));
                    _globalSave = (CommaSaveData)temp;
                    file.Close();
                    _playerData = _globalSave.GetPlayerSave();
                    _inputSetting = _globalSave.GetInputSave();
                    _audioSetting = _globalSave.GetAudioSave();
                    _videoSetting = _globalSave.GetVideoSave();

                    Debug.Log("Game data loaded successfully!");
                }
                catch
                {
                    CreateCommaGameData();
                    LoadCommaGameData();
                    Debug.Log("No valid save data is found! try");
                }

            }
            else
            {
                CreateCommaGameData();
                LoadCommaGameData();
                Debug.Log("No valid save data is found! else");
            }
        }

        private void Init()
        {
            // Old Save data
            ////_playerData ??= new();
            ////_audioSetting ??= new();
            ////_videoSetting ??= new();
            ////_inputSetting ??= new();

            //////if (!PlayerPrefs.HasKey("PlayerData")) _isNew = true;

            ////InitiateData<PlayerSaveData>(ref _playerData, "PlayerData");
            ////InitiateData<AudioSaveData>(ref _audioSetting, "AudioData");
            ////InitiateData<VideoSaveData>(ref _videoSetting, "VideoData");
            ////InitiateData<InputSaveData>(ref _inputSetting, "InputData");
            
            // New Save Data
            LoadCommaGameData();
        }

        //private void InitiateData<T>(ref T data, string prefsName) 
        //{
        //    if(!PlayerPrefs.HasKey(prefsName))
        //    {
        //        SaveData<T>(ref data, prefsName);
        //        return;
        //    }
        //    var testData = JsonUtility.FromJson<T>(PlayerPrefs.GetString(prefsName));
        //    if (testData != null)
        //    {
        //         data = testData;
        //    }
        //}

        //private void SaveData<T>(ref T data, string prefsName)
        //{
        //    PlayerPrefs.SetString(prefsName, JsonUtility.ToJson(data));
        //    PlayerPrefs.Save();
        //}

        private void ChangeDataAsObject<T>(ref T target, T newObject)
        {
            target = newObject;
        }

        private void SaveAllDataToDisk()
        {
            //SaveData<PlayerSaveData>(ref _playerData, "PlayerData");
            //SaveData<AudioSaveData>(ref _audioSetting, "AudioData");
            //SaveData<VideoSaveData>(ref _videoSetting, "VideoData");
            //SaveData<InputSaveData>(ref _inputSetting, "InputData");
            SaveCommaGameData();
        }

        /// <summary>
        /// Change save data value as a whole object by re-assign its reference
        /// </summary>
        /// <param name="newObject">PlayerSaveData</param>
        public static void ChangeDataReference(PlayerSaveData newObject)
        {
            Instance.ChangeDataAsObject<PlayerSaveData>(ref Instance._playerData, newObject);
        }
        /// <summary>
        /// Change save data value as a whole object by re-assign its reference
        /// </summary>
        /// <param name="newObject">AudioSaveData</param>
        public static void ChangeDataReference(AudioSaveData newObject)
        {

            Instance.ChangeDataAsObject<AudioSaveData>(ref Instance._audioSetting, newObject);
        }
        /// <summary>
        /// Change save data value as a whole object by re-assign its reference
        /// </summary>
        /// <param name="newObject">VideoSaveData</param>
        public static void ChangeDataReference(VideoSaveData newObject)
        {

            Instance.ChangeDataAsObject<VideoSaveData>(ref Instance._videoSetting, newObject);
        }
        /// <summary>
        /// Change save data value as a whole object by re-assign its reference
        /// </summary>
        /// <param name="newObject">InputSaveData</param>
        public static void ChangeDataReference(InputSaveData newObject)
        {

            Instance.ChangeDataAsObject<InputSaveData>(ref Instance._inputSetting, newObject);
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

        public static bool IsNewPlayer()
        {
           return Instance._playerData.IsNewData();
        }
        public static void ResetPlayerData()
        {
            //Instance._playerData = new();
            //PlayerPrefs.SetString("PlayerData", JsonUtility.ToJson(Instance._playerData));
            //PlayerPrefs.Save();
            //Instance.Init();
            Instance._playerData = new();
            Instance.SaveCommaGameData();
        }

    }
}