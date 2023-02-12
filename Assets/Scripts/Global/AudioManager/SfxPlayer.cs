
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Comma.Global.AudioManager
{
    [System.Serializable]
    public struct SfxClip
    {
        [SerializeField] private string _name;
        [SerializeField] private AudioClip _audioClip;
        [SerializeField] private float _defaultVolume;
        public string Name => _name;
        public AudioClip AudioClip => _audioClip;
        public float Volume => _defaultVolume;
    }
    public class SfxPlayer : MonoBehaviour
    {
        [SerializeField] private SfxClip[] _sfxClips;
        [SerializeField] private AudioMixerGroup _mixer;
        private Dictionary<string, SfxClip> _sfxBank;
        private List<AudioSource> _sourceBank;

        public static SfxPlayer Instance { get; private set; }

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
            _sfxBank = new();
            _sourceBank = new();

            for (int i =0; i < _sfxClips.Length; i++)
            {
                _sfxBank.Add(_sfxClips[i].Name, _sfxClips[i]);
            }

            for (int i = 0; i < 2; i++)
            {
                // Add source 0 = loop; 1 = once
                var source = gameObject.AddComponent<AudioSource>();
                source.playOnAwake = false;
                source.loop = i == 0;

                _sourceBank.Add(source);
            }

            DontDestroyOnLoad(gameObject);
        }

        private string _playedLoop = "";
        public void PlaySfx(string audioName, bool isLooping = false)
        {
            int loop = isLooping ? 0 : 1;
            if (isLooping) _playedLoop = audioName;
            var source = _sourceBank[loop];
            var audio = _sfxBank[audioName];
            source.clip = audio.AudioClip;
            source.volume = audio.Volume;
            if (!source.isPlaying)
            {
                source.Play();
            }
        }

        public void StopLoopingSfx()
        {
            var source = _sourceBank[0];
            source.Stop();
            _playedLoop = "";
            source.clip = null;
            source.volume = 0f;
        }
        public bool IsPlaying => _sourceBank[1].isPlaying;
        public string PlayedLoop => _playedLoop;
    }
}