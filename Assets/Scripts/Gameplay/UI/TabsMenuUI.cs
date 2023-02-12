using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Comma.Gameplay.UI
{
    [System.Serializable]
    public struct TabPair
    {
        [SerializeField] private Button _tabButton;
        [SerializeField] private GameObject _tabContent;
        [SerializeField] private GameObject _indicator;
                
        public Button Button => _tabButton;
        public GameObject Content => _tabContent;
        public GameObject Indicator => _indicator;
    }
    public class TabsMenuUI : MonoBehaviour
    {
         [Header("Initialization")]
        [SerializeField] private TabPair[] _tabs;
         
         private int _activeTabIndex;

         private void Start()
         {
             _tabs[0].Button.onClick.Invoke();
         }

         private void OnEnable()
         {
             for (int i = 0; i < _tabs.Length; i++)
             {
                 int index = i;
                 _tabs[i].Button.onClick.AddListener(() => OnTabButton(index));
             }
         }

         private void OnDisable()
         {
             foreach (TabPair tab in _tabs)
             {
                 tab.Button.onClick.RemoveAllListeners();
             }
         }
         
         private void OnTabButton(int index)
         {
            _tabs[_activeTabIndex].Content.SetActive(false);

             //_tabs[_activeTabIndex].Button.GetComponent<TMP_Text>().fontStyle = FontStyles.UpperCase;
            _tabs[_activeTabIndex].Indicator?.SetActive(false);
            _tabs[index].Indicator?.SetActive(true);


            _tabs[index].Content.SetActive(true);
             //_tabs[index].Button.GetComponent<TMP_Text>().fontStyle = FontStyles.Bold | FontStyles.UpperCase;
            _activeTabIndex = index;
         }
    }
}