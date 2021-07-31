using System;
using UnityEngine;


namespace UnityCommons.Event {
    public enum YeetType {
        Yeet,
        GigaYeet,
        YeetGod
    }

    public class SomeEventQueue : EventQueue<YeetType> {
    }
    
    public class TestingEvent : MonoBehaviour {
        private void Start() {
            var inst = SomeEventQueue.Instance;
            Debug.Log(inst.GetType());
            
            EventQueue<YeetType> yeetEventQueue = EventQueue<YeetType>.Instance;
            EventQueue<YeetType> megaEventQueue = EventQueue.For<YeetType>();
            EventQueue<YeetType> gigaEventQueue = EventQueue.Of<EventQueue<YeetType>>();
            SomeEventQueue godEventQueue = EventQueue.Of<SomeEventQueue>();
            
            Debug.Log(yeetEventQueue == megaEventQueue);
            Debug.Log(yeetEventQueue == gigaEventQueue);
            Debug.Log(yeetEventQueue == godEventQueue);
        }
    }
}