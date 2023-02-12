using Cinemachine;
using Comma.Gameplay.Player;
using Comma.Global.SaveLoad;
using System.Collections;
using UnityEngine;

namespace Comma.Gameplay.Environment
{
    public class AutoSave : MonoBehaviour
    {
        [SerializeField] private float _timeAutoSave = 15f;
        [SerializeField] private Transform _playerToTrack;
        [SerializeField] private ProgressiveCamera _cameraToTrack;
        [SerializeField] private bool _enableAutoSave = true;

        private void Awake()
        {
            if (_playerToTrack == null)
            {
                _playerToTrack = FindObjectOfType<PlayerMovement>()?.gameObject.transform;
            }

            if (_cameraToTrack == null)
            {
                _cameraToTrack = FindObjectOfType<ProgressiveCamera>();
            }
        }
        private void Start()
        {
            if (!_enableAutoSave) return;
            StartCoroutine(RunAutoSave());
        }

        IEnumerator RunAutoSave()
        {
            yield return new WaitForSeconds(_timeAutoSave);
            PlayerSaveData saveData = SaveSystem.GetPlayerData();
            saveData.SetLastPosition(_playerToTrack.position);
            saveData.SetCameraScale(_cameraToTrack.GetCurrentScale());
            SaveSystem.SaveDataToDisk();
            print("Auto Save");
            StartCoroutine(RunAutoSave());
        }
    }
}
