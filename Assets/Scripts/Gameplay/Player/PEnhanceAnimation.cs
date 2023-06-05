using Comma.Utility.Collections;
using System;
using System.Collections;
using UnityEngine;

namespace Comma.Gameplay.Player
{
    public struct PEAnimationPrevFrame
    {
        public bool WasRunning { get; set; }
        public bool WasGrounded { get; set; }
        public bool WasMoving { get; set; }
        public bool WasJumping { get; set; }
        public float GroundGap { get; set; }
    }
    [RequireComponent(typeof(PEnhanceMovement))]
    public class PEnhanceAnimation : MonoBehaviour
    {
        /*
         * This script is specifically been made for PEnhanceMovement
         * Please make adjustment if you want to use it for 
         * other player movement script
         * 
         */

        // Animator controller
        [SerializeField] private Animator _animator;
        [SerializeField] private PEnhanceSound _playerSFX;
        [SerializeField]
        [Range(0f, 1f)] private float _minJumpThreshold = .25f;

        [Header("Disable Input Time")]
        [SerializeField] private float _startWalk = 0f;
        [SerializeField] private float _endRun = 0f;
        [SerializeField] private float _startJump = 0.1f;
        [SerializeField] private float _land = .35f;



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
            _prevFrame.WasMoving = _enhanceMovement.IsMoving;
            _prevFrame.WasJumping = _enhanceMovement.PreviouslyJumping;
            _prevFrame.GroundGap = _enhanceMovement.GroundDistance;
        }
        /// <summary>
        /// Set all animation variables for non cutscene states
        /// </summary>
        private void SetNormalAnimation()
        {
            if (_waitForSingleLoopAnimation) return;
            _isMoveInteract = _moveableDetection.Holded;
            _push = _moveableDetection.Push;

            /*
             * ANIMATION TYPE
             * [0] NORMAL -> IDLE/WALK/RUN
             * [1] MOVE INTERACT -> PULL/PUSH
             * [2] NON MOVE INTERACT -> PORTAL/ORB INTERACT
             */
            _animator.SetFloat("Type", _isNonMoveInteract ? 2f : _isMoveInteract ? 1f : 0f);


            // Set Is Grounded (float (0,1))
            _animator.SetFloat("Ground", Converter.BoolToNum(_enhanceMovement.IsGrounded));

            HorizontalAnimation();

            VerticalAnimation();

            // Set Portal variable (Only executed when type == 2)
            _animator.SetFloat("Portal", Converter.BoolToNum(_portal));
            // Set push/pull variable (Only executed when type == 1)
            _animator.SetFloat("Push", Converter.BoolToNum(_push));
        }
        private void HorizontalAnimation()
        {
            // variable storing 0
            var xSpeed = 0f;

            /* 
             * || STATE OF HORIZONTAL MOVEMENT ||
             * START WALK => WALK/RUN => END RUN (STOP)
             * 
             * || ANIM CONSTANTS ||
             * [-3] WALK START
             * [-2] RUN LOOP
             * [-1] WALK LOOP
             * [0] IDLE LOOP
             * [1] WALK LOOP
             * [2] RUN LOOP
             * [3] END RUN
             */

            // ==SFX==

            if (!_enhanceMovement.IsMoving || !_enhanceMovement.IsGrounded)
            {
                _playerSFX.StopRunSFX();
                _playerSFX.StopWalkSFX();
            }
            else if (_enhanceMovement.IsMoving && _enhanceMovement.IsGrounded)
            {
                if (_enhanceMovement.IsRunning)
                {
                    _playerSFX.PlayRunSFX();
                }
                else
                {
                    _playerSFX.PlayWalkSFX();
                }
            }

            // ==WALK/RUN==
            /*
             * START WALK
             * Grounded
             * Previously not moving and is currently moving
             * Not pressing run button
             */
            if (_enhanceMovement.IsGrounded && !_prevFrame.WasMoving && 
                _enhanceMovement.IsMoving && !_enhanceMovement.IsRunning)
            {
                xSpeed = -3f;
                _animator.SetFloat("Ground", 1f);
                StartCoroutine(DisableInputForSeconds(_startWalk, Dummy.VoidFunction));
            }
            /*
             * END RUN
             * Grounded
             * Previously running (and moving)
             * Currently not moving
             */
            else if (_enhanceMovement.IsGrounded && (_prevFrame.WasMoving && _prevFrame.WasRunning) &&
                !_enhanceMovement.IsMoving)
            {
                xSpeed = 3f;
                _animator.SetFloat("Ground", 1f);
                StartCoroutine(DisableInputForSeconds(_endRun, Dummy.VoidFunction));
            }
            /*
             * WALK/RUN LOOP
             * Grounded
             * Is currently moving (walk/run)
             */
            else
            {
                xSpeed = (_enhanceMovement.Movement.x > 0 ? 1f : -1f) * 
                    Converter.MinMaxNormalizer(0, _enhanceMovement.MaxSpeed, 
                    Mathf.Abs(_enhanceMovement.Movement.x)) * (_enhanceMovement.IsRunning ? 2f : 1f);
            }

            _animator.SetFloat("XSpeed", xSpeed);
        }
        private void VerticalAnimation()
        {
            // Define local variable storing vertical speed
            var ySpeed = _enhanceMovement.Movement.y;

            /* 
             * || STATE OF VERTICAL MOVEMENT ||
             * START JUMP => JUMP IN AIR => LANDING  (NORMAL)
             * JUMP IN AIR => LANDING (FREE FALL)
             * 
             * || ANIM CONSTANTS ||
             * [-2] LANDING
             * [-2 > x > 2] JUMP LOOP (IN AIR)
             * [2] START JUMP (ANTICIPATION)
             */

            // ==START JUMP==
            if (_prevFrame.WasGrounded && !_enhanceMovement.IsGrounded && _enhanceMovement.Movement.y >= 0)
            {
                _playerSFX.PlayJumpSFX(); // Play START JUMP SFX
                ySpeed = 2f;
                _animator.SetFloat("Ground", 0f); // Anticipation if player still detected
                StartCoroutine(DisableInputForSeconds(_startJump, Dummy.VoidFunction));
            }
            // ==LANDING==
            else if (!_prevFrame.WasGrounded && _enhanceMovement.IsGrounded && _enhanceMovement.Movement.y <= 0)
            {
                // Free fall condition
                if (!_prevFrame.WasJumping && Mathf.Abs(_prevFrame.GroundGap - _enhanceMovement.GroundDistance) < _minJumpThreshold) 
                {
                    // Don't play jump animation if the gap is too small
                    _animator.SetFloat("YSpeed", 0);
                    _animator.SetFloat("Ground", 1f);
                    return; 
                }
                // Normal landing condition
                _playerSFX.PlayLandSFX();
                ySpeed = -2f;
                _animator.SetFloat("Ground", 0f);
                // Make sure landing animation finish before receive next input
                // by disabling player input for a few miliseconds
                StartCoroutine(DisableInputForSeconds(_land, () =>
                {
                    _enhanceMovement.PreviouslyJumping = false;
                    return true;
                }));
            }
            // ==JUMP IN AIR==
            else
            {
                if (!_prevFrame.WasJumping && _enhanceMovement.GroundDistance < _minJumpThreshold)
                {
                    _animator.SetFloat("YSpeed", 0);
                    _animator.SetFloat("Ground", 1f);
                    return;
                }

                ySpeed = ySpeed == 0 ? 0 : Mathf.Sign(ySpeed);
            }

            // Set animation parameter
            _animator.SetFloat("YSpeed", ySpeed); 
        }
        

        private void SetAnimationInCutscene(bool inCutscene)
        {
            _animator.enabled = !inCutscene;
        }
        /// <summary>
        /// Disable player input for character movement
        /// </summary>
        /// <param name="time">How long input should be disabled</param>
        /// <param name="invokeAfter">Function to be invoked after wait time is finished</param>
        /// <returns></returns>
        IEnumerator DisableInputForSeconds(float time, Func<bool> invokeAfter)
        {
            // Don't update animation state until the wait time is finish
            _waitForSingleLoopAnimation = true;
            // Disable user input for movement
            _enhanceMovement.InputDisabled = true;
            yield return new WaitForSeconds(time);
            // Enable user input for movement
            _enhanceMovement.InputDisabled = false;
            // Invoke function (if any)
            invokeAfter();
            // Permits script to update animation state
            _waitForSingleLoopAnimation = false;
        }
    }

}
