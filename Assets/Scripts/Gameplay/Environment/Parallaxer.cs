using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Comma.Gameplay.Environment
{
    public class Parallaxer : MonoBehaviour
    {
        private float _length, _startpos;
        [SerializeField] private GameObject mainCamera;
        [SerializeField] private float parallexEffect;
        private float _speed;
        private void Start()
        {
            _startpos = transform.position.x;
            _length = GetComponent<SpriteRenderer>().bounds.size.x;
        }

        private void FixedUpdate()
        {
            ScrollBackground();
        }

        private void ScrollBackground()
        {
            _speed = parallexEffect * -1f;
            float temp = (mainCamera.transform.position.x * (1 - _speed));
            float dist = (mainCamera.transform.position.x * _speed);
            
            transform.position = new Vector3(_startpos + dist, transform.position.y, transform.position.z);
            
            if (temp > _startpos + _length) _startpos += _length;
            else if (temp < _startpos - _length) _startpos -= _length;
        }
    }
}


