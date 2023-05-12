using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Comma.Gameplay.Environment
{
    public class PlatformTriggerArea : MonoBehaviour
    {
        public GroundButton _groundButton;
        public Animator actuator;
        

        private void OnEnable()
        {
            _groundButton.OnGroundButtonPush += EnableTrigger;
        }

        private void OnDisable()
        {
            _groundButton.OnGroundButtonPush -= EnableTrigger;
        }

        private void EnableTrigger(bool state)
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = state;
            gameObject.GetComponent<SpriteRenderer>().enabled = state;
            Debug.Log(state);

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            actuator.enabled = true;
        }
    }

}
