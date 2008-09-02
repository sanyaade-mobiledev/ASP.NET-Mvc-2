namespace System.Web.Mvc {
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web.Mvc.Resources;

    internal sealed class ModelBindersInfo {

        private IModelBinder _defaultBinder;
        private Dictionary<Type, IModelBinder> _registeredBinders = new Dictionary<Type, IModelBinder>();

        public IDictionary<Type, IModelBinder> Binders {
            get {
                return _registeredBinders;
            }
        }

        public IModelBinder DefaultBinder {
            get {
                if (_defaultBinder == null) {
                    _defaultBinder = new DefaultModelBinder();
                }
                return _defaultBinder;
            }
            set {
                _defaultBinder = value;
            }
        }

        public IModelBinder GetBinder(Type modelType) {
            // Try to look up a converter for this type. We use this order of precedence:
            // 1. Converter registered in the global table
            // 2. Converter attribute defined on the type
            // 3. Default converter

            IModelBinder converter;
            if (Binders.TryGetValue(modelType, out converter)) {
                return converter;
            }

            CustomModelBinderAttribute[] attrs = (CustomModelBinderAttribute[])modelType.GetCustomAttributes(typeof(CustomModelBinderAttribute), false /* inherit */);
            switch (attrs.Length) {
                case 0:
                    return DefaultBinder;
                case 1:
                    converter = attrs[0].GetBinder();
                    return converter;
                default:
                    throw new InvalidOperationException(String.Format(CultureInfo.CurrentUICulture,
                        MvcResources.ModelBindersInfo_MultipleAttributes, modelType.FullName));
            }
        }

    }
}
