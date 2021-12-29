using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnityCommons {
    public delegate Task AsyncUpdateDelegate();

    public sealed class AsyncUpdateEvent {
        private readonly List<AsyncUpdateDelegate> delegates = new();

        private AsyncUpdateEvent Add(AsyncUpdateDelegate @delegate) {
            if (@delegate == null) {
                throw new ArgumentNullException(nameof(@delegate));
            }
            
            delegates.Add(@delegate);
            return this;
        }
        
        private AsyncUpdateEvent Remove(AsyncUpdateDelegate @delegate) {
            if (@delegate == null) {
                throw new ArgumentNullException(nameof(@delegate));
            }
            
            delegates.Remove(@delegate);
            return this;
        }

        internal async Task InvokeSequential() {
            foreach (AsyncUpdateDelegate @delegate in delegates)
                await @delegate().ConfigureAwait(false);
        }

        internal async Task InvokeParallel() {
            await Task.WhenAll(delegates.Select(@delegate => @delegate())).ConfigureAwait(false);
        }

        public static AsyncUpdateEvent operator +(AsyncUpdateEvent @event, AsyncUpdateDelegate @delegate) {
            return @event.Add(@delegate);
        }

        public static AsyncUpdateEvent operator -(AsyncUpdateEvent @event, AsyncUpdateDelegate @delegate) {
            return @event.Remove(@delegate);
        }
    }
}