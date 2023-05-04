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

        private PEnhanceMovement _enhanceMovement;
        private void Start()
        {
            _enhanceMovement = GetComponent<PEnhanceMovement>();

        }
        private void SetAnimationInCutscene(bool inCutscene)
        {
            _animator.enabled = !inCutscene;
        }
    }

}
