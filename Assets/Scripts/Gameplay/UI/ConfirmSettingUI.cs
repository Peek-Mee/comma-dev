using Comma.Global.SaveLoad;
using Comma.Global.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI
{
    public class ConfirmSettingUI : MonoBehaviour
    {
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
            SaveSystem.SaveDataToDisk();
            gameObject.SetActive(false);
        }
        private void OnNoButton()
        {
            VideoSetting.VideoSettingInstance.CancelVideoSetting();
            gameObject.SetActive(false);
        }
    }
}