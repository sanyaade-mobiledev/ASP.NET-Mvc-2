<%@ Control Language="C#" CodeBehind="MultilineText_Edit.ascx.cs" Inherits="DynamicDataProject.MultilineText_EditField" %>

<asp:TextBox ID="TextBox1" runat="server" CssClass="DDControl" TextMode="MultiLine" Text='<%# FieldValueEditString %>' Columns="80" Rows="5"></asp:TextBox>

<asp:RequiredFieldValidator runat="server" id="RequiredFieldValidator1" CssClass="DDControl" ControlToValidate="TextBox1" Display="Dynamic" Enabled="false" />
<asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator1" CssClass="DDControl" ControlToValidate="TextBox1" Display="Dynamic" Enabled="false" />
<asp:DomainValidator runat="server" ID="DomainValidator1" CssClass="droplist" Display="Dynamic" />
