<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="ProductsList.aspx.cs"
    Inherits="DynamicDataProject.ProductsList" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<asp:Content ID="headContent" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:DynamicDataManager ID="DynamicDataManager1" runat="server" AutoLoadForeignKeys="true" />
    <h2>
        <%= table.DisplayName%></h2>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                HeaderText="List of validation errors" />
            <asp:DynamicValidator runat="server" ID="GridViewValidator" ControlToValidate="ListView1"
                Display="None" />
            <asp:ListView ID="ListView1" runat="server" DataSourceID="GridDataSource" InsertItemPosition="FirstItem">
                <LayoutTemplate>
                    <table>
                        <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                    </table>
                </LayoutTemplate>
                <ItemTemplate>
                    <tr>
                        <td valign="top">
                            <asp:LinkButton runat="server" CommandName="Edit">Edit</asp:LinkButton>
                        </td>
                        <td>
                            <asp:DynamicEntity runat="server" />
                        </td>
                    </tr>
                </ItemTemplate>
                <EditItemTemplate>
                    <tr>
                        <td valign="top">
                            <asp:LinkButton runat="server" CommandName="Update">Update</asp:LinkButton>
                            <asp:LinkButton runat="server" CommandName="Cancel" CausesValidation="false">Cancel</asp:LinkButton>
                        </td>
                        <td>
                            <asp:DynamicEntity runat="server" Mode="Edit" />
                        </td>
                    </tr>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <tr>
                        <td valign="top">
                            <asp:LinkButton runat="server" CommandName="Insert" ValidationGroup="Insert">Insert</asp:LinkButton>
                            <asp:LinkButton runat="server" CommandName="Cancel" CausesValidation="false">Cancel</asp:LinkButton>
                        </td>
                        <td>
                            <asp:DynamicEntity runat="server" Mode="Insert" ValidationGroup="Insert" />
                        </td>
                    </tr>
                </InsertItemTemplate>
            </asp:ListView>
            
            <aspX:LinqDataSource ID="GridDataSource" runat="server" EnableUpdate="true" EnableInsert="true"
                EnableDelete="true" ContextTypeName="DynamicDataProject.NorthwindDataContext"
                TableName="Products">
            </aspX:LinqDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
