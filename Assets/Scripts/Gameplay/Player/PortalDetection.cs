using Comma.Gameplay.DetectableObject;
using Comma.Global.PubSub;
using System.Collections;
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
            EventConnector.Subscribe("OnPlayerUsePortal", new(OnPlyerUsePortal));
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

        private void Update()
        {
            if (!_isInPortalArea) return;
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (_portal == null) return;
                _portal.Interact();
            }
        }
        private void OnPlyerUsePortal(object msg)
        {
            var message = (OnPlayerUsePortal)msg;
            transform.position = message.Destination;
        }
    }
}