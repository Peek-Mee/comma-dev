using Comma.Global.PubSub;
using Comma.Global.SaveLoad;
using Comma.Utility.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

namespace Comma.Gameplay.Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
    public class PEnhanceMovement : MonoBehaviour
    {
        [Header("Movement Variables")]
        [SerializeField] private float _walkSpd = 80.0f;
        [SerializeField] private float _runSpd = 120.0f;
        [SerializeField] private float _inAirSpd = 65.0f;
        [SerializeField] private float _jumpForce = 300.0f;
        [SerializeField] private float _gravityRatio = 1.0f;

        [Header("Collision Checkers")]
        [SerializeField] private LayerMask[] _groundLayers;
        [SerializeField]
        [Range(0.2f, 0.35f)] private float _checkRadius = 0.25f;
        [SerializeField]
        [Range(0.2f, 1.0f)] private float _checkExpensiveRadius = 0.52f;
        //[SerializeField] private GameObject _bottomChecker;
        [SerializeField] private Transform _normalBtmChecker;
        [SerializeField] private Transform _expensiveBtmChecker;

        #region ExternalBinding
        private Vector2 _movement;
        public bool InCutScene { get; set; } = false;
        /// <summary>
        /// Get character movement for animation purpose
        /// </summary>
        public Vector2 Movement => _movement;
        /// <summary>
        /// Get character bottom/feet position in Vector2
        /// </summary>
        public Vector2 BottomPosition => _normalBtmChecker.transform.position;
        /// <summary>
        /// Get Character's grounded state
        /// </summary>
        public bool IsGrounded => _isGrounded;
        #endregion

        #region Initialization
        // Physics
        private Rigidbody2D _rigidbody;
        private Collider2D _collider;
        private SpriteRenderer _sprite;
        private float _currentSpd;
        private Vector2 _platformVector;
        private List<int> _layerAfterConversion = new();
        // Inputs
        private float _horizontalInput = 0f;
        private bool _jumpInput = false;
        private bool _sprintInput = false;
        private bool _pauseInput = false;
        // Player States
        private bool _isWalking = false;
        private bool _isGrounded = false;
        private bool _wasGrounded = false;
        private bool _isFaceRight = true;
        private bool _isMoveAfterJump = false;
        private bool _hasFoundLayerForLand = false;
        int _layerDetectedBySwap;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
            _sprite = GetComponent<SpriteRenderer>();

            // Disable any simulation until preparations are all completed
            _rigidbody.Sleep();
        }
        private void Start()
        {
            InitLayerConversion();
            _layerDetectedBySwap = gameObject.layer;

            EventConnector.Subscribe("OnPlayerSwapDown", new(SwapCharacterDown));
            EventConnector.Subscribe("OnPlayerMove", new(OnMoveInput));
            EventConnector.Subscribe("OnPlayerJump", new(OnJumpInput));
            EventConnector.Subscribe("OnPlayerSprint", new(OnSprintInput));
            EventConnector.Subscribe("OnGamePause", new(OnGamePause));

            //InitSpawn();

            Physics2D.IgnoreLayerCollision(_layerAfterConversion[0],
                _layerAfterConversion[1], true);

            // Enable physics simulation
            _rigidbody.WakeUp();
        }
        private void OnDisable()
        {
            EventConnector.Unsubscribe("OnPlayerSwapDown", new(Dummy.VoidAction));
            EventConnector.Unsubscribe("OnPlayerMove", new(Dummy.VoidAction));
            EventConnector.Unsubscribe("OnPlayerJump", new(Dummy.VoidAction));
            EventConnector.Unsubscribe("OnPlayerSprint", new(Dummy.VoidAction));
            EventConnector.Unsubscribe("OnGamePause", new(Dummy.VoidAction));
        }
        #endregion

        #region PubSub
        // Receive input message for horizontal move [<int>]
        private void OnMoveInput(object message)
        {
            OnPlayerMove ctx = (OnPlayerMove)message;
            _horizontalInput = ctx.Direction.x;
        }
        // Receive input message for jump [<empty>]
        private void OnJumpInput(object message)
        {
            _jumpInput = true;
        }
        // Receive input message for sprint [<bool>]
        private void OnSprintInput(object message)
        {
            OnPlayerSprint ctx = (OnPlayerSprint)message;
            _sprintInput = ctx.Sprint;
        }
        // Receive input message for Pause Event [<bool>]
        private void OnGamePause(object message)
        {
            bool ctx = (bool)message;
            _pauseInput = ctx;
            _horizontalInput = 0;
            _jumpInput = false;
            _sprintInput = false;
            // Don't simulate physics on pause
            _rigidbody.simulated = !_pauseInput;
        }
        private void SwapCharacterDown(object message)
        {
            if (_isSwappingDown) return;
            StartCoroutine(JumpBeforeSwap());
        }
        #endregion

        #region Physics
        // Physics variables
        private bool _isBottomCollided = false;
        // Walk
        private void Walk()
        {
            if (_isWalking)
            {
                if (!_isGrounded)
                {
                    // If move in the air
                    _currentSpd = _inAirSpd;
                }
                else
                {
                    // Run or walk
                    _ = _sprintInput ? _currentSpd = _runSpd :
                        _currentSpd = _walkSpd;
                }
            }
        }
        private void Jump()
        {
            if (_jumpInput)
            {
                _jumpInput = false;
                if (_isGrounded)
                {
                    var force = new Vector2(_rigidbody.velocity.x, _jumpForce * 50.0f);
                    _rigidbody.AddForce(force);
                    //_isMoveAfterJump = true;
                }
            }
        }
        // Fall
        private void Fall()
        {
            if (_isGrounded)
            {
                //if (_isMoveAfterJump) _isMoveAfterJump = false;
                return;
            }
            _rigidbody.velocity += _gravityRatio * Time.deltaTime * Physics2D.gravity;
            // TO DO
        }
        // Move
        private void Move()
        {
            if (!_isWalking && _isGrounded && _hasLandedAfterSwap )
            {
                _rigidbody.velocity = Vector2.zero;
                return;
            }
            else if(_isWalking && !_isGrounded && _isMoveAfterJump)
            {
                _isMoveAfterJump = false;
            }
            else if (!_isWalking || !_isGrounded || !_hasLandedAfterSwap) return;

            Vector2 velocity = _rigidbody.velocity;
            velocity.x = _currentSpd * _horizontalInput;

            float lastDir = _isFaceRight ? 1 : -1;
            float xDir = velocity.x == 0 ? lastDir : Mathf.Sign(velocity.x);
            float slope = Vector2.Angle(_platformVector, Vector2.up);
            float mag = Mathf.Abs(velocity.x);
            float yDir = xDir == Mathf.Sign(_platformVector.x) ? -1 : 1;
            velocity.y = (!_isGrounded && velocity.y > 0) ? velocity.y :
                Mathf.Sin(slope * Mathf.Deg2Rad) * mag * yDir;
            velocity.x = Mathf.Cos(slope * Mathf.Deg2Rad) * mag * xDir;
            _rigidbody.velocity = velocity;
        }
        
        #endregion

        #region Appearance
        private void Flip()
        {
            if (_isFaceRight && _horizontalInput < 0)
            {
                _isFaceRight = !_isFaceRight;
            }
            else if (!_isFaceRight && _horizontalInput > 0)
            {
                _isFaceRight = !_isFaceRight;
            }

            // Flip if not facing right
            _sprite.flipX = !_isFaceRight;
        }
        #endregion

        #region Swap Mechanism
        //Variables
        private int _currentLayerIdx;
        private bool _isTimeToSwapEdge;
        // Check when first spawn
        private void InitLayerConversion()
        {
            for (int i = 0; i < _groundLayers.Length; i++)
            {
                _layerAfterConversion.Add(Converter.BitToLayer(_groundLayers[i]));
            }
        }
       
        // Swap Layer
        private void SwapLayer()
        {
            _ = _currentLayerIdx == 0 ? _currentLayerIdx = 1 : _currentLayerIdx = 0;

            // Remember Layer is different from LayerMask
            gameObject.layer = _layerAfterConversion[_currentLayerIdx];
        }
        private void SwapLayer(int layer)
        {
            int _currentLyr = _layerAfterConversion[_currentLayerIdx];
            int _altLayer = _layerAfterConversion[_currentLayerIdx == 0 ? 1 : 0];
            if (_currentLyr != layer)
            {
                // Swap layer if the target layer is exist
                if (_altLayer == layer)
                {
                    SwapLayer();
                }
                // Don't do anything if the layer doesn't exist
            }
        }
        private void EdgeSwapLayer()
        {
            RaycastHit2D hit;
            int layerIdxToCheck = _currentLayerIdx == 0 ? 1 : 0;
            hit = Physics2D.Raycast(_normalBtmChecker.position, Vector2.down, _checkRadius, _groundLayers[layerIdxToCheck]);
            if (hit)
            {
                if (hit.collider.isTrigger) return;
                gameObject.layer = _layerAfterConversion[layerIdxToCheck];
                _currentLayerIdx = layerIdxToCheck;
                _isTimeToSwapEdge = false;

            }
            
        }

        // Edge Detection
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Edge"))
            {
                //SwapLayer();
                _isTimeToSwapEdge = true;
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Edge"))
            {
                //SwapLayer();
                _isTimeToSwapEdge = false;
            }
        }
        #endregion

        #region Execution
        private void Update()
        {
            // set movement variable for animation purpose
            _movement = _rigidbody.velocity;

            // Don't process anything if is in cutscene
            if (InCutScene) return;

            // Check if player input horizontal move
            _isWalking = Mathf.Abs(_horizontalInput) > 0.01f;

            // Set was grounded
            _wasGrounded = _isGrounded;

            // Check player grounded
            _isGrounded = ThreeDetectionGround();


            //// Check the possibility of player stepping into 
            //// the opposite layer
            ThreeDetectionLayerGround();

            // Check player swap edge
            if (_isTimeToSwapEdge)
            {
                EdgeSwapLayer();
            }

        }
        private void FixedUpdate()
        {
            // Set was grounded
            _wasGrounded = _isGrounded;

            // Check player grounded
            _isGrounded = ThreeDetectionGround();


            //// Check the possibility of player stepping into 
            //// the opposite layer
            ThreeDetectionLayerGround();

            // Check and process sprite flip
            Flip();

            // Calculating character's horizontal move
            Walk();

            // Calculating character's vertical move
            Jump();
            Fall();

            // Actually move the character based on the calculation
            Move();
        }
        #endregion
        #region Gizmos
        private void OnDrawGizmos()
        {
            
            //Gizmos.DrawLine(_normalBtmChecker.position, _normalBtmChecker.position - (Vector3.up * 5.0f));

            foreach (GroundVariable ray in _detectors)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(ray.Position, ray.Position - (Vector3.up * ray.RayLength));
                Gizmos.color = Color.blue;
                Vector3 newPos = ray.Position + Vector3.right * .02f;
                Gizmos.DrawLine(newPos, newPos - (2.0f * ray.RayLength * Vector3.up));
            }
        }
        #endregion
        #region Layer Correction

        bool _isSwappingDown = false;
        
        public bool IsSwapDown => _isSwappingDown;
        
        private IEnumerator  JumpBeforeSwap()
        {
            if (!_isGrounded)
            {
                _isSwappingDown = true;
                StartCoroutine(SwapDownCharacter());
                yield return null;
            }
            else
            {
                // RND try to add a little jump for detail
                // Before swapping layer
                _isSwappingDown = true;
                var force = new Vector2((_isFaceRight ? 1 : -1) * _jumpForce * 15f, _jumpForce * 30.0f);
                _rigidbody.AddForce(force);
                yield return new WaitUntil(() => { return _isGrounded; });
                // Swap
                StartCoroutine(SwapDownCharacter());
            }
            
        }
        private IEnumerator SwapDownCharacter()
        {
            SwapLayer();
            yield return new WaitUntil(() => { return _isGrounded && !_wasGrounded; });
            _isSwappingDown = false;
        }
        #endregion

        #region 3-detection ground
        [Serializable]
        public struct GroundVariable
        {
            [SerializeField] private Transform _transform;
            [SerializeField] private float _rayLength;
            public Vector3 Position => _transform == null? Vector3.zero : _transform.position;
            public float RayLength => _rayLength;
        }
        [SerializeField] private GroundVariable[] _detectors;
        [SerializeField] private LayerMask _allGroundLayers;
        private bool _hasLandedAfterSwap = true;
        private bool ThreeDetectionGround()
        {
            RaycastHit2D hit;
            foreach(GroundVariable detector in _detectors)
            {
                hit = Physics2D.Raycast(detector.Position, Vector2.down, detector.RayLength, _groundLayers[_currentLayerIdx]);
                // in case not ground detected
                if (hit.collider == null) continue;
                // ignore trigger collider
                if (hit.collider.isTrigger) continue;
                _platformVector = hit.normal;
                return true;
            }

            _platformVector = Vector3.up;
            return false;
        }
        private void ThreeDetectionLayerGround()
        {
            if (_isSwappingDown) return;
            RaycastHit2D hit;
            foreach (GroundVariable detector in _detectors)
            {
                hit = Physics2D.Raycast(detector.Position, Vector2.down, detector.RayLength * 2.0f, _allGroundLayers);
                // in case not ground detected
                if (hit.collider == null) continue;
                // ignore trigger collider
                if (hit.collider.isTrigger) continue;
                int layer = hit.collider.gameObject.layer;
                // ignore if the same layer
                if (layer == gameObject.layer) continue;
                // ignore if inside collider
                if (Checker.IsWithin(hit.collider, detector.Position + Vector3.up * 0.02f)) continue;
                if (_movement.y > 0) continue;
                // swap layer
                _hasFoundLayerForLand = false;
                SwapLayer(layer);
                _wasGrounded = true;
                StartCoroutine(ChangingLayerNormal());
            }
        }
        IEnumerator ChangingLayerNormal()
        {
            yield return new WaitUntil(() => _isGrounded && !_wasGrounded);
            _hasLandedAfterSwap = true;
        }
        #endregion
    }
}