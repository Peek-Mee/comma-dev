using Comma.Global.PubSub;
using Comma.Global.SaveLoad;
using UnityEngine;

namespace Comma.Gameplay.DetectableObject
{
    [RequireComponent(typeof(Collider2D))]
    public class PortalActivation : MonoBehaviour
    {
        [SerializeField] private string _portalIdToTrigger;

        private void Awake()
        {
            PlayerSaveData save = SaveSystem.GetPlayerData();
            bool isActive = save.IsPortalInCollection(_portalIdToTrigger);
            if (isActive) gameObject.SetActive(false);
            
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                EventConnector.Publish("OnNearPortal", new OnPlayerNearPortal(_portalIdToTrigger));
            }
        }
    }
}
