using Comma.Gameplay.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Comma.CutScene
{
    public class CutScenePlayerController : MonoBehaviour
    {
        [SerializeField] private GameObject _player;

        private Rigidbody2D _playerRigid;
        private Collider2D _playerColl;
        private PlayerMovement _playerMovement;
        private PlayerAnimationController _playerAnimator;

        private void Awake()
        {
            _playerRigid = _player.GetComponent<Rigidbody2D>();
            _playerMovement = _player.GetComponent<PlayerMovement>();
            _playerAnimator = _player.GetComponent<PlayerAnimationController>();
            _playerColl = _player.GetComponent<Collider2D>();
        }

        public void OnEnterCutscene()
        {
            
            _playerMovement.InCutScene = true;
            _playerRigid.isKinematic= true;
            _playerColl.enabled = false;
            SetDefaultAnimator();

        }
        public void OnExitCutscene()
        {
            _playerMovement.InCutScene= false;
            _playerRigid.isKinematic= false;
            _playerColl.enabled = true;
            SetDefaultAnimator();
        }

        private void SetDefaultAnimator()
        {
            _playerAnimator.Idle = true;
            _playerAnimator.XSpeed= 0;
            _playerAnimator.YSpeed= 0;
            _playerAnimator.Move = false;
            _playerAnimator.Push = false;
            _playerAnimator.Pull = false;
            _playerAnimator.EndFall = false;
            _playerAnimator.StartJump = false;
            _playerAnimator.StartWalk= false;
            _playerAnimator.StartRun= false;
        }
    }
}
