using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI;

namespace Microsoft.Web.DynamicData.Mvc {
    // REVIEW: This stuff is all hard-coded to DLINQ right now.
    //
    // I also don't like having access to ViewData here; this class should be ignorant of
    // how errors are reported, so things which can fail should optionally return model
    // errors and property errors. Ideally, we could factor something like this into the
    // base framework for Dynamic Data, once we figure out what the CUD interface is.

    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class DynamicDataHelper : IDisposable {
        DataContext _context;
        MetaTable _table;
        ViewDataDictionary _viewData;
        IViewDataContainer _viewDataContainer;

        public DynamicDataHelper(IViewDataContainer viewDataContainer, Type entityType) {
            _viewDataContainer = viewDataContainer;
            EntityType = entityType;
        }

        public DynamicDataHelper(IViewDataContainer viewDataContainer, string tableName) {
            _viewDataContainer = viewDataContainer;
            _table = Model.GetTable(tableName);
            EntityType = _table.EntityType;
        }

        public DynamicDataHelper(ViewDataDictionary viewData, Type entityType) {
            _viewData = viewData;
            EntityType = entityType;
        }

        public DynamicDataHelper(ViewDataDictionary viewData, string tableName) {
            _viewData = viewData;
            _table = Model.GetTable(tableName);
            EntityType = _table.EntityType;
        }

        public virtual void Dispose() {
            if (_context != null) {
                _context.Dispose();
                _context = null;
            }
        }

        public IEnumerable<MetaColumn> Columns {
            get { return Table.Columns; }
        }

        protected DataContext Context {
            get {
                if (_context == null) {
                    _context = (DataContext)Table.CreateContext();
                }

                return _context;
            }
        }

        public IEnumerable<MetaColumn> DisplayColumns {
            get { return Table.Columns.Where(c => c.Scaffold); }
        }

        public IEnumerable<MetaColumn> DisplayShortColumns {
            get { return Table.Columns.Where(c => c.Scaffold && !c.IsLongString); }
        }

        protected Type EntityType {
            get;
            private set;
        }

        public bool HasErrors {
            get;
            protected set;
        }

        public MetaModel Model {
            get { return ViewData.MetaModel(); }
            set { ViewData.SetMetaModel(value); }
        }

        public MetaTable Table {
            get {
                if (_table == null)
                    _table = Model.GetTable(EntityType);

                return _table;
            }
        }

        protected ViewDataDictionary ViewData {
            get {
                if (_viewData == null)
                    _viewData = _viewDataContainer.ViewData;

                return _viewData;
            }
        }

        protected void AddEntityError(string message) {
            HasErrors = true;
            ViewData.ModelErrors().Add(new ModelError(message));
        }

        protected void AddFieldError(MetaColumn column, string message) {
            HasErrors = true;
            ModelPropertyState modelPropertyState = ViewData.ModelState().GetPropertyState(column.Name);
            modelPropertyState.PropertyErrors.Add(new ModelPropertyError(message));
        }

        public bool Delete(object entity) {
            var table = Context.GetTable(EntityType);
            table.DeleteOnSubmit(entity);
            return SubmitChanges();
        }

        public bool FillEntity(NameValueCollection parameters, object entity) {
            foreach (string propName in parameters) {
                MetaColumn column;
                if (!Table.TryGetColumn(propName, out column))
                    continue;

                string propValue = parameters[propName];
                ViewData.ModelState().Add(new ModelPropertyState(propName, propValue));

                if (propValue == String.Empty && column.ConvertEmptyStringToNull)
                    propValue = null;

                if (propValue == null && column.IsRequired) {
                    string error = column.RequiredErrorMessage;
                    if (String.IsNullOrEmpty(error)) {
                        error = String.Format("The field '{0}' is required", column.Name);
                    }

                    AddFieldError(column, error);
                    continue;
                }

                object value = null;

                var fkColumn = column as MetaForeignKeyColumn;
                if (fkColumn != null) {    // For foreign keys, we need to get the parent entity in order to assign it
                    IQueryable parentEntityQuery = ExpressionUtility.ApplyWhereClause(
                        fkColumn.ParentTable.GetQuery(Context),
                        fkColumn.ParentTable,
                        fkColumn.ParentTable.PrimaryKeyColumns[0],
                        propValue);

                    value = parentEntityQuery.FirstOrDefault();
                }
                else if (propValue != null) {    // Convert it to the correct type for the field
                    TypeConverter converter = TypeDescriptor.GetConverter(column.ColumnType);

                    try {
                        value = converter.ConvertFromString(propValue);
                    }
                    catch (Exception e) {
                        AddFieldError(column, e.Message);
                        continue;
                    }
                }

                if (propValue != null) {
                    foreach (var validationAttribute in column.Attributes.OfType<ValidationAttribute>()) {
                        if (!validationAttribute.IsValid(value)) {
                            AddFieldError(column, validationAttribute.FormatErrorMessage(column.Name));
                        }
                    }
                }

                try {
                    column.EntityTypeProperty.SetValue(entity, value, null);
                }
                catch (TargetInvocationException e) {
                    AddFieldError(column, e.InnerException.Message);
                    continue;
                }
            }

            return !HasErrors;
        }

        public RouteValueDictionary GetRouteData(object entity) {
            RouteValueDictionary result = new RouteValueDictionary();
            GetRouteData(entity, result);
            return result;
        }

        public void GetRouteData(object entity, RouteValueDictionary routeValues) {
            foreach (var column in Table.PrimaryKeyColumns) {
                routeValues.Add(column.Name, DataBinder.GetPropertyValue(entity, column.Name));
            }
        }

        protected virtual IQueryable GetQuery() {
            return Table.GetQuery(Context);
        }

        public bool Insert(object entity) {
            Context.GetTable(EntityType).InsertOnSubmit(entity);
            return SubmitChanges();
        }

        bool SubmitChanges() {
            try {
                Context.SubmitChanges();
                return true;
            }
            catch (Exception e) {
                AddEntityError(e.Message);
                return false;
            }
        }

        public bool TryFillNewEntity(NameValueCollection parameters, out object entity) {
            entity = Activator.CreateInstance(EntityType);
            return FillEntity(parameters, entity);
        }

        // REVIEW: Will we assume that everybody will support object tracking? If so, we
        // can get rid of the entity passed here.
        public bool Update(object entity) {
            return SubmitChanges();
        }
    }

    public class DynamicDataHelper<TEntity> : DynamicDataHelper where TEntity : class, new() {
        public DynamicDataHelper(IViewDataContainer viewDataContainer) : base(viewDataContainer, typeof(TEntity)) { }

        public DynamicDataHelper(ViewDataDictionary viewData) : base(viewData, typeof(TEntity)) { }

        public IQueryable<TEntity> Query {
            get { return (IQueryable<TEntity>)GetQuery(); }
        }

        public bool Delete(TEntity entity) {
            return base.Delete(entity);
        }

        public bool FillEntity(NameValueCollection parameters, TEntity entity) {
            return base.FillEntity(parameters, entity);
        }

        public bool Insert(TEntity entity) {
            return base.Insert(entity);
        }

        public bool TryFillNewEntity(NameValueCollection parameters, out TEntity entity) {
            entity = new TEntity();
            return FillEntity(parameters, entity);
        }

        public bool Update(TEntity entity) {
            return base.Update(entity);
        }
    }
}