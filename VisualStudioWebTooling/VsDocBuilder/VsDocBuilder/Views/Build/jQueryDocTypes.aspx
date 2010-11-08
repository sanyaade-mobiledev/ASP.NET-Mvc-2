<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<!DOCTYPE html>
<html>
<head>
    <title>All the "types" referenced in the jQuery Doc API</title>
</head>
<body>
    <div>
        <ul>
            <% foreach (var t in Model) { %>
            <li><%: t %></li>
            <% } %>
        </ul>
    </div>
</body>
</html>