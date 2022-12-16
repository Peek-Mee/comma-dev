using System;
using UnityEngine;
using UnityEngine.UI;

namespace Comma.Home.UI
{
    public class CreditsUI : MonoBehaviour
    {
        [Header("Credits Button")] 
        [SerializeField] private Button _backButton;

        private void OnEnable()
        {
            _backButton.onClick.RemoveAllListeners();
            _backButton.onClick.AddListener(OnBackButton);
        }

        private void OnDisable()
        {
            _backButton.onClick.RemoveAllListeners();
        }

        private void OnBackButton()
        {
            gameObject.SetActive(false);
        }
    }
}