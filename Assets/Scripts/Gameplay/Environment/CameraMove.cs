using System;
using UnityEngine;

namespace Comma.Gameplay.Environment
{
    public class CameraMove : MonoBehaviour
    {
        [SerializeField] private float _speed;
        private void Update()
        {
            transform.position += Vector3.right * _speed * Time.deltaTime;
        }
    }
}