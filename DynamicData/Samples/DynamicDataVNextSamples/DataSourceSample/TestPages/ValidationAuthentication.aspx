<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="ValidationAuthentication.aspx.cs" Inherits="DataSourcesDemo.ExternalPaging" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="description">
        The example below shows how to use the <strong>DomainDataSource</strong>
        with a <strong>Authentication</strong> and <strong>Validation</strong>.
    </div>
    
    <div style="margin-bottom:5px;">
        <asp:LoginStatus ID="LoginStatus1" LoginText="" LogoutPageUrl="~/TestPages/ValidationAuthentication.aspx" runat="server" />
    </div>    
    <asp:Login ID="Login1" runat="server" />
                
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
        HeaderText="List of validation errors" />
        
    <asp:DomainValidator ID="DomainValidator1" runat="server" ControlToValidate="products"></asp:DomainValidator>
    <br />
    <br />
    
    <asp:DomainDataSource ID="dataSource" runat="server" DomainServiceTypeName="DataSourcesDemo.LinqToEntitiesNorthwindDomainService" SelectMethod="GetProducts">
    </asp:DomainDataSource>
    
    <asp:GridView ID="products" DataKeyNames="ProductID, SupplierID, CategoryID " AllowSorting="True" DataSourceID="dataSource"
        CssClass="grid" GridLines="Horizontal" AllowPaging="True" runat="server" AutoGenerateEditButton="True"
        EnableModelValidation="True" AutoGenerateColumns="False">
        <RowStyle CssClass="row" />
        <AlternatingRowStyle CssClass="atl_row" />
        <SelectedRowStyle CssClass="selected_row" />
        <Columns>
            <asp:TemplateField HeaderText="ProductName" SortExpression="ProductName">
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox7" runat="server" Text='<%# Bind("ProductName") %>'></asp:TextBox>
                    <asp:DomainValidator runat="server" DataField="ProductName"></asp:DomainValidator>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label7" runat="server" Text='<%# Bind("ProductName") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="QuantityPerUnit" 
                SortExpression="QuantityPerUnit">
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("QuantityPerUnit") %>'></asp:TextBox>
                    <asp:DomainValidator runat="server" DataField="QuantityPerUnit"></asp:DomainValidator>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label3" runat="server" Text='<%# Bind("QuantityPerUnit") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="UnitPrice" SortExpression="UnitPrice">
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("UnitPrice") %>'></asp:TextBox>
                    <asp:DomainValidator runat="server" DataField="UnitPrice"></asp:DomainValidator>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label4" runat="server" Text='<%# Bind("UnitPrice") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="UnitsInStock" SortExpression="UnitsInStock">
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("UnitsInStock") %>'></asp:TextBox>
                    <asp:DomainValidator runat="server" DataField="UnitsInStock"></asp:DomainValidator>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("UnitsInStock") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="UnitsOnOrder" SortExpression="UnitsOnOrder">
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox8" runat="server" Text='<%# Bind("UnitsOnOrder") %>'></asp:TextBox>
                    <asp:DomainValidator runat="server" DataField="UnitsOnOrder"></asp:DomainValidator>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label9" runat="server" Text='<%# Bind("UnitsOnOrder") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="ReorderLevel" SortExpression="ReorderLevel">
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("ReorderLevel") %>'></asp:TextBox>
                    <asp:DomainValidator runat="server" DataField="ReorderLevel"></asp:DomainValidator>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("ReorderLevel") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:CheckBoxField DataField="Discontinued" HeaderText="Discontinued" SortExpression="Discontinued" />
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
