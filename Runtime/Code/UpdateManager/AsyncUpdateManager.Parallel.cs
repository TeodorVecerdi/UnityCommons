using System;

namespace UnityCommons {
    public static partial class AsyncUpdateManager {
        public sealed class Parallel : MonoSingleton<Parallel> {
            private readonly AsyncUpdateEvent onUpdate = new AsyncUpdateEvent();
            private readonly AsyncUpdateEvent onLateUpdate = new AsyncUpdateEvent();
            private readonly AsyncUpdateEvent onFixedUpdate = new AsyncUpdateEvent();

            public AsyncUpdateEvent OnUpdate {
                get => onUpdate;
                set {
                    if (value != onUpdate) throw new InvalidOperationException("Cannot change OnUpdate event");
                } 
            }
        
            public AsyncUpdateEvent OnLateUpdate {
                get => onLateUpdate;
                set {
                    if (value != onLateUpdate) throw new InvalidOperationException("Cannot change OnLateUpdate event");
                } 
            }
        
            public AsyncUpdateEvent OnFixedUpdate {
                get => onFixedUpdate;
                set {
                    if (value != onFixedUpdate) throw new InvalidOperationException("Cannot change OnFixedUpdate event");
                } 
            }


            private async void Update() {
                await OnUpdate.InvokeParallel();
            }

            private async void LateUpdate() {
                await OnLateUpdate.InvokeParallel();
            }

            private async void FixedUpdate() {
                await OnFixedUpdate.InvokeParallel();
            }
        }
        
    }
}