<%@ Page Language="C#" MasterPageFile="~/Shared/Site.Master" AutoEventWireup="true" CodeBehind="AccessDenied.aspx.cs" Inherits="WebFormRoutingDemoWebApp.AccessDenied" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Access Denied!</h1>
    <p>
        You're seeing this page as part of a demo of URL authorization.
        To see how Routing can get around URL authorization, <a href="/backdoor">click here</a>.
    </p>
</asp:Content>
