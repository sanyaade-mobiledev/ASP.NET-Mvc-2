using System.Collections.Generic;
using System.Security.Permissions;
using System.Web;

namespace Microsoft.Web.DynamicData.Mvc {
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class ModelPropertyState {
        private List<ModelPropertyError> _propertyErrors = new List<ModelPropertyError>();

        public ModelPropertyState(object modelObject) {
            ModelObject = modelObject;
        }

        public ModelPropertyState(string propertyName) {
            PropertyName = propertyName;
        }

        public ModelPropertyState(string propertyName, string attemptedValue) {
            PropertyName = propertyName;
            AttemptedValue = attemptedValue;
        }

        public ModelPropertyState(string propertyName, object modelObject, string attemptedValue) {
            PropertyName = propertyName;
            ModelObject = modelObject;
            AttemptedValue = attemptedValue;
        }

        public string AttemptedValue {
            get;
            private set;
        }

        public object ModelObject {
            get;
            private set;
        }

        public IList<ModelPropertyError> PropertyErrors {
            get {
                return _propertyErrors;
            }
        }

        public string PropertyName {
            get;
            private set;
        }
    }
}