<%@ Control Language="C#" CodeBehind="DateTime_Edit.ascx.cs" Inherits="DynamicDataProject.DateTime_EditField" %>

<asp:TextBox ID="TextBox1" runat="server" CssClass="DDTextBox" Text='<%# FieldValueEditString %>' Columns="20"></asp:TextBox>

<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" CssClass="DDControl" ControlToValidate="TextBox1" Display="Dynamic" Enabled="false" />
<asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator1" CssClass="DDControl" ControlToValidate="TextBox1" Display="Dynamic" Enabled="false" />
<asp:DomainValidator runat="server" ID="DomainValidator1" CssClass="droplist" Display="Dynamic" />
<asp:CustomValidator runat="server" ID="DateValidator" CssClass="DDControl" ControlToValidate="TextBox1" Display="Dynamic" EnableClientScript="false" Enabled="false" OnServerValidate="DateValidator_ServerValidate" />

