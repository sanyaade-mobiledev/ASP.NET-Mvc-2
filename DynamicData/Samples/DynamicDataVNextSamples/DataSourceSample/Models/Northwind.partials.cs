namespace DataSourcesDemo {
    using System.ComponentModel.DataAnnotations;

    [MetadataType(typeof(Product_MD))]
    public partial class Product {
    }

    public class Product_MD {
        [Range(0, 200)]
        public int UnitsInStock { get; set; }

        [Required]
        public string ProductName { get; set; }
    }
}
