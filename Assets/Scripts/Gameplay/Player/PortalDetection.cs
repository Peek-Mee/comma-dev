using Comma.Gameplay.DetectableObject;
using System.Collections;
using UnityEngine;

namespace Comma.Gameplay.Player
{
    [RequireComponent(typeof(Collider2D))]
    public class PortalDetection : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Portal"))
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    IDetectable coll = collision.gameObject.GetComponent<IDetectable>();
                    coll?.Interact();
                }
            }
        }
    }
}