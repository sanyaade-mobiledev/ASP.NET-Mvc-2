﻿namespace System.Web.Mvc {
    using System;
    using System.Diagnostics.CodeAnalysis;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public abstract class ViewResultBase : ActionResult {
        private TempDataDictionary _tempData;
        private ViewDataDictionary _viewData;
        private IViewEngine _viewEngine;
        private string _viewName;

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly",
            Justification = "This entire type is meant to be mutable.")]
        public TempDataDictionary TempData {
            get {
                if (_tempData == null) {
                    _tempData = new TempDataDictionary();
                }
                return _tempData;
            }
            set {
                _tempData = value;
            }
        }

        public IView View {
            get;
            set;
        }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly",
            Justification = "This entire type is meant to be mutable.")]
        public ViewDataDictionary ViewData {
            get {
                if (_viewData == null) {
                    _viewData = new ViewDataDictionary();
                }
                return _viewData;
            }
            set {
                _viewData = value;
            }
        }

        public IViewEngine ViewEngine {
            get {
                return _viewEngine ?? ViewEngines.DefaultEngine;
            }
            set {
                _viewEngine = value;
            }
        }

        public string ViewName {
            get {
                return _viewName ?? String.Empty;
            }
            set {
                _viewName = value;
            }
        }

        public override void ExecuteResult(ControllerContext context) {
            if (context == null) {
                throw new ArgumentNullException("context");
            }
            if (String.IsNullOrEmpty(ViewName)) {
                ViewName = context.RouteData.GetRequiredString("action");
            }

            ViewEngineResult result = null;

            if (View == null) {
                result = FindView(context);
                View = result.View;
            }

            ViewContext viewContext = new ViewContext(context, View, ViewData, TempData);
            View.Render(viewContext, context.HttpContext.Response.Output);

            if (result != null) {
                result.ViewEngine.ReleaseView(context, View);
            }
        }

        protected abstract ViewEngineResult FindView(ControllerContext context);
    }
}
