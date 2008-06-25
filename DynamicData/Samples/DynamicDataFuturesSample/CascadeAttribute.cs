using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicDataFuturesSample {
    public class CascadeAttribute : Attribute {

        public String ParentColumn { get; private set; }

        public CascadeAttribute(string parentColumn) {
            ParentColumn = parentColumn;
        }
    }
}