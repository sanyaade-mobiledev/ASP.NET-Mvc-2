<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeBehind="ProductsList.aspx.cs" Inherits="DynamicDataFuturesSample.ProductsList" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<%@ Register Src="~/DynamicData/Content/FilterUserControl.ascx" TagName="DynamicFilter" TagPrefix="asp" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:DynamicDataManager ID="DynamicDataManager1" runat="server" AutoLoadForeignKeys="true" />
    
    <h2><%= table.DisplayName%></h2>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <Triggers>
    </Triggers>
        <ContentTemplate>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                HeaderText="List of validation errors" />
            <asp:DynamicValidator runat="server" ID="GridViewValidator" ControlToValidate="GridView1" Display="None" />
            <br />
            
            Categories:
            <asp:DynamicFilter runat="server" ID="CategoryFilter" ContextTypeName="DynamicDataFuturesSample.NorthwindDataContext" TableName="Products" DataField="Category" OnSelectedIndexChanged="OnFilterSelectedIndexChanged" />
            <br />
            Suppliers:
            <asp:DynamicFilter runat="server" ID="SupplierFilter" ContextTypeName="DynamicDataFuturesSample.NorthwindDataContext" TableName="Products" DataField="Supplier" OnSelectedIndexChanged="OnFilterSelectedIndexChanged" />
            <br />
            
            Minimum Units in Stock: <asp:Label ID="UnitsInStock_Value" runat="server" />
            <asp:TextBox ID="UnitsInStock_Slider" runat="server" Text="0" AutoPostBack="true" />
            <ajaxToolkit:SliderExtender runat="server" TargetControlID="UnitsInStock_Slider" Minimum="0" Maximum="150"
                BoundControlID="UnitsInStock_Value" RaiseChangeOnlyOnMouseUp="true" />

            <asp:GridView ID="GridView1" runat="server" DataSourceID="GridDataSource" AllowPaging="True"
                AllowSorting="True" CssClass="gridview">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:DynamicHyperLink ID="EditHyperLink" runat="server" Action="Edit" Text="Edit" />&nbsp;
                            <asp:LinkButton ID="DeleteLinkButton" runat="server" CommandName="Delete"
                                CausesValidation="false" Text="Delete" OnClientClick='return confirm("Are you sure you want to delete this item?");' />&nbsp;
                            <asp:DynamicHyperLink ID="DetailsHyperLink" runat="server" Text="Details" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerStyle CssClass="footer" />
                <PagerTemplate>
                    <asp:GridViewPager ID="GridViewPager1" runat="server" />
                </PagerTemplate>
                <EmptyDataTemplate>
                    There are currently no items in this table.
                </EmptyDataTemplate>
            </asp:GridView>
            
            <%-- AutoGenerateWhereClause has to be set to false and Where has to be non-empty. --%>
            <asp:LinqDataSource ID="GridDataSource" runat="server" EnableDelete="true" AutoGenerateWhereClause="false"
                ContextTypeName="DynamicDataFuturesSample.NorthwindDataContext" TableName="Products"
                Where="UnitsInStock &gt;= @UnitsInStock" >
                <WhereParameters>
                    <asp:DynamicControlParameter ControlId="CategoryFilter" />
                    <asp:DynamicControlParameter ControlId="SupplierFilter" />
                    <asp:ControlParameter ControlID="UnitsInStock_Slider" DefaultValue="0" PropertyName="Text"
                        Type="Int32" Name="UnitsInStock" />
                </WhereParameters>
            </asp:LinqDataSource>
            
            <br />
            <div class="bottomhyperlink">
                <asp:DynamicHyperLink ID="InsertHyperLink" runat="server" Action="Insert"><img id="Img1" runat="server" src="~/DynamicData/Content/Images/plus.gif" alt="Insert new item" />Insert new item</asp:DynamicHyperLink>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
