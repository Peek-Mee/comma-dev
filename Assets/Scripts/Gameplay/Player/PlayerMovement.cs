using System;
using System.Collections;
using UnityEngine;

namespace Comma.Gameplay.CharacterMovement
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
    public class PlayerMovement : MonoBehaviour
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
        private int _currentLayer = 0;

        // Private
        private Rigidbody2D _rigidbody2D;
        private SpriteRenderer _playerSprite;
        private bool _isInCutScene = false;
        private bool _isFlipProhibited = false;
        // #######

        [Header("Movement")]
        [SerializeField] private float _normalSpeed;
        [SerializeField] private float _runSpeed;
        private float _currentSpeed;
        
        private float horizontalInput;
        private float verticalInput;
        private bool isDashing = false;
        private bool _isFacingRight = true;
        
        [Header("Jump")]
        //[SerializeField] private Transform _groundCheck;
        //[SerializeField] private float _groundDistance;
        [SerializeField] private float _jumpForce;
        [SerializeField] private float _fallMultiplier;
        
        [Header("Animation")]
        private Animator _animator;
        private const string On_Idle= "OnIdle";
        private const string On_Move = "OnMove";
        private const string On_Jump = "OnJump";
        private const string On_Fall = "OnFall";
        private const string Speed = "Speed";
        
        [Header("State")]
        [SerializeField] private PlayerState _playerState;
        [SerializeField] private bool _isGrounded;
        [SerializeField] private bool _isWalking;
        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _playerSprite = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            //print($"Layer 1: {!Physics2D.GetIgnoreLayerCollision(3, 7)}");
            //print($"Layer 2: {_groundLayers[1].value}");
            if (!Physics2D.GetIgnoreLayerCollision(3, 7) || !Physics2D.GetIgnoreLayerCollision(7, 3))
            {
                Physics2D.IgnoreLayerCollision(3, 7, true);
                Physics2D.IgnoreLayerCollision(7, 3, true);
               
            }
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
            _isWalking = Mathf.Abs(horizontalInput) > 0.1f;
            _isGrounded = IsGrounded();
            
            //if (!_isGrounded)
            //{
            //    //print("Hello");
                
            //}

            ChangePlayerState();
            OnWalk();
            OnFlip();
            OnJump();
            OnFall();
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
            }
            else
            {
                if (_rigidbody2D.velocity.y > 0)
                {
                    _playerState = PlayerState.Jump; 
                    if (EligibleToSwapLayer() && wasEligible)
                    {
                        SwapLayer();
                    }
                }
                else
                {
                    _playerState = PlayerState.Fall;
                }

                wasEligible = !EligibleToSwapLayer();
            }
        }
        private void OnMove()
        {
            if(!_isWalking)return;
            _rigidbody2D.velocity = new Vector2(horizontalInput * _currentSpeed, _rigidbody2D.velocity.y);
        }

        private void OnWalk()
        {
            if (_isWalking)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    _currentSpeed = _runSpeed;
                    IdleAnimation(false);
                    MoveAnimation(2,_isGrounded);
                }
                else
                {
                    _currentSpeed = _normalSpeed;
                    IdleAnimation(false);
                    MoveAnimation(1,_isGrounded);
                }
            }
            else
            {
                _currentSpeed = _normalSpeed;
                IdleAnimation(_isGrounded && !_isWalking);
                MoveAnimation(0);
            }
        }
        private void OnJump()
        {
            //JumpAnimation(false);
            if(Input.GetKeyDown(KeyCode.Space) && IsGrounded())
            {
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _jumpForce);
                
                JumpAnimation(true);
                IdleAnimation(false);
                MoveAnimation();
                FallAnimation(false);
            }
        }

        private void OnFall()
        {
            FallAnimation(false);
            Vector2 gravity = new Vector2(0, -Physics2D.gravity.y);
            if(_rigidbody2D.velocity.y < 0)
            {
                _rigidbody2D.velocity -= gravity * _fallMultiplier * Time.deltaTime;
            }
            if(_rigidbody2D.velocity.y < (-_jumpForce/2))
            {
                IdleAnimation(false);
                MoveAnimation();
                JumpAnimation(false);
                FallAnimation(true);
            }
        }

        private void OnFlip()
        {
            if (_isFacingRight && horizontalInput < 0)
            {
                _isFacingRight = !_isFacingRight;
            }
            else if (!_isFacingRight && horizontalInput > 0)
            {
                _isFacingRight = !_isFacingRight;
            }
            if (_isFlipProhibited) return;
            _playerSprite.flipX = !_isFacingRight;
        }
        #region SwapLayerMask
        private bool wasEligible = false;

        private bool EligibleToSwapLayer()
        {
            int layerToCheck;
            _ = _currentLayer == 0 ? layerToCheck = 1 :
                layerToCheck = 0;
            Physics2D.IgnoreLayerCollision(3, 7, false);
            Physics2D.IgnoreLayerCollision(7, 3, false);
            RaycastHit2D hit = Physics2D.Raycast(_ground.position,
                Vector2.down, _checkRadius, _groundLayers[layerToCheck]);
            Physics2D.IgnoreLayerCollision(3, 7, true);
            Physics2D.IgnoreLayerCollision(7, 3, true);

            if (hit.collider == null) return true;
            else return false;
        }

        private void SwapLayer()
        {
            _ = _currentLayer == 0 ? _currentLayer = 1 :
                _currentLayer = 0;

            gameObject.layer = BitToLayer(_groundLayers[_currentLayer]);
        }

        private void OnLayerEdge()
        {
            SwapLayer();
        }

        private int BitToLayer(int bitmask)
        {
            int res = bitmask > 0 ? 0 : 31;
            while(bitmask > 1)
            {
                bitmask >>= 1;
                res++;
            }
            return res;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Edge"))
            {
                OnLayerEdge();
            }
        }
        #endregion

        #region Animation
        private void IdleAnimation(bool isIdle)
        {
            _animator.SetBool(On_Idle, isIdle);
        }
        private void MoveAnimation(float speed = 0, bool ismove = false)
        {
            _animator.SetFloat(Speed,speed);
            _animator.SetBool(On_Move, ismove);
        }
        private void JumpAnimation(bool isJumping)
        {
            _animator.SetBool(On_Jump, isJumping);
        }
        private void FallAnimation(bool isFalling)
        {
            _animator.SetBool(On_Fall, isFalling);
        }
        #endregion

        private bool IsGrounded()
        {
            return Physics2D.Raycast(_ground.position, Vector2.down, _checkRadius, _groundLayers[_currentLayer]);

        }
        
    }
}

