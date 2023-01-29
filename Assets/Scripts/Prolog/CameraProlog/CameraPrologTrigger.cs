using Cinemachine;
using Comma.Global.PubSub;
using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Comma.Prolog.CameraProlog
{
    [Serializable]
    public class CameraPrologData
    {
        public Vector3 Offset;
        public float Size;
    }
    public class CameraPrologTrigger : MonoBehaviour
    {
        [SerializeField] private CameraPrologData _cameraPrologData;

        private void Start()
        {
            _cameraPrologData.Size = GetComponent<SpriteRenderer>().bounds.extents.x;
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                EventConnector.Publish("OnCameraPrologTrigger", new OnCameraPrologTrigger(_cameraPrologData));
            }
        }
    }
}