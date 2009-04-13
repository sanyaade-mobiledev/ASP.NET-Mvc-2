using System;
using System.Diagnostics;
using System.Data.Services.Common;
using System.Reflection;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Web.Data.Services.Client {
    // Used as a workaround to override certain methods on DataServiceContext
    // TODO: find a more permanent solution
    public interface IDataServiceContext {
        void AddObject(string entitySetName, object entity);
        void AttachTo(string entitySetName, object entity);
        void AttachTo(string entitySetName, object entity, string etag);
    }
}
