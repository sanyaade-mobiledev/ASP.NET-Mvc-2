<%@ Control Language="C#" CodeBehind="DateAjaxCalendar_Edit.ascx.cs" Inherits="DynamicDataFuturesSample.DynamicData.FieldTemplates.DateAjaxCalendar_EditField" %>

<asp:TextBox ID="TextBox1" runat="server" Text='<%# FieldValueEditString %>' CssClass="droplist"></asp:TextBox>
<ajaxToolkit:CalendarExtender ID="defaultCalendarExtender" runat="server" TargetControlID="TextBox1" />

<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" CssClass="droplist" ControlToValidate="TextBox1" Display="Dynamic" Enabled="false" />
<asp:DynamicValidator runat="server" ID="DynamicValidator1" CssClass="droplist" ControlToValidate="TextBox1" Display="Dynamic" />