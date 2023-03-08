using Comma.Global.SaveLoad;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Comma.Home.UI
{
    public class WarningNewGameUI : MonoBehaviour
    {
        [Header("Warning New Game Buttons")]
        [SerializeField] private Button _yesButton;
        [SerializeField] private Button _noButton;
        [Header("Panels")]
        private Animator _animator;


        [Header("Scene Management")]
        [SerializeField] private string _gameplaySceneName = "Prolog";

        private void Awake()
        {
            _animator= GetComponent<Animator>();
        }
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
            SaveSystem.ResetPlayerData();
            SceneManager.LoadScene(_gameplaySceneName);
        }
        private void OnNoButton()
        {
            //gameObject.SetActive(false);
            _animator.SetTrigger("exit");
        }
        public void BackToMenu()
        {
            gameObject.SetActive(false);
        }
    }
}