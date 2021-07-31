using UnityEngine;

namespace UnityCommons {
    public static partial class Extensions {
        public static void Log(this object value) {
            Debug.Log(value);
        }

        public static void LogWarning(this object value) {
            Debug.LogWarning(value);
        }

        public static void LogError(this object value) {
            Debug.LogError(value);
        }
        
        public static void Log(this object value, Object context) {
            Debug.Log(value, context);
        }

        public static void LogWarning(this object value, Object context) {
            Debug.LogWarning(value, context);
        }

        public static void LogError(this object value, Object context) {
            Debug.LogError(value, context);
        }
    }
}