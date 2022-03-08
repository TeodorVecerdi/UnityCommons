using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityCommons {
    [Serializable]
    public class SerializedDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver {
        [SerializeField] private List<TKey> keys;
        [SerializeField] private List<TValue> values;

        public void OnBeforeSerialize() {
            keys = new List<TKey>(Count);
            values = new List<TValue>(Count);
            foreach (KeyValuePair<TKey, TValue> pair in this) {
                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
        }

        public void OnAfterDeserialize() {
            Clear();

            if (keys == null || values == null) return;

            if (keys.Count != values.Count) {
                throw new Exception($"there are {keys.Count} keys and {values.Count} values after deserialization. Make sure that both key and value types are serializable.");
            }

            for (int i = 0; i < keys.Count; i++) {
                Add(keys[i], values[i]);
            }
        }
    }
}