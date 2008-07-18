<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<% if (ViewData.ModelErrors().Count > 0) { %>
    <div class="entity-error">The following errors occurred:</div>
    <% foreach (var error in ViewData.ModelErrors()) { %>
        <div class="entity-error">* <%= error.ErrorMessage %></div>
    <% } %>
<% } %>