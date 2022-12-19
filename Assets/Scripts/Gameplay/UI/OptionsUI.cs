using System;
using UnityEngine;
using UnityEngine.UI;

namespace Comma.Gameplay.UI
{
    public class OptionsUI : MonoBehaviour
    {
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
        private void RemoveAllButtonListeners()
        {
            _backButton.onClick.RemoveAllListeners();
        }
        private void OnBackButton()
        {
            gameObject.SetActive(false);
        }
    }
}