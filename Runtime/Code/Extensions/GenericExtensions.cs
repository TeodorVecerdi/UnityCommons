namespace UnityCommons {
    public static partial class Extensions {
        public static void Log<T>(this T value) {
            UnityEngine.Debug.Log(value);
        }

        public static void LogWarning<T>(this T value) {
            UnityEngine.Debug.LogWarning(value);
        }

        public static void LogError<T>(this T value) {
            UnityEngine.Debug.LogError(value);
        }
    }
}