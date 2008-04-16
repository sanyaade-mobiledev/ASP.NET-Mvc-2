<%@ Page Language="$language$" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="$autoeventwireup$" CodeBehind="Index.aspx.$languageext$" Inherits="$safeprojectname$.$indexviewclassname$" %>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%= Html.Encode(ViewData$indexerleft$"Message"$indexerright$) %></h2>
    <p>
        To learn more about ASP.NET MVC visit <a href="http://asp.net/mvc" title="ASP.NET MVC Website">http://asp.net/mvc</a>.
    </p>
</asp:Content>
