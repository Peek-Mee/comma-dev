using Comma.Global.PubSub;
using Comma.Global.SaveLoad;
using System;
using UnityEngine;

namespace Comma.Gameplay.DetectableObject
{
    [Serializable]
    public struct PortalDestination
    {
        
	[SerializeField] private int _orbNeeded;
    [SerializeField] private Portal _secondaryPortal;
	
	public int OrbNeeded => _orbNeeded;
	public Portal SecondaryPortal => _secondaryPortal;

        public PortalDestination(int orbNum, Portal portal)
        {
            _orbNeeded = orbNum;
            _secondaryPortal = portal;
        }

    }
    [RequireComponent(typeof(Collider2D))]
    public class Portal : MonoBehaviour, IDetectable
    {
        [SerializeField] private string _portalId;
        //[SerializeField] private bool _isMainPortal;
        [SerializeField] private PortalDestination[] _destinations;
        [SerializeField] private GameObject _portalSprite;
        private bool isActivated;

        private void Awake()
        {
            isActivated = SaveSystem.GetPlayerData().IsPortalInCollection(_portalId);

            if (isActivated)
            {
                _portalSprite.SetActive(true);
            }
            else
            {
                _portalSprite.SetActive(false);
            }
        }

        private void TeleportPlayer()
        {
            EventConnector.Publish("OnPlayerUsePortal", 
                new OnPlayerUsePortal(_destinations[0].SecondaryPortal.GetPosition()));

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag("Player") || isActivated) return;

            PlayerSaveData player = SaveSystem.GetPlayerData();

            if (player.GetOrbsInHand() == _destinations[0].OrbNeeded)
            {
                player.SubmitOrb();
                player.AddPortalToCollections(_portalId);
                player.AddPortalToCollections(_destinations[0].SecondaryPortal.GetObjectId());
                SaveSystem.SaveDataToDisk();
            }
        }

        public string GetObjectId()
        {
            return _portalId;
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public void Interact()
        {
            if (!isActivated) return;

            TeleportPlayer();
        }
    }
}