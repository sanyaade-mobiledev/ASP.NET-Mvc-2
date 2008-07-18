<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="Microsoft.Web.DynamicData.Mvc.DynamicScaffoldViewPage" %>
<script runat="server">
    protected override void OnInit(EventArgs e) {
        if (DynamicData.Table.IsReadOnly)
            throw new HttpException(404, "Not Found");

        Title = DynamicData.Table.DisplayName;
        base.OnInit(e);
    }
</script>
<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
    <div class="dyndata">
        <h1>Edit entry from table <%= DynamicData.Table.DisplayName %></h1>
        <br />
        <%= Html.RenderUserControl("~/Views/Shared/EntityErrors.ascx") %>
        <form action="<%= Request.RawUrl %>" method="post" class="edit-view" id="ddeditform">
            <%= Html.DynamicEntity(ViewData.Model, DataBoundControlMode.Edit) %>
            <div>
                <a href="#" onclick="ddeditform.submit()">Update</a> &nbsp;
                <%= Html.ScaffoldLink("Cancel", DynamicData.Table, "list") %>
            </div>
        </form>
    </div>
</asp:Content>