namespace Microsoft.Web.Preview.Services {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;
    using System.Collections;

    public class ObjectMapperBridgeTransformer : IBridgeResponseTransformer {
        private string _selector;
        private Dictionary<string, string> _propertyMap;

        public void Initialize(BridgeTransformData data) {
            if (!data.Dictionaries.TryGetValue("propertyMap", out _propertyMap)) {
                throw new ArgumentException("ObjectMapperBridgeTransformer requires a selectedNodes dictionary");
            }
            if (!data.Attributes.TryGetValue("selector", out _selector)) {
                throw new ArgumentException("ObjectMapperBridgeTransformer requires a selector attribute");
            }
        }

        public object Transform(object results) {
            // REVIEW: Probably should rethrow exceptions with different text
            IEnumerable objects = null;
            if (_selector == "this") {
                objects = results as IEnumerable;
                if (objects == null) {
                    List<object> l = new List<object>();
                    l.Add(results);
                    objects = l;
                }
            }
            else {
                objects = System.Web.UI.DataBinder.Eval(results, _selector) as IEnumerable;
                if (objects == null) {
                    throw new InvalidOperationException("ObjectMapperBridgeTransformer: selector property must contain an IEnumerable");
                }
            }

            List<Dictionary<string, object>> returnValue = new List<Dictionary<string, object>>();
            foreach (object obj in objects) {
                Dictionary<string, object> shreddedObj = new Dictionary<string, object>();
                foreach (string key in _propertyMap.Keys) {
                    shreddedObj[key] = System.Web.UI.DataBinder.Eval(obj, _propertyMap[key]);
                }
                returnValue.Add(shreddedObj);
            }
            return returnValue;
        }
    }
}
