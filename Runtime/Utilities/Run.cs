using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace UnityCommons {
    public static class Run {
        /// <summary>
        /// Runs <paramref name="action"/> every <paramref name="ticks"/> updates/ticks. <paramref name="updateType"/> determines whether
        /// <paramref name="action"/> is ran in the Update, LateUpdate, or FixedUpdate loop
        /// </summary>
        /// <returns>An IDisposable which can be used to remove <paramref name="action"/> from updating by calling <code>.Dispose()</code> on it</returns>
        [MustUseReturnValue] public static IDisposable EveryTicks(int ticks, UpdateType updateType, Action action) {
            Initialize();
            return RunUtilityUpdater.Instance.EveryTicks(ticks, action, updateType);
        }

        /// <summary>
        /// Runs <paramref name="action"/> every <paramref name="ticks"/> updates/ticks.
        /// </summary>
        /// <returns>An IDisposable which can be used to remove <paramref name="action"/> from updating by calling <code>.Dispose()</code> on it</returns>
        [MustUseReturnValue] public static IDisposable EveryTicks(int ticks, Action action) {
            Initialize();
            return RunUtilityUpdater.Instance.EveryTicks(ticks, action, UpdateType.Normal);
        }

        /// <summary>
        /// Runs <paramref name="action"/> every frame. <paramref name="updateType"/> determines whether
        /// <paramref name="action"/> is ran in the Update, LateUpdate, or FixedUpdate loop
        /// </summary>
        /// <returns>An IDisposable which can be used to remove <paramref name="action"/> from updating by calling <code>.Dispose()</code> on it</returns>
        [MustUseReturnValue] public static IDisposable EveryFrame(UpdateType updateType, Action action) {
            Initialize();
            return RunUtilityUpdater.Instance.EveryFrame(action, updateType);
        }

        /// <summary>
        /// Runs <paramref name="action"/> every Update loop (every frame).
        /// </summary>
        /// <returns>An IDisposable which can be used to remove <paramref name="action"/> from updating by calling <code>.Dispose()</code> on it</returns>
        [MustUseReturnValue] public static IDisposable EveryFrame(Action action) {
            Initialize();
            return RunUtilityUpdater.Instance.EveryFrame(action, UpdateType.Normal);
        }

        /// <summary>
        /// Runs <paramref name="action"/> every <paramref name="rate"/> seconds, with an initial delay of <value>0</value> seconds.
        /// </summary>
        /// <returns>An IDisposable which can be used to remove <paramref name="action"/> from being run by calling <code>.Dispose()</code> on it</returns>
        [MustUseReturnValue] public static IDisposable Every(float rate, Action action) {
            Initialize();
            return RunUtilityUpdater.Instance.Every(action, rate, 0);
        }

        /// <summary>
        /// Runs <paramref name="action"/> every <paramref name="rate"/> seconds, with an initial delay of <paramref name="initialDelay"/> seconds.
        /// </summary>
        /// <returns>An IDisposable which can be used to remove <paramref name="action"/> from being run by calling <code>.Dispose()</code> on it</returns>
        [MustUseReturnValue] public static IDisposable Every(float rate, float initialDelay, Action action) {
            Initialize();
            return RunUtilityUpdater.Instance.Every(action, rate, initialDelay);
        }

        /// <summary>
        /// Runs <paramref name="action"/> after <paramref name="delay"/> seconds.
        /// </summary>
        /// <returns>An IDisposable which can be used to cancel the call of <paramref name="action"/> by calling <code>.Dispose()</code> on it</returns>
        public static IDisposable After(float delay, Action action) {
            Initialize();
            return RunUtilityUpdater.Instance.After(action, delay);
        }

        private static void Initialize() {
            if (RunUtilityUpdater.IsInitialized) {
                return;
            }

            RunUtilityUpdater instance = new GameObject("RunUtilityUpdater").AddComponent<RunUtilityUpdater>();
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
                ClearQueue(removeUpdate);
                foreach (Function function in functions) {
                    if (function.updateType != UpdateType.Normal) continue;
                    if (function is TickFunction tickFunction) {
                        if (Time.frameCount % tickFunction.ticks != 0) continue;
                    }

                    function.action?.Invoke();
                }
                ClearQueue(removeUpdate);
            }

            private void LateUpdate() {
                ClearQueue(removeLate);
                foreach (Function function in functions) {
                    if (function.updateType != UpdateType.Late) continue;
                    if (function is TickFunction tickFunction) {
                        if (Time.frameCount % tickFunction.ticks != 0) continue;
                    }

                    function.action?.Invoke();
                }
                ClearQueue(removeLate);
            }

            private void FixedUpdate() {
                ClearQueue(removeFixed);
                foreach (Function function in functions) {
                    if (function.updateType != UpdateType.Fixed) continue;
                    if (function is TickFunction tickFunction) {
                        int fixedFrameCount = Mathf.RoundToInt(Time.fixedTime / Time.fixedDeltaTime);
                        if (fixedFrameCount % tickFunction.ticks != 0) continue;
                    }

                    function.action?.Invoke();
                }
                ClearQueue(removeFixed);
            }

            internal IDisposable EveryTicks(int ticks, Action action, UpdateType updateType) {
                TickFunction function = new TickFunction(ticks, action, updateType);
                functions.Add(function);
                return new FunctionDisposable(this, function);
            }

            internal IDisposable EveryFrame(Action action, UpdateType updateType) {
                Function function = new Function(action, updateType);
                functions.Add(function);
                return new FunctionDisposable(this, function);
            }

            internal IDisposable Every(Action action, float rate, float initialDelay) {
                return new CoroutineDisposable(this, StartCoroutine(Runner(action, rate, initialDelay)));
            }

            internal IDisposable After(Action action, float delay) {
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

            private void ClearQueue(Queue<Function> queue) {
                while (queue.Count > 0) {
                    Function func = queue.Dequeue();
                    functions.Remove(func);
                }
            }

            internal void QueueFree(Function function) {
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

        private class TickFunction : Function {
            // ReSharper disable once InconsistentNaming
            internal readonly int ticks;

            public TickFunction(int ticks, Action action, UpdateType updateType) : base(action, updateType) {
                this.ticks = ticks;
            }
        }

        public enum UpdateType {
            Normal,
            Late,
            Fixed
        }
    }
}