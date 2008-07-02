<%@ Page Language="C#" MasterPageFile="~/Site.master" CodeBehind="Edit.aspx.cs" Inherits="DynamicDataFuturesSample.Regions_Edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:DynamicDataManager ID="DynamicDataManager1" runat="server" AutoLoadForeignKeys="true" />
    
    <p>Click on the "Region Description". The cursor will focus on the text box.</p>
    
    <h2>Edit entry from table <%= table.DisplayName %></h2>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        
            <asp:FormView ID="FormView1" runat="server" DataKeyNames="RegionID" DataSourceID="DetailsDataSource" DefaultMode="Edit">
                <EditItemTemplate>
                    <table class="detailstable">
                        <tr>
                            <td class="bold"><asp:DynamicLabel ID="DynamicLabelRegionID" runat="server" AssociatedControlID="DynamicControlRegionID" Text="Region ID" /></td>
                            <td><asp:DynamicControl ID="DynamicControlRegionID" runat="server" Mode="Edit" DataField="RegionID" /></td>
                        </tr>
                        <tr>
                            <td class="bold"><asp:DynamicLabel ID="DynamicLabelRegionDescription" runat="server" AssociatedControlID="DynamicControlRegionDescription" Text="Region Description:" /></td>
                            <td><asp:DynamicControl ID="DynamicControlRegionDescription" runat="server" Mode="Edit" DataField="RegionDescription" /></td>
                        </tr>
                        <tr>
                            <td class="bold"><asp:DynamicLabel ID="DynamicLabelTerritories" runat="server" AssociatedControlID="DynamicControlTerritories" Text="Territories" /></td>
                            <td><asp:DynamicControl ID="DynamicControlTerritories" runat="server" Mode="Edit" DataField="Territories" /></td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:LinkButton ID="UpdateButton" runat="server" CausesValidation="True" CommandName="Update" Text="Update" />
                                <asp:LinkButton ID="UpdateCancelButton" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel" />
                            </td>
                        </tr>
                    </table>
                </EditItemTemplate>
            </asp:FormView>
            
            <asp:LinqDataSource ID="DetailsDataSource" runat="server" EnableUpdate="True">
                <WhereParameters>
                    <asp:DynamicQueryStringParameter />
                </WhereParameters>
            </asp:LinqDataSource>
            
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
