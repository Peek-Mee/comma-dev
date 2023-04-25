using Comma.Global.PubSub;
using Comma.Utility.Collections;
using UnityEngine;

namespace Comma.Gameplay.Player
{
    [RequireComponent(typeof(Collider2D))]
    public class SwapLayerDown : MonoBehaviour
    {
        private bool _isPlaceToSwap = false;

        private void Start()
        {
            EventConnector.Subscribe("OnSwapDownInput", new(OnDownInput));
        }

        private void OnDisable()
        {
            EventConnector.Unsubscribe("OnSwapDownInput", new(Dummy.VoidAction));
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            
            if (collision.CompareTag("SwapLayerDown"))
            {
                // Only use trigger from below
                if (collision.transform.position.y >= transform.position.y) return;
                _isPlaceToSwap = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("SwapLayerDown"))
            {
                // Only use trigger from below
                if (collision.transform.position.y >= transform.position.y) return;
                _isPlaceToSwap = false;
            }
        }
        private void OnDownInput(object msg)
        {
            if (!_isPlaceToSwap) return;
            //OnPlayerMove move = (OnPlayerMove)msg;
            //if (move.Direction.y != -1) return;
            _isPlaceToSwap = false;
            EventConnector.Publish("OnPlayerSwapDown", new OnPlayerSwapDown());
        }
    }
}