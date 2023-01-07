using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Comma.Gameplay.Player
{
    public enum AnimationState
    {
        Idle,
        Walk,
        Run,
        Fall,
        GetUp,
        Transition,
    }
    [System.Serializable]
    public class TriggerCutscene
    {
        [SerializeField] private string _cutsceneName;
        [Header("Trigger")]
        [SerializeField] private Vector2 _minCutscenePosition;
        [SerializeField] private Vector2 _maxCutscenePosition;
        
        [Header("Target")]
        [SerializeField] private  Vector2  _newPosition;
        [SerializeField] private  Vector2 _targetPosition;
        [SerializeField] private float _speed;
        [SerializeField] private int _gravity;

        [Header("Condition")]
        [SerializeField] private float _delayNewPosition;
        [SerializeField] private float _delayRemove;
        [SerializeField] private bool _isRemove;
        private bool _isChangePosition;
        private bool _isNewPosition;
        
        [Header("Animation")]
        [SerializeField] private Animator _animator;
        [SerializeField] private AnimationState _animationState;

        public string Name => _cutsceneName;
        public Vector2 MinCutscenePosition
        {
            get { return _minCutscenePosition; }
            set { _minCutscenePosition = value; }
        }

        public Vector2 MaxCutscenePosition
        {
            get { return _maxCutscenePosition; }
            set { _maxCutscenePosition = value; }
        }
        public Vector2 NewPosition => _newPosition;
        public Vector2 TargetPosition => _targetPosition;
        public float Speed => _speed;
        public int Gravity => _gravity;
        public float DelayNewPosition => _delayNewPosition;
        public float DelayRemove => _delayRemove;
        public bool IsRemove => _isRemove;
        public bool IsNewPosition
        {
            get => _isNewPosition;
            set => _isNewPosition = value;
        }
        public bool IsChangePosition
        {
            get => _isChangePosition;
            set => _isChangePosition = value;
        }
        public Animator Animator => _animator;
        public AnimationState AnimationState => _animationState;
        public TriggerCutscene()
        {
            _isChangePosition = false;
            _isNewPosition = true;
        }
    }
    public class PlayerCutScene : MonoBehaviour
    {
        [SerializeField] private GameObject _transitionPanel;
        [SerializeField] private List<TriggerCutscene> _cutscenes;
        private Vector2 _currentPosition;
        private bool _isCutscene;

        private void Update()
        {
            GetCurrentPosition();
            
            PlayCutScene();
        }
        private void GetCurrentPosition()
        {
            if(_isCutscene)return;
            _currentPosition = transform.position;
        }
        private void PlayCutScene()
        {
            foreach (TriggerCutscene cut in _cutscenes)
            {
                var cutScene = cut;
                var xMin = cut.MinCutscenePosition.x;
                var xMax = cut.MaxCutscenePosition.x;
                var yMin = cut.MinCutscenePosition.y;
                var yMax = cut.MaxCutscenePosition.y;
                var newPos = cut.NewPosition;
                var targetPos = cut.TargetPosition;
                if (transform.position.x >= xMin && transform.position.x < xMax && transform.position.y >= yMin && transform.position.y < yMax)
                {
                    _isCutscene = true;
                    
                    StartCoroutine(WaitNewPosition(cut.DelayNewPosition,cutScene));
                    StartCoroutine(WaitRemoveCutScene(cut.DelayRemove,cutScene));
                    
                    PlayAnimations(cut.AnimationState,cut.Animator);
                    
                    NewPosition(cutScene,newPos);
                    TargetPosition(cutScene,targetPos);
                    
                    GetComponent<Rigidbody2D>().gravityScale = cut.Gravity;
                    
                    Debug.Log("Cutscene " + cut.Name);
                    return;
                }
            }
            GetComponent<Rigidbody2D>().gravityScale = 1;
            _isCutscene = false;
            Debug.Log("No Cutscene");
        }
        private void NewPosition(TriggerCutscene cut,Vector2 position)
        {
            if(position == Vector2.zero)return;
            if(cut.IsNewPosition)
            {
                if(!cut.IsChangePosition)return;
                transform.position = position;
                cut.IsNewPosition = false;
                Debug.Log("New Position");
            }
        }
        private void TargetPosition(TriggerCutscene cut,Vector2 target)
        {
            if(target == Vector2.zero)return;
            if(!cut.IsChangePosition)return;
            transform.position = Vector2.MoveTowards(transform.position, target, cut.Speed * Time.deltaTime);
            Debug.Log("Target");
        }
        private IEnumerator WaitNewPosition(float delay, TriggerCutscene cut)
        {
            yield return new WaitForSeconds(delay);
            cut.IsChangePosition = true;
        }
        private IEnumerator WaitRemoveCutScene(float delay, TriggerCutscene cut)
        {
            yield return new WaitForSeconds(delay);
            if (cut.IsRemove)
            {
                RemoveAnimation(cut.AnimationState,cut.Animator);
                _cutscenes.Remove(cut);
                Debug.Log("Remove Cutscene");
            }
        }
        private void PlayAnimations(AnimationState state,Animator anim)
        {
            switch (state)
            {
                case AnimationState.Idle:
                    break;
                case AnimationState.Walk:
                    break;
                case AnimationState.Run:
                    break;
                case AnimationState.Fall:
                    break;
                case AnimationState.GetUp:
                    anim.SetBool("GetUp",true);
                    break;
                case AnimationState.Transition:
                    _transitionPanel.SetActive(true);
                    anim.SetBool("GetUp",false);
                    break;
            }
        }
        private void RemoveAnimation(AnimationState state,Animator anim)
        {
            switch (state)
            {
                case AnimationState.Idle:
                    break;
                case AnimationState.Walk:
                    break;
                case AnimationState.Run:
                    break;
                case AnimationState.Fall:
                    break;
                case AnimationState.GetUp:
                    anim.SetBool("GetUp",false);
                    break;
                case AnimationState.Transition:
                    _transitionPanel.SetActive(false);
                    anim.SetBool("GetUp",false);
                    break;
            }
        }
    }
}