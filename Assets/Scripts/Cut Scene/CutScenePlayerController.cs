using Comma.Gameplay.Player;
using Comma.Global.AudioManager;
using Comma.Global.SaveLoad;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Comma.CutScene
{
    //[RequireComponent(typeof(Playeable))]
    public class CutScenePlayerController : MonoBehaviour
    {
        [SerializeField] private string _cutsceneId;
        [SerializeField] private GameObject _player;
        private Rigidbody2D _playerRigid;
        private Collider2D _playerColl;
        private PlayerMovement _playerMovement;
        private PlayerAnimationController _playerAnimator;

        private void Awake()
        {
            bool data = SaveSystem.GetPlayerData().IsCutsceneInCollection(_cutsceneId);
            if (data)
            {
                gameObject.SetActive(false);
                return;
            }

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
            SaveSystem.GetPlayerData().AddCutsceneToCollection(_cutsceneId);
            SaveSystem.SaveDataToDisk();
        }
        public void OnExitCutscene()
        {
            _playerMovement.InCutScene= false;
            _playerRigid.isKinematic= false;
            _playerColl.enabled = true;
            SetDefaultAnimator();
            SaveSystem.GetPlayerData().AddCutsceneToCollection(_cutsceneId);
            SaveSystem.SaveDataToDisk();
            gameObject.SetActive(false);
        }
        public void OnEndingCutscene()
        {
            SaveSystem.ResetPlayerData();
            BgmPlayer.Instance.PlayBgm(0);
            SceneManager.LoadSceneAsync("Main Menu");
            gameObject.SetActive(false);
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
