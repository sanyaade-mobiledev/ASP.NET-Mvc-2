﻿<%@ Control Language="C#" Inherits="Microsoft.Web.DynamicData.Mvc.MvcFieldTemplate" %>
<script runat="server">
    protected string DisplayName {
        get { return FormatFieldValue(FkColumn.ParentTable.GetDisplayString(FieldValue)); }
    }
    protected MetaForeignKeyColumn FkColumn {
        get { return (MetaForeignKeyColumn)Column; }
    }
    protected RouteValueDictionary FkRouteValues {
        get {
            var result = new RouteValueDictionary();

            foreach (var fkName in FkColumn.ForeignKeyNames) {
                object value = Table.Provider.EvaluateForeignKey(Entity, fkName);
                if (value != null)
                    result.Add(fkName, value);
            }

            return result;
        }
    }
</script>
<%= Html.ScaffoldLink(DisplayName, FkColumn.ParentTable, "show", FkRouteValues) %>