using Comma.Global.PubSub;
using Comma.Global.SaveLoad;
using Comma.Utility.Collections;
using UnityEngine;

namespace Comma.Gameplay.DetectableObject
{
    public class MoveableObject : MonoBehaviour, IMoveableObject
    {
        [SerializeField] private string _objectId;
        private bool _isInteracted;
        private Rigidbody2D _target;
        private int _defLayer;
        private bool _initialize;
        private PhysicsMaterial2D _material;

        #region PubSub
        private void OnDisable()
        {
            EventConnector.Unsubscribe("OnGamePause", new(Dummy.VoidAction));
        }
        #endregion
        #region Pause
        private void OnGamePause(object message)
        {
            bool ctx = (bool)message;
            _rigidbody2D.simulated = !ctx;
        }
        #endregion

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _defLayer = gameObject.layer;
            _rigidbody2D.constraints = RigidbodyConstraints2D.None;
            _material = _rigidbody2D.sharedMaterial;
        }

        private void Start()
        {
            if (SaveSystem.GetPlayerData().IsObjectInteracted(_objectId))
            {
                transform.position = SaveSystem.GetPlayerData().GetObjectPosition(_objectId);
            }
            _initialize = true;
            EventConnector.Subscribe("OnGamePause", new(OnGamePause));
        }

        private void Update()
        {
            if (_initialize)
            {
                if (_rigidbody2D.velocity.y >= 0f)
                {
                    _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePosition;
                    _initialize = false;
                }
            }
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
            gameObject.layer = 0;
            _rigidbody2D.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
            _isInteracted = true;
        }
        public void UnInteract()
        {
            gameObject.layer = _defLayer;
            _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePosition;
            _rigidbody2D.sharedMaterial = _material;
            SaveSystem.GetPlayerData().SetInteractedObject(_objectId, transform.position);
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
            if (_target == null) return;
            _rigidbody2D.sharedMaterial = _target.sharedMaterial;
            Vector2 newVel = new(_target.velocity.x, _rigidbody2D.velocity.y);
            _rigidbody2D.velocity = newVel;
        }
        
    }
}