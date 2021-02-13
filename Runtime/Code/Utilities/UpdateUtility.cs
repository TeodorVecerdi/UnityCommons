using System;
using System.Collections.Generic;

namespace UnityCommons {
    public class UpdateUtility : MonoSingleton<UpdateUtility> {
        private readonly List<Function> functions = new List<Function>();

        private void Update() {
            foreach (var function in functions) {
                function.action();
            }
        }

        private void RemoveAction(Function function) {
            functions.Remove(function);
        }

        /// <summary>
        /// Adds <paramref name="action"/> to the updater to be run every frame.
        /// </summary>
        /// <returns>An IDisposable which can be used to remove <paramref name="action"/> from updating by calling .Dispose() on it</returns>
        public static IDisposable AddUpdateAction(Action action) {
            var function = new Function(action);
            Instance.functions.Add(function);
            return new ActionCanceler(Instance, function);
        }


        private class ActionCanceler : IDisposable {
            private readonly UpdateUtility owner;
            private readonly Function function;

            public ActionCanceler(UpdateUtility owner, Function function) {
                this.owner = owner;
                this.function = function;
            }

            public void Dispose() {
                owner.RemoveAction(function);
            }
        }

        private class Function {
            internal readonly Action action;

            public Function(Action action) {
                this.action = action;
            }
        }
    }
}