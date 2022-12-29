using Comma.Global.PubSub;
using Comma.Global.SaveLoad;
using System;
using System.Collections;
using UnityEngine;

namespace Comma.Gameplay.DetectableObject
{
    [Serializable]
    public struct PortalDestination
    {
        [SerializeField] private int orbNeeded;
        [SerializeField] private Portal secondaryPortal;

        public int OrbNeeded => orbNeeded;
        public Portal SecondaryPortal=> secondaryPortal;
        
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

            if (player.GetOrbsInHand() >= _destinations[0].OrbNeeded)
            {
                player.SubmitOrb();
                player.AddPortalToCollections(_portalId);
                player.AddPortalToCollections(_destinations[0].SecondaryPortal.GetObjectId());
                SaveSystem.SaveDataToDisk();
                isActivated = true;
                _portalSprite.SetActive(true);
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