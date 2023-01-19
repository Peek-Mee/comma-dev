using Comma.Global.PubSub;
using System;
using UnityEngine;

namespace Comma.Gameplay.Environment
{
    [Serializable]
    public struct CameraChangeData
    {
        [SerializeField] private float _scaleFactor;
        [SerializeField] private float _transitionTime;
        [SerializeField] private Vector2 _cameraOffset;
        [SerializeField] private bool _isNull;

        public float ScaleFactor => _scaleFactor;
        public float TransitionTime => _transitionTime;
        public Vector2 Offset => _cameraOffset;
        public bool IsNull => _isNull;
    }

    [RequireComponent(typeof(Collider2D))]
    public class CameraChangeTrigger : MonoBehaviour
    {
        [SerializeField] private CameraChangeData _onEnter;
        [SerializeField] private CameraChangeData _onExit;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag("Player")) return;
            OnTriggerCamera(_onEnter);
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!collision.CompareTag("Player")) return;
            OnTriggerCamera(_onExit);
        }

        private void OnTriggerCamera(CameraChangeData data)
        {
            EventConnector.Publish("OnCameraChangeTrigger", new OnCameraChangeTrigger(data));
        }
    }
}
