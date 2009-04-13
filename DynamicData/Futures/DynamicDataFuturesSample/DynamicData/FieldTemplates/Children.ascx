<%@ Control Language="C#" CodeBehind="Children.ascx.cs" Inherits="DynamicDataProject.ChildrenField" %>

<asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="<%# GetChildrenPath() %>" />