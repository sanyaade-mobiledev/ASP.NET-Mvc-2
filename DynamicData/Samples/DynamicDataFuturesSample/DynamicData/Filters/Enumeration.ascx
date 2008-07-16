<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Enumeration.ascx.cs" Inherits="DynamicDataFuturesSample.Enumeration_Filter" %>

<%-- Only one of these controls will be activated depending on whether the underlying enum type is in flags mode or not --%>

<asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="true" Enabled="false" Visible="false">
    <asp:ListItem Text="All" Value="" Selected="True" />
</asp:DropDownList>

<asp:CheckBoxList ID="CheckBoxList1" runat="server" AutoPostBack="true" Enabled="false" Visible="false">
</asp:CheckBoxList>