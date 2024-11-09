using System.Collections;
using System.Collections.Generic;
using PMFramework;
using UnityEngine;

public class ESingletonTwo : Singleton<ESingletonTwo>
{
    public void DoTwo()
    {
        Debug.Log("Calling Two!");
    }
}
