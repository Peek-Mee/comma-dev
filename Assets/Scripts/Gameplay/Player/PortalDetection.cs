using Comma.Gameplay.DetectableObject;
using Comma.Global.PubSub;
using UnityEngine;

namespace Comma.Gameplay.Player
{
    [RequireComponent(typeof(Collider2D))]
    public class PortalDetection : MonoBehaviour
    {
        private bool _isInPortalArea = false;
        private IDetectable _portal;

        private void Start()
        {
            EventConnector.Subscribe("OnPlayerUsePortal", new(OnPlayerUsePortal));
            EventConnector.Subscribe("OnPlayerInteract", new(OnInteractInput));
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Portal"))
            {
                _isInPortalArea = true;
                _portal = collision.GetComponent<IDetectable>();
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Portal"))
            {
                _isInPortalArea = false;
                _portal = null;
            }
        }

        private void OnPlayerUsePortal(object msg)
        {
            var message = (OnPlayerUsePortal)msg;
            transform.position = message.Destination;
        }
        private void OnInteractInput(object msg)
        {
            if (!_isInPortalArea || _portal == null) return;
            _portal.Interact();
        }
    }
}