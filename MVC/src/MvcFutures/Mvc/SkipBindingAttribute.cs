﻿namespace Microsoft.Web.Mvc {
    using System;
    using System.Web;
    using System.Web.Mvc;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public sealed class SkipBindingAttribute : CustomModelBinderAttribute {
        private static readonly NullBinder _nullBinder = new NullBinder();

        public override IModelBinder GetBinder() {
            return _nullBinder;
        }

        private class NullBinder : IModelBinder {
            public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext) {
                return null;
            }
        }
    }
}