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
    public void PlayWalkSFX()
    {
        //_walkAudioSource.Play();
        _runAudioSource.Stop();
        _runAudioSource.volume = 0;
        _runAudioSource.loop = false;
        _walkAudioSource.loop = true;
        _walkAudioSource.volume = 1;
        // if(Time.time - _lastPlayWalk > _walkPlayRate)
        // {
        //     _lastPlayWalk = Time.time;
        //     //PlaySFX("Walk");
        // }
    }
    public void RemoveSFX(string audioName)
    {
        _walkAudioSource.loop = false;
        _runAudioSource.loop = false;
        _walkAudioSource.volume = 0;
        _runAudioSource.volume = 0;
        // SFXClip audio = Array.Find(_sfxClips, sfx => sfx.Name == audioName);
        // if (audio == null)
        // {
        //     Debug.LogWarning($"SFX: <color=red> {name} </color> not found!");
        //     return;
        // }
    }
    public void PlayRunSFX()
    {
        //_runAudioSource.Play();
        _walkAudioSource.Stop();
        _walkAudioSource.volume = 0;
        
        _walkAudioSource.loop = false;
        _runAudioSource.loop = true;
        _runAudioSource.volume = 1;
        // RemoveSFX("Walk");
        // if (Time.time - _lastPlayRun > _runPlayRate)
        // {
        //     _lastPlayRun = Time.time;
        //     PlaySFX("Run");
        // }
        //_lastPlayRun = 0;
    }
    IEnumerator WalkSFX()
    {
        while(_isWalk)
        {
            _isWalk = false;
            PlaySFX("Walk");
            yield return new WaitForSeconds(_walkPlayRate);
            _isWalk = true;
        }
        
    }
}
}

