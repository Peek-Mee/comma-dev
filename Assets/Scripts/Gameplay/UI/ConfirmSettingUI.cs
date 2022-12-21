using Comma.Global.SaveLoad;
using Comma.Global.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI
{
    public class ConfirmSettingUI : MonoBehaviour
    {
        [SerializeField] private GameObject _optionsPopUp;
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
            gameObject.SetActive(false);
            _optionsPopUp.SetActive(false);
        }
        private void OnNoButton()
        {
            VideoSetting.Instance.CancelVideoSetting();
            gameObject.SetActive(false);
            _optionsPopUp.SetActive(false);
        }
    }
}