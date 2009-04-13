using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Data.Services.Common;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.DynamicData;
using System.Web.UI;
//using System.Web.UI.WebControls;
using Microsoft.Web.Data.UI.WebControls;
using System.Collections;

namespace Microsoft.Web.Data.Services.Client {
    public class DataServiceLinqDataSource : LinqDataSource {
        protected override void OnInit(EventArgs args) {
            base.OnInit(args);

            Selecting += delegate(object sender, LinqDataSourceSelectEventArgs e) {
                e.Arguments.RetrieveTotalRowCount = false;
                e.Arguments.TotalRowCount = e.Arguments.StartRowIndex + 11;
            };
        }

        protected override LinqDataSourceView CreateView() {
            return new DataServiceLinqDataSourceView(this, "DefaultView", Context);
        }

        /// <summary>
        /// This method is only callable when using Dynamic Data.  It causes the related entities to be
        /// preloaded.  e.g. in Product, the Category and Supplier are made available.
        /// </summary>
        public void AutoLoadForeignKeys() {

            Selecting += delegate(object sender, LinqDataSourceSelectEventArgs e) {
                MetaTable table = this.GetTable();

                var context = (DataServiceContext)table.CreateContext();

                var query = table.GetQuery(context);
                if (!String.IsNullOrEmpty(table.ForeignKeyColumnsNames)) {
                    MethodInfo addQueryOptionMethod = query.GetType().GetMethod("AddQueryOption");
                    if (addQueryOptionMethod != null) {
                        query = (IQueryable)addQueryOptionMethod.Invoke(query, new object[] { "$expand", table.ForeignKeyColumnsNames });
                        e.Result = query;
                    }
                }
            };
        }
    }

    public class DataServiceLinqDataSourceView : LinqDataSourceView {
        private Dictionary<string, string> _etagMap;
        private DataServiceContext _dataServiceContext;

        public DataServiceLinqDataSourceView(DataServiceLinqDataSource owner, string name, HttpContext context)
            : base(owner, name, context) {
        }

        protected override void LoadViewState(object savedState) {
            if (savedState == null) {
                base.LoadViewState(null);
            }
            else {
                var myState = (Pair)savedState;
                base.LoadViewState(myState.First);
                _etagMap = (Dictionary<string, string>)myState.Second;
            }
        }

        protected override object SaveViewState() {
            var myState = new Pair();
            myState.First = base.SaveViewState();
            if (_dataServiceContext != null) {
                myState.Second = _dataServiceContext.Entities.Where(ed => !String.IsNullOrEmpty(ed.ETag)).ToDictionary(
                    ed => DataServiceUtilities.BuildCompositeKey(ed.Entity), ed => ed.ETag);
            }
            return myState;
        }

        protected override void OnContextCreated(LinqDataSourceStatusEventArgs e) {
            base.OnContextCreated(e);

            _dataServiceContext = (DataServiceContext)e.Result;
        }

        protected override void ValidateContextType(Type contextType, bool selecting) { }
        protected override void ValidateTableType(Type tableType, bool selecting) { }

        protected override IEnumerable ExecuteSelect(DataSourceSelectArguments arguments) {

            // REVIEW: TEMPORARY: Astoria has a bug which causes it to throw a NullReferenceException
            // when it can't connect ot the server.  Catch it and throw something clearer.
            // When Astoria fixes this, we can remove this logic
            try {
                return base.ExecuteSelect(arguments);
            }
            catch (TargetInvocationException e) {
                if (e.InnerException is NullReferenceException)
                    throw new Exception(String.Format(
                        "An error occured trying to connect to the data service using the context '{0}'",
                        ContextType.FullName));

                throw;
            }
        }

        protected override void DeleteDataObject(object dataContext, object table, object oldDataObject) {
            var dataServiceContext = (DataServiceContext)dataContext;
            var idataServiceContext = dataContext as IDataServiceContext;
            string etag = null;
            if (this._etagMap != null && this._etagMap.TryGetValue(DataServiceUtilities.BuildCompositeKey(oldDataObject), out etag)) {
                if (idataServiceContext != null)
                    idataServiceContext.AttachTo(TableName, oldDataObject, etag);
                else
                    dataServiceContext.AttachTo(TableName, oldDataObject, etag);
            }
            else {
                if (idataServiceContext != null)
                    idataServiceContext.AttachTo(TableName, oldDataObject);
                else
                    dataServiceContext.AttachTo(TableName, oldDataObject);
            }
            dataServiceContext.DeleteObject(oldDataObject);
            dataServiceContext.SaveChanges();
        }

        public override void Insert(IDictionary values, DataSourceViewOperationCallback callback) {

            // Keep track of the values to do foreign key processing in InsertDataObject
            _values = values;
            base.Insert(values, callback);
        }

        protected override void InsertDataObject(object dataContext, object table, object newDataObject) {
            var dataServiceContext = (DataServiceContext)dataContext;
            var idataServiceContext = dataContext as IDataServiceContext;
            if (idataServiceContext != null)
                idataServiceContext.AddObject(TableName, newDataObject);
            else
                dataServiceContext.AddObject(TableName, newDataObject);
            ProcessForeignKeys(dataServiceContext, newDataObject, _values);
            dataServiceContext.SaveChanges();
        }

        private IDictionary _values;
        public override void Update(System.Collections.IDictionary keys, IDictionary values, IDictionary oldValues, System.Web.UI.DataSourceViewOperationCallback callback) {

            // Keep track of the values to do foreign key processing in UpdateDataObject
            _values = values;
            base.Update(keys, values, oldValues, callback);
        }

        protected override void UpdateDataObject(object dataContext, object table, object oldDataObject, object newDataObject) {

            var dataServiceContext = (DataServiceContext)dataContext;
            var idataServiceContext = dataContext as IDataServiceContext;
            string etag = null;
            if (this._etagMap != null && this._etagMap.TryGetValue(DataServiceUtilities.BuildCompositeKey(oldDataObject), out etag)) {
                if (idataServiceContext != null)
                    idataServiceContext.AttachTo(TableName, newDataObject, etag);
                else
                    dataServiceContext.AttachTo(TableName, newDataObject, etag);
            }
            else {
                if (idataServiceContext != null)
                    idataServiceContext.AttachTo(TableName, newDataObject);
                else
                    dataServiceContext.AttachTo(TableName, newDataObject);
            }

            ProcessForeignKeys(dataServiceContext, newDataObject, _values);

            dataServiceContext.UpdateObject(newDataObject);
            DataServiceResponse dataServiceResponse = dataServiceContext.SaveChanges();
        }

        private void ProcessForeignKeys(DataServiceContext dataServiceContext, object dataObject, IDictionary values) {
            foreach (string key in values.Keys) {
                // Check if it looks like a FK, e.g. Category.CategoryID
                string[] parts = key.Split('.');
                if (parts.Length != 2)
                    continue;

                // Get the name of the entity ref property, e.g. Category
                string entityRefPropertyName = parts[0];

                // Create an 'empty' relaty entity, e.g a Category
                PropertyInfo propInfo = dataObject.GetType().GetProperty(entityRefPropertyName);
                object entityRefObject = Activator.CreateInstance(propInfo.PropertyType);

                // Set the PK in the related entity, e.g. set the CategoryID in the Category
                PropertyInfo subPropInfo = propInfo.PropertyType.GetProperty(parts[1]);
                subPropInfo.SetValue(
                    entityRefObject,
                    Convert.ChangeType(values[key], subPropInfo.PropertyType),
                    null);

                // Find the entity set property for the association
                var entitySetProp = DataServiceUtilities.FindEntitySetProperty(
                    dataServiceContext.GetType(), propInfo.PropertyType);

                // Attach the related entity and set it as the link on the main entity
                if (entitySetProp != null) {
                    dataServiceContext.AttachTo(entitySetProp.Name, entityRefObject);
                    dataServiceContext.SetLink(dataObject, entityRefPropertyName, entityRefObject);
                }
            }
        }

        private static bool IsKeyColumn(PropertyInfo pi) {
            // Astoria convention:
            // 1) try the DataServiceKey attribute
            // 2) if not attribute, try <typename>ID
            // 3) finally, try just ID

            object[] attribs = pi.DeclaringType.GetCustomAttributes(typeof(DataServiceKeyAttribute), true);
            if (attribs != null && attribs.Length > 0) {
                Debug.Assert(attribs.Length == 1);
                return ((DataServiceKeyAttribute)attribs[0]).KeyNames.Contains(pi.Name);
            }

            if (pi.Name.Equals(pi.DeclaringType.Name + "ID", System.StringComparison.OrdinalIgnoreCase)) {
                return true;
            }

            if (pi.Name == "ID") {
                return true;
            }

            return false;
        }
    }
}
