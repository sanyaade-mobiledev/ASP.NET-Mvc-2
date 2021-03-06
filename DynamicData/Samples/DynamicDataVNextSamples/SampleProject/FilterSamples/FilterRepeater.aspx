﻿<%@ Page Language="C#" MasterPageFile="~/Site.master" CodeBehind="FilterRepeater.aspx.cs" Inherits="DynamicDataProject.SamplePages.FilterRepeater_Page" %>

<%@ Register src="~/DynamicData/Content/GridViewPager.ascx" tagname="GridViewPager" tagprefix="asp" %>

<asp:Content ID="headContent" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:DynamicDataManager ID="DynamicDataManager1" runat="server" AutoLoadForeignKeys="true" />

    <p>All filters are generated by a <code>asp:<%= typeof(QueryableFilterRepeater).Name %></code> control. The repeater's
    item template contains a <code>asp:<%= typeof(DynamicFilter).Name %></code> control that gets configured to display a
    filter for a given column. The list of columns to automatically filter is determined from the table's MetaColumns (by
    default foreign key and boolean columns get included) as well as by columns that have <%= typeof(System.ComponentModel.DataAnnotations.DisplayAttribute).Name %>
    declared with AutoGenerateFilter set to true. The data source references the repeater using the <code>asp:<%= typeof(DynamicFilterExpression).Name %></code> control.</p>

    <h2><%= table.DisplayName%></h2>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                HeaderText="List of validation errors" />
            <asp:DynamicValidator runat="server" ID="GridViewValidator" ControlToValidate="GridView1" Display="None" />
            
            <asp:QueryableFilterRepeater runat="server" ID="FilterRepeater">
                <ItemTemplate>
                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("DisplayName") %>' />
                    <asp:DynamicFilter runat="server" ID="DynamicFilter" OnFilterChanged="OnFilterSelectionChanged" /><br />
                </ItemTemplate>
            </asp:QueryableFilterRepeater>
            
            <br />

            <asp:GridView ID="GridView1" runat="server" DataSourceID="GridDataSource" EnablePersistedSelection="true"
                AllowPaging="True" AllowSorting="True" CssClass="gridview">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="DeleteLinkButton" runat="server" CommandName="Delete"
                                CausesValidation="true" Text="Delete"
                                OnClientClick='return confirm("Are you sure you want to delete this item?");'
                            />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

                <PagerStyle CssClass="footer"/>        
                <PagerTemplate>
                    <asp:GridViewPager runat="server" />
                </PagerTemplate>
                <EmptyDataTemplate>
                    There are currently no items in this table.
                </EmptyDataTemplate>
            </asp:GridView>
            
            <aspX:LinqDataSource ID="GridDataSource" runat="server" EnableDelete="true" ContextTypeName="DynamicDataProject.NorthwindDataContext"
                TableName="Products">
            </aspX:LinqDataSource>
            
            <aspX:QueryExtender ID="QueryBlock1" TargetControlID="GridDataSource" runat="server">
                <asp:DynamicFilterExpression ControlID="FilterRepeater" />
            </aspX:QueryExtender>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
