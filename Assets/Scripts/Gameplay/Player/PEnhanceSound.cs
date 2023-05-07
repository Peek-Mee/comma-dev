using System;
using System.Collections;
using System.Collections.Generic;
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
    private IEnumerator PlaySequentialEmitter(AudioClip[] clips, Func<bool> stopCondition)
    {
        yield return null;
        while (!stopCondition())
        {

        }
    }
}
