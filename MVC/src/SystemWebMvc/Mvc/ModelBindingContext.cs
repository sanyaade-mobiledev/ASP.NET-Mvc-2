namespace System.Web.Mvc {
    using System;
    using System.Web;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class ModelBindingContext : ControllerContext {

        private object _cachedModel;
        private Func<object> _modelProvider;
        private ModelStateDictionary _modelState;
        private Predicate<string> _propertyFilter;

        public ModelBindingContext(ControllerContext controllerContext, IValueProvider valueProvider, Type modelType, string modelName, Func<object> modelProvider, ModelStateDictionary modelState, Predicate<string> propertyFilter) :
            base(controllerContext) {
            if (valueProvider == null) {
                throw new ArgumentNullException("valueProvider");
            }
            if (modelType == null) {
                throw new ArgumentNullException("modelType");
            }

            ValueProvider = valueProvider;
            _modelProvider = modelProvider;
            ModelName = modelName ?? String.Empty;
            ModelType = modelType;
            _modelState = modelState;
            _propertyFilter = propertyFilter;
        }

        public object Model {
            get {
                // we only want to call the model provider once, then cache its value
                if (_modelProvider != null) {
                    _cachedModel = _modelProvider();
                    _modelProvider = null;
                }
                return _cachedModel;
            }
        }

        public string ModelName {
            get;
            private set;
        }

        public Type ModelType {
            get;
            private set;
        }

        public ModelStateDictionary ModelState {
            get {
                if (_modelState == null) {
                    _modelState = new ModelStateDictionary();
                }
                return _modelState;
            }
        }

        public IValueProvider ValueProvider {
            get;
            private set;
        }

        public bool ShouldUpdateProperty(string propertyName) {
            return (_propertyFilter != null) ? _propertyFilter(propertyName) : true;
        }

    }
}
