//using Cinemachine;
//using Comma.Global.PubSub;
//using System;
//using UnityEngine;

//namespace Comma.Gameplay.Environment
//{
    
//    public class CameraZoom : MonoBehaviour
//    {
//        private void Start()
//        {
//            EventConnector.Subscribe("OnCameraChangeTrigger", new(OnCameraChangeTrigger));
            
//        }
//        private void Awake()
//        {
//            _defaultScale = _cineCamera.m_Lens.OrthographicSize;
//            _composer = _cineCamera.GetCinemachineComponent<CinemachineComposer>();
//        }

//        #region
//        [SerializeField] private CinemachineVirtualCamera _cineCamera;
//        private float _defaultScale;
//        private CinemachineComposer _composer;
//        private void OnCameraChangeTrigger(object msg)
//        {
//            OnCameraChangeTrigger m = (OnCameraChangeTrigger)msg;
//            CameraChangeData message = m.Data;
//            if (message.IsNull) return;
//            var newOrtoSize = _defaultScale * message.ScaleFactor;
//            var lastOrtoSize = _cineCamera.m_Lens.OrthographicSize;

//            LeanTween.value(lastOrtoSize, newOrtoSize, message.TransitionTime).setOnUpdate((float val) =>
//            {
//                _cineCamera.m_Lens.OrthographicSize =  val;
//            });

//            //_cineCamera.Ge
//            //_composer.m_TrackedObjectOffset = message.Offset;

//        }
//        #endregion
       
//    }
//}