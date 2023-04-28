using UnityEngine;

public class PortalAnimation : MonoBehaviour
{
    private Material Portal;
    public float Dissolving;

    private void Start()
    {
        Portal.SetFloat("_DissolveAmmount", Dissolving);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            Dissolving += 20;
            Portal.SetFloat("_DissolveAmmount", Dissolving);
        }
    }
}
