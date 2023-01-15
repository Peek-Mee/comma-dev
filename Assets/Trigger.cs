using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Trigger : MonoBehaviour
{
    [SerializeField] private GameObject GC;
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

    public void TriggerCloseExit()
    {
        GC.GetComponent<ButtonFunction>().exitOffAnim();
    }

    public void panelOff()
    {
        GC.GetComponent<ButtonFunction>().panelOff();
    }

    public void optionOff()
    {
        GC.GetComponent<ButtonFunction>().optionOffAnim();
    }
}
