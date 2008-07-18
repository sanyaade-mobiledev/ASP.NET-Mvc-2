using System.Collections.Generic;
using System.Security.Permissions;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;
using System.Web.UI;

namespace Microsoft.Web.DynamicData.Mvc {
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class MvcFieldTemplate : ViewUserControl, IFieldTemplate {
        ModelPropertyState _modelPropertyState;
        bool _triedModelPropertyState;

        public MetaColumn Column {
            get { return Host.Column; }
        }

        public object Entity {
            get { return ViewData.Model; }
        }

        public IEnumerable<ModelPropertyError> Errors {
            get {
                if (ModelPropertyState != null)
                    return ModelPropertyState.PropertyErrors;
                return new ModelPropertyError[0];
            }
        }

        protected object FieldValue {
            get {
                if (Entity == null)
                    return null;
                return DataBinder.GetPropertyValue(Entity, Column.Name);
            }
        }

        protected string FieldValueEditString {
            get {
                if (ModelPropertyState != null)
                    return ModelPropertyState.AttemptedValue;
                return FormattingOptions.FormatEditValue(FieldValue);
            }
        }

        public virtual string FieldValueString {
            get { return FormatFieldValue(FieldValue); }
        }

        public MetaForeignKeyColumn ForeignKeyColumn {
            get { return Column as MetaForeignKeyColumn; }
        }

        public virtual IFieldFormattingOptions FormattingOptions {
            get { return Host.FormattingOptions; }
        }

        protected bool HasPreviousErrors {
            get { return ModelPropertyState != null && ModelPropertyState.PropertyErrors.Count > 0; }
        }

        public IFieldTemplateHost Host {
            get;
            private set;
        }

        private ModelPropertyState ModelPropertyState {
            get {
                if (!_triedModelPropertyState) {
                    _triedModelPropertyState = true;
                    _modelPropertyState = ViewData.ModelState().GetPropertyState(Column.Name);
                }

                return _modelPropertyState;
            }
        }

        public MetaTable Table {
            get { return Column.Table; }
        }

        public virtual string FormatFieldValue(object fieldValue) {
            return FormattingOptions.FormatValue(fieldValue);
        }

        void IFieldTemplate.SetHost(IFieldTemplateHost host) {
            Host = host;
        }
    }
}