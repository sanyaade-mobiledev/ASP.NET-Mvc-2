namespace Microsoft.Web.Preview.Services {
    using System;
    using System.Web;
    using System.Web.Hosting;

    // Temporary module which we need to support access to asbx extensions for people unable to change IIS mappings
    public sealed class BridgeModule : IHttpModule {
        void IHttpModule.Init(HttpApplication context) {
            if (context == null) {
                throw new ArgumentNullException("context");
            }
            context.BeginRequest += new EventHandler(OnBeginRequestHandler);
        }

        void IHttpModule.Dispose() {
        }

        internal const string BridgeSuffix = "bridge.axd";
        internal const int BridgeSuffixLength = 10;

        void OnBeginRequestHandler(Object source, EventArgs eventArgs) {
            HttpContext context = ((HttpApplication)source).Context;
            string bridgePath = context.Request.FilePath;

            // The paths we rewrite will be in the form of /atlas/BCL/MSNBridge.axd[/js] --> /atlas/BCL/MSN.asbx[/js]
            if (bridgePath.EndsWith(BridgeSuffix, StringComparison.OrdinalIgnoreCase)) {
                // Replace AtlasBridge.axd with .asbx and that's the real path
                bridgePath = context.Request.Path.Substring(0, bridgePath.Length - BridgeSuffixLength) + ".asbx";

                // Only rewrite the path if the bridge file exists
                VirtualPathProvider pathProvider = HostingEnvironment.VirtualPathProvider;
                if (pathProvider.FileExists(bridgePath)) {
                    context.RewritePath(bridgePath, context.Request.PathInfo, null);
                }
            }
        }
    }

}
