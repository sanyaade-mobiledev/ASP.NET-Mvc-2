namespace System.Web.Mvc {
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading;

    internal sealed class ActionMethodDispatcherCache {

        private ReaderWriterLock _rwLock = new ReaderWriterLock();
        private Dictionary<MethodInfo, ActionMethodDispatcher> _dispatchDictionary = new Dictionary<MethodInfo, ActionMethodDispatcher>();

        // This method could potentially return multiple dispatchers for the same methodInfo due to
        // upgrading the lock from reader to writer, but the dictionary won't be corrupted as a result.
        public ActionMethodDispatcher GetDispatcher(MethodInfo methodInfo) {
            ActionMethodDispatcher dispatcher;

            _rwLock.AcquireReaderLock(Timeout.Infinite);
            try {    
                if (_dispatchDictionary.TryGetValue(methodInfo, out dispatcher)) {
                    return dispatcher;
                }
            } finally {
                _rwLock.ReleaseReaderLock();
            }

            // if we got this far, the dispatcher was not in the cache
            dispatcher = new ActionMethodDispatcher(methodInfo);
            _rwLock.AcquireWriterLock(Timeout.Infinite);
            try {
                _dispatchDictionary[methodInfo] = dispatcher;
                return dispatcher;
            }
            finally {
                _rwLock.ReleaseWriterLock();
            }
        }

    }
}
