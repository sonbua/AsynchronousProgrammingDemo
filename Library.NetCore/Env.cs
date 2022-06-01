using System;
using System.Threading;

namespace Library.NetCore
{
    public class Env
    {
        private readonly int _currentManagedThreadId;
        private readonly SynchronizationContext _synchronizationContext;

        private Env()
        {
            _currentManagedThreadId = Environment.CurrentManagedThreadId;
            _synchronizationContext = SynchronizationContext.Current;
        }

        public override string ToString()
        {
            return $"ThreadId: {_currentManagedThreadId}, SyncCtx: {_synchronizationContext}";
        }

        public static Env Current => new Env();
    }
}