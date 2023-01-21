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
        [SerializeField] private AudioSource _walkAudioSource;
        [SerializeField] private AudioSource _runAudioSource;

        [Header("WALK SFX")]
        private bool _isWalk = true;
        private float _lastPlayWalk;
        [SerializeField] private float _walkPlayRate;

        [Header("RUN SFX")]
        private bool _isRun = true;
        private float _lastPlayRun;
        [SerializeField] private float _runPlayRate;

        [Header("PULL SFX")]
        private float _lastPlayPull;
        [SerializeField] private float _pullPlayRate;

        [Header("PUSH SFX")]
        private float _lastPlayPush;
        [SerializeField] private float _pushPlayRate;


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
        private void Update()
        {
            //StartCoroutine(WalkSFX());
            //PlayWalkSFX();
            //PlayRunSFX();
        }
        public void StopSFX()
        {
            _audioSource.Stop();
        }
        public void PlaySFX(string audioName)
        {
            SFXClip audio = Array.Find(_sfxClips, sfx => sfx.Name == audioName);
            if (audio == null)
            {
                Debug.LogWarning($"SFX: <color=red> {name} </color> not found!");
                return;
            }
            _audioSource.PlayOneShot(audio.Clip);
        }
        public void PlayWalkSFX(bool canPlay)
        {
            if (canPlay)
            {
                _walkAudioSource.pitch = _walkPlayRate;
                if (_walkAudioSource.isPlaying) return;
                _walkAudioSource.Play();
            }
            else
            {
                StopMovementSFX("Walk");
            }

            // if(Time.time - _lastPlayWalk > _walkPlayRate)
            // {
            //     _lastPlayWalk = Time.time;
            //     //PlaySFX("Walk");
            // }
        }
        public void PlayRunSFX(bool canPlay)
        {
            if (canPlay)
            {
                _walkAudioSource.pitch = _runPlayRate;
                if (_walkAudioSource.isPlaying) return;
                _walkAudioSource.Play();
            }
            else
            {
                StopMovementSFX("Walk");
            }
        }
        public void PlayPullSFX()
        {
            if(Time.time - _lastPlayPull > _pullPlayRate)
            {
                _lastPlayPull = Time.time;
                PlaySFX("Pull");
                Debug.Log("PULL SFX");
            }
        }
        public void PlayPushSFX()
        {
            if (Time.time - _lastPlayPush > _pushPlayRate)
            {
                _lastPlayPush = Time.time;
                PlaySFX("Push");
                Debug.Log("PUSH SFX");
            }
        }
        public void StopMovementSFX(string audioName)
        {
            //_walkAudioSource.loop = false;
            //_runAudioSource.loop = false;
            _walkAudioSource.Stop();
            //_runAudioSource.Stop();
            // SFXClip audio = Array.Find(_sfxClips, sfx => sfx.Name == audioName);
            // if (audio == null)
            // {
            //     Debug.LogWarning($"SFX: <color=red> {name} </color> not found!");
            //     return;
            // }
        }
        public void StopObjectSFX()
        {
            _lastPlayPull = 0;
            _lastPlayPush = 0;
            _audioSource.Stop();
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

