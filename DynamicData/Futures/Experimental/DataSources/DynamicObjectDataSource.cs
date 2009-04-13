using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Web.Compilation;
using System.Web.DynamicData;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;

namespace Microsoft.Web.DynamicData {
    public class DynamicObjectDataSource : ObjectDataSource, IDynamicDataSource {
        internal Type DataObjectType { get; set; }

        public DynamicObjectDataSource() { }

        protected override void OnInit(EventArgs e) {
            base.OnInit(e);

            DataObjectType = BuildManager.GetType(DataObjectTypeName, true);

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
                var contextConfiguration = new ContextConfiguration() {
                    MetadataProviderFactory = (type => new InMemoryMetadataTypeDescriptionProvider(type, new AssociatedMetadataTypeTypeDescriptionProvider(type)))
                };

                var typeDescriptionProvider = contextConfiguration.MetadataProviderFactory(DataObjectType);
                var typeDescriptor = typeDescriptionProvider.GetTypeDescriptor(DataObjectType);
                model = new MetaModel();
                model.RegisterContext(
                    new SimpleModelProvider(contextType, DataObjectType, typeDescriptor),
                    contextConfiguration);
            }

            Inserted += new ObjectDataSourceStatusEventHandler(DynamicObjectDataSource_Inserted);
            Updated += new ObjectDataSourceStatusEventHandler(DynamicObjectDataSource_Updated);
        }

        void DynamicObjectDataSource_Updated(object sender, ObjectDataSourceStatusEventArgs e) {
            if (e.Exception != null) {
                OnException(new DynamicValidatorEventArgs(e.Exception.InnerException, DynamicDataSourceOperation.Update));
            }
        }

        void DynamicObjectDataSource_Inserted(object sender, ObjectDataSourceStatusEventArgs e) {
            if (e.Exception != null) {
                OnException(new DynamicValidatorEventArgs(e.Exception.InnerException, DynamicDataSourceOperation.Insert));
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
            get { return new ParameterCollection(); }
        }

        #endregion

        /// <summary>
        /// Dummy context class use to register each object type uniquely with ASP.NET
        /// </summary>
        internal class DummyContext<T> {
        }
    }
}
