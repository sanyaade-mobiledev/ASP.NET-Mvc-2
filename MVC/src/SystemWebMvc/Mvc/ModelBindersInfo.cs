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

        public IModelBinder GetBinder(Type modelType, bool fallbackToDefault) {
            return GetBinder(modelType, (fallbackToDefault) ? DefaultBinder : null);
        }

        private IModelBinder GetBinder(Type modelType, IModelBinder fallbackBinder) {
            // Try to look up a binder for this type. We use this order of precedence:
            // 1. Binder registered in the global table
            // 2. Binder attribute defined on the type
            // 3. Supplied fallback binder

            IModelBinder binder;
            if (Binders.TryGetValue(modelType, out binder)) {
                return binder;
            }

            CustomModelBinderAttribute[] attrs = (CustomModelBinderAttribute[])modelType.GetCustomAttributes(typeof(CustomModelBinderAttribute), false /* inherit */);
            switch (attrs.Length) {
                case 0:
                    return fallbackBinder;
                case 1:
                    binder = attrs[0].GetBinder();
                    return binder;
                default:
                    throw new InvalidOperationException(String.Format(CultureInfo.CurrentUICulture,
                        MvcResources.ModelBindersInfo_MultipleAttributes, modelType.FullName));
            }
        }

    }
}
