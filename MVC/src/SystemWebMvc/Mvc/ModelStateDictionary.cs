namespace System.Web.Mvc {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc.Resources;

    // TODO: Make this class implement IDictionary<,> rather than Dictionary

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [SuppressMessage("Microsoft.Usage", "CA2237:MarkISerializableTypesWithSerializable",
        Justification="We need to refactor this type to implement IDictionary<,> rather than subclass Dictionary<,>.")]
    public class ModelStateDictionary : Dictionary<string, ModelState> {

        public ModelStateDictionary()
            : base(StringComparer.OrdinalIgnoreCase) {
        }

        public ModelStateDictionary(ModelStateDictionary dictionary)
            : base(dictionary, StringComparer.OrdinalIgnoreCase) {
        }

        public bool IsValid {
            get {
                return Values.All(modelState => modelState.Errors.Count == 0);
            }
        }

        public void AddModelError(string key, string attemptedValue, Exception exception) {
            ModelState modelState = GetModelStateForKey(key, attemptedValue);
            modelState.Errors.Add(exception);
        }

        public void AddModelError(string key, string attemptedValue, string errorMessage) {
            ModelState modelState = GetModelStateForKey(key, attemptedValue);
            modelState.Errors.Add(errorMessage);
        }

        private ModelState GetModelStateForKey(string key, string attemptedValue) {
            if (String.IsNullOrEmpty(key)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "key");
            }

            ModelState modelState;
            if (!TryGetValue(key, out modelState)) {
                modelState = new ModelState();
                this[key] = modelState;
            }

            modelState.AttemptedValue = attemptedValue;
            return modelState;
        }

    }
}
