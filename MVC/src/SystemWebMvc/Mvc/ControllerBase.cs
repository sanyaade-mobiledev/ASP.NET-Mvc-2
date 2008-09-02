namespace System.Web.Mvc {
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using System.Web.Routing;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public abstract class ControllerBase : IController {

        private TempDataDictionary _tempDataDictionary;
        private ViewDataDictionary _viewDataDictionary;

        public ControllerContext ControllerContext {
            get;
            set;
        }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly",
            Justification = "This property is settable so that unit tests can provide mock implementations.")]
        public TempDataDictionary TempData {
            get {
                if (_tempDataDictionary == null) {
                    _tempDataDictionary = new TempDataDictionary();
                }
                return _tempDataDictionary;
            }
            set {
                _tempDataDictionary = value;
            }
        }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly",
            Justification = "This property is settable so that unit tests can provide mock implementations.")]
        public ViewDataDictionary ViewData {
            get {
                if (_viewDataDictionary == null) {
                    _viewDataDictionary = new ViewDataDictionary();
                }
                return _viewDataDictionary;
            }
            set {
                _viewDataDictionary = value;
            }
        }

        protected virtual void Execute(RequestContext requestContext) {
            if (requestContext == null) {
                throw new ArgumentNullException("requestContext");
            }

            Initialize(requestContext);
            ExecuteCore();
        }

        protected abstract void ExecuteCore();

        protected virtual void Initialize(RequestContext requestContext) {
            ControllerContext = new ControllerContext(requestContext, this);
        }

        #region IController Members
        void IController.Execute(RequestContext requestContext) {
            Execute(requestContext);
        }
        #endregion
    }
}
