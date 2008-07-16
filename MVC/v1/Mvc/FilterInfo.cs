namespace System.Web.Mvc {
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class FilterInfo {

        private IList<IActionFilter> _actionFilters;
        private IList<IAuthorizationFilter> _authorizationFilters;
        private IList<IExceptionFilter> _exceptionFilters;
        private IList<IResultFilter> _resultFilters;

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly",
            Justification = "Users may want to replace the entire collection.")]
        public IList<IActionFilter> ActionFilters {
            get {
                return GetListOrEmpty(ref _actionFilters);
            }
            set {
                _actionFilters = value;
            }
        }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly",
            Justification = "Users may want to replace the entire collection.")]
        public IList<IAuthorizationFilter> AuthorizationFilters {
            get {
                return GetListOrEmpty(ref _authorizationFilters);
            }
            set {
                _authorizationFilters = value;
            }
        }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly",
            Justification = "Users may want to replace the entire collection.")]
        public IList<IExceptionFilter> ExceptionFilters {
            get {
                return GetListOrEmpty(ref _exceptionFilters);
            }
            set {
                _exceptionFilters = value;
            }
        }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly",
            Justification = "Users may want to replace the entire collection.")]
        public IList<IResultFilter> ResultFilters {
            get {
                return GetListOrEmpty(ref _resultFilters);
            }
            set {
                _resultFilters = value;
            }
        }

        private static IList<T> GetListOrEmpty<T>(ref IList<T> list) {
            if (list == null) {
                list = new List<T>();
            }
            return list;
        }
    }
}
