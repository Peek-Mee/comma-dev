using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace   Comma.Utility.Collections
{
    public class DebugLog : MonoBehaviour
    {
        [SerializeField] private bool _activateDebugger = true;
        [SerializeField] private TMP_Text _leftPanel;
        [SerializeField] private TMP_Text _righthPanel;
        [SerializeField] private bool _showFPS;
        [SerializeField] private GameObject[] _toDebug;

        private void Update()
        {
            // Reset Button
            if (Input.GetKeyDown(KeyCode.R))
            {
                string now = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene(now);
            }

            // Debugging
            _leftPanel.text = "";
            _righthPanel.text = "";
            if (!_activateDebugger) return;

            // FPS counter
            if(_showFPS)
            {
                int fps = (int)(1f / Time.unscaledDeltaTime);
                string color = fps > 60 ? "green" : fps > 30 ? "orange": "red";
                _leftPanel.text += $"<b><color=\"{color}\">{fps} FPS</b></color>\n";
            }

            // Debugging from modul
            int count = 0;
            foreach(GameObject go in _toDebug)
            {
                IDebugger data = go.GetComponent<IDebugger>();
                if (data == null) continue;


                switch (count%2 == 0)
                {
                    case true:
                        _leftPanel.text += data.ToDebug();
                        break;
                    case false:
                        _righthPanel.text += data.ToDebug();
                        break;
                }

                count++;
            }
        }
    }
}
