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
        private float _yActiveButton, _initialyActiveButton;

        public bool isShouldHold = true;
        public event System.Action<bool> OnGroundButtonPush;
        void Start()
        {
            _initialyActiveButton = _activeButton.transform.localPosition.y;
            _yActiveButton = _activeButton.transform.localScale.y;
            _groundTrigger = _trigger.GetComponent<GroundTrigger>();
        }

        void Update()
        {
            //make active button stop float on default location
            if (_activeButton.transform.localPosition.y <= _initialyActiveButton - _yActiveButton/2)
            {
                if (!isShouldHold)
                {
                    _activeButton.constraints = RigidbodyConstraints2D.FreezeAll;
                }
            }
            
            OnGroundButtonPush?.Invoke(_groundTrigger.isTriggered);
        }
    }

}
