using System;
using UnityEngine;

namespace Comma.Gameplay.Backup
{
    public class CharacterBackup : MonoBehaviour
    {
        private Rigidbody2D _rigidbody2D;

        [Header("Movement")]
        [SerializeField] private Transform _topdownCheck;
        [SerializeField] private float _topdownDistance;
        [SerializeField] private float _speed;
        private float horizontalInput;
        private float verticalInput;
        private bool _isFacingRight = true;
        
        [Header("Jump")]
        [SerializeField] private Transform _groundCheck;
        [SerializeField] private float _groundDistance;
        [SerializeField] private float _jumpForce;

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
            
            OnJump();
            if(_isFacingRight && horizontalInput < 0)
            {
                OnFlip();
            }
            else if(!_isFacingRight && horizontalInput > 0)
            {
                OnFlip();
            }
        }

        private void FixedUpdate()
        {
            OnMove();
            OnMoveTopDown();
        }

        private void OnMove()
        {
            _rigidbody2D.velocity = new Vector2(horizontalInput * _speed, _rigidbody2D.velocity.y);
        }
        private void OnMoveTopDown()
        {
            if(IsTopDown()) 
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, verticalInput * _speed);
        }
        private void OnJump()
        {
            if(Input.GetKeyDown(KeyCode.Space) && IsGrounded())
            {
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _jumpForce);
            }
        }
        
        private void OnFlip()
        {
            _isFacingRight = !_isFacingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
        private bool IsGrounded()
        {
            return Physics2D.Raycast(_groundCheck.position, Vector2.down, _groundDistance, LayerMask.GetMask("Ground"));
        }
        
        private bool IsTopDown()
        {
            
            return Physics2D.Raycast(_topdownCheck.position, Vector2.zero, _topdownDistance, LayerMask.GetMask("Ground"));
        }
    }
}

