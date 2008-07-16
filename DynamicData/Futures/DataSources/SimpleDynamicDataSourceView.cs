using System;
using System.Web.UI.WebControls;
using System.Web.DynamicData;
using System.Linq;
using System.Web.UI;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace Microsoft.Web.DynamicData {
    internal class SimpleDynamicDataSourceView : DataSourceView {

        private SimpleDynamicDataSource _owner;

        public SimpleDynamicDataSourceView(SimpleDynamicDataSource owner, string viewName)
            : base(owner, viewName) {
            _owner = owner;
        }
        
        protected override IEnumerable ExecuteSelect(DataSourceSelectArguments arguments) {
            return new object[] { _owner.DataObject };
        }

        protected override int ExecuteUpdate(IDictionary keys, IDictionary values, IDictionary oldValues) {

            object newObject;

            try {
                newObject = BuildDataObject(_owner.DataObjectType, values);
            }
            catch (LinqDataSourceValidationException e) {
                // allow user to handle conversion or dlinq property validation exceptions.
                _owner.OnException(new DynamicValidatorEventArgs(e, DynamicDataSourceOperation.Update));
                throw;
            }

            var eventArgs = new SimpleDynamicDataSourceCompleteEventArgs() { NewObject = newObject };

            _owner.OnComplete(eventArgs);

            return 1;
        }

        private object BuildDataObject(Type dataObjectType, IDictionary inputParameters) {
            return DataSourceUtilities.BuildDataObject(_owner.DataObject, inputParameters);
        }
    }

}

