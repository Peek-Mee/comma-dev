using Comma.Gameplay.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Comma.CutScene
{
    [RequireComponent(typeof(Collider2D))]
    public class DisableEnableJump : MonoBehaviour
    {
        [SerializeField] private string _connectedCutsceneTrigger;


        private void Awake()
        {
            
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                PlayerMovement player = collision.GetComponent<PlayerMovement>();
                player.IsJumpProhibited = true;
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                PlayerMovement player = collision.GetComponent<PlayerMovement>();
                player.IsJumpProhibited = false;
            }
        }
    }

}
