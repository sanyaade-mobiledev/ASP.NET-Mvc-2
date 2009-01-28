namespace Microsoft.Web.Mvc {
    using System;
    using System.Threading;
    using System.Web;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public sealed class SingleFireEvent {

        private int _hasFired; // 0 = false, 1 = true

        // returns true if this is the first call to Signal(), false otherwise
        public bool Signal() {
            int oldValue = Interlocked.Exchange(ref _hasFired, 1);
            return (oldValue == 0);
        }

    }
}
