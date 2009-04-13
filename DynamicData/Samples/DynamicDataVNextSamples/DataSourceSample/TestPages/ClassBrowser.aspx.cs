using System;
using System.Linq;
using System.Web.UI.WebControls;
using DataSourcesDemo.ClassBrowser;
using Microsoft.Web.Data.UI.WebControls.Expressions;

namespace DataSourcesDemo {
    public partial class Reflector : System.Web.UI.Page {
    protected override void OnInit(EventArgs e) {
            base.OnInit(e);
            assemblies.DataSource = ReflectionDomainService.LoadedAssemblies.OrderBy(a => a.Key, StringComparer.OrdinalIgnoreCase);
            assemblies.DataBind();
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);
            Form.DefaultButton = search.UniqueID;

            search.Click += (s, args) => {
                types.SelectedIndex = -1;
            };
        }

        protected void OnQuery(object sender, CustomExpressionEventArgs e) {
            bool genericTypes = Convert.ToBoolean(e.Values.First().Value);
            if (genericTypes) {
                e.Query = e.Query.Cast<TypeWrapper>().Where(t => t.Type.IsGenericType);
            }
        }

        protected void OnTypesCommand(object sender, ListViewCommandEventArgs e) {
            if (e.CommandName == "Minimize") {                
                types.SelectedIndex = -1;
            }
        }                      
    }
}