<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="Basic.aspx.cs" Inherits="DataSourcesDemo.Basic" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="description">
        The example below shows how to use the <strong>DomainDataSource</strong> with a
        simple parameter.
    </div>
    <strong>Min Units In Stock: </strong>
    <fieldset>
        <asp:TextBox runat="server" ID="unitsInStock"></asp:TextBox>
        <asp:Button Text="Query" runat="server" />
    </fieldset>
    <br />
    <asp:DomainDataSource ID="dataSource" runat="server" DomainServiceTypeName="DataSourcesDemo.LinqToSqlNorthwindDomainService"
        SelectMethod="GetProductsWithMinPrice">
        <SelectParameters>
            <asp:ControlParameter ControlID="unitsInStock" Name="unitsInStock" />
        </SelectParameters>
    </asp:DomainDataSource>
    <asp:GridView ID="products" DataKeyNames="ProductID" AllowSorting="True" CssClass="grid"
        GridLines="Horizontal" AllowPaging="True" DataSourceID="dataSource" runat="server"
        AutoGenerateColumns="true" AutoGenerateEditButton="true">
        <RowStyle CssClass="row" />
        <AlternatingRowStyle CssClass="atl_row" />
        <SelectedRowStyle CssClass="selected_row" />
        <PagerStyle CssClass="pager" />
        <PagerSettings Mode="NumericFirstLast" />
        <EmptyDataTemplate>
            <div class="desc" style="color: Red">
                No Results Found
            </div>
        </EmptyDataTemplate>
    </asp:GridView>
</asp:Content>
