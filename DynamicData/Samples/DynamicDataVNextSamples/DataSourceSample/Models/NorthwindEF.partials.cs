namespace DataSourcesDemo.Entities {
    using System.ComponentModel.DataAnnotations;
    using System.Web.DomainServices;

    [MetadataType(typeof(Product_MD))]
    public partial class Products {
    }

    [CustomValidation(typeof(Product_MD), "ValidateProduct")]
    public class Product_MD {
        [Range(0, 200)]
        public int UnitsInStock { get; set; }

        public static bool ValidateReorderLevel(int reorderLevel) {
            if (reorderLevel == 105) {
                return false;
            }

            return true;
        }

        [Exclude]
        public int UnitsOnOrder { get; set; }

        public static bool ValidateProduct(Products product, ValidationContext context, out ValidationResult result) {
            result = null;
            if (product.ReorderLevel < 0) {
                result = new ValidationResult("Reorder level is out of range", new[] { "ReorderLevel" });
                return false;
            }
            return true;
        }
    }
}
