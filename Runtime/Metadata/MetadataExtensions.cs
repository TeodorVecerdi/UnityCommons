using System.Collections.Generic;
using UnityEngine;

namespace UnityCommons {
    public static class MetadataExtensions {
        private static readonly Dictionary<int, Metadata> metadataCache = new Dictionary<int, Metadata>();

        public static Metadata GetMetadataComponent(this GameObject gameObject) {
            int instanceId = gameObject.GetInstanceID();

            if (metadataCache.ContainsKey(instanceId)) {
                Metadata metadataComponent = metadataCache[instanceId];
                if (metadataComponent == null) {
                    metadataComponent = gameObject.AddComponent<Metadata>();
                    metadataCache[instanceId] = metadataComponent;
                }

                return metadataComponent;
            }

            Metadata metadata = gameObject.GetComponent<Metadata>();
            if (metadata == null) {
                metadata = gameObject.AddComponent<Metadata>();
            }

            metadataCache.Add(instanceId, metadata);
            return metadata;
        }

        public static Metadata GetMetadataComponent(this Component component) {
            return component.gameObject.GetMetadataComponent();
        }

        public static void RemoveMetadata(this GameObject gameObject, string key) {
            gameObject.GetMetadataComponent().Remove(key);
        }

        public static void RemoveMetadata(this Component component, string key) {
            component.gameObject.GetMetadataComponent().Remove(key);
        }

        public static bool TryRemoveMetadata(this GameObject gameObject, string key) {
            return gameObject.GetMetadataComponent().TryRemove(key);
        }

        public static bool TryRemoveMetadata(this Component component, string key) {
            return component.gameObject.GetMetadataComponent().TryRemove(key);
        }

        public static void ClearMetadata(this GameObject gameObject) {
            gameObject.GetMetadataComponent().Clear();
        }

        public static void ClearMetadata(this Component component) {
            component.gameObject.GetMetadataComponent().Clear();
        }

        public static bool HasMetadata(this GameObject gameObject, string key) {
            return gameObject.GetMetadataComponent().Has(key);
        }

        public static bool HasMetadata(this Component component, string key) {
            return component.gameObject.GetMetadataComponent().Has(key);
        }

        public static bool HasMetadata<T>(this GameObject gameObject, string key) {
            return gameObject.GetMetadataComponent().Has<T>(key);
        }

        public static bool HasMetadata<T>(this Component component, string key) {
            return component.gameObject.GetMetadataComponent().Has<T>(key);
        }

        public static T GetMetadata<T>(this GameObject gameObject, string key) {
            return gameObject.GetMetadataComponent().Get<T>(key);
        }

        public static T GetMetadata<T>(this Component component, string key) {
            return component.gameObject.GetMetadataComponent().Get<T>(key);
        }

        public static bool TryGetMetadata<T>(this GameObject gameObject, string key, out T value) {
            return gameObject.GetMetadataComponent().TryGet(key, out value);
        }

        public static bool TryGetMetadata<T>(this Component component, string key, out T value) {
            return component.gameObject.GetMetadataComponent().TryGet(key, out value);
        }

        public static void SetMetadata<T>(this GameObject gameObject, string key, T value) {
            gameObject.GetMetadataComponent().Set(key, value);
        }

        public static void SetMetadata<T>(this Component component, string key, T value) {
            component.gameObject.GetMetadataComponent().Set(key, value);
        }

        public static bool TrySetMetadata<T>(this GameObject gameObject, string key, T value) {
            return gameObject.GetMetadataComponent().TrySet(key, value);
        }

        public static bool TrySetMetadata<T>(this Component component, string key, T value) {
            return component.gameObject.GetMetadataComponent().TrySet(key, value);
        }
    }
}