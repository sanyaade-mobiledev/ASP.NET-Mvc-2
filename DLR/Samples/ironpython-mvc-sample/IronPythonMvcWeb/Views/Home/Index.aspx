<%@ Page Language="IronPython" MasterPageFile="~/Views/Shared/Site.master" %>

<asp:Content ID="Foo" ContentPlaceHolderID="MainContent" runat="server">
    <div>
    	<%= ViewData["Message"] %>
    </div>
</asp:Content>