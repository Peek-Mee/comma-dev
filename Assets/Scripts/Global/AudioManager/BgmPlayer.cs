using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Comma.Global.AudioManager
{
    [System.Serializable]
    public struct AudioConfig
    {
        [SerializeField] private AudioClip audioClip;
        [SerializeField] private float _startVolume;
        public AudioClip Clip => audioClip;
        public float StartVolume => _startVolume;
    }
    public class BgmPlayer : MonoBehaviour
    {
        [SerializeField] private AudioMixerGroup _bgmMixer;
        [SerializeField] private AudioConfig[] _bgmList;
        [SerializeField] private float _fadeInTime = 1.5f;
        [SerializeField] private float _fadeOutTime = .8f;

        [Header("Configuration")]
        [SerializeField] private int _indexPlayedAwake;
        private int _currentIdx;

        private List<AudioSource> _audioSources;

        public static BgmPlayer Instance { get; private set; }

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

            _audioSources = new List<AudioSource>();
            for (int i = 0; i < _bgmList.Length; i++)
            {
                var audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.playOnAwake= false;
                audioSource.outputAudioMixerGroup = _bgmMixer;
                audioSource.clip = _bgmList[i].Clip;
                audioSource.volume = 0f;
                audioSource.loop = true;
                audioSource.mute = true;
                _audioSources.Add(audioSource);

            }

            DontDestroyOnLoad(gameObject);
        }

        public void PlayBgm(int index, bool isFirst = false)
        {
            BlendBgm(index, isFirst);
        }

        private void BlendBgm(int targetIndex, bool isFirst = false)
        {
            var source = _audioSources[targetIndex];
            var defaultVolume = _bgmList[targetIndex].StartVolume;
            source.mute = false;
            source.Play();
            switch (isFirst)
            {
                case true:
                    LeanTween.value(0f, 1f, _fadeInTime).setOnUpdate(val =>
                    {
                        source.volume = val * defaultVolume;

                    }).setOnComplete(() =>
                    {
                        _currentIdx = targetIndex;
                    });
                    break;
                case false:
                    var curSource = _audioSources[_currentIdx];
                    var curVolume = _bgmList[_currentIdx].StartVolume;
                    LeanTween.value(0f, 1f, _fadeInTime).setOnUpdate(val =>
                    {
                        source.volume = val * defaultVolume;

                    });
                    LeanTween.value(0f, 1f, _fadeOutTime).setOnUpdate(val =>
                    {
                        curSource.volume = (1f - val) * curVolume;

                    }).setOnComplete(() =>
                    {
                        curSource.mute = true;
                        curSource.Stop();
                        _currentIdx = targetIndex;
                    });
                    break;
            }
        }
    }

}
