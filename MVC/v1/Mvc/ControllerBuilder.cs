﻿namespace System.Web.Mvc {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Web;
    using System.Web.Mvc.Resources;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class ControllerBuilder {

        private Func<IControllerFactory> _factoryThunk;
        private static ControllerBuilder _instance = new ControllerBuilder();
        private HashSet<string> _namespaces = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        public ControllerBuilder() {
            SetControllerFactory(new DefaultControllerFactory() {
                ControllerBuilder = this
            });
        }

        public static ControllerBuilder Current {
            get {
                return _instance;
            }
        }

        public HashSet<string> DefaultNamespaces {
            get {
                return _namespaces;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate",
            Justification = "Calling method multiple times might return different objects.")]
        public IControllerFactory GetControllerFactory() {
            IControllerFactory controllerFactoryInstance = _factoryThunk();
            return controllerFactoryInstance;
        }

        public void SetControllerFactory(IControllerFactory controllerFactory) {
            if (controllerFactory == null) {
                throw new ArgumentNullException("controllerFactory");
            }

            _factoryThunk = () => controllerFactory;
        }

        public void SetControllerFactory(Type controllerFactoryType) {
            if (controllerFactoryType == null) {
                throw new ArgumentNullException("controllerFactoryType");
            }
            if (!typeof(IControllerFactory).IsAssignableFrom(controllerFactoryType)) {
                throw new ArgumentException(
                    String.Format(
                        CultureInfo.CurrentUICulture,
                        MvcResources.ControllerBuilder_MissingIControllerFactory,
                        controllerFactoryType),
                    "controllerFactoryType");
            }

            _factoryThunk = delegate() {
                try {
                    return (IControllerFactory) Activator.CreateInstance(controllerFactoryType);
                }
                catch (Exception ex) {
                    throw new InvalidOperationException(
                        String.Format(
                            CultureInfo.CurrentUICulture,
                            MvcResources.ControllerBuilder_ErrorCreatingControllerFactory,
                            controllerFactoryType),
                        ex);
                }
            };
        }
    }
}
