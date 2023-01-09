using System;
using System.Collections;
using Comma.Gameplay.Player;
using Comma.Global.PubSub;
using UnityEngine;
using UnityEngine.Serialization;

namespace Comma.Gameplay.Environment
{
    [Serializable]
    public class CutSceneData
    {
        public string _cutSceneName;
        [SerializeField] private Vector2 _newPosition;
        [SerializeField] private Vector2 _targetPosition;
        [SerializeField] private float _speed;
        [SerializeField] private float _delayNewPosition;
        private bool _isNewPosition;
        
        [Header("Animations")]
        [SerializeField] private AnimationStates _animationStates;
        [SerializeField] private float _delayPlayAnimation;

        [Header("Remove?")]
        [SerializeField] private bool _canRemove;

        [SerializeField] private float _timeRemove;
        public Vector2 NewPosition => _newPosition;
        public Vector2 TargetPosition => _targetPosition;
        public float Speed => _speed;
        public float DelayNewPosition => _delayNewPosition;
        public AnimationStates AnimStates => _animationStates;
        public float DelayAnimation => _delayPlayAnimation;
        public bool IsNewPosition
        {
            get => _isNewPosition;
            set => _isNewPosition = value;
        }

        public bool CanRemove
        {
            get => _canRemove;
            set => _canRemove = value;
        }
        public float TimeRemove => _timeRemove;
        
        public CutSceneData()
        {
            _isNewPosition = true;
        }
    }
    public class TriggerCutScene : MonoBehaviour
    { 
        [SerializeField] private CutSceneData _onPlay,_onRemove;
        
        private void OnTriggerStay2D(Collider2D collider)
        {
            if(!collider.CompareTag("Player"))return;
            OnTriggerCutScene(_onPlay);
            StartCoroutine(RemoveCutScene(_onPlay));
        }

        private void OnTriggerCutScene(CutSceneData data)
        {
            EventConnector.Publish("OnCutSceneTrigger", new OnCutSceneTrigger(data));
        }

        private IEnumerator RemoveCutScene(CutSceneData data)
        {
            if(!data.CanRemove)yield break;
            yield return new WaitForSeconds(data.TimeRemove);
            gameObject.SetActive(false);
            data.CanRemove = false;
            OnTriggerCutScene(_onRemove);

        }
    }
}