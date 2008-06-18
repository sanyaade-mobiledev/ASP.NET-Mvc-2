using System;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Web.DynamicData.ModelProviders;
using System.Web.UI.WebControls;
using System.Web.DynamicData;
using System.Linq;
using System.Data.Services.Client;
using System.Web;
using System.Web.UI;

namespace Microsoft.Web.DynamicData.Extensions {
    public class DataServiceModelProvider : DataModelProvider {

        private ReadOnlyCollection<TableProvider> _tables;
        private Uri _serviceRoot;

        public DataServiceModelProvider(Type contextType, Uri serviceRoot) {
            if (contextType == null) throw new ArgumentNullException("contextType");
            if (serviceRoot == null) throw new ArgumentNullException("serviceRoot");

            this.ContextType = contextType;
            this._serviceRoot = serviceRoot;

            object context = CreateContext();
            ContextType = context.GetType();

            var tables = new Collection<TableProvider>();

            foreach (PropertyInfo prop in ContextType.GetProperties()) {
                if (prop.PropertyType.GetGenericArguments().Length != 1)
                    continue;

                tables.Add(new DataServiceTableProvider(this, prop));
            }

            _tables = new ReadOnlyCollection<TableProvider>(tables);

            foreach (var table in tables) {
                ((DataServiceTableProvider)table).Initialize();
            }
        }

        public override object CreateContext() {
            return Activator.CreateInstance(ContextType, this._serviceRoot);
        }

        public override ReadOnlyCollection<TableProvider> Tables {
            get {
                return _tables;
            }
        }
    }
}

