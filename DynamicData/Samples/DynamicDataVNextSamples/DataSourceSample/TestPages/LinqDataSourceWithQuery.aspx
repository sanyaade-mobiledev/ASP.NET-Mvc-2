<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="LinqDataSourceWithQuery.aspx.cs" Inherits="DataSourcesDemo.LinqDataSourceWithQuery" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="description">
        The example below shows how to use the <strong>LinqDataSource</strong> with the
        <strong>&lt;Query&gt;</strong> block .
    </div>
    <table style="border-collapse: collapse" cellpadding="4">
        <tr>
            <td>
                <strong>Search:</strong>
            </td>
            <td>
                <asp:TextBox runat="server" ID="name"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <strong>Range (UnitPrice):</strong>
            </td>
            <td>
                <asp:TextBox Width="30" MaxLength="4" runat="server" ID="from"></asp:TextBox>
                <span class="spacer">and</span>
                <asp:TextBox Width="30" MaxLength="4" runat="server" ID="to"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <strong>Discontinued:</strong>
            </td>
            <td>
                <asp:CheckBox ID="disc" runat="server" AutoPostBack="true" />
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:Button ID="Button1" runat="server" Text="Query" />
            </td>
        </tr>
    </table>
    <br />
    <futures:LinqDataSource ID="dataSource" runat="server" ContextTypeName="DataSourcesDemo.NorthwindDataContext"
        EnableUpdate="true" EnableDelete="true" TableName="Products">
    </futures:LinqDataSource>
    <futures:QueryExtender TargetControlID="dataSource" runat="server">
        <asp:CustomExpression OnQuerying="FilterProducts" />
        <asp:SearchExpression DataFields="ProductName, Supplier.CompanyName" SearchType="StartsWith">
            <asp:ControlParameter ControlID="name" />
        </asp:SearchExpression>
        <asp:RangeExpression DataField="UnitPrice" MinType="Inclusive" MaxType="Inclusive">
            <asp:ControlParameter ControlID="from" />
            <asp:ControlParameter ControlID="to" />
        </asp:RangeExpression>
        <asp:PropertyExpression>
            <asp:ControlParameter ControlID="disc" Name="Discontinued" />
        </asp:PropertyExpression>
    </futures:QueryExtender>
    <asp:GridView ID="products" DataKeyNames="ProductID" AllowSorting="True" CssClass="grid"
        GridLines="Horizontal" AllowPaging="True" DataSourceID="dataSource" runat="server">
        <RowStyle CssClass="row" />
        <AlternatingRowStyle CssClass="atl_row" />
        <SelectedRowStyle CssClass="selected_row" />
        <Columns>
            <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" ShowSelectButton="True" />
        </Columns>
        <PagerStyle CssClass="pager" />
        <PagerSettings Mode="NumericFirstLast" />
        <EmptyDataTemplate>
            <div class="desc" style="color: Red">
                No Results Found
            </div>
        </EmptyDataTemplate>
    </asp:GridView>
</asp:Content>
