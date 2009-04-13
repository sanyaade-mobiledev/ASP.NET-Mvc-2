<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="DataSourcesDemo.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<h2>WebForms</h2>
    This project showcases some of the new functionality in ASP.NET.
    <ul>
        <li>
            <strong>DomainDataSource</strong> (formerly known as BusinessLogicDataSource)
        </li>
        <li>
            <strong>DomainValidator</strong> - A validator that knows about the DomainService's validation framework
        </li>
        <li>
            <strong>QueryExtender</strong> - A control that offers a declarative syntax for expressing queries against datasources
        </li>
    </ul>
    Select a sample from the left to begin.
</asp:Content>
