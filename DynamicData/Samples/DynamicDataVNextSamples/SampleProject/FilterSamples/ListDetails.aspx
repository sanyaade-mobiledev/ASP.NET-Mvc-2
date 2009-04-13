﻿<%@ Page Language="C#" MasterPageFile="~/Site.master" CodeBehind="ListDetails.aspx.cs" Inherits="DynamicDataProject.SamplePages.ListDetails" %>

<%@ Register src="~/DynamicData/Content/GridViewPager.ascx" tagname="GridViewPager" tagprefix="asp" %>

<asp:Content ID="headContent" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:DynamicDataManager ID="DynamicDataManager1" runat="server" AutoLoadForeignKeys="true" />

    <h2><%= GridDataSource.GetTable().DisplayName %></h2>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                HeaderText="List of validation errors" />
            <asp:DynamicValidator runat="server" ID="GridViewValidator" ControlToValidate="GridView1" Display="None" />
            <asp:DynamicValidator runat="server" ID="DetailsViewValidator" ControlToValidate="DetailsView1" Display="None" />

            <asp:GridView ID="GridView1" runat="server" DataSourceID="GridDataSource" EnablePersistedSelection="true"
                AutoGenerateSelectButton="True" AutoGenerateEditButton="True" AutoGenerateDeleteButton="true"
                AllowPaging="True" AllowSorting="True" OnDataBound="OnGridViewDataBound"
                OnRowEditing="OnGridViewRowEditing" OnSelectedIndexChanging="OnGridViewSelectedIndexChanging"
                OnRowDeleted="OnGridViewRowDeleted" OnRowUpdated="OnGridViewRowUpdated"
                OnRowCreated="OnGridViewRowCreated" CssClass="gridview">

                <PagerStyle CssClass="footer" />        
                <SelectedRowStyle CssClass="selected" />
                <PagerTemplate>
                    <asp:GridViewPager runat="server" />
                </PagerTemplate>
                <EmptyDataTemplate>
                    There are currently no items in this table.
                </EmptyDataTemplate>
            </asp:GridView>

            <aspX:LinqDataSource ID="GridDataSource" runat="server" EnableDelete="true" EnableUpdate="true" ContextTypeName="DynamicDataProject.NorthwindDataContext"
                TableName="Products">
            </aspX:LinqDataSource>
            
            <asp:Panel ID="DetailsPanel" runat="server">
                <br /><br />
                
                <asp:DetailsView ID="DetailsView1" runat="server" DataSourceID="DetailsDataSource"
                    AutoGenerateEditButton="true" AutoGenerateDeleteButton="true" AutoGenerateInsertButton="true"
                    OnModeChanging="OnDetailsViewModeChanging" OnPreRender="OnDetailsViewPreRender"
                    OnItemDeleted="OnDetailsViewItemDeleted" OnItemUpdated="OnDetailsViewItemUpdated"
                    OnItemInserted="OnDetailsViewItemInserted" CssClass="detailstable" FieldHeaderStyle-CssClass="bold">
                </asp:DetailsView>
            
                <aspX:LinqDataSource ID="DetailsDataSource" runat="server" EnableDelete="true" EnableInsert="true"
                    EnableUpdate="true" ContextTypeName="DynamicDataProject.NorthwindDataContext" TableName="Products">
                </aspX:LinqDataSource>
                
                <aspX:QueryExtender TargetControlID="DetailsDataSource" runat="server">
                    <asp:ControlFilterExpression ControlID="GridView1" />
                </aspX:QueryExtender>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>