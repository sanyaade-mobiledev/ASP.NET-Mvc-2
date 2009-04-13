using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.DynamicData;

namespace DynamicDataProject {

    [MetadataType(typeof(Product_MD))]
    public partial class Product {

        private class Product_MD {
            [FilterUIHint("MultiForeignKey")]
            public object Category { get; set; }
            [FilterUIHint("BooleanRadio")]
            public object Discontinued { get; set; }
            [FilterUIHint("Range")]
            [Range(0,150)]
            public object UnitsInStock { get; set; }
            [FilterUIHint("Autocomplete")]
            public object Supplier { get; set; }
        }
    }
}
