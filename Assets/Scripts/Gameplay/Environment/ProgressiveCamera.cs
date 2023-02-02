using Cinemachine;
using Comma.Global.PubSub;
using Comma.Utility.Collections;
using UnityEngine;

namespace Comma.Gameplay.Environment
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class ProgressiveCamera : MonoBehaviour, IDebugger
    {
        private CinemachineVirtualCamera _camera;
        private float _defaultOrthoSize;
        private Vector2 _defaultOffset;
        private Transform _targetFollow;

        // Transition
        private bool _isTransition;
        private float _orthoSizeStart;
        private float _orthoSizeFinish;
        private float _distance;
        private float _xPositionStart;

        private void Awake()
        {
            _camera = GetComponent<CinemachineVirtualCamera>();
            _defaultOrthoSize = _camera.m_Lens.OrthographicSize;
            _targetFollow = _camera.m_Follow;
        }
        private void Start()
        {
            EventConnector.Subscribe("OnEnterCameraTrigger", new(EnterCameraTrigger));
            EventConnector.Subscribe("OnExitCameraTrigger", new(ExitCameraTrigger));
        }

        #region PubSub
        private void EnterCameraTrigger(object msg)
        {
            OnEnterCameraTrigger message = (OnEnterCameraTrigger)msg;
            _orthoSizeStart = _defaultOrthoSize * message.StartScale;
            _orthoSizeFinish= _defaultOrthoSize * message.FinishScale;
            _distance = message.Distance;
            _xPositionStart = _targetFollow.position.x;
            _isTransition = true;
        }
        private void ExitCameraTrigger(object msg)
        {
            _isTransition = false;
            float newRatio = Mathf.Clamp01(Mathf.Abs(_targetFollow.position.x - _xPositionStart) / _distance);
        }

        private void Update()
        {
            if (!CheckAvailability()) return;
            _camera.m_Lens.OrthographicSize = CalculateNewOrthoSize();
        }
        #endregion
        #region Calculation
        private bool CheckAvailability()
        {
            return _isTransition;
        }
        private float CalculateNewOrthoSize()
        {
            
            float currentPosition = _targetFollow.position.x;
            float newRatio = Mathf.Clamp01(Mathf.Abs(currentPosition - _xPositionStart) / _distance);
            float newOrthoSize = _orthoSizeStart + (newRatio * (_orthoSizeFinish - _orthoSizeStart));
            return newOrthoSize;
        }
        #endregion
        public string ToDebug()
        {
            float _current = _camera.m_Lens.OrthographicSize;
            float from = _isTransition ? _orthoSizeStart : _current;
            float to = _isTransition ? _orthoSizeFinish : _current;
            string returner = "\n<b>Progressive Camera</b>\n";
            returner += $"Transition: <i>{_isTransition}</i>\n";
            returner += $"Current: <i>{_camera.m_Lens.OrthographicSize}</i>\n";
            returner += $"From: <i>{from}</i>\n";
            returner += $"To: <i>{to}</i>\n";

            return returner;
        }

    }
}
