using System.Linq;
using System.Security.Permissions;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;

namespace Microsoft.Web.DynamicData.Mvc {
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class DynamicScaffoldController<TEntity> : Controller where TEntity : class, new() {
        protected virtual IQueryable<TEntity> EntitiesQuery {
            get {
                var query = DynamicData.Query;

                foreach (string name in Request.QueryString) {
                    MetaColumn column;
                    if (!DynamicData.Table.TryGetColumn(name, out column))
                        continue;

                    query = ExpressionUtility.ApplyWhereClause(query, DynamicData.Table, column, Request.QueryString[name]);
                }

                return query;
            }
        }

        protected virtual IQueryable<TEntity> EntityQuery {
            get {
                var query = EntitiesQuery;

                foreach (var column in DynamicData.Table.PrimaryKeyColumns) {
                    object value = Request.QueryString[column.Name];
                    if (value == null)
                        return null;

                    query = ExpressionUtility.ApplyWhereClause(query, DynamicData.Table, column, value);
                }

                return query;
            }
        }

        /// <inheritdoc/>
        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            DynamicData = new DynamicDataHelper<TEntity>(ViewData);
            ViewData.SetDynamicData(DynamicData);

            base.OnActionExecuting(filterContext);
        }

        /// <inheritdoc/>
        protected override void OnResultExecuted(ResultExecutedContext filterContext) {
            base.OnResultExecuted(filterContext);

            if (DynamicData != null) {
                DynamicData.Dispose();
                DynamicData = null;
            }
        }

        protected DynamicDataHelper<TEntity> DynamicData {
            get;
            private set;
        }

        public virtual ActionResult Delete() {
            if (!Request.IsPost())
                return this.HttpStatusCode(HttpStatusCode.MethodNotAllowed);

            var entity = EntityQuery.FirstOrDefault<TEntity>();
            if (entity == null)
                return this.HttpStatusCode(HttpStatusCode.NotFound);

            return OnDelete(entity);
        }

        public virtual ActionResult Edit() {
            if (Request.IsPost())
                return Edit_Post();
            else
                return Edit_Get();
        }

        protected virtual ActionResult Edit_Get() {
            var entity = EntityQuery.FirstOrDefault<TEntity>();
            if (entity == null)
                return this.HttpStatusCode(HttpStatusCode.NotFound);

            return OnEdit(entity);
        }

        protected virtual ActionResult Edit_Post() {
            var entity = EntityQuery.FirstOrDefault<TEntity>();
            if (entity == null)
                return this.HttpStatusCode(HttpStatusCode.NotFound);

            if (!DynamicData.FillEntity(Request.Form, entity))
                return OnEdit(entity);

            return OnUpdate(entity);
        }

        public virtual ActionResult List(string sort, int? show, int? page) {
            var query = EntitiesQuery;

            if (sort != null) {
                if (sort.StartsWith("-"))
                    query = ExpressionUtility.ApplyOrderByDescendingClause(query, sort.Substring(1));
                else
                    query = ExpressionUtility.ApplyOrderByClause(query, sort);
            }

            return OnList(new PagedList<TEntity>(query, page ?? 1, show ?? 10), sort);
        }

        public virtual ActionResult New() {
            if (Request.IsPost())
                return New_Post();
            else
                return New_Get();
        }

        protected virtual ActionResult New_Get() {
            return OnNew(new TEntity());
        }

        protected virtual ActionResult New_Post() {
            TEntity entity;
            if (!DynamicData.TryFillNewEntity(Request.Form, out entity))
                return OnNew(entity);

            return OnInsert(entity);
        }

        public virtual ActionResult Show() {
            var entity = EntityQuery.FirstOrDefault<TEntity>();
            if (entity == null)
                return this.HttpStatusCode(HttpStatusCode.NotFound);

            return OnShow(entity);
        }

        // Overrides for performing the specific actions for the controller

        protected virtual ActionResult OnDelete(TEntity entity) {
            DynamicData.Delete(entity);

            return this.RedirectToScaffold(DynamicData.Table, "list");
        }

        protected virtual ActionResult OnEdit(TEntity entity) {
            return View("PageTemplates/Edit", entity);
        }

        protected virtual ActionResult OnInsert(TEntity entity) {
            if (!DynamicData.Insert(entity))
                return OnNew(entity);

            return this.RedirectToScaffold(DynamicData.Table, "list");
        }

        protected virtual ActionResult OnList(PagedList<TEntity> entities, string sort) {
            ViewData["sort"] = sort;

            return View("PageTemplates/List", entities);
        }

        protected virtual ViewResult OnNew(TEntity entity) {
            return View("PageTemplates/New", entity);
        }

        protected virtual ActionResult OnShow(TEntity entity) {
            return View("PageTemplates/Show", entity);
        }

        protected virtual ActionResult OnUpdate(TEntity entity) {
            if (!DynamicData.Update(entity))
                return OnEdit(entity);

            return this.RedirectToScaffold(DynamicData.Table, "list");
        }
    }
}