<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="DynamicDataProject.SamplePages.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ul>
        <li>
            <asp:HyperLink runat="server" NavigateUrl="~/FilterSamples/IndividualFilters.aspx">Individual filters</asp:HyperLink>
            <ul>
                <li>
                    <asp:HyperLink runat="server" NavigateUrl="~/FilterSamples/AutocompleteFilter.aspx">Autocomplete filter</asp:HyperLink></li>
                <li>
                    <asp:HyperLink runat="server" NavigateUrl="~/FilterSamples/BooleanFilter.aspx">Boolean filter</asp:HyperLink></li>
                <li>
                    <asp:HyperLink runat="server" NavigateUrl="~/FilterSamples/BooleanRadioFilter.aspx">BooleanRadio filter</asp:HyperLink></li>
                <li>
                    <asp:HyperLink runat="server" NavigateUrl="~/FilterSamples/ForeignKeyFilter.aspx">ForeignKey filter</asp:HyperLink></li>
                <li>
                    <asp:HyperLink runat="server" NavigateUrl="~/FilterSamples/MultiForeignKeyFilter.aspx">MultiForeignKey filter</asp:HyperLink>
                </li>
                <li>
                    <asp:HyperLink runat="server" NavigateUrl="~/FilterSamples/RangeFilter.aspx">Range filter</asp:HyperLink></li>
            </ul>
        </li>
        <li>
            <asp:HyperLink runat="server" NavigateUrl="~/FilterSamples/FilterRepeater.aspx">Filter repeater</asp:HyperLink></li>
        <li>
            <asp:HyperLink runat="server" NavigateUrl="~/FilterSamples/ListDetails.aspx">List-Details</asp:HyperLink></li>
        <li>
            <asp:HyperLink runat="server" NavigateUrl="~/FilterSamples/ParentChildren.aspx">Parent-Children</asp:HyperLink></li>
        <li>
            <asp:HyperLink runat="server" NavigateUrl="~/FilterSamples/Everything.aspx">Everything - illustrates all the above concepts applied in one page.</asp:HyperLink></li>
    </ul>
</asp:Content>
