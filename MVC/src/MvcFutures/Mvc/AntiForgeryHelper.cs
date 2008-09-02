namespace Microsoft.Web.Mvc {
    using System;
    using System.Web;
    using System.Web.Mvc;
    using Token = AntiForgeryToken;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class AntiForgeryHelper {

        private const string TokenName = "__MVC_AntiForgeryToken";

        private AntiForgeryTokenSerializer _serializer;

        protected AntiForgeryTokenSerializer Serializer {
            get {
                return _serializer ?? DefaultAntiForgeryTokenSerializer.Instance;
            }
            set {
                _serializer = value;
            }
        }

        public string AntiForgeryToken(HtmlHelper helper, string salt) {
            if (helper == null) {
                throw new ArgumentNullException("helper");
            }

            string formValue = GetAntiForgeryToken(helper.ViewContext.HttpContext, salt);

            TagBuilder builder = new TagBuilder("input");
            builder.Attributes["type"] = "hidden";
            builder.Attributes["name"] = TokenName;
            builder.Attributes["value"] = formValue;
            return builder.ToString(TagRenderMode.SelfClosing);
        }

        private string GetAntiForgeryToken(HttpContextBase httpContext, string salt) {
            Token cookieToken;
            HttpCookie cookie = httpContext.Request.Cookies[TokenName];
            if (cookie != null) {
                cookieToken = Serializer.Deserialize(cookie.Value);
            }
            else {
                cookieToken = Token.NewToken();
                string cookieValue = Serializer.Serialize(cookieToken);
                HttpCookie newCookie = new HttpCookie(TokenName, cookieValue);
                httpContext.Response.Cookies.Set(newCookie);
            }


            Token formToken = new AntiForgeryToken(cookieToken) {
                CreationDate = DateTime.Now,
                Salt = salt
            };
            string formValue = Serializer.Serialize(formToken);
            return formValue;
        }

    }
}
