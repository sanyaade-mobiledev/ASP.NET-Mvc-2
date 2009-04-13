using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Web.DynamicData.ModelProviders;

namespace Microsoft.Web.Data.Services.Client {
    public class DataServiceDataModelProvider : DataModelProvider {
        private ReadOnlyCollection<TableProvider> _tables;

        public DataServiceDataModelProvider(Type contextType) {
            if (contextType == null) throw new ArgumentNullException("contextType");

            this.ContextType = contextType;

            var tables = new Collection<TableProvider>();

            foreach (PropertyInfo prop in DataServiceUtilities.EnumerateEntitySetProperties(contextType)) {
                tables.Add(new DataServiceTableProvider(this, prop));
            }

            this._tables = new ReadOnlyCollection<TableProvider>(tables);

            foreach (var table in tables) {
                ((DataServiceTableProvider)table).Initialize();
            }
        }

        public override object CreateContext() {
            return Activator.CreateInstance(ContextType);
        }

        public override ReadOnlyCollection<TableProvider> Tables {
            get { return _tables; }
        }
    }
}

