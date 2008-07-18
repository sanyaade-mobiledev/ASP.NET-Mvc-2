using System;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Permissions;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;

namespace Microsoft.Web.DynamicData.Mvc {
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public static class DynamicFieldErrorsExtensions {
        // Dynamic field errors based on lambda expressions (experimental!)

        public static string DynamicFieldErrors(this HtmlHelper html, Expression<Func<object>> expression) {
            object entity;
            string fieldName;

            if (!ExpressionUtility.TryGetEntityAndFieldNameFromExpression(expression.Body, out entity, out fieldName))
                throw new ArgumentException("DynamicField expression of unknown type: " + expression.Body.GetType().FullName + "\r\n" + expression.Body.ToString());

            return DynamicFieldErrors(html, entity, fieldName);
        }

        // Dynamic field errors based on entity + column

        public static string DynamicFieldErrors(this HtmlHelper html, object entity, MetaColumn column) {
            return DynamicFieldErrors(html, entity, column.Name);
        }

        // Dynamic field errors based on entity + field name

        public static string DynamicFieldErrors(this HtmlHelper html, object entity, string fieldName) {
            var state = html.ViewDataContainer.ViewData.ModelState().GetPropertyState(fieldName);
            if (state == null || state.PropertyErrors.Count < 1)
                return "";

            return "<span class='error'>" + state.PropertyErrors.Aggregate("", (result, error) => result += "* " + error.ErrorMessage + "<br />") + "</span>";
        }
    }
}