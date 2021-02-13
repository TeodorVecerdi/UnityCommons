namespace UnityCommons {
	/// <summary>
	/// Creates a MonoBehaviour singleton of type <typeparamref name="T"/>. Ensures that only a single instance exists.
	/// </summary>
	/// <typeparam name="T">Component type</typeparam>
	public abstract class MonoSingleton<T> : UnityEngine.MonoBehaviour where T : UnityEngine.Component {
		private static readonly System.Type type = typeof(T);
		private static T instance;
		public static T Instance {
			get {
				// Find first object of type T. Other instances are destroyed when Awake is called on them.
				if (instance == null) instance = FindObjectOfType<T>(true);
				if (instance != null) return instance;
				
				// Create an object if cannot find an already existing one.
				return instance = new UnityEngine.GameObject($"MonoSingleton<{type.Name}>", typeof(T)).GetComponent<T>();
			}
		}
		
		protected virtual void Awake() {
			if (Instance != null && Instance != this) {
				UnityEngine.Debug.LogError($"Cannot have multiple instances of {type.Name}. Destroying excess instances.");
				Destroy(this);
			}
		}
	}
}