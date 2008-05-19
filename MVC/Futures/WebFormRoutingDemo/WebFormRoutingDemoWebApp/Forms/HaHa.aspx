<%@ Page Language="C#" MasterPageFile="~/Shared/Site.Master" AutoEventWireup="true" CodeBehind="HaHa.aspx.cs" Inherits="WebFormRoutingDemoWebApp.Forms.HaHa" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    This page is physcially located at /Forms/HaHa.aspx. But due to the magic of 
    routing, it appears to be at a different URL.
</asp:Content>
