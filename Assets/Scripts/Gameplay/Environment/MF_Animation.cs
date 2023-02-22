using UnityEngine;

namespace Comma.Gameplay.Environment
{
    public class MF_Animation : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        [SerializeField] private bool _idle;
        [SerializeField] private bool _move;
        [SerializeField] private float _xSpeed;
        [SerializeField] private bool _inAir;
        [SerializeField] private bool _startWalk;
        [SerializeField] private bool _startRun;
        [SerializeField] private bool _startJump;
        [SerializeField] private bool _startLand;

        private void Update()
        {
            _animator.SetBool("Idle", _idle);
            _animator.SetBool("StartWalk", _startWalk);
            _animator.SetBool("StartRun", _startRun);
            _animator.SetBool("Move", _move);
            _animator.SetBool("InAir", _inAir);
            _animator.SetBool("StartJump", _startJump);
            _animator.SetBool("StartLand", _startLand);
        }
    }
}
