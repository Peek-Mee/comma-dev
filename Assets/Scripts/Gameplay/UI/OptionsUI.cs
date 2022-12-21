using System;
using Comma.Global.SaveLoad;
using UnityEngine;
using UnityEngine.UI;

namespace Comma.Gameplay.UI
{
    public class OptionsUI : MonoBehaviour
    {
        [SerializeField] private GameObject _confirmSettingPopUp;
        
        [Header("Options Button")]
        [SerializeField] private Button _backButton;
        
        private void OnEnable()
        {
            RemoveAllButtonListeners();
            _backButton.onClick.AddListener(OnBackButton);
        }
        private void OnDisable()
        {
            RemoveAllButtonListeners();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CheckConfirmSetting();
            }
        }

        private void RemoveAllButtonListeners()
        {
            _backButton.onClick.RemoveAllListeners();
        }
        private void OnBackButton()
        {
            CheckConfirmSetting();
        }
        private void CheckConfirmSetting()
        {
            var isConfirm = SaveSystem.SaveDataToDisk();
            if (isConfirm)
            {
                _confirmSettingPopUp.SetActive(true);
            }
            else
            {
                _confirmSettingPopUp.SetActive(false);
                gameObject.SetActive(false);
            }
        }
    }
}