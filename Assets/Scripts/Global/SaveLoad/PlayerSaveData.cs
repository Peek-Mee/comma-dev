using System;
using System.Collections.Generic;
using UnityEngine;

namespace Comma.Global.SaveLoad
{
    [Serializable]
    public class PlayerSaveData : ICloneable
    {
        [SerializeField] private Vector3 _lastPosition;
        [SerializeField] private List<string> _orbsCollection;
        [SerializeField] private int _orbsInHand;
        [SerializeField] private List<string> _portalsCollection;

        public PlayerSaveData()
        {
            _lastPosition = Vector3.zero;
            _orbsCollection = new();
            _orbsInHand = 0;
            _portalsCollection = new();
        }
        /// <summary>
        /// Get the last player position saved in the disk
        /// </summary>
        /// <returns>Vector3</returns>
        public Vector3 GetLastPosition() { return _lastPosition; }
        /// <summary>
        /// Set a new position as player last position. This function only set the data dirty
        /// </summary>
        /// <param name="position">New position to be saved</param>
        public void SetLastPosition(Vector3 position) { _lastPosition = position; }
        /// <summary>
        /// Add collected orb to the collection. 
        /// </summary>
        /// <param name="id">Orb ID</param>
        public void AddOrbToCollection(string id)
        {
            if (!IsOrbInCollection(id))
            {
                _orbsCollection.Add(id);
                _orbsInHand++;
            }

        }
        /// <summary>
        /// Is orb ID in the collection?
        /// </summary>
        /// <param name="id">Orb ID</param>
        /// <returns>bool</returns>
        public bool IsOrbInCollection(string id) { return _orbsCollection.Contains(id); }
        /// <summary>
        /// Get current number of orbs in player hand/inventory
        /// </summary>
        /// <returns>int</returns>
        public int GetOrbsInHand() { return _orbsInHand; }
        /// <summary>
        /// Hand all orbs in hand to the portal
        /// </summary>
        public void SubmitOrb() { _orbsInHand = 0; }
        /// <summary>
        /// Add used portal to the collection
        /// </summary>
        /// <param name="id"></param>
        public void AddPortalToCollections(string id)
        {
            if (!IsPortalInCollection(id)) _portalsCollection.Add(id);
        }
        /// <summary>
        /// Is portal was already used?
        /// </summary>
        /// <param name="id">Portal ID</param>
        /// <returns>bool</returns>
        public bool IsPortalInCollection(string id) { return _portalsCollection.Contains(id); }

        public object Clone()
        {
            return (PlayerSaveData)this.MemberwiseClone();
        }
    }
}