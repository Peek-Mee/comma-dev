using Comma.Gameplay.DetectableObject;
using Comma.Global.AudioManager;
using Comma.Global.PubSub;
using System.Collections;
using UnityEngine;

namespace Comma.Gameplay.Player
{
    [RequireComponent(typeof(Collider2D))]
    public class OrbDetection : MonoBehaviour
    {
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Orb"))
            {
                IDetectable coll = collision.gameObject.GetComponent<IDetectable>();
                coll?.Interact();
                SFXController.Instance.PlayObtainOrbSFX();
            }
        }

        

    }
}