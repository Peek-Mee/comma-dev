using System.Collections;
using UnityEngine;

namespace Comma.Global.AudioManager
{
    public class BGMController : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSourceBGM;
        [SerializeField] private AudioClip _homeClip;
        [SerializeField] private AudioClip _gameplayClip;

        [Header("Audio Value")]
        [SerializeField] private float _homeAudioVolume;
        [SerializeField] private float _gameplayAudioVolume;
        [SerializeField] private float _homeAudioDelay;
        [SerializeField] private float _gameplayAudioDelay;

        public static BGMController Instance { get; private set; }

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
            StartCoroutine(PlayHomeAudio());
        }

        private IEnumerator PlayHomeAudio()
        {
            float currentTime = 0;
            while (currentTime < _homeAudioDelay)
            {
                if(!_audioSourceBGM.isPlaying)
                {
                    _audioSourceBGM.Play();
                }
                _audioSourceBGM.clip = _homeClip;
                currentTime += Time.deltaTime;
                _audioSourceBGM.volume = Mathf.Lerp(0, _homeAudioVolume, currentTime / _homeAudioDelay);
                yield return null;
            }
            yield break;
        }
        private IEnumerator PlayGameplayAudio()
        {
            float currentTime = 0;
            while (currentTime < _gameplayAudioDelay)
            {
                if (!_audioSourceBGM.isPlaying)
                {
                    _audioSourceBGM.Play();
                }
                _audioSourceBGM.clip = _gameplayClip;
                currentTime += Time.deltaTime;
                _audioSourceBGM.volume = Mathf.Lerp(0, _gameplayAudioVolume, currentTime / _gameplayAudioDelay);
                yield return null;
            }
            yield break;
        }
        public void StartCourotineHome()
        {
            StartCoroutine(FadeOutHomeAudio());
        }
        public void StartCourotineGameplay()
        {
            StartCoroutine(FadeOutGameplayAudio());
        }
        public IEnumerator FadeOutHomeAudio()
        {
            float currentTime = 0;
            while (currentTime < _homeAudioDelay)
            {
                currentTime += Time.deltaTime;
                _audioSourceBGM.volume = Mathf.Lerp(_audioSourceBGM.volume, 0, currentTime / _homeAudioDelay);
                yield return null;
            }
            StartCoroutine(PlayGameplayAudio());
            yield break;
        }
        public IEnumerator FadeOutGameplayAudio()
        {
            float currentTime = 0;
            while (currentTime < _gameplayAudioDelay)
            {
                currentTime += Time.deltaTime;
                _audioSourceBGM.volume = Mathf.Lerp(_audioSourceBGM.volume, 0, currentTime / _gameplayAudioDelay);
                yield return null;
            }
            StartCoroutine(PlayHomeAudio());
            yield break;
        }

    }
}