﻿namespace System.Web.Mvc {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc.Resources;
    using System.Web.Routing;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class DefaultControllerFactory : IControllerFactory {

        private IBuildManager _buildManager;
        private ControllerBuilder _controllerBuilder;
        private ControllerTypeCache _instanceControllerTypeCache;
        private static ControllerTypeCache _staticControllerTypeCache = new ControllerTypeCache();

        internal IBuildManager BuildManager {
            get {
                if (_buildManager == null) {
                    _buildManager = new BuildManagerWrapper();
                }
                return _buildManager;
            }
            set {
                _buildManager = value;
            }
        }

        internal ControllerBuilder ControllerBuilder {
            get {
                return _controllerBuilder ?? ControllerBuilder.Current;
            }
            set {
                _controllerBuilder = value;
            }
        }

        internal ControllerTypeCache ControllerTypeCache {
            get {
                return _instanceControllerTypeCache ?? _staticControllerTypeCache;
            }
            set {
                _instanceControllerTypeCache = value;
            }
        }

        public RequestContext RequestContext {
            get;
            set;
        }

        protected internal virtual IController CreateController(RequestContext requestContext, string controllerName) {
            if (requestContext == null) {
                throw new ArgumentNullException("requestContext");
            }
            if (String.IsNullOrEmpty(controllerName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "controllerName");
            }
            RequestContext = requestContext;
            Type controllerType = GetControllerType(controllerName);
            IController controller = GetControllerInstance(controllerType);
            return controller;
        }

        protected internal virtual void DisposeController(IController controller) {
            IDisposable disposable = controller as IDisposable;
            if (disposable != null) {
                disposable.Dispose();
            }
        }

        protected internal virtual IController GetControllerInstance(Type controllerType) {
            if (controllerType == null) {
                throw new ArgumentNullException(
                    "controllerType",
                    String.Format(
                        CultureInfo.CurrentUICulture,
                        MvcResources.DefaultControllerFactory_NoControllerFound,
                        RequestContext.HttpContext.Request.Path));
            }
            if (!typeof(IController).IsAssignableFrom(controllerType)) {
                throw new ArgumentException(
                    String.Format(
                        CultureInfo.CurrentUICulture,
                        MvcResources.DefaultControllerFactory_MissingIController,
                        controllerType),
                    "controllerType");
            }
            try {
                return Activator.CreateInstance(controllerType) as IController;
            }
            catch (Exception ex) {
                throw new InvalidOperationException(
                    String.Format(
                        CultureInfo.CurrentUICulture,
                        MvcResources.DefaultControllerFactory_ErrorCreatingController,
                        controllerType),
                    ex);
            }
        }

        protected internal virtual Type GetControllerType(string controllerName) {
            if (String.IsNullOrEmpty(controllerName)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "controllerName");
            }

            // first search in the current route's namespace collection
            object routeNamespacesObj;
            Type match;
            if (RequestContext != null && RequestContext.RouteData.DataTokens.TryGetValue("Namespaces", out routeNamespacesObj)) {
                IEnumerable<string> routeNamespaces = routeNamespacesObj as IEnumerable<string>;
                if (routeNamespaces != null) {
                    HashSet<string> nsHash = new HashSet<string>(routeNamespaces, StringComparer.OrdinalIgnoreCase);
                    match = GetControllerTypeWithinNamespaces(controllerName, nsHash);
                    if (match != null) {
                        return match;
                    }
                }
            }

            // then search in the application's default namespace collection
            HashSet<string> nsDefaults = new HashSet<string>(ControllerBuilder.DefaultNamespaces, StringComparer.OrdinalIgnoreCase);
            match = GetControllerTypeWithinNamespaces(controllerName, nsDefaults);
            if (match != null) {
                return match;
            }

            // if all else fails, search every namespace
            return GetControllerTypeWithinNamespaces(controllerName, null /* namespaces */);
        }

        private Type GetControllerTypeWithinNamespaces(string controllerName, HashSet<string> namespaces) {
            // Once the master list of controllers has been created we can quickly index into it
            ControllerTypeCache.EnsureInitialized(BuildManager);

            IList<Type> matchingTypes = ControllerTypeCache.GetControllerTypes(controllerName, namespaces);
            switch (matchingTypes.Count) {
                case 0:
                    // no matching types
                    return null;

                case 1:
                    // single matching type
                    return matchingTypes[0];

                default:
                    // multiple matching types
                    string typeNames = String.Join(", ", matchingTypes.Select(t => t.FullName).ToArray());
                    throw new InvalidOperationException(
                        String.Format(
                            CultureInfo.CurrentUICulture,
                            MvcResources.DefaultControllerFactory_ControllerNameAmbiguous,
                            controllerName, typeNames));
            }
        }

        #region IControllerFactory Members
        IController IControllerFactory.CreateController(RequestContext context, string controllerName) {
            return CreateController(context, controllerName);
        }

        void IControllerFactory.DisposeController(IController controller) {
            DisposeController(controller);
        }
        #endregion
    }
}
