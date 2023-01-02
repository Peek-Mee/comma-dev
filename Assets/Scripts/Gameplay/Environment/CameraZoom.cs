using System;
using UnityEngine;

namespace Comma.Gameplay.Environment
{
    [System.Serializable]
    public struct ZoomTrigger
    {
        [SerializeField] private Vector2 _minPosition;
        [SerializeField] private Vector2 _maxPosition;
        [SerializeField] private float _targetZoom;

        public Vector2 MinPosition => _minPosition;
        public Vector2 MaxPosition => _maxPosition;
        public float Zoom => _targetZoom;
    }
    public class CameraZoom : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private GameObject _player;
        
        [SerializeField] private float _normalZoom;

        [SerializeField] private ZoomTrigger[] _zoomTriggers;
        [SerializeField] private float _zoomSpeed;

        private void Start()
        {
            //var body = vCam.GetCinemachineComponent<CinemachineFramingTransposer>();
        }
        
        private void Update()
        {
            CheckZoom();
        }

        private void CheckZoom()
        {
            var xPlayerPosition = _player.transform.position.x;
            var yPlayerPosition = _player.transform.position.y;
            
            foreach (var zoomTrigger in _zoomTriggers)
            {
                var xMin = zoomTrigger.MinPosition.x;
                var xMax = zoomTrigger.MaxPosition.x;
                var yMin = zoomTrigger.MinPosition.y;
                var yMax = zoomTrigger.MaxPosition.y;
                
                
                if (xPlayerPosition >= xMin && xPlayerPosition <= xMax && yPlayerPosition >= yMin && yPlayerPosition <= yMax)
                {
                    if(_camera.orthographicSize == zoomTrigger.Zoom)return;
                    _camera.orthographicSize = Mathf.MoveTowards(_camera.orthographicSize, zoomTrigger.Zoom, _zoomSpeed * Time.deltaTime);
                    return;
                }
            }
            if(_camera.orthographicSize == _normalZoom)return;
            _camera.orthographicSize = Mathf.MoveTowards(_camera.orthographicSize, _normalZoom, _zoomSpeed * Time.deltaTime);
        }
    }
}