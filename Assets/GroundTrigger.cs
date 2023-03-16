using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Comma.Gameplay.Environment
{
    public class GroundTrigger : MonoBehaviour
    {
        public bool isTriggered = false;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.name == "Button")
            {
                isTriggered = true;
            }
        }
        
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.name == "Button")
            {
                isTriggered = false;
            }
        }
    }

}
