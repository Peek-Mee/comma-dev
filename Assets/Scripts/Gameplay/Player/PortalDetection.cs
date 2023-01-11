using Comma.Gameplay.DetectableObject;
using Comma.Global.PubSub;
using Comma.Utility.Collections;
using UnityEngine;

namespace Comma.Gameplay.Player
{
    [RequireComponent(typeof(Collider2D))]
    public class PortalDetection : MonoBehaviour, IDebugger
    {
        private bool _isInPortalArea = false;
        private IDetectable _portal;
        private int _currentInstanceId;

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
                _currentInstanceId = collision.GetInstanceID();
            }
        }
        //private void OnTriggerStay2D(Collider2D collision)
        //{
        //    if (_portal != null) return;
        //    _portal = collision.GetComponentInParent<IDetectable>();
        //}
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Portal"))
            {
                if (collision.GetInstanceID() != _currentInstanceId) return;
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

        public string ToDebug()
        {
            string returner = "\n";
            returner += $"In Portal: <i>{_isInPortalArea}</i>\n";
            returner += $"Portal ID: <i>{_portal?.GetObjectId()}</i>\n" ;
            //returner += $"Portal ID: <i>{_portal?.GetObjectId()}</i>\n";

            return returner;
            
        }
    }
}