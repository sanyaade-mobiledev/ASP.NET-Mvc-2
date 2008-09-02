namespace System.Web.Mvc {
    using System;
    using System.Collections.Generic;
    using System.Web;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public static class ModelBinders {

        private static readonly ModelBindersInfo _bindersInfo = InitializeBindersInfo();

        public static IDictionary<Type, IModelBinder> Binders {
            get {
                return _bindersInfo.Binders;
            }
        }

        public static IModelBinder DefaultBinder {
            get {
                return _bindersInfo.DefaultBinder;
            }
            set {
                _bindersInfo.DefaultBinder = value;
            }
        }

        public static IModelBinder GetBinder(Type modelType) {
            if (modelType == null) {
                throw new ArgumentNullException("modelType");
            }

            return _bindersInfo.GetBinder(modelType);
        }

        private static ModelBindersInfo InitializeBindersInfo() {
            // We need to provide a default DateTime converter because the DateTime TypeConverter incorrectly
            // marks String.Empty -> DateTime conversions as successful.

            ModelBindersInfo bindersInfo = new ModelBindersInfo();
            DateTimeModelBinder dateTimeBinder = new DateTimeModelBinder();
            bindersInfo.Binders[typeof(DateTime)] = dateTimeBinder;
            bindersInfo.Binders[typeof(DateTime?)] = dateTimeBinder;
            return bindersInfo;
        }

    }
}
