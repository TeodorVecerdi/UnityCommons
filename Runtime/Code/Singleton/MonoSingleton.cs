using UnityEngine;

namespace UnityCommons {
	/// <summary>
	/// Creates a MonoBehaviour singleton of type <typeparamref name="T"/>. Ensures that only a single instance exists.
	/// </summary>
	/// <typeparam name="T">Component type</typeparam>
	public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T> {
		private static readonly System.Type type = typeof(T);
		private static T instance;
		public static bool IsInitialized => instance != null;
		
		public static T Instance {
			get {
				// Find first object of type T. Other instances are destroyed when Awake is called on them.
				#if UNITY_2020_1_OR_NEWER
				if (instance == null) instance = FindObjectOfType<T>(true);
				#else
				if (instance == null) {
					T[] objects = Resources.FindObjectsOfTypeAll<T>();
					if (objects.Length > 0) instance = objects[0];
				}
				#endif
				if (instance != null) return instance;

				// Create an object if cannot find an already existing one.
				Debug.LogWarning($"MonoSingleton<{type.Name}> could not be found!");
				return instance = null;
			}
		}
		
		private void Awake() {
			if (Instance != null && Instance != this) {
				Debug.LogError($"Cannot have multiple instances of {type.Name}. Destroying excess instances.");
				Destroy(this);
				return;
			}
			
			OnAwake();
		}
		
		protected virtual void OnAwake() {}
	}
}