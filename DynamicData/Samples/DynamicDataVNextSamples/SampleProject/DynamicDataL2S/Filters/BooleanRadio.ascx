<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BooleanRadio.ascx.cs"
    Inherits="DynamicDataProject.DynamicData.Filters.BooleanRadio" %>

<asp:RadioButtonList runat="server" ID="RadioButtonList1" AutoPostBack="true" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged"
    RepeatDirection="Horizontal" RepeatLayout="Flow">
    <asp:ListItem Text="All" Value="" Selected="True" />
    <asp:ListItem Text="True" Value="True" />
    <asp:ListItem Text="False" Value="False" />
</asp:RadioButtonList>
