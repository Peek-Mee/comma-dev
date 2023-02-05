using Comma.Gameplay.Player;
using Comma.Global.SaveLoad;
using System.Collections;
using UnityEngine;

namespace Comma.Gameplay.Environment
{
    public class AutoSave : MonoBehaviour
    {
        [SerializeField] private float _timeAutoSave = 60f;
        [SerializeField] private Transform _playerToTrack;
        [SerializeField] private bool _isAutoSaveEnabled = true;

        private void Awake()
        {
            if (_playerToTrack != null) return;

            _playerToTrack = FindObjectOfType<PlayerMovement>()?.gameObject.transform;
        }
        private void Start()
        {
            if (!_isAutoSaveEnabled) return;
            StartCoroutine(RunAutoSave());
        }

        IEnumerator RunAutoSave()
        {
            yield return new WaitForSeconds(_timeAutoSave);
            SaveSystem.GetPlayerData().SetLastPosition(_playerToTrack.position);
            SaveSystem.SaveDataToDisk();
            StartCoroutine(RunAutoSave());
        }
    }
}
