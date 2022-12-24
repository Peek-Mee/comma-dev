using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Trigger : MonoBehaviour
{
    public void TriggerChangeScene()
    {
        if (SceneManager.GetActiveScene().name == "In Game Mockup")
        {
            SceneManager.LoadScene("Main - Menu Mockup");
        } else
        {
            SceneManager.LoadScene("In Game Mockup");
        }
        
    }
}
