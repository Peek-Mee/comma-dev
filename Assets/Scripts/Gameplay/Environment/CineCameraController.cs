using Cinemachine;
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
        private float _isTransition;
        private float _currentScale;


        private void Awake()
        {
            _camera= GetComponent<CinemachineVirtualCamera>();
            _defaultScale = _camera.m_Lens.OrthographicSize;
            _currentScale = _defaultScale;
            _offset = _camera.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset;
        }

        #region Transition Handler
        private void HandleTransitionCamera(float newSize)
        {
            _camera.m_Lens.OrthographicSize = newSize;
        }
        #endregion

        public string ToDebug()
        {
            string returner = "<b>CineCamera Controller</b>\n";
            returner += $"Transition: <color=\"red\">{_isTransition}</color>\n";
            returner += $"Scale: <color=\"red\">{_currentScale / _defaultScale}</color>\n";

            return returner;
        }
    }
}
