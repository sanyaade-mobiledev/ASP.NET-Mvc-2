<%@ Page Language="C#" MasterPageFile="~/Shared/Site.Master" AutoEventWireup="true" CodeBehind="SecretPage.aspx.cs" Inherits="WebFormRoutingDemoWebApp.Admin.SecretPage" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <p>
        This page is in the physical <strong>Admin</strong> directory. We have a URL auth rule 
        like so...</p>
<pre><code>&lt;authorization>
  &lt;deny users="*" />
&lt;/authorization>
</code></pre>
    <p>
        ...that disallows access to this directory. However, if you are seeing this, 
        you clicked on a route that doesn't perform URL auth on the physical directory 
        and thus were back door'd into here.
    </p>
    <p>
        <strong>NOTE:</strong> This is merely a demo to highlight that it is important 
        to understand how routing relates to URL authorization.
    </p>
    <p>
        You'll notice that the following two links route to this page, but are both 
        properly secured. One because the route that matches has <code>CheckPhysicalUrlAccess</code> 
        set to true.
    </p>
    <p>
        Security on the other route works because it includes /admin/ which is what we secured in 
        web.config.
    </p>
</asp:Content>
