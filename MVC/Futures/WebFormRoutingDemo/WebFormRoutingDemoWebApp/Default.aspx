﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebFormRoutingDemoWebApp._Default" %>
<%@ Import Namespace="System.Web.Routing" %>
<%@ Import Namespace="WebFormRouting" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Routing w/ WebForms Demo</title>
    <style type="text/css">
        body {font-family: Verdana; font-size: small;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <h1>Routing With WebForms Demo</h1>
    <div>
        <table>
            <tr>
                <td><%= this.RouteLink("Named") %></td>
                <td>which really maps to /forms/blech.aspx</td>
            </tr>
            <tr>
                <td><%= this.RouteLink("Numbers") %></td>
                <td>which really maps to /forms/HaHa.aspx</td>
            </tr>
            <tr>
                <td><%= this.RouteLink("Admin") %></td>
                <td>which really maps to /admin/secretpage.aspx but access is <em>blocked</em> by Url Auth</td>
            </tr>
            <tr>
                <td><%= this.RouteLink("Blocked") %></td>
                <td>which really maps to /admin/secretpage.aspx but access is <em>blocked</em> by Url Auth because we performed the check on the actual physical location</td>
            </tr>
            <tr>
                <td><%= this.RouteLink("Secret") %></td>
                <td>which really maps to /admin/secretpage.aspx but access is <em>allowed</em> by Url Auth because we performed the check on the actual physical location</td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
