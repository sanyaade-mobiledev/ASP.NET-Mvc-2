using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Web;
using System.Globalization;

namespace WebFormRouting {
    public class HtmlHelper 
    {
        IRoutablePage _page;
        RouteCollection _routes;

        public HtmlHelper(IRoutablePage page, RouteCollection routes) 
        {
            _page = page;
            _routes = routes;
        }

        public virtual string RouteLink(string text, string name) {
            return RouteLink(text, name, null);
        }

        public virtual string RouteLink(string text, object values) {
            return RouteLink(text, new RouteValueDictionary(values));
        }

        public virtual string RouteLink(string text, RouteValueDictionary values) {
            return RouteLink(text, values, null);
        }

        public virtual string RouteLink(string text, object values, object htmlAttributes) {
            return RouteLink(text, new RouteValueDictionary(values), new RouteValueDictionary(htmlAttributes));
        }

        public virtual string RouteLink(string text, RouteValueDictionary values, IDictionary<string, object> htmlAttributes) {
            return RouteLink(text, null, values, htmlAttributes);
        }

        public virtual string RouteLink(string text, string name, object values) {
            return RouteLink(text, name, new RouteValueDictionary(values));
        }

        public virtual string RouteLink(string text, string name, RouteValueDictionary values) {
            return RouteLink(text, name, values, null);
        }

        public virtual string RouteLink(string text, string name, object values, object htmlAttributes) {
            return RouteLink(text, null, new RouteValueDictionary(values), new RouteValueDictionary(htmlAttributes));
        }

        public virtual string RouteLink(string text, string name, RouteValueDictionary values, IDictionary<string, object> htmlAttributes) 
        {
            string url = _page.Url.RouteUrl(name, values);

            string attributeHtml = string.Empty;
            if (htmlAttributes != null && htmlAttributes.Count > 0) {
                foreach (var key in htmlAttributes.Keys) {
                    attributeHtml += " " + key + "=\"" + HttpUtility.HtmlAttributeEncode(Convert.ToString(htmlAttributes[key], CultureInfo.InvariantCulture)) + "\"";
                }
            }

            if (!String.IsNullOrEmpty(url)) {
                //I do something funky here with the link text for demo purposes
                return string.Format("<a href=\"{0}\"{1}>{2}</a>", url, attributeHtml, text);
            }
            return null;
        }


    }
}
