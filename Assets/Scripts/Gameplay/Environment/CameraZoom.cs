using System;
using UnityEngine;

namespace Comma.Gameplay.Environment
{
    [System.Serializable]
    public struct ZoomTrigger
    {
        [SerializeField] private float _minPosition;
        [SerializeField] private float _maxPosition;
        [SerializeField] private float _targetZoom;
        [SerializeField] private bool _isGrounded;

        public float MinPosition => _minPosition;
        public float MaxPosition => _maxPosition;
        public bool IsGround => _isGrounded;
        public float Zoom => _targetZoom;
    }
    public class CameraZoom : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private GameObject _player;
        
        [SerializeField] private float _normalZoom;
        [SerializeField] private float _minZoom;
        [SerializeField] private float _maxZoom;
        
        [SerializeField] private ZoomTrigger[] _zoomTriggers;
        [SerializeField] private float _zoomSpeed;

        private void Update()
        {
            CheckZoom();
        }

        private void CheckZoom()
        {
            var playerPosition = _player.transform.position;
            foreach (var zoomTrigger in _zoomTriggers)
            {
                var isGrounded = zoomTrigger.IsGround;
                var minPosition = zoomTrigger.MinPosition;
                var maxPosition = zoomTrigger.MaxPosition;
                
                if (playerPosition.x >= minPosition && playerPosition.x < maxPosition && isGrounded)
                {
                    if(_camera.orthographicSize == zoomTrigger.Zoom)return;
                    _camera.orthographicSize = Mathf.MoveTowards(_camera.orthographicSize, zoomTrigger.Zoom, _zoomSpeed * Time.deltaTime);
                    Debug.Log("Zoom in Ground");
                }
                else if (playerPosition.x >= minPosition && playerPosition.x < maxPosition && !isGrounded)
                {
                    if(_camera.orthographicSize == zoomTrigger.Zoom)return;
                   _camera.orthographicSize = Mathf.MoveTowards(_camera.orthographicSize, zoomTrigger.Zoom, _zoomSpeed * Time.deltaTime);
                    Debug.Log("Zoom in Under Ground");
                }
                else
                {
                    if(_camera.orthographicSize == _normalZoom)return;
                   _camera.orthographicSize = Mathf.MoveTowards(_camera.orthographicSize, _normalZoom, _zoomSpeed * Time.deltaTime);
                   Debug.Log("Zoom Normal");
                }
            }
        }
    }
}