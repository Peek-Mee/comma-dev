using PMFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomCaller : MonoBehaviour
{

    public void Start()
    {
        StartCoroutine(Call());
    }
    // Start is called before the first frame update
    IEnumerator Call()
    {
        yield return new WaitUntil(() => SingletonManager.m_Isntance.IsAllSetup);
        ESingletonOne.m_Instance.DoOne();
        ESingletonTwo.m_Instance.DoTwo();
        ESingletonThree.m_Instance.DoThree();
    }
}
