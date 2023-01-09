using System;
using System.Collections;
using System.Collections.Generic;
using Comma.Gameplay.Environment;
using Comma.Global.PubSub;
using UnityEngine;

namespace Comma.Gameplay.Player
{
    public class PlayerTriggerCS : MonoBehaviour
    {
        [SerializeField] private Animator _playerAnimator;
        [SerializeField] private Animator _transitionAnimator;
        [SerializeField] private GameObject _transitionPanel;
        private void Start()
        {
            EventConnector.Subscribe("OnCutSceneTrigger",new(OnCutScene));
        }

        private void OnCutScene(object msg)
        {
            OnCutSceneTrigger m = (OnCutSceneTrigger)msg;
            CutSceneData data = m.Data;

            StartCoroutine(PlayAnimations(data));
            StartCoroutine(PlayerNewPosition(data));
            PlayerTargetPosition(data);
            
            Debug.Log("On Cut Scene " + data._cutSceneName);
        }

        private IEnumerator PlayerNewPosition(CutSceneData data)
        {
            if (data.NewPosition == Vector2.zero) yield break;
            if (data.IsNewPosition)
            {
                yield return new WaitForSeconds(data.DelayNewPosition);
                transform.position = data.NewPosition;
                data.IsNewPosition = false;
            }
        }

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
            yield return new WaitForSeconds(data.DelayNewPosition);
        }

        private IEnumerator PlayAnimations(CutSceneData data)
        {
            yield return new WaitForSeconds(data.DelayAnimation);
            var state = data.AnimStates;
            switch (state)
            {
                case AnimationStates.Idle:
                    _playerAnimator.SetBool("Idle",true);
                    _playerAnimator.SetBool("GetUp",false);
                    _transitionPanel.SetActive(false);
                    break;
                case AnimationStates.Walk:
                    break;
                case AnimationStates.Run:
                    break;
                case AnimationStates.Fall:
                    break;
                case AnimationStates.GetUp:
                    _playerAnimator.SetBool("GetUp",true);
                    break;
                case AnimationStates.Transition:
                    _transitionPanel.SetActive(true);
                    _playerAnimator.SetBool("GetUp",false);
                    break;
            }
        }
    }
}