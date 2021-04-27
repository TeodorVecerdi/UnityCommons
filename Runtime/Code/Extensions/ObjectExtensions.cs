namespace UnityCommons {
    public static partial class Extensions {
        public static void Log(this object value) {
            UnityEngine.Debug.Log(value);
        }

        public static void LogWarning(this object value) {
            UnityEngine.Debug.LogWarning(value);
        }

        public static void LogError(this object value) {
            UnityEngine.Debug.LogError(value);
        }
    }
}