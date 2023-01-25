using System;
using UnityEngine;
using UnityEngine.UI;

namespace Comma.Home.UI
{
    public class WarningQuitUI : MonoBehaviour
    {
        [Header("Warning Quit Buttons")]
        [SerializeField] private Button _yesButton;
        [SerializeField] private Button _noButton;

        [Header("Panels")]
        [SerializeField] private GameObject _panel;
        private Animator _animator;
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
            //Application.Quit();
            _animator.SetTrigger("exit");
        }
        private void OnNoButton()
        {
            //gameObject.SetActive(false);
            _animator.SetTrigger("exit");
        }
        public void QuitApp()
        {
            Application.Quit();
        }
        public void BackToMenu()
        {
            _panel.SetActive(false);
        }
    }
}