<%@ Control Language="C#" Inherits="Microsoft.Web.DynamicData.Mvc.MvcFieldTemplate" %>
<%= Html.CheckBox(Column.Name, "", "true", (FieldValue != null && ((bool)FieldValue)))%>