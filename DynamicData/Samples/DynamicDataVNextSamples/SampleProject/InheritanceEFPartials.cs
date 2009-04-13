using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DynamicDataEFProject {
    class InheritanceEFMetadata {
        [ScaffoldColumn(false)]
        public object ID { get; set; }
    }

    [MetadataType(typeof(InheritanceEFMetadata))]
    public partial class Person { }

    [MetadataType(typeof(InheritanceEFMetadata))]
    public partial class Contact { }

    [MetadataType(typeof(InheritanceEFMetadata))]
    public partial class Employee { }

    [MetadataType(typeof(InheritanceEFMetadata))]
    public partial class SalesPerson { }

    [MetadataType(typeof(InheritanceEFMetadata))]
    public partial class Programmer { }
}
