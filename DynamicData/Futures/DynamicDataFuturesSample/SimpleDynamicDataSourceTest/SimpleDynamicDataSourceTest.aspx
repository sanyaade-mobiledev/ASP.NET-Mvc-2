<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="SimpleDynamicDataSourceTest.aspx.cs" Inherits="DynamicDataFuturesSample.SimpleDynamicDataSourceTest" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <h2>Data input</h2>
            <asp:ValidationSummary runat="server" EnableClientScript="true"
                HeaderText="List of validation errors" />

            <asp:DetailsView ID="DetailsView1" runat="server" DefaultMode="Edit" DataSourceID="DataSource1"
                AutoGenerateEditButton="true" EnableModelValidation="true">
            </asp:DetailsView>
            
            <asp:SimpleDynamicDataSource ID="DataSource1" runat="server">
            </asp:SimpleDynamicDataSource>
        
            <h2>Result</h2>
            <asp:DetailsView ID="DetailsView2" runat="server">
            </asp:DetailsView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
