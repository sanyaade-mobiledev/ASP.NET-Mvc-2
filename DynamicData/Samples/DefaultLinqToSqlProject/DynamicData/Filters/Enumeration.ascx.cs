using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web.DynamicData;
using System.Web.UI;

namespace DynamicDataProject {
    public partial class EnumerationFilter : System.Web.DynamicData.QueryableFilterUserControl {
        public override IQueryable GetQueryable(IQueryable source) {
            return source;
        }
    
    }
}
