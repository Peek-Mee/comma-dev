using Comma.Gameplay.DetectableObject;
using UnityEngine;

namespace Comma.Gameplay.Player
{
    [RequireComponent(typeof(Collider2D))]
    public class MoveableDetection : MonoBehaviour
    {
        private bool isHoldingObject = false;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Moveable"))
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (isHoldingObject)
                    {
                        isHoldingObject = false;
                    }
                    else
                    {
                        IDetectable coll = collision.gameObject.GetComponent<IDetectable>();
                        coll?.Interact();
                    }

                }
            }
        }
    }
}