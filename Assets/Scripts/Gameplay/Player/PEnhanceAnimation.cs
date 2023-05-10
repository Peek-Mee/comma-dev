using Comma.Global.PubSub;
using Comma.Utility.Collections;
using System.Collections;
using UnityEngine;

namespace Comma.Gameplay.Player
{
    public struct PEAnimationPrevFrame
    {
        public bool WasRunning { get; set; }
        public bool WasGrounded { get; set; }
        public bool WasMoving { get; set; }
    }
    [RequireComponent(typeof(PEnhanceMovement))]
    public class PEnhanceAnimation : MonoBehaviour
    {
        /*
         * This script is specifically been made for PEnhanceMovement
         * Please make adjustment if you want to use it for 
         * other player movement script
         * 
         * 
         */

        // Animator controller
        [SerializeField] private Animator _animator;
        [SerializeField] private PEnhanceSound _playerSFX;
        private PEAnimationPrevFrame _prevFrame;

        private bool _isMoveInteract;
        private bool _isNonMoveInteract;
        private bool _portal;
        private bool _push;
        private bool _waitForSingleLoopAnimation;

        private PEnhanceMovement _enhanceMovement;
        private MoveableDetection _moveableDetection;
        private void Start()
        {
            _enhanceMovement = GetComponent<PEnhanceMovement>();
            _moveableDetection = GetComponent<MoveableDetection>();
        }

        private void Update()
        {
            SetNormalAnimation();
            SetLastAnimationState();
        }
        private void FixedUpdate()
        {
            SetNormalAnimation();
            SetLastAnimationState();
        }
        private void SetLastAnimationState()
        {
            _prevFrame.WasGrounded = _enhanceMovement.IsGrounded;
            _prevFrame.WasRunning = _enhanceMovement.IsRunning;
            _prevFrame.WasMoving = Mathf.Abs(_enhanceMovement.Movement.x) > 0.1f;
        }
        /// <summary>
        /// Set all animation variables for non cutscene states
        /// </summary>
        private void SetNormalAnimation()
        {
            if (_waitForSingleLoopAnimation) return;
            _isMoveInteract = _moveableDetection.Holded;
            _push = _moveableDetection.Push;

            // Set Animation Type {0: normal; 1: MoveInteract; 2: NonMoveInteract; }
            _animator.SetFloat("Type", _isNonMoveInteract ? 2f : _isMoveInteract ? 1f : 0f);
            // Set Is Grounded (float (0,1))
            _animator.SetFloat("Ground", Converter.BoolToNum(_enhanceMovement.IsGrounded));
            

            // Logic for horizontal movement
            var xSpeed = 0f;
            if (_prevFrame.WasRunning && !_enhanceMovement.IsRunning && !_enhanceMovement.IsMoving)
            {
                // Start moving Animation
                xSpeed = 4f;
                _animator.SetFloat("Ground", 1f);
                StartCoroutine(DisableInputForSeconds(.45f));
            }
            else
            {
                // Moving Loop animation
                xSpeed = (_enhanceMovement.Movement.x >= 0 ? 1f : -1f) * Converter.MinMaxNormalizer(0, _enhanceMovement.MaxSpeed,
                Mathf.Abs(_enhanceMovement.Movement.x)) * (_enhanceMovement.IsRunning ? 2f : 1f);
                
                if (xSpeed == 0f && _enhanceMovement.IsGrounded)
                {
                    _playerSFX.StopWalkSFX();

                }
                else if (xSpeed != 0f && !_enhanceMovement.IsRunning && _enhanceMovement.IsGrounded)
                {
                    _playerSFX.PlayWalkSFX();
                }
        }

        _animator.SetFloat("XSpeed", xSpeed);

            // Logic for vertical movement
            var ySpeed = _enhanceMovement.Movement.y;
            if (!_prevFrame.WasGrounded && _enhanceMovement.IsGrounded && _enhanceMovement.Movement.y <= 0)
            {
                // Landing Animation
                ySpeed = -2f;
                _animator.SetFloat("Ground", 0f); 
                StartCoroutine(DisableInputForSeconds(.5f));
            }
            else if (_prevFrame.WasGrounded && !_enhanceMovement.IsGrounded && _enhanceMovement.Movement.y >= 0)
            {
                // Start Jumping Animation
                ySpeed = 2f;
                _animator.SetFloat("Ground", 0f);
                StartCoroutine(DisableInputForSeconds(.25f));
            }
            else
            {
                // Loop in the air while jumping/falling
                ySpeed = ySpeed == 0 ? 0 : Mathf.Sign(ySpeed);
            }
            _animator.SetFloat("YSpeed", ySpeed);

            // Set Portal variable (Only executed when type == 2)
            _animator.SetFloat("Portal", Converter.BoolToNum(_portal));
            // Set push/pull variable (Only executed when type == 1)
            _animator.SetFloat("Push", Converter.BoolToNum(_push));
        }
        private void UpdateSingleLoopAnimation()
        {

        }

        private void SetAnimationInCutscene(bool inCutscene)
        {
            _animator.enabled = !inCutscene;
        }
        IEnumerator DisableInputForSeconds(float time)
        {
            _waitForSingleLoopAnimation = true;
            //print("Wait For Single Loop [Start]");
            _enhanceMovement.InputDisabled = true;
            //EventConnector.Publish("InputEnabled", false);
            yield return new WaitForSeconds(time);
            _enhanceMovement.InputDisabled = false;
            //EventConnector.Publish("InputEnabled", true);
            //print("Wait For Single Loop [End]");
            _waitForSingleLoopAnimation = false;
        }
    }

}
