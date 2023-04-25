using Comma.Global.PubSub;
using Comma.Global.SaveLoad;
using Comma.Utility.Collections;
using System.Collections.Generic;
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
        private bool _isFaceRight = true;
        private bool _isMoveAfterJump = false;
        private bool _hasFoundLayerForLand = false;

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

            EventConnector.Subscribe("OnPlayerSwapDown", new(OnSwapDown));
            EventConnector.Subscribe("OnPlayerMove", new(OnMoveInput));
            EventConnector.Subscribe("OnPlayerJump", new(OnJumpInput));
            EventConnector.Subscribe("OnPlayerSprint", new(OnSprintInput));
            EventConnector.Subscribe("OnGamePause", new(OnGamePause));

            InitSpawn();

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
        // Receive input message for SwapDown Event [<empty>]
        private void OnSwapDown(object message)
        {
            SwapLayer();
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
                    _isMoveAfterJump = true;
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
            if (!_isWalking && _isGrounded)
            {
                _rigidbody.velocity = Vector2.zero;
                return;
            }
            else if(_isWalking && !_isGrounded && _isMoveAfterJump)
            {
                _isMoveAfterJump = false;
            }
            else if (!_isWalking || !_isGrounded) return;

            Vector2 velocity = _rigidbody.velocity;
            velocity.x = _currentSpd * _horizontalInput;

            float lastDir = _isFaceRight ? 1 : -1;
            float xDir = velocity.x == 0 ? lastDir : Mathf.Sign(velocity.x);
            float slope = Vector2.Angle(_platformVector, Vector2.up);
            float mag = Mathf.Abs(velocity.x);
            float yDir = xDir == Mathf.Sign(_platformVector.x) ? -1 : 1;
            //if (_isGrounded)
            //{
            //    //velocity.y = Mathf.Sin(slope * Mathf.Deg2Rad) * mag * yDir;
            //}
            velocity.y = (!_isGrounded && velocity.y > 0) ? velocity.y :
                Mathf.Sin(slope * Mathf.Deg2Rad) * mag * yDir;
            velocity.x = Mathf.Cos(slope * Mathf.Deg2Rad) * mag * xDir;
            _rigidbody.velocity = velocity;
        }
        // Ground Check
        private bool GroundCheck()
        {
            // Case 1: If everything goes smoothly (<45deg)
            // Only check from the middle of the character
            RaycastHit2D normalCase = Physics2D.Raycast(_normalBtmChecker.position,
                Vector2.down, _checkRadius, _groundLayers[_currentLayerIdx]);
            if (normalCase)
            {
                if (!normalCase.collider.isTrigger)
                {
                    _platformVector = normalCase.normal;
                    return true;
                }
            }

            // If grounded not detected but bottom is collided with something
            if (!_isBottomCollided) return false;

            // Case 2: If platform slope is more than 60 deg
            // or not reached by first raycast despite bottom collided
            // Extended raycast if platform >60deg
            RaycastHit2D normalExt = Physics2D.Raycast(_normalBtmChecker.position,
                Vector2.down * 2f, _checkRadius, _groundLayers[_currentLayerIdx]);
            if (normalExt)
            {
                if (!normalExt.collider.isTrigger)
                {
                    _platformVector = normalExt.normal;
                    return true;
                }
            }

            // Preventive Ground Checker
            // Case 3: If bottom is collided but no raycast can reach
            // from the normal position.
            // Note: This is the most expensive checker to avoid player
            // stuck on the edge. Thus, hopefully, player won't be come to
            // this state in the normal game run

            // Use Overlaps circle
            Collider2D[] overlaps = Physics2D.OverlapCircleAll(_expensiveBtmChecker.position,
                _checkExpensiveRadius, _groundLayers[_currentLayerIdx]);
            foreach (Collider2D coll in overlaps)
            {
                if (coll.CompareTag("Ground") && !coll.isTrigger)
                {
                    _platformVector = Vector2.up;
                    return true;
                }
            }

            _platformVector = Vector2.up;
            return false;
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            foreach (var contact in collision.contacts)
            {
                if (contact.collider.isTrigger) continue;
                //if (!contact.collider.CompareTag("Ground")) continue;
                var temp = GroundedFromContact(contact);
                if (temp)
                {
                    _isBottomCollided = true;
                    return;
                }
            }
        }
        private void OnCollisionStay2D(Collision2D collision)
        {
            if (_isBottomCollided) return;
            foreach (var contact in collision.contacts)
            {
                if (contact.collider.isTrigger) continue;
                //if (!contact.collider.CompareTag("Ground")) continue;
                var temp = GroundedFromContact(contact);
                if (temp)
                {
                    _isBottomCollided = true;
                    return;
                }
            }
        }
        private void OnCollisionExit2D(Collision2D collision)
        {
            _isBottomCollided = false;
        }

        private bool GroundedFromContact(ContactPoint2D contact)
        {
            float height = _collider.bounds.extents.y * 0.8f;
            bool condition = contact.point.y <= transform.position.y - height;
            return condition;
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
        private bool _wasPermitToSwap = false;
        private bool _isTimeToSwapEdge;
        // Check when first spawn
        private void InitLayerConversion()
        {
            for (int i = 0; i < _groundLayers.Length; i++)
            {
                _layerAfterConversion.Add(Converter.BitToLayer(_groundLayers[i]));
            }
        }
        private void InitSpawn()
        {
            //var save = SaveSystem.GetPlayerData();
            //if (!save.IsNewData())
            //{
            //    transform.position = save.GetLastPosition();
            //}

            RaycastHit2D hit;
            // Check for ground[n]
            for (int i = 0; i < _groundLayers.Length; i++)
            {
                // Remember Layer and LayerMask are different
                hit = Physics2D.Raycast(_normalBtmChecker.position, Vector2.down, 5.0f, _groundLayers[i]);
                if (hit)
                {
                    if (!hit.collider.isTrigger)
                    {
                        // If found a layer, then use this
                        gameObject.layer = hit.collider.gameObject.layer;
                        _currentLayerIdx = i;
                        _hasFoundLayerForLand = true;
                        return;
                    }
                }
            }

            // If can't find a ground
            //SwapLayer(save.GetCurrentLayer());
        }
        private void CheckPlayerSwap()
        {
            if (_isGrounded) _wasPermitToSwap = false;
            else
            {
                if (_rigidbody.velocity.y > 0)
                {
                    if (SwapLayerPermit() && _wasPermitToSwap)
                    {
                        _isTimeToSwapEdge = false;
                        SwapLayer();
                    }
                }
                _wasPermitToSwap = !SwapLayerPermit();
            }
        }
        // Can player swap layer?
        private bool SwapLayerPermit()
        {
            // Check the opposite layer
            int layerIdxToCheck;
            _ = _currentLayerIdx == 0 ? layerIdxToCheck = 1 :
                layerIdxToCheck = 0;
            RaycastHit2D ray = Physics2D.Raycast(_normalBtmChecker.position,
                Vector2.down, _checkRadius, _groundLayers[layerIdxToCheck]);

            if (ray.collider == null) return true;
            // This one to set _wasPermit 
            // The idea is to give false if the player
            // is still not above the opposite layer
            if (ray.collider.CompareTag("Ground") && !ray.collider.isTrigger)
            {
                return false;
            }
            return true;
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

            // Check player grounded
            _isGrounded = GroundCheck();

            // Check the possibility of player stepping into 
            // the opposite layer
            CheckPlayerSwap();

            // Check player swap edge
            if (_isTimeToSwapEdge)
            {
                EdgeSwapLayer();
            }

            // Check and process sprite flip
            Flip();

            // Calculating character's horizontal move
            Walk();

            // Calculating character's vertical move
            Jump();
            Fall();

        }
        private void FixedUpdate()
        {
            // Worst case if game save corrupted
            // Search any ground until found a solid one
            if (!_hasFoundLayerForLand)
            {
                InitSpawn();
            }
            // Actually move the character based on the calculation
            // on Walk, Jump, Fall
            Move();
        }
        #endregion
        #region Gizmos
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(_normalBtmChecker.position, _normalBtmChecker.position - (Vector3.up * 5.0f));
        }
        #endregion
    }
}