using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityCommons {
    public static class Run {
        private static readonly List<Action> threadSafeActions = new List<Action>();
        
        /// <summary>
        /// Runs <paramref name="action"/> every frame. <paramref name="updateType"/> determines whether
        /// <paramref name="action"/> is ran in the Update, LateUpdate, or FixedUpdate loop
        /// </summary>
        /// <returns>An IDisposable which can be used to remove <paramref name="action"/> from updating by calling .Dispose() on it</returns>
        public static IDisposable EveryFrame(UpdateType updateType, Action action) {
            return RunUtilityUpdater.Instance.EveryFrame(action, updateType);
        }

        /// <summary>
        /// Runs <paramref name="action"/> every Update loop (every frame).
        /// </summary>
        /// <returns>An IDisposable which can be used to remove <paramref name="action"/> from updating by calling .Dispose() on it</returns>
        public static IDisposable EveryFrame(Action action) {
            return RunUtilityUpdater.Instance.EveryFrame(action, UpdateType.Normal);
        }

        /// <summary>
        /// Runs <paramref name="action"/> every <paramref name="rate"/> seconds, with an initial delay of <value>0</value> seconds.
        /// </summary>
        /// <returns>An IDisposable which can be used to remove <paramref name="action"/> from being run by calling .Dispose() on it</returns>
        public static IDisposable Every(float rate, Action action) {
            return RunUtilityUpdater.Instance.Every(action, rate, 0);
        }

        /// <summary>
        /// Runs <paramref name="action"/> every <paramref name="rate"/> seconds, with an initial delay of <paramref name="initialDelay"/> seconds.
        /// </summary>
        /// <returns>An IDisposable which can be used to remove <paramref name="action"/> from being run by calling .Dispose() on it</returns>
        public static IDisposable Every(float rate, float initialDelay, Action action) {
            return RunUtilityUpdater.Instance.Every(action, rate, initialDelay);
        }

        /// <summary>
        /// Runs <paramref name="action"/> after <paramref name="delay"/> seconds.
        /// </summary>
        /// <returns>An IDisposable which can be used to cancel the call of <paramref name="action"/> by calling .Dispose() on it</returns>
        public static IDisposable After(float delay, Action action) {
            return RunUtilityUpdater.Instance.After(action, delay);
        }
        
        /// <summary>
        /// Runs <paramref name="action"/> once, in the next update loop.
        /// </summary>
        public static void Once(Action action) {
            threadSafeActions.Add(action);
        }
        
        // Force instance creation on load
        [RuntimeInitializeOnLoadMethod]
        private static void InitializeOnSceneLoad() {
            _ = RunUtilityUpdater.Instance;
        }

        private class RunUtilityUpdater : MonoSingleton<RunUtilityUpdater> {
            private readonly List<Function> functions = new List<Function>();
            private readonly Queue<Function> removeUpdate = new Queue<Function>();
            private readonly Queue<Function> removeLate = new Queue<Function>();
            private readonly Queue<Function> removeFixed = new Queue<Function>();

            protected override void OnAwake() {
                gameObject.hideFlags = HideFlags.HideAndDontSave;
            }

            private void Update() {
                foreach (var function in functions) {
                    if(function.updateType != UpdateType.Normal) continue;
                    function.action?.Invoke();
                }
                
                foreach (var action in threadSafeActions) {
                    action?.Invoke();
                }
                threadSafeActions.Clear();

                while (removeUpdate.Count > 0) {
                    var func = removeUpdate.Dequeue();
                    functions.Remove(func);
                }
            }

            private void LateUpdate() {
                foreach (var function in functions) {
                    if(function.updateType != UpdateType.Late) continue;
                    function.action?.Invoke();
                }

                while (removeLate.Count > 0) {
                    var func = removeLate.Dequeue();
                    functions.Remove(func);
                }
            }

            private void FixedUpdate() {
                foreach (var function in functions) {
                    if(function.updateType != UpdateType.Fixed) continue;
                    function.action?.Invoke();
                }

                while (removeFixed.Count > 0) {
                    var func = removeFixed.Dequeue();
                    functions.Remove(func);
                }
            }

            public IDisposable EveryFrame(Action action, UpdateType updateType) {
                var function = new Function(action, updateType);
                functions.Add(function);
                return new FunctionDisposable(this, function);
            }

            public IDisposable Every(Action action, float rate, float initialDelay) {
                return new CoroutineDisposable(this, StartCoroutine(Runner(action, rate, initialDelay)));
            }
            
            public IDisposable After(Action action, float delay) {
                return new CoroutineDisposable(this, StartCoroutine(Delayer(action, delay)));
            }

            private IEnumerator Runner(Action action, float rate, float initialDelay) {
                yield return new WaitForSeconds(initialDelay);

                while (true) {
                    yield return null;
                    action?.Invoke();
                    yield return new WaitForSeconds(rate);
                }
            }
            
            private IEnumerator Delayer(Action action, float delay) {
                yield return new WaitForSeconds(delay);
                yield return null;
                action?.Invoke();
            }

            public void QueueFree(Function function) {
                switch (function.updateType) {
                    case UpdateType.Normal:
                        removeUpdate.Enqueue(function);
                        break;
                    case UpdateType.Late:
                        removeLate.Enqueue(function);
                        break;
                    case UpdateType.Fixed:
                        removeFixed.Enqueue(function);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private class CoroutineDisposable : IDisposable {
            private readonly RunUtilityUpdater owner;
            private readonly Coroutine coroutine;
            private bool disposed;

            public CoroutineDisposable(RunUtilityUpdater owner, Coroutine coroutine) {
                this.owner = owner;
                this.coroutine = coroutine;
            }

            public void Dispose() {
                if (disposed) return;
                
                owner.StopCoroutine(coroutine);
                disposed = true;
            }
        }
        
        private class FunctionDisposable : IDisposable {
            private readonly RunUtilityUpdater owner;
            private readonly Function function;
            private bool disposed;

            public FunctionDisposable(RunUtilityUpdater owner, Function function) {
                this.owner = owner;
                this.function = function;
            }

            public void Dispose() {
                if (disposed) return;
                
                owner.QueueFree(function);
                disposed = true;
            }
        }
        
        private class Function {
            // ReSharper disable once InconsistentNaming
            internal readonly Action action;
            // ReSharper disable once InconsistentNaming
            internal readonly UpdateType updateType;

            public Function(Action action, UpdateType updateType) {
                this.action = action;
                this.updateType = updateType;
            }
        }

        public enum UpdateType {
            Normal,
            Late,
            Fixed
        }
    }
}