using Comma.Global.PubSub;
using Comma.Global.SaveLoad;
using Comma.Utility.Collections;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Comma.Gameplay.Player
{
    public enum PlayerState
    {
        Idle,
        Walk,
        Run,
        Jump,
        Fall,
    }

    [RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
    public class PlayerMovement : MonoBehaviour, IDebugger
    {
        [Header("Collider Checks")]
        [SerializeField]
        [Range(0.2f, 0.35f)]
        private float _checkRadius = .25f;
        [SerializeField]
        private Transform _ground;
        [SerializeField]
        private Transform _ceil;
        [SerializeField]
        private Transform _rightHand;
        [SerializeField]
        private Transform _leftHand;
        [SerializeField]
        private LayerMask[] _groundLayers;
        private List<int> _layerValue;
        private int _currentLayer = 0;

        // Private
        private Rigidbody2D _rigidbody2D;
        private SpriteRenderer _playerSprite;
        private bool _isInCutScene = false;
        public bool InCutScene { get; set; }
        private bool _isFlipProhibited = false;
        public bool IsFlipProhibited
        {
            get { return _isFlipProhibited; }
            set { _isFlipProhibited = value; }
        }
        public bool IsJumpProhibited { get; set; }
        // #######

        [Header("Movement")]
        [SerializeField] private float _normalSpeed;
        [SerializeField] private float _runSpeed;
        private float _currentSpeed;
        private Vector2 _currentPlatformDegree;

        private bool isDashing = false;
        private bool _isFacingRight = true;
     
        
        [Header("Jump")]
        [SerializeField] private float _jumpForce;
        [SerializeField] private float _fallMultiplier;

        [Header("Animation")]
        [SerializeField] private PlayerAnimationController _playerAnimator;
        
        [Header("State")]
        private PlayerState _playerState;
        private bool _isGrounded;
        private bool _wasGrounded;
        private bool _isWalking;

        private void Awake()
        {
            _layerValue = new();
        }
        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _playerSprite = GetComponent<SpriteRenderer>();
            //init layer value
            foreach(var layer in _groundLayers)
            {
                _layerValue.Add(Converter.BitToLayer(layer));
            }

            // PubSub Area
            EventConnector.Subscribe("OnPlayerSwapDown", new(OnPlayerSwapDown));
            EventConnector.Subscribe("OnPlayerMove", new(OnMoveInput));
            EventConnector.Subscribe("OnPlayerJump", new(OnJumpInput));
            EventConnector.Subscribe("OnPlayerSprint", new(OnSprintInput));
        }

        #region PubSub
        private void OnPlayerSwapDown(object msg)
        {
            SwapLayer();
        }

        // User Input
        private void OnMoveInput(object message)
        {
            OnPlayerMove msg = (OnPlayerMove)message;
            _horizontalUserInput = msg.Direction.x;
            
        }
        private void OnJumpInput(object message)
        {
            _isPressJump = true;
        }
        private void OnSprintInput(object message)
        {
            OnPlayerSprint msg = (OnPlayerSprint)message;
            _isHoldSprint = msg.Sprint;

        }
        /////////////////////////////
        #endregion

        #region Movement From User Input
        private float _horizontalUserInput = 0f;
        public float GetInput
        {
            get { return _horizontalUserInput; }
            //set { _horizontalUserInput = value; }
        }
        private bool _isHoldSprint = false;
        private bool _isPressJump = false;

        #endregion

        private void Update()
        {
            if (_isInCutScene) return;
            if (!Physics2D.GetIgnoreLayerCollision(_layerValue[0], _layerValue[1]) || !Physics2D.GetIgnoreLayerCollision(_layerValue[1], _layerValue[0]))
            {
                Physics2D.IgnoreLayerCollision(_layerValue[0], _layerValue[1], true);
                Physics2D.IgnoreLayerCollision(_layerValue[1], _layerValue[0], true);
               
            }
            _isWalking = Mathf.Abs(_horizontalUserInput) > 0.1f;
            _isGrounded = IsGrounded();
            if (_isGrounded)
            {
                _playerAnimator.Move = _isWalking;
            }
            else
            {
                _playerAnimator.Move = false;
            }
            

            ChangePlayerState();
            OnWalk();
            OnFlip();
            OnJump();
            OnFall();
            _wasGrounded = _isGrounded;
        }

        private void FixedUpdate()
        {
            OnMove();
        }
        private void ChangePlayerState()
        {
            if (_isGrounded)
            {
                if (_isWalking)
                {
                    _playerState = PlayerState.Walk;
                }
                else
                {
                    _playerState = PlayerState.Idle;
                }
                _wasEligible = false;
            }
            else
            {
                if (_rigidbody2D.velocity.y > 0)
                {
                    _playerState = PlayerState.Jump; 
                    if (EligibleToSwapLayer() && _wasEligible)
                    {
                        SwapLayer();
                    }
                }
                else
                {
                    _playerState = PlayerState.Fall;
                }

                _wasEligible = !EligibleToSwapLayer();
            }
        }

        private bool _isAbleToMoveAfterJump = false;
        private void OnMove()
        {
            if ((!_isWalking && _isGrounded) || _playerAnimator.StartWalk || _playerAnimator.StartRun)
            {
                _rigidbody2D.velocity= Vector2.zero;
                return;
            }
            else if (_isWalking && !_isGrounded && _isAbleToMoveAfterJump)
            {
                _isAbleToMoveAfterJump= false;
            }
            else if (!_isWalking || !_isGrounded ) return;
            var vel = _rigidbody2D.velocity;
            vel.x = _currentSpeed * _horizontalUserInput;
            _ = _isGrounded ? vel.x *= .5f : vel.x *= 1f;

            float lastDirection = _isFacingRight ? 1 : -1;
            float xDirection = vel.x == 0 ? lastDirection : Mathf.Sign(vel.x);
            float slopeAngle = Vector2.Angle(_currentPlatformDegree, Vector2.up);
            float magnitude = Mathf.Abs(vel.x);
            float yDirection = xDirection == Mathf.Sign(_currentPlatformDegree.x) ? -1 : 1;
            vel.y = _playerState == PlayerState.Jump ? 
                vel.y : Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * magnitude * yDirection;
            vel.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * magnitude * xDirection;


            _rigidbody2D.velocity = vel;
           
        }

        private void OnWalk()
        {
            if (_playerAnimator.PortalInteract) return;
            if (_isWalking)
            {
                if (_isHoldSprint && !(_playerAnimator.Pull || _playerAnimator.Push))
                {
                    if (_playerAnimator.Idle)
                    {
                        _playerAnimator.StartRun = true;
                    }
                    _playerAnimator.Idle = false;
                    _playerAnimator.XSpeed = 2f;
                    _ = _playerState == PlayerState.Jump || _playerState == PlayerState.Fall ?
                        _currentSpeed = _normalSpeed * 0.5f : _currentSpeed = _runSpeed;
                }
                else
                {
                    if (_horizontalUserInput != 0 && _playerAnimator.Idle && !_playerAnimator.Pull && !_playerAnimator.Push)
                    {
                        _playerAnimator.StartWalk = true;
                    }
                    _playerAnimator.Idle = false;
                    _playerAnimator.XSpeed = 1f;
                    _ = _playerState == PlayerState.Jump || _playerState == PlayerState.Fall ?
                        _currentSpeed = _normalSpeed * 0.5f : _currentSpeed = _normalSpeed;
                }
            }
            else
            {
                _currentSpeed = _normalSpeed;
                _playerAnimator.Idle = _isGrounded;
                _playerAnimator.XSpeed = 0f;
            }
        }

        private void OnJump()
        {
            if (_playerAnimator.EndFall == false)
            {
                if (_playerAnimator.StartJump == true) _playerAnimator.YSpeed = 0f;
                else _playerAnimator.YSpeed = .5f;
            }

            if (_isPressJump)
            {
                _isPressJump = false;
                if (IsJumpProhibited) return;
                if (_playerAnimator.PortalInteract) return;
                if (_playerAnimator.Pull || _playerAnimator.Push) return;
                if (_isGrounded)
                {
                    _isAbleToMoveAfterJump = true;
                    _rigidbody2D.AddForce(new Vector2(_rigidbody2D.velocity.x, _jumpForce * 50f));
                    _playerAnimator.Idle = false;
                    _playerAnimator.StartJump = true;
                    _playerAnimator.EndFall= false;
                }
            }

        }

        [SerializeField] private float _rangeNearGround = 3f;
        private void OnFall()
        {
            if (_wasGrounded) return;
            Vector2 gravity = -Physics2D.gravity;
            _rigidbody2D.velocity -= _fallMultiplier * Time.deltaTime * gravity;


            if (_playerAnimator.YSpeed == .5f)
            {
                RaycastHit2D nearGround = IsCollide(_ground.position, Vector2.down, _rangeNearGround);
                if (nearGround && !_wasGrounded)
                {
                    _playerAnimator.YSpeed = 1f;
                    _playerAnimator.EndFall = true;
                }

            }
            else if (_playerAnimator.YSpeed == 1f && _isGrounded) _playerAnimator.EndFall = false;

        }

        private void OnFlip()
        {
            if (_isFlipProhibited) return;
            if (_isFacingRight && _horizontalUserInput < 0)
            {
                _isFacingRight = !_isFacingRight;
            }
            else if (!_isFacingRight && _horizontalUserInput > 0)
            {
                _isFacingRight = !_isFacingRight;
            }
          
            _playerSprite.flipX = !_isFacingRight;
        }
        #region SwapLayerMask
        private bool _wasEligible = false;

        private bool EligibleToSwapLayer()
        {
            int layerToCheck;
            _ = _currentLayer == 0 ? layerToCheck = 1 :
                layerToCheck = 0;
            Physics2D.IgnoreLayerCollision(_layerValue[0], _layerValue[1], false);
            Physics2D.IgnoreLayerCollision(_layerValue[1], _layerValue[0], false);
            RaycastHit2D hit = Physics2D.Raycast(_ground.position,
                Vector2.down, _checkRadius, _groundLayers[layerToCheck]);
            Physics2D.IgnoreLayerCollision(_layerValue[0], _layerValue[1], true);
            Physics2D.IgnoreLayerCollision(_layerValue[1], _layerValue[0], true);

            if (hit.collider == null) return true;
            else return false;
        }

        private void SwapLayer()
        {
            _ = _currentLayer == 0 ? _currentLayer = 1 :
                _currentLayer = 0;

            gameObject.layer = Converter.BitToLayer(_groundLayers[_currentLayer]);
        }

        private void OnLayerEdge()
        {
            SwapLayer();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Edge"))
            {
                OnLayerEdge();
            }
        }
        #endregion

        private bool IsGrounded()
        {


            RaycastHit2D tryToCheckGround = IsCollide(_ground.position, Vector2.down, _checkRadius);
            if (tryToCheckGround.collider.gameObject != gameObject)
            {
                _currentPlatformDegree = tryToCheckGround.normal;
                return tryToCheckGround;
            }
            else
            {
                Collider2D[] colls = Physics2D.OverlapCircleAll(_ground.position, .52f, _groundLayers[_currentLayer]);
                if (colls.Length <= 0) return false;
                for (int i =0; i < colls.Length; i++)
                {
                    if (colls[i].gameObject != gameObject)
                    {
                        _currentPlatformDegree = new(0, 1);
                        return true;
                    }
                }
            }

            return false;
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_ground.position, .52f);
        }

        private RaycastHit2D IsCollide(Vector3 from, Vector2 direction, float distance)
        {
            RaycastHit2D hit = Physics2D.Raycast(from, direction, distance, _groundLayers[_currentLayer]);
            return hit;
        }

        public string ToDebug()
        {
            
            string returner = "\n<b>Player Movement</b>\n";
            returner += "Facing: <i>" + (_isFacingRight ? "Right" : "Left") +"</i>\n";
            returner += $"In Ground: <i>{_isGrounded}</i>\n";
            returner += $"Velocity: <i>{_rigidbody2D.velocity}</i>\n";
            returner += $"Position: <i>{transform.position}</i>\n";
            returner += $"Current Layer: <i>{LayerMask.LayerToName(Converter.BitToLayer(_groundLayers[_currentLayer]))}</i>\n";
            returner += $"State: <i>{_playerState}</i>\n";
            returner += $"Orbs: <i>{SaveSystem.GetPlayerData().GetOrbsInHand()}</i>\n";

            return returner;
        }
    }
}

