namespace DlrMvcLib {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Web.Scripting.UI;
    using System.Web.Mvc;
    using System.Web.UI;

    public class DlrViewMaster : ScriptMaster {
        public AjaxHelper Ajax {
            get {
                return ViewPage.Ajax;
            }
        }

        public HtmlHelper Html {
            get {
                return ViewPage.Html;
            }
        }

        public TempDataDictionary TempData {
            get {
                return ViewPage.TempData;
            }
        }

        public UrlHelper Url {
            get {
                return ViewPage.Url;
            }
        }

        public ViewContext ViewContext {
            get {
                return ViewPage.ViewContext;
            }
        }

        public ViewDataDictionary ViewData {
            get {
                return ViewPage.ViewData;
            }
        }

        public HtmlTextWriter Writer {
            get {
                return ViewPage.Writer;
            }
        }

        internal DlrViewPage ViewPage {
            get {
                DlrViewPage viewPage = Page as DlrViewPage;
                if (viewPage == null) {
                    throw new InvalidOperationException();
                }
                return viewPage;
            }
        }    
    }
}
