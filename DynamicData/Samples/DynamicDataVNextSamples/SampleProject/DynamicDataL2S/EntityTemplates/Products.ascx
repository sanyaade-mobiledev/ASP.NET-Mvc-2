<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="Products.ascx.cs" Inherits="DynamicDataProject.DynamicData.EntityTemplates.Products" %>

<tr class="td">
    <td class="DDLightHeader">
        Name
    </td>
    <td>
        <asp:DynamicControl runat="server" DataField="ProductName" />
    </td>
    <td class="DDLightHeader">
        Category
    </td>
    <td>
        <asp:DynamicControl runat="server" DataField="Category" />
    </td>
</tr>
