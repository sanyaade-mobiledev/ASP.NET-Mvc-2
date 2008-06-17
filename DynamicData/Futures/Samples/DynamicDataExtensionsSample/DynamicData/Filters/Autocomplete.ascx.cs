using System;
using System.Web.DynamicData;

namespace DynamicDataExtensionsSample {
    public partial class Autocomplete_Filter : FilterUserControlBase {
        protected void Page_Init(object sender, EventArgs e) {
            var fkColumn = Column as MetaForeignKeyColumn;

            //// dynamically build the context key so the web service knows which table we're talking about
            //autoComplete1.ContextKey = fkColumn.ParentTable.Provider.DataModel.ContextType.FullName + "#" + fkColumn.ParentTable.Name;
            autoComplete1.ContextKey = AutocompleteFilterService.GetContextKey(fkColumn.ParentTable);

            // add javascript to create postback when user selects an item in the list
            var method = "function(source, eventArgs ) {\r\n" +
                "var valueField = document.getElementById('" + AutocompleteValue.ClientID + "');\r\n" +
                "valueField.value = eventArgs.get_value();\r\n" +
                "setTimeout('" + Page.ClientScript.GetPostBackEventReference(AutocompleteTextBox, null).Replace("'", "\\'") + "', 0);\r\n" +
                "}";
            autoComplete1.OnClientItemSelected = method;

            // modify behaviorID so it does not clash with other autocomplete extenders on the page
            autoComplete1.Animations = autoComplete1.Animations.Replace(autoComplete1.BehaviorID, AutocompleteTextBox.UniqueID);
            autoComplete1.BehaviorID = AutocompleteTextBox.UniqueID;
        }

        public void ClearButton_Click(object sender, EventArgs e) {
            // this would probably be better handled using client javascirpt
            AutocompleteValue.Value = String.Empty;
            AutocompleteTextBox.Text = String.Empty;
        }

        public override string SelectedValue {
            get {
                if (IsPostBack) {
                    if (!String.IsNullOrEmpty(AutocompleteValue.Value)) {
                        return AutocompleteValue.Value;
                    } else {
                        return null;
                    }
                }
                return null;
            }
        }
    }
}