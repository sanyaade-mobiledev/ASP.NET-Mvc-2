<%@ Control Language="C#" CodeBehind="ManyToMany.ascx.cs" Inherits="DynamicDataEFProject.ManyToManyField" %>

<asp:Repeater ID="Repeater1" runat="server">
    <ItemTemplate>
    <asp:HyperLink runat="server"
        Text="<%# ChildrenColumn.ChildTable.GetDisplayString(Page.GetDataItem()) %>"
        NavigateUrl="<%# ChildrenColumn.ChildTable.GetActionPath(PageAction.Details, Page.GetDataItem()) %>" />
    </ItemTemplate>
</asp:Repeater>

