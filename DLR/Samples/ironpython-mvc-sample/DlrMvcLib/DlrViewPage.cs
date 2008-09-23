using System.Web.Mvc;
using Microsoft.Web.Scripting.UI;
using System.Web;
using System.Web.UI;

namespace DlrMvcLib {
    public class DlrViewPage : ScriptPage {
        ViewPage _viewPage = new ViewPage();

        public AjaxHelper Ajax {
            get {
                return _viewPage.Ajax;
            }
            set {
                _viewPage.Ajax = value;
            }
        }

        public HtmlHelper Html {
            get {
                return _viewPage.Html;
            }
            set {
                _viewPage.Html = value;
            }
        }

        public void InitHelpers() {
            _viewPage.InitHelpers();
        }

        public string MasterLocation {
            get {
                return _viewPage.MasterLocation;
            }
            set {
                _viewPage.MasterLocation = value;
            }
        }

        public void RenderView(ViewContext viewContext) {
            _viewPage.ViewContext = viewContext;
            _viewPage.InitHelpers();
            ((IHttpHandler)this).ProcessRequest(HttpContext.Current);
        }

        public TempDataDictionary TempData {
            get { 
                return ViewContext.TempData;    
            }
        }

        public UrlHelper Url {
            get {
                return _viewPage.Url;
            }
            set {
                _viewPage.Url = value;
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

        public ViewContext ViewContext {
            get {
                return _viewPage.ViewContext;
            }
            set {
                _viewPage.ViewContext = value;
            }
        }

        public HtmlTextWriter Writer {
            get;
            private set;
        }
        

        public ViewDataDictionary ViewData {
            get {
                return ViewContext.ViewData;
            }
        }
    }
}
