<%@ Control Language="C#" CodeBehind="ForeignKey_Edit.ascx.cs" Inherits="DynamicDataProject.ForeignKey_EditField" %>

<asp:DropDownList ID="DropDownList1" runat="server" CssClass="DDDropDown">
</asp:DropDownList>

<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" CssClass="DDControl" ControlToValidate="DropDownList1" Display="Dynamic" Enabled="false" />
<asp:DynamicValidator runat="server" ID="DynamicValidator1" CssClass="DDControl" ControlToValidate="DropDownList1" Display="Dynamic" />

