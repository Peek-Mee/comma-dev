using UnityEngine;

namespace Comma.Gameplay.DetectableObject
{
    public class MoveableObject : MonoBehaviour, IMoveableObject
    {
        [SerializeField] private string _objectId;
        private bool _isInteracted;
        private Rigidbody2D _target;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
        }

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
            _rigidbody2D.constraints = RigidbodyConstraints2D.None ;
            _isInteracted = true;
        }
        public void UnInteract()
        {
            _rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
            _isInteracted = false;
            GetDetection(null, 0);
        }
        public void GetDetection(Rigidbody2D rigid, float dir)
        {
            _target = rigid;
            //_objectDirection = _distance * dir;
            //if (rigid == null) return;
            //var newPos = new Vector2(_target.transform.position.x - (-_objectDirection), transform.position.y);
            //transform.position = newPos;
        }

        private Rigidbody2D _rigidbody2D;
        [SerializeField] private float _distance;
        private float _objectDirection;

        private void FixedUpdate()
        {
            if (!_isInteracted) return;

            Vector2 newVel = new(_target.velocity.x, _rigidbody2D.velocity.y);
            _rigidbody2D.velocity = newVel;
        }
        
    }
}