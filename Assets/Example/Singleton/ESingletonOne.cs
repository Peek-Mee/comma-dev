
using PMFramework;
using UnityEngine;

public class ESingletonOne : Singleton<ESingletonOne>
{
    public void DoOne()
    {
        Debug.Log("Calling one!");
    }
}
