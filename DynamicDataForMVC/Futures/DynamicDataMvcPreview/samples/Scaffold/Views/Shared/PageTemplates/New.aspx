<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" ValidateRequest="false" Inherits="Microsoft.Web.DynamicData.Mvc.DynamicScaffoldViewPage"  %>
<script runat="server">
    protected override void OnInit(EventArgs e) {
        if (DynamicData.Table.IsReadOnly)
            throw new HttpException(404, "Not Found");

        Title = "Add new entry to table " + DynamicData.Table.DisplayName;
        base.OnInit(e);
    }
</script>
<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
    <div class="dyndata">
        <h1><%= Page.Title %></h1>
        <br />
        <%= Html.RenderUserControl("~/Views/Shared/EntityErrors.ascx") %>
        <form action="<%= Request.RawUrl %>" method="post" class="edit-view" id="ddnewform">
            <%= Html.DynamicEntity(DynamicData.Table.EntityType, DataBoundControlMode.Insert) %>
            <div>
                <a href="#" onclick="ddnewform.submit()">Insert</a> &nbsp;
                <%= Html.ScaffoldLink("Cancel", DynamicData.Table, "list") %>
            </div>
        </form>
    </div>
</asp:Content>