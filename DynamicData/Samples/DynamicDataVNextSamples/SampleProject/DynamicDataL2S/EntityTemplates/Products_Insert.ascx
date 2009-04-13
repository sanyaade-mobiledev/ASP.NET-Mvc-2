<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Products_Insert.ascx.cs" Inherits="DynamicDataProject.DynamicDataL2S.EntityTemplates.Products_Insert" %>

<tr class="td">
    <td class="DDLightHeader">
        Name
    </td>
    <td>
        <asp:DynamicControl runat="server" DataField="ProductName" Mode="Insert" OnLoad="DynamicControl_Load" />
    </td>
    <td class="DDLightHeader">
        Category
    </td>
    <td>
        <asp:DynamicControl runat="server" DataField="Category" Mode="Insert" OnLoad="DynamicControl_Load" />
    </td>
</tr>