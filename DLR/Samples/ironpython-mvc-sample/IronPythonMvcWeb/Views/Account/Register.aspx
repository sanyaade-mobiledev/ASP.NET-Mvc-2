<%@ Page Language="IronPython" MasterPageFile="~/Views/Shared/Site.Master" %>

<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Account Creation</h2>
    <p>
        Use the form below to create a new account. 
    </p>
    <p>
        Passwords are required to be a minimum of <%=Html.Encode(ViewData["PasswordLength"])%> characters in length.
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
        <form method="post" action="<%= Html.AttributeEncode(Url.Action("Register")) %>">
        <div>
            <table border="1">
                <tr>
                    <td>Username:</td>
                    <td><%= Html.TextBox("username") %></td>
                </tr>
                <tr>
                    <td>Email:</td>
                    <td><%= Html.TextBox("email") %></td>
                </tr>
                <tr>
                    <td>Password:</td>
                    <td><%= Html.Password("password") %></td>
                </tr>
                <tr>
                    <td>Confirm password:</td>
                    <td><input type="password" name="password" value="" /></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td><input type="submit" value="Register" /></td>
                </tr>
            </table>
        </div>
    </form>
</asp:Content>
