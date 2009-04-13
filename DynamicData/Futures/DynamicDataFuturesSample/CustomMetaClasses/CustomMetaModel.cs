using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.DynamicData.ModelProviders;

namespace DynamicDataFuturesSample {
    public class CustomMetaModel: MetaModel {
        protected override MetaTable CreateTable(TableProvider provider) {
            return new CustomMetaTable(this, provider);
        }
    }
}
