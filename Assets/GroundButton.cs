using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Comma.Gameplay.Environment
{
    public class GroundButton : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _activeButton;
        [SerializeField] private GameObject _trigger;
        
        private GroundTrigger _groundTrigger;
        private float _yActiveButton;

        public bool isShouldHold = true;
        public event System.Action<bool> OnGroundButtonPush;
        void Start()
        {
            _yActiveButton = _activeButton.transform.localPosition.y;
            _groundTrigger = _trigger.GetComponent<GroundTrigger>();
        }

        void Update()
        {
            //make active button stop float on default location
            if (_activeButton.transform.localPosition.y < _yActiveButton)
            {
                _activeButton.gravityScale = -1;
            }
            else
            {
                _activeButton.gravityScale = 0;
                _activeButton.velocity = Vector3.zero;
                _activeButton.angularVelocity = 0;
            }
            //Debug.Log(_activeButton.transform.localPosition.y+" compare "+_yActiveButton);
            //Debug.Log(_groundTrigger.isTriggered);

            //publish event if trigger is hit
            OnGroundButtonPush?.Invoke(_groundTrigger.isTriggered);
        }
    }

}
