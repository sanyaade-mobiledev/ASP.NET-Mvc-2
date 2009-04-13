using System;
using System.Web.DynamicData;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace Microsoft.Web.DynamicData {

    public class SimpleDynamicDataSource : DataSourceControl, IDynamicDataSource {
        protected override DataSourceView GetView(string viewName) {
            return new SimpleDynamicDataSourceView(this, viewName);
        }

        public ICustomTypeDescriptor CustomTypeDescriptor { get; set; }

        internal object DataObject { get; set; }

        public void SetDataObject(object dataObject, DetailsView detailsView) {

            DataObject = dataObject;
            TypeDescriptionProvider typeDescriptionProvider;

            CustomTypeDescriptor = dataObject as ICustomTypeDescriptor;
            Type dataObjectType;

            if (CustomTypeDescriptor == null) {
                dataObjectType = dataObject.GetType();
                typeDescriptionProvider = TypeDescriptor.GetProvider(DataObject);
                CustomTypeDescriptor = typeDescriptionProvider.GetTypeDescriptor(DataObject);
            }
            else {
                dataObjectType = GetEntityType();
                typeDescriptionProvider = new TrivialTypeDescriptionProvider(CustomTypeDescriptor);
            }

            // Set the context type and entity set name on ourselves. Note that in this scenario those
            // concepts are somewhat artificial, since we don't have a real context. 

            // Set the ContextType to the dataObjectType, which is a bit strange but harmless
            Type contextType = dataObjectType;

            ((IDynamicDataSource)this).ContextType = contextType;

            // We can set the entity set name to anything, but using the
            // DataObjectType makes some Dynamic Data error messages clearer.
            ((IDynamicDataSource)this).EntitySetName = dataObjectType.Name;

            MetaModel model = null;
            try {
                model = MetaModel.GetModel(contextType);
            }
            catch {
                model = new MetaModel();
                model.RegisterContext(
                    new SimpleModelProvider(contextType, dataObjectType, dataObject),
                    new ContextConfiguration() {
                        MetadataProviderFactory = (type => typeDescriptionProvider)
                    });
            }

            MetaTable table = model.GetTable(dataObjectType);

            if (detailsView != null) {
                detailsView.RowsGenerator = new AdvancedFieldGenerator(table, false);
            }
        }

        public event EventHandler<SimpleDynamicDataSourceCompleteEventArgs> Complete;

        internal void OnComplete(SimpleDynamicDataSourceCompleteEventArgs eventArgs) {
            if (Complete != null) {
                Complete(this, eventArgs);
            }
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

        private Type GetEntityType() {
            // TODO: need to somehow come up with a Type that's unique to the CustomTypeDescriptor being used!
            return typeof(DummyType);
        }

        internal class DummyType { }
    }
}

