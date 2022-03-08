using System.Collections.Generic;
using UnityEngine;

namespace UnityCommons {
    public static class MetadataExtensions {
        private static readonly Dictionary<int, Metadata> metadataCache = new Dictionary<int, Metadata>();

        public static Metadata GetMetadata(this GameObject gameObject) {
            int instanceId = gameObject.GetInstanceID();
            if (metadataCache.ContainsKey(instanceId)) {
                return metadataCache[instanceId];
            }

            Metadata metadata = gameObject.GetComponent<Metadata>();
            if (metadata == null) {
                metadata = gameObject.AddComponent<Metadata>();
            }

            metadataCache.Add(instanceId, metadata);
            return metadata;
        }

        public static Metadata GetMetadata(this Component component) {
            return component.gameObject.GetMetadata();
        }

        public static bool HasMetadata(this GameObject gameObject, string key) {
            return gameObject.GetMetadata().Has(key);
        }

        public static bool HasMetadata(this Component component, string key) {
            return component.gameObject.GetMetadata().Has(key);
        }

        public static bool HasMetadata<T>(this GameObject gameObject, string key) {
            return gameObject.GetMetadata().Has<T>(key);
        }

        public static bool HasMetadata<T>(this Component component, string key) {
            return component.gameObject.GetMetadata().Has<T>(key);
        }

        public static T GetMetadata<T>(this GameObject gameObject, string key) {
            return gameObject.GetMetadata().Get<T>(key);
        }

        public static T GetMetadata<T>(this Component component, string key) {
            return component.gameObject.GetMetadata().Get<T>(key);
        }

        public static bool TryGetMetadata<T>(this GameObject gameObject, string key, out T value) {
            return gameObject.GetMetadata().TryGet(key, out value);
        }

        public static bool TryGetMetadata<T>(this Component component, string key, out T value) {
            return component.gameObject.GetMetadata().TryGet(key, out value);
        }

        public static void RemoveMetadata(this GameObject gameObject, string key) {
            gameObject.GetMetadata().Remove(key);
        }

        public static void RemoveMetadata(this Component component, string key) {
            component.gameObject.GetMetadata().Remove(key);
        }

        public static bool TryRemoveMetadata(this GameObject gameObject, string key) {
            return gameObject.GetMetadata().TryRemove(key);
        }

        public static bool TryRemoveMetadata(this Component component, string key) {
            return component.gameObject.GetMetadata().TryRemove(key);
        }

        public static void SetMetadata<T>(this GameObject gameObject, string key, T value) {
            gameObject.GetMetadata().Set(key, value);
        }

        public static void SetMetadata<T>(this Component component, string key, T value) {
            component.gameObject.GetMetadata().Set(key, value);
        }

        public static bool TrySetMetadata<T>(this GameObject gameObject, string key, T value) {
            return gameObject.GetMetadata().TrySet(key, value);
        }

        public static bool TrySetMetadata<T>(this Component component, string key, T value) {
            return component.gameObject.GetMetadata().TrySet(key, value);
        }
    }
}