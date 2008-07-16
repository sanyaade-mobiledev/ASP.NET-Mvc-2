namespace Microsoft.Web.Mvc {
    using System;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Collections.Generic;

    public class RequireHttpMethodAttribute : ActionFilterAttribute {
        public RequireHttpMethodAttribute() : this(null) { 
        }

        public RequireHttpMethodAttribute(params string[] methods) {
            if (methods == null) {
                methods = new string[0];
            }
            Methods = methods;
        }

        public ICollection<string> Methods {
            get;
            private set;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            string currentMethod = filterContext.HttpContext.Request.HttpMethod;

            if (Methods.Any(method => method != null && String.Equals(method.Trim(), currentMethod, StringComparison.OrdinalIgnoreCase))) {
                return;
            }
            
            throw new HttpException(405, "Method Not Allowed"); //TODO: Resource string.
        }
    }
}
