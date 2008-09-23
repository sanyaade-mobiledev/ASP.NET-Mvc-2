<%@ Page Language="IronPython" MasterPageFile="~/Views/Shared/Site.Master" %>

<asp:Content ID="changePasswordContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Change Password</h2>
    <p>
        Use the form below to change your password. 
    </p>
    <p>
        New passwords are required to be a minimum of <%=Html.Encode(ViewData["PasswordLength"])%> characters in length.
    </p>
<%
errors = ViewData["errors"]
if errors is not None:
    %>
                <ul class="error">
<%
    for error in errors:
        %>
                    <li><%= Html.Encode(error) %></li>
    <%%>
                </ul>
<%%>
    <form method="post" action="<%= Html.AttributeEncode(Url.Action("ChangePassword")) %>">
        <div>
            <table>
                <tr>
                    <td>Current password:</td>
                    <td><%= Html.Password("currentPassword") %></td>
                </tr>
                <tr>
                    <td>New password:</td>
                    <td><%= Html.Password("newPassword") %></td>
                </tr>
                <tr>
                    <td>Confirm new password:</td>
                    <td><%= Html.Password("confirmPassword") %></td>
                </tr>
                <tr>
                    <td></td>
                    <td><input type="submit" value="Change Password" /></td>
                </tr>
            </table>
        </div>
    </form>
</asp:Content>
