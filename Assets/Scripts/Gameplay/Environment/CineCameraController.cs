using Cinemachine;
using Comma.Global.PubSub;
using Comma.Utility.Collections;
using System;
using UnityEngine;

namespace Comma.Gameplay.Environment
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CineCameraController : MonoBehaviour, IDebugger
    {
        private CinemachineVirtualCamera _camera;
        private float _defaultScale;
        private Vector3 _offset;
        private int _playerDirection;
        private int _scaleDirection;

        // transition
        private bool _isTransition;
        private float _currentSize;
        private float _targetSize;
        private float _zoomSpeed;


        private void Awake()
        {
            _camera= GetComponent<CinemachineVirtualCamera>();
            _defaultScale = _camera.m_Lens.OrthographicSize;
            _currentSize = _defaultScale;
            _offset = _camera.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset;
        }
        private void Start()
        {
            EventConnector.Subscribe("OnPlayerMove", new(OnMoveInput));
            EventConnector.Subscribe("OnCameraChangeTrigger", new(OnCameraChange));
        }
        #region PubSub
        private void OnMoveInput(object msg)
        {
            OnPlayerMove message = (OnPlayerMove)msg;
            _playerDirection = (int)message.Direction.x;
        }
        private void OnCameraChange(object msg)
        {
            OnCameraChangeTrigger message = (OnCameraChangeTrigger)msg;
            CameraChangeData data = message.Data;
            if (data.IsNull) return;

            _targetSize = data.ScaleFactor * _defaultScale;
            _scaleDirection = _targetSize > 0 ? 1 : -1; 
            _zoomSpeed= data.ZoomSpeed;
            _isTransition= true;
        }

        #endregion

        #region Transition Handler
        private void Update()
        {
            if (!_isTransition) return;
            if (_playerDirection == 0) return;
            if (Mathf.Abs(_targetSize - _currentSize) < .1f)
            {
                _isTransition = false;
                return;
            }
            _camera.m_Lens.OrthographicSize +=  _scaleDirection * _playerDirection * _zoomSpeed * Time.deltaTime;
            _currentSize = _camera.m_Lens.OrthographicSize;

            //_camera.m_Lens.OrthographicSize = Mathf.Lerp(_camera.m_Lens.OrthographicSize,
            //    _targetSize, _zoomSpeed);
        }
        private void HandleTransitionCamera(float newSize)
        {
            _camera.m_Lens.OrthographicSize = newSize;
        }
        #endregion

        public string ToDebug()
        {
            string returner = "\n<b>CineCamera Controller</b>\n";
            returner += $"Transition: <i>{_isTransition}</i>\n";
            returner += $"Current: <i>{_currentSize}</i>\n";
            returner += $"Speed: <i>{_zoomSpeed}</i>\n";
            returner += $"Target: <i>{_targetSize}</i>\n";

            return returner;
        }
    }
}
