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
    [SerializeField] private AudioClip[] _startPullPushClips;
    [SerializeField] private AudioClip[] _pullPushClips;
    [SerializeField] private AudioClip[] _jumpClips;
    [SerializeField] private AudioClip[] _landClips;
    [SerializeField] private AudioClip[] _portalInteractClips;
    [SerializeField] private AudioClip[] _orbInteractClips;
    
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
    #region Run
    private bool _isRunPlaying;
    private bool _isTimeToStopRun;
    public void PlayRunSFX()
    {
        if (_isRunPlaying) return;
        _isRunPlaying = true;
        _isTimeToStopRun = false;
        StartCoroutine(PlaySequenceAnimationSFX(_runClips, () => _isTimeToStopRun));
    }
    public void StopRunSFX()
    {
        _isTimeToStopRun = true;
        _isRunPlaying = false;
        StopSequenceSFX();
    }
    #endregion
    #region Push Pull
    private bool _isPullPlaying;
    private bool _isTimeToStopPull;
    public void PlayPullSFX()
    {
        if (_isPullPlaying) return;
        _isPullPlaying = true;
        _isTimeToStopPull = false;
        StartCoroutine(PlaySequenceAnimationSFX(_pullPushClips, () => _isTimeToStopPull));
    }
    public void StopPullSFX()
    {
        _isTimeToStopPull = true;
        _isWalkPlaying = false;
        StopSequenceSFX();
    }
    public void PlayPullStartSFX()
    {
        PlayOneShootSFX(_startPullPushClips);
    }
    
    #endregion
    #region Jump
    public void PlayJumpSFX()
    {
        PlayOneShootSFX(_jumpClips);
    }

    public void PlayLandSFX()
    {
        PlayOneShootSFX(_landClips);
    }
    #endregion

    private void StopSequenceSFX()
    {
        _audioSource?.Stop();
        _audioSource.clip = null;
    }

    private IEnumerator PlaySequenceAnimationSFX(AudioClip[] clips, Func<bool> stop)
    {
        if (stop())
        {
            yield return null;
        }
        else
        {
            yield return new WaitUntil(()=> !_audioSource.isPlaying);
            var random = UnityEngine.Random.Range(0, clips.Length);
            _audioSource.clip = clips[random];
            _audioSource?.Play();
            StartCoroutine(PlaySequenceAnimationSFX(clips, stop));
        }
    }
    private void PlayOneShootSFX(AudioClip[] clips)
    {
        var random = UnityEngine.Random.Range(0, clips.Length);
        _audioSource.PlayOneShot(clips[random]);
    }
}
