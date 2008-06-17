using System;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Web.DynamicData.ModelProviders;
using System.Web.UI.WebControls;
using System.Web.DynamicData;
using System.Linq;
using System.Web.UI;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.Web.DynamicData.Extensions {

    public class SimpleDynamicDataSource : DataSourceControl, IDynamicDataSource {
        protected override DataSourceView GetView(string viewName) {
            return new SimpleDynamicDataSourceView(this, viewName);
        }

        internal object DataObject { get; set; }
        internal Type DataObjectType { get; set; }

        public void SetDataObject(object dataObject) {
            SetDataObject(dataObject, null);
        }

        public void SetDataObject(object dataObject, DetailsView detailsView) {
            SetDataObjectType(dataObject.GetType(), detailsView);
            DataObject = dataObject;
        }

        private void SetDataObjectType(Type dataObjectType, DetailsView detailsView) {

            DataObjectType = dataObjectType;

            // Make a generic type to give us a unique key to pass to the MetaModel API
            Type contextType = typeof(DummyContext<>).MakeGenericType(DataObjectType);

            // Set the context type and entity set name on ourselves. Note that in this scenario those
            // concept are somewhat artificial, since we don't have a real context. We can set the entity set name
            // to anything, but using the DataObjectType makes some Dynamic Data error messages clearer.
            ((IDynamicDataSource)this).ContextType = contextType;
            ((IDynamicDataSource)this).EntitySetName = DataObjectType.Name;

            MetaModel model = null;
            try {
                model = MetaModel.GetModel(contextType);
            }
            catch {
                model = new MetaModel();
                model.RegisterContext(
                    new SimpleDynamicDataSourceModelProvider(contextType, DataObjectType),
                    new ContextConfiguration() {
                        MetadataProviderFactory = (type => new InMemoryMetadataTypeDescriptionProvider(type, new AssociatedMetadataTypeTypeDescriptionProvider(type)))
                    });
            }

            MetaTable table = model.GetTable(DataObjectType);

            if (detailsView != null) {
                detailsView.RowsGenerator = new OrderedColumnsFieldGenerator(table);
            }
        }

        public event EventHandler<SimpleDynamicDataSourceCompleteEventArgs> Complete;

        internal void OnComplete(SimpleDynamicDataSourceCompleteEventArgs eventArgs) {
            Complete(this, eventArgs);
        }

        internal void OnException(DynamicValidatorEventArgs eventArgs) {
            Exception(this, eventArgs);
        }

        #region IDynamicDataSource Members

        bool IDynamicDataSource.AutoGenerateWhereClause { get; set; }

        Type IDynamicDataSource.ContextType { get; set; }

        bool IDynamicDataSource.EnableDelete { get; set; }

        bool IDynamicDataSource.EnableInsert { get; set; }

        bool IDynamicDataSource.EnableUpdate { get; set; }

        string IDynamicDataSource.EntitySetName { get; set; }

        public event EventHandler<DynamicValidatorEventArgs> Exception;

        string IDynamicDataSource.Where { get; set; }

        ParameterCollection IDynamicDataSource.WhereParameters {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region IDataSource Members

        event EventHandler IDataSource.DataSourceChanged {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        DataSourceView IDataSource.GetView(string viewName) {
            return GetView(viewName);
        }

        ICollection IDataSource.GetViewNames() {
            throw new NotImplementedException();
        }

        #endregion


        internal class SimpleDynamicDataSourceModelProvider : DataModelProvider {

            private ReadOnlyCollection<TableProvider> _tables;

            public SimpleDynamicDataSourceModelProvider(Type contextType, Type entityType) {
                ContextType = contextType;

                var tables = new Collection<TableProvider>();

                var table = new SimpleDynamicDataSourceTableProvider(this, entityType);
                tables.Add(table);

                _tables = new ReadOnlyCollection<TableProvider>(tables);
            }

            public override object CreateContext() {
                throw new NotImplementedException();
            }

            public override ReadOnlyCollection<TableProvider> Tables {
                get {
                    return _tables;
                }
            }
        }

        internal class SimpleDynamicDataSourceTableProvider : TableProvider {

            private ReadOnlyCollection<ColumnProvider> _columns;

            public SimpleDynamicDataSourceTableProvider(DataModelProvider model, Type entityType)
                : base(model) {
                Name = entityType.Name;
                EntityType = entityType;

                var columns = new Collection<ColumnProvider>();

                foreach (PropertyInfo columnProp in EntityType.GetProperties()) {
                    columns.Add(new SimpleDynamicDataSourceColumnProvider(this, columnProp));
                }

                _columns = new ReadOnlyCollection<ColumnProvider>(columns);
            }

            public override IQueryable GetQuery(object context) {
                throw new NotImplementedException();
            }

            public override ReadOnlyCollection<ColumnProvider> Columns {
                get {
                    return _columns;
                }
            }
        }

        internal class SimpleDynamicDataSourceColumnProvider : ColumnProvider {

            private PropertyInfo _prop;

            public SimpleDynamicDataSourceColumnProvider(TableProvider table, PropertyInfo prop)
                : base(table) {
                _prop = prop;
                Name = _prop.Name;
                ColumnType = _prop.PropertyType;
                EntityTypeProperty = prop;
                Nullable = true;

                IsSortable = true;
            }
        }
        
        /// <summary>
        /// Dummy context class use to register each object type uniquely with ASP.NET
        /// </summary>
        internal class DummyContext<T> {
        }
    }
}

