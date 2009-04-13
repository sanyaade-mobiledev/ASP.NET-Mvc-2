<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="Products_Edit.ascx.cs" Inherits="DynamicDataProject.DynamicData.EntityTemplates.Products_Edit" %>

<tr class="td">
    <td class="DDLightHeader">
        Name
    </td>
    <td>
        <asp:DynamicControl runat="server" DataField="ProductName" Mode="Edit" OnLoad="DynamicControl_Load" />
    </td>
    <td class="DDLightHeader">
        Category
    </td>
    <td>
        <asp:DynamicControl runat="server" DataField="Category" Mode="Edit" OnLoad="DynamicControl_Load" />
    </td>
</tr>