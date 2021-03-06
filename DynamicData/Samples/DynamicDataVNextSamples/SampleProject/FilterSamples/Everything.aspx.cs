﻿using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Web.DynamicData;

namespace DynamicDataProject.SamplePages {
    public partial class Everything : System.Web.UI.Page {
        protected void Page_Init(object sender, EventArgs e) {
            DynamicDataManager1.RegisterControl(ParentGridView, true /*setSelectionFromUrl*/);
            DynamicDataManager1.RegisterControl(ChildrenGridView);
            DynamicDataManager1.RegisterControl(ChildrenDetailsView);
        }

        protected void Page_Load(object sender, EventArgs e) {
            MetaTable table = ParentGridDataSource.GetTable();
            Title = table.DisplayName;

            // Disable various options if the table is readonly
            if (table.IsReadOnly) {
                ChildrenPanel.Visible = false;
                ParentGridView.AutoGenerateSelectButton = false;
                ParentGridView.AutoGenerateEditButton = false;
                ParentGridView.AutoGenerateDeleteButton = false;
            }
        }
        protected void OnGridViewDataBound(object sender, EventArgs e) {
            if (ParentGridView.Rows.Count == 0) {
                //DetailsView1.ChangeMode(DetailsViewMode.Insert);
            }
        }
        protected void OnFilterSelectionChanged(object sender, EventArgs e) {
            ParentGridView.EditIndex = -1;
            ParentGridView.PageIndex = 0;
        }

        protected void OnGridViewSelectedIndexChanging(object sender, EventArgs e) {
            ParentGridView.EditIndex = -1;
            ParentGridView.SelectedIndex = -1;
            ChildrenGridView.SelectedIndex = 1;
        }

        protected void OnDetailsViewItemDeleted(object sender, DetailsViewDeletedEventArgs e) {
            ChildrenGridView.DataBind();
        }

        protected void OnDetailsViewItemUpdated(object sender, DetailsViewUpdatedEventArgs e) {
            ChildrenGridView.DataBind();
        }

        protected void OnDetailsViewItemInserted(object sender, DetailsViewInsertedEventArgs e) {
            ChildrenGridView.DataBind();
        }

        protected void OnDetailsViewModeChanging(object sender, DetailsViewModeEventArgs e) {
            if (e.NewMode != DetailsViewMode.ReadOnly) {
                ParentGridView.EditIndex = -1;
            }
        }

        protected void OnDetailsViewPreRender(object sender, EventArgs e) {
            //int rowCount = DetailsView1.Rows.Count;
            //if (rowCount > 0) {
            //    SetDeleteConfirmation(DetailsView1.Rows[rowCount - 1]);
            //}
        }

        private void SetDeleteConfirmation(TableRow row) {
            foreach (Control c in row.Cells[0].Controls) {
                if (c is LinkButton) {
                    LinkButton btn = (LinkButton)c;
                    if (btn.CommandName == DataControlCommands.DeleteCommandName) {
                        btn.OnClientClick = "return confirm('Are you sure you want to delete this item?');";
                    }
                }
            }
        }
    }
}
