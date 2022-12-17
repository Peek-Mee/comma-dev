using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Comma.Gameplay.Environment
{
    public class ScrollParallaxer : MonoBehaviour
    {
        private float _startpos;
        [SerializeField] private GameObject mainCamera;
        [SerializeField] private float parallexEffect;
        private float _speed;
        private void Start()
        {
            _startpos = transform.position.x;
        }

        private void FixedUpdate()
        {
            ScrollBackground();
        }

        private void ScrollBackground()
        {
            // Speed convert from parallax effect
            _speed = parallexEffect * -1f;
            
            // Move background
            float dist = (mainCamera.transform.position.x * _speed);
            transform.position = new Vector3(_startpos + dist, transform.position.y, transform.position.z);
        }
    }
}


