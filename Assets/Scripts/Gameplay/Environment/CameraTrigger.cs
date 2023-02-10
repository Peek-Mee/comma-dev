using Comma.Global.PubSub;
using UnityEngine;

namespace Comma.Gameplay.Environment
{
    [RequireComponent(typeof(Collider2D))]
    public class CameraTrigger : MonoBehaviour
    {
        [SerializeField] private bool _rightToLeft;
        [SerializeField] private float _startZoomScale;
        [SerializeField] private float _finishZoomScale;
        [SerializeField] private Vector2 _startOffset;
        [SerializeField] private Vector2 _finishOffset;

        private Collider2D _coll;
        private float _distance;
        private void Awake()
        {
            _coll = GetComponent<Collider2D>();
            _distance = _coll.bounds.size.x;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            float playerSizeX = other.bounds.size.x/2f;
            bool playerFromLeft = IsPlayerFromLeft(other);
            OnEnterCameraTrigger val;
            _ = _rightToLeft == playerFromLeft ? val = new(_rightToLeft ? 1 : -1, _startZoomScale, _finishZoomScale, _startOffset, _finishOffset, _distance + playerSizeX) :
                val = new(_rightToLeft ? 1: -1, _finishZoomScale, _startZoomScale, _finishOffset, _startOffset, _distance + playerSizeX);

            EventConnector.Publish("OnEnterCameraTrigger", val);
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            EventConnector.Publish("OnExitCameraTrigger", new OnExitCameraTrigger());
        }

        private bool IsPlayerFromLeft(Collider2D target)
        {
            Vector2 normal = target.transform.position - transform.position;

            return normal.x > 0 ? true : false;
        }
    }

}
