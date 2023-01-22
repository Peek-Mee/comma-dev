using UnityEngine;
using System.Collections;
using System;

namespace Comma.Global.AudioManager
{
    [Serializable]
    public class SFXClip
    {
        [SerializeField] private string _SFXName;
        [SerializeField] private AudioClip _audioClip;

        public string Name => _SFXName;
        public AudioClip Clip => _audioClip;
    }
    public class SFXController : MonoBehaviour
    {
        [SerializeField] private SFXClip[] _sfxClips;
        private AudioSource _audioSource;
        [SerializeField] private AudioSource _movementAudioSource;

        [Header("WALK SFX")]
        [SerializeField] private float _walkPlayRate;
        private bool _isWalk = true;
        private float _lastPlayWalk;

        [Header("RUN SFX")]
        [SerializeField] private float _runPlayRate;
        private bool _isRun = true;
        private float _lastPlayRun;

        [Header("JUMP SFX")]
        [SerializeField] private float _jumpPlayRate;
        private float _lastPlayJump;

        [Header("INTERACT OBJECT SFX")]
        private bool isInteractSFX;

        [Header("PULL SFX")]
        [SerializeField] private float _pullPlayRate;
        private float _lastPlayPull;

        [Header("PUSH SFX")]
        [SerializeField] private float _pushPlayRate;
        private float _lastPlayPush;

        public static SFXController Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                Instance = this;
            }
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void PlaySFX(string audioName)
        {
            SFXClip audio = Array.Find(_sfxClips, sfx => sfx.Name == audioName);
            if (audio == null)
            {
                Debug.LogWarning($"SFX: <color=red> {name} </color> not found!");
                return;
            }
            _audioSource.PlayOneShot(audio.Clip);
        }

        public void PlayMovementSFX(bool isWalk, bool isRun)
        {
            if(isWalk)
            {
                _movementAudioSource.pitch = _walkPlayRate;
                if (_movementAudioSource.isPlaying) return;
                _movementAudioSource.Play();
            }
            else if(isRun)
            {
                _movementAudioSource.pitch = _runPlayRate;
                if (_movementAudioSource.isPlaying) return;
                _movementAudioSource.Play();
            }
            else
            {
                // Stop Movement Audio
                _movementAudioSource.Stop();
            }
          
        }

        public void PlayJumpSFX(bool canJump)
        {
            if (!canJump) return;
            if(Time.time - _lastPlayJump > _jumpPlayRate)
            {
                _lastPlayJump = Time.time;
                PlaySFX("Jump");
            }
            
        }

        #region Object Interact SFX
        public void PlayInteractObjectSFX()
        {
            PlaySFX("InteractObject");
        }

        public void PlayPullSFX()
        {
            if(Time.time - _lastPlayPull > _pullPlayRate)
            {
                _lastPlayPull = Time.time;
                PlaySFX("Pull");
                isInteractSFX = true;
                return;
            }
        }

        public void PlayPushSFX()
        {
            if (Time.time - _lastPlayPush > _pushPlayRate)
            {
                _lastPlayPush = Time.time;
                PlaySFX("Push");
                isInteractSFX = true;
                return;
            }
        }

        public void StopObjectSFX()
        {
            if (!isInteractSFX) return;
            _lastPlayPull = 0;
            _lastPlayPush = 0;
            _audioSource.Stop();
            isInteractSFX = false;
        }
        #endregion

        public void PlayInteractPortalSFX()
        {
            PlaySFX("InteractPortal");
        }

        public void PlayObtainOrbSFX()
        {
            PlaySFX("ObtainOrb");
        }

        IEnumerator WalkSFX()
        {
            while (_isWalk)
            {
                _isWalk = false;
                PlaySFX("Walk");
                yield return new WaitForSeconds(_walkPlayRate);
                _isWalk = true;
            }

        }
    }
}

