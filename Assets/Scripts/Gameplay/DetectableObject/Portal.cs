using Comma.Global.PubSub;
using Comma.Global.SaveLoad;
using JetBrains.Annotations;
using System;
using UnityEngine;

namespace Comma.Gameplay.DetectableObject
{
    [Serializable]
    public struct PortalDestination
    {
        [SerializeField] private int orbNeeded;
        [SerializeField] private Portal connectedPortal;

        public int OrbNeeded => orbNeeded;
        public Portal ConnectedPortal=> connectedPortal;
        
    }
    [RequireComponent(typeof(Collider2D))]
    public class Portal : MonoBehaviour, IDetectable
    {
        [SerializeField] private string _portalId;
        [SerializeField] private bool _isMainPortal;
        [SerializeField] private PortalDestination[] _destinations;
        [SerializeField] private SpriteRenderer _portalSprite;
        //[SerializeField] private GameObject _portalSprite;
        private bool isActivated;
        private Collider2D _coll;

        private void Awake()
        {
            isActivated = SaveSystem.GetPlayerData().IsPortalInCollection(_portalId);
            if (!_isMainPortal) return;
            _coll = gameObject.GetComponent<Collider2D>();
            if (!isActivated)
            {
                //gameObject.SetActive(false);
                _portalSprite.enabled = false;
                _coll.enabled = false;
            }
            EventConnector.Subscribe("OnNearPortal", new(OnPortalTrigger));
        }

        #region PubSub
        private void OnPortalTrigger(object msg)
        {
            OnPlayerNearPortal message = (OnPlayerNearPortal)msg;
            if (message.Portal == _portalId)
            {

                PlayerSaveData player = SaveSystem.GetPlayerData();

                if (player.GetOrbsInHand() >= _destinations[0].OrbNeeded)
                {
                    player.SubmitOrb();
                    player.AddPortalToCollections(_portalId);
                    player.AddPortalToCollections(_destinations[0].ConnectedPortal.GetObjectId());
                    SaveSystem.SaveDataToDisk();
                    isActivated = true;
                    _portalSprite.enabled = true;
                    _coll.enabled = true;
                }
            }
            
        }
        #endregion
        private void TeleportPlayer()
        {
            EventConnector.Publish("OnPlayerUsePortal", 
                new OnPlayerUsePortal(_destinations[0].ConnectedPortal.GetPosition()));

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
            
            if (!isActivated && _isMainPortal) return;

            TeleportPlayer();
        }
    }
}