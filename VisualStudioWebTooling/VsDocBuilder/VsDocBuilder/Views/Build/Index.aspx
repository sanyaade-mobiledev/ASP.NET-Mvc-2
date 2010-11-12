﻿<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>-vsdoc Builder</title>
    <link href="../../Content/Site.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <div id="pageContainer">
        <div id="memberList">
            <select id="members" multiple="multiple" size="22">
                <option>loading...</option>
            </select>
        </div>
        <div id="memberDetails">
            <input id="merge" type="button" value="Download &amp; Merge Doc" />
            <span class="checkbox" id="useParaCheckbox"><input type="checkbox" id="usePara" /><label for="usePara">Generate &lt;para&gt;</label></span>
            <p id="status"></p>
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
                    <label for="docComment">Doc Comment:</label>
                    <pre id="docComment"></pre>
                </li>
                <li>
                    <label for="value">Body:</label>
                    <pre id="value"></pre>
                </li>
            </ul>
        </div>
        <div id="outputPane" class="wide">
            <label for="output">Log:</label>
            <textarea id="output" rows="6" cols="120" readonly="readonly" wrap="off"></textarea>
        </div>
        <div id="buildDocPane" class="wide">
            <input type="button" id="buildDoc" value="Build Doc File" />
            <label for="docFile">Doc File:</label>
            <textarea id="docFile" rows="20" cols="120" readonly="readonly" wrap="off"></textarea>
        </div>
    </div>
    <script src="../../Scripts/jquery-1.4.3.js" type="text/javascript"></script>
    <script src="../../Scripts/underscore.js" type="text/javascript"></script>
    <script src="../../Scripts/build.js" type="text/javascript"></script>
</body>
</html>