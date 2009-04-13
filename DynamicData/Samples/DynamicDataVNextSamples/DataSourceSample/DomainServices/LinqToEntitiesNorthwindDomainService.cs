namespace DataSourcesDemo {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.DomainServices;
    using System.Web.DomainServices.LinqToEntities;
    using DataSourcesDemo.Entities;
    using Microsoft.Web.Data;

    // Un-comment this line if you want to enable authentication on this domain service.    
    public class LinqToEntitiesNorthwindDomainService : LinqToEntitiesDomainService<NorthwindEntities> {
        public IQueryable<Products> GetProducts() {
            return Context.Products.Include("Categories").Include("Suppliers");
        }

        [Query]
        public IQueryable<Products> GetProductsWithMinPrice(int unitsInStock) {
            return Context.Products.Where(p => p.UnitsInStock >= unitsInStock);
        }

        public IQueryable<Products> GetProductsExternalPaging(int rowIndex, int rowSize, string sortColumn, out int totalCount) {
            IQueryable<Products> source = Context.Products.SortBy(sortColumn);
            totalCount = Context.Products.Count();
            return source.Skip(rowIndex).Take(rowSize);
        }

        [Insert]
        public void InsertProduct(Products product) {
            Context.AddToProducts(product);
        }

        [Delete]
        public void DeleteProduct(Products originalEntity) {
            Context.Attach(originalEntity);
            Context.DeleteObject(originalEntity);
        }

        [Update]
        // Require the admin role to update a product
        [RequiresRoles("Admins")]
        public void UpdateProduct(Products newEntity, Products originalEntity) {
            Context.AttachAsModified(newEntity, originalEntity);
        }

        public virtual IQueryable<Categories> GetCategories() {
            return Context.Categories;
        }

        public virtual void InsertCategories(Categories newCategories) {
            Context.AddToCategories(newCategories);
        }

        public virtual void UpdateCategories(Categories newCategories, Categories originalCategories) {
            Context.AttachAsModified(newCategories, originalCategories);
        }

        public virtual void DeleteCategories(Categories originalCategories) {
            Context.Attach(originalCategories);
            Context.DeleteObject(originalCategories);
        }
    }
}
