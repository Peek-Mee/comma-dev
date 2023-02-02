using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonFunction : MonoBehaviour
{

    [SerializeField] private GameObject Panel;
    [SerializeField] private GameObject Options;
    [SerializeField] private GameObject credits;
    [SerializeField] private GameObject transition;
    [SerializeField] private GameObject pause;

    [SerializeField] private GameObject exit;

    [SerializeField] private GameObject control;
    [SerializeField] private GameObject audio;
    [SerializeField] private GameObject video;

    [SerializeField] private string[] screenRes;
    [SerializeField] private GameObject Res;

    [SerializeField] private int index;

    [SerializeField] private Sprite[] onOffImages;

    [SerializeField] private GameObject[] windowedIndicator;


    public void continueGame()
    {
        transition.SetActive(true);
    }

    public void continueInGame()
    {
        pause.SetActive(false);
    }


    public void optionsOn()
    {
        Panel.SetActive(true);
        Options.SetActive(true);
        audio.SetActive(true);
        
    }

    public void optionsOff()
    {
        //Panel.SetActive(false);

        Options.GetComponent<Animator>().SetTrigger("exit");

        
    }

    public void exitOn()
    {
        Panel.SetActive(true);
        exit.SetActive(true);
    }

    public void exitOff()
    {
        exit.GetComponent<Animator>().SetTrigger("exit");
    }

    public void exitOffAnim()
    {
        //Panel.SetActive(false);
        Panel.GetComponent<Animator>().SetTrigger("exit");
        exit.SetActive(false);
    }

    public void optionOffAnim()
    {
        Options.SetActive(false);
        control.SetActive(false);
        video.SetActive(false);
        Panel.GetComponent<Animator>().SetTrigger("exit");
    }

    public void creditOn()
    {
        Panel.SetActive(true);
        credits.SetActive(true);
    }
    
    public void creditOff()
    {
        //Panel.SetActive(false);
        Panel.GetComponent<Animator>().SetTrigger("exit");
        credits.SetActive(false);
    }

    public void panelOff()
    {
        Panel.SetActive(false);
    }

    public void controlOn()
    {
        control.SetActive(true);
        audio.SetActive(false);
        video.SetActive(false);

    }

    public void audioOn()
    {
        control.SetActive(false);
        audio.SetActive(true);
        video.SetActive(false);

    }

    public void videoOn()
    {
        control.SetActive(false);
        audio.SetActive(false);
        video.SetActive(true);
    }

    public void WindowedIndicator()
    {
        if (windowedIndicator[0].GetComponent<Image>().sprite == onOffImages[0])
        {
            windowedIndicator[0].GetComponent<Image>().sprite = onOffImages[1];
        } else
        {
            windowedIndicator[0].GetComponent<Image>().sprite = onOffImages[0];
        }
    }

    public void musicIndicator()
    {
        if (windowedIndicator[1].GetComponent<Image>().sprite == onOffImages[0])
        {
            windowedIndicator[1].GetComponent<Image>().sprite = onOffImages[1];
        }
        else
        {
            windowedIndicator[1].GetComponent<Image>().sprite = onOffImages[0];
        }
    }

    public void sfxIndicator()
    {
        if (windowedIndicator[2].GetComponent<Image>().sprite == onOffImages[0])
        {
            windowedIndicator[2].GetComponent<Image>().sprite = onOffImages[1];
        }
        else
        {
            windowedIndicator[2].GetComponent<Image>().sprite = onOffImages[0];
        }
    }

    public void lowRes()
    {
        if (index != 0)
        {
            index = index - 1;
        }
        Res.GetComponent<TextMeshProUGUI>().text = screenRes[index];
    }

    public void upRes()
    {
        if (index != 2)
        {
            index = index + 1;
        }
        Res.GetComponent<TextMeshProUGUI>().text = screenRes[index];
    }
}
