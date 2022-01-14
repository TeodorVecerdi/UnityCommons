using System;

namespace UnityCommons {
    public class UpdateManager : MonoSingleton<UpdateManager> {
        private static readonly Action nop = () => { };

        public event Action OnUpdate = nop;
        public event Action OnLateUpdate = nop;
        public event Action OnFixedUpdate = nop;

        private void Update() {
            OnUpdate();
        }

        private void LateUpdate() {
            OnLateUpdate();
        }

        private void FixedUpdate() {
            OnFixedUpdate();
        }
    }
}