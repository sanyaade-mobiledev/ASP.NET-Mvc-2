<%@ Page Language="C#" MasterPageFile="~/Site.master" CodeBehind="Default.aspx.cs" Inherits="DynamicDataProject._Default" %>

<asp:Content ID="headContent" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server" />
    
    <h2 class="DDSubHeader">Sample custom pages</h2>
    <ul>
        <li><a href="FilterSamples/Default.aspx">Filtering samples</a></li>
        <li><a href="EntityTemplateSamples/ProductsList.aspx">Entity Templates and ListView sample</a></li>
    </ul>
    
    <table width="100%">
        <tr>
            <td valign="top" style="width:50%" >
                <h2 class="DDSubHeader">My LINQ to SQL tables</h2>
                
                <asp:GridView ID="Menu1" runat="server" AutoGenerateColumns="false" CssClass="DDGridView"
                    RowStyle-CssClass="td" HeaderStyle-CssClass="th" CellPadding="6">
                    <Columns>
                        <asp:TemplateField HeaderText="Table Name" SortExpression="TableName">
                            <ItemTemplate>
                                <asp:DynamicHyperLink runat="server"><%# Eval("DisplayName") %></asp:DynamicHyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
            <td valign="top" style="width:50%">
                <h2 class="DDSubHeader">My Entity Framework tables</h2>
                
                <asp:GridView ID="Menu2" runat="server" AutoGenerateColumns="false" CssClass="DDGridView"
                    RowStyle-CssClass="td" HeaderStyle-CssClass="th" CellPadding="6">
                    <Columns>
                        <asp:TemplateField HeaderText="Table Name" SortExpression="TableName">
                            <ItemTemplate>
                                <asp:DynamicHyperLink runat="server"><%# Eval("DisplayName") %></asp:DynamicHyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>

</asp:Content>


