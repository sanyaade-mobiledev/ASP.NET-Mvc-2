<%@ Control Language="C#" CodeBehind="Enumeration_Edit.ascx.cs" Inherits="DynamicDataProject.Enumeration_EditField" %>

<asp:DropDownList ID="DropDownList1" runat="server" CssClass="droplist" />

<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" CssClass="droplist" ControlToValidate="DropDownList1" Display="Dynamic" Enabled="false" />
<asp:DynamicValidator runat="server" ID="DynamicValidator1" CssClass="droplist" ControlToValidate="DropDownList1" Display="Dynamic" />