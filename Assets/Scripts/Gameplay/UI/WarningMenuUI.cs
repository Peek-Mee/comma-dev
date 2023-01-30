using Comma.Global.AudioManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Comma.Gameplay.UI
{
    public class WarningMenuUI : MonoBehaviour
    {
        [Header("Warning Menu Buttons")]
        [SerializeField] private Button _yesButton;
        [SerializeField] private Button _noButton;
        
        [Header("Scene Management")]
        [SerializeField] private string _gameplaySceneName = "Gameplay";
        
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
            BGMController.Instance.StartCourotineGameplay();
            SceneManager.LoadScene(_gameplaySceneName);
        }
        private void OnNoButton()
        {
            gameObject.SetActive(false);
        }
    }
}