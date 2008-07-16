<%@ Control Language="C#" CodeBehind="ForeignKey_Edit.ascx.cs" Inherits="DynamicDataFuturesSample.ForeignKey_EditField" %>

<asp:DropDownList ID="DropDownList1" runat="server" CssClass="droplist">
</asp:DropDownList>

<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" CssClass="droplist" ControlToValidate="DropDownList1" Display="Dynamic" Enabled="false"></asp:RequiredFieldValidator>
<asp:DynamicValidator runat="server" ID="DynamicValidator1" CssClass="droplist" ControlToValidate="DropDownList1" Display="Dynamic" />