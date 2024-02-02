using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PMFramework;

public class ESingletonThree : Singleton<ESingletonThree>
{
    public void DoThree()
    {
        Debug.Log("Calling three!");
    }
}
