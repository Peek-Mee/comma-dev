using System;
using Comma.Gameplay.Player;
using Comma.Gameplay.DetectableObject;
using Comma.Global.AudioManager;
using UnityEngine;
using UnityEngine.Serialization;
using Comma.Global.PubSub;

namespace Comma.Gameplay.Player
{
    [RequireComponent(typeof(Collider2D))]
    public class MoveableDetection : MonoBehaviour
    {
        private PlayerMovement _player;
        private bool isHoldingObject = false; 
        [SerializeField] private LayerMask _layerMask; 
        [SerializeField] private float _distance,_radius;
        private PlayerAnimationController _playerAnimator;

        private void Awake()
        {
            _playerAnimator = GetComponent<PlayerAnimationController>();
        }
        private void Start()
        {
            _player = GetComponent<PlayerMovement>();
            EventConnector.Subscribe("OnInteractInput", new(OnInteract));

        }

        #region PubSub
        private bool _isHoldMoveable;
        private IMoveableObject _moveableObject;
        private bool _objectOnRight;
        private float _direction;
        private void OnInteract(object msg)
        {
            _moveableObject.Interact();
            _moveableObject.UnInteract();

            if (_isHoldMoveable)
            {
                _isHoldMoveable = false;
                SFXController.Instance.StopObjectSFX();
            }
            else if (_moveableObject != null)
            {
                _isHoldMoveable = true;
                SFXController.Instance.PlayInteractObjectSFX();
            }
        }
        #endregion

        #region Detection
        private bool _inGrabArea;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Moveable"))
            {
                _inGrabArea = true;
                _moveableObject = collision.GetComponent<IMoveableObject>();

                Vector2 normal = collision.transform.position - transform.position;
                _objectOnRight = normal.x > 0;
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Moveable"))
            {
                _inGrabArea = false;
                _moveableObject.UnInteract();
                _moveableObject = null;
            }
        }
        private void ChangeAnimationState()
        {
            int normalizeDirection = (int) _direction * (_objectOnRight ? 1 : -1);

            _playerAnimator.Push = normalizeDirection > 0;
            _playerAnimator.Pull = normalizeDirection < 0;

            // Play SFX
            switch (normalizeDirection)
            {
                case 0:
                    SFXController.Instance.StopObjectSFX();
                    break;
                case 1:
                    SFXController.Instance.PlayPushSFX();
                    break;
                case -1:
                    SFXController.Instance.PlayPullSFX();
                    break;
            }


        } 
        #endregion

        private void Update()
        {
            OnHitMoveable();
        }

        private void OnHitMoveable()
        {
            var rightDirection = new Vector2(transform.position.x + _distance, transform.position.y);
            var leftDirection = new Vector2(transform.position.x - _distance, transform.position.y);
            RaycastHit2D rightHit = Physics2D.CircleCast(rightDirection, _radius, rightDirection,0,_layerMask);
            RaycastHit2D leftHit = Physics2D.CircleCast(leftDirection, _radius, leftDirection,0,_layerMask);
            
            if(rightHit.collider != null && rightHit.collider.CompareTag("Moveable"))
            {
                GetInput(rightHit.collider,1);
                RightTriggerAnimation(_player.GetInput);
                Debug.Log("Hit Moveable");
            }
            else if(leftHit.collider != null && leftHit.collider.CompareTag("Moveable"))
            {
                GetInput(leftHit.collider,-1);
                LeftTriggerAnimation(_player.GetInput);
                Debug.Log("Hit Moveable");
            }
        }
        private void GetInput(Collider2D col,float direction)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (isHoldingObject)
                {
                    //Play idle animation
                    _player.IsFlipProhibited = false;
                    isHoldingObject = false;
                    var moveable = col.GetComponent<MoveableObject>();
                    moveable.UnInteract();

                    SFXController.Instance.StopObjectSFX();
                }
                else
                {
                    isHoldingObject = true;
                    PlayerFlip(direction);
                    _player.IsFlipProhibited = true;
                    IDetectable detectable = col.gameObject.GetComponent<IDetectable>();
                    detectable?.Interact();
                    var moveable = col.GetComponent<MoveableObject>();
                    moveable.GetDetection(this,direction);

                    SFXController.Instance.PlayInteractObjectSFX();
                }
            }
        }
        private void LeftTriggerAnimation(float input)
        {
            if (!isHoldingObject) return;// Play Idle Animation
            if (input == 0)
            {
               SFXController.Instance.StopObjectSFX();
            }
            else if(input > 0)
            {
                //Play Pull ANim
                SFXController.Instance.PlayPullSFX();
            }
            else if (input < 0)
            {
                //Play Push Animation
                SFXController.Instance.PlayPushSFX();
            }
        }
        private void RightTriggerAnimation(float input)
        {
            if (!isHoldingObject) return;// Play Idle Animation
            if (input == 0)
            {
                SFXController.Instance.StopObjectSFX();
            }
            else if (input > 0)
            {
                //Play Push Anim
                SFXController.Instance.PlayPushSFX();
            }
            else if (input < 0)
            {
                //Play Pull Animation
                SFXController.Instance.PlayPullSFX();
            }
        }

        private void PlayerFlip(float dir)
        {
            if (dir > 0)
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
            else if(dir < 0)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
        }

        private void OnDrawGizmos()
        {
            var rightDirection = new Vector2(transform.position.x + _distance, transform.position.y);
            var leftDirection = new Vector2(transform.position.x - _distance, transform.position.y);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(rightDirection, _radius);
            Gizmos.DrawWireSphere(leftDirection,_radius);
        }

        
    }
}