using Comma.Global.SaveLoad;
using System.Collections;
using UnityEngine;

namespace Comma.Gameplay.Player
{
    public class Spawn : MonoBehaviour
    {
        
        private void Start()
        {
            Vector3 lastPost = SaveSystem.GetPlayerData().GetLastPosition();
            if (lastPost != Vector3.zero && lastPost != null)
            {
                transform.position = lastPost;
            }
        }
    }
}