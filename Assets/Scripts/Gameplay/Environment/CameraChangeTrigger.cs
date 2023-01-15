using Comma.Global.PubSub;
using System;
using UnityEngine;

namespace Comma.Gameplay.Environment
{
    [Serializable]
    public struct CameraChangeData
    {
        [SerializeField] private float _scaleFactor;
        [SerializeField] private float _zoomSpeed;
        [SerializeField] private Vector2 _cameraOffset;
        [SerializeField] private bool _isNull;

        public float ScaleFactor => _scaleFactor;
        public float ZoomSpeed => _zoomSpeed;
        public Vector2 Offset => _cameraOffset;
        public bool IsNull => _isNull;

        public CameraChangeData(float factor, float speed, Vector2 offset, bool isNull)
        {
            _scaleFactor = factor;
            _zoomSpeed = speed;
            _cameraOffset = offset;
            _isNull = isNull;
        }
    }

    [RequireComponent(typeof(Collider2D))]
    public class CameraChangeTrigger : MonoBehaviour
    {
        [SerializeField] private CameraChangeData _onEnter;
        [SerializeField] private CameraChangeData _onExit;
        [SerializeField] private bool _fromLeft;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag("Player")) return;
            Vector2 normal = collision.transform.position - transform.position;
            bool isFromLeft = normal.x > 0 ? true : false;
            float directedScale = _fromLeft == isFromLeft ? _onEnter.ScaleFactor : _onEnter.ScaleFactor * -1;


            CameraChangeData newData = new(
                directedScale, _onEnter.ZoomSpeed, _onEnter.Offset, false);
            OnTriggerCamera(newData);
        }
        //private void OnTriggerExit2D(Collider2D collision)
        //{
        //    if (!collision.CompareTag("Player")) return;
        //    OnTriggerCamera(_onExit);
        //}

        private void OnTriggerCamera(CameraChangeData data)
        {
            EventConnector.Publish("OnCameraChangeTrigger", new OnCameraChangeTrigger(data));
        }
    }
}
