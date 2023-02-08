using Cinemachine;
using Comma.Utility.Collections;
using UnityEngine;

namespace Comma.Gameplay.Environment
{
    //[RequireComponent(typeof(SpriteRenderer))]
    public class Parallaxer : MonoBehaviour, IDebugger
    {
        [Tooltip("Only Use three sprites")]
        [Header("General")]
        [SerializeField] private bool _loop = true;
        [SerializeField] private bool _persistent;
        [SerializeField] private float _minX;
        [SerializeField] private float _maxX;
        private bool _isStarted;

        [Header("Seemless Loop")]
        [SerializeField] private SpriteRenderer[] _sprites;
        private float _length;
        private Vector3 _farLeft;
        private Vector3 _farRight;
        private int _leftIndex;
        private int _rightIndex;

        [Header("Parallax")]
        [SerializeField] private bool _parallax = true;
        [SerializeField] private CinemachineVirtualCamera _cameraToFollow;
        private Transform _targetTransform;
        [SerializeField] private float _parallaxSpeed = .1f;

        // Keeping track
        private Vector3 _startPoint;
        private Vector3 _camStart;
        private bool _isInitiated;

        private void Awake()
        {
            _targetTransform = _cameraToFollow.transform;
            var screen = GetCoverableOrthoCamera();
            if (_targetTransform.position.x - screen.x > _maxX || _targetTransform.position.x - screen.x < _minX) return;

            InitPosition();
        }
        private void InitPosition()
        {
            _length = _sprites[0].bounds.size.x;
            _farLeft = -1.5f * _length * new Vector3(1, 0, 0);
            _farRight = 1.5f * _length * new Vector3(1, 0, 0);


            _leftIndex = 0;
            _rightIndex = 2;

            if (!_persistent)
            {
                Vector3 pos = transform.position;
                transform.position = new(
                    _targetTransform.position.x, pos.y, pos.z);
            }
            _camStart = _targetTransform.position;
            _startPoint = transform.position;
            _isStarted = true;
            _isInitiated= true;
        }
        private void Update()
        {
            var screen = GetCoverableOrthoCamera();
            if (!_isStarted)
            {
                if (_targetTransform.position.x - screen.x > _maxX || _targetTransform.position.x - screen.x < _minX) return;
                if (_isInitiated) _isStarted = true;
                else InitPosition();
            }

            if (_loop) CheckForOutOfBound(_targetTransform.position);

            // Check if player out of range to loop and parallax
            if (_targetTransform.position.x - screen.x > _maxX || _targetTransform.position.x - screen.x < _minX) _isStarted = false;

            // Do Parallax
            if (_parallax)
            {
                float ratio = (_targetTransform.position.x - _camStart.x) * _parallaxSpeed;
                Vector3 newPos = _startPoint;
                newPos.x -= ratio;
                transform.position = newPos;
            }
        }

        private int GetPreviousIndex(int currentIndex)
        {
            if (currentIndex == 0) return _sprites.Length - 1;
            return currentIndex - 1;
        }
        private int GetNextIndex(int currentIndex)
        {
            if (currentIndex == _sprites.Length - 1) return 0;
            return currentIndex + 1;
        }

        
        private void CheckForOutOfBound(Vector3 currentPosition)
        {
            Vector2 inside = GetCoverableOrthoCamera();
            // camera right side almost there...
            if (currentPosition.x + (inside.x/2f) >= transform.TransformPoint(_farRight).x * .80f)
            {
                int targetIndex = GetNextIndex(_rightIndex);

                _rightIndex = targetIndex;
                _leftIndex = GetNextIndex(_leftIndex);

                _farRight.x += _length;
                _farLeft.x += _length;

                Vector3 currentPos = _sprites[_rightIndex].transform.position;
                _sprites[_rightIndex].transform.position = new((3f * _length) + currentPos.x, currentPos.y);

            }
            // camera left side almost there...
            else if (currentPosition.x - (inside.x/2f) <= transform.TransformPoint(_farLeft).x * .80f)
            {
                int targetIndex = GetPreviousIndex(_leftIndex);

                _leftIndex = targetIndex;
                _rightIndex = GetPreviousIndex(_rightIndex);

                _farLeft.x -= _length;
                _farRight.x -= _length;

                Vector3 currentPos = _sprites[_leftIndex].transform.position;
                _sprites[_leftIndex].transform.position = new( currentPos.x - (3f * _length), currentPos.y);
            }
        }

        

        private Vector2 GetCoverableOrthoCamera()
        {
            float ratio = (float)Screen.width / Screen.height;
            float height = _cameraToFollow.m_Lens.OrthographicSize * 2f;
            float width = ratio * height;
            return new(width, height);

        }

        public string ToDebug()
        {
            string returner = "\n<b>Parallaxer</b>\n";
            returner += $"Coverable: <i>{_length}</i>\n";
            returner += $"Current: <i>{_cameraToFollow.transform.position}</i>\n";
            returner += $"Left Idx: <i>{_leftIndex}</i>\n";
            returner += $"Right Idx: <i>{_rightIndex}</i>\n";
            returner += $"Left Far: <i>{_farLeft}</i>\n";
            returner += $"Rigth Far: <i>{_farRight}</i>\n";

            return returner;
        }
    }


}
