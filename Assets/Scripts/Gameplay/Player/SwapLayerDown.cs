using Comma.Global.PubSub;
using UnityEngine;

namespace Comma.Gameplay.Player
{
    [RequireComponent(typeof(Collider2D))]
    public class SwapLayerDown : MonoBehaviour
    {
        private bool _isPlaceToSwap;

        private void Start()
        {
            EventConnector.Subscribe("OnPlayerMove", new(OnDownInput));
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("SwapLayerDown"))
            {
                _isPlaceToSwap = true;
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
            OnPlayerMove move = (OnPlayerMove)msg;
            if (move.Direction.y != -1) return;
            _isPlaceToSwap = false;
            EventConnector.Publish("OnPlayerSwapDown", new OnPlayerSwapDown());
        }
    }
}