namespace UnityCommons.Runtime {
    public static partial class Singleton {
        public abstract class New<T> where T : New<T>, new() {
            private static T instance;
            public static T Instance => instance ??= new T();
            public static bool IsInitialized => instance != null;
        }
    }
}