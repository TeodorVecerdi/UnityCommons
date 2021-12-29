namespace UnityCommons.Runtime {
    public static partial class Singleton {
        public abstract class Create<T> where T : Create<T> {
            private static T instance;
            public static T Instance => instance ??= MakeInstance();
            public static bool IsInitialized => instance != null;

            protected abstract T CreateInstance();
            
            private static T MakeInstance() {
                return ((T)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(T))).CreateInstance();
            }
        }
    }
}