using Cinemachine;
using Comma.Global.PubSub;
using System.Collections;
using UnityEngine;

namespace Comma.Prolog.CameraProlog
{
    public class CameraPrologController: MonoBehaviour
    {
        [SerializeField] private float _time;
        [SerializeField] private CinemachineVirtualCamera _camera;
        private CinemachineTransposer _transposer;

        private void Start()
        {
            EventConnector.Subscribe("OnCameraPrologTrigger", CameraProlog);
            _transposer = _camera.GetCinemachineComponent<CinemachineTransposer>();
        }
        private void CameraProlog(object msg)
        {
            OnCameraPrologTrigger m = (OnCameraPrologTrigger)msg;
            CameraPrologData data = m.Data;
            _transposer.m_FollowOffset = Vector3.MoveTowards(_transposer.m_FollowOffset, data.Offset, _time*Time.deltaTime);

        }
    }
}