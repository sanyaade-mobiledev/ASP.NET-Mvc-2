﻿<%@ Page Language="C#" MasterPageFile="~/Site.master" CodeBehind="Everything.aspx.cs" Inherits="DynamicDataProject.SamplePages.Everything" %>

<%@ Register src="~/DynamicData/Content/GridViewPager.ascx" tagname="GridViewPager" tagprefix="asp" %>

<asp:Content ID="headContent" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:DynamicDataManager ID="DynamicDataManager1" runat="server" AutoLoadForeignKeys="true" />

    <h2><%= ParentGridDataSource.GetTable().DisplayName %></h2>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                HeaderText="List of validation errors" />
            <asp:DynamicValidator runat="server" ID="GridView1Validator" ControlToValidate="ParentGridView" Display="None" />
            <asp:DynamicValidator runat="server" ID="GridView2Validator" ControlToValidate="ChildrenGridView" Display="None" />
            <asp:DynamicValidator runat="server" ID="DetailsViewValidator" ControlToValidate="ChildrenDetailsView" Display="None" />

            <asp:QueryableFilterRepeater runat="server" ID="ParentFilterRepeater">
                <ItemTemplate>
                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("DisplayName") %>' />
                    <asp:DynamicFilter runat="server" ID="DynamicFilter" OnFilterChanged="OnFilterSelectionChanged" /><br />
                </ItemTemplate>
            </asp:QueryableFilterRepeater>
            
            <br />

            <asp:GridView ID="ParentGridView" runat="server" DataSourceID="ParentGridDataSource"
                EnablePersistedSelection="true" AutoGenerateSelectButton="True" AutoGenerateEditButton="True"
                AllowPaging="True" AllowSorting="True" OnDataBound="OnGridViewDataBound" OnSelectedIndexChanging="OnGridViewSelectedIndexChanging"
                CssClass="gridview">
                <PagerStyle CssClass="footer" />
                <SelectedRowStyle CssClass="selected" />
                <PagerTemplate>
                    <asp:GridViewPager runat="server" />
                </PagerTemplate>
                <EmptyDataTemplate>
                    There are currently no items in this table.
                </EmptyDataTemplate>
            </asp:GridView>

            <aspX:LinqDataSource ID="ParentGridDataSource" runat="server" EnableDelete="true" EnableUpdate="true" ContextTypeName="DynamicDataProject.NorthwindDataContext"
                TableName="Categories">
            </aspX:LinqDataSource>
            
            <aspX:QueryExtender TargetControlID="ParentGridDataSource" runat="server">
                <asp:DynamicFilterExpression ControlID="ParentFilterRepeater" />
            </aspX:QueryExtender>
            
            <asp:Panel ID="ChildrenPanel" runat="server" style="float:left; width:400px; margin-right:30px;">
                <br /><br />
                <h3>Products in this category</h3>
                Discontinued: <asp:DynamicFilter DataField="Discontinued" runat="server" ID="GridView2DiscontinuedFilter" />
                <br /><br />
                <asp:GridView ID="ChildrenGridView" runat="server" DataSourceID="ChildrenGridDataSource" CssClass="gridview"
                    AutoGenerateSelectButton="true" AutoGenerateDeleteButton="true"
                    AllowPaging="true" AllowSorting="true" AutoGenerateColumns="false" >
                    <Columns>
                        <asp:DynamicField DataField="ProductName" />
                        <asp:DynamicField DataField="UnitsInStock" />
                        <asp:DynamicField DataField="Discontinued" />
                    </Columns>
                    <EmptyDataTemplate>
                        No items matching the given criteria.
                    </EmptyDataTemplate>
                    <PagerTemplate>
                        <asp:GridViewPager ID="GridViewPager1" runat="server" />
                    </PagerTemplate>
                          <PagerStyle CssClass="footer" />        
                <SelectedRowStyle CssClass="selected" />
                </asp:GridView>
            
                <aspX:LinqDataSource ID="ChildrenGridDataSource" runat="server" EnableDelete="true" EnableInsert="true"
                    EnableUpdate="true" ContextTypeName="DynamicDataProject.NorthwindDataContext" TableName="Products">
                </aspX:LinqDataSource>
                
                <aspX:QueryExtender TargetControlID="ChildrenGridDataSource" runat="server">
                    <asp:ControlFilterExpression ControlID="ParentGridView" Column="Category" />
                    <asp:DynamicFilterExpression ControlID="GridView2DiscontinuedFilter" />
                </aspX:QueryExtender>
            </asp:Panel>
            <asp:Panel ID="ChildrenDetailsPanel" runat="server">
                <br /><br />
                <h3>Product details</h3>
                
                <asp:DetailsView ID="ChildrenDetailsView" runat="server" DataSourceID="ChildrenDetailsDataSource"
                    AutoGenerateEditButton="true" AutoGenerateDeleteButton="true" AutoGenerateInsertButton="true"
                    OnModeChanging="OnDetailsViewModeChanging" OnPreRender="OnDetailsViewPreRender"
                    OnItemDeleted="OnDetailsViewItemDeleted" OnItemUpdated="OnDetailsViewItemUpdated"
                    OnItemInserted="OnDetailsViewItemInserted" CssClass="detailstable" FieldHeaderStyle-CssClass="bold">
                </asp:DetailsView>
                
                <aspX:LinqDataSource ID="ChildrenDetailsDataSource" runat="server" EnableDelete="true" EnableInsert="true"
                    EnableUpdate="true" ContextTypeName="DynamicDataProject.NorthwindDataContext"
                    TableName="Products">
                </aspX:LinqDataSource>
                
                <aspX:QueryExtender TargetControlID="ChildrenDetailsDataSource" runat="server">
                    <asp:ControlFilterExpression ControlID="ChildrenGridView" />
                </aspX:QueryExtender>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
