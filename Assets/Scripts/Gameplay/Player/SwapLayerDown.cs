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

        private void Start()
        {
            EventConnector.Subscribe("OnSwapDownInput", new(OnDownInput));
            _enhanceMovement = GetComponent<PEnhanceMovement>();
        }

        private void OnDisable()
        {
            EventConnector.Unsubscribe("OnSwapDownInput", new(Dummy.VoidAction));
        }
        private void Update()
        {
            if (!_isWaitToLand ) return;
            //_isWaitToLand = !_enhanceMovement.IsGrounded && _enhanceMovement.Movement.y <= 0;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            
            if (collision.CompareTag("SwapLayerDown"))
            {
                // Only use trigger from below
                RaycastHit2D hit;
                hit = Physics2D.Raycast(_enhanceMovement.BottomPosition + new Vector2(0, 0.2f), Vector2.down, 0.4f, _swapDownLayer);
                if (hit.collider != null)
                {
                    
                    if (Checker.IsWithin(hit.collider, _enhanceMovement.BottomPosition))
                    {
                        _isPlaceToSwap = true;
                    }
                    else
                    {
                        _isPlaceToSwap = false;
                    }
                    return;
                }
                _isPlaceToSwap = false;
            }
        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (_isPlaceToSwap) return;
            if (_isWaitToLand) return;
            if (!collision.CompareTag("SwapLayerDown")) return;
            // Only use trigger from below
            RaycastHit2D hit;
            hit = Physics2D.Raycast(_enhanceMovement.BottomPosition + new Vector2(0, 0.2f), Vector2.down, 0.4f, _swapDownLayer);
            if (hit.collider != null)
            {

                if (Checker.IsWithin(hit.collider, _enhanceMovement.BottomPosition))
                {
                    _isPlaceToSwap = true;
                }
                else
                {
                    _isPlaceToSwap = false;
                }
                return;
            }
            _isPlaceToSwap = false;
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
            //_isPlaceToSwap = false;
            //_isWaitToLand = true;
            
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