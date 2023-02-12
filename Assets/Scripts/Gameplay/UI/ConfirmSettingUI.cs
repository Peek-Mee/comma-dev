using Comma.Global.SaveLoad;
using Comma.Global.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI
{
    public class ConfirmSettingUI : MonoBehaviour
    {
        [SerializeField] private GameObject _optionsPopUp;
        [SerializeField] private GameObject _pausePanel;
        [Header("Confirm Setting Buttons")]
        [SerializeField] private Button _yesButton;
        [SerializeField] private Button _noButton;
        
        private void OnEnable()
        {
            RemoveAllButtonListeners();
            _yesButton.onClick.AddListener(OnYesButton);
            _noButton.onClick.AddListener(OnNoButton);
        }

        private void OnDisable()
        {
            RemoveAllButtonListeners();
        }
        private void RemoveAllButtonListeners()
        {
            _yesButton.onClick.RemoveAllListeners();
            _noButton.onClick.RemoveAllListeners();
        }
        private void OnYesButton()
        {
            VideoSetting.Instance.AcceptVideoSetting();
            AudioSetting.Instance.AcceptAudioSetting();
            InputSetting.Instance.ApplyInputSetting();
            _pausePanel.SetActive(true);
            _optionsPopUp.SetActive(false);
            gameObject.SetActive(false);
        }
        private void OnNoButton()
        {
            VideoSetting.Instance.CancelVideoSetting();
            AudioSetting.Instance.CancelAudioSetting();
            InputSetting.Instance.CancelInputSetting();
            _pausePanel.SetActive(true);
            gameObject.SetActive(false);
            _optionsPopUp.SetActive(false);
        }
    }
}