using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveControl : MonoBehaviour
{
    public float dissolveAmount;
    public float dissolveSpeed;
    public bool isDissolving;
    [ColorUsage(true, true)]
    public Color fadeOutColor;
    [ColorUsage(true, true)]
    public Color fadeInColor;

    public Material mat;
    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
        print("Materials " + Resources.FindObjectsOfTypeAll(typeof(Material)).Length);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            isDissolving = true;
        if (Input.GetKeyDown(KeyCode.S))
            isDissolving = false;
        if (isDissolving)
            DissolveOut(dissolveSpeed, fadeOutColor);
        if (!isDissolving)
            DissolveIn(dissolveSpeed, fadeOutColor);

        mat.SetFloat("_DissolveAmount", dissolveAmount);
    }

    public void DissolveOut (float speed, Color color)
    {
        mat.SetColor("_OutlineColor", color);
        if (dissolveAmount > 0)
            dissolveAmount -= Time.deltaTime * speed;
    }

    public void DissolveIn(float speed, Color color)
    {
        mat.SetColor("_OutlineColor", color);
        if (dissolveAmount < 1)
            dissolveAmount += Time.deltaTime * speed;
    }
}
