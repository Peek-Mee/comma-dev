using UnityEngine;

namespace Comma.Gameplay.Environment
{
    public class RepeatParallaxer : MonoBehaviour
    {
        [SerializeField] private GameObject[] _backgroundsLoop;
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private float _choke;
        public float _speed;
        private Vector2 _screenBound;

        private void Start()
        {
            _screenBound = _mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, _mainCamera.transform.position.z));
        }
        private void LateUpdate()
        {
            foreach (GameObject obj in _backgroundsLoop)
            {
                RepositionBackground(obj);
            }
        }
        private void RepositionBackground(GameObject obj)
        {
            Transform[] bg = obj.GetComponentsInChildren<Transform>();
            if (bg.Length > 1)
            {
                GameObject first = bg[1].gameObject;
                GameObject last = bg[bg.Length - 1].gameObject;

                float lastExtent = last.GetComponent<SpriteRenderer>().bounds.extents.x - _choke;
                float firsExtent = first.GetComponent<SpriteRenderer>().bounds.extents.x - _choke;

                if (_mainCamera.transform.position.x + _screenBound.x > last.transform.position.x - lastExtent)
                {
                    // Reposition Background to right
                    first.transform.SetAsLastSibling();
                    first.transform.position = new Vector3(last.transform.position.x + lastExtent * 2, last.transform.position.y, last.transform.position.z);
                }
                else if (_mainCamera.transform.position.x - _screenBound.x < first.transform.position.x - firsExtent)
                {
                    // Reposition Background to left
                    last.transform.SetAsFirstSibling();
                    last.transform.position = new Vector3(first.transform.position.x - firsExtent * 2, first.transform.position.y, first.transform.position.z);
                }
            }
        }
    }
}