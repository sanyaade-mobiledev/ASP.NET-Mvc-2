namespace Microsoft.Web.Preview.Services {
    using System.Collections;
    using System.Collections.Generic;
    using Microsoft.Web.Util;

    public class BridgeCache : IBridgeRequestCache {
        private static Dictionary<NGenWrapper<long>, object> _cache = new Dictionary<NGenWrapper<long>, object>();

        public void Initialize(BridgeTransformData data) { }

        public void Clear() {
            _cache.Clear();
        }

        public object Lookup(BridgeContext context) {
            object result = null;
            if (_cache.TryGetValue(GetCacheKey(context), out result)) {
                return result;
            }
            return null;
        }

        public void Put(BridgeContext context) {
            _cache[GetCacheKey(context)] = context.BridgeResponse.Response;
        }

        // Generate hash from Method and all args
        private long GetCacheKey(BridgeContext context) {
            HashCodeCombiner hasher = new HashCodeCombiner();

            hasher.AddObject(context.BridgeUrl);
            hasher.AddObject(context.ServiceRequest.Method);
            ComputeHashCode(context.ServiceRequest.Args, hasher);

            return hasher.CombinedHash;
        }

        // Need to recursively shred dictionaries collections, arrays
        private void ComputeHashCode(object obj, HashCodeCombiner hasher) {
            IDictionary dict = obj as IDictionary;
            if (dict != null) {
                foreach (object key in dict.Keys) {
                    hasher.AddObject(key);
                    ComputeHashCode(dict[key], hasher);
                }
            }
            else {
                IEnumerable en = obj as IEnumerable;
                if ((en != null) && !(obj is string)) {
                    foreach (object o in en) {
                        ComputeHashCode(o, hasher);
                    }
                }
                else {
                    hasher.AddObject(obj);
                }
            }
        }
    }

    /*
     * Class used to combine several hashcodes into a single hashcode
     */
    internal class HashCodeCombiner {

        private long _combinedHash;

        internal HashCodeCombiner() {
            // Start with a seed (obtained from String.GetHashCode implementation)
            _combinedHash = 5381;
        }

        internal void AddInt(int n) {
            _combinedHash = ((_combinedHash << 5) + _combinedHash) ^ n;
        }

        internal void AddObject(string s) {
            if (s != null)
                AddInt(s.GetHashCode());
        }

        internal void AddObject(object o) {
            if (o != null)
                AddInt(o.GetHashCode());
        }

        internal long CombinedHash { get { return _combinedHash; } }
    }
}
