namespace System.Web.Mvc {
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using System.Web.UI;

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
            Ajax = new AjaxHelper(ViewContext);
            Html = new HtmlHelper(ViewContext, this);
            Url = new UrlHelper(ViewContext);
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

        protected virtual void SetViewData(ViewDataDictionary viewData) {
            _viewData = viewData;
        }
    }
}
