using UnityEngine;

namespace UnityCommons {
	public abstract class MonoSingleton<T> : MonoBehaviour where T : Component {
		private static T instance;
		public static T Instance {
			get {
				// Find first object of type T. Other instances are destroyed when Awake is called on them.
				if (instance == null) instance = FindObjectOfType<T>();
				if (instance != null) return instance;
				
				// Create an object if cannot find an already existing one.
				var go = new GameObject($"MonoSingleton<{typeof(T).Name}>") { hideFlags = HideFlags.HideAndDontSave };
				go.SetActive(true);
				instance = go.AddComponent<T>();
				return instance;
			}
		}
		
		protected virtual void Awake() {
			if (Instance != null && Instance != this) {
				Debug.LogError($"Cannot have multiple instances of {typeof(T).Name}. Destroying excess instances.");
				Destroy(this);
			}
		}
	}
}