using Comma.Utility.Collections;
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
        [SerializeField] private List<string> _cutsceneCollection;
        [SerializeField] private float _lastCameraScale;
        [SerializeField] private bool _isNew;
        [SerializeField] private int _layer;
        [SerializeField] private StringVector3Dictionary _interactedObject;

        public PlayerSaveData()
        {
            _lastPosition = Vector3.zero;
            _orbsCollection = new();
            _orbsInHand = 0;
            _portalsCollection = new();
            _cutsceneCollection = new();
            _lastCameraScale= 1f;
            _isNew = true;
            _layer = 1;
            _interactedObject = new();
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
        /// Was portal already used?
        /// </summary>
        /// <param name="id">Portal ID</param>
        /// <returns>bool</returns>
        public bool IsPortalInCollection(string id) { return _portalsCollection.Contains(id); }
        
        /// <summary>
        /// Add played cutscene to collection
        /// </summary>
        /// <param name="id"></param>
        public void AddCutsceneToCollection(string id)
        {
            if (!IsCutsceneInCollection(id)) _cutsceneCollection.Add(id);
        }
        /// <summary>
        /// Check if cutscene was already played in the past
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsCutsceneInCollection(string id) { return _cutsceneCollection.Contains(id); }

        /// <summary>
        /// Set latest player camera scale
        /// </summary>
        /// <param name="scale"></param>
        public void SetCameraScale(float scale)
        {
            _lastCameraScale = scale;
        }
        /// <summary>
        /// Get the latest camera scale saved in the disk
        /// </summary>
        /// <returns></returns>
        public float GetCameraScale()
        {
            return _lastCameraScale;
        }
        public bool IsNewData()
        {
            return _isNew;
        }
        public void SetOldData()
        {
            _isNew = false;
        }
        public int GetCurrentLayer()
        {
            return _layer;
        }
        public void SetLastLayer(int layer)
        {
            _layer = layer;
        }

        public void SetInteractedObject(string id, Vector3 pos)
        {
            if (_interactedObject.ContainsKey(id))
            {
                _interactedObject[id] = pos;
            }
            else
            {
                _interactedObject.Add(id, pos);
            }
        }
        public bool IsObjectInteracted(string id)
        {
            return _interactedObject.ContainsKey(id);
        }
        public Vector3 GetObjectPosition(string id)
        {
            return _interactedObject[id];
        }
        public object Clone()
        {
            return (PlayerSaveData)this.MemberwiseClone();
        }
    }
}