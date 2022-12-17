using System;
using UnityEngine;

namespace Comma.Gameplay.UI
{
    public class GameplayUI : MonoBehaviour
    {
        [SerializeField] private GameObject _pausePanel;

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                _pausePanel.SetActive(!_pausePanel.activeSelf);
            }
        }
    }
}