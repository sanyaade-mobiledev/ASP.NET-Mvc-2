<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ClassBrowser.aspx.cs"
    Inherits="DataSourcesDemo.Reflector" EnableViewState="false" Title="Class Browser" %>

<%@ Register TagPrefix="cb" Namespace="DataSourcesDemo.ClassBrowser.Expressions"
    Assembly="DataSourcesDemo" %>
<%@ Import Namespace="DataSourcesDemo.ClassBrowser" %>
<asp:Content ContentPlaceHolderID="head" runat="server">
    <link rel="Stylesheet" href="/Content/classbrowser.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="description">
        The example below shows how the <strong>DomainDataSource</strong> can be
        used with the <strong>&lt;Query&gt;</strong> to compose complex expressions.
    </div>
    <h3>Web Reflector</h3>
    <!-- Types -->
    <asp:DomainDataSource ID="typesSource" runat="server" DomainServiceTypeName="DataSourcesDemo.ClassBrowser.ReflectionDomainService"
        SelectMethod="GetTypes">
        <SelectParameters>
            <asp:ControlParameter ControlID="assemblies" Name="assemblyName" />
        </SelectParameters>
    </asp:DomainDataSource>
    <futures:QueryExtender  TargetControlID="typesSource" runat="server">    
        <asp:SearchExpression ComparisonType="OrdinalIgnoreCase" DataFields="Type.Name" SearchType="StartsWith">
            <asp:ControlParameter ControlID="searchTypes" />
        </asp:SearchExpression>            
        <asp:CustomExpression OnQuerying="OnQuery">
            <asp:ControlParameter ControlID="genericTypes" />
        </asp:CustomExpression>
        <cb:IncludeExpression>
            <asp:ControlParameter ControlID="classes" Name="Type.IsClass" />
            <asp:ControlParameter ControlID="enums" Name="Type.IsEnum" />
            <asp:ControlParameter ControlID="interfaces" Name="Type.IsInterface" />
        </cb:IncludeExpression>
        <asp:OrderByExpression DataField="Type.Name" Direction="Ascending" />
    </futures:QueryExtender>
    <table cellpadding="4">
        <tr>
            <td>
                <strong>Assembly:</strong>
            </td>
            <td>
                <asp:DropDownList ID="assemblies" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                    DataTextField="Key" DataValueField="Key">
                    <asp:ListItem Text="All" Value=""></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <strong>Search Types:</strong>
            </td>
            <td>
                <asp:TextBox ID="searchTypes" runat="server" Width="284px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <strong>Type Filters:</strong>
            </td>
            <td>
                <asp:CheckBox ID="classes" runat="server" AutoPostBack="true" Checked="true" Text="Classes" />
                <asp:CheckBox ID="enums" runat="server" AutoPostBack="true" Checked="true" Text="Enums" />
                <asp:CheckBox ID="interfaces" runat="server" AutoPostBack="true" Checked="true" Text="Interfaces" />
                <asp:CheckBox ID="genericTypes" runat="server" AutoPostBack="true" Checked="false"
                    Text="Generic Types" />
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:Button ID="search" runat="server" Text="Search" />
            </td>
        </tr>
    </table>
    <br />
    <asp:ListView runat="server" ID="types" DataKeyNames="Type" DataSourceID="typesSource"
        OnItemCommand="OnTypesCommand">
        <LayoutTemplate>
            <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
        </LayoutTemplate>
        <ItemTemplate>
            <div class="type">
                <strong>
                    <asp:ImageButton runat="server" ImageUrl="~/images/expand.png" CommandName="Select" />
                    <%# Formatter.GetImage(((TypeWrapper)Container.DataItem).Type)%>
                    <%# Formatter.FormatType(((TypeWrapper)Container.DataItem).Type, false)%>
                    <%# Formatter.FormatBaseType(((TypeWrapper)Container.DataItem).Type)%>
                    &nbsp;,&nbsp;
                    <%# Formatter.FormatAssemblyName(((TypeWrapper)Container.DataItem).Type.Assembly)%>
                </strong>
            </div>
        </ItemTemplate>
        <SelectedItemTemplate>
            <div class="type">
                <strong>
                    <asp:ImageButton runat="server" ImageUrl="~/images/minimize.png" CommandName="Minimize" />
                    <%# Formatter.GetImage(((TypeWrapper)Container.DataItem).Type)%>
                    <%# Formatter.FormatType(((TypeWrapper)Container.DataItem).Type, false)%>
                </strong>
                <div style="padding-bottom: 2px">
                </div>
                <!-- Properties -->
                <asp:DomainDataSource ID="propertiesSource" runat="server" DomainServiceTypeName="DataSourcesDemo.ClassBrowser.ReflectionDomainService"
                    SelectMethod="GetProperties">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="types" Name="typeName" PropertyName="SelectedDataKey.Value.FullName" />
                    </SelectParameters>
                </asp:DomainDataSource>
                <futures:QueryExtender TargetControlID="propertiesSource" runat="server">
                    <asp:OrderByExpression DataField="PropertyInfo.Name" Direction="Ascending" />
                </futures:QueryExtender>
                <asp:ListView runat="server" ID="properties" DataSourceID="propertiesSource">
                    <LayoutTemplate>
                        <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <div class="member">
                            <img src="/images/property.png" alt="property" />
                            <%# Formatter.FormatProperty(((PropertyInfoWrapper)Container.DataItem).PropertyInfo)%>
                        </div>
                    </ItemTemplate>
                </asp:ListView>
                <!-- Methods  -->
                <asp:DomainDataSource ID="methodSource" runat="server" DomainServiceTypeName="DataSourcesDemo.ClassBrowser.ReflectionDomainService"
                    SelectMethod="GetMethods">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="types" Name="typeName" PropertyName="SelectedDataKey.Value.FullName" />
                    </SelectParameters>
                </asp:DomainDataSource>
                <futures:QueryExtender TargetControlID="methodSource" runat="server">
                    <asp:OrderByExpression DataField="MethodInfo.Name" Direction="Ascending" />
                </futures:QueryExtender>
                <asp:ListView runat="server" ID="methods" DataSourceID="methodSource">
                    <LayoutTemplate>
                        <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <div class="member">
                            <img src="/images/method.png" alt="method" />
                            <%# Formatter.FormatMethod(((MethodInfoWrapper)Container.DataItem).MethodInfo)%>
                        </div>
                    </ItemTemplate>
                </asp:ListView>
                <!-- Fields -->
                <asp:DomainDataSource ID="fieldsSource" runat="server" DomainServiceTypeName="DataSourcesDemo.ClassBrowser.ReflectionDomainService"
                    SelectMethod="GetFields">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="types" Name="typeName" PropertyName="SelectedDataKey.Value.FullName" />
                    </SelectParameters>
                </asp:DomainDataSource>
                <futures:QueryExtender TargetControlID="fieldsSource" runat="server">
                    <asp:OrderByExpression DataField="FieldInfo.Name" Direction="Ascending" />
                </futures:QueryExtender>
                <asp:ListView runat="server" ID="fields" DataSourceID="fieldsSource">
                    <LayoutTemplate>
                        <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <div class="member">
                            <img src="/images/field.png" alt="field" />
                            <%# Formatter.FormatField(((FieldInfoWrapper)Container.DataItem).FieldInfo)%>
                        </div>
                    </ItemTemplate>
                </asp:ListView>
            </div>
        </SelectedItemTemplate>
        <EmptyDataTemplate>
            <strong>No Results found</strong>
        </EmptyDataTemplate>
    </asp:ListView>
    <br />
    <br />
    <asp:DataPager ID="typesPager" PageSize="30" PagedControlID="types" runat="server">
        <Fields>
            <asp:NumericPagerField />
        </Fields>
    </asp:DataPager>
</asp:Content>
