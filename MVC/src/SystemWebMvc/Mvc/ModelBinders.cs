﻿namespace System.Web.Mvc {
    using System;
    using System.Collections.Generic;
    using System.Web;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public static class ModelBinders {

        private static readonly ModelBindersInfo _bindersInfo = new ModelBindersInfo();

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
            return GetBinder(modelType, true /* fallbackToDefault */);
        }

        public static IModelBinder GetBinder(Type modelType, bool fallbackToDefault) {
            if (modelType == null) {
                throw new ArgumentNullException("modelType");
            }

            return _bindersInfo.GetBinder(modelType, fallbackToDefault);
        }

    }
}