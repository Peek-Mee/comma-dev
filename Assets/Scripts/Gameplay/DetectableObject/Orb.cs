using Comma.Global.SaveLoad;
using UnityEngine;

namespace Comma.Gameplay.DetectableObject
{
    public class Orb : MonoBehaviour, IDetectable
    {
        [SerializeField] private string _orbId;

        private void Awake()
        {
            bool isCollected = SaveSystem.GetPlayerData().IsOrbInCollection(_orbId);
            if (!isCollected) return;

            gameObject.SetActive(false);
            
        }

        private void PlayerInteraction()
        {
            SaveSystem.GetPlayerData().AddOrbToCollection(_orbId);
            SaveSystem.SaveDataToDisk();
            gameObject.SetActive(false);
        }

        public string GetObjectId()
        {
            return _orbId;
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public void Interact()
        {
            PlayerInteraction();
        }
    }
}