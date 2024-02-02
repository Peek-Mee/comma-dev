using PMFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using USceneManager = UnityEngine.SceneManagement.SceneManager;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private string m_mainMenu;

    private void Awake()
    {
        StartCoroutine(WaitForSingleton());
    }
    IEnumerator WaitForSingleton()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => SingletonManager.m_Isntance.IsAllSetup);
        USceneManager.LoadScene(m_mainMenu);
    }
}
