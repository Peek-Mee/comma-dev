using System;
using System.Collections;
using Comma.Gameplay.Player;
using Comma.Global.PubSub;
using UnityEngine;

namespace Comma.Gameplay.Environment
{
    public enum AnimationStates
    {
        Idle,
        Walk,
        Run,
        Falling,
        GetUp,
        Transition
    }
    [Serializable]
    public class CutSceneData
    {
        public string CutSceneName;
        public bool IsCutScene { get; private set; }
        [Header("Transform")]
        public Vector2 NewPosition;
        public Vector2 TargetPosition;
        public float Speed;
        public bool IsNewPosition { get; private set; }
        //public float DelayNewPosition;
        
        [Header("Animations")]
        public AnimationStates AnimationStates;
        public float DelayAnimation;

        [Header("Remove?")]
        public bool CanRemove;
        public float TimeRemove;
        public CutSceneData()
        {
            IsNewPosition = true;
            IsCutScene = true;
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