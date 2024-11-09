using Comma.Global.PubSub;
using Comma.Utility.Collections;
using System.Collections;
using UnityEngine;

namespace Comma.Gameplay.Player
{
    [RequireComponent(typeof(Collider2D))]
    public class SwapLayerDown : MonoBehaviour
    {
        private bool _isPlaceToSwap = false;
        private bool _isWaitToLand = false;
        private PEnhanceMovement _enhanceMovement;
        [SerializeField] private LayerMask _swapDownLayer;
        private readonly Vector2 _calibrator = new(0, .2f);

        private void Start()
        {
            EventConnector.Subscribe("OnSwapDownInput", new(OnDownInput));
            _enhanceMovement = GetComponent<PEnhanceMovement>();
        }

        private void OnDisable()
        {
            EventConnector.Unsubscribe("OnSwapDownInput", new(Dummy.VoidAction));
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            
            if (collision.CompareTag("SwapLayerDown"))
            {
                if (Checker.IsWithin(collision, _enhanceMovement.BottomPosition + _calibrator))
                {
                    _isPlaceToSwap = true;
                }
                else
                {
                    _isPlaceToSwap = false;
                }
            }
        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (_isPlaceToSwap) return;
            if (_isWaitToLand) return;
            if (!collision.CompareTag("SwapLayerDown")) return;
            if (Checker.IsWithin(collision, _enhanceMovement.BottomPosition + _calibrator))
            {
                _isPlaceToSwap = true;
            }
            else
            {
                _isPlaceToSwap = false;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("SwapLayerDown"))
            {
                _isPlaceToSwap = false;
                
            }
        }
        private void OnDownInput(object msg)
        {
            if (!_isPlaceToSwap) return;
            if (_isWaitToLand) return;
            
            StartCoroutine(WaitToLand());
        }
        IEnumerator WaitToLand()
        {
            EventConnector.Publish("OnPlayerSwapDown", new OnPlayerSwapDown());
            _isPlaceToSwap = false;
            _isWaitToLand = true;
            yield return new WaitUntil(() => !_enhanceMovement.IsSwapDown);
            _isWaitToLand = false;
        }
    }
}