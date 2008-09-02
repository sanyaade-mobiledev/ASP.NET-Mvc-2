namespace Microsoft.Web.Mvc {
    using System;
    using System.IO;
    using System.Web;
    using System.Web.UI;
    using Microsoft.Web.Mvc.Resources;

    // TODO: This type is very difficult to unit-test because of an outstanding bug (DevDiv 201629)
    // that requires mocking much of the hosting environment before calling Page.ProcessRequest().
    // For now, we just perform functional tests of this feature.

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public sealed class DefaultAntiForgeryTokenSerializer : AntiForgeryTokenSerializer {

        private static TokenPersister _persister;
        private static readonly DefaultAntiForgeryTokenSerializer _singleton = new DefaultAntiForgeryTokenSerializer();

        private DefaultAntiForgeryTokenSerializer() {
        }

        public static DefaultAntiForgeryTokenSerializer Instance {
            get {
                return _singleton;
            }
        }

        private static AntiForgeryTokenValidationException CreateValidationException(Exception innerException) {
            return new AntiForgeryTokenValidationException(MvcResources.AntiForgeryToken_ValidationFailed, innerException);
        }

        public override AntiForgeryToken Deserialize(string serializedToken) {
            if (String.IsNullOrEmpty(serializedToken)) {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "serializedToken");
            }

            EnsurePersister();
            try {
                Triplet deserializedObj = (Triplet)_persister.StateFormatter.Deserialize(serializedToken);
                return new AntiForgeryToken() {
                    Salt = (string)deserializedObj.First,
                    Value = (string)deserializedObj.Second,
                    CreationDate = (DateTime)deserializedObj.Third
                };
            }
            catch (Exception ex) {
                throw CreateValidationException(ex);
            }
        }

        private static void EnsurePersister() {
            if (_persister == null) {
                _persister = TokenPersister.CreatePersister();
            }
        }

        public override string Serialize(AntiForgeryToken token) {
            if (token == null) {
                throw new ArgumentNullException("token");
            }

            Triplet objToSerialize = new Triplet() {
                First = token.Salt,
                Second = token.Value,
                Third = token.CreationDate
            };

            EnsurePersister();
            string serializedValue = _persister.StateFormatter.Serialize(objToSerialize);
            return serializedValue;
        }

        private sealed class TokenPersister : PageStatePersister {
            private TokenPersister(Page page)
                : base(page) {
            }

            public new IStateFormatter StateFormatter {
                get {
                    return base.StateFormatter;
                }
            }

            public static TokenPersister CreatePersister() {
                // This code instantiates a page and tricks it into thinking that it's servicing
                // a postback scenario with encrypted ViewState, which is required to make the
                // StateFormatter properly decrypt data.  Specifically, this code sets the
                // internal Page.ContainsEncryptedViewState flag.
                TextWriter writer = TextWriter.Null;
                HttpResponse response = new HttpResponse(writer);
                HttpRequest request = new HttpRequest("DummyFile.aspx", "http://localhost/DummyUrl", "__EVENTTARGET=true&__VIEWSTATEENCRYPTED=true");
                HttpContext context = new HttpContext(request, response);

                Page page = new Page() {
                    EnableViewStateMac = true,
                    ViewStateEncryptionMode = ViewStateEncryptionMode.Always
                };
                page.ProcessRequest(context);

                return new TokenPersister(page);
            }

            public override void Load() {
                throw new NotImplementedException();
            }

            public override void Save() {
                throw new NotImplementedException();
            }
        }

    }
}
