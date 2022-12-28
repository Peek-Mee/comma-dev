using System;
using System.Net.NetworkInformation;
using Comma.Global.SaveLoad;
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
                if(_pausePanel.activeSelf)return; 
                _pausePanel.SetActive(true);
            }
        }
    }
}