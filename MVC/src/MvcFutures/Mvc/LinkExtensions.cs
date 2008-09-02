namespace Microsoft.Web.Mvc {
    using System;
    using System.Globalization;
    using System.Linq.Expressions;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public static class LinkExtensions {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings", Justification = "This is a UI method and is required to use strings as Uri"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an Extension Method which allows the user to provide a strongly-typed argument via Expression")]
        public static string BuildUrlFromExpression<T>(this HtmlHelper helper, Expression<Action<T>> action) where T : Controller {
            return LinkBuilder.BuildUrlFromExpression<T>(helper.ViewContext, action);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "This method is specifically for Expressions and requires T")]
        public static RouteValueDictionary BuildQueryStringFromExpression(MethodCallExpression call) {
            return LinkBuilder.BuildParameterValuesFromExpression(call);
        }

        /// <summary>
        /// Creates an anchor tag based on the passed in controller type and method
        /// </summary>
        /// <typeparam name="T">The Controller Type</typeparam>
        /// <param name="action">The Method to route to</param>
        /// <param name="linkText">The linked text to appear on the page</param>
        /// <returns>System.String</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an Extension Method which allows the user to provide a strongly-typed argument via Expression")]
        public static string ActionLink<T>(this HtmlHelper helper, Expression<Action<T>> action, string linkText) where T : Controller {
            return ActionLink<T>(helper, action, linkText, null);
        }

        /// <summary>
        /// Creates an anchor tag based on the passed in controller type and method
        /// </summary>
        /// <typeparam name="T">The Controller Type</typeparam>
        /// <param name="action">The Method to route to</param>
        /// <param name="linkText">The linked text to appear on the page</param>
        /// <returns>System.String</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an Extension Method which allows the user to provide a strongly-typed argument via Expression")]
        public static string ActionLink<T>(this HtmlHelper helper, Expression<Action<T>> action, string linkText, object htmlAttributes) where T : Controller {

            //TODO: refactor this to work with ActionLink in the core
            string linkFormat = "<a href=\"{0}\" {1}>{2}</a>";
            string atts = string.Empty;

            if (htmlAttributes != null)
                atts = HtmlExtensionUtility.ConvertObjectToAttributeList(htmlAttributes);

            string link = BuildUrlFromExpression(helper, action);
            string result = string.Format(CultureInfo.InvariantCulture, linkFormat, link, atts, helper.Encode(linkText));
            return result;
        }
    }
}
