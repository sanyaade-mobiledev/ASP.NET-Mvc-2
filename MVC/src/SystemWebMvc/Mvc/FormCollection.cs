namespace System.Web.Mvc {
    using System;
    using System.Collections.Specialized;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Web;
    using System.Web.Mvc.Resources;

    [SuppressMessage("Microsoft.Usage", "CA2237:MarkISerializableTypesWithSerializable",
        Justification = "It is not anticipated that users will need to serialize this type.")]
    [SuppressMessage("Microsoft.Design", "CA1035:ICollectionImplementationsHaveStronglyTypedMembers",
        Justification = "It is not anticipated that users will call FormCollection.CopyTo().")]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [FormCollectionBinder]
    public class FormCollection : NameValueCollection, IValueProvider {

        public FormCollection() {
        }

        public FormCollection(NameValueCollection collection) {
            if (collection == null) {
                throw new ArgumentNullException("collection");
            }

            Add(collection);
        }

        public virtual ValueProviderResult GetValue(string name) {
            if (String.IsNullOrEmpty(name)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "name");
            }

            string[] rawValue = GetValues(name);
            if (rawValue == null) {
                return null;
            }

            string attemptedValue = this[name];
            return new ValueProviderResult(rawValue, attemptedValue, CultureInfo.CurrentCulture);
        }

        private sealed class FormCollectionBinderAttribute : CustomModelBinderAttribute {

            // since the FormCollectionModelBinder.BindModel() method is thread-safe, we only need to keep
            // a single instance of the binder around
            private static readonly FormCollectionModelBinder _binder = new FormCollectionModelBinder();

            public override IModelBinder GetBinder() {
                return _binder;
            }

            // this class is used for generating a FormCollection object
            private sealed class FormCollectionModelBinder : IModelBinder {
                public ModelBinderResult BindModel(ModelBindingContext bindingContext) {
                    if (bindingContext == null) {
                        throw new ArgumentNullException("bindingContext");
                    }

                    return new ModelBinderResult(new FormCollection(bindingContext.HttpContext.Request.Form));
                }
            }
        }

    }
}
