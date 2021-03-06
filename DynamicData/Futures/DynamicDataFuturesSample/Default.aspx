<%@ Page Language="C#" MasterPageFile="~/Site.master" CodeBehind="Default.aspx.cs" Inherits="DynamicDataFuturesSample._Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="starting_page">
    <h2>Dynamic Data Futures sample site</h2>
    
    <p>This site demonstrates the use of various features supported by the Dynamic Data 
        Futures assembly.&nbsp; 
        Click <a href="TableList.aspx">here</a> to go to the standard DynamicData 
        starting page that lists all the tables.</p>
        
    <h3>Set metadata attributes programmatically in addition to declaratively</h3>
    <p>Normally, when using Dynamic Data, you set declarative attributes on your model 
        using CLR attributes.&nbsp; But for more dynamic scenarios, you may want to set 
        those attributes via code.&nbsp; The InMemoryMetadataManager class supports this 
        scenario.</p>
    <p><asp:DynamicHyperLink runat="server" TableName="Products" Action="Edit" ProductID="1">Click here</asp:DynamicHyperLink> to see this being used 
        in the standard Edit Product page.&nbsp; A call is made in global.asax.cs to set 
        a range programmatically on the UnitsInStock column.&nbsp; Try setting a value 
        greater than 1000.</p>
    
    <h3>Display database image columns</h3>
    <p>By default, Dynamic Data does not display image (or in general all binary) fields 
        present in a database. The DbImage and DbImage_Edit field templates display the 
        Picture column as &lt;img&gt; tags that point to a custom HTTP handler that retrieves 
        the images from the database and serves them as the response. The edit template 
        allows you to upload an image to replace the existing one.</p>
    <p><asp:DynamicHyperLink runat="server" TableName="Categories">Click here</asp:DynamicHyperLink> to see the Picture column in the Category table display an image from 
        the database in the default List scaffold page.</p>
        
    <h3>Populate Insert templates with values from filters</h3>
    <p>By default, Dynamic Data does not prepopulate relationship (foreign key) fields 
        in an Insert view with default values. The 
        DefaultValueHelper class contains helper methods that set up the Dynamic Data 
        templates in such a way that the relationship fields are populated using values 
        from the URL.</p>
    <p><asp:DynamicHyperLink runat="server" TableName="Territories">Click here</asp:DynamicHyperLink> to see this being used in the standard scaffold 
        Insert page for the 
        Territories table. Note that the table is already filtered by the Western region. Click on 
        the &quot;Insert new item&quot; link (or <asp:DynamicHyperLink runat="server" Action="Insert" TableName="Territories" RegionID="2">here</asp:DynamicHyperLink>). 
        Note that the Region dropdown will already have the Western region selected as the value.</p>
    
    <h3>Use attributes to order the fields displayed by Dynamic Data</h3>
    <p>Normally, when a Dynamic Data page is displayed using a page template, all the 
        fields are shown in the order in which they are declared on the class.&nbsp; One 
        way to reorder them is to create a custom page, but sometimes this is overkill.&nbsp; 
        This new feature gives you an alternative: add declarative ColumnOrder 
        attributes to the model to influence the ordering.&nbsp; The default 'order' is 
        0, so use a negative number for fields you want to show first, and a positive 
        number for fields you want to show last.</p>
    <p><asp:DynamicHyperLink runat="server" TableName="Products">Click here</asp:DynamicHyperLink>  to see this feature applied to the 
        Products list page using the standard scaffold.&nbsp; e.g. note how the 
        UnitsInStock column is displayed first because it has the lowest ColumnOrder 
        number.</p>

    <h3>Using Dynamic Data on simple objects</h3>
    <p>This demonstrates how Dynamic Data can be used as a data input framework in 
        simple scenarios that don't involve any database.&nbsp; Basically, you pass it some 
        object of any type, and it gives you a Dynamic Data UI to edit its fields.&nbsp; Once 
        it's done, you get the new object back!&nbsp; You can customize the UI using the 
        standard Dynamic Data techniques: add metadata attributes to the various fields 
        to affect their behavior.</p>
    <p><a href="SimpleDynamicDataSourceTest/SimpleDynamicDataSourceTest.aspx">Click here</a> 
        to see a sample page that uses this.</p>
        
    <h3>Using Dynamic Data with ObjectDataSource</h3>
    <p>This demonstrates how Dynamic Data can be used in scenarios that use an 
        ObjectDataSource.&nbsp; This is done by using the derived 
        DynamicObjectDataSource, which adds the functionality that allows Dynamic Data 
        to work with ObjectDataSource.</p>
    <p><a href="DynamicObjectDataSourceTest/DynamicObjectDataSourceTest.aspx">Click here</a> 
        to see a sample page that uses this.</p>
        
    <h3>Use enumerated column types</h3>
    <p>The LINQ to SQL designer allows you to declare the type of a column to be an enumerated type. 
        The Enumeration_Edit field template takes advantage of this information to 
        generate a UI that contains a list of the possible values for the enumerated 
        type.</p>
    <p><asp:DynamicHyperLink runat="server" TableName="Products" Action="Edit" ProductID="1">Click</asp:DynamicHyperLink> to see this in action. The 
        ReorderLevel column of the Products table has been mapped to a new enumerated 
        type ReorderLevelEnum. The edit UI for the ReorderLevel column shows a drop-down 
        with a list of potential values from that type.</p>
        
    <h3>Use pretty URLs</h3>
    <p>The new PrettyDynamicDataRoute extends DynamicDataRoute and makes it easy to have 
        an app automatically use &#39;pretty&#39; URLs for all tables.&nbsp; e.g. instead of 
        /Products/Edit.aspx?ProductID=5, you can have /Products/Edit/5.&nbsp; You can 
        see this route being used in global.asax.cs.</p>
    <p><a href="Products/Edit/5">Click here</a> to go to a product Edit page, and check 
        out what the URL looks like.</p>
</div>    
</asp:Content>


