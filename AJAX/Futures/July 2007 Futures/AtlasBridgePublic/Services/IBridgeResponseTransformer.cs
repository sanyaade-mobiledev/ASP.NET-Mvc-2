namespace Microsoft.Web.Preview.Services {
    public interface IBridgeResponseTransformer {
        void Initialize(BridgeTransformData data);
        object Transform(object results);
    }
}
