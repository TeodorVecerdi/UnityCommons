using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityCommons.Event {
    public class EventQueue<T> {
        public static EventQueue<T> Instance => EventQueue.For<T>();
    }
    public static class EventQueue {
        public static EventQueue<T> For<T>() => EventQueueManager.Instance.GetOrCreate_EventType<T>();
        public static T Of<T>() => EventQueueManager.Instance.GetOrCreate_QueueType<T>();
        
        private class EventQueueManager : MonoSingleton<EventQueueManager> {
            private readonly Dictionary<Type, object> queues = new Dictionary<Type, object>();
            private static readonly Type genericQueueType = typeof(EventQueue<>);

            public EventQueue<T> GetOrCreate_EventType<T>() {
                var t = typeof(T);
                if (queues.ContainsKey(t)) return (EventQueue<T>)queues[t];
                return (EventQueue<T>) (queues[t] = new EventQueue<T>());
            }
            
            public T GetOrCreate_QueueType<T>() {
                var t = typeof(T);
                Type eventQueueType;
                if (t.IsGenericType && t.GetGenericTypeDefinition() == genericQueueType) {
                    eventQueueType = t.GenericTypeArguments[0];
                } else if (t.BaseType is {IsGenericType: true} && t.BaseType.GetGenericTypeDefinition() == genericQueueType) {
                    eventQueueType = t.BaseType.GenericTypeArguments[0];
                } else {
                    Debug.Assert(false, "Type specified is not an EventQueue type");
                    return default(T);
                }
                
                
                if (queues.ContainsKey(eventQueueType)) return (T)queues[eventQueueType];
                
                return (T) (queues[eventQueueType] = Activator.CreateInstance<T>());
            }
        }
    }
    
}