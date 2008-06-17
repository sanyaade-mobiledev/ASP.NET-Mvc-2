using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.DynamicData;
using Microsoft.Web.DynamicData.Extensions;

namespace DynamicDataExtensionsSample {
    public partial class Integer_Filter : FilterUserControlBase, ISelectionChangedAware {

        public event EventHandler SelectionChanged {
            add {
                DropDownList1.SelectedIndexChanged += value;
            }
            remove {
                DropDownList1.SelectedIndexChanged -= value;
            }
        }

        protected void Page_Init(object sender, EventArgs e) {
            var items = Column.Table.GetQuery();

            // row
            var entityParam = Expression.Parameter(Column.Table.EntityType, "row");
            // row => row.Property
            var columnLambda = Expression.Lambda(Expression.Property(entityParam, Column.EntityTypeProperty), entityParam);
            // Items.Select(row => row.Property)
            var selectCall = Expression.Call(typeof(Queryable), "Select", new Type[] { items.ElementType, columnLambda.Body.Type }, items.Expression, columnLambda);
            // Items.Select(row => row.Property).Distinct
            var distinctCall = Expression.Call(typeof(Queryable), "Distinct", new Type[] { Column.EntityTypeProperty.PropertyType }, selectCall);

            var result = items.Provider.CreateQuery(distinctCall);

            foreach (var item in result) {
                DropDownList1.Items.Add(item.ToString());
            }
        }

        public override string SelectedValue {
            get {
                return DropDownList1.SelectedValue;
            }
        }
    }
}