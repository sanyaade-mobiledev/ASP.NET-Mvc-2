using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Globalization;
using System.Web.Compilation;
using System.IO;

namespace DlrMvcLib {
    public class DlrView : IView {
        public DlrView(string viewPath)
            : this(viewPath, null) {
        }

        public DlrView(string viewPath, string masterPath) {
            if (String.IsNullOrEmpty(viewPath)) {
                throw new ArgumentException();
            }

            ViewPath = viewPath;
            MasterPath = masterPath ?? String.Empty;
        }

        public string MasterPath {
            get;
            private set;
        }

        public string ViewPath {
            get;
            private set;
        }

        public virtual void Render(ViewContext viewContext, TextWriter writer) {
            if (viewContext == null) {
                throw new ArgumentNullException("viewContext");
            }

            object viewInstance = BuildManager.CreateInstanceFromVirtualPath(ViewPath, typeof(object));
            if (viewInstance == null) {
                throw new InvalidOperationException(
                    String.Format(
                        CultureInfo.CurrentUICulture,
                        "",
                        ViewPath));
            }

            DlrViewPage viewPage = viewInstance as DlrViewPage;
            if (viewPage != null) {
                RenderViewPage(viewContext, viewPage);
                return;
            }

            ViewUserControl viewUserControl = viewInstance as ViewUserControl;
            if (viewUserControl != null) {
                RenderViewUserControl(viewContext, viewUserControl);
                return;
            }

            throw new InvalidOperationException(
                String.Format(
                    CultureInfo.CurrentUICulture,
                    "",
                    ViewPath));
        }

        private void RenderViewPage(ViewContext context, DlrViewPage page) {
            if (!String.IsNullOrEmpty(MasterPath)) {
                page.MasterLocation = MasterPath;
            }

            IViewDataContainer dataContainer = page as IViewDataContainer;
            if (dataContainer != null) {
                dataContainer.ViewData = context.ViewData;
            }
            
            page.RenderView(context);
        }

        private void RenderViewUserControl(ViewContext context, ViewUserControl control) {
            if (!String.IsNullOrEmpty(MasterPath)) {
                throw new InvalidOperationException();
            }

            control.ViewData = context.ViewData;
            control.RenderView(context);
        }
    }
}
