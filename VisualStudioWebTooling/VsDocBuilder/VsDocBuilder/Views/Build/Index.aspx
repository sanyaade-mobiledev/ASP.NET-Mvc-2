<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<BuildModel>" %>
<%@ Import Namespace="VsDocBuilder.Models" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>-vsdoc Builder</title>
    <link href="../../Content/Site.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <div id="pageContainer">
        <h1>jQuery vsdoc File Generator <span>for Visual Studio 2010</span></h1>
        <div id="chooseVersion">
            <form method="get" action="./">
                <label for="version">Choose a jQuery version:</label>
                <%= Html.DropDownList("ver", Model.Versions, new { id = "version" })%>
                <span class="checkbox" id="useParaCheckbox"><%= Html.CheckBox("para", Model.GenerateParaTags, new { id = "usePara"}) %><label for="usePara">Generate &lt;para&gt;</label></span>
                <input type="submit" value="Go!" />
            </form>
            <p id="status"></p>
        </div>
        <div id="memberPane">
            <div id="memberList">
                <select id="members" multiple="multiple" size="22">
                    <option>loading...</option>
                </select>
            </div>
            <div id="memberDetails">
                <ul>
                    <li>
                        <label for="name">Name:</label>
                        <input id="name" type="text" readonly="readonly" size="50" />
                    </li>
                    <li>
                        <label for="aliases">Aliases:</label>
                        <input id="aliases" type="text" size="50" />
                    </li>
                    <li>
                        <label for="type">Type:</label>
                        <input id="type" type="text" readonly="readonly" size="50" />
                    </li>
                    <li>
                        <label for="value">Body:</label>
                        <code id="value"></code>
                    </li>
                    <li>
                        <label for="docComment">Doc Comment:</label>
                        <code id="docComment"></code>
                    </li>
                </ul>
            </div>
        </div>
        <div id="buildDocPane" class="wide">
            <input type="button" id="buildDoc" value="Build Doc File" />
            <label for="docFile">Doc File:</label>
            <textarea id="docFile" rows="20" cols="120" readonly="readonly" wrap="off"></textarea>
        </div>
        <div id="outputPane" class="wide">
            <label for="output">Log:</label>
            <textarea id="output" rows="6" cols="120" readonly="readonly" wrap="off"></textarea>
        </div>
    </div>
    <script src="../../Scripts/jquery-<%= Model.Version %>.js" type="text/javascript"></script>
    <script src="../../Scripts/underscore.js" type="text/javascript"></script>
    <script src="../../Scripts/build.js" type="text/javascript"></script>
</body>
</html>