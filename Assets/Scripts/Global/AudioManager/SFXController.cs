using UnityEngine;
using System.Collections;
using System;

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
        PlayWalkSFX();
        //PlayRunSFX();
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
        if(Time.time - _lastPlayWalk > _walkPlayRate)
        {
            _lastPlayWalk = Time.time;
            PlaySFX("Walk");
        }
    }
    public void PlayRunSFX()
    {
        if (Time.time - _lastPlayRun > _runPlayRate)
        {
            _lastPlayRun = Time.time;
            PlaySFX("Run");
        }
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
