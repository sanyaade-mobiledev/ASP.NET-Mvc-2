<%@ Page Title="Dynamic Data Site" Language="C#" MasterPageFile="~/Views/shared/site.master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="dyndata">
        <h1>My Tables</h1>
        <br />
        <table cellpadding="0" cellspacing="0" border="0" class="table-view list-view">
            <thead>
                <tr><th>Table Name</th></tr>
            </thead>
            <tbody>
                <% foreach (var table in MetaModel.Default.Tables.Where(t => t.Scaffold).OrderBy(t => t.DisplayName)) { %>
                    <tr><td><%= Html.ScaffoldLink(table.DisplayName, table, "list") %></td></tr>
                <% } %>
            </tbody>
        </table>
    </div>
</asp:Content>