<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="DynamicObjectDataSourceTest.aspx.cs"
    Inherits="DynamicDataFuturesSample.DynamicObjectDataSourceTest.DynamicObjectDataSourceTest" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                HeaderText="List of validation errors" />
            <asp:DynamicValidator runat="server" ID="DetailsViewValidator" ControlToValidate="DetailsView1" Display="None" />

            <asp:DetailsView ID="DetailsView1" runat="server" AllowPaging="True" AutoGenerateInsertButton="True"
                DataSourceID="ObjectDataSource1" EnableModelValidation="true">
            </asp:DetailsView>
            
            <asp:DynamicObjectDataSource ID="ObjectDataSource1" runat="server" DataObjectTypeName="DynamicDataFuturesSample.NewData"
                InsertMethod="Insert" SelectMethod="Select" TypeName="DynamicDataFuturesSample.AggregateData">
            </asp:DynamicObjectDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
