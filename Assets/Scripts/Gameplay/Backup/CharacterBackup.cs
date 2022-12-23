using System;
using UnityEngine;

namespace Comma.Gameplay.Backup
{
    public class CharacterBackup : MonoBehaviour
    {
        private Rigidbody2D _rigidbody2D;
        
        [Header("Movement")]
        [SerializeField] private float _speed;
        private float horizontalInput;
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
        }

        private void OnMove()
        {
            _rigidbody2D.velocity = new Vector2(horizontalInput * _speed, _rigidbody2D.velocity.y);
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
    }
}

