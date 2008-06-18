using System.ComponentModel.DataAnnotations;
using Microsoft.Web.DynamicData.Extensions;
using System.ComponentModel;
using System.Resources;

namespace DynamicDataExtensionsSample {
    [MetadataType(typeof(Product_MD))]
    [LocalizedDisplayName(typeof(Resources.Resources), "Products_DisplayName")]
    public partial class Product {

        // Note: the column oders don't need to be sequential, but are just used as a sorting factor.
        // Columns that don't have a sort order use a default order of 0, which places them between
        // columns that have negative and a positive numbers.
        public class Product_MD {
            // Display the Category and Supplier filters using the Autocomplete.ascx filter control
            [Filter(FilterControl = "Autocomplete")]
            [ColumnOrder(-2)]
            public object Category { get; set; }

            [Filter(FilterControl = "Autocomplete")]
            public object Supplier { get; set; }

            // Display the Discontinued filter using the BooleanRadio.ascx filter control
            // Make sure the Discontinued filter is displayed first
            [Filter(FilterControl = "BooleanRadio", Order = 1)]
            public object Discontinued { get; set; }

            // Display the UnitsInStock filter using Integer.ascx filter control
            [Filter(FilterControl = "Integer")]
            [ColumnOrder(-4)]
            [Required(ErrorMessage = "This field is required [from MetadataType]")]
            public object UnitsInStock { get; set; }

            [ColumnOrder(6)]
            public object QuantityPerUnit { get; set; }

            [ColumnOrder(8)]
            public object UnitPrice { get; set; }

            [ColumnOrder(10)]
            public object UnitsOnOrder { get; set; }

            [UIHint("Enumeration")]
            [Filter(FilterControl = "Enumeration")]
            [Required]
            public object ReorderLevel { get; set; }
        }
    }

    [MetadataType(typeof(Order_Detail_MD))]
    public partial class Order_Detail {

        public class Order_Detail_MD {
            // Use the Cascade.ascx filter control. 
            //
            // Specify that the list of items in the products filter should be
            // filtered by the Product.Category foreign key column.
            [Filter(FilterControl = "Cascade")]
            [Cascade("Category")]
            public object Product { get; set; }

            // Don't show the Order filter
            [Filter(Enabled = false)]
            public object Order { get; set; }
        }
    }

    [MetadataType(typeof(Category_MD))]
    public partial class Category {

        public class Category_MD {
            [UIHint("DbImage")]
            [LocalizedDisplayName(typeof(Resources.Resources), "Category_Picture_DisplayName")]
            public object Picture { get; set; }

            [LocalizedDisplayName(typeof(Resources.Resources), "Category_Products_DisplayName")]
            public object Products { get; set; }

            [LocalizedDisplayName(typeof(Resources.Resources), "Category_Description_DisplayName")]
            public object Description { get; set; }

            [LocalizedDisplayName(typeof(Resources.Resources), "Category_CategoryName_DisplayName")]
            public object CategoryName { get; set; }
        }
    }
}