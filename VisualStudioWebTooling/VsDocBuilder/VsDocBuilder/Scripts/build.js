﻿/// <reference path="jquery-1.4.2.js" />
/// <reference path="underscore.js" />

String.prototype.supplant = function (o) {
    return this.replace(/{([^{}]*)}/g,
        function (a, b) {
            var r = o[b];
            return typeof r === 'string' || typeof r === 'number' || typeof r === 'function' ? r : a;
        }
    );
};

$(function () {
    var members = [],
        membersP = [],
        $usePara = $("#usePara"),
        $name = $("#name"),
        $aliases = $("#aliases"),
        $type = $("#type"),
        $docComment = $("#docComment"),
        $value = $("#value"),
        $output = $("#output");

    function log(msg) {
        var txt = $.trim($output.val());
        $output.val((txt.length > 0 ? txt + "\r\n" : "") + msg);
    }

    members.push({
        name: "jQuery",
        aliases: "$",
        ref: jQuery,
        doc: ""
    });

    var m;
    for (m in jQuery) {
        members.push({
            name: "jQuery." + m,
            aliases: "",
            ref: jQuery[m],
            doc: ""
        });
    }
    for (m in jQuery.prototype) {
        membersP.push({
            name: "jQuery.prototype." + m,
            aliases: "",
            ref: jQuery.prototype[m],
            doc: ""
        });
    }
    members.sort(function (a, b) {
        return a.name > b.name ? 1 : -1;
    });
    membersP.sort(function (a, b) {
        return a.name > b.name ? 1 : -1;
    });

    var $members = $("#members");
    $members.find("option").remove();
    $.each(members.concat(membersP), function () {
        $("<option id='{name}'>{name}</option>".supplant(this))
            .data("m", this)
            .appendTo($members);
    });

    $members.bind("click keyup", function () {
        var i = this.selectedIndex;
        var m = $(this).find("option").eq(i).data("m");
        $name.val(m.name);
        $aliases.val(m.aliases);
        $type.val(typeof (m.ref));
        function linify(t) {
            return $("<span/>").text(t).html().replace(/\r\n/g, "<br/>");
        }
        $value.text(linify(serialize(m.ref, true)));
        $docComment.html(linify(m.doc));
    });

    var argsRegEx = /\s*function\s*\((.*)\)/;

    function injectParaTags(text) {
        /// <param name="text" type="String">The text to inject para tags into</param>
        var result = $.trim(text)
            .replace(/(\r\n)|\n/g, "</para>\r\n<para>") // Replace all new lines with </para>\r\n<para>
            .replace(/<\/para>/, ""); // Remove the first </para> as the first paragraph doesn't need to be wrapped
        if (result.indexOf("<para>") >= 0)
            result = result + "</para>"; // Add last closing </para>
        return result;
    }

    function injectNewLinePrefixSlashes(text, paddingLength) {
        text = $.trim(text);
        if (typeof (paddingLength) !== "number") paddingLength = 4;
        var padding = "";
        _(paddingLength).times(function () { padding = padding + " "; });
        return text.replace(/(\r\n)|\n/g, "\r\n/// " + padding);
    }

    function makeDocComment(entry, ref, aliases, generatePara) {
        // { name:"", returns:"", summary:"", parameters: [{ name:"", type:"", summary:""}] }
        if (!entry || !entry.summary) return "";

        if (generatePara) entry.summary = injectParaTags(entry.summary);

        entry.summary = injectNewLinePrefixSlashes($.trim(entry.summary));

        if (typeof (aliases) === "string" && $.trim(aliases) !== "") {
            // Alias is defined, replace instances of entry name with first alias name in summary
            var alias = aliases;
            if (aliases.indexOf(",") > 0) {
                alias = aliases.split(",")[0];
            }
            alias = $.trim(alias);
            entry.summary = entry.summary.replace(new RegExp(entry.name + "\\(", "g"), alias + "(");
        }

        var comment = ("/// <summary>\r\n" +
                       "///     {summary}\r\n" +
                       "/// </summary>\r\n").supplant(entry);

        if (typeof (ref) !== "function")
            return comment;

        // Get real parameter names from actual signature
        var paramMatches = argsRegEx.exec(ref.toString());
        if (paramMatches) {
            var realParameters = paramMatches[1].replace(/\s/g, "").split(",");

            if (entry.parameters) {
                $.each(entry.parameters, function (i) {
                    // { name:"", type:"", summary:"" }
                    this.name = realParameters[i];
                    comment += ("/// <param name=\"{name}\" type=\"{type}\">\r\n" +
                                "///     {summary}\r\n" +
                                "/// </param>\r\n")
                        .supplant({
                            name: $.trim(this.name),
                            type: $.trim(this.type),
                            summary: injectNewLinePrefixSlashes(generatePara ? injectParaTags($.trim(this.summary)) : $.trim(this.summary))
                        });

                    if (this.type === "Element")
                        comment = comment.replace("type=\"Element\"", "domElement=\"true\"");
                });
            }
        }

        if (entry.returns) {
            comment = comment + "/// <returns type=\"{returns}\" />\r\n".supplant(entry);
        }

        return comment;
    }

    function serialize(obj, recurse) {
        if (typeof obj !== "undefined") {
            if ($.isArray(obj)) {
                var vArr = "[";
                for (var i = 0; i < obj.length; i++) {
                    if (i > 0) vArr += ",";
                    vArr += serialize(obj[i], recurse);
                }
                vArr += "]"
                return vArr;

            } else if (typeof obj === "string") {
                return "'" + obj + "'";

            } else if (typeof obj === "number") {
                return isFinite(obj) ? obj.toString() : null;

            } else if (typeof obj === "object") {
                if (typeof obj.getDay === "function") {
                    return "new Date({y}, {m}, {d})".supplant({
                        y: obj.getFullYear(),
                        m: obj.getMonth(),
                        d: obj.getDate()
                    });
                }
                if (recurse) {
                    var vobj = [];
                    for (attr in obj) {
                        if (typeof obj[attr] !== "function") {
                            vobj.push('"' + attr + '": ' + serialize(obj[attr], false));
                        }
                    }
                    if (vobj.length > 0)
                        return "{ " + vobj.join(",\r\n") + " }";
                    else
                        return "{}";
                } else {
                    return "{}";
                }
            } else {
                return obj.toString();
            }
        }
        return "";
    }

    function downloadAndMergeDoc() {
        var t = this;
        t.disabled = true;
        var $status = $("#status").show().text("downloading documentation...");

        var jQueryDocJsonUrl = document.location.toString();
        if (jQueryDocJsonUrl.lastIndexOf("/") === jQueryDocJsonUrl.length - 1) {
            jQueryDocJsonUrl = jQueryDocJsonUrl.substr(0, jQueryDocJsonUrl.length - 1);
        }
        jQueryDocJsonUrl += "/jQueryDoc";

        if (document.location.search != "") {
            jQueryDocJsonUrl = jQueryDocJsonUrl.replace("/" + document.location.search, "");
        }

        $.getJSON(jQueryDocJsonUrl, null, function (doc) {
            // doc = { name:"", returns:"", summary:"", parameters: [{ name:"", type:"", summary:""}] }
            $status.text("merging...");

            var docEntriesFoundOnOppositeToExpected = [];
            var docEntriesWithNoMatch = [];

            var generatePara = $usePara.get(0).checked;

            $.each(doc, function () {
                var name = this.name;
                if (name !== "jQuery") {
                    name = name.substr(0, "jQuery.".length) === "jQuery." ?
                       name.replace(".", "\\.") : "jQuery\\.prototype\\." + name;
                }

                var $option = $("#" + name).eq(0);

                if ($option.length === 0) {

                    var nameToTry = name.indexOf("jQuery\\.prototype") === 0 ?
                        name.replace("\\.prototype\\.", "\\.") :
                        name.replace("jQuery\\.", "jQuery\\.prototype\\.");

                    $option = $("#" + nameToTry).eq(0);

                    if ($option.length === 0) {
                        docEntriesWithNoMatch.push(this);
                        return true;
                    }

                    docEntriesFoundOnOppositeToExpected.push(this);
                }

                var data = $option.data("m");
                if (data) {
                    data.doc = makeDocComment(this, data.ref, data.aliases, generatePara);
                    $option.data("m", data);
                    if (data.doc) {
                        $option.addClass("has-doc");
                    }
                }
            });

            var problemMembersTemplate = "  {name}({params}) : {summary}";
            $.each([[docEntriesFoundOnOppositeToExpected, "\r\nThe following {length} entries in the jQuery doc API were found in the wrong place (protoype instead of function or vice versa):\r\n"],
                    [docEntriesWithNoMatch, "\r\nThe following {length} entries in the jQuery doc API had no matching members on the jQuery object:\r\n"]],
                function () {
                    var arr = this[0],
                        msg = this[1];
                    if (arr.length > 0) {
                        log(msg.supplant(arr));
                        $.each(arr, function () {
                            log(problemMembersTemplate.supplant(
                                { name: this.name,
                                    params: _.pluck(this.parameters, "name").join(", "),
                                    summary: $.trim(this.summary)
                                }) + "\r\n"
                            );
                        });
                    }
                }
            );

            $("#members").css("visibility", "visible"); // Need to force a re-layout

            $status.text("done!");
            window.setTimeout(function () {
                $status.fadeOut("fast", function () {
                    $status.text("")
                });
            }, 3000);
            t.disabled = false;

        });
    }

    downloadAndMergeDoc();
    window.setTimeout(function () {
        $members.get(0).selectedIndex = 0;
        $members.click();
    }, 100);

    var jQueryPrivates = {
        access: function (elems, key, value, exec, fn, pass) {
            var length = elems.length;
            // Setting many attributes
            if (typeof key === "object") { for (var k in key) { access(elems, k, key[k], exec, fn, value); } return elems; }
            // Setting one attribute
            if (value !== undefined) {
                // Optionally, function values get executed if exec is true
                exec = !pass && exec && jQuery.isFunction(value); for (var i = 0; i < length; i++) { fn(elems[i], key, exec ? value.call(elems[i], i, fn(elems[i], key)) : value, pass); } return elems;
            }
            // Getting an attribute
            return length ? fn(elems[0], key) : undefined;
        }
    };

    $("#buildDoc").click(function () {
        var file = "", member;

        function injectDoc(fnString, doc) {
            var injectAt = fnString.indexOf("{") + 1;
            return fnString.substr(0, injectAt) + "\r\n" + doc + fnString.substr(injectAt);
        }

        file += "/*\r\n" +
                "* This file has been generated to support Visual Studio IntelliSense.\r\n" +
                "* You should not use this file at runtime inside the browser--it is only\r\n" +
                "* intended to be used only for design-time IntelliSense.  Please use the\r\n" +
                "* standard jQuery library for all production use.\r\n" +
                "*\r\n" +
                "* Comment version: {version}\r\n" +
                "*/\r\n\r\n";

        file += "/*!\r\n" +
                "* jQuery JavaScript Library v{version}\r\n" +
                "* http://jquery.com/\r\n" +
                "*\r\n" +
                "* Distributed in whole under the terms of the MIT\r\n" +
                "*\r\n" +
                "* Copyright 2010, John Resig\r\n" +
                "*\r\n" +
                "* Permission is hereby granted, free of charge, to any person obtaining\r\n" +
                "* a copy of this software and associated documentation files (the\r\n" +
                "* \"Software\"), to deal in the Software without restriction, including\r\n" +
                "* without limitation the rights to use, copy, modify, merge, publish,\r\n" +
                "* distribute, sublicense, and/or sell copies of the Software, and to\r\n" +
                "* permit persons to whom the Software is furnished to do so, subject to\r\n" +
                "* the following conditions:\r\n" +
                "*\r\n" +
                "* The above copyright notice and this permission notice shall be\r\n" +
                "* included in all copies or substantial portions of the Software.\r\n" +
                "*\r\n" +
                "* THE SOFTWARE IS PROVIDED \"AS IS\", WITHOUT WARRANTY OF ANY KIND,\r\n" +
                "* EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF\r\n" +
                "* MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND\r\n" +
                "* NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE\r\n" +
                "* LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION\r\n" +
                "* OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION\r\n" +
                "* WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.\r\n" +
                "*\r\n" +
                "* Includes Sizzle.js\r\n" +
                "* http://sizzlejs.com/\r\n" +
                "* Copyright 2010, The Dojo Foundation\r\n" +
                "* Released under the MIT and BSD Licenses.\r\n" +
                "*/\r\n\r\n";

        file = file.supplant({ version: $("#version").val() });

        file += "(function ( window, undefined ) {\r\n";

        $members.find("option").each(function () {
            member = $(this).data("m");
            var refBody = member.ref.toString();

            if (refBody.indexOf("[native code]") >= 0 || $.trim(refBody) === "" || typeof (member.ref) === "string")
                return true;

            if (member.name === "jQuery") {
                file += "var jQuery = " + injectDoc(refBody, member.doc) + ";";
                for (var priv in jQueryPrivates) {
                    file += "\r\nfunction {name} {body};".supplant({
                        name: priv,
                        body: jQueryPrivates[priv].toString().substr("function ".length)
                    });
                }
            } else {
                //if (member.name === "jQuery.data") debugger;
                file += "\r\n{name} = {body};".supplant({
                    name: member.name,
                    body: typeof (member.ref) === "function" ? injectDoc(refBody, member.doc) : serialize(member.ref, true)
                });
            }
        });

        file += "\r\njQuery.fn = jQuery.prototype;";
        file += "\r\njQuery.fn.init.prototype = jQuery.fn;";
        file += "\r\nwindow.jQuery = window.$ = jQuery;";
        file += "\r\n})(window);";

        $("#docFile").val(file);
    });

    $("#outputPane label").click(function () {
        $(this).next().toggle();
    });
});