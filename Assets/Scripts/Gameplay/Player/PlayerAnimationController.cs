using UnityEngine;

namespace Comma.Gameplay.Player
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator _playerAnimator;

        [SerializeField] private string _varOnIdle = "OnIdle";
        [SerializeField] private string _varOnStartWalk = "OnStartWalk";
        [SerializeField] private string _varOnStartRun = "OnStartRun";
        [SerializeField] private string _varXSpeed = "XSpeed";
        [SerializeField] private string _varOnStartJump = "OnStartJump";
        [SerializeField] private string _varOnEndFall = "OnEndFall";
        [SerializeField] private string _varYSpeed = "YSpeed";
        [SerializeField] private string _varOnMove = "OnMove";

        public bool Idle { get; set; } = true;
        public bool Move { get; set; } = false;
        public bool StartWalk { get; set; } = false;
        public bool StartRun { get; set; } = false;
        public bool StartJump { get; set; } = false;
        public bool EndFall { get; set; } = false;
        public float XSpeed { get; set; } = 0f;
        public float YSpeed { get; set; } = 0f;

        private void Update()
        {
            _playerAnimator.SetBool(_varOnIdle, Idle);
            _playerAnimator.SetBool(_varOnMove, Move);
            _playerAnimator.SetBool(_varOnStartWalk, StartWalk);
            _playerAnimator.SetBool(_varOnStartRun, StartRun);
            _playerAnimator.SetBool(_varOnStartJump, StartJump);
            _playerAnimator.SetBool(_varOnEndFall, EndFall);
            _playerAnimator.SetFloat(_varXSpeed, XSpeed);
            _playerAnimator.SetFloat(_varYSpeed, YSpeed);
        }

        public void StartWalkFinish()
        {
            StartWalk= false;
        }
        public void StartJumpFinish()
        {
            StartJump= false;
        }
        public void StartRunFinish()
        {
            StartRun= false;
        }
    }

}
