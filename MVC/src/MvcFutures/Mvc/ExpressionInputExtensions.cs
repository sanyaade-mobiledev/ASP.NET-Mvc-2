namespace Microsoft.Web.Mvc {
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq.Expressions;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using Microsoft.Web.Mvc.Internal;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public static class ExpressionInputExtensions {
        private const int TextAreaRows = 2;
        private const int TextAreaColumns = 20;

        public static string TextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression) where TModel : class {
            return htmlHelper.TextBoxFor(expression, new RouteValueDictionary());
        }

        public static string TextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes) where TModel : class {
            return htmlHelper.TextBoxFor(expression, new RouteValueDictionary(htmlAttributes));
        }

        public static string TextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes) where TModel : class {
            string inputName = ExpressionHelper.GetInputName(expression);

            // REVIEW:  We may not want to actually use the expression to get the default value.
            //          For example, if the property is an Int32, we may want to render blank, not 0.
            //          Consider checking the modelstate first for a value, before we get the value from the expression.
            TProperty value = GetValue(htmlHelper, expression);
            return htmlHelper.TextBox(inputName, value, htmlAttributes);
        }

        public static string TextAreaFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression) where TModel : class {
            return htmlHelper.TextAreaFor(expression, TextAreaRows, TextAreaColumns, new RouteValueDictionary());
        }

        public static string TextAreaFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes) where TModel : class {
            return htmlHelper.TextAreaFor(expression, TextAreaRows, TextAreaColumns, new RouteValueDictionary(htmlAttributes));
        }

        public static string TextAreaFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes) where TModel : class {
            return htmlHelper.TextAreaFor(expression, TextAreaRows, TextAreaColumns, htmlAttributes);
        }

        public static string TextAreaFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, int rows, int columns, IDictionary<string, object> htmlAttributes) where TModel : class {
            string inputName = ExpressionHelper.GetInputName(expression);

            // REVIEW:  We may not want to actually use the expression to get the default value.
            //          For example, if the property is an Int32, we may want to render blank, not 0.
            //          Consider checking the modelstate first for a value, before we get the value from the expression.
            TProperty value = GetValue(htmlHelper, expression);
            return htmlHelper.TextArea(inputName, Convert.ToString(value, CultureInfo.CurrentCulture), rows, columns, htmlAttributes);
        }

        private static TProperty GetValue<TModel, TProperty>(HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression) where TModel : class {
            TModel model = htmlHelper.ViewData.Model;
            if (model == null) {
                return default(TProperty);
            }
            Func<TModel, TProperty> func = expression.Compile();
            return func(model);
        }
    }
}
