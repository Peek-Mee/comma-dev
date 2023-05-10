using Comma.Gameplay.Player;
using System;
using System.Collections;
using UnityEngine;

public class PEnhanceSound : MonoBehaviour
{
    private AudioSource _audioSource;

    // Database
    [SerializeField] private AudioClip[] _walkClips;
    [SerializeField] private AudioClip[] _runClips;
    [SerializeField] private AudioClip[] _jumpClips;
    [SerializeField] private AudioClip[] _landClips;
    
    private void OnEnable()
    {
        if (!TryGetComponent(out _audioSource))
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    #region Walk
    private bool _isWalkPlaying;
    private bool _isTimeToStopWalk;
    public void PlayWalkSFX()
    {
        if (_isWalkPlaying) return;
        _isTimeToStopWalk = false;
        _isWalkPlaying = true;
        StartCoroutine(PlaySequenceAnimationSFX(_walkClips, () =>  _isTimeToStopWalk));
    }
    public void StopWalkSFX()
    {
        _isTimeToStopWalk = true;
        _isWalkPlaying = false;
        StopSequenceSFX();
    }
    #endregion

    private void PlayAnimationAudioInSequence(AudioClip[] clips, Func<bool> stopCondition)
    {
        while (!stopCondition())
        {
            if (_audioSource.isPlaying) continue;
            var random = UnityEngine.Random.Range(0, clips.Length);
            _audioSource.clip = clips[random];
        }
        _audioSource?.Stop();
        _audioSource.clip = null;
    }
    private void StopSequenceSFX()
    {
        _audioSource?.Stop();
        _audioSource.clip = null;
    }

    private IEnumerator PlaySequenceAnimationSFX(AudioClip[] clips, Func<bool> stop)
    {
        if (stop())
        {
            //_audioSource?.Stop();
            //_audioSource.clip = null;
            yield return null;
        }
        else
        {
            yield return new WaitUntil(()=> !_audioSource.isPlaying);
            var random = UnityEngine.Random.Range(0, clips.Length-1);
            _audioSource.clip = clips[random];
            _audioSource?.Play();
            StartCoroutine(PlaySequenceAnimationSFX(clips, stop));
        }
    }
    private IEnumerator PlaySequenceSFXLoop(AudioClip clip, Func<bool> stop)
    {
        if (stop()) yield return null;
        else
        {
            yield return new WaitUntil( ()=> _audioSource.isPlaying);
            _audioSource.loop = true;
            _audioSource.clip = clip;
            _audioSource?.Play();
        }
    }
}
