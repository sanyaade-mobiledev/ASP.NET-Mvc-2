namespace DataSourcesDemo {
    using System;
    using Microsoft.Web.Data;
    using System.IO;
    using System.Linq;
    using System.Web.DomainServices;
    using System.Web.DomainServices.LinqToSql;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Linq;

    public class LinqToSqlNorthwindDomainService : LinqToSqlDomainService<NorthwindDataContext> {
        public IQueryable<Product> GetProducts() {
            return Context.Products;
        }

        [Query]
        public IQueryable<Product> GetProductsWithMinPrice(int unitsInStock) {
            return Context.Products.Where(p => p.UnitsInStock >= unitsInStock);
        }

        [Insert]
        public void InsertProduct(Product product) {
            Context.Products.InsertOnSubmit(product);
        }

        [Delete]
        public void DeleteProduct(Product originalEntity) {
            Context.Products.Attach(originalEntity, false);
            Context.Products.DeleteOnSubmit(originalEntity);
        }

        [Update]
        public virtual void UpdateProduct(Product newEntity, Product originalEntity) {
            if (newEntity.UnitPrice <= 0) {
                throw new ValidationException("Unit price is out of range");
            }

            Context.Products.Attach(newEntity, originalEntity);
        }

        
        public virtual IQueryable<Category> GetCategories() {
            return Context.Categories;
        }

        public virtual void InsertCategory(Category newCategory) {
            Context.Categories.InsertOnSubmit(newCategory);
        }

        public virtual void UpdateCategory(Category newCategory, Category originalCategory) {
            Context.Categories.Attach(newCategory, originalCategory);
        }

        public virtual void DeleteCategory(Category originalCategory) {
            Context.Categories.Attach(originalCategory, false);
            Context.Categories.DeleteOnSubmit(originalCategory);
        }

   }
}