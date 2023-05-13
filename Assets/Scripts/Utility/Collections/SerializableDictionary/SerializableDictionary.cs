using System;
using System.Collections.Generic;
using UnityEngine;

namespace Comma.Utility.Collections
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> :
        Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] private List<TKey> keys = new();
        [SerializeField] private List<TValue> values = new();

        public void OnAfterDeserialize()
        {
            this.Clear();
            if (keys.Count != values.Count)
            {
                throw new Exception
                    ($"there are {keys.Count} keys and {values.Count} values after deserialization.");

            }
            for (int i = 0; i < keys.Count; i++)
            {
                this.Add(keys[i], values[i]);
            }
        }

        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();
            foreach(KeyValuePair<TKey, TValue> pair in this)
            {
                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
        }
    }
}
