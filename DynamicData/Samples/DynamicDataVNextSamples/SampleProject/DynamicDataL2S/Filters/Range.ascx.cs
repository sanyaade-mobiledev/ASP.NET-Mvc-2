using System;
using System.Linq;
using System.Web.UI;
using System.Web.DynamicData;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.ComponentModel;

namespace DynamicDataProject.DynamicData.Filters {
    public partial class Range : QueryableFilterUserControl {
        public string Minimum {
            get {
                return MinTextBox.Text;
            }
        }

        public string Maximum {
            get {
                return MaxTextBox.Text;
            }
        }

        protected void Page_Load(object sender, EventArgs e) {
            MinTextBox.TextChanged += new EventHandler(TextChanged);
            MaxTextBox.TextChanged += new EventHandler(TextChanged);

            if (!Page.IsPostBack) {
                InitializeSlider();
            }
        }

        void TextChanged(object sender, EventArgs e) {
            OnFilterChanged();
        }

        private void InitializeSlider() {
            RangeAttribute rangeAttribute = Column.Attributes.OfType<RangeAttribute>().FirstOrDefault();
            if (rangeAttribute == null) {
                throw new InvalidOperationException(String.Format("A range filter was loaded for column '{0}' but that column does not have a RangeAttribute declared", Column.Name));
            }

            int minimum = Convert.ToInt32(rangeAttribute.Minimum);
            int maximum = Convert.ToInt32(rangeAttribute.Maximum);

            MultiHandleSliderExtender1.Minimum = minimum;
            MultiHandleSliderExtender1.Maximum = maximum;
            MinTextBox.Text = minimum.ToString();
            MaxTextBox.Text = maximum.ToString();
        }

        public override IQueryable GetQueryable(IQueryable source) {
            ParameterExpression parameterExpression = Expression.Parameter(source.ElementType, "item");
            Expression body = BuildQueryBody(parameterExpression);

            LambdaExpression lambda = Expression.Lambda(body, parameterExpression);
            MethodCallExpression whereCall = Expression.Call(typeof(Queryable), "Where", new Type[] { source.ElementType }, source.Expression, Expression.Quote(lambda));
            return source.Provider.CreateQuery(whereCall);
        }

        private Expression BuildQueryBody(ParameterExpression parameterExpression) {
            Expression propertyExpression = GetValue(CreatePropertyExpression(parameterExpression, Column.Name));
            TypeConverter converter = TypeDescriptor.GetConverter(Column.ColumnType);
            BinaryExpression minimumComparison = BuildCompareExpression(propertyExpression, converter.ConvertFromString(Minimum), Expression.GreaterThanOrEqual);
            BinaryExpression maximumComparison = BuildCompareExpression(propertyExpression, converter.ConvertFromString(Maximum), Expression.LessThanOrEqual);
            return Expression.AndAlso(minimumComparison, maximumComparison);
        }

        private BinaryExpression BuildCompareExpression(Expression propertyExpression, object value, Func<Expression, Expression, BinaryExpression> comparisonFunction) {
            ConstantExpression valueExpression = Expression.Constant(value);
            BinaryExpression comparison = comparisonFunction(propertyExpression, valueExpression);
            return comparison;
        }
    }
}