namespace Microsoft.Web.Mvc {
    using System;
    using System.Threading;
    using System.Web;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public sealed class NoAsyncTimeoutAttribute : AsyncTimeoutAttribute {

        public NoAsyncTimeoutAttribute()
            : base(Timeout.Infinite) {
        }

    }
}
