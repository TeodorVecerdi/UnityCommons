using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityCommons {
    public static class UpdateUtility {
        /// <summary>
        /// Adds <paramref name="action"/> to the updater to be run every frame.
        /// </summary>
        /// <returns>An IDisposable which can be used to remove <paramref name="action"/> from updating by calling .Dispose() on it</returns>
        public static IDisposable Create(Action action) {
            var function = new Function(action);
            UpdateUtilityUpdater.Instance.Functions.Add(function);
            return new ActionCanceler(UpdateUtilityUpdater.Instance, function);
        }

        #region Private Classes

        private class UpdateUtilityUpdater : MonoSingleton<UpdateUtilityUpdater> {
            public readonly List<Function> Functions = new List<Function>();
            private readonly Queue<Function> removeQueue = new Queue<Function>();

            protected override void OnAwake() {
                gameObject.hideFlags = HideFlags.HideAndDontSave;
            }

            private void Update() {
                foreach (var function in Functions) {
                    function.action();
                }

                while (removeQueue.Count > 0) {
                    var func = removeQueue.Dequeue();
                    Functions.Remove(func);
                }
            }

            public void QueueFree(Function function) {
                removeQueue.Enqueue(function);
            }
        }

        private class ActionCanceler : IDisposable {
            private readonly UpdateUtilityUpdater owner;
            private readonly Function function;
            private bool disposed;

            public ActionCanceler(UpdateUtilityUpdater owner, Function function) {
                this.owner = owner;
                this.function = function;
                disposed = false;
            }

            public void Dispose() {
                if (!disposed) {
                    owner.QueueFree(function);
                    disposed = true;
                }
            }
        }

        private class Function {
            // ReSharper disable once InconsistentNaming
            internal readonly Action action;

            public Function(Action action) {
                this.action = action;
            }
        }

        #endregion
    }
}