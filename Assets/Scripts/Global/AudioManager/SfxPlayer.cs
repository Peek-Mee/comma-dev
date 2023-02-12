
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
        [SerializeField] private float _finishPercentage = .8f;
        private Dictionary<string, SfxClip> _sfxBank;
        private AudioSource _source;

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

            for (int i =0; i < _sfxClips.Length; i++)
            {
                _sfxBank.Add(_sfxClips[i].Name, _sfxClips[i]);
            }

            //for (int i = 0; i < 2; i++)
            //{
            //    // Add source 0 = loop; 1 = once
            //    var source = gameObject.AddComponent<AudioSource>();
            //    source.outputAudioMixerGroup= _mixer;
            //    source.playOnAwake = false;
            //    source.loop = i == 0;

            //    _sourceBank.Add(source);
            //}

            var source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.loop = true;
            source.outputAudioMixerGroup = _mixer;
            _source = source;

        }

        private string _playedLoop = "";
        public void PlaySFX(string audioName, bool oneShoot = false)
        {

            switch (oneShoot)
            {
                case true:
                    _source.PlayOneShot(_sfxBank[audioName].AudioClip);
                    break;
                case false:
                    if (_source.clip != null)
                    {
                        if (!IsAbleToTransition()) return;
                    }
                    _playedLoop = audioName;
                    _source.clip = _sfxBank[audioName].AudioClip;
                    _source.Play();
                    break;
            }


        }
        public void StopSFX()
        {
            
            _source.clip = null;
            _playedLoop = "";
        }
        
        private bool IsAbleToTransition()
        {
            return _source.time >= _source.clip.length * _finishPercentage;
        }
        
        public string PlayedLoop => _playedLoop;
    }
}