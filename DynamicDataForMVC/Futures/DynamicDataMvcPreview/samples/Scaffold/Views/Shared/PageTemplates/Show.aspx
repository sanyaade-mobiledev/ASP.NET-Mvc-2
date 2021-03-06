﻿<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="Microsoft.Web.DynamicData.Mvc.DynamicScaffoldViewPage" %>
<script runat="server">
    protected override void OnInit(EventArgs e) {
        Title = DynamicData.Table.DisplayName;
        base.OnInit(e);
    }
</script>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <div class="dyndata">
        <h1>Entry from table <%= DynamicData.Table.DisplayName %></h1>
        <br />
        <%= Html.DynamicEntity(ViewData.Model) %>
        <br />
        <% var routeData = DynamicData.GetRouteData(ViewData.Model); %>
        <div>
            <%= Html.ScaffoldLink("Edit", DynamicData.Table, "edit", routeData) %> &nbsp;
            <form action="<%= Url.ScaffoldUrl(DynamicData.Table, "delete", routeData) %>" method="post" id="dddelete">
                <a href="#" onclick="if (confirm('Are you sure you want to delete this item?')) { dddelete.submit() }">Delete</a>
            </form>
            &nbsp; <%= Html.ScaffoldLink("Show All Items", DynamicData.Table, "list") %>
        </div>
    </div>
</asp:Content>