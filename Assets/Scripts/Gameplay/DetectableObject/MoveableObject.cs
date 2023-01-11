using System;
using System.Collections;
using Comma.Global.PubSub;
using UnityEngine;

namespace Comma.Gameplay.DetectableObject
{
    public class MoveableObject : MonoBehaviour, IDetectable
    {
        [SerializeField] private string _objectId;
        private bool _isInteracted;
        public string GetObjectId()
        {
            return _objectId;
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public void Interact()
        {
            _isInteracted = true;
            return;
        }

        public void UnInteract()
        {
            _isInteracted = false;
        }
        private float _horizontalUserInput;
        private Rigidbody2D _rigidbody2D;
        [SerializeField] private float _speed;
        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            EventConnector.Subscribe("OnPlayerMove", new(OnMoveInput));
        }

        private void FixedUpdate()
        {
            if (_isInteracted)
            {
                var vel = _rigidbody2D.velocity;
                vel.x = _horizontalUserInput * _speed;
                _rigidbody2D.velocity = vel;
            }
        }

        private void OnMoveInput(object message)
        {
            OnPlayerMove msg = (OnPlayerMove)message;
            print(msg.Direction);
            _horizontalUserInput = msg.Direction.x;
        }
    }
}