using System;
using System.Collections;
using Comma.Gameplay.Player;
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
            _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.None;
            _isInteracted = true;
        }
        public void UnInteract()
        {
            _rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            _isInteracted = false;
            GetDetection(null,0);
        }
        public void GetDetection(MoveableDetection detection, float dir)
        {
            _playerDetected = detection;
            _objectDirection = _distance * dir;
        }
        private MoveableDetection _playerDetected;
        private float _horizontalUserInput;
        private Rigidbody2D _rigidbody2D;
        [SerializeField] private float _distance;
        private float _objectDirection;
        [SerializeField] private float _speed;
        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            EventConnector.Subscribe("OnPlayerMove", new(OnMoveInput));
        }

        private void FixedUpdate()
        {
            if (_isInteracted && _playerDetected!= null)
            {
                //_rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.None;
                var objectPos = transform.position;
                var detectionPos = _playerDetected.transform.position;
                var newPos = new Vector2(detectionPos.x -(-_objectDirection), transform.position.y);

                if(Vector2.Distance(detectionPos,objectPos)>_distance)
                {
                    transform.position = newPos;
                }

                //var vel = _rigidbody2D.velocity;
                //vel.x = _horizontalUserInput * _speed;
                //_rigidbody2D.velocity = vel;
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