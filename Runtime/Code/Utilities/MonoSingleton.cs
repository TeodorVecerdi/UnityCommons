namespace UnityCommons {
    internal interface IMonoSingleton {}
	/// <summary>
	/// Creates a MonoBehaviour singleton of type <typeparamref name="T"/>. Ensures that only a single instance exists.
	/// </summary>
	/// <typeparam name="T">Component type</typeparam>
    public abstract class MonoSingleton<T> : UnityEngine.MonoBehaviour, IMonoSingleton where T : MonoSingleton<T> {
		private static readonly System.Type type = typeof(T);
		private static T instance;
		public static T Instance {
			get {
				// Find first object of type T. Other instances are destroyed when Awake is called on them.
				#if UNITY_2020_1_OR_NEWER
				if (instance == null) instance = FindObjectOfType<T>(true);
				#else
				if (instance == null) {
					var objects = UnityEngine.Resources.FindObjectsOfTypeAll<T>();
					if (objects.Length > 0) instance = objects[0];
				}
				#endif
				if (instance != null) return instance;

				// Create an object if cannot find an already existing one.
				return instance = new UnityEngine.GameObject($"MonoSingleton<{type.Name}>", type).GetComponent<T>();
			}
		}
		
		private void Awake() {
			if (Instance != null && Instance != this) {
				UnityEngine.Debug.LogError($"Cannot have multiple instances of {type.Name}. Destroying excess instances.");
				Destroy(this);
				return;
			}
			
			OnAwake();
		}
		
		protected virtual void OnAwake() {}
	}
}