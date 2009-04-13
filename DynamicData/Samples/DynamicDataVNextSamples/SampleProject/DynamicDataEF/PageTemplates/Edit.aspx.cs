﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.DynamicData;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicDataEFProject {
    public partial class Edit : System.Web.UI.Page {
        protected MetaTable table;
    
        protected void Page_Init(object sender, EventArgs e) {
            table = DetailsDataSource.SetTableFromRoute();
            DetailsDataSource.EntityTypeFilter = table.Name;
        }
    
        protected void Page_Load(object sender, EventArgs e) {
            Title = table.DisplayName;
            DetailsDataSource.Include = table.ForeignKeyColumnsNames;
        }
    
        protected void FormView1_ItemCommand(object sender, FormViewCommandEventArgs e) {
            if (e.CommandName == DataControlCommands.CancelCommandName) {
                Response.Redirect(table.ListActionPath);
            }
        }
    
        protected void FormView1_ItemUpdated(object sender, FormViewUpdatedEventArgs e) {
            if (e.Exception == null || e.ExceptionHandled) {
                Response.Redirect(table.ListActionPath);
            }
        }
    
        protected void FormView1_DataBound(object sender, EventArgs e) {
            object dataItem = FormView1.DataItem;
    
            if (dataItem != null) {
                if (dataItem is ICustomTypeDescriptor) {
        dataItem = ((ICustomTypeDescriptor)dataItem).GetPropertyOwner(null);
    }
                MetaTable itemTypeMetaTable = MetaTable.GetTable(dataItem.GetType());
    
                if (!table.Equals(itemTypeMetaTable)) {
                    Response.Redirect(itemTypeMetaTable.GetActionPath(PageAction.Edit, FormView1.DataItem));
                }
            }
        }
    
    }
}
