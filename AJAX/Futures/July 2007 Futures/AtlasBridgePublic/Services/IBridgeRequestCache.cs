namespace Microsoft.Web.Preview.Services {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IBridgeRequestCache {
        void Initialize(BridgeTransformData data);
        object Lookup(BridgeContext context);
        void Put(BridgeContext context);
        void Clear();
    }
}
