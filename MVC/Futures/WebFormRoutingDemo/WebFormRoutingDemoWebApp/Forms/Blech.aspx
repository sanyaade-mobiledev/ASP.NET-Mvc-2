<%@ Page Language="C#" MasterPageFile="~/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Blech.aspx.cs" Inherits="WebFormRoutingDemoWebApp.Forms.Blech" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <p>
        This page is physcially located at /Forms/Blech.aspx. But due to the magic of 
        routing, it appears to be at a different URL.

    </p>
</asp:Content>
