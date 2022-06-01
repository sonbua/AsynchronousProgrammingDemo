using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Library.NetFramework
{
    public static class AsyncContext
    {
        public static void Run(Func<Task> func)
        {
            var originalSyncContext = SynchronizationContext.Current;

            try
            {
                var uiThreadSyncContext = new PseudoUIThreadSynchronizationContext();
                SynchronizationContext.SetSynchronizationContext(uiThreadSyncContext);

                var task = func();
                task.ContinueWith(_ => uiThreadSyncContext.Complete());

                uiThreadSyncContext.StartConsumingMessage();

                task.GetAwaiter().GetResult();
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(originalSyncContext);
            }
        }

        private class PseudoUIThreadSynchronizationContext : SynchronizationContext
        {
            private readonly BlockingCollection<KeyValuePair<SendOrPostCallback, object>> _workQueue;

            public PseudoUIThreadSynchronizationContext()
            {
                _workQueue = new BlockingCollection<KeyValuePair<SendOrPostCallback, object>>();
            }

            public void StartConsumingMessage()
            {
                foreach (var message in _workQueue.GetConsumingEnumerable())
                {
                    message.Key(message.Value);
                }
            }

            public void Complete() => _workQueue.CompleteAdding();

            public override void Post(SendOrPostCallback d, object state)
            {
                _workQueue.Add(new KeyValuePair<SendOrPostCallback, object>(d, state));
            }
        }
    }
}