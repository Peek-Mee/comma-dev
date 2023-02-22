using Comma.Global.PubSub;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Comma.Gameplay.UI
{
    public class PauseUI : MonoBehaviour
    {
        [SerializeField] private GameObject _pausePanel;
        [SerializeField] private GameObject _pauseParentMenu;
        [Header("Pause Buttons")]
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _optionsButton;
        [SerializeField] private Button _menuButton;

        [Header("Pop Ups")]
        [SerializeField] private GameObject _optionsPopUp;
        [SerializeField] private GameObject _warningMenuPopUp;

        private void Start()
        {
            EventConnector.Subscribe("OnPauseInput", new(OnPause));
        }

        #region PubSub
        private void OnPause(object msg)
        {
            if (_optionsPopUp.activeSelf) return;
            _pausePanel.SetActive(!_pausePanel.activeSelf);
            EventConnector.Publish("OnGamePause", _pausePanel.activeSelf);
        }
        #endregion
        private void OnEnable()
        {
            _continueButton.onClick.AddListener(OnContinueButton);
            _optionsButton.onClick.AddListener(OnOptionsButton);
            _menuButton.onClick.AddListener(OnMenuButton);
        }

        private void OnDisable()
        {
            RemoveAllButtonListeners();
        }

        private void RemoveAllButtonListeners()
        {
            _continueButton.onClick.RemoveAllListeners();
            _optionsButton.onClick.RemoveAllListeners();
            _menuButton.onClick.RemoveAllListeners();
        }

        private void OnContinueButton()
        {
            _pausePanel.SetActive(false);
        }
        private void OnOptionsButton()
        {
            _pauseParentMenu.SetActive(false);
            _optionsPopUp.SetActive(true);
        }
        private void OnMenuButton()
        {
            _pauseParentMenu.SetActive(false);
            _warningMenuPopUp.SetActive(true);
        }
    }
}