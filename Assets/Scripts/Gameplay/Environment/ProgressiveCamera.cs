using Cinemachine;
using Comma.Global.PubSub;
using Comma.Global.SaveLoad;
using Comma.Utility.Collections;
using UnityEngine;

namespace Comma.Gameplay.Environment
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class ProgressiveCamera : MonoBehaviour, IDebugger
    {
        private CinemachineVirtualCamera _camera;
        private float _defaultOrthoSize;
        private Transform _defaultFollowedPlayer;
        private CinemachineTransposer _cameraTransposer;
        private Vector3 _defaultOffset;

        // Transition
        private bool _isZoomTransition;
        private float _orthoSizeStart;
        private float _orthoSizeFinish;
        private float _zoomDistance;
        private float _zoomXStart;
        private LeanTweenType _zoomEasing;

        // Offset
        private bool _isOffsetTransition;
        private Vector2 _startOffset = new();
        private Vector2 _finishOffset = new();
        private float _offsetDistance;
        private float _offsetXStart;

        // Stop/Change Camera Follow
        private bool _isStopTransition;
        private Transform _newFollow;
        private bool _stopCamera;



        private void Awake()
        {
            _camera = GetComponent<CinemachineVirtualCamera>();
            _defaultOrthoSize = _camera.m_Lens.OrthographicSize;
            _defaultFollowedPlayer = _camera.m_Follow;
            _cameraTransposer = _camera.GetCinemachineComponent<CinemachineTransposer>();
            _defaultOffset = _cameraTransposer.m_FollowOffset;
        }
        private void Start()
        {
            //EventConnector.Subscribe("OnEnterCameraTrigger", new(EnterCameraTrigger));
            EventConnector.Subscribe("OnZoomCameraTrigger", new(EnterZoomCameraTrigger));
            EventConnector.Subscribe("OnOffsetCameraTrigger", new(EnterOffsetCameraTrigger));
            EventConnector.Subscribe("OnStopCameraMovement", new(EnterStopCameraTrigger));
            EventConnector.Subscribe("OnCameraTriggerExit", new(ExitCameraTrigger));
            //PlayerSaveData data = SaveSystem.GetPlayerData();
            //float savedScale;
            //if (data == null)
            //{
            //    savedScale = 1f;
            //}
            //else
            //{
            //    savedScale = data.GetCameraScale();
            //}
            //_camera.m_Lens.OrthographicSize = savedScale * _defaultOrthoSize;
        }
        private void OnDisable()
        {
            EventConnector.Unsubscribe("OnZoomCameraTrigger", Dummy.VoidAction);
            EventConnector.Unsubscribe("OnOffsetCameraTrigger", Dummy.VoidAction);
            EventConnector.Unsubscribe("OnStopCameraMovement", Dummy.VoidAction);
            EventConnector.Unsubscribe("OnCameraTriggerExit", Dummy.VoidAction);


        }
        #region PubSub
        //private void EnterCameraTrigger(object msg)
        //{
        //    OnEnterCameraTrigger message = (OnEnterCameraTrigger)msg;
        //    _orthoSizeStart = _defaultOrthoSize * message.StartScale;
        //    _orthoSizeFinish= _defaultOrthoSize * message.FinishScale;
        //    _distance = message.Distance;
        //    _xPositionStart = _targetFollow.position.x;
        //    _isTransition = true;
        //}
        private void EnterZoomCameraTrigger(object msg)
        {
            OnZoomCameraTrigger message = (OnZoomCameraTrigger)msg;
            _orthoSizeStart = _defaultOrthoSize * message.StartScale;
            _orthoSizeFinish = _defaultOrthoSize * message.FinishScale;
            _zoomDistance = message.Distance;
            _zoomXStart= _defaultFollowedPlayer.position.x;
            _zoomEasing = message.Easing;
            _isZoomTransition = true;
        }
        private void EnterOffsetCameraTrigger(object msg)
        {
            OnOffsetCameraTrigger message = (OnOffsetCameraTrigger)msg;
            _startOffset = message.StartOffset;
            _finishOffset = message.FinishOffset;
            _offsetDistance = message.Distance;
            _offsetXStart = _defaultFollowedPlayer.position.x;
            _isOffsetTransition = true;
        }
        private void EnterStopCameraTrigger(object msg)
        {
            OnStopCameraMoveTrigger message = (OnStopCameraMoveTrigger)msg;
            _newFollow = message.NewFollowObject;
            _stopCamera = message.StopCameraMovement;
            _isStopTransition = true;
        }
        private void ExitCameraTrigger(object msg)
        {
            OnCameraTriggerExit message = (OnCameraTriggerExit)msg;
            switch (message.CameraTriggerType)
            {
                case CameraTrigger.CameraTriggerType.Zoom:
                    _isZoomTransition = false;
                    break;
                case CameraTrigger.CameraTriggerType.Offset:
                    _isOffsetTransition = false;
                    break;
                case CameraTrigger.CameraTriggerType.Attachment:
                    _isStopTransition = true;
                    _stopCamera = false;
                    _newFollow = null;
                    break;
                default: 
                    
                    break;
            }
            //_isTransition = false;
            
            //float newRatio = Mathf.Clamp01(Mathf.Abs(_targetFollow.position.x - _xPositionStart) / _distance);
        }

        private void LateUpdate()
        {
            if (_isZoomTransition)
            {
                _camera.m_Lens.OrthographicSize = CalculateNewOrthoSize();

            }
            if (_isOffsetTransition)
            {
                if (_cameraTransposer != null)
                {
                    _cameraTransposer.m_FollowOffset = CalculateNewOffset();
                }
            }
            if (_isStopTransition)
            {
                if (!_stopCamera)
                {
                    _camera.Follow = _defaultFollowedPlayer;
                }
                else
                {
                    _camera.Follow = _newFollow;
                }
                _isStopTransition = false;
            }
        }
        #endregion
        #region Calculation
        private bool CheckAvailability()
        {
            return _isZoomTransition;
        }
        private float CalculateNewOrthoSize()
        {
            
            float currentPosition = _defaultFollowedPlayer.position.x;
            float newRatio = Mathf.Clamp01(Mathf.Abs(currentPosition - _zoomXStart) / _zoomDistance);
            //float ratioEasing = LeanTween.easeInCirc
            float newOrthoSize = _orthoSizeStart + (newRatio * (_orthoSizeFinish - _orthoSizeStart));
            return newOrthoSize;
        }
        private Vector3 CalculateNewOffset()
        {
            var result = _defaultOffset;
            var currentPos = _defaultFollowedPlayer.position.x;
            var newRatio = Mathf.Clamp01(Mathf.Abs(currentPos - _offsetXStart) / _offsetDistance);
            result.x = _startOffset.x + (newRatio * (_finishOffset.x - _startOffset.x));
            result.y = _startOffset.y + (newRatio * (_finishOffset.y - _startOffset.y));
            return result;

        }
        #endregion

        public float GetCurrentScale()
        {
            return _camera.m_Lens.OrthographicSize/_defaultOrthoSize;
        }
        public string ToDebug()
        {
            float _current = _camera.m_Lens.OrthographicSize;
            float from = _isZoomTransition ? _orthoSizeStart : _current;
            float to = _isZoomTransition ? _orthoSizeFinish : _current;
            string returner = "\n<b>Progressive Camera</b>\n";
            returner += $"Transition: <i>{_isZoomTransition}</i>\n";
            returner += $"Current: <i>{_camera.m_Lens.OrthographicSize}</i>\n";
            returner += $"From: <i>{from}</i>\n";
            returner += $"To: <i>{to}</i>\n";

            return returner;
        }

    }
}
