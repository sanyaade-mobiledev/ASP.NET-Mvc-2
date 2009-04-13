using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DynamicDataFuturesSample {

    public partial class NorthwindDataContext {
        //partial void InsertTrainRoute(TrainRoute instance) {
        //    //instance.ID = new System.Guid();
        //}
    }

    [MetadataType(typeof(Product_MD))]
    public partial class Product {

        // Note: the column oders don't need to be sequential, but are just used as a sorting factor.
        // Columns that don't have a sort order use a default order of 0, which places them between
        // columns that have negative and a positive numbers.
        public class Product_MD {
            [Display(Order = -2)]
            [Required]
            public object Category { get; set; }

            [Display(Order = -4, Name = "Units In Stock Name")]
            [Required(ErrorMessage = "This field is required [from MetadataType]")]
            public object UnitsInStock { get; set; }

            [Display(Order = 6)]
            public object QuantityPerUnit { get; set; }

            [Display(Order = 8)]
            public object UnitPrice { get; set; }

            [Display(Order = 10)]
            [UIHint("IntegerSlider")]
            [Range(0, 150, ErrorMessage = "Wrong range for UnitsOnOrder")]
            public object UnitsOnOrder { get; set; }

            [Required]
            public object ReorderLevel { get; set; }
        }
    }

    [MetadataType(typeof(Category_MD))]
    [DisplayName("My Categories")]
    public partial class Category {

        public class Category_MD {
            [UIHint("DbImage")]
            public object Picture { get; set; }
        }
    }

    [MetadataType(typeof(Employee_MD))]
    public partial class Employee {
        public class Employee_MD {
            [DataType(DataType.Url)]
            public object PhotoPath { get; set; }
        }
    }


    [MetadataType(typeof(TrainRoute_MD))]
    public partial class TrainRoute {
        partial void OnCreated() {
            //ID = new System.Guid();
        }

        public class TrainRoute_MD {
            //[ScaffoldColumn(false)]
            public object ID { get; set; }
        }
    }
}