using System.Collections.Generic;
using UnityEngine;

namespace UnityCommons {
    public abstract class ObjectPool<T> : MonoBehaviour {
        [Header("Pool Settings")]
        [SerializeField, Tooltip("The number of items initially allocated to the pool.")] private int initialPoolSize = 16;
        [SerializeField, Tooltip("Represents the factor by which the pool grows in size. For example, a growth factor of 2.0 means that the pool doubles in size once it runs out of elements")] private float growthFactor = 1.5f;
        
        private Queue<T> pool;
        private int size;
        private bool initialized;
        
        private void Awake() {
            pool = new Queue<T>();
            size = 0;
            initialized = false;
        }
        
        /// <summary>
        /// Returns a new instance of type <typeparamref name="T"/>.
        /// </summary>
        protected abstract T CreatePooled();
        
        /// <summary>
        /// Called when an item is requested from the pool.
        /// </summary>
        protected virtual void OnGet(T pooledItem) {
        }

        /// <summary>
        /// Called when an item is returned back to the pool.
        /// </summary>
        protected virtual void OnReturn(T pooledItem) {
        }
        
        /// <summary>
        /// Gets an item from the pool.
        /// </summary>
        public T Get() {
            if (!initialized) {
                Allocate(initialPoolSize);
                initialized = true;
            }
            
            if (pool.Count == 0) {
                Grow();
            }

            T pooledItem = pool.Dequeue();
            OnGet(pooledItem);
            return pooledItem;
        }

        /// <summary>
        /// Returns <paramref name="pooledItem"/> to the pool.
        /// </summary>
        public void Return(T pooledItem) {
            OnReturn(pooledItem);
            pool.Enqueue(pooledItem);
        }
        
        private void Allocate(int amount) {
            size += amount;
            for (int i = 0; i < amount; i++) {
                pool.Enqueue(CreatePooled());
            }
        }

        private void Grow() {
            if (growthFactor <= 1.0f) growthFactor = 1.5f;
            int newItems = Mathf.CeilToInt(size * growthFactor) - size;
            Allocate(newItems);
        }
    }
}