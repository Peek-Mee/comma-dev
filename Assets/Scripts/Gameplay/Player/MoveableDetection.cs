using System;
using Comma.Gameplay.CharacterMovement;
using Comma.Gameplay.DetectableObject;
using Comma.Global.AudioManager;
using UnityEngine;
using UnityEngine.Serialization;

namespace Comma.Gameplay.Player
{
    [RequireComponent(typeof(Collider2D))]
    public class MoveableDetection : MonoBehaviour
    {
        private PlayerMovement _player;
        private bool isHoldingObject = false; 
        [SerializeField] private LayerMask _layerMask; 
        [SerializeField] private float _distance,_radius;

        private void Start()
        {
            _player = GetComponent<PlayerMovement>();
        }

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
                GetInput(rightHit.collider);
                RightTriggerAnimation(_player.GetInput);
                Debug.Log("Right hand detection Moveable "+rightHit.collider.gameObject.name);
            }
            else if(leftHit.collider != null && leftHit.collider.CompareTag("Moveable"))
            {
                GetInput(leftHit.collider);
                LeftTriggerAnimation(_player.GetInput);
                Debug.Log("Left hand detection Moveable "+leftHit.collider.gameObject.name);
            }
        }
        private void GetInput(Collider2D col)
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
                }
                else
                {
                    _player.IsFlipProhibited = true;
                    isHoldingObject = true;
                    IDetectable detectable = col.gameObject.GetComponent<IDetectable>();
                    detectable?.Interact();
                }
            }
        }
        private void LeftTriggerAnimation(float input)
        {
            if (!isHoldingObject) return;// Play Idle Animation
            if (input == 0)
            {
                SFXController.Instance.StopCurrentSFX();
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
                SFXController.Instance.StopCurrentSFX();
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

        private void OnDrawGizmos()
        {
            var rightDirection = new Vector2(transform.position.x + _distance, transform.position.y);
            var leftDirection = new Vector2(transform.position.x - _distance, transform.position.y);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(rightDirection, _radius);
            Gizmos.DrawWireSphere(leftDirection,_radius);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Moveable"))
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (isHoldingObject)
                    {
                        isHoldingObject = false;
                    }
                    else
                    {
                        IDetectable coll = collision.gameObject.GetComponent<IDetectable>();
                        coll?.Interact();
                    }
            
                }
            }
        }
    }
}