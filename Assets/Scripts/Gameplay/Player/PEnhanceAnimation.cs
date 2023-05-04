using Comma.Utility.Collections;
using UnityEngine;

namespace Comma.Gameplay.Player
{
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

        private bool _isMoveInteract;
        private bool _isNonMoveInteract;
        private bool _portal;
        private bool _push;

        private PEnhanceMovement _enhanceMovement;
        private void Start()
        {
            _enhanceMovement = GetComponent<PEnhanceMovement>();

        }

        private void Update()
        {
            SetNormalAnimation();   
        }

        private void SetNormalAnimation()
        {
            _animator.SetFloat("Type", _isNonMoveInteract ? 2f : _isMoveInteract ? 1f : 0f);
            _animator.SetFloat("Ground", Converter.BoolToNum(_enhanceMovement.IsGrounded));
            
            var xSpeed = Converter.MinMaxNormalizer(0, _enhanceMovement.MaxSpeed,
                Mathf.Abs(_enhanceMovement.Movement.x)) * (_enhanceMovement.IsRunning ? 2f : 1f);
            _animator.SetFloat("XSpeed", xSpeed);
            var ySpeed = _enhanceMovement.Movement.y;
            _animator.SetFloat("YSpeed", ySpeed == 0 ? 0 : Mathf.Sign(ySpeed));

            _animator.SetFloat("Portal", Converter.BoolToNum(_portal));
            _animator.SetFloat("Push", Converter.BoolToNum(_push));
        }

        private void SetAnimationInCutscene(bool inCutscene)
        {
            _animator.enabled = !inCutscene;
        }
    }

}
