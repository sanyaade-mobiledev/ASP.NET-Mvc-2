﻿<%@ Page Language="C#" MasterPageFile="~/Site.master" CodeBehind="ForeignKeyFilter.aspx.cs" Inherits="DynamicDataProject.SamplePages.ForeignKeyFilter" %>

<%@ Register src="~/DynamicData/Content/GridViewPager.ascx" tagname="GridViewPager" tagprefix="asp" %>

<asp:Content ID="headContent" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:DynamicDataManager ID="DynamicDataManager1" runat="server" AutoLoadForeignKeys="true" />

    <h2><%= table.DisplayName%> ForeignKey filter</h2>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                HeaderText="List of validation errors" />
            <asp:DynamicValidator runat="server" ID="GridViewValidator" ControlToValidate="GridView1" Display="None" />
            
            <asp:Button ID="PostBackButton" runat="server" Text="Cause Postback" /><br />
            Category: <asp:DynamicFilter runat="server" ID="CategoryFilter" DataField="Category" Filter="ForeignKey" OnFilterChanged="OnFilterSelectedIndexChanged" /><br />

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
                <asp:DynamicFilterExpression ControlID="CategoryFilter" />
            </aspX:QueryExtender>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
