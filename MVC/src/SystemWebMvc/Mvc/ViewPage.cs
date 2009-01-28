namespace System.Web.Mvc {
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    [FileLevelControlBuilder(typeof(ViewPageControlBuilder))]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class ViewPage : Page, IViewDataContainer {

        private string _masterLocation;
        private ViewDataDictionary _viewData;

        public AjaxHelper Ajax {
            get;
            set;
        }

        public HtmlHelper Html {
            get;
            set;
        }

        public string MasterLocation {
            get {
                return _masterLocation ?? String.Empty;
            }
            set {
                _masterLocation = value;
            }
        }

        public object Model {
            get {
                return ViewData.Model;
            }
        }

        public TempDataDictionary TempData {
            get {
                return ViewContext.TempData;
            }
        }

        public UrlHelper Url {
            get;
            set;
        }

        public ViewContext ViewContext {
            get;
            set;
        }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly",
            Justification = "This is the mechanism by which the ViewPage gets its ViewDataDictionary object.")]
        public ViewDataDictionary ViewData {
            get {
                if (_viewData == null) {
                    SetViewData(new ViewDataDictionary());
                }
                return _viewData;
            }
            set {
                SetViewData(value);
            }
        }

        public HtmlTextWriter Writer {
            get;
            private set;
        }

        public virtual void InitHelpers() {
            Ajax = new AjaxHelper(ViewContext, this);
            Html = new HtmlHelper(ViewContext, this);
            Url = new UrlHelper(ViewContext.RequestContext);
        }

        [SuppressMessage("Microsoft.Security", "CA2109:ReviewVisibleEventHandlers",
            Justification = "This isn't really an event handler.")]
        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);

            ReplaceHtmlTitle();
        }

        [SuppressMessage("Microsoft.Security", "CA2109:ReviewVisibleEventHandlers")]
        protected override void OnPreInit(EventArgs e) {
            base.OnPreInit(e);

            if (!String.IsNullOrEmpty(MasterLocation)) {
                MasterPageFile = MasterLocation;
            }
        }

        protected override void Render(HtmlTextWriter writer) {
            Writer = writer;
            try {
                base.Render(writer);
            }
            finally {
                Writer = null;
            }
        }

        public virtual void RenderView(ViewContext viewContext) {
            ViewContext = viewContext;
            InitHelpers();
            // Tracing requires Page IDs to be unique.
            ID = Guid.NewGuid().ToString();
            ProcessRequest(HttpContext.Current);
        }

        internal void ReplaceHtmlTitle() {
            // If we have a <head runat="server"> tag, the <head> element will automatically try to add
            // a <title> child if one doesn't already exist. This was leading to a problem with putting
            // a <title> element within a ContentPlaceHolder inside the <head> element, since <title>
            // isn't a direct child of <head> so isn't detected by the runtime. The end result was that
            // two <title> elements were being output: one auto-injected by <head>, the other from the
            // ContentPlaceHolder tag. We try to detect this scenario and suppress the auto-injected
            // <title> element by replacing it with an object that renders nothing.

            // Special case: if the Page directive has a Title attribute <%@ Page Title = ... %>, we
            // want to render the default auto-injected title tag since it can understand this attribute.
            if (Header != null && !Header.Controls.OfType<HtmlTitle>().Any() && String.IsNullOrEmpty(Title)) {
                Header.Controls.Add(new EmptyTitle());
            }
        }

        protected virtual void SetViewData(ViewDataDictionary viewData) {
            _viewData = viewData;
        }

        private sealed class EmptyTitle : HtmlTitle {
            protected override void Render(HtmlTextWriter writer) {
            }
        }

    }
}
