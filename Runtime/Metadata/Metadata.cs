using UnityEngine;

namespace UnityCommons {
    public class Metadata : MonoBehaviour {
        [SerializeField, HideInInspector] private SerializedDictionary<string, object> metadata = new SerializedDictionary<string, object>();

        public void Remove(string key) {
            metadata.Remove(key);
        }

        public bool TryRemove(string key) {
            return metadata.Remove(key);
        }

        public void Clear() {
            metadata.Clear();
        }

        public bool Has(string key) {
            return metadata.ContainsKey(key);
        }

        public bool Has<T>(string key) {
            return metadata.ContainsKey(key) && metadata[key] is T;
        }

        public void Set<T>(string key, T value) {
            metadata[key] = value;
        }

        public bool TrySet<T>(string key, T value) {
            if (metadata.ContainsKey(key)) {
                return false;
            }

            metadata[key] = value;
            return true;
        }

        public T Get<T>(string key) {
            return (T) metadata[key];
        }

        public bool TryGet<T>(string key, out T value) {
            if (!metadata.ContainsKey(key) || !(metadata[key] is T)) {
                value = default;
                return false;
            }

            value = (T) metadata[key];
            return true;
        }
    }
}