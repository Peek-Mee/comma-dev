using System;
using System.Collections;
using System.Collections.Generic;
using Comma.Gameplay.Environment;
using Comma.Global.PubSub;
using UnityEngine;

namespace Comma.Gameplay.Player
{
    public class CutSceneDetection : MonoBehaviour
    {
        [SerializeField] private Animator _playerAnimator;
        [SerializeField] private GameObject _transitionPanel;

        private bool _isCutScene;
        [Header("Rigidbody")]
        [SerializeField] private float _fallMultiplier = 4f;
        private Rigidbody2D _rb;
        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            EventConnector.Subscribe("OnCutSceneTrigger",new(OnCutScene));
        }
        private void Update()
        {
            
        }
        private void OnCutScene(object msg)
        {
            OnCutSceneTrigger m = (OnCutSceneTrigger)msg;
            CutSceneData data = m.Data;
            
            _isCutScene = true;
            
            StartCoroutine(PlayAnimations(data));
            PlayerFalling(data);
            
            //StartCoroutine(PlayerNewPosition(data));
            //PlayerTargetPosition(data);
            
            Debug.Log("On Cut Scene " + data.CutSceneName);
        }
        private void PlayerFalling(CutSceneData data)
        {
            if (data.AnimationStates != AnimationStates.Falling) return;
            Vector2 gravity = -Physics2D.gravity;
            _rb.velocity -= _fallMultiplier * Time.deltaTime * gravity;
        }
        //private IEnumerator PlayerNewPosition(CutSceneData data)
        //{
        //    if (data.NewPosition == Vector2.zero) yield break;
        //    if (data.IsNewPosition)
        //    {
        //        yield return new WaitForSeconds(data.DelayNewPosition);
        //        transform.position = data.NewPosition;
        //        data.IsNewPosition = false;
        //    }
        //}

        private void PlayerTargetPosition(CutSceneData data)
        {
            var current = transform.position;
            var target = data.TargetPosition;
            var speed = data.Speed * Time.deltaTime;
            if(target == Vector2.zero)return;
            transform.position = Vector2.MoveTowards(current, target, speed);
        }
        private IEnumerator RemoveCutScene(CutSceneData data)
        {
            yield return new WaitForSeconds(0);
        }

        private IEnumerator PlayAnimations(CutSceneData data)
        {
            yield return new WaitForSeconds(data.DelayAnimation);
            var state = data.AnimationStates;
            switch (state)
            {
                case AnimationStates.Idle:
                    break;
                case AnimationStates.Walk:
                    break;
                case AnimationStates.Run:
                    break;
                case AnimationStates.Falling: 
                    _playerAnimator.SetBool("Fall_Rotation", true);
                    _playerAnimator.SetBool("Fall_Idle", false);
                    break;
                case AnimationStates.GetUp:
                    break;
                case AnimationStates.Transition:
                    if (_transitionPanel != null) { _transitionPanel.SetActive(true);}
                    break;
            }
        }
    }
}