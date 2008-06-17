using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web;
using System.Data.Services.Client;
using System.Web.DynamicData;
using System.Reflection;

namespace Microsoft.Web.DynamicData.Extensions {
    public class DataServiceLinqDataSource : LinqDataSource {

        protected override void OnInit(EventArgs args) {
            base.OnInit(args);

            MetaTable table = this.GetTable();

            ContextCreating += delegate(object sender, LinqDataSourceContextEventArgs e) {
                e.ObjectInstance = table.CreateContext();
            };

            Selecting += delegate(object sender, LinqDataSourceSelectEventArgs e) {
                var context = (DataServiceContext)table.CreateContext();

                var query = table.GetQuery(context);
                if (!String.IsNullOrEmpty(table.ForeignKeyColumnsNames)) {
                    MethodInfo addQueryOptionMethod = query.GetType().GetMethod("AddQueryOption");
                    if (addQueryOptionMethod != null) {
                        query = (IQueryable)addQueryOptionMethod.Invoke(query, new object[] { "$expand", table.ForeignKeyColumnsNames });
                        e.Result = query;
                    }
                }

                e.Arguments.RetrieveTotalRowCount = false;
                e.Arguments.TotalRowCount = e.Arguments.StartRowIndex + 11;
            };
        }

        protected override LinqDataSourceView CreateView() {
            return new DataServiceLinqDataSourceView(this, "DefaultView", Context);
        }
    }

    public class DataServiceLinqDataSourceView : LinqDataSourceView {
        public DataServiceLinqDataSourceView(LinqDataSource owner, string name, HttpContext context)
            : base(owner, name, context) { }

        protected override void ValidateContextType(Type contextType, bool selecting) { }
        protected override void ValidateTableType(Type tableType, bool selecting) { }

        protected override void DeleteDataObject(object dataContext, object table, object oldDataObject) {
            var dataServiceContext = (DataServiceContext)dataContext;
            dataServiceContext.AttachTo(TableName, oldDataObject);
            dataServiceContext.DeleteObject(oldDataObject);
            dataServiceContext.SaveChanges();
        }

        protected override void InsertDataObject(object dataContext, object table, object newDataObject) {
            var dataServiceContext = (DataServiceContext)dataContext;
            dataServiceContext.AddObject(TableName, newDataObject);
            dataServiceContext.SaveChanges();
        }

        protected override void UpdateDataObject(object dataContext, object table, object oldDataObject, object newDataObject) {

            // TODO: TEMPORARY simplistic implementation doesn't deal with optmisitic concurrency

            var dataServiceContext = (DataServiceContext)dataContext;
            dataServiceContext.AttachTo(TableName, newDataObject);
            dataServiceContext.UpdateObject(newDataObject);
            DataServiceResponse dataServiceResponse = dataServiceContext.SaveChanges();
        }
    }
}
