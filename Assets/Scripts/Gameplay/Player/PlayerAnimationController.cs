using Comma.Global.AudioManager;
using Comma.Global.SaveLoad;
using Comma.Utility.Collections;
using UnityEngine;

namespace Comma.Gameplay.Player
{
    public class PlayerAnimationController : MonoBehaviour, IDebugger
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
        [SerializeField] private string _varOnPull = "OnPull";
        [SerializeField] private string _varOnPush = "OnPush";
        [SerializeField] private string _varOnPortalInteract = "OnPortalInteract";
        [SerializeField] private string _varOnWaitInteract = "OnWaitInteract";

        public bool Idle { get; set; } = true;
        public bool Move { get; set; } = false;
        public bool StartWalk { get; set; } = false;
        public bool StartRun { get; set; } = false;
        public bool StartJump { get; set; } = false;
        public bool EndFall { get; set; } = false;
        public bool Push { get; set; } = false;
        public bool Pull { get; set; } = false;
        public bool PortalInteract { get; set; } = false;
        public bool WaitInteract { get; set; } = false;
        public float XSpeed { get; set; } = 0f;
        public float YSpeed { get; set; } = 0f;

        private SfxPlayer _sfxPlayer;
        private void Start()
        {
            _sfxPlayer = SfxPlayer.Instance;
        }
        private void Update()
        {
            _playerAnimator.SetBool(_varOnIdle, Idle);
            _playerAnimator.SetBool(_varOnMove, Move);
            _playerAnimator.SetBool(_varOnStartWalk, StartWalk);
            _playerAnimator.SetBool(_varOnStartRun, StartRun);
            _playerAnimator.SetBool(_varOnStartJump, StartJump);
            _playerAnimator.SetBool(_varOnEndFall, EndFall);
            _playerAnimator.SetBool(_varOnPush, Push);
            _playerAnimator.SetBool(_varOnPull, Pull);
            _playerAnimator.SetBool(_varOnPortalInteract, PortalInteract);
            _playerAnimator.SetBool(_varOnWaitInteract, WaitInteract);
            _playerAnimator.SetFloat(_varXSpeed, XSpeed);
            _playerAnimator.SetFloat(_varYSpeed, YSpeed);

            var SFXWalk = Move && XSpeed == 1 && StartWalk == false;
            var SFXRun = Move && XSpeed == 2 && StartRun == false;
            //SFXController.Instance.PlayMovementSFX(SFXWalk, SFXRun);
            //SFXController.Instance.PlayJumpSFX(StartJump);
            //SFXController.Instance.PlayLandingSFX(EndFall);
            //SFXController.Instance.PlayWalkSFX(Move && XSpeed > 1.01);
            if (_sfxPlayer != null)
            {
                if (Idle)
                {
                    _sfxPlayer.StopLoopingSfx();
                    return;
                }
                else if (StartJump && !_sfxPlayer.IsPlaying) _sfxPlayer.PlaySfx("Jump");
                else if (EndFall && !_sfxPlayer.IsPlaying) _sfxPlayer.PlaySfx("Land");
                else if (PortalInteract && !_sfxPlayer.IsPlaying) _sfxPlayer.PlaySfx("PortalInteract");
                if (Move && XSpeed == 1 && _sfxPlayer.PlayedLoop != "Walk")
                    _sfxPlayer.PlaySfx("Walk", true);
                else if (Move && XSpeed == 2 && _sfxPlayer.PlayedLoop != "Run")
                    _sfxPlayer.PlaySfx("Run", true);

            }
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
        public void EndFallFinish()
        {
            EndFall = false;
        }
        public void EndPortalInteract()
        {
            PortalInteract= false;
            WaitInteract= false;
        }

        public string ToDebug()
        {

            string returner = "\n<b>Player Animation</b>\n";
            returner += $"Idle: <i>{Idle}</i>\n";
            returner += $"Move: <i>{Move}</i>\n";
            returner += $"Start Walk: <i>{StartWalk}</i>\n";
            returner += $"Start Run: <i>{StartRun}</i>\n";
            returner += $"Start Jump: <i>{StartJump}</i>\n";
            returner += $"End Fall: <i>{EndFall}</i>\n";
            returner += $"X Speed: <i>{XSpeed}</i>\n";
            returner += $"Y Speed: <i>{YSpeed}</i>\n";

            return returner;
        }
    }

}
