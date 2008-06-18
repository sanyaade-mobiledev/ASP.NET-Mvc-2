using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Web.DynamicData.ModelProviders;
using System.Web.UI;

namespace Microsoft.Web.DynamicData.Extensions {
    internal class DataServiceTableProvider : TableProvider {

        private PropertyInfo _prop;
        private ReadOnlyCollection<ColumnProvider> _columns;

        public DataServiceTableProvider(DataModelProvider model, PropertyInfo prop)
            : base(model) {
            _prop = prop;
            Name = prop.Name;
            EntityType = _prop.PropertyType.GetGenericArguments()[0];

            var columns = new Collection<ColumnProvider>();

            bool first = true;
            foreach (PropertyInfo columnProp in EntityType.GetProperties()) {
                columns.Add(new DataServiceColumnProvider(this, columnProp, first));
                first = false;
            }

            _columns = new ReadOnlyCollection<ColumnProvider>(columns);
        }

        public override IQueryable GetQuery(object context) {
            object contextPropValue = _prop.GetValue(context, null);

            if (contextPropValue is IQueryable)
                return (IQueryable)contextPropValue;

            IList list = (IList)contextPropValue;
            return list.AsQueryable();
        }

        public override ReadOnlyCollection<ColumnProvider> Columns {
            get {
                return _columns;
            }
        }

        public override object EvaluateForeignKey(object row, string foreignKeyName) {
            return DataBinder.Eval(row, foreignKeyName);
        }

        internal void Initialize() {
            foreach (DataServiceColumnProvider column in Columns) {
                column.Initialize();
            }
        }
    }
}

