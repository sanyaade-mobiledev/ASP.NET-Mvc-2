// Name:        PreviewScript.debug.js
// Assembly:    Microsoft.Web.Preview
// Version:     2.0.21022.0
// FileVersion: 2.0.21022.0
Type.registerNamespace('Sys.Preview');
if (Sys.Browser.agent !== Sys.Browser.InternetExplorer) {
    XMLDocument.prototype.selectNodes = function XMLDocument$selectNodes(path, contextNode) {
        contextNode = contextNode ? contextNode : this;
        var xpath = new XPathEvaluator();
        var result = xpath.evaluate(path, contextNode,
                                    this.createNSResolver(doc.documentElement),
                                    XPathResult.ORDERED_NODE_SNAPSHOT_TYPE, null);
        var nodeList = new Array(result.snapshotLength);
        for(var i = 0; i < result.snapshotLength; i++) {
            nodeList[i] = result.snapshotItem(i);
        }
        return nodeList;
    }
    XMLDocument.prototype.selectSingleNode = function XMLDocument$selectSingleNode(path, contextNode) {
        path += '[1]';
        var nodes = this.selectNodes(path, contextNode);
        if (nodes.length != 0) {
            for (var i = 0; i < nodes.length; i++) {
                if (nodes[i]) {
                    return nodes[i];
                }
            }
        }
        return null;
    }
    XMLDocument.prototype.transformNode = function XMLDocument$transformNode(xsl) {
        var xslProcessor = new XSLTProcessor();
        xslProcessor.importStylesheet(xsl);
        var ownerDocument = document.implementation.createDocument("", "", null);
        var transformedDoc = xslProcessor.transformToDocument(this);
        return transformedDoc.xml;
    }
    Node.prototype.selectNodes = function Node$selectNodes(path) {
        var doc = this.ownerDocument;
        return doc.selectNodes(path, this);
    }
    Node.prototype.selectSingleNode = function Node$selectSingleNode(path) {
        var doc = this.ownerDocument;
        return doc.selectSingleNode(path, this);
    }
    Node.prototype.__defineGetter__('baseName', function() {
        return this.localName;
    });
    Node.prototype.__defineGetter__('text', function() {
        return this.textContent;
    });
    Node.prototype.__defineSetter__('text', function(value) {
        this.textContent = value;
    });
    Node.prototype.__defineGetter__('xml', function() {
        return (new XMLSerializer()).serializeToString(this);
    });
}
if (Sys.Browser.agent === Sys.Browser.Firefox) {
    DocumentFragment.prototype.getElementById = function DocumentFragment$getElementById(id) {
        var nodeQueue = [];
        var childNodes = this.childNodes;
        var node;
        var c;
        for (c = 0; c < childNodes.length; c++) {
            node = childNodes[c];
            if (node.nodeType == 1) {
                Array.enqueue(nodeQueue, node);
            }
        }
        while (nodeQueue.length) {
            node = Array.dequeue(nodeQueue);
            if (node.id == id) {
                return node;
            }
            childNodes = node.childNodes;
            if (childNodes.length != 0) {
                for (c = 0; c < childNodes.length; c++) {
                    node = childNodes[c];
                    if (node.nodeType == 1) {
                        Array.enqueue(nodeQueue, node);
                    }
                }
            }
        }
        return null;
    }
    DocumentFragment.prototype.createElement = function DocumentFragment$createElement(tagName) {
        return document.createElement(tagName);
    }
}
Sys.Preview.MarkupParser = new function() {
}
Sys.Preview.MarkupParser._defaultNamespaceURI = 'http://schemas.microsoft.com/xml-script/2005';
Sys.Preview.MarkupParser._cachedNamespaceURILists = {};
Sys.Preview.MarkupParser.getNodeName = function Sys$Preview$MarkupParser$getNodeName(node) {
    return node.localName || node.baseName;
}
Sys.Preview.MarkupParser.initializeObject = function Sys$Preview$MarkupParser$initializeObject(instance, node, markupContext) {
    var td = Sys.Preview.TypeDescriptor.getTypeDescriptor(instance);
    if (!td) {
        return null;
    }
    var supportsBatchedUpdates = false;
    if ((instance.beginUpdate && instance.endUpdate && instance !== Sys.Application)) {
        supportsBatchedUpdates = true;
        instance.beginUpdate();
    }
    var i, a;
    var attr, attrName;
    var propertyInfo, propertyName, propertyType, propertyValue;
    var eventInfo, eventValue;
    var setter, getter;
    var nodeName;
    var properties = td._properties;
    var events = td._events;
    var attributes = node.attributes;
    if (attributes) {
        for (a = attributes.length - 1; a >= 0; a--) {
            attr = attributes[a];
            attrName = attr.nodeName;
            
                        if(attrName === "id" && Sys.UI.Control.isInstanceOfType(instance)) continue;
            propertyInfo = properties[attrName];
            if (propertyInfo) {
                propertyType = propertyInfo.type;
                propertyValue = attr.nodeValue;
                if (propertyType && (propertyType === Object || propertyType === Sys.Component || propertyType.inheritsFrom(Sys.Component))) {
                    markupContext.addReference(instance, propertyInfo, propertyValue);
                }
                else {
                    if (propertyInfo.isDomElement) {
                        propertyValue = markupContext.findElement(propertyValue);
                    }
                    else {
                        if (propertyType === Array) {
                            propertyValue = Array.parse('[' + propertyValue + ']');
                        }
                                                else if (propertyType && propertyType !== String) {
                                                        if(Type.isEnum(propertyType)) {
                                propertyValue = propertyType.parse(propertyValue, true);
                            }
                            else {
                                if(propertyValue === "" && propertyType === Number) {
                                    propertyValue = 0;
                                }
                                else {
                                    propertyValue =
                                        (propertyType.parseInvariant || propertyType.parse)(propertyValue);
                                }
                            }
                        }
                    }
                    propertyName = propertyInfo.name;
                    setter = instance['set_' + propertyName];
                    setter.call(instance, propertyValue);
                }
            }
            else {
                eventInfo = events[attrName];
                if (eventInfo) {
                    var handler = Function.parse(attr.nodeValue);
                                        if (handler) {
                        eventValue = instance['add_' + eventInfo.name];
                        if (eventValue) {
                            eventValue.apply(instance, [handler]);
                        }
                    }
                }
            }
        }
    }
        var childNodes = node.childNodes;
    if (childNodes && (childNodes.length != 0)) {
        for (i = childNodes.length - 1; i >= 0; i--) {
            var childNode = childNodes[i];
            if (childNode.nodeType != 1) {
                continue;
            }
            
            nodeName = Sys.Preview.MarkupParser.getNodeName(childNode);
            propertyInfo = properties[nodeName];
            if (propertyInfo) {
                propertyName = propertyInfo.name;
                propertyType = propertyInfo.type;
                if (propertyInfo.readOnly) {
                    getter = instance['get_' + propertyName];
                    var nestedObject = getter.call(instance);
                    if (propertyType === Array) {
                        if (childNode.childNodes.length) {
                            var items = Sys.Preview.MarkupParser.parseNodes(childNode.childNodes, markupContext);
                            for (var itemIndex = 0; itemIndex < items.length; itemIndex++) {
                                var item = items[itemIndex];
                                                                if(typeof(nestedObject.add) === "function") {
                                    nestedObject.add(item);
                                }
                                else {
                                    Array.add(nestedObject, item);
                                    if(typeof(item.setOwner) === "function") {
                                                                                item.setOwner(instance);
                                    }
                                }
                            }
                        }
                    }
                    else if (propertyType === Object) {
                                                attributes = childNode.attributes;
                        for (a = attributes.length - 1; a >= 0; a--) {
                            attr = attributes[a];
                            nestedObject[attr.nodeName] = attr.nodeValue;
                        }
                                                                                                                                                                    }
                    else {
                                                Sys.Preview.MarkupParser.initializeObject(nestedObject, childNode, markupContext);
                    }
                }
                else {
                    propertyValue = null;
                    if (propertyType == String) {
                        propertyValue = childNode.text;
                    }
                    else if (childNode.childNodes.length != 0) {
                        var valueNode;
                        for (var childNodeIndex = 0; childNodeIndex < childNode.childNodes.length; childNodeIndex++) {
                            if (childNode.childNodes[childNodeIndex].nodeType != 1) {
                                continue;
                            }
                            valueNode = childNode.childNodes[childNodeIndex];
                            break;
                        }
                        if (valueNode) {
                            propertyValue = Sys.Preview.MarkupParser.parseNode(valueNode, markupContext);
                        }
                    }
                    if (propertyValue) {
                        setter = instance['set_' + propertyName];
                        setter.call(instance, propertyValue);
                    }
                }
            }
            else {
                eventInfo = events[nodeName];
                if (eventInfo) {
                    var actions = Sys.Preview.MarkupParser.parseNodes(childNode.childNodes, markupContext);
                    if (actions.length) {
                        eventValue = instance["add_" + eventInfo.name];
                        if(eventValue) {
                            for (var e = 0; e < actions.length; e++) {
                                var action = actions[e];
                                                                                                action.set_eventName(eventInfo.name);
                                action.set_eventSource(instance);
                            }
                        }
                    }
                }
                else {
                                        var type = null;
                    var upperName = nodeName.toUpperCase();
                    if(upperName === 'BINDINGS') {
                        type = Sys.Preview.BindingBase;
                    }
                    else if(upperName === 'BEHAVIORS') {
                        type = Sys.UI.Behavior;
                    }
                    if(type) {
                        if (childNode.childNodes.length) {
                            var items = Sys.Preview.MarkupParser.parseNodes(childNode.childNodes, markupContext);
                            for (var itemIndex = 0; itemIndex < items.length; itemIndex++) {
                                var item = items[itemIndex];
                                                                
                                if(typeof(item.setOwner) === "function") {
                                    item.setOwner(instance);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    if (supportsBatchedUpdates) {
        markupContext.addEndUpdate(instance);
    }
    return instance;
}
Sys.Preview.MarkupParser.parseNode = function Sys$Preview$MarkupParser$parseNode(node, markupContext) {
    var parsedObject = null;
    var tagType = Sys.Preview.MarkupParser._getTagType(node);
    if (tagType) {
        var parseMethod = tagType.parseFromMarkup;
        if (!parseMethod) {
            var baseType = tagType.getBaseType();
            while (baseType) {
                parseMethod = baseType.parseFromMarkup;
                if (parseMethod) {
                    break;
                }
                baseType = baseType.getBaseType();
            }
            tagType.parseFromMarkup = parseMethod;
        }
        if (parseMethod) {
            parsedObject = parseMethod.call(null, tagType, node, markupContext);
        }
    }
    return parsedObject;
}
Sys.Preview.MarkupParser.parseNodes = function Sys$Preview$MarkupParser$parseNodes(nodes, markupContext) {
    var objects = [];
    for (var i = 0; i < nodes.length; i++) {
        var objectNode = nodes[i];
        if (objectNode.nodeType !== 1) {
                        continue;
        }
        var processedObject = Sys.Preview.MarkupParser.parseNode(objectNode, markupContext);
        if (processedObject) {
            Array.add(objects, processedObject);
        }
    }
    return objects;
}
Sys.Preview.MarkupParser.processDocument = function Sys$Preview$MarkupParser$processDocument(markupContext) {
    
    var scripts = [];
    var scriptElements = document.getElementsByTagName('script');
    for (var e = 0; e < scriptElements.length; e++) {
        if (scriptElements[e].type == 'text/xml-script') {
            var scriptElement = scriptElements[e];
            var scriptMarkup = scriptElement.innerHTML;
            if (scriptMarkup.startsWith('<!--')) {
                var startIndex = scriptMarkup.indexOf('<', 1);
                var endIndex = scriptMarkup.lastIndexOf('>');
                endIndex = scriptMarkup.lastIndexOf('>', endIndex - 1);
                scriptMarkup = scriptMarkup.substring(startIndex, endIndex + 1);
            }
            if (scriptMarkup.length == 0) {
                continue;
            }
            var scriptDOM;
            if (Sys.Net.XMLDOM) {
                scriptDOM = new Sys.Net.XMLDOM(scriptMarkup);
            }
            else {
                                scriptDOM = new XMLDOM(scriptMarkup);
            }
            var scriptDocumentNode = null;
            var pageElements = scriptDOM.getElementsByTagName("page");
            if (pageElements.length) {
                scriptDocumentNode = pageElements[0];
            }
            
            if (scriptDocumentNode && Sys.Preview.MarkupParser.getNodeName(scriptDocumentNode) === "page") {
                Array.add(scripts, scriptDocumentNode);
            }
        }
    }
    Sys.Preview.MarkupParser.processDocumentScripts(markupContext, scripts);
}
Sys.Preview.MarkupParser.processDocumentScripts = function Sys$Preview$MarkupParser$processDocumentScripts(markupContext, scripts) {
    markupContext.open();
    for (var s = 0; s < scripts.length; s++) {
        var componentNodes = [];
        var scriptDocumentNode = scripts[s];
        var scriptDocumentItemNodes = scriptDocumentNode.childNodes;
        for (var i = scriptDocumentItemNodes.length - 1; i >= 0; i--) {
            var node = scriptDocumentItemNodes[i];
            if (node.nodeType !== 1) {
                continue;
            }
            var nodeName = Sys.Preview.MarkupParser.getNodeName(node);
            if (nodeName) nodeName = nodeName.toLowerCase();
            if (nodeName === 'components') {
                for (var c = 0; c < node.childNodes.length; c++) {
                    var componentNode = node.childNodes[c];
                    if (componentNode.nodeType !== 1) {
                        continue;
                    }
                    Array.add(componentNodes, componentNode);
                }
            }
        } 
        if (componentNodes.length) {
            Sys.Preview.MarkupParser.parseNodes(componentNodes, markupContext);
        }
    } 
    markupContext.close();
}
Sys.Preview.MarkupParser._getDefaultNamespaces = function Sys$Preview$MarkupParser$_getDefaultNamespaces() {
    if(!Sys.Preview.MarkupParser._defaultNamespaces) {
        var list = [ Sys, Sys.UI, Sys.Net, Sys.Preview, Sys.Preview.UI,
                     Sys.Preview.Net, Sys.Preview.Data, Sys.Preview.UI.Data,
                     Sys.Preview.Services.Components ];
        if(Sys.Preview.UI.Effects) Array.add(list, Sys.Preview.UI.Effects);
        Sys.Preview.MarkupParser._defaultNamespaces = list;
    }
    return Sys.Preview.MarkupParser._defaultNamespaces;
}
Sys.Preview.MarkupParser._processNamespaceURI = function Sys$Preview$MarkupParser$_processNamespaceURI(namespaceURI) {
    if(!namespaceURI || namespaceURI === Sys.Preview.MarkupParser._defaultNamespaceURI) {
        return Sys.Preview.MarkupParser._getDefaultNamespaces();
    }
    
        var start = namespaceURI.slice(0, 12).toLowerCase();     if(start === "javascript:") {
        namespaceURI = namespaceURI.slice(11);
        if(!namespaceURI.length) {
            return [];
        }
    }
    var nspaceList = namespaceURI.split(',');
    list = [];
    for(var i=0; i < nspaceList.length; i++) {
        var nspaceName = nspaceList[i];
        if(nspaceName.startsWith(' ')) nspaceName = nspaceName.trimStart();
        if(nspaceName.endsWith(' ')) nspaceName = nspaceName.trimEnd();
        if(!nspaceName.length) continue;
        var nspace = null;
        try { nspace = eval(nspaceName) } catch(e) { }
        if(nspace) {
            Array.add(list, nspace);
        }
    }
    return list;
}
Sys.Preview.MarkupParser._getTagType = function Sys$Preview$MarkupParser$_getTagType(node) {
    
    var tagName = Sys.Preview.MarkupParser.getNodeName(node);
    var namespaceURI = node.namespaceURI || Sys.Preview.MarkupParser._defaultNamespaceURI;
    var nspaceList = Sys.Preview.MarkupParser._cachedNamespaceURILists[namespaceURI];
    if (typeof(nspaceList) === 'undefined') {
        nspaceList = Sys.Preview.MarkupParser._processNamespaceURI(namespaceURI);
        Sys.Preview.MarkupParser._cachedNamespaceURILists[namespaceURI] = nspaceList;
    }
    var upperTagName = tagName.toUpperCase();
    for(var i=0; i < nspaceList.length; i++) {
        var nspace = nspaceList[i];
        var type = Type.parse(tagName, nspace);
        if(typeof(type) === 'function') {
            return type;
        }
    }
        if(upperTagName === "APPLICATION") {
        return Sys._Application;
    }
        if(upperTagName === "WEBREQUESTMANAGER") {
        return Sys.Net._WebRequestManager;
    }
    
    return null;
}
Sys.Preview.MarkupContext = function Sys$Preview$MarkupContext(document, global, parentContext, dataContext) {
    /// <param name="document"></param>
    /// <param name="global" type="Boolean"></param>
    /// <param name="parentContext" type="Object" optional="true" mayBeNull="true"></param>
    /// <param name="dataContext" type="Object" optional="true" mayBeNull="true"></param>
    var e = Function._validateParams(arguments, [
        {name: "document"},
        {name: "global", type: Boolean},
        {name: "parentContext", type: Object, mayBeNull: true, optional: true},
        {name: "dataContext", type: Object, mayBeNull: true, optional: true}
    ]);
    if (e) throw e;
    this._document = document;
    this._global = global;
    this._parentContext = parentContext;
    this._dataContext = dataContext || null;
    this._objects = { };
    this._pendingReferences = [];
    this._pendingEndUpdates = [];
}
    function Sys$Preview$MarkupContext$get_dataContext() {
        /// <value type="Object"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        
        if (this._dataContextHidden) {
            return null;
        }
        return this._dataContext;
    }
    function Sys$Preview$MarkupContext$get_isGlobal() {
        /// <value type="Boolean"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._global;
    }
    function Sys$Preview$MarkupContext$addComponent(component, noRegisterWithApp) {
        /// <param name="component" type="Object"></param>
        /// <param name="noRegisterWithApp" type="Boolean" optional="true"></param>
        var e = Function._validateParams(arguments, [
            {name: "component", type: Object},
            {name: "noRegisterWithApp", type: Boolean, optional: true}
        ]);
        if (e) throw e;
        var id = component.get_id();
        if(id) {
            this._addComponentByID(id, component, noRegisterWithApp);
        }
    }
    function Sys$Preview$MarkupContext$removeComponent(component) {
        /// <param name="component" type="Object"></param>
        var e = Function._validateParams(arguments, [
            {name: "component", type: Object}
        ]);
        if (e) throw e;
        var id = component.get_id();
        if(id) {
            this._removeComponentByID(id);
        }
        if(this._global && Sys.Component.isInstanceOfType(component)) {
                        Sys.Application.removeComponent(object);
        }
    }
    function Sys$Preview$MarkupContext$findComponent(id, parent) {
        /// <param name="id" type="String"></param>
        /// <param name="parent" optional="true" mayBeNull="true"></param>
        /// <returns type="Object" mayBeNull="true"></returns>
        var e = Function._validateParams(arguments, [
            {name: "id", type: String},
            {name: "parent", mayBeNull: true, optional: true}
        ]);
        if (e) throw e;
        if(parent) {
            return Sys.Application.findComponent(id, parent);
        }
        else {
            var object = this._objects[id];
            if (!object) {
                parent = this._parentContext || Sys.Application;
                object = parent.findComponent(id);
            }
            return object;
        }
    }
    function Sys$Preview$MarkupContext$getComponents() {
        /// <returns type="Array"></returns>
        if (arguments.length !== 0) throw Error.parameterCount();
        var res = [];
        var objects = this._objects;
        for (var name in objects) {
            res[res.length] = objects[name];
        }
        return res;
    }
    function Sys$Preview$MarkupContext$_addComponentByID(id, object, noRegisterWithApp) {
        
        this._objects[id] = object;
        if(!noRegisterWithApp && this._global && Sys.Component.isInstanceOfType(object)) {
                        Sys.Application.addComponent(object);
        }
    }
    function Sys$Preview$MarkupContext$addEndUpdate(instance) {
        /// <param name="instance"></param>
        var e = Function._validateParams(arguments, [
            {name: "instance"}
        ]);
        if (e) throw e;
        
        Array.add(this._pendingEndUpdates, instance);
    }
    function Sys$Preview$MarkupContext$addReference(instance, propertyInfo, reference) {
        /// <param name="instance" type="Object"></param>
        /// <param name="propertyInfo" type="Object"></param>
        /// <param name="reference" type="String"></param>
        var e = Function._validateParams(arguments, [
            {name: "instance", type: Object},
            {name: "propertyInfo", type: Object},
            {name: "reference", type: String}
        ]);
        if (e) throw e;
        
        Array.add(this._pendingReferences, { o: instance, p: propertyInfo, r: reference });
    }
    function Sys$Preview$MarkupContext$close() {
        
        this._opened = false;
        this._dataContext = null;
                                        var i;
        for (i = 0; i < this._pendingReferences.length; i++) {
            var pendingReference = this._pendingReferences[i];
            var instance = pendingReference.o;
            var propertyInfo = pendingReference.p;
            var propertyValue = pendingReference.r;
            var object = this.findComponent(propertyValue);
            
            var setter = instance['set_' + propertyInfo.name];
            setter.call(instance, object);
        }
        this._pendingReferences = null;
        for (i = 0; i < this._pendingEndUpdates.length; i++) {
            this._pendingEndUpdates[i].endUpdate();
        }
        this._pendingEndUpdates = null;
    }
    function Sys$Preview$MarkupContext$dispose() {
        if (!this._global) {
                                                                        for (var o in this._objects) {
                if (Sys.IDisposable.isImplementedBy(this._objects[o])) {
                    this._objects[o].dispose();
                }
                this._objects[o] = null;
            }
        }
        this._document = null;
        this._parentContext = null;
        this._dataContext = null;
        this._objects = null;
        this._pendingReferences = null;
        this._pendingEndUpdates = null;
    }
    function Sys$Preview$MarkupContext$findElement(id) {
        /// <param name="id" type="String"></param>
        /// <returns></returns>
        var e = Function._validateParams(arguments, [
            {name: "id", type: String}
        ]);
        if (e) throw e;
        if (this._opened) {
            
            var element = Sys.UI.DomElement.getElementById(id, this._document);
            if (!element && this._parentContext) {
                element = Sys.UI.DomElement.getElementById(id, this._parentContext);
            }
            return element;
        }
        return null;
    }
    function Sys$Preview$MarkupContext$hideDataContext() {
        /// <returns type="Boolean"></returns>
        if (arguments.length !== 0) throw Error.parameterCount();
        
        if (!this._dataContextHidden) {
            this._dataContextHidden = true;
            return true;
        }
        return false;
    }
    function Sys$Preview$MarkupContext$open() {
        
        this._pendingReferences = [];
        this._pendingEndUpdates = [];
        this._opened = true;
    }
    function Sys$Preview$MarkupContext$restoreDataContext() {
        
        this._dataContextHidden = false;
    }
Sys.Preview.MarkupContext.prototype = {
    _dataContextHidden: false,
    _opened: false,
    get_dataContext: Sys$Preview$MarkupContext$get_dataContext,
    get_isGlobal: Sys$Preview$MarkupContext$get_isGlobal,
        addComponent: Sys$Preview$MarkupContext$addComponent,
    removeComponent: Sys$Preview$MarkupContext$removeComponent,
    findComponent: Sys$Preview$MarkupContext$findComponent,
    getComponents: Sys$Preview$MarkupContext$getComponents,
    
    _addComponentByID: Sys$Preview$MarkupContext$_addComponentByID,
    addEndUpdate: Sys$Preview$MarkupContext$addEndUpdate,
    addReference: Sys$Preview$MarkupContext$addReference,
    close: Sys$Preview$MarkupContext$close,
    dispose: Sys$Preview$MarkupContext$dispose,
    findElement: Sys$Preview$MarkupContext$findElement,
    hideDataContext: Sys$Preview$MarkupContext$hideDataContext,
    open: Sys$Preview$MarkupContext$open,
    restoreDataContext: Sys$Preview$MarkupContext$restoreDataContext
}
Sys.Preview.MarkupContext.registerClass('Sys.Preview.MarkupContext', null, Sys.IContainer);
Sys.Preview.MarkupContext.createGlobalContext = function Sys$Preview$MarkupContext$createGlobalContext() {
    /// <returns type="Sys.Preview.MarkupContext"></returns>
    if (arguments.length !== 0) throw Error.parameterCount();
    return new Sys.Preview.MarkupContext(document, true);
}
Sys.Preview.MarkupContext.createLocalContext = function Sys$Preview$MarkupContext$createLocalContext(documentFragment, parentContext, dataContext) {
    /// <param name="documentFragment" optional="false" mayBeNull="false"></param>
    /// <param name="parentContext" optional="false" mayBeNull="false"></param>
    /// <param name="dataContext" optional="true" mayBeNull="true"></param>
    /// <returns type="Sys.Preview.MarkupContext"></returns>
    var e = Function._validateParams(arguments, [
        {name: "documentFragment"},
        {name: "parentContext"},
        {name: "dataContext", mayBeNull: true, optional: true}
    ]);
    if (e) throw e;
    return new Sys.Preview.MarkupContext(documentFragment, false, parentContext, dataContext);
}
Sys.Component.descriptor = {
    properties: [ {name: 'dataContext', type: Object},
                  {name: 'id', type: String},
                  {name: 'isInitialized', type: Boolean, readOnly: true},
                  {name: 'isUpdating', type: Boolean, readOnly: true} ],
    events: [ {name: 'propertyChanged'} ]
}
Sys.UI.Control.descriptor = {
    properties: [ {name: 'element', type: Object, readOnly: true},
                  {name: 'role', type: String, readOnly: true},
                  {name: 'parent', type: Object},
                  {name: 'visible', type: Boolean},
                  {name: 'visibilityMode', type: Sys.UI.VisibilityMode} ],
    methods:    [ {name: 'addCssClass', parameters: [ {name: 'className', type: String} ] },
                  {name: 'removeCssClass', parameters: [ {name: 'className', type: String} ] },
                  {name: 'toggleCssClass', parameters: [ {name: 'className', type: String} ] } ]
}
Sys.UI.Behavior.descriptor = {
    properties: [ {name: 'name', type: String} ]
}
Sys.Component.parseFromMarkup = function Sys$Component$parseFromMarkup(type, node, markupContext) {
                        var newComponent = new type();
                                    var dataContextHidden = false;
    var dataContext = markupContext.get_dataContext();
    if (dataContext) {
        dataContextHidden = markupContext.hideDataContext();
    }
    var component = Sys.Preview.MarkupParser.initializeObject(newComponent, node, markupContext);
    if (component) {
        markupContext.addComponent(component);
        if (dataContext) {
            component.set_dataContext(dataContext);
        }
    }
    else {
        newComponent.dispose();
    }
        if (dataContextHidden) {
        markupContext.restoreDataContext();
    }
    return component;
}
Sys.UI.Control.parseFromMarkup = function Sys$UI$Control$parseFromMarkup(type, node, markupContext) {
    /// <param name="type" type="Type"></param>
    /// <param name="node"></param>
    /// <param name="markupContext" type="Sys.Preview.MarkupContext"></param>
    /// <returns type="Sys.UI.Control"></returns>
    var e = Function._validateParams(arguments, [
        {name: "type", type: Type},
        {name: "node"},
        {name: "markupContext", type: Sys.Preview.MarkupContext}
    ]);
    if (e) throw e;
    var idAttribute = node.attributes.getNamedItem('id');
    
    var id = idAttribute.nodeValue;
    var associatedElement = markupContext.findElement(id);
    
    var dataContextHidden = false;
    var dataContext = markupContext.get_dataContext();
    if (dataContext) {
        dataContextHidden = markupContext.hideDataContext();
    }
                    var newControl = new type(associatedElement);
    var control = Sys.Preview.MarkupParser.initializeObject(newControl, node, markupContext);
    if (control) {
        
        markupContext.addComponent(control);
        if (dataContext) {
            control.set_dataContext(dataContext);
        }
    }
    else {
        newControl.dispose();
    }
    if (dataContextHidden) {
        markupContext.restoreDataContext();
    }
    return control;
}
Sys.UI.Behavior.parseFromMarkup = function Sys$UI$Behavior$parseFromMarkup(type, node, markupContext) {
    /// <param name="type" type="Type"></param>
    /// <param name="node"></param>
    /// <param name="markupContext" type="Sys.Preview.MarkupContext"></param>
    /// <returns type="Sys.UI.Behavior"></returns>
    var e = Function._validateParams(arguments, [
        {name: "type", type: Type},
        {name: "node"},
        {name: "markupContext", type: Sys.Preview.MarkupContext}
    ]);
    if (e) throw e;
    var associatedElement;
    var id;
    var elementAttribute = node.attributes.getNamedItem('elementID');
    if(!elementAttribute) {
                var parentNode = node.parentNode;
        if(parentNode) {             parentNode = parentNode.parentNode;             if(parentNode && parentNode.attributes) {
                                var idAttribute = parentNode.attributes.getNamedItem('id');
                if(idAttribute) {
                    id = idAttribute.nodeValue;
                    associatedElement = markupContext.findElement(id);
                }
            }
        }
        
    }
    else {
        
        if(elementAttribute.nodeValue.length) {
            id = elementAttribute.nodeValue;
            associatedElement = markupContext.findElement(id);
            
        }
                 node.attributes.removeNamedItem('elementID');
    }
    var newBehavior = new type(associatedElement);
    var behavior = Sys.Preview.MarkupParser.initializeObject(newBehavior, node, markupContext);
    if (behavior) {
                if (elementAttribute) {
            node.attributes.setNamedItem(elementAttribute);
        }
        markupContext.addComponent(behavior);
    }
    else {
        newBehavior.dispose();
    }
    return behavior;
}
function $object(id, context) {
    return Sys.Application.findComponent(id, context);
}
Sys._Application.descriptor = {
    events: [ {name: 'init'}, {name: 'load'}, {name: 'unload'} ]
}
Sys._Application.parseFromMarkup = function Sys$_Application$parseFromMarkup(type, node, markupContext) {
    /// <param name="type" type="Type"></param>
    /// <param name="node"></param>
    /// <param name="markupContext" type="Sys.Preview.MarkupContext"></param>
    /// <returns type="Sys._Application"></returns>
    var e = Function._validateParams(arguments, [
        {name: "type", type: Type},
        {name: "node"},
        {name: "markupContext", type: Sys.Preview.MarkupContext}
    ]);
    if (e) throw e;
    if (!markupContext.get_isGlobal()) {
        return null;
    }
    var id = null;
    var idAttribute = node.attributes.getNamedItem('id');
    if (idAttribute) {
        id = idAttribute.nodeValue;
        node.attributes.removeNamedItem('id');
    }
    Sys.Preview.MarkupParser.initializeObject(Sys.Application, node, markupContext);
    if (idAttribute) {
        node.attributes.setNamedItem(idAttribute);
    }
    if (id && (markupContext.findComponent(id) !== Sys.Application)) {
        markupContext._addComponentByID(id, Sys.Application, true);
    }
    return Sys.Application;
}
Sys.Application.getMarkupContext = function Sys$Application$getMarkupContext() {
    return this._markupContext;
}
Sys.Application.__initHandler = function Sys$Application$__initHandler() {
    var a = Sys.Application;
	a.remove_init(Sys.Application.__initHandler);
		Sys.Preview.MarkupParser.processDocument(a._markupContext);
}
Sys.Application.__unloadHandler = function Sys$Application$__unloadHandler() {
    var a = Sys.Application;
    a.remove_unload(a.__unloadHandler);
    if(a._markupContext) {
        a._markupContext.dispose();
        a._markupContext = null;
    }
}
if(!Sys.Application._markupContext) {
    Sys.Application._markupContext = Sys.Preview.MarkupContext.createGlobalContext();
    Sys.Application.add_init(Sys.Application.__initHandler);
    Sys.Application.add_unload(Sys.Application.__unloadHandler);
}
Sys.Preview.IAction = function Sys$Preview$IAction() {
    throw Error.notImplemented();
}
    function Sys$Preview$IAction$execute() {
        throw Error.notImplemented();
    }
    function Sys$Preview$IAction$setOwner() {
        throw Error.notImplemented();
    }
Sys.Preview.IAction.prototype = {
    
    execute: Sys$Preview$IAction$execute,
    
    setOwner: Sys$Preview$IAction$setOwner
}
Sys.Preview.IAction.registerInterface('Sys.Preview.IAction');
//////////////////////////////////////////////////////////////////////////////
Sys.Preview.Attributes = new function() {
    this.defineAttribute = function this$defineAttribute(attributeName) {
        this[attributeName] = attributeName;
    }
}
//////////////////////////////////////////////////////////////////////////////
Sys.Preview.TypeDescriptor = function Sys$Preview$TypeDescriptor() {
    this._properties = {};
    this._events = {};
    this._methods = {};
    this._attributes = {};
}
Sys.Preview.TypeDescriptor.registerClass('Sys.Preview.TypeDescriptor');
Sys.Preview.TypeDescriptor.prototype.addAttribute = function Sys$Preview$TypeDescriptor$addAttribute(attributeName, attributeValue) {
    /// <param name="attributeName" type="String"></param>
    /// <param name="attributeValue" type="String"></param>
    var e = Function._validateParams(arguments, [
        {name: "attributeName", type: String},
        {name: "attributeValue", type: String}
    ]);
    if (e) throw e;
    this._attributes[attributeName] = attributeValue;
}
Sys.Preview.TypeDescriptor.prototype.addEvent = function Sys$Preview$TypeDescriptor$addEvent(eventName) {
    /// <param name="eventName" type="String"></param>
    /// <returns type="Object"></returns>
    var e = Function._validateParams(arguments, [
        {name: "eventName", type: String}
    ]);
    if (e) throw e;
    return this._events[eventName] = { name: eventName };
}
Sys.Preview.TypeDescriptor.prototype.addMethod = function Sys$Preview$TypeDescriptor$addMethod(methodName, associatedParameters) {
    /// <param name="methodName" type="String"></param>
    /// <param name="associatedParameters" type="Array" optional="true" mayBeNull="true"></param>
    /// <returns type="Object"></returns>
    var e = Function._validateParams(arguments, [
        {name: "methodName", type: String},
        {name: "associatedParameters", type: Array, mayBeNull: true, optional: true}
    ]);
    if (e) throw e;
    return this._methods[methodName] = { name: methodName, parameters: associatedParameters };
}
Sys.Preview.TypeDescriptor.prototype.addProperty = function Sys$Preview$TypeDescriptor$addProperty(propertyName, propertyType, readOnly, isDomElement, associatedAttributes) {
    /// <param name="propertyName" type="String"></param>
    /// <param name="propertyType" type="Type" mayBeNull="true"></param>
    /// <param name="readOnly" type="Boolean" optional="true"></param>
    /// <param name="isDomElement" type="Boolean" optional="true"></param>
    /// <param name="associatedAttributes" parameterArray="true" optional="true" mayBeNull="true"></param>
    /// <returns type="Object"></returns>
    var e = Function._validateParams(arguments, [
        {name: "propertyName", type: String},
        {name: "propertyType", type: Type, mayBeNull: true},
        {name: "readOnly", type: Boolean, optional: true},
        {name: "isDomElement", type: Boolean, optional: true},
        {name: "associatedAttributes", mayBeNull: true, optional: true, parameterArray: true}
    ]);
    if (e) throw e;
    if (propertyType === Sys.UI.DomElement) {
                throw Error.argumentType("propertyType", Sys.UI.DomElement, Object, "Use isDomElement with a null type for element properties.\ne.g., for descriptors use { name: 'foo', isDomElement: true, type: null }");
    }
    
    readOnly = !!readOnly;
    var attribs;
    if (associatedAttributes) {
        attribs = { };
        for (var i = 4; i < arguments.length; i += 2) {
            var attribute = arguments[i];
            var value = arguments[i + 1];
            attribs[attribute] = value;
        }
    }
    return this._properties[propertyName] = { name: propertyName, type: propertyType, 'readOnly': readOnly, 'isDomElement': isDomElement, attributes: attribs };
}
Sys.Preview.TypeDescriptor.createParameter = function Sys$Preview$TypeDescriptor$createParameter(parameterName, parameterType, isDomElement, isInteger) {
    /// <param name="parameterName" type="String"></param>
    /// <param name="parameterType" type="Type" mayBeNull="true"></param>
    /// <param name="isDomElement" type="Boolean" optional="true"></param>
    /// <param name="isInteger" type="Boolean" optional="true"></param>
    /// <returns type="Object"></returns>
    var e = Function._validateParams(arguments, [
        {name: "parameterName", type: String},
        {name: "parameterType", type: Type, mayBeNull: true},
        {name: "isDomElement", type: Boolean, optional: true},
        {name: "isInteger", type: Boolean, optional: true}
    ]);
    if (e) throw e;
    return { name: parameterName, type: parameterType, 'isDomElement': !!isDomElement, 'isInteger': !!isInteger };
}
Sys.Preview.TypeDescriptor.getTypeDescriptor = function Sys$Preview$TypeDescriptor$getTypeDescriptor(instance) {
    /// <param name="instance" type="Object"></param>
    /// <returns type="Sys.Preview.TypeDescriptor"></returns>
    var e = Function._validateParams(arguments, [
        {name: "instance", type: Object}
    ]);
    if (e) throw e;
    var type = Object.getType(instance);
    var td = type._descriptor;
    if (!td && !type._descriptorChecked) {
        if (Sys.Preview.ITypeDescriptorProvider.isImplementedBy(instance)) {
            td = instance.getDescriptor();
        }
        else {
                        td = Sys.Preview.TypeDescriptor.generateDescriptor(type);
        }
        type._descriptor = td;
        type._descriptorChecked = true;
    }
    return td;
}
Sys.Preview.TypeDescriptor.generateBaseDescriptor = function Sys$Preview$TypeDescriptor$generateBaseDescriptor(instance) {
    /// <param name="instance" type="Object"></param>
    /// <returns type="Sys.Preview.TypeDescriptor"></returns>
    var e = Function._validateParams(arguments, [
        {name: "instance", type: Object}
    ]);
    if (e) throw e;
    var baseType = instance.getBaseType();
    return Sys.Preview.TypeDescriptor.generateDescriptor(baseType);
}
Sys.Preview.TypeDescriptor.generateDescriptor = function Sys$Preview$TypeDescriptor$generateDescriptor(type) {
    /// <param name="type" type="Sys.Type"></param>
    /// <returns type="Sys.Preview.TypeDescriptor"></returns>
    var e = Function._validateParams(arguments, [
        {name: "type", type: Sys.Type}
    ]);
    if (e) throw e;
    var td = null;
    var current = type;
    while(current) {
        if(current.descriptor) {
            if(!td) td = new Sys.Preview.TypeDescriptor();
            Sys.Preview.TypeDescriptor.append(td, current.descriptor);
        }
        current = current.getBaseType();
    }
    return td;
}
Sys.Preview.TypeDescriptor.append = function Sys$Preview$TypeDescriptor$append(td, descriptor) {
    /// <param name="td" type="Sys.Preview.TypeDescriptor"></param>
    /// <param name="descriptor" type="Object"></param>
    var e = Function._validateParams(arguments, [
        {name: "td", type: Sys.Preview.TypeDescriptor},
        {name: "descriptor", type: Object}
    ]);
    if (e) throw e;
    if (descriptor.properties) {
        var length = descriptor.properties.length;
        for (var i = 0; i < length; i++) {
            var property = descriptor.properties[i];
            var propertyName = property.name;
            var associatedAttributes = property.attributes;
            var readOnly = !!(property.readOnly);
            var isDomElement = !!(property.isDomElement);
            var isInteger = !!(property.isInteger);
            if (! td._properties[propertyName]) {
                                var args = [propertyName, property.type, readOnly, isDomElement];
                if(typeof(associatedAttributes) === 'array') {
                    for(var j=0, l = associatedAttributes.length; j < l; j++) {
                        var attrib = associatedAttributes[j];
                        args[args.length] = attrib.name;
                        args[args.length] = attrib.value;
                    }
                }
                var propInfo = td.addProperty.apply(td, args);
                                propInfo.isInteger = isInteger;
            }
        }
    }
    if (descriptor.events) {
        var length = descriptor.events.length;
        for (var i = 0; i < length; i++) {
            var eventName = descriptor.events[i].name
            if (! td._events[eventName]) {
                td.addEvent(eventName);
            }
        }
    }
    if (descriptor.methods) {
        var length = descriptor.methods.length;
        for (var i = 0; i < length; i++) {
            var methodName = descriptor.methods[i].name;
            if (! td._methods[methodName]) {
                                var params = descriptor.methods[i].params;
                if(!params) params = descriptor.methods[i].parameters;
                if (params) {
                    td.addMethod(methodName, params);
                }
                else {
                    td.addMethod(methodName);
                }
            }
        }
    }
    if (descriptor.attributes) {
        var length = descriptor.attributes.length;
        for (var i = 0; i < length; i++) {
            var attributeName = descriptor.attributes[i].name
            if (! td._attributes[attributeName]) {
                td.addAttribute(attributeName, descriptor.attributes[i].value);
            }
        }
    }
}
Sys.Preview.TypeDescriptor.unload = function Sys$Preview$TypeDescriptor$unload() {
}
Sys.Preview.TypeDescriptor.getAttribute = function Sys$Preview$TypeDescriptor$getAttribute(instance, attributeName) {
    /// <param name="instance" type="Object"></param>
    /// <param name="attributeName" type="String"></param>
    /// <returns type="Object"></returns>
    var e = Function._validateParams(arguments, [
        {name: "instance", type: Object},
        {name: "attributeName", type: String}
    ]);
    if (e) throw e;
    var td = Sys.Preview.TypeDescriptor.getTypeDescriptor(instance);
    return td._attributes[attributeName];
}
Sys.Preview.TypeDescriptor.getPropertyType = function Sys$Preview$TypeDescriptor$getPropertyType(instance, propertyName, key) {
    /// <param name="instance" type="Object"></param>
    /// <param name="propertyName" type="String"></param>
    /// <param name="key" optional="true" mayBeNull="true"></param>
    /// <returns type="Type"></returns>
    var e = Function._validateParams(arguments, [
        {name: "instance", type: Object},
        {name: "propertyName", type: String},
        {name: "key", mayBeNull: true, optional: true}
    ]);
    if (e) throw e;
    if (Sys.Preview.ICustomTypeDescriptor.isImplementedBy(instance)) {
        return Object;
    }
    if (key) {
        return Object;
    }
    var td = Sys.Preview.TypeDescriptor.getTypeDescriptor(instance);
    if(!td) return Object;
    var propertyInfo = td._properties[propertyName];
    return propertyInfo.type || null;
}
Sys.Preview.TypeDescriptor.invokeMethod = function Sys$Preview$TypeDescriptor$invokeMethod(instance, methodName, parameters) {
    /// <param name="instance" type="Object"></param>
    /// <param name="methodName" type="String"></param>
    /// <param name="parameters" type="Object" mayBeNull="true"></param>
    /// <returns type="Object"></returns>
    var e = Function._validateParams(arguments, [
        {name: "instance", type: Object},
        {name: "methodName", type: String},
        {name: "parameters", type: Object, mayBeNull: true}
    ]);
    if (e) throw e;
    if (Sys.Preview.ICustomTypeDescriptor.isImplementedBy(instance)) {
        return instance.invokeMethod(methodName, parameters);
    }
    var gtd = Sys.Preview.TypeDescriptor.getTypeDescriptor, td;
    if (gtd) {
        td = gtd(instance);
    }
    if (!td) {
                
        return instance[methodName].call(instance);
    }
    var methodInfo = td._methods[methodName];
    var method = instance[methodInfo.name];
                    if (!parameters || !methodInfo.parameters || !methodInfo.parameters.length) {
        return method.call(instance);
    }
    else {
        var arguments = [];
        for (var i = 0; i < methodInfo.parameters.length; i++) {
            var parameterInfo = methodInfo.parameters[i];
            var value = parameters[parameterInfo.name];
            if (typeof(value) === "undefined") {
                value = parameters[parameterInfo.name.toLowerCase()];
            }
                        value = Sys.Preview.TypeDescriptor._evaluateValue(parameterInfo.type, parameterInfo.isDomElement, parameterInfo.isInteger, value);
            arguments[i] = value;
        }
        return method.apply(instance, arguments);
    }
}
Sys.Preview.TypeDescriptor._evaluateValue = function Sys$Preview$TypeDescriptor$_evaluateValue(targetType, isDomElement, isInteger, value) {
                if(!targetType) {
        return value;
    }
    var valueType = typeof(value);
    if(valueType === "undefined" || value === null) {
        return value;
    }
    if(isDomElement) {
                if(valueType === "string") {
            value = Sys.UI.DomElement.getElementById(value);
        }
    }
    else if(targetType === Object || targetType === Sys.Component || targetType.inheritsFrom(Sys.Component)) {
                if(valueType === "string") {
            value = Sys.Application.findComponent(value);
        }
    }
    else {
        if(targetType !== String && valueType === "string") {
                        if(Type.isEnum(targetType)) {
                                value = targetType.parse(value, true);
            }
            else {
                if(value === "" && targetType === Number) {
                    value = 0;
                }
                else {
                    value = (targetType.parseInvariant || targetType.parse)(value);
                    if (targetType === Number && isInteger) {
                        value = Math.floor(value);
                    }
                }
            }
        }
        else if(targetType === String && valueType !== "string") {
                        value = value.toString();
        }
        else if(targetType === Number && isInteger) {
            value = Math.floor(value);
        }
    }
    return value;
}
Sys.Preview.TypeDescriptor.getProperty = function Sys$Preview$TypeDescriptor$getProperty(instance, propertyName, key) {
    /// <param name="instance" type="Object"></param>
    /// <param name="propertyName" type="String" mayBeNull="true"></param>
    /// <param name="key" optional="true" mayBeNull="true"></param>
    /// <returns type="Object"></returns>
    var e = Function._validateParams(arguments, [
        {name: "instance", type: Object},
        {name: "propertyName", type: String, mayBeNull: true},
        {name: "key", mayBeNull: true, optional: true}
    ]);
    if (e) throw e;
    if (Sys.Preview.ICustomTypeDescriptor.isImplementedBy(instance)) {
        return instance.getProperty(propertyName, key);
    }
    var gtd = Sys.Preview.TypeDescriptor.getTypeDescriptor, td;
    if (gtd) {
        td = gtd(instance);
    }
    if (!td) {
        field = Sys.Preview.TypeDescriptor._getValue(instance, propertyName);
        if (field && key) {
            field = key.indexOf('.') === -1 ? 
                Sys.Preview.TypeDescriptor._getValue(field, key) :
                Sys.Preview.TypeDescriptor._evaluatePath(field, key);
        }
        return field;
    }
    var propertyInfo = td._properties[propertyName];
    var object = Sys.Preview.TypeDescriptor._getValue(instance, propertyInfo.name, true);
    if (key) {
        object = key.indexOf('.') === -1 ?
            Sys.Preview.TypeDescriptor._getValue(object, key) :
            Sys.Preview.TypeDescriptor._evaluatePath(object, key);
    }
    return object;
}
Sys.Preview.TypeDescriptor._getValue = function Sys$Preview$TypeDescriptor$_getValue(obj, name, requireGetter) {
    if (typeof(obj["get_" + name]) === "function") {
        return obj["get_" + name]();
    }
    else if (!requireGetter) {
        return obj[name];
    }
    else {
        throw Error.invalidOperation(String.format('Get accessor was not found for "{0}" property on object of type "{1}".', name, Object.getTypeName(obj)));
    }
}
Sys.Preview.TypeDescriptor._setValue = function Sys$Preview$TypeDescriptor$_setValue(obj, name, value, requireSetter) {
    if (typeof (obj["set_" + name]) === "function") {
        obj["set_" + name](value);
    }
    else if (!requireSetter) {
        obj[name] = value;
    }
    else {
        throw Error.invalidOperation(String.format('Set accessor was not found for "{0}" property on object of type "{1}".', name, Object.getTypeName(obj)));
    }
}
Sys.Preview.TypeDescriptor.setProperty = function Sys$Preview$TypeDescriptor$setProperty(instance, propertyName, value, key) {
    /// <param name="instance" type="Object" mayBeNull="false"></param>
    /// <param name="propertyName" type="String" mayBeNull="true"></param>
    /// <param name="value" mayBeNull="true"></param>
    /// <param name="key" optional="true" mayBeNull="true"></param>
    var e = Function._validateParams(arguments, [
        {name: "instance", type: Object},
        {name: "propertyName", type: String, mayBeNull: true},
        {name: "value", mayBeNull: true},
        {name: "key", mayBeNull: true, optional: true}
    ]);
    if (e) throw e;
    if (Sys.Preview.ICustomTypeDescriptor.isImplementedBy(instance)) {
        instance.setProperty(propertyName, value, key);
        return;
    }
    var gtd = Sys.Preview.TypeDescriptor.getTypeDescriptor, td;
    if (gtd) {
        td = gtd(instance);
    }
    if (!td) {
                if(!key) {
            Sys.Preview.TypeDescriptor._setValue(instance, propertyName, value);
        }
        else {
            instance = Sys.Preview.TypeDescriptor._getValue(instance, propertyName);
            if(key.indexOf('.') === -1) {
                Sys.Preview.TypeDescriptor._setValue(instance, key, value)
            }
            else {
                Sys.Preview.TypeDescriptor._setPath(instance, key, value);
            }
        }
        return;
    }
    var propertyInfo = td._properties[propertyName];
    if (key) {
        var object = Sys.Preview.TypeDescriptor._getValue(instance, propertyInfo.name, true);
        if(key.indexOf('.') === -1) {
            Sys.Preview.TypeDescriptor._setValue(object, key, value);
        }
        else {
            Sys.Preview.TypeDescriptor._setPath(object, key, value);
        }
    }
    else {
                value = Sys.Preview.TypeDescriptor._evaluateValue(propertyInfo.type, propertyInfo.isDomElement, propertyInfo.isInteger, value);
        Sys.Preview.TypeDescriptor._setValue(instance, propertyInfo.name, value, true);
    }
}
Sys.Preview.TypeDescriptor._evaluatePath = function Sys$Preview$TypeDescriptor$_evaluatePath(instance, path) {
    var part;
    var parts = path.split('.');
    var current = instance;
    for (var i=0, l = parts.length; i < l; i++) {
        part = parts[i];
        current = Sys.Preview.TypeDescriptor._getValue(current, part);
        if(typeof(current) === 'undefined' || current === null) return null;
    }
    return current;
}
Sys.Preview.TypeDescriptor._setPath = function Sys$Preview$TypeDescriptor$_setPath(instance, path, value) {
    var current = instance;
    var parts = path.split('.');
    var part;
    for (var i=0; i < parts.length-1; i++) {
        part = parts[i];
        current = Sys.Preview.TypeDescriptor._getValue(current, part);
        if(!current) break;
    }
    if (current) {
        Sys.Preview.TypeDescriptor._setValue(current, parts[parts.length-1], value);
    }
}
if(Sys.Browser.agent === Sys.Browser.Safari) {
    Sys.Preview.TypeDescriptor.prototype._addEvent = Sys.Preview.TypeDescriptor.prototype.addEvent;
    Sys.Preview.TypeDescriptor.prototype._addProperty = Sys.Preview.TypeDescriptor.prototype.addProperty;
    Sys.Preview.TypeDescriptor.prototype.addEvent = function Sys$Preview$TypeDescriptor$addEvent(eventName) {
        this._addEvent(eventName);
        var lcEventName = eventName.toLowerCase();
        if (eventName != lcEventName) {
            this._addEvent(lcEventName);
            this._events[lcEventName].name = eventName;
        }
    }
    Sys.Preview.TypeDescriptor.prototype.addProperty = function Sys$Preview$TypeDescriptor$addProperty(propertyName, propertyType, readOnly, isDomElement) {
        var propInfo = this._addProperty.apply(this, arguments);
        var lcPropertyName = propertyName.toLowerCase();
        if (propertyName !== lcPropertyName) {
            var baseArguments = [];
            Array.add(baseArguments, lcPropertyName);
            for (var a = 1; a < arguments.length; a++) {
                Array.add(baseArguments, arguments[a]);
            }
            this._addProperty.apply(this, baseArguments);
            this._properties[lcPropertyName].name = propertyName;
        }
        return propInfo;
    }
}
Sys.Preview.ITypeDescriptorProvider = function Sys$Preview$ITypeDescriptorProvider() {
    throw Error.notImplemented();
}
    function Sys$Preview$ITypeDescriptorProvider$getDescriptor() {
        throw Error.notImplemented();
    }
Sys.Preview.ITypeDescriptorProvider.prototype = {
    getDescriptor: Sys$Preview$ITypeDescriptorProvider$getDescriptor
}
Sys.Preview.ITypeDescriptorProvider.registerInterface('Sys.Preview.ITypeDescriptorProvider');
Sys.Preview.ICustomTypeDescriptor = function Sys$Preview$ICustomTypeDescriptor() {
    throw Error.notImplemented();
}
    function Sys$Preview$ICustomTypeDescriptor$getProperty() {
        throw Error.notImplemented();
    }
    function Sys$Preview$ICustomTypeDescriptor$setProperty() {
        throw Error.notImplemented();
    }
    function Sys$Preview$ICustomTypeDescriptor$invokeMethod() {
        throw Error.notImplemented();
    }
Sys.Preview.ICustomTypeDescriptor.prototype = {
    getProperty: Sys$Preview$ICustomTypeDescriptor$getProperty,
    
    setProperty: Sys$Preview$ICustomTypeDescriptor$setProperty,
    
    invokeMethod: Sys$Preview$ICustomTypeDescriptor$invokeMethod
}
Sys.Preview.ICustomTypeDescriptor.registerInterface('Sys.Preview.ICustomTypeDescriptor');
Sys.Preview.CollectionChangedEventArgs = function Sys$Preview$CollectionChangedEventArgs(action, changedItem) {
    /// <param name="action" type="Sys.Preview.NotifyCollectionChangedAction"></param>
    /// <param name="changedItem" type="Object" mayBeNull="true"></param>
    var e = Function._validateParams(arguments, [
        {name: "action", type: Sys.Preview.NotifyCollectionChangedAction},
        {name: "changedItem", type: Object, mayBeNull: true}
    ]);
    if (e) throw e;
    Sys.Preview.CollectionChangedEventArgs.initializeBase(this);
    this._action = action;
    this._changedItem = changedItem;
}
    function Sys$Preview$CollectionChangedEventArgs$get_action() {
        /// <value type="Sys.Preview.NotifyCollectionChangedAction"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._action;
    }
    function Sys$Preview$CollectionChangedEventArgs$get_changedItem() {
        /// <value mayBeNull="true"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._changedItem;
    }
Sys.Preview.CollectionChangedEventArgs.prototype = {
    get_action: Sys$Preview$CollectionChangedEventArgs$get_action,
    get_changedItem: Sys$Preview$CollectionChangedEventArgs$get_changedItem
}
Sys.Preview.CollectionChangedEventArgs.registerClass('Sys.Preview.CollectionChangedEventArgs', Sys.EventArgs);
Sys.Preview.CollectionChangedEventArgs.descriptor = {
    properties: [   {name: 'action', type: Sys.Preview.NotifyCollectionChangedAction, readOnly: true},
                    {name: 'changedItem', type: Object, readOnly: true} ]
}
Sys.Preview.BindingDirection = function Sys$Preview$BindingDirection() {
    throw Error.invalidOperation();
}
Sys.Preview.BindingDirection.prototype = {
    In: 0,
    Out: 1,
    InOut: 2
}
Sys.Preview.BindingDirection.registerEnum('Sys.Preview.BindingDirection');
Sys.Preview.BindingEventArgs = function Sys$Preview$BindingEventArgs(value, direction, targetPropertyType, transformerArgument) {
    /// <param name="value" mayBeNull="true"></param>
    /// <param name="direction" type="Sys.Preview.BindingDirection"></param>
    /// <param name="targetPropertyType" type="Type" mayBeNull="true"></param>
    /// <param name="transformerArgument" mayBeNull="true"></param>
    var e = Function._validateParams(arguments, [
        {name: "value", mayBeNull: true},
        {name: "direction", type: Sys.Preview.BindingDirection},
        {name: "targetPropertyType", type: Type, mayBeNull: true},
        {name: "transformerArgument", mayBeNull: true}
    ]);
    if (e) throw e;
    Sys.Preview.BindingEventArgs.initializeBase(this);
    this._value = value;
    this._direction = direction;
    this._targetPropertyType = targetPropertyType;
    this._transformerArgument = transformerArgument;
}
    function Sys$Preview$BindingEventArgs$get_direction() {
        /// <value type="Sys.Preview.BindingDirection"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._direction;
    }
    function Sys$Preview$BindingEventArgs$get_targetPropertyType() {
        /// <value type="Type" mayBeNull="true"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._targetPropertyType;
    }
    function Sys$Preview$BindingEventArgs$get_transformerArgument() {
        /// <value mayBeNull="true"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._transformerArgument;
    }
    function Sys$Preview$BindingEventArgs$get_value() {
        /// <value mayBeNull="true"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._value;
    }
    function Sys$Preview$BindingEventArgs$set_value(value) {
        var e = Function._validateParams(arguments, [{name: "value", mayBeNull: true}]);
        if (e) throw e;
        this._value = value;
    }
Sys.Preview.BindingEventArgs.prototype = {
    get_direction: Sys$Preview$BindingEventArgs$get_direction,
    get_targetPropertyType: Sys$Preview$BindingEventArgs$get_targetPropertyType,
    get_transformerArgument: Sys$Preview$BindingEventArgs$get_transformerArgument,
    
    get_value: Sys$Preview$BindingEventArgs$get_value,
    set_value: Sys$Preview$BindingEventArgs$set_value
}
Sys.Preview.BindingEventArgs.descriptor = {
    properties: [   {name: 'direction', type: Sys.Preview.BindingDirection, readOnly: true},
                    {name: 'targetPropertyType', type: Type, readOnly: true},
                    {name: 'transformerArgument', readOnly: true},
                    {name: 'value'} ]
}
Sys.Preview.BindingEventArgs.registerClass('Sys.Preview.BindingEventArgs', Sys.CancelEventArgs);
Sys.Preview.BindingBase = function Sys$Preview$BindingBase(target) {
    /// <param name="target" optional="true" mayBeNull="true"></param>
    var e = Function._validateParams(arguments, [
        {name: "target", mayBeNull: true, optional: true}
    ]);
    if (e) throw e;
    Sys.Preview.BindingBase.initializeBase(this);
    if(target) {
        this._target = target;
    }
}
    function Sys$Preview$BindingBase$get_automatic() {
        /// <value type="Boolean"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._automatic;
    }
    function Sys$Preview$BindingBase$set_automatic(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: Boolean}]);
        if (e) throw e;
        if (!this._source) {
            this._automatic = value;
        }
    }
    function Sys$Preview$BindingBase$get_dataContext() {
        /// <value type="Object" mayBeNull="true"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._dataContext;
    }
    function Sys$Preview$BindingBase$set_dataContext(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: Object, mayBeNull: true}]);
        if (e) throw e;
        if (!this._source) {
            this._dataContext = value;
        }
    }
    function Sys$Preview$BindingBase$get_dataPath() {
        /// <value type="String" mayBeNull="true"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._dataPath;
    }
    function Sys$Preview$BindingBase$set_dataPath(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: String, mayBeNull: true}]);
        if (e) throw e;
        if (!this._source) {
            this._dataPath = value;
        }
    }
    function Sys$Preview$BindingBase$get_target() {
        /// <value type="Object" mayBeNull="true"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._target;
    }
    function Sys$Preview$BindingBase$set_target(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: Object, mayBeNull: true}]);
        if (e) throw e;
        
        this._target = value;
    }
    function Sys$Preview$BindingBase$get_property() {
        /// <value type="String" mayBeNull="true"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._property;
    }
    function Sys$Preview$BindingBase$set_property(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: String, mayBeNull: true}]);
        if (e) throw e;
        if (!this._source) {
            this._property = value;
        }
    }
    function Sys$Preview$BindingBase$get_propertyKey() {
        /// <value mayBeNull="true"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._propertyKey;
    }
    function Sys$Preview$BindingBase$set_propertyKey(value) {
        var e = Function._validateParams(arguments, [{name: "value", mayBeNull: true}]);
        if (e) throw e;
        if (!this._source) {
            this._propertyKey = value;
        }
    }
    function Sys$Preview$BindingBase$get_transformerArgument() {
        /// <value mayBeNull="true"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._transformerArgument;
    }
    function Sys$Preview$BindingBase$set_transformerArgument(value) {
        var e = Function._validateParams(arguments, [{name: "value", mayBeNull: true}]);
        if (e) throw e;
        this._transformerArgument = value;
    }
    function Sys$Preview$BindingBase$add_transform(handler) {
        var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
        if (e) throw e;
        this.get_events().addHandler("transform", handler);
    }
    function Sys$Preview$BindingBase$remove_transform(handler) {
        var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
        if (e) throw e;
        this.get_events().removeHandler("transform", handler);
    }
    function Sys$Preview$BindingBase$dispose() {
        this._dataContext = null;
        this._source = null;
        this._target = null;
        Sys.Preview.BindingBase.callBaseMethod(this, 'dispose');
    }
    function Sys$Preview$BindingBase$evaluate(direction) {
        /// <param name="direction" type="Number"></param>
        var e = Function._validateParams(arguments, [
            {name: "direction", type: Number}
        ]);
        if (e) throw e;
        
        if (this._bindingExecuting) {
            return;
        }
        this._bindingExecuting = true;
        if (direction === Sys.Preview.BindingDirection.In) {
            this.evaluateIn();
        }
        else {
            this.evaluateOut();
        }
        this._bindingExecuting = false;
    }
    function Sys$Preview$BindingBase$evaluateIn() {
        var targetPropertyType = Sys.Preview.TypeDescriptor.getPropertyType(this._target, this._property, this._propertyKey);
        var value = this._getSourceValue(targetPropertyType);
        var canceled = false;
        var handler = this.get_events().getHandler("transform");
        if (handler) {
            var be = new Sys.Preview.BindingEventArgs(value, Sys.Preview.BindingDirection.In, targetPropertyType, this._transformerArgument);
            handler(this, be);
            canceled = be.get_cancel();
            value = be.get_value();
        }
        if (!canceled) {
            Sys.Preview.TypeDescriptor.setProperty(this._target, this._property, value, this._propertyKey);
        }
    }
    function Sys$Preview$BindingBase$evaluateOut() {
    }
    function Sys$Preview$BindingBase$initialize() {
        Sys.Preview.BindingBase.callBaseMethod(this, 'initialize');
        
        this._source = this._dataContext;
        if (!this._source) {
                                    this._source = this._target.get_dataContext();
        }
        
        if (this._dataPath && this._dataPath.indexOf('.') > 0) {
            this._dataPathParts = this._dataPath.split('.');
        }
    }
    function Sys$Preview$BindingBase$_evaluateDataPath() {
        
        var object = this._source;
        for (var i = 0; i < this._dataPathParts.length - 1; i++) {
            object = Sys.Preview.TypeDescriptor.getProperty(object, this._dataPathParts[i]);
            if (!object) {
                return null;
            }
        }
        return object;
    }
    function Sys$Preview$BindingBase$_get_dataPathParts() {
        return this._dataPathParts;
    }
    function Sys$Preview$BindingBase$_getSource() {
        return this._source;
    }
    function Sys$Preview$BindingBase$_getSourceValue(targetPropertyType) {
                if (this._dataPath && this._dataPath.length) {
            var propertyObject = this._source;
            var propertyName = this._dataPath;
            if (this._dataPathParts) {
                propertyObject = this._evaluateDataPath();
                if (propertyObject === null) {
                    return null;
                }
                propertyName = this._dataPathParts[this._dataPathParts.length - 1];
            }
            return Sys.Preview.TypeDescriptor.getProperty(propertyObject, propertyName);
        }
        if (this._source && Sys.Preview.ICustomTypeDescriptor.isImplementedBy(this._source)) {
            return this._source.getProperty('');
        }
        return this._source;
    }
    function Sys$Preview$BindingBase$_getTargetValue(destinationType) {
                var value = Sys.Preview.TypeDescriptor.getProperty(this._target, this._property, this._propertyKey);
        var handler = this.get_events().getHandler('transform');
        if(handler) {
            var be = new Sys.Preview.BindingEventArgs(value, Sys.Preview.BindingDirection.Out, destinationType, this._transformerArgument);
            handler(this, be);
            var canceled = be.get_cancel();
            if (!canceled) {
                value = be.get_value();
            }
            else {
                value = null;
            }
        }
        return value;
    }
    function Sys$Preview$BindingBase$setOwner(owner) {
        /// <param name="owner"></param>
        var e = Function._validateParams(arguments, [
            {name: "owner"}
        ]);
        if (e) throw e;
                        this.set_target(owner);
    }
Sys.Preview.BindingBase.prototype = {
    _target: null,
    _property: null,
    _propertyKey: null,
    _dataContext: null,
    _dataPath: null,
    _dataPathParts: null,
    _transformerArgument: null,
    _automatic: true,
    _bindingExecuting: false,
    _source: null,
    get_automatic: Sys$Preview$BindingBase$get_automatic,
    set_automatic: Sys$Preview$BindingBase$set_automatic,
    get_dataContext: Sys$Preview$BindingBase$get_dataContext,
    set_dataContext: Sys$Preview$BindingBase$set_dataContext,
    get_dataPath: Sys$Preview$BindingBase$get_dataPath,
    set_dataPath: Sys$Preview$BindingBase$set_dataPath,
    get_target: Sys$Preview$BindingBase$get_target,
    set_target: Sys$Preview$BindingBase$set_target,
    get_property: Sys$Preview$BindingBase$get_property,
    set_property: Sys$Preview$BindingBase$set_property,
    get_propertyKey: Sys$Preview$BindingBase$get_propertyKey,
    set_propertyKey: Sys$Preview$BindingBase$set_propertyKey,
    get_transformerArgument: Sys$Preview$BindingBase$get_transformerArgument,
    set_transformerArgument: Sys$Preview$BindingBase$set_transformerArgument,
    add_transform: Sys$Preview$BindingBase$add_transform,
    remove_transform: Sys$Preview$BindingBase$remove_transform,
    dispose: Sys$Preview$BindingBase$dispose,
    evaluate: Sys$Preview$BindingBase$evaluate,
    evaluateIn: Sys$Preview$BindingBase$evaluateIn,
    evaluateOut: Sys$Preview$BindingBase$evaluateOut,
    initialize: Sys$Preview$BindingBase$initialize,
    _evaluateDataPath: Sys$Preview$BindingBase$_evaluateDataPath,
    _get_dataPathParts: Sys$Preview$BindingBase$_get_dataPathParts,
    _getSource: Sys$Preview$BindingBase$_getSource,
    _getSourceValue: Sys$Preview$BindingBase$_getSourceValue,
    _getTargetValue: Sys$Preview$BindingBase$_getTargetValue,
    setOwner: Sys$Preview$BindingBase$setOwner
}
Sys.Preview.BindingBase.descriptor = {
    properties: [   {name: 'target', type: Object},
                    {name: 'automatic', type: Boolean},
                    {name: 'dataContext', type: Object},
                    {name: 'dataPath', type: String},
                    {name: 'property', type: String},
                    {name: 'propertyKey' },
                    {name: 'transformerArgument', type: String} ],
    methods: [ {name: 'evaluateIn'} ],
    events: [ {name: 'transform'} ]
}
Sys.Preview.BindingBase.registerClass('Sys.Preview.BindingBase', Sys.Component, Sys.IDisposable);
Sys.Preview.BindingBase.parseFromMarkup = function Sys$Preview$BindingBase$parseFromMarkup(type, node, markupContext) {
    /// <param name="type" type="Type"></param>
    /// <param name="node"></param>
    /// <param name="markupContext" type="Sys.Preview.MarkupContext"></param>
    /// <returns type="Sys.Preview.BindingBase"></returns>
    var e = Function._validateParams(arguments, [
        {name: "type", type: Type},
        {name: "node"},
        {name: "markupContext", type: Sys.Preview.MarkupContext}
    ]);
    if (e) throw e;
    var newBinding = new type();
    var builtInTransform;
    var transformAttribute = node.attributes.getNamedItem('transform');
    if (transformAttribute) {
        var transformValue = transformAttribute.nodeValue;
        builtInTransform = Sys.Preview.BindingBase.Transformers[transformValue];
    }
    if (builtInTransform) {
        newBinding.add_transform(builtInTransform);
        node.attributes.removeNamedItem('transform');
    }
    var binding = Sys.Preview.MarkupParser.initializeObject(newBinding, node, markupContext);
    if (builtInTransform) {
        node.attributes.setNamedItem(transformAttribute)
    }
    if (binding) {
        markupContext.addComponent(binding);
        return binding;
    }
    else {
        newBinding.dispose();
    }
    return null;
}
Sys.Preview.BindingBase.Transformers = { };
Sys.Preview.BindingBase.Transformers.Invert = function Sys$Preview$BindingBase$Transformers$Invert(sender, eventArgs) {
    eventArgs.set_value(!eventArgs.get_value());
}
Sys.Preview.BindingBase.Transformers.ToString = function Sys$Preview$BindingBase$Transformers$ToString(sender, eventArgs) {
    
    var value = eventArgs.get_value();
    var newValue = '';
    var formatString = eventArgs.get_transformerArgument();
    var placeHolder = (formatString && (formatString.length !== 0)) ? formatString.indexOf('{0}') : -1;
    if (placeHolder != -1) {
        newValue = String.format(formatString, value);
    }
    else if (value) {
        newValue = value.toString();
    }
    else {
        newValue = formatString;
    }
    eventArgs.set_value(newValue);
}
Sys.Preview.BindingBase.Transformers.ToLocaleString = function Sys$Preview$BindingBase$Transformers$ToLocaleString(sender, eventArgs) {
    
    var value = eventArgs.get_value();
    var newValue = '';
    var formatString = eventArgs.get_transformerArgument();
    var placeHolder = (formatString && (formatString.length !== 0)) ? formatString.indexOf('{0}') : -1;
    if (placeHolder !== -1) {
        newValue = String.format(formatString, value.toLocalString ? value.toLocalString() : value.toString());
    }
    else if (value) {
        newValue = value.toLocaleString();
    }
    else {
        newValue = formatString;
    }
    eventArgs.set_value(newValue);
}
Sys.Preview.BindingBase.Transformers.Add = function Sys$Preview$BindingBase$Transformers$Add(sender, eventArgs) {
    var value = eventArgs.get_value();
    if (typeof(value) !== 'number') {
        if(value === "") {
            value = 0;
        }
        else {
            value = Number.parseInvariant(value);
        }
    }
    var delta = eventArgs.get_transformerArgument();
    if (!delta) {
        delta = 1;
    }
    if (typeof(delta) !== 'number') {
        if(value === "") {
            delta = 0;
        }
        else {
            delta = Number.parseInvariant(delta);
        }
    }
    if (eventArgs.get_direction() === Sys.Preview.BindingDirection.Out) {
        delta = -delta;
    }
    var newValue = value + delta;
    if (eventArgs.get_targetPropertyType() !== 'number') {
        newValue = newValue.toString();
    }
    eventArgs.set_value(newValue);
}
Sys.Preview.BindingBase.Transformers.Multiply = function Sys$Preview$BindingBase$Transformers$Multiply(sender, eventArgs) {
    var value = eventArgs.get_value();
    if (typeof(value) !== 'number') {
        if(value === "") {
            value = 0;
        }
        else {
            value = Number.parseInvariant(value);
        }
    }
    var factor = eventArgs.get_transformerArgument();
    if (!factor) {
        factor = 1;
    }
    if (typeof(factor) !== 'number') {
        if(factor === "") {
            factor = 0;
        }
        else {
            factor = Number.parseInvariant(factor);
        }
    }
    var newValue;
    if (eventArgs.get_direction() === Sys.Preview.BindingDirection.Out) {
        newValue = value / factor;
    }
    else {
        newValue = value * factor;
    }
    if (eventArgs.get_targetPropertyType() !== 'number') {
        newValue = newValue.toString();
    }
    eventArgs.set_value(newValue);
}
Sys.Preview.BindingBase.Transformers.Compare = function Sys$Preview$BindingBase$Transformers$Compare(sender, eventArgs) {
    
    var value = eventArgs.get_value();
    var compareValue = eventArgs.get_transformerArgument();
    if (compareValue === null) {
        value = value ? true : false;
    }
    else {
        value = (value === compareValue);
    }
    eventArgs.set_value(value);
}
Sys.Preview.BindingBase.Transformers.CompareInverted = function Sys$Preview$BindingBase$Transformers$CompareInverted(sender, eventArgs) {
    
    var value = eventArgs.get_value();
    var compareValue = eventArgs.get_transformerArgument();
    if (compareValue === null) {
        value = value ? false : true;
    }
    else {
        value = (value !== compareValue);
    }
    eventArgs.set_value(value);
}
Sys.Preview.BindingBase.Transformers.RSSTransform = function Sys$Preview$BindingBase$Transformers$RSSTransform(sender, eventArgs) {
    
    function getNodeValue(source, xPath) {
        var node = source.selectSingleNode(xPath);
        if (node) {
            return node.nodeValue;
        }
        return null;
    }
    var xmlNodes = eventArgs.get_value();
    if (!xmlNodes) {
        return;
    }
                    var dataItems = new Sys.Preview.Data.DataTable([
        new Sys.Preview.Data.DataColumn('title', String, null, false, true),
        new Sys.Preview.Data.DataColumn('description', String, null, false, true),
        new Sys.Preview.Data.DataColumn('link', String, null, false, true),
        new Sys.Preview.Data.DataColumn('author', String, null, false, true),
        new Sys.Preview.Data.DataColumn('category', String, null, false, true),
        new Sys.Preview.Data.DataColumn('comments', String, null, false, true),
        new Sys.Preview.Data.DataColumn('guid', String, null, true, true),
        new Sys.Preview.Data.DataColumn('pubDate', String, null, false, true),
        new Sys.Preview.Data.DataColumn('source', String, null, false, true)
    ]);
        for (var i = 0; i < xmlNodes.length; i++) {
        var xmlNode = xmlNodes[i];
        if (!xmlNode || (xmlNode.nodeType != 1)) {
            continue;
        }
        var dataItem = {
            title : getNodeValue(xmlNode, './title/text()'),
            description : getNodeValue(xmlNode, './description/text()'),
            link : getNodeValue(xmlNode, './link/text()'),
            author : getNodeValue(xmlNode, './author/text()'),
            category : getNodeValue(xmlNode, './category/text()'),
            comments : getNodeValue(xmlNode, './comments/text()'),
            guid : getNodeValue(xmlNode, './guid/text()'),
            pubDate : getNodeValue(xmlNode, './pubDate/text()'),
            source : getNodeValue(xmlNode, './source/text()')
        };
        dataItems.add(dataItem);
    }
    eventArgs.set_value(dataItems);
}
Sys.Preview.Binding = function Sys$Preview$Binding(target) {
    /// <param name="target" optional="true" mayBeNull="true"></param>
    var e = Function._validateParams(arguments, [
        {name: "target", mayBeNull: true, optional: true}
    ]);
    if (e) throw e;
    Sys.Preview.Binding.initializeBase(this, [target]);
}
    function Sys$Preview$Binding$get_direction() {
        /// <value type="Sys.Preview.BindingDirection"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._direction;
    }
    function Sys$Preview$Binding$set_direction(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: Sys.Preview.BindingDirection}]);
        if (e) throw e;
        if (!this._getSource()) {
            this._direction = value;
        }
    }
    function Sys$Preview$Binding$dispose() {
        var target = this.get_target();
        var source = this._getSource();
        if (this._targetNotificationHandler) {
            target.remove_propertyChanged(this._targetNotificationHandler);
            this._targetNotificationHandler = null;
        }
        if (this._sourceNotificationHandler) {
            source.remove_propertyChanged(this._sourceNotificationHandler);
            this._sourceNotificationHandler = null;
        }
        if (this._targetDisposingHandler) {
            target.remove_disposing(this._targetDisposingHandler);
            this._targetDisposingHandler = null;
        }
        if (this._sourceDisposingHandler) {
            source.remove_disposing(this._sourceDisposingHandler);
            this._sourceDisposingHandler = null;
        }
        Sys.Preview.Binding.callBaseMethod(this, 'dispose');
    }
    function Sys$Preview$Binding$evaluateOut() {
        var propertyObject;
        var propertyName;
        var dataPathParts = this._get_dataPathParts();
        if (dataPathParts) {
            propertyObject = this._evaluateDataPath();
            propertyName = dataPathParts[dataPathParts.length - 1];
            if (!propertyObject) {
                return;
            }
        }
        else {
            propertyObject = this._getSource();
            propertyName = this.get_dataPath();
        }
        
        var sourcePropertyType = Sys.Preview.TypeDescriptor.getPropertyType(propertyObject, propertyName);
        var value = this._getTargetValue(sourcePropertyType);
        if (value !== null) {
            Sys.Preview.TypeDescriptor.setProperty(propertyObject, propertyName, value);
        }
    }
    function Sys$Preview$Binding$initialize() {
        Sys.Preview.Binding.callBaseMethod(this, 'initialize');
        if (this.get_automatic()) {
            if (this._direction !== Sys.Preview.BindingDirection.In) {
                var target = this.get_target();
                if (Sys.INotifyPropertyChange.isImplementedBy(target)) {
                    this._targetNotificationHandler = Function.createDelegate(this, this._onTargetPropertyChanged);
                    target.add_propertyChanged(this._targetNotificationHandler);
                }
                if (Sys.INotifyDisposing.isImplementedBy(target)) {
                    this._targetDisposingHandler = Function.createDelegate(this, this._onDisposing);
                    target.add_disposing(this._targetDisposingHandler);
                }
            }
            if (this._direction !== Sys.Preview.BindingDirection.Out) {
                var source = this._getSource();
                if (Sys.INotifyPropertyChange.isImplementedBy(source)) {
                    this._sourceNotificationHandler = Function.createDelegate(this, this._onSourcePropertyChanged);
                    source.add_propertyChanged(this._sourceNotificationHandler);
                }
                if (Sys.INotifyDisposing.isImplementedBy(source)) {
                    this._sourceDisposingHandler = Function.createDelegate(this, this._onDisposing);
                    source.add_disposing(this._sourceDisposingHandler);
                }
                this.evaluate(Sys.Preview.BindingDirection.In);
            }
        }
    }
    function Sys$Preview$Binding$_onSourcePropertyChanged(sender, eventArgs) {
        /// <param name="sender" type="Object"></param>
        /// <param name="eventArgs" type="Sys.EventArgs"></param>
        var e = Function._validateParams(arguments, [
            {name: "sender", type: Object},
            {name: "eventArgs", type: Sys.EventArgs}
        ]);
        if (e) throw e;
        
        var compareProperty = this.get_dataPath();
        var dataPathParts = this._get_dataPathParts();
        if (dataPathParts) {
            compareProperty = dataPathParts[0];
        }
        var propertyName = eventArgs.get_propertyName();
        if (!propertyName || (propertyName === compareProperty)) {
            this.evaluate(Sys.Preview.BindingDirection.In);
        }
    }
    function Sys$Preview$Binding$_onTargetPropertyChanged(sender, eventArgs) {
        /// <param name="sender" type="Object"></param>
        /// <param name="eventArgs" type="Sys.EventArgs"></param>
        var e = Function._validateParams(arguments, [
            {name: "sender", type: Object},
            {name: "eventArgs", type: Sys.EventArgs}
        ]);
        if (e) throw e;
        
        var propertyName = eventArgs.get_propertyName();
        if (!propertyName || (propertyName === this.get_property())) {
            this.evaluate(Sys.Preview.BindingDirection.Out);
        }
    }
    function Sys$Preview$Binding$_onDisposing(sender, eventArgs) {
        this.dispose();
    }
Sys.Preview.Binding.prototype = {
    _targetNotificationHandler: null,
    _sourceNotificationHandler: null,
    _direction: Sys.Preview.BindingDirection.In,
    get_direction: Sys$Preview$Binding$get_direction,
    set_direction: Sys$Preview$Binding$set_direction,
    dispose: Sys$Preview$Binding$dispose,
    evaluateOut: Sys$Preview$Binding$evaluateOut,
    initialize: Sys$Preview$Binding$initialize,
    _onSourcePropertyChanged: Sys$Preview$Binding$_onSourcePropertyChanged,
    _onTargetPropertyChanged: Sys$Preview$Binding$_onTargetPropertyChanged,
    _onDisposing: Sys$Preview$Binding$_onDisposing
}
Sys.Preview.Binding.descriptor = {
    properties: [ {name: 'direction', type: Sys.Preview.BindingDirection} ],
    methods: [ {name: 'evaluateOut'} ]
}
Sys.Preview.Binding.registerClass('Sys.Preview.Binding', Sys.Preview.BindingBase);
Sys.Preview.XPathBinding = function Sys$Preview$XPathBinding() {
    Sys.Preview.XPathBinding.initializeBase(this);
}
    function Sys$Preview$XPathBinding$get_xpath() {
        /// <value type="String" mayBeNull="true"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._xpath;
    }
    function Sys$Preview$XPathBinding$set_xpath(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: String, mayBeNull: true}]);
        if (e) throw e;
        if (!this._getSource()) {
            this._xpath = value;
        }
    }
    function Sys$Preview$XPathBinding$initialize() {
        Sys.Preview.XPathBinding.callBaseMethod(this, 'initialize');
        if (this.get_automatic()) {
            this.evaluate(Sys.Preview.BindingDirection.In);
        }
    }
    function Sys$Preview$XPathBinding$_getSourceValue(targetPropertyType) {
        var source = Sys.Preview.XPathBinding.callBaseMethod(this, '_getSourceValue');
        if (!source) {
            return null;
        }
        if (Array.isInstanceOfType(targetPropertyType)) {
            var nodes = source.selectNodes(this._xpath);
                                                var list = [];
            for (var i = 0; i < nodes.length; i++) {
                var node = nodes[i];
                if (!node || (node.nodeType !== 1)) {
                    continue;
                }
                Array.add(list, node);
            }
            return list;
        }
        else {
            var node = source.selectSingleNode(this._xpath);
            if (node) {
                return node.nodeValue;
            }
            return null;
        }
    }
Sys.Preview.XPathBinding.prototype = {
    _xpath: null,
    get_xpath: Sys$Preview$XPathBinding$get_xpath,
    set_xpath: Sys$Preview$XPathBinding$set_xpath,
    initialize: Sys$Preview$XPathBinding$initialize,
    _getSourceValue: Sys$Preview$XPathBinding$_getSourceValue
}
Sys.Preview.XPathBinding.descriptor = {
    properties: [ {name: 'xpath', type: String} ]
}
Sys.Preview.XPathBinding.registerClass('Sys.Preview.XPathBinding', Sys.Preview.BindingBase);
Sys.Preview.Action = function Sys$Preview$Action() {
    Sys.Preview.Action.initializeBase(this);
}
    function Sys$Preview$Action$get_eventSource() {
        /// <value type="Object" mayBeNull="true"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._eventSource;
    }
    function Sys$Preview$Action$set_eventSource(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: Object, mayBeNull: true}]);
        if (e) throw e;
        
        if(!this.get_isInitialized()) {
            this._eventSource = value;
        }
    }
    function Sys$Preview$Action$get_eventName() {
        /// <value type="String" mayBeNull="true"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._eventName;
    }
    function Sys$Preview$Action$set_eventName(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: String, mayBeNull: true}]);
        if (e) throw e;
        
        if(!this.get_isInitialized()) {
            this._eventName = value;
        }
    }
    function Sys$Preview$Action$get_target() {
        /// <value type="Object" mayBeNull="true"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._target;
    }
    function Sys$Preview$Action$set_target(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: Object, mayBeNull: true}]);
        if (e) throw e;
        this._target = value;
    }
    function Sys$Preview$Action$get_dataContext() {
        /// <value type="Sys.Preview.Action"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this;
    }
    function Sys$Preview$Action$get_eventArgs() {
        /// <value type="Sys.EventArgs"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._eventArgs;
    }
    function Sys$Preview$Action$get_result() {
        /// <value type="Object" mayBeNull="true"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._result;
    }
    function Sys$Preview$Action$get_sender() {
        /// <value type="Object"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._eventSource;
    }
    function Sys$Preview$Action$get_bindings() {
        /// <value type="Array" elementType="Sys.Preview.Binding"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        if(!this._bindings) {
            this._bindings = Sys.Component.createCollection(this);
            this._bindings.add_collectionChanged(Function.createDelegate(this, this._bindingChanged));
        }
        return this._bindings;
    }
    function Sys$Preview$Action$_bindingChanged(sender, args) {
        if(args.get_action() === Sys.Preview.NotifyCollectionChangedAction.Add) {
            args.get_changedItem().set_automatic(false);
        }
    }
    function Sys$Preview$Action$dispose() {
                if(this._sourceHandler) {
            this._eventSource["remove_" + this._eventName](this._sourceHandler);
            this._sourceHandler = null;
        }
                if(this._sourceDisposingHandler) {
            this._eventSource.remove_disposing(this._sourceDisposingHandler);
            this._sourceDisposingHandler = null;
        }
                if(this._targetDisposingHandler) {
            this._target.remove_disposing(this._targetDisposingHandler);
            this._targetDisposingHandler = null;
        }
        
        this._target = null;
        this._eventSource = null;
        Sys.Preview.Action.callBaseMethod(this, 'dispose');
            }
    function Sys$Preview$Action$performAction() {
        throw Error.notImplemented();
    }
    function Sys$Preview$Action$execute(sender, eventArgs) {
        /// <param name="sender" type="Object"></param>
        /// <param name="eventArgs" type="Sys.EventArgs"></param>
        var e = Function._validateParams(arguments, [
            {name: "sender", type: Object},
            {name: "eventArgs", type: Sys.EventArgs}
        ]);
        if (e) throw e;
        this._eventArgs = eventArgs;
        
        var bindings = this.get_bindings();
        var binding;
        var bindingType;
        if(bindings) {
            var i;
            for (i = 0; i < bindings.length; i++) {
                binding = bindings[i];
                bindingType = binding ? Object.getType(binding) : null;
                if(bindingType && (bindingType === Sys.Preview.Binding || Sys.Preview.Binding.inheritsFrom(bindingType))) {
                    if(binding.get_direction() !== Sys.Preview.BindingDirection.Out) {
                                                binding.evaluateIn();
                    }
                }
                else {
                    binding.evaluateIn();
                }
            }
        }
        
        this._result = this.performAction();
        if(bindings) {
            for (i = 0; i < bindings.length; i++) {
                binding = bindings[i];
                bindingType = binding ? Object.getType(binding) : null;
                if(bindingType && (bindingType === Sys.Preview.Binding || Sys.Preview.Binding.inheritsFrom(bindingType))) {
                    if(binding.get_direction() !== Sys.Preview.BindingDirection.In) {
                                                binding.evaluateOut();
                    }
                }
                else {
                    binding.evaluateOut();
                }
            }
        }
        
        this._eventArgs = null;
        this._result = null;
    }
    function Sys$Preview$Action$initialize() {
        if(this._eventSource) {
                                                                        var td = Sys.Preview.TypeDescriptor.getTypeDescriptor(this._eventSource);
            if(td) {
                if(Sys.INotifyDisposing.isImplementedBy(this._eventSource)) {
                    this._sourceDisposeHandler = Function.createDelegate(this, this._sourceDisposing);
                    this._eventSource.add_disposing(this._sourceDisposeHandler);
                }
            
                
                var eventInfo = td._events[this.get_eventName()];
                
                
                this._sourceHandler = Function.createDelegate(this, this.execute);
                this._eventName = eventInfo.name;
                this._eventSource["add_" + this._eventName](this._sourceHandler);
            }
        }
        
        if(this._target && Sys.INotifyDisposing.isImplementedBy(this._target)) {
            this._targetDisposeHandler = Function.createDelegate(this, this._targetDisposing);
            this._target.add_disposing(this._targetDisposeHandler);
        }
        
        Sys.Preview.Action.callBaseMethod(this, 'initialize');
    }
    function Sys$Preview$Action$setOwner(eventSource) {
        /// <param name="eventSource" type="Object"></param>
        var e = Function._validateParams(arguments, [
            {name: "eventSource", type: Object}
        ]);
        if (e) throw e;
        
        if(!this.get_isInitialized()) {
            this._eventSource = eventSource;
        }
                                                            }
    function Sys$Preview$Action$_sourceDisposing() {
        this.dispose();
    }
    function Sys$Preview$Action$_targetDisposing() {
        this.dispose();
    }
Sys.Preview.Action.prototype = {
    _eventSource: null,
    _eventName: null,
    _eventArgs: null,
    _result: null,
    _target: null,
    _bindings: null,
    
    get_eventSource: Sys$Preview$Action$get_eventSource,
    set_eventSource: Sys$Preview$Action$set_eventSource,
    
    get_eventName: Sys$Preview$Action$get_eventName,
    set_eventName: Sys$Preview$Action$set_eventName,
    
    get_target: Sys$Preview$Action$get_target,
    set_target: Sys$Preview$Action$set_target,
    
    get_dataContext: Sys$Preview$Action$get_dataContext,
    
    get_eventArgs: Sys$Preview$Action$get_eventArgs,
    
    get_result: Sys$Preview$Action$get_result,
    get_sender: Sys$Preview$Action$get_sender,
    
    get_bindings: Sys$Preview$Action$get_bindings,
    
    _bindingChanged: Sys$Preview$Action$_bindingChanged,
    dispose: Sys$Preview$Action$dispose,
    
    performAction: Sys$Preview$Action$performAction,
    
    execute: Sys$Preview$Action$execute,
    
    initialize: Sys$Preview$Action$initialize,
    
    setOwner: Sys$Preview$Action$setOwner,
    
    _sourceDisposing: Sys$Preview$Action$_sourceDisposing,
    _targetDisposing: Sys$Preview$Action$_targetDisposing
}
Sys.Preview.Action.descriptor = {
    properties: [   {name: 'eventSource', type: Object},
                    {name: 'eventName', type: String},
                    {name: 'bindings', type: Array, readOnly: true},
                    {name: 'eventArgs', type: Sys.EventArgs, readOnly: true},
                    {name: 'result', type: Object, readOnly: true},
                    {name: 'sender', type: Object, readOnly: true},
                    {name: 'target', type: Object} ]
}
Sys.Preview.Action.registerClass('Sys.Preview.Action', Sys.Component, Sys.Preview.IAction);
Sys.Preview.Action.parseFromMarkup = function Sys$Preview$Action$parseFromMarkup(type, node, markupContext) {
    /// <param name="type" type="Type"></param>
    /// <param name="node"></param>
    /// <param name="markupContext" type="Sys.Preview.MarkupContext"></param>
    /// <returns type="Sys.Preview.Action"></returns>
    var e = Function._validateParams(arguments, [
        {name: "type", type: Type},
        {name: "node"},
        {name: "markupContext", type: Sys.Preview.MarkupContext}
    ]);
    if (e) throw e;
    var newAction = new type();
    
    var action = Sys.Preview.MarkupParser.initializeObject(newAction, node, markupContext);
    if (action) {
        markupContext.addComponent(action);
        return action;
    }
    else {
        newAction.dispose();
    }
    return null;
}
Sys.Preview.InvokeMethodAction = function Sys$Preview$InvokeMethodAction() {
    Sys.Preview.InvokeMethodAction.initializeBase(this);
}
    function Sys$Preview$InvokeMethodAction$get_method() {
        /// <value type="String"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._method;
    }
    function Sys$Preview$InvokeMethodAction$set_method(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: String}]);
        if (e) throw e;
        this._method = value;
    }
    function Sys$Preview$InvokeMethodAction$get_parameters() {
        /// <value type="Object"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        if (!this._parameters) {
            this._parameters = { };
        }
        return this._parameters;
    }
    function Sys$Preview$InvokeMethodAction$performAction() {
        /// <returns type="Function"></returns>
        if (arguments.length !== 0) throw Error.parameterCount();
        return Sys.Preview.TypeDescriptor.invokeMethod(this.get_target(), this._method, this._parameters);
    }
Sys.Preview.InvokeMethodAction.prototype = {
    _method: null,
    _parameters: null,
    
    get_method: Sys$Preview$InvokeMethodAction$get_method,
    set_method: Sys$Preview$InvokeMethodAction$set_method,
    
    get_parameters: Sys$Preview$InvokeMethodAction$get_parameters,
    
    performAction: Sys$Preview$InvokeMethodAction$performAction
}
Sys.Preview.InvokeMethodAction.descriptor = {
    properties: [ {name: 'method', type: String},
                  {name: 'parameters', type: Object, readOnly: true} ]
}
Sys.Preview.InvokeMethodAction.registerClass('Sys.Preview.InvokeMethodAction', Sys.Preview.Action);
Sys.Preview.SetPropertyAction = function Sys$Preview$SetPropertyAction() {
    Sys.Preview.SetPropertyAction.initializeBase(this);
}
    function Sys$Preview$SetPropertyAction$get_property() {
        /// <value type="String" mayBeNull="true"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._property;
    }
    function Sys$Preview$SetPropertyAction$set_property(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: String, mayBeNull: true}]);
        if (e) throw e;
        this._property = value;
    }
    function Sys$Preview$SetPropertyAction$get_propertyKey() {
        /// <value mayBeNull="true"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._propertyKey;
    }
    function Sys$Preview$SetPropertyAction$set_propertyKey(value) {
        var e = Function._validateParams(arguments, [{name: "value", mayBeNull: true}]);
        if (e) throw e;
        this._propertyKey = value;
    }
    function Sys$Preview$SetPropertyAction$get_value() {
        /// <value mayBeNull="true"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._value;
    }
    function Sys$Preview$SetPropertyAction$set_value(value) {
        var e = Function._validateParams(arguments, [{name: "value", mayBeNull: true}]);
        if (e) throw e;
        this._value = value;
    }
    function Sys$Preview$SetPropertyAction$performAction() {
        /// <returns type="Object" mayBeNull="true"></returns>
        if (arguments.length !== 0) throw Error.parameterCount();
        Sys.Preview.TypeDescriptor.setProperty(this.get_target(), this._property, this._value, this._propertyKey);
        return null;
    }
Sys.Preview.SetPropertyAction.prototype = {
    _property: null,
    _propertyKey: null,
    _value: null,
    
    get_property: Sys$Preview$SetPropertyAction$get_property,
    set_property: Sys$Preview$SetPropertyAction$set_property,
    
    get_propertyKey: Sys$Preview$SetPropertyAction$get_propertyKey,
    set_propertyKey: Sys$Preview$SetPropertyAction$set_propertyKey,
    
    get_value: Sys$Preview$SetPropertyAction$get_value,
    set_value: Sys$Preview$SetPropertyAction$set_value,
    
    performAction: Sys$Preview$SetPropertyAction$performAction
}
Sys.Preview.SetPropertyAction.descriptor = {
    properties: [   {name: 'property', type: String},
                    {name: 'propertyKey' },
                    {name: 'value', type: String} ]
}
Sys.Preview.SetPropertyAction.registerClass('Sys.Preview.SetPropertyAction', Sys.Preview.Action);
Sys.Preview.PostBackAction = function Sys$Preview$PostBackAction() {
    Sys.Preview.PostBackAction.initializeBase(this);
}
    function Sys$Preview$PostBackAction$get_target() {
        /// <value type="String" mayBeNull="true"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._target;
    }
    function Sys$Preview$PostBackAction$set_target(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: String, mayBeNull: true}]);
        if (e) throw e;
        this._target = value;
    }
    function Sys$Preview$PostBackAction$get_eventArgument() {
        /// <value type="String" mayBeNull="true"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._eventArgument;
    }
    function Sys$Preview$PostBackAction$set_eventArgument(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: String, mayBeNull: true}]);
        if (e) throw e;
        this._eventArgument = value;
    }
    function Sys$Preview$PostBackAction$performAction() {
        /// <returns type="Object"></returns>
        if (arguments.length !== 0) throw Error.parameterCount();
        __doPostBack(this.get_target(), this.get_eventArgument());
        return null;
    }
Sys.Preview.PostBackAction.prototype = {
    _eventArgument: null,
    
        get_target: Sys$Preview$PostBackAction$get_target,
    set_target: Sys$Preview$PostBackAction$set_target,
        
    get_eventArgument: Sys$Preview$PostBackAction$get_eventArgument,
    set_eventArgument: Sys$Preview$PostBackAction$set_eventArgument,
    
    performAction: Sys$Preview$PostBackAction$performAction
}
Sys.Preview.PostBackAction.descriptor = {
    properties: [   {name: 'eventArgument', type: String},
                    {name: 'target', type: String} ]
}
Sys.Preview.PostBackAction.registerClass('Sys.Preview.PostBackAction', Sys.Preview.Action);
///////////////////////////////////////////////////////////////////////////////
Sys.Preview.Counter = function Sys$Preview$Counter() {
    Sys.Preview.Counter.initializeBase(this);
}
    function Sys$Preview$Counter$get_canDecrement() {
        /// <value type="Boolean"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return isNaN(this._lowerBound) || (this._value > this._lowerBound);
    }
    function Sys$Preview$Counter$get_canIncrement() {
        /// <value type="Boolean"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return isNaN(this._upperBound) || (this._value < this._upperBound);
    }
    function Sys$Preview$Counter$get_lowerBound() {
        /// <value type="Number"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._lowerBound;
    }
    function Sys$Preview$Counter$set_lowerBound(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: Number}]);
        if (e) throw e;
        if ((isNaN(value) && isNaN(this._lowerBound)) || (value === this._lowerBound)) return;
        var oldCanDecrement = this.get_canDecrement();
        this._lowerBound = value;
        this.raisePropertyChanged('lowerBound');
        if (oldCanDecrement !== this.get_canDecrement()) {
            this.raisePropertyChanged('canDecrement');
        }
    }
    function Sys$Preview$Counter$get_upperBound() {
        /// <value type="Number"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._upperBound;
    }
    function Sys$Preview$Counter$set_upperBound(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: Number}]);
        if (e) throw e;
        if ((isNaN(value) && isNaN(this._upperBound)) || (value === this._upperBound)) return;
        var oldCanIncrement = this.get_canIncrement();
        this._upperBound = value;
        this.raisePropertyChanged('upperBound');
        if (oldCanIncrement !== this.get_canIncrement()) {
            this.raisePropertyChanged('canIncrement');
        }
    }
    function Sys$Preview$Counter$get_value() {
        /// <value type="Number"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._value;
    }
    function Sys$Preview$Counter$set_value(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: Number}]);
        if (e) throw e;
        if ((isNaN(this._lowerBound) || (value >= this._lowerBound)) &&
            (isNaN(this._upperBound) || (value <= this._upperBound)) && (this._value !== value)) {
            var oldCanDecrement = this.get_canDecrement();
            var oldCanIncrement = this.get_canIncrement();
            this._value = value;
            this.raisePropertyChanged('value');
            if (oldCanDecrement !== this.get_canDecrement()) {
                this.raisePropertyChanged('canDecrement');
            }
            if (oldCanIncrement !== this.get_canIncrement()) {
                this.raisePropertyChanged('canIncrement');
            }
        }
    }
    function Sys$Preview$Counter$decrement() {
        this.set_value(this._value - 1);
    }
    function Sys$Preview$Counter$increment() {
        this.set_value(this._value + 1);
    }
Sys.Preview.Counter.prototype = {
    _value: 0,
    _lowerBound: Number.NaN,
    _upperBound: Number.NaN,
    
    get_canDecrement: Sys$Preview$Counter$get_canDecrement,
    
    get_canIncrement: Sys$Preview$Counter$get_canIncrement,
    
    get_lowerBound: Sys$Preview$Counter$get_lowerBound,
    set_lowerBound: Sys$Preview$Counter$set_lowerBound,
    
    get_upperBound: Sys$Preview$Counter$get_upperBound,
    set_upperBound: Sys$Preview$Counter$set_upperBound,
    
    get_value: Sys$Preview$Counter$get_value,
    set_value: Sys$Preview$Counter$set_value,
    
    decrement: Sys$Preview$Counter$decrement,
    
    increment: Sys$Preview$Counter$increment
}
Sys.Preview.Counter.descriptor = {
    properties: [   {name: 'value', type: Number},
                    {name: 'lowerBound', type: Number},
                    {name: 'upperBound', type: Number},
                    {name: 'canDecrement', type: Boolean, readOnly: true},
                    {name: 'canIncrement', type: Boolean, readOnly: true} ],
    methods: [ {name: 'increment'}, {name: 'decrement'} ]
}
Sys.Preview.Counter.registerClass('Sys.Preview.Counter', Sys.Component);
///////////////////////////////////////////////////////////////////////////////
Sys.Preview.Timer = function Sys$Preview$Timer() {
    Sys.Preview.Timer.initializeBase(this);
    
    this._interval = 1000;
    this._enabled = false;
    this._timer = null;
}
    function Sys$Preview$Timer$get_interval() {
        /// <value type="Number"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._interval;
    }
    function Sys$Preview$Timer$set_interval(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: Number}]);
        if (e) throw e;
        if (this._interval !== value) {
            this._interval = value;
            this.raisePropertyChanged('interval');
            
            if (!this.get_isUpdating() && (this._timer !== null)) {
                this.restartTimer();
            }
        }
    }
    function Sys$Preview$Timer$get_enabled() {
        /// <value type="Boolean"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._enabled;
    }
    function Sys$Preview$Timer$set_enabled(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: Boolean}]);
        if (e) throw e;
        if (value !== this.get_enabled()) {
            this._enabled = value;
            this.raisePropertyChanged('enabled');
            if (!this.get_isUpdating()) {
                if (value) {
                    this._startTimer();
                }
                else {
                    this._stopTimer();
                }
            }
        }
    }
    function Sys$Preview$Timer$add_tick(handler) {
        var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
        if (e) throw e;
        this.get_events().addHandler("tick", handler);
    }
    function Sys$Preview$Timer$remove_tick(handler) {
        var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
        if (e) throw e;
        this.get_events().removeHandler("tick", handler);
    }
    function Sys$Preview$Timer$dispose() {
        this.set_enabled(false);
        this._stopTimer();
        
        Sys.Preview.Timer.callBaseMethod(this, 'dispose');
    }
    function Sys$Preview$Timer$updated() {
        Sys.Preview.Timer.callBaseMethod(this, 'updated');
        if (this._enabled) {
            this.restartTimer();
        }
    }
    function Sys$Preview$Timer$_timerCallback() {
                            var handler = this.get_events().getHandler("tick");
        if (handler) {
            handler(this, Sys.EventArgs.Empty);
        }
    }
    function Sys$Preview$Timer$restartTimer() {
        this._stopTimer();
        this._startTimer();
    }
    function Sys$Preview$Timer$_startTimer() {
        this._timer = window.setInterval(Function.createDelegate(this, this._timerCallback), this._interval);
    }
    function Sys$Preview$Timer$_stopTimer() {
        window.clearInterval(this._timer);
        this._timer = null;
    }
Sys.Preview.Timer.prototype = {
    get_interval: Sys$Preview$Timer$get_interval,
    set_interval: Sys$Preview$Timer$set_interval,
    
    get_enabled: Sys$Preview$Timer$get_enabled,
    set_enabled: Sys$Preview$Timer$set_enabled,
        add_tick: Sys$Preview$Timer$add_tick,
    remove_tick: Sys$Preview$Timer$remove_tick,
    dispose: Sys$Preview$Timer$dispose,
    
    updated: Sys$Preview$Timer$updated,
    _timerCallback: Sys$Preview$Timer$_timerCallback,
    
    restartTimer: Sys$Preview$Timer$restartTimer,
    _startTimer: Sys$Preview$Timer$_startTimer,
    _stopTimer: Sys$Preview$Timer$_stopTimer
}
Sys.Preview.Timer.descriptor = {
    properties: [   {name: 'interval', type: Number},
                    {name: 'enabled', type: Boolean} ],
    events: [ {name: 'tick'} ]
}
Sys.Preview.Timer.registerClass('Sys.Preview.Timer', Sys.Component);
Sys.Preview.Reference = function Sys$Preview$Reference() {
}
    function Sys$Preview$Reference$get_component() {
        /// <value type="Sys.Component" mayBeNull="true"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._component;
    }
    function Sys$Preview$Reference$set_component(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: Sys.Component, mayBeNull: true}]);
        if (e) throw e;
        this._component = value;
    }
    function Sys$Preview$Reference$get_onscriptload() {
        /// <value mayBeNull="true"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._onload;
    }
    function Sys$Preview$Reference$set_onscriptload(value) {
        var e = Function._validateParams(arguments, [{name: "value", mayBeNull: true}]);
        if (e) throw e;
        this._onload = value;
    }
    function Sys$Preview$Reference$dispose() {
        this._component = null;
    }
Sys.Preview.Reference.prototype = {
    _component: null,
    _onload: null,
    
    get_component: Sys$Preview$Reference$get_component,
    set_component: Sys$Preview$Reference$set_component,
    
    get_onscriptload: Sys$Preview$Reference$get_onscriptload,
    set_onscriptload: Sys$Preview$Reference$set_onscriptload,
    
    dispose: Sys$Preview$Reference$dispose
}
Sys.Preview.Reference.descriptor = {
    properties: [ { name: 'component', type: Object },
                  { name: 'onscriptload', type: String } ]
}
Sys.Preview.Reference.registerClass('Sys.Preview.Reference', null, Sys.IDisposable);
Sys.Preview.Reference.parseFromMarkup = function Sys$Preview$Reference$parseFromMarkup(type, node, markupContext) {
    var newReference = new Sys.Preview.Reference();
    var reference = Sys.Preview.MarkupParser.initializeObject(newReference, node, markupContext);
    if (reference) {
        return reference;
    }
    newReference.dispose();
    return null;
}
Sys.Preview.INotifyCollectionChanged = function Sys$Preview$INotifyCollectionChanged() {
    if (arguments.length !== 0) throw Error.parameterCount();
    throw Error.notImplemented();
}
    function Sys$Preview$INotifyCollectionChanged$add_collectionChanged() {
    var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
    if (e) throw e;
        throw Error.notImplemented();
    }
    function Sys$Preview$INotifyCollectionChanged$remove_collectionChanged() {
    var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
    if (e) throw e;
        throw Error.notImplemented();
    }
Sys.Preview.INotifyCollectionChanged.prototype = {
    add_collectionChanged: Sys$Preview$INotifyCollectionChanged$add_collectionChanged,
    remove_collectionChanged: Sys$Preview$INotifyCollectionChanged$remove_collectionChanged
}
Sys.Preview.INotifyCollectionChanged.registerInterface('Sys.Preview.INotifyCollectionChanged');
Sys.Preview.NotifyCollectionChangedAction = function Sys$Preview$NotifyCollectionChangedAction() {
    throw Error.invalidOperation();
}
Sys.Preview.NotifyCollectionChangedAction.prototype = {
    Add: 0,
    Remove: 1,
    Reset: 2
}
Sys.Preview.NotifyCollectionChangedAction.registerEnum('Sys.Preview.NotifyCollectionChangedAction');
Sys.Preview.ITask = function Sys$Preview$ITask() {
    throw Error.notImplemented();
}
    function Sys$Preview$ITask$execute() {
        throw Error.notImplemented();
    }
Sys.Preview.ITask.prototype = {
        execute: Sys$Preview$ITask$execute
}
Sys.Preview.ITask.registerInterface('Sys.Preview.ITask');
Sys.Preview._TaskManager = function Sys$Preview$_TaskManager() {
    Sys.Application.registerDisposableObject(this);
    this._tasks = [];
}
    function Sys$Preview$_TaskManager$addTask(task) {
        /// <param name="task" type="Sys.Preview.ITask"></param>
        var e = Function._validateParams(arguments, [
            {name: "task", type: Sys.Preview.ITask}
        ]);
        if (e) throw e;
        
        Array.enqueue(this._tasks, task);
        this._startTimeout();
    }
    function Sys$Preview$_TaskManager$dispose() {
        if (this._timeoutCookie) {
            window.clearTimeout(this._timeoutCookie);
        }
        if (this._tasks && this._tasks.length) {
            for (var i = this._tasks.length - 1; i >= 0; i--) {
                this._tasks[i].dispose();
            }
        }
        this._tasks = null;
        this._timeoutHandler = null;
        Sys.Application.unregisterDisposableObject(this);
    }
    function Sys$Preview$_TaskManager$_onTimeout() {
        this._timeoutCookie = 0;
                var task = Array.dequeue(this._tasks);
        if (!task.execute()) {
                        Array.enqueue(this._tasks, task);
        }
                if (this._tasks.length) {
            this._startTimeout();
        }
    }
    function Sys$Preview$_TaskManager$_startTimeout() {
        if (!this._timeoutCookie) {
            if (!this._timeoutHandler) {
                this._timeoutHandler = Function.createDelegate(this, this._onTimeout);
            }
            this._timeoutCookie = window.setTimeout(this._timeoutHandler,  0);
        }
    }
Sys.Preview._TaskManager.prototype = {
    _timeoutCookie: null,
    _timeoutHandler: null,
    addTask: Sys$Preview$_TaskManager$addTask,
    dispose: Sys$Preview$_TaskManager$dispose,
    _onTimeout: Sys$Preview$_TaskManager$_onTimeout,
    _startTimeout: Sys$Preview$_TaskManager$_startTimeout
}
Sys.Preview._TaskManager.registerClass('Sys.Preview._TaskManager', null, Sys.IDisposable);
Sys.Preview.TaskManager = new Sys.Preview._TaskManager();
Type.registerNamespace('Sys.Preview.Net');
Sys.Preview.Net.ServiceMethodRequest = function Sys$Preview$Net$ServiceMethodRequest() {
    Sys.Preview.Net.ServiceMethodRequest.initializeBase(this);
}
    function Sys$Preview$Net$ServiceMethodRequest$get_url() {
        /// <value type="String"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._url;
    }
    function Sys$Preview$Net$ServiceMethodRequest$set_url(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: String}]);
        if (e) throw e;
        this._url = value;
    }
    function Sys$Preview$Net$ServiceMethodRequest$get_methodName() {
        /// <value type="String"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._methodName;
    }
    function Sys$Preview$Net$ServiceMethodRequest$set_methodName(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: String}]);
        if (e) throw e;
        this._methodName = value;
    }
    function Sys$Preview$Net$ServiceMethodRequest$get_useGet() {
        /// <value type="Boolean"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._useGet;
    }
    function Sys$Preview$Net$ServiceMethodRequest$set_useGet(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: Boolean}]);
        if (e) throw e;
        this._useGet = value;
    }
    function Sys$Preview$Net$ServiceMethodRequest$get_parameters() {
        /// <value type="Object"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        if (this._parameters === null) {
            this._parameters = {};
        }
        return this._parameters;
    }
    function Sys$Preview$Net$ServiceMethodRequest$get_result() {
        /// <value type="Object" mayBeNull="true"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._result;
    }
    function Sys$Preview$Net$ServiceMethodRequest$get_timeoutInterval() {
        /// <value type="Number"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._timeoutInterval;
    }
    function Sys$Preview$Net$ServiceMethodRequest$set_timeoutInterval(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: Number}]);
        if (e) throw e;
        this._timeoutInterval = value;
    }
    function Sys$Preview$Net$ServiceMethodRequest$add_completed(handler) {
        var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
        if (e) throw e;
        this.get_events().addHandler("completed", handler);
    }
    function Sys$Preview$Net$ServiceMethodRequest$remove_completed(handler) {
        var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
        if (e) throw e;
        this.get_events().removeHandler("completed", handler);
    }
    function Sys$Preview$Net$ServiceMethodRequest$add_timeout(handler) {
        var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
        if (e) throw e;
        this.get_events().addHandler("timeout", handler);
    }
    function Sys$Preview$Net$ServiceMethodRequest$remove_timeout(handler) {
        var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
        if (e) throw e;
        this.get_events().removeHandler("timeout", handler);
    }
    function Sys$Preview$Net$ServiceMethodRequest$add_error(handler) {
        var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
        if (e) throw e;
        this.get_events().addHandler("error", handler);
    }
    function Sys$Preview$Net$ServiceMethodRequest$remove_error(handler) {
        var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
        if (e) throw e;
        this.get_events().removeHandler("error", handler);
    }
    function Sys$Preview$Net$ServiceMethodRequest$invoke(userContext) {
        /// <param name="userContext" mayBeNull="true" optional="true"></param>
        /// <returns type="Boolean"></returns>
        var e = Function._validateParams(arguments, [
            {name: "userContext", mayBeNull: true, optional: true}
        ]);
        if (e) throw e;
        if (this._request !== null) {
            return false;
        }
                        var params = { parameters: this.get_parameters(), loadMethod: "" };
        this._request = Sys.Net.WebServiceProxy.invoke(this._url, this._methodName, this._useGet, params, onMethodComplete, onMethodError, this, this._timeoutInterval);
        function onMethodComplete(result, target, methodName) {
            target._request = null;
            target._userContext = userContext;
            target._result = result;
            var handler = target.get_events().getHandler("completed");
            if (handler) {
                handler(target, Sys.EventArgs.Empty);
            }
        }
        function onMethodError(result, target, methodName) {
            target._request = null;
            target._userContext = userContext;
                        target._result = result;
            var isTimeout = false;
            if (result.get_errorStatus) isTimeout = (result.get_errorStatus() === 2);
            else if (result.get_timedOut) isTimeout = result.get_timedOut();
            var handler;
            if (isTimeout) {
                handler = target.get_events().getHandler("timeout");
            }
            else {
                handler = target.get_events().getHandler("error");
            }
            if (handler) {
                handler(target, Sys.EventArgs.Empty);
            }
        }
        return true;
    }
Sys.Preview.Net.ServiceMethodRequest.prototype = {
    _url: null,
    _methodName: null,
    _parameters: null,
    _userContext: null,
    _result: null,
    _request: null,
    _timeoutInterval: 0,
    _useGet: true,
    get_url: Sys$Preview$Net$ServiceMethodRequest$get_url,
    set_url: Sys$Preview$Net$ServiceMethodRequest$set_url,
    get_methodName: Sys$Preview$Net$ServiceMethodRequest$get_methodName,
    set_methodName: Sys$Preview$Net$ServiceMethodRequest$set_methodName,
    get_useGet: Sys$Preview$Net$ServiceMethodRequest$get_useGet,
    set_useGet: Sys$Preview$Net$ServiceMethodRequest$set_useGet,
    get_parameters: Sys$Preview$Net$ServiceMethodRequest$get_parameters,
    get_result: Sys$Preview$Net$ServiceMethodRequest$get_result,
    get_timeoutInterval: Sys$Preview$Net$ServiceMethodRequest$get_timeoutInterval,
    set_timeoutInterval: Sys$Preview$Net$ServiceMethodRequest$set_timeoutInterval,
        add_completed: Sys$Preview$Net$ServiceMethodRequest$add_completed,
    remove_completed: Sys$Preview$Net$ServiceMethodRequest$remove_completed,
    add_timeout: Sys$Preview$Net$ServiceMethodRequest$add_timeout,
    remove_timeout: Sys$Preview$Net$ServiceMethodRequest$remove_timeout,
    add_error: Sys$Preview$Net$ServiceMethodRequest$add_error,
    remove_error: Sys$Preview$Net$ServiceMethodRequest$remove_error,
    invoke: Sys$Preview$Net$ServiceMethodRequest$invoke
}
Sys.Preview.Net.ServiceMethodRequest.registerClass('Sys.Preview.Net.ServiceMethodRequest', Sys.Component);
Sys.Preview.Net.ServiceMethodRequest.descriptor = {
    properties: [   {name: 'url', type: String},
                    {name: 'methodName', type: String},
                    {name: 'parameters', type: Object, readOnly: true},
                    {name: 'result', type: Object, readOnly: true},
                    {name: 'timeoutInterval', type: Number},
                    {name: 'useGet', type: Boolean} ],
    methods: [ {name: 'invoke', parameters: [ {name: "userContext" } ] } ],
    events: [   {name: 'completed'},
                {name: 'timeout'},
                {name: 'error'} ]
}
Sys.Preview.Net.BridgeMethod = function Sys$Preview$Net$BridgeMethod() {
    Sys.Preview.Net.BridgeMethod.initializeBase(this);
}
    function Sys$Preview$Net$BridgeMethod$add_dataAvailable(handler) {
        var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
        if (e) throw e;
        this.get_events().addHandler('dataAvailable', handler);
    }
    function Sys$Preview$Net$BridgeMethod$remove_dataAvailable(handler) {
        var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
        if (e) throw e;
        this.get_events().removeHandler('dataAvailable', handler);
    }
    function Sys$Preview$Net$BridgeMethod$get_data() {
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._data;
    }
    function Sys$Preview$Net$BridgeMethod$set_data(value) {
        this._data = value;
        this.raisePropertyChanged('data');
        this._set_isReady(true);
        var handler = this.get_events().getHandler('dataAvailable');
        if (handler) {
            handler(this, Sys.EventArgs.Empty);
        }
    }
    function Sys$Preview$Net$BridgeMethod$get_isReady() {
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._isReady;
    }
    function Sys$Preview$Net$BridgeMethod$_set_isReady(value) {
        if (this._isReady !== value) {
            this._isReady = value;
            this.raisePropertyChanged("isReady");
        }
    }
    function Sys$Preview$Net$BridgeMethod$get_parameters() {
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._parameters;
    }
    function Sys$Preview$Net$BridgeMethod$get_bridgeURL() {
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._bridgeURL;
    }
    function Sys$Preview$Net$BridgeMethod$set_bridgeURL(value) {
        this._bridgeURL = value;
        this.raisePropertyChanged('bridgeURL');
    }
    function Sys$Preview$Net$BridgeMethod$get_onError() {
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._onError;
    }
    function Sys$Preview$Net$BridgeMethod$set_onError(value) {
        this._onError = value;
        this.raisePropertyChanged('onError');
    }
    function Sys$Preview$Net$BridgeMethod$get_bridgeMethod() {
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._bridgeMethod;
    }
    function Sys$Preview$Net$BridgeMethod$set_bridgeMethod(value) {
        this._bridgeMethod = value;
        this.raisePropertyChanged('bridgeMethod');
    }
    function Sys$Preview$Net$BridgeMethod$dispose() {
        this._data = null;        
        Sys.Preview.Net.BridgeMethod.callBaseMethod(this, 'dispose');
    }
    function Sys$Preview$Net$BridgeMethod$_onRequestComplete(result, userContext, methodName) {
        if (userContext) {
            userContext.set_data(result);
        }
    }
    function Sys$Preview$Net$BridgeMethod$invoke() {
        this._set_isReady(false);
        var callMethodArgs = {"method" : this._bridgeMethod, "args" : this._parameters };
        Sys.Net.WebServiceProxy.invoke(this._bridgeURL, "__invokeBridge", false, callMethodArgs, this._onRequestComplete, (this._onError ? eval(this._onError) : null), this, 0);
    }
Sys.Preview.Net.BridgeMethod.prototype = {
    _data: null,
    _bridgeURL: "",
    _bridgeMethod: "",
    _isReady: false,
    _onError: "",
    _parameters: {},
    add_dataAvailable: Sys$Preview$Net$BridgeMethod$add_dataAvailable,
    remove_dataAvailable: Sys$Preview$Net$BridgeMethod$remove_dataAvailable,
    
    get_data: Sys$Preview$Net$BridgeMethod$get_data,
    set_data: Sys$Preview$Net$BridgeMethod$set_data,
    
    get_isReady: Sys$Preview$Net$BridgeMethod$get_isReady,
    _set_isReady: Sys$Preview$Net$BridgeMethod$_set_isReady,
    
    get_parameters: Sys$Preview$Net$BridgeMethod$get_parameters,
    
    get_bridgeURL: Sys$Preview$Net$BridgeMethod$get_bridgeURL,
    set_bridgeURL: Sys$Preview$Net$BridgeMethod$set_bridgeURL,
    
    get_onError: Sys$Preview$Net$BridgeMethod$get_onError,
    set_onError: Sys$Preview$Net$BridgeMethod$set_onError,
    get_bridgeMethod: Sys$Preview$Net$BridgeMethod$get_bridgeMethod,
    set_bridgeMethod: Sys$Preview$Net$BridgeMethod$set_bridgeMethod,
    
    dispose: Sys$Preview$Net$BridgeMethod$dispose,
    
    _onRequestComplete: Sys$Preview$Net$BridgeMethod$_onRequestComplete,
    
    invoke: Sys$Preview$Net$BridgeMethod$invoke
}
Sys.Preview.Net.BridgeMethod.descriptor = {
    properties: [
        { name: "data", type: Object },
        { name: "isReady", type: Boolean },
        { name: "bridgeURL", type: String },
        { name: "parameters", type: Object },
        { name: "bridgeMethod", type: String },
        { name: "onError", type: String }
    ],
    methods: [
        { name: "invoke" }
    ],
    events: [
        { name: "dataAvailable" }
    ]
}
Sys.Preview.Net.BridgeMethod.registerClass('Sys.Preview.Net.BridgeMethod', Sys.Component);
Type.registerNamespace('Sys.Preview.Services.Components');
Type.registerNamespace("Sys.Preview.Services.Components");
Sys.Preview.Services.Components.Profile = function Sys$Preview$Services$Components$Profile() {
    Sys.Preview.Services.Components.Profile.initializeBase(this);
}
    function Sys$Preview$Services$Components$Profile$get_autoSave() {
        /// <value type="Boolean"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._autoSave;
    }
    function Sys$Preview$Services$Components$Profile$set_autoSave(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: Boolean}]);
        if (e) throw e;
        this._autoSave = value;
    }
    function Sys$Preview$Services$Components$Profile$get_isDirty() {
        /// <value type="Boolean"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._isDirty;
    }
    function Sys$Preview$Services$Components$Profile$get_path() {
        /// <value type="String" mayBeNull="true"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return Sys.Services.ProfileService.get_path();
    }
    function Sys$Preview$Services$Components$Profile$set_path(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: String, mayBeNull: true}]);
        if (e) throw e;
        Sys.Services.ProfileService.set_path(value);
    }
    function Sys$Preview$Services$Components$Profile$add_loadComplete(handler) {
        var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
        if (e) throw e;
        this.get_events().addHandler('loadComplete', handler);
    }
    function Sys$Preview$Services$Components$Profile$remove_loadComplete(handler) {
        var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
        if (e) throw e;
        this.get_events().removeHandler('loadComplete', handler);
    }
    function Sys$Preview$Services$Components$Profile$add_saveComplete(handler) {
        var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
        if (e) throw e;
        this.get_events().addHandler('saveComplete', handler);
    }
    function Sys$Preview$Services$Components$Profile$remove_saveComplete(handler) {
        var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
        if (e) throw e;
        this.get_events().removeHandler('saveComplete', handler);
    }
    function Sys$Preview$Services$Components$Profile$getProperty(name, key) {
        /// <param name="name" type="String" optional="false" mayBeNull="false"></param>
        /// <param name="key" optional="true" mayBeNull="true"></param>
        /// <returns></returns>
        var e = Function._validateParams(arguments, [
            {name: "name", type: String},
            {name: "key", mayBeNull: true, optional: true}
        ]);
        if (e) throw e;
        var props = Sys.Services.ProfileService.properties;
        if(key) {
            var group = props[name];
            return group ? (group[key]||null) : null;
        }
        return props[name] || null;
    }
    function Sys$Preview$Services$Components$Profile$initialize() {
        Sys.Preview.Services.Components.Profile.callBaseMethod(this, 'initialize');
        
        var empty = true;
        for(var v in Sys.Services.ProfileService.properties) {
            empty = false;
            break;
        }
        if (empty) {
            this.load(); 
        }
    }
    function Sys$Preview$Services$Components$Profile$invokeMethod(methodName, parameters) {
        if(methodName === "save") {
            this.save.apply(this, parameters);
        }
        else if(methodName === "load") {
            this.load.apply(this, parameters);
        }
    }
    function Sys$Preview$Services$Components$Profile$load(propertyNames) {
        /// <param name="propertyNames" type="Array" elementType="String" mayBeNull="true" optional="true" elementMayBeNull="false"></param>
        var e = Function._validateParams(arguments, [
            {name: "propertyNames", type: Array, mayBeNull: true, optional: true, elementType: String}
        ]);
        if (e) throw e;
        if(!this.loadCallback) this.loadCallback = Function.createDelegate(this,this._loadComplete);
        Sys.Services.ProfileService.load(propertyNames, this.loadCallback);
    }
    function Sys$Preview$Services$Components$Profile$save(propertyNames) {
        /// <param name="propertyNames" type="Array" elementType="String" mayBeNull="true" optional="true" elementMayBeNull="false"></param>
        var e = Function._validateParams(arguments, [
            {name: "propertyNames", type: Array, mayBeNull: true, optional: true, elementType: String}
        ]);
        if (e) throw e;
        if(!this.saveCallback) this.saveCallback = Function.createDelegate(this,this._saveComplete);
        Sys.Services.ProfileService.save(propertyNames, this.saveCallback);
    }
    function Sys$Preview$Services$Components$Profile$setProperty(name, value, key) {
        /// <param name="name" type="String" optional="false" mayBeNull="false"></param>
        /// <param name="value" optional="false" mayBeNull="true"></param>
        /// <param name="key" optional="true" mayBeNull="true"></param>
        var e = Function._validateParams(arguments, [
            {name: "name", type: String},
            {name: "value", mayBeNull: true},
            {name: "key", mayBeNull: true, optional: true}
        ]);
        if (e) throw e;
        var props = Sys.Services.ProfileService.properties;
        var wasDirty=null;
        var fullName = name;
        if(key) {
            var group = props[fullName];
            if(!group) {
                group = new Sys.Services.ProfileGroup();
                props[fullName] = group;
            }
            fullName = fullName + '.' + key;
            group[key] = value;
            wasDirty = this._isDirty;
            this._isDirty = true;
            this.raisePropertyChanged(fullName);
        }
        else {
            props[fullName] = value;
            wasDirty = this._isDirty;
            this._isDirty = true;
            this.raisePropertyChanged(fullName);
        }
        
                if (wasDirty === false) {
            this.raisePropertyChanged('isDirty');
        }
        
        if (this._autoSave && this._isDirty) {
                        this.save([fullName]);
        }
    }
    function Sys$Preview$Services$Components$Profile$_loadComplete() {
                        this._isDirty = false;
        var handler = this.get_events().getHandler('loadComplete');
        if(handler) {
            handler(this, Sys.EventArgs.Empty);
        }
    }
    function Sys$Preview$Services$Components$Profile$_saveComplete() {
        this._isDirty = false;
        this.raisePropertyChanged('isDirty');
        var handler = this.get_events().getHandler('saveComplete');
        if(handler) {
            handler(this, Sys.EventArgs.Empty);
        }
    }
    function Sys$Preview$Services$Components$Profile$_saveIfDirty() {
        if (this._isDirty) {
            this.save();
        }
    }
Sys.Preview.Services.Components.Profile.prototype = {
    _isDirty: false,
    _autoSave: false,
    
    get_autoSave: Sys$Preview$Services$Components$Profile$get_autoSave,
    set_autoSave: Sys$Preview$Services$Components$Profile$set_autoSave,
    
    get_isDirty: Sys$Preview$Services$Components$Profile$get_isDirty,
    
    get_path: Sys$Preview$Services$Components$Profile$get_path,
    set_path: Sys$Preview$Services$Components$Profile$set_path,
    
    add_loadComplete: Sys$Preview$Services$Components$Profile$add_loadComplete,
    remove_loadComplete: Sys$Preview$Services$Components$Profile$remove_loadComplete,
    
    add_saveComplete: Sys$Preview$Services$Components$Profile$add_saveComplete,
    remove_saveComplete: Sys$Preview$Services$Components$Profile$remove_saveComplete,
    
    getProperty: Sys$Preview$Services$Components$Profile$getProperty,
    
    initialize: Sys$Preview$Services$Components$Profile$initialize,    
       
    invokeMethod: Sys$Preview$Services$Components$Profile$invokeMethod,
    load: Sys$Preview$Services$Components$Profile$load,
        
    save: Sys$Preview$Services$Components$Profile$save,
        
    setProperty: Sys$Preview$Services$Components$Profile$setProperty,
    _loadComplete: Sys$Preview$Services$Components$Profile$_loadComplete,
    _saveComplete: Sys$Preview$Services$Components$Profile$_saveComplete,
    _saveIfDirty: Sys$Preview$Services$Components$Profile$_saveIfDirty    
}
Sys.Preview.Services.Components.Profile.descriptor = {
    properties: [   {name: 'autoSave', type: Boolean},
                    {name: 'path', type: String},
                    {name: 'isDirty', type: Boolean, readOnly: true} ],
    methods: [  {name: 'load'},
                {name: 'save'} ],
    events: [   {name: 'loadComplete'},
                {name: 'saveComplete'} ]
}
Sys.Preview.Services.Components.Profile.registerClass('Sys.Preview.Services.Components.Profile', Sys.Component, Sys.Preview.ICustomTypeDescriptor);
Sys.Preview.Services.Components.Profile.parseFromMarkup = function Sys$Preview$Services$Components$Profile$parseFromMarkup(type, node, markupContext) {
    if (!markupContext.get_isGlobal()) {
        return null;
    }
    var id=null;
    var idAttribute = node.attributes.getNamedItem('id');
    if (idAttribute) {
        id = idAttribute.nodeValue;
        node.attributes.removeNamedItem('id')
    }
    
    Sys.Preview.MarkupParser.initializeObject(Sys.Preview.Services.Components.Profile.instance, node, markupContext);
    
    if (id && id.length) {
        markupContext._addComponentByID(id, Sys.Preview.Services.Components.Profile.instance, true);
        node.attributes.setNamedItem(idAttribute);
    }
    return Sys.Preview.Services.Components.Profile.instance;
}
Sys.Preview.Services.Components.Profile.instance = new Sys.Preview.Services.Components.Profile();
Type.registerNamespace('Sys.Preview.Data');
Sys.Component.prototype.get_dataContext = function Sys$Component$get_dataContext() {
    return (this._dataContext || null);
}
Sys.Component.prototype.set_dataContext = function Sys$Component$set_dataContext(value) {
    this._dataContext = value;
}
Sys.UI.Control.prototype.get_dataContext = function Sys$UI$Control$get_dataContext() {
    var dc = Sys.UI.Control.callBaseMethod(this, 'get_dataContext');
    if (!dc) {
        var parent = this.get_parent();
        if (parent) {
            dc = parent.get_dataContext();
        }
    }
    return dc;
}
Sys.UI.Control.prototype.set_dataContext = Sys.Component.prototype.set_dataContext;
Sys.UI.Behavior.prototype.get_dataContext = function Sys$UI$Behavior$get_dataContext() {
    var dc = Sys.UI.Behavior.callBaseMethod(this, 'get_dataContext');
    if (!dc) {
        if (this.control) {
            dc = this.control.get_dataContext();
        }
        else {
            var e = this.get_element();
            if (e) {
                                                var c = e.control;
                if (c) dc = c.get_dataContext();
            }
        }
    }
    return dc;
}
Sys.UI.Behavior.prototype.set_dataContext = Sys.Component.prototype.set_dataContext;
Sys.Preview.Data.IData = function Sys$Preview$Data$IData() {
    throw Error.notImplemented();
}
    function Sys$Preview$Data$IData$add() {
        throw Error.notImplemented();
    }
    function Sys$Preview$Data$IData$clear() {
        throw Error.notImplemented();
    }
    function Sys$Preview$Data$IData$get_length() {
        if (arguments.length !== 0) throw Error.parameterCount();
        throw Error.notImplemented();
    }
    function Sys$Preview$Data$IData$getRow() {
        throw Error.notImplemented();
    }
    function Sys$Preview$Data$IData$remove() {
        throw Error.notImplemented();
    }
Sys.Preview.Data.IData.prototype = {
    add: Sys$Preview$Data$IData$add,
    clear: Sys$Preview$Data$IData$clear,
    get_length: Sys$Preview$Data$IData$get_length,
    getRow: Sys$Preview$Data$IData$getRow,
    remove: Sys$Preview$Data$IData$remove
}
Sys.Preview.Data.IData.registerInterface('Sys.Preview.Data.IData');
Sys.Preview.Data.DataRowState = function Sys$Preview$Data$DataRowState() {
    throw Error.invalidOperation();
}
Sys.Preview.Data.DataRowState.prototype = {
    Unchanged: 0,
    Added: 1,
    Deleted: 2,
    Detached: 3,
    Modified: 4
}
Sys.Preview.Data.DataRowState.registerEnum('Sys.Preview.Data.DataRowState');
Sys.Preview.Data.SortDirection = function Sys$Preview$Data$SortDirection() {
    throw Error.invalidOperation();
}
Sys.Preview.Data.SortDirection.prototype = {
    Ascending: 0,
    Descending: 1
}
Sys.Preview.Data.SortDirection.registerEnum('Sys.Preview.Data.SortDirection');
Sys.Preview.Data.ServiceType = function Sys$Preview$Data$ServiceType() {
    throw Error.invalidOperation();
}
Sys.Preview.Data.ServiceType.prototype = {
    DataService: 0,
    Handler: 1
}
Sys.Preview.Data.ServiceType.registerEnum('Sys.Preview.Data.ServiceType');
Sys.Preview.Data.DataColumn = function Sys$Preview$Data$DataColumn(columnName, dataType, defaultValue, isKey, isReadOnly) {
                        this._columnName = columnName;
    this._dataType = dataType;
    this._defaultValue = defaultValue;
    this._readOnly = isReadOnly;
    this._key = isKey;
}
    function Sys$Preview$Data$DataColumn$get_columnName() {
        if (arguments.length !== 0) throw Error.parameterCount();
                return this._columnName;
    }
    function Sys$Preview$Data$DataColumn$get_dataType() {
        if (arguments.length !== 0) throw Error.parameterCount();
                return this._dataType;
    }
    function Sys$Preview$Data$DataColumn$get_defaultValue() {
        if (arguments.length !== 0) throw Error.parameterCount();
                return this._defaultValue;
    }
    function Sys$Preview$Data$DataColumn$get_isKey() {
        if (arguments.length !== 0) throw Error.parameterCount();
                return this._key;
    }
    function Sys$Preview$Data$DataColumn$get_readOnly() {
        if (arguments.length !== 0) throw Error.parameterCount();
                return !!this._readOnly;
    }
    function Sys$Preview$Data$DataColumn$dispose() {
        this._columnName = null;
        this._dataType = null;
        this._defaultValue = null;
    }
Sys.Preview.Data.DataColumn.prototype = {
    get_columnName: Sys$Preview$Data$DataColumn$get_columnName,
    get_dataType: Sys$Preview$Data$DataColumn$get_dataType,
    get_defaultValue: Sys$Preview$Data$DataColumn$get_defaultValue,
    get_isKey: Sys$Preview$Data$DataColumn$get_isKey,
    get_readOnly: Sys$Preview$Data$DataColumn$get_readOnly,
    dispose: Sys$Preview$Data$DataColumn$dispose
}
Sys.Preview.Data.DataColumn.parseFromJson = function Sys$Preview$Data$DataColumn$parseFromJson(json) {
    /// <param name="json" type="Object" optional="false" mayBeNull="false"></param>
    var e = Function._validateParams(arguments, [
        {name: "json", type: Object}
    ]);
    if (e) throw e;
    return new Sys.Preview.Data.DataColumn(json.name, typeof (json.dataType === 'string') ? eval(json.dataType) : json.dataType, json.defaultValue, json.isKey, json.readOnly);
}
Sys.Preview.Data.DataColumn.registerClass('Sys.Preview.Data.DataColumn', null, Sys.IDisposable);
Sys.Preview.Data.DataColumn.descriptor = {
    properties: [ { name: 'columnName', type: String, readOnly: true },
                  { name: 'dataType', type: Sys.Type, readOnly: true },
                  { name: 'defaultValue', readOnly: true },
                  { name: 'isKey', type: Boolean, readOnly: true },
                  { name: 'readOnly', type: Boolean, readOnly: true } ]
}
Sys.Preview.Data.DataRow = function Sys$Preview$Data$DataRow(objectDataRow, dataTableOwner, index) {
                this._owner = dataTableOwner;
    this._row = objectDataRow;
    this._index = index;
}
    function Sys$Preview$Data$DataRow$get_events() {
    if (arguments.length !== 0) throw Error.parameterCount();
                if (!this._events) {
            this._events = new Sys.EventHandlerList();
        }
        return this._events;
    }
    function Sys$Preview$Data$DataRow$add_propertyChanged(handler) {
    var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
    if (e) throw e;
                if(this._disposed) {
            
            return;
        }
        this.get_events().addHandler("propertyChanged", handler);
    }
    function Sys$Preview$Data$DataRow$remove_propertyChanged(handler) {
    var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
    if (e) throw e;
                if(this._disposed) return;
        this.get_events().removeHandler("propertyChanged", handler);
    }
    function Sys$Preview$Data$DataRow$_onPropertyChanged(propertyName) {
                var handler = this.get_events().getHandler("propertyChanged");
        if (handler) {
            handler(this, new Sys.PropertyChangedEventArgs(propertyName));
        }
    }
    function Sys$Preview$Data$DataRow$get_isDirty() {
    if (arguments.length !== 0) throw Error.parameterCount();
                return typeof(this._row._original) === "object";
    }
    function Sys$Preview$Data$DataRow$get_index() {
    if (arguments.length !== 0) throw Error.parameterCount();
                return this._index;
    }
    function Sys$Preview$Data$DataRow$_set_index(index) {
                this._index = index;
    }
    function Sys$Preview$Data$DataRow$get_rowObject() {
    if (arguments.length !== 0) throw Error.parameterCount();
                return typeof(this._row._rowObject) !== "undefined" ? this._row._rowObject : this._row;
    }
    function Sys$Preview$Data$DataRow$get_selected() {
    if (arguments.length !== 0) throw Error.parameterCount();
                return this._selected;
    }
    function Sys$Preview$Data$DataRow$set_selected(value) {
        if (this._selected !== value) {
            this._selected = value;
            this._onPropertyChanged("$selected");
        }
    }
    function Sys$Preview$Data$DataRow$get_state() {
    if (arguments.length !== 0) throw Error.parameterCount();
                return this._state;
    }
    function Sys$Preview$Data$DataRow$_set_state(value) {
        this._state = value;
    }
    function Sys$Preview$Data$DataRow$get_table() {
    if (arguments.length !== 0) throw Error.parameterCount();
                return this._owner;
    }
    function Sys$Preview$Data$DataRow$_set_table(value) {
        this._owner = value;
    }
    function Sys$Preview$Data$DataRow$dispose() {
        delete this._events;
        this._row = null;
        this._owner = null;
        this._disposed = true;
    }
    function Sys$Preview$Data$DataRow$getProperty(name, key) {
                                if (!name) {
            return typeof(this._row._rowObject) !== "undefined" ? this._row._rowObject : this._row;
        }
        switch(name) {
        case "$isDirty":
            return this.get_isDirty();
        case "$index":
            return this._index;
        case "$selected":
            return this.get_selected();
        }
        return Sys.Preview.TypeDescriptor.getProperty(this._row, name, key);
    }
    function Sys$Preview$Data$DataRow$setProperty(name, value, key) {
                                if (name === "$selected") {
            this.set_selected(value);
            return;
        }
        if (this._row[name] === value) return;
        var isDirty = this.get_isDirty();
        if (!isDirty && this._owner && (this.get_state() === Sys.Preview.Data.DataRowState.Unchanged)) {
            var original = {};
            for (var columnName in this._row) {
                if ((columnName.charAt(0) !== '_') && (typeof(this._row[columnName]) !== "function")) {
                    original[columnName] = this._row[columnName];
                }
            }
            this._row._original = original;
            this._set_state(Sys.Preview.Data.DataRowState.Modified);
        }
                Sys.Preview.TypeDescriptor.setProperty(this._row, name, value, key);
        this._onPropertyChanged(name);
        if (!isDirty) {
            this._onPropertyChanged("$isDirty");
        }
        this._owner.raiseRowChanged(this._row);
    }
    function Sys$Preview$Data$DataRow$invokeMethod(methodName, parameters) {
                    }
Sys.Preview.Data.DataRow.prototype = {
    _state: Sys.Preview.Data.DataRowState.Unchanged,
    _selected: false,
    _events: null,
    get_events: Sys$Preview$Data$DataRow$get_events,
    add_propertyChanged: Sys$Preview$Data$DataRow$add_propertyChanged,
    remove_propertyChanged: Sys$Preview$Data$DataRow$remove_propertyChanged,
    _onPropertyChanged: Sys$Preview$Data$DataRow$_onPropertyChanged,
    get_isDirty: Sys$Preview$Data$DataRow$get_isDirty,
    get_index: Sys$Preview$Data$DataRow$get_index,
    _set_index: Sys$Preview$Data$DataRow$_set_index,
    get_rowObject: Sys$Preview$Data$DataRow$get_rowObject,
    get_selected: Sys$Preview$Data$DataRow$get_selected,
    set_selected: Sys$Preview$Data$DataRow$set_selected,
    get_state: Sys$Preview$Data$DataRow$get_state,
    _set_state: Sys$Preview$Data$DataRow$_set_state,
    get_table: Sys$Preview$Data$DataRow$get_table,
    _set_table: Sys$Preview$Data$DataRow$_set_table,
    dispose: Sys$Preview$Data$DataRow$dispose,
    
    getProperty: Sys$Preview$Data$DataRow$getProperty,
    setProperty: Sys$Preview$Data$DataRow$setProperty,
    invokeMethod: Sys$Preview$Data$DataRow$invokeMethod
}
Sys.Preview.Data.DataRow.registerClass('Sys.Preview.Data.DataRow', null, Sys.Preview.ICustomTypeDescriptor, Sys.INotifyPropertyChange, Sys.IDisposable);
Sys.Preview.Data.DataRow.descriptor = {
    properties: [ { name: '$isDirty', type: Boolean, readOnly: true },
                  { name: '$index', type: Number, readOnly: true },
                  { name: '$selected', type: Boolean } ],
    events: [ { name: 'propertyChanged', readOnly: true } ]
}
Sys.Preview.Data.DataRowView = function Sys$Preview$Data$DataRowView(dataRow, index) {
            this._row = dataRow;
    this._index = index;
}
    function Sys$Preview$Data$DataRowView$get_events() {
    if (arguments.length !== 0) throw Error.parameterCount();
                if (!this._events) {
            this._events = new Sys.EventHandlerList();
        }
        return this._events;
    }
    function Sys$Preview$Data$DataRowView$add_propertyChanged(handler) {
    var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
    if (e) throw e;
                this.get_events().addHandler("propertyChanged", handler);
    }
    function Sys$Preview$Data$DataRowView$remove_propertyChanged(handler) {
    var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
    if (e) throw e;
                this.get_events().removeHandler("propertyChanged", handler);
    }
    function Sys$Preview$Data$DataRowView$_onPropertyChanged(propertyName) {
        var handler = this.get_events().getHandler("propertyChanged");
        if (handler) {
            handler(this, new Sys.PropertyChangedEventArgs(propertyName));
        }
    }
    function Sys$Preview$Data$DataRowView$get_dataIndex() {
    if (arguments.length !== 0) throw Error.parameterCount();
                return this._row.get_index();
    }
    function Sys$Preview$Data$DataRowView$get_index() {
    if (arguments.length !== 0) throw Error.parameterCount();
                return this._index;
    }
    function Sys$Preview$Data$DataRowView$_set_index(value) {
        this._index = value;
    }
    function Sys$Preview$Data$DataRowView$get_isDirty() {
    if (arguments.length !== 0) throw Error.parameterCount();
                return this._row.get_isDirty();
    }
    function Sys$Preview$Data$DataRowView$_get_row() {
        return this._row;
    }
    function Sys$Preview$Data$DataRowView$get_rowObject() {
    if (arguments.length !== 0) throw Error.parameterCount();
                return this._row.get_rowObject();
    }
    function Sys$Preview$Data$DataRowView$get_selected() {
    if (arguments.length !== 0) throw Error.parameterCount();
                return this._row.get_selected();
    }
    function Sys$Preview$Data$DataRowView$set_selected(value) {
        this._row.set_selected(value);
    }
    function Sys$Preview$Data$DataRowView$get_table() {
    if (arguments.length !== 0) throw Error.parameterCount();
                return this._row.get_table();
    }
    function Sys$Preview$Data$DataRowView$dispose() {
        if (this._row && this._rowPropertyChanged) {
            this._row.remove_propertyChanged(this._rowPropertyChanged);
        }
        delete this._events;
        this._row = null;
    }
    function Sys$Preview$Data$DataRowView$initialize() {
        this._rowPropertyChanged = Function.createDelegate(this, this._onRowPropertyChanged);
        this._row.add_propertyChanged(this._rowPropertyChanged);
    }
    function Sys$Preview$Data$DataRowView$_onRowPropertyChanged(sender, args) {
        this._onPropertyChanged(args.get_propertyName());
    }
    function Sys$Preview$Data$DataRowView$getProperty(name, key) {
                                if (name === "$index") return this._index;
        if (name === "$dataIndex") return this._row.get_index();
        return this._row.getProperty(name, key);
    }
    function Sys$Preview$Data$DataRowView$setProperty(name, value, key) {
                                this._row.setProperty(name, value, key);
    }
    function Sys$Preview$Data$DataRowView$invokeMethod(methodName, parameters) {
                    }
Sys.Preview.Data.DataRowView.prototype = {
    _rowPropertyChanged: null,
    _events: null,
    get_events: Sys$Preview$Data$DataRowView$get_events,
    add_propertyChanged: Sys$Preview$Data$DataRowView$add_propertyChanged,
    remove_propertyChanged: Sys$Preview$Data$DataRowView$remove_propertyChanged,
    _onPropertyChanged: Sys$Preview$Data$DataRowView$_onPropertyChanged,
    get_dataIndex: Sys$Preview$Data$DataRowView$get_dataIndex,
    get_index: Sys$Preview$Data$DataRowView$get_index,
    _set_index: Sys$Preview$Data$DataRowView$_set_index,
    get_isDirty: Sys$Preview$Data$DataRowView$get_isDirty,
    _get_row: Sys$Preview$Data$DataRowView$_get_row,
    get_rowObject: Sys$Preview$Data$DataRowView$get_rowObject,
    get_selected: Sys$Preview$Data$DataRowView$get_selected,
    set_selected: Sys$Preview$Data$DataRowView$set_selected,
    get_table: Sys$Preview$Data$DataRowView$get_table,
    dispose: Sys$Preview$Data$DataRowView$dispose,
    initialize: Sys$Preview$Data$DataRowView$initialize,
    _onRowPropertyChanged: Sys$Preview$Data$DataRowView$_onRowPropertyChanged,
        getProperty: Sys$Preview$Data$DataRowView$getProperty,
    setProperty: Sys$Preview$Data$DataRowView$setProperty,
    invokeMethod: Sys$Preview$Data$DataRowView$invokeMethod
}
Sys.Preview.Data.DataRowView.registerClass('Sys.Preview.Data.DataRowView', null, Sys.Preview.ICustomTypeDescriptor, Sys.INotifyPropertyChange, Sys.IDisposable);
Sys.Preview.Data.DataRowView.descriptor = {
    properties: [ { name: '$dataIndex', type: Number, readOnly: true },
                  { name: '$isDirty', type: Boolean, readOnly: true },
                  { name: '$index', type: Number, readOnly: true },
                  { name: '$selected', type: Boolean } ],
    events: [ { name: 'propertyChanged', readOnly: true } ]
}
Sys.Preview.Data.DataRowCollection = function Sys$Preview$Data$DataRowCollection(dataRowViews, dataTable) {
            this._rows = dataRowViews;
    this._dataTable = dataTable;
}
    function Sys$Preview$Data$DataRowCollection$get_events() {
    if (arguments.length !== 0) throw Error.parameterCount();
                if (!this._events) {
            this._events = new Sys.EventHandlerList();
        }
        return this._events;
    }
    function Sys$Preview$Data$DataRowCollection$add_propertyChanged(handler) {
    var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
    if (e) throw e;
                this.get_events().addHandler("propertyChanged", handler);
    }
    function Sys$Preview$Data$DataRowCollection$remove_propertyChanged(handler) {
    var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
    if (e) throw e;
                this.get_events().removeHandler("propertyChanged", handler);
    }
    function Sys$Preview$Data$DataRowCollection$_onPropertyChanged(propertyName) {
                var handler = this.get_events().getHandler("propertyChanged");
        if (handler) {
            handler(this, new Sys.PropertyChangedEventArgs(propertyName));
        }
    }
    function Sys$Preview$Data$DataRowCollection$add_collectionChanged(handler) {
    var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
    if (e) throw e;
                this.get_events().addHandler("collectionChanged", handler);
    }
    function Sys$Preview$Data$DataRowCollection$remove_collectionChanged(handler) {
    var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
    if (e) throw e;
                this.get_events().removeHandler("collectionChanged", handler);
    }
    function Sys$Preview$Data$DataRowCollection$_onCollectionChanged(action, changedItem) {
                        var handler = this.get_events().getHandler("collectionChanged");
        if (handler) {
            handler(this, new Sys.Preview.CollectionChangedEventArgs(action, changedItem));
        }
    }
    function Sys$Preview$Data$DataRowCollection$_get_dataTable() {
                return this._dataTable;
    }
    function Sys$Preview$Data$DataRowCollection$get_length() {
    if (arguments.length !== 0) throw Error.parameterCount();
                return this._rows.length;
    }
    function Sys$Preview$Data$DataRowCollection$add(rowObject) {
                var row = this._dataTable.add(rowObject);
        var rv = new Sys.Preview.Data.DataRowView(row, this._rows.length);
        rv.initialize();
        if (typeof (this._rows.add) === "function") {
            this._rows.add(rv);
        }
        else {
            Array.add(this._rows, rv);
        }
        if (this._indexToRow) {
            this._indexToRow[row.get_dataIndex()] = row;
        }
    }
    function Sys$Preview$Data$DataRowCollection$clear() {
        this._suspendNotifications = true;
        for (var i = this._rows.length - 1; i >= 0; i--) {
            this._dataTable.remove(this._rows[i]._get_row());
        }
        this._rows = [];
        this._indexToRow = null;
        this._suspendNotifications = false;
        this._onCollectionChanged(Sys.Preview.NotifyCollectionChangedAction.Reset, null);
    }
    function Sys$Preview$Data$DataRowCollection$getRow(index) {
                        return this._rows[index];
    }
    function Sys$Preview$Data$DataRowCollection$getItem(index) {
                        return this.getRow(index);
    }
    function Sys$Preview$Data$DataRowCollection$remove(rowObject) {
                this._dataTable.remove(rowObject._get_row());
    }
    function Sys$Preview$Data$DataRowCollection$dispose() {
        if (this._dataTable && this._tableCollectionChanged) {
            this._dataTable.remove_collectionChanged(this._tableCollectionChanged);
            this._tableCollectionChanged = null;
        }
        delete this._events;
        this._rows = null;
        this._dataTable = null;
    }
    function Sys$Preview$Data$DataRowCollection$initialize() {
        if (this._dataTable.add_collectionChanged) {
            this._tableCollectionChanged = Function.createDelegate(this, this.onTableCollectionChanged);
            this._dataTable.add_collectionChanged(this._tableCollectionChanged);
        }
    }
    function Sys$Preview$Data$DataRowCollection$ensureLookupTable() {
        if (!this._indexToRow) {
            this._indexToRow = [];
            for (var j = this._rows.length - 1; j >= 0; j--) {
                var row = this._rows[j];
                this._indexToRow[row.get_dataIndex()] = row;
            }
        }
    }
    function Sys$Preview$Data$DataRowCollection$onTableCollectionChanged(sender, args) {
                        if (this._suspendNotifications) return;
        switch (args.get_action()) {
            case Sys.Preview.NotifyCollectionChangedAction.Reset:
                this._rows = [];
                this._indexToRow = null;
                this._onCollectionChanged(Sys.Preview.NotifyCollectionChangedAction.Reset, changedItem);
                return;
            case Sys.Preview.NotifyCollectionChangedAction.Remove:
                var changedItem = args.get_changedItem();
                this.ensureLookupTable();
                var idx = changedItem.get_index();
                if (this._indexToRow[idx]) {
                    if (typeof (this._rows.remove) === "function") {
                        this._rows.remove(this._indexToRow[idx]);
                    }
                    else {
                        Array.remove(this._rows, this._indexToRow[idx]);
                    }
                    delete this._indexToRow[idx];
                    this._onCollectionChanged(Sys.Preview.NotifyCollectionChangedAction.Remove, changedItem);
                }
                return;
        }
    }
Sys.Preview.Data.DataRowCollection.prototype = {
    _indexToRow: null,
    _tableCollectionChanged: null,
    _suspendNotifications: false,
    _events: null,
    get_events: Sys$Preview$Data$DataRowCollection$get_events,
    add_propertyChanged: Sys$Preview$Data$DataRowCollection$add_propertyChanged,
    remove_propertyChanged: Sys$Preview$Data$DataRowCollection$remove_propertyChanged,
    _onPropertyChanged: Sys$Preview$Data$DataRowCollection$_onPropertyChanged,
    add_collectionChanged: Sys$Preview$Data$DataRowCollection$add_collectionChanged,
    remove_collectionChanged: Sys$Preview$Data$DataRowCollection$remove_collectionChanged,
    _onCollectionChanged: Sys$Preview$Data$DataRowCollection$_onCollectionChanged,
    _get_dataTable: Sys$Preview$Data$DataRowCollection$_get_dataTable,
    get_length: Sys$Preview$Data$DataRowCollection$get_length,
    add: Sys$Preview$Data$DataRowCollection$add,
    clear: Sys$Preview$Data$DataRowCollection$clear,
    getRow: Sys$Preview$Data$DataRowCollection$getRow,
    getItem: Sys$Preview$Data$DataRowCollection$getItem,
    remove: Sys$Preview$Data$DataRowCollection$remove,
    dispose: Sys$Preview$Data$DataRowCollection$dispose,
    initialize: Sys$Preview$Data$DataRowCollection$initialize,
    ensureLookupTable: Sys$Preview$Data$DataRowCollection$ensureLookupTable,
    onTableCollectionChanged: Sys$Preview$Data$DataRowCollection$onTableCollectionChanged
}
Sys.Preview.Data.DataRowCollection.registerClass('Sys.Preview.Data.DataRowCollection', null, Sys.Preview.Data.IData, Sys.INotifyPropertyChange, Sys.Preview.INotifyCollectionChanged, Sys.IDisposable);
Sys.Preview.Data.DataRowCollection.descriptor = {
    properties: [ { name: 'length', type: Number, readOnly: true } ],
    methods: [ { name: 'add' },
               { name: 'clear' },
               { name: 'remove' } ],
    events: [ { name: 'collectionChanged', readOnly: true },
              { name: 'propertyChanged', readOnly: true } ]
}
Sys.Preview.Data.DataTable = function Sys$Preview$Data$DataTable(columns, tableArray) {
            this._array = Array.isInstanceOfType(tableArray) ? tableArray : [];
    this._columns = Array.isInstanceOfType(columns) ? columns : [];
    this._rows = [];
    this._deletedRows = [];
    this._newRows = [];
    this._updatedRows = [];
    this._columnDictionary = {};
    this._keys = null;
    this._events = null;
}
    function Sys$Preview$Data$DataTable$get_events() {
    if (arguments.length !== 0) throw Error.parameterCount();
                if (!this._events) {
            this._events = new Sys.EventHandlerList();
        }
        return this._events;
    }
    function Sys$Preview$Data$DataTable$add_propertyChanged(handler) {
    var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
    if (e) throw e;
                if(this._disposed) return;
        this.get_events().addHandler("propertyChanged", handler);
    }
    function Sys$Preview$Data$DataTable$remove_propertyChanged(handler) {
    var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
    if (e) throw e;
                if(this._disposed) return;
        this.get_events().removeHandler("propertyChanged", handler);
    }
    function Sys$Preview$Data$DataTable$_onPropertyChanged(propertyName) {
                if(this._disposed) return;
        var handler = this.get_events().getHandler("propertyChanged");
        if (handler) {
            handler(this, new Sys.PropertyChangedEventArgs(propertyName));
        }
    }
    function Sys$Preview$Data$DataTable$add_collectionChanged(handler) {
    var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
    if (e) throw e;
                if(this._disposed) return;
        this.get_events().addHandler("collectionChanged", handler);
    }
    function Sys$Preview$Data$DataTable$remove_collectionChanged(handler) {
    var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
    if (e) throw e;
                if(this._disposed) return;
        this.get_events().removeHandler("collectionChanged", handler);
    }
    function Sys$Preview$Data$DataTable$_onCollectionChanged(action, changedItem) {
                        if(this._disposed) return;
        var handler = this.get_events().getHandler("collectionChanged");
        if (handler) {
            handler(this, new Sys.Preview.CollectionChangedEventArgs(action, changedItem));
        }
    }
    function Sys$Preview$Data$DataTable$get_columns() {
    if (arguments.length !== 0) throw Error.parameterCount();
                return this._columns;
    }
    function Sys$Preview$Data$DataTable$get_keyNames() {
    if (arguments.length !== 0) throw Error.parameterCount();
                if(this._disposed) return null;
        if (!this._keys) {
            this._keys = [];
            var len = this._columns.length;
            for (var i = 0; i < len; i++) {
                var col = this._columns[i];
                if (col.get_isKey()) {
                    Array.add(this._keys, col.get_columnName());
                }
            }
        }
        return this._keys;
    }
    function Sys$Preview$Data$DataTable$get_isDirty() {
    if (arguments.length !== 0) throw Error.parameterCount();
                if(this._disposed) return false;
        return (this._deletedRows.length !== 0) || (this._newRows.length !== 0) || (this._updatedRows.length !== 0);
    }
    function Sys$Preview$Data$DataTable$get_length() {
    if (arguments.length !== 0) throw Error.parameterCount();
                if(this._disposed) return 0;
        return this._array.length;
    }
    function Sys$Preview$Data$DataTable$add(rowObject) {
                        if(this._disposed) return null;
        var row;
        if (Sys.Preview.Data.DataRow.isInstanceOfType(rowObject)) {
            row = rowObject;
            row._set_table(this);
            rowObject = rowObject.get_rowObject();
        }
        else {
            row = new Sys.Preview.Data.DataRow(rowObject, this);
        }
        var index = this._array.length;
        row._set_index(index);
        var columns = this.get_columns();
        if (columns) {
            for(var i = columns.length - 1; i >= 0; i--) {
                var column = columns[i];
                if (typeof(rowObject[column.get_columnName()]) === "undefined") {
                    rowObject[column.get_columnName()] = column.get_defaultValue();
                }
            }
        }
        var oldIsDirty = this.get_isDirty();
        this._array[index] = rowObject;
        this._rows[index] = row;
        Array.add(this._newRows, rowObject);
        row._set_state(Sys.Preview.Data.DataRowState.Added);
        this._onCollectionChanged(Sys.Preview.NotifyCollectionChangedAction.Add, row);
        this._onPropertyChanged("length");
        if (!oldIsDirty) {
            this._onPropertyChanged("isDirty");
        }
        return row;
    }
    function Sys$Preview$Data$DataTable$clear() {
        if (this.get_length() > 0) {
            var oldIsDirty = this.get_isDirty();
            for (var i = this._array.length - 1; i >= 0; i--) {
                var row = this._array[i];
                if (row && !Array.contains(this._newRows, row)) {
                    Array.add(this._deletedRows, row);
                    this._rows[i]._set_state(Sys.Preview.Data.DataRowState.Deleted);
                }
            }
            this._rows = [];
            this._array = [];
            this._newRows = [];
            this._updatedRows = [];
            this._onCollectionChanged(Sys.Preview.NotifyCollectionChangedAction.Reset, null);
            this._onPropertyChanged("length");
            if (!oldIsDirty) {
                this._onPropertyChanged("isDirty");
            }
        }
    }
    function Sys$Preview$Data$DataTable$createRow(initialData) {
                        if(this._disposed) return null;
        var obj = {},
            undef = {};
        for (var i = this._columns.length - 1; i >= 0; i--) {
            var column = this._columns[i];
            var columnName = column.get_columnName();
            var val = undef;
            if (initialData) {
                val = Sys.Preview.TypeDescriptor.getProperty(initialData, columnName);
            }
            if ((val === undef) || (typeof(val) === "undefined")) {
                val = column.get_defaultValue();
            }
            obj[columnName] = val;
        }
        var row = new Sys.Preview.Data.DataRow(obj, this, -1);
        row._set_state(Sys.Preview.Data.DataRowState.Detached);
        return row;
    }
    function Sys$Preview$Data$DataTable$getChanges() {
                if(this._disposed) return null;
        return {updated : this._updatedRows, inserted : this._newRows, deleted : this._deletedRows};
    }
    function Sys$Preview$Data$DataTable$getColumn(name) {
                        if(this._disposed) return null;
        var col = this._columnDictionary[name];
        if (col) {
            return col;
        }
        for (var c = this._columns.length - 1; c >= 0; c--) {
            var column = this._columns[c];
            if (column.get_columnName() === name) {
                this._columnDictionary[name] = column;
                return column;
            }
        }
        
        return null;
    }
    function Sys$Preview$Data$DataTable$getRow(index) {
                        if(this._disposed) return null;
        var row = this._rows[index];
        if (!row) {
            var rowObject = this._array[index];
            if (rowObject) {
                row = Sys.Preview.Data.DataRow.isInstanceOfType(rowObject) ? rowObject : new Sys.Preview.Data.DataRow(rowObject, this, index);
                this._rows[index] = row;
            }
        }
        return row;
    }
    function Sys$Preview$Data$DataTable$getItem(index) {
                        return this.getRow(index);
    }
    function Sys$Preview$Data$DataTable$remove(rowObject) {
                if(this._disposed) return;
        if (Sys.Preview.Data.DataRow.isInstanceOfType(rowObject)) {
            rowObject = rowObject.get_rowObject();
        }
        var oldIsDirty = this.get_isDirty();
        var index = Array.indexOf(this._array, rowObject);
        var row = this.getItem(index);
        if(typeof(this._array.removeAt) === "function") {
            this._array.removeAt(index);
        }
        else {
            Array.removeAt(this._array, index);
        }
        Array.removeAt(this._rows, index);
        index = Array.indexOf(this._newRows, rowObject);
        if (index !== -1) {
            Array.removeAt(this._newRows, index);
        }
        else {
            Array.add(this._deletedRows, rowObject);
        }
        row._set_state(Sys.Preview.Data.DataRowState.Deleted);
        this._onCollectionChanged(Sys.Preview.NotifyCollectionChangedAction.Remove, row);
        this._onPropertyChanged("length");
        if (oldIsDirty !== this.get_isDirty()) {
            this._onPropertyChanged("isDirty");
        }
    }
    function Sys$Preview$Data$DataTable$dispose() {
        delete this._events;
        this._disposed = true;
        var i, row;
        if (this._rows) {
            for (i = this._rows.length - 1; i >= 0; i--) {
                row = this._rows[i];
                if (row) {
                    this._rows[i].dispose();
                }
            }
        }
        if (this._deletedRows) {
            for (i = this._deletedRows.length - 1; i >= 0; i--) {
                row = this._deletedRows[i];
                if (row && row.dispose) {
                    row.dispose();
                }
            }
        }
        if (this._newRows) {
            for (i = this._newRows.length - 1; i >= 0; i--) {
                row = this._newRows[i];
                if (row && row.dispose) {
                    row.dispose();
                }
            }
        }
        if (this._updatedRows) {
            for (i = this._updatedRows.length - 1; i >= 0; i--) {
                row = this._updatedRows[i];
                if (row && row.dispose) {
                    row.dispose();
                }
            }
        }
        this._rows = null;
        this._deletedRows = null;
        this._newRows = null;
        this._updatedRows = null;
        this._columns = null;
        this._array = null;
        this._keys = null;
    }
    function Sys$Preview$Data$DataTable$raiseRowChanged(changedItem) {
                if(this._disposed) return;
        if ((Array.indexOf(this._updatedRows, changedItem) === -1) &&
            (Array.indexOf(this._newRows, changedItem) === -1)) {
            var oldIsDirty = this.get_isDirty();
            Array.add(this._updatedRows, changedItem);
            if (!oldIsDirty) {
                this._onPropertyChanged("isDirty");
            }
        }
    }
Sys.Preview.Data.DataTable.prototype = {
    get_events: Sys$Preview$Data$DataTable$get_events,
    add_propertyChanged: Sys$Preview$Data$DataTable$add_propertyChanged,
    remove_propertyChanged: Sys$Preview$Data$DataTable$remove_propertyChanged,
    _onPropertyChanged: Sys$Preview$Data$DataTable$_onPropertyChanged,
    add_collectionChanged: Sys$Preview$Data$DataTable$add_collectionChanged,
    remove_collectionChanged: Sys$Preview$Data$DataTable$remove_collectionChanged,
    _onCollectionChanged: Sys$Preview$Data$DataTable$_onCollectionChanged,
    get_columns: Sys$Preview$Data$DataTable$get_columns,
    get_keyNames: Sys$Preview$Data$DataTable$get_keyNames,
    get_isDirty: Sys$Preview$Data$DataTable$get_isDirty,
    get_length: Sys$Preview$Data$DataTable$get_length,
    add: Sys$Preview$Data$DataTable$add,
    clear: Sys$Preview$Data$DataTable$clear,
    createRow: Sys$Preview$Data$DataTable$createRow,
    getChanges: Sys$Preview$Data$DataTable$getChanges,
    getColumn: Sys$Preview$Data$DataTable$getColumn,
    getRow: Sys$Preview$Data$DataTable$getRow,
    getItem: Sys$Preview$Data$DataTable$getItem,
    remove: Sys$Preview$Data$DataTable$remove,
    dispose: Sys$Preview$Data$DataTable$dispose,
    raiseRowChanged: Sys$Preview$Data$DataTable$raiseRowChanged
}
Sys.Preview.Data.DataTable.parseFromJson = function Sys$Preview$Data$DataTable$parseFromJson(json) {
    /// <param name="json" type="Object" optional="false" mayBeNull="false"></param>
    var e = Function._validateParams(arguments, [
        {name: "json", type: Object}
    ]);
    if (e) throw e;
    var columnArray = null;
    if(json.columns) {
        columnArray = [];
        for(var i=0; i < json.columns.length; i++) {
            Array.add(columnArray, Sys.Preview.Data.DataColumn.parseFromJson(json.columns[i]));
        }
    }
        return new Sys.Preview.Data.DataTable(columnArray, json.rows);
}
Sys.Preview.Data.DataTable.registerClass('Sys.Preview.Data.DataTable', null, Sys.Preview.Data.IData, Sys.INotifyPropertyChange, Sys.Preview.INotifyCollectionChanged, Sys.IDisposable);
Sys.Preview.Data.DataTable.descriptor = {
    properties: [ { name: 'columns', type: Array, readOnly: true },
                  { name: 'keyNames', type: Array, readOnly: true },
                  { name: 'length', type: Number, readOnly: true },
                  { name: 'isDirty', type: Boolean, readOnly: true } ],
    methods: [ { name: 'add' },
               { name: 'clear' },
               { name: 'remove' } ],
    events: [ { name: 'collectionChanged', readOnly: true },
              { name: 'propertyChanged', readOnly: true } ]
}
Sys.Preview.Data.DataView = function Sys$Preview$Data$DataView() {
                    Sys.Preview.Data.DataView.initializeBase(this);
}
    function Sys$Preview$Data$DataView$get_data() {
    if (arguments.length !== 0) throw Error.parameterCount();
                                        return this._data;
    }
    function Sys$Preview$Data$DataView$set_data(data) {
                
        if (!this._dataChangedDelegate) {
            this._dataChangedDelegate = Function.createDelegate(this, this.onDataChanged);
        }
        this._filteredTable = null;
        if (this._data && this._data.remove_collectionChanged) {
            this._data.remove_collectionChanged(this._dataChangedDelegate);
        }
        this._data = data;
        if (this._data && this._data.add_collectionChanged) {
            this._data.add_collectionChanged(this._dataChangedDelegate);
        }
        this.raisePropertyChanged('data');
        this.raisePropertyChanged('filteredData');
    }
    function Sys$Preview$Data$DataView$get_filteredData() {
    if (arguments.length !== 0) throw Error.parameterCount();
                                        this.ensureFilteredData();
        return this._filteredTable;
    }
    function Sys$Preview$Data$DataView$get_filters() {
    if (arguments.length !== 0) throw Error.parameterCount();
                                        if (!this._filters) {
            this._filters = Sys.Component.createCollection(this);
            if (!this._dataChangedDelegate) {
                this._dataChangedDelegate = Function.createDelegate(this, this.onDataChanged);
            }
            this._filters.add_collectionChanged(this._dataChangedDelegate);
        }
        return this._filters;
    }
    function Sys$Preview$Data$DataView$get_hasNextPage() {
    if (arguments.length !== 0) throw Error.parameterCount();
                                        this.ensureFilteredData();
        return (this.get_pageIndex() < this.get_pageCount() - 1);
    }
    function Sys$Preview$Data$DataView$get_hasPreviousPage() {
    if (arguments.length !== 0) throw Error.parameterCount();
                                        if (!this._data) return false;
        return (this.get_pageIndex() > 0);
    }
    function Sys$Preview$Data$DataView$get_length() {
    if (arguments.length !== 0) throw Error.parameterCount();
                                        this.ensureFilteredData();
        return this._filteredTable ? (this._filteredTable.length? this._filteredTable.length : this._filteredTable.get_length()) : 0;
    }
    function Sys$Preview$Data$DataView$get_pageCount() {
    if (arguments.length !== 0) throw Error.parameterCount();
                                        if (this._pageSize === 0) {
            return 1;
        }
        this.ensureFilteredData();
        if (!this._filteredRows) return 1;
        return Math.floor((this._filteredRows.length - 1) / this._pageSize) + 1;
    }
    function Sys$Preview$Data$DataView$get_pageIndex() {
    if (arguments.length !== 0) throw Error.parameterCount();
                                        return this._pageIndex;
    }
    function Sys$Preview$Data$DataView$set_pageIndex(value, dontRaiseFilteredDataChanged) {
                        
        var count = this.get_pageCount();
        if (value >= count) {
            value = (count > 0 ? count - 1 : 0);
        }
        if (value !== this._pageIndex) {
            var oldState = this.prepareChange();
            this._pageIndex = value;
            this._paginatedRows = null;
            this.triggerChangeEvents(oldState, false);
            if (!dontRaiseFilteredDataChanged) {
                this.raisePropertyChanged('filteredData');
            }
        }
    }
    function Sys$Preview$Data$DataView$get_pageSize() {
    if (arguments.length !== 0) throw Error.parameterCount();
                                        return this._pageSize;
    }
    function Sys$Preview$Data$DataView$set_pageSize(value) {
        if (this._pageSize !== value) {
            var oldState = this.prepareChange();
            this._pageSize = value;
            this._paginatedRows = null;
            this.triggerChangeEvents(oldState, true);
            this.raisePropertyChanged('filteredData');
        }
    }
    function Sys$Preview$Data$DataView$get_sortColumn() {
    if (arguments.length !== 0) throw Error.parameterCount();
                                        return this._sortColumn;
    }
    function Sys$Preview$Data$DataView$set_sortColumn(value) {
        this.sort(value, this._sortDirection);
    }
    function Sys$Preview$Data$DataView$get_sortDirection() {
    if (arguments.length !== 0) throw Error.parameterCount();
                                        return this._sortDirection;
    }
    function Sys$Preview$Data$DataView$set_sortDirection(value) {
        this.sort(this._sortColumn, value);
    }
    function Sys$Preview$Data$DataView$dispose() {
        this._disposed = true;
        if (this._filters) {
            this._filters.dispose();
            this._filters = null;
        }
        if (this._data && this._dataChangedDelegate) {
            if(this._data.removeCollectionChanged) this._data.remove_collectionChanged(this._dataChangedDelegate);
            this._dataChangedDelegate = null;
            this._data = null;
        }
        Sys.Preview.Data.DataView.callBaseMethod(this, 'dispose');
    }
    function Sys$Preview$Data$DataView$getItem(index) {
                                                return this._filteredTable ? this._filteredTable[index] : null;
    }
    function Sys$Preview$Data$DataView$initialize() {
        Sys.Preview.Data.DataView.callBaseMethod(this, 'initialize');
        if (this._filters) {
            for (var i = 0; i < this._filters.length; i++) {
                this._filters[i].initialize(this);
            }
        }
    }
    function Sys$Preview$Data$DataView$sort(sortColumn, sortDirection) {
                                                var colChanged = (sortColumn !== this._sortColumn);
        var dirChanged = (sortDirection !== this._sortDirection);
        if (colChanged || dirChanged) {
            this._sortColumn = sortColumn;
            this._sortDirection = sortDirection;
            if (colChanged) {
                this.raisePropertyChanged('sortColumn');
            }
            if (dirChanged) {
                this.raisePropertyChanged('sortDirection');
            }
            this._sorted = false;
            this.set_pageIndex(0, true);
            this.raisePropertyChanged('filteredData');
        }
    }
    function Sys$Preview$Data$DataView$_raiseFilterChanged(filter) {
                this._dataChangedDelegate(this, Sys.EventArgs.Empty);
    }
    function Sys$Preview$Data$DataView$compareRows(row1, row2) {
                                var sortColumn = this.get_sortColumn();
        var sortDirection = this.get_sortDirection();
        if (row1.getProperty(sortColumn) === row2.getProperty(sortColumn)) return 0;
        if (row1.getProperty(sortColumn) < row2.getProperty(sortColumn)) {
            return (sortDirection === Sys.Preview.Data.SortDirection.Ascending) ? -1 : 1;
        }
        return (sortDirection === Sys.Preview.Data.SortDirection.Ascending) ? 1 : -1;
    }
    function Sys$Preview$Data$DataView$onDataChanged(sender, args) {
                        if(this._disposed) return;
        if (args !== Sys.EventArgs.Empty) {
            var item = args.get_changedItem();
            var filters = this.get_filters();
            if (item && !this.isValidAfterFiltering.call(item, filters, filters.length)) {
                                return;
            }
                    }
        this._filteredTable = null;
        this.raisePropertyChanged('filteredData');
    }
    function Sys$Preview$Data$DataView$ensureFilteredData() {
        if (this._updating || !this._data) return;
        this._updating = true;
        var oldState = this.prepareChange();
        if ((typeof(this._data.length) === "number") && (this._data.length === 0)) {
            this._filteredRows = [];
            this._paginatedRows = [];
            this._filteredTable = new Sys.Preview.Data.DataRowCollection([], this._data);
            this._filteredTable.initialize();
            this._sorted = true;
        }
        else {
            if (!this._filteredTable) {
                this._filteredRows = [];
                this._paginatedRows = null;
                this._filteredTable = null;
                var filters = this.get_filters();
                var filterLength = filters.length;
                var dataLength = this._data.get_length ? this._data.get_length() : (typeof(this._data.length) !== 'undefined' ? this._data.length : 0);
                for (var i = 0; i < dataLength; i++) {
                    var item = this._data.getItem? this._data.getItem(i) : this._data[i];
                    if (!Sys.Preview.Data.DataRow.isInstanceOfType(item)) {
                        item = new Sys.Preview.Data.DataRow(item, null, i);
                    }
                    if (this.isValidAfterFiltering.call(this, item, filters, filterLength)) {
                        var rv = new Sys.Preview.Data.DataRowView(item, i);
                        rv.initialize();
                        Array.add(this._filteredRows, rv);
                    }
                }
            }
            if (!this._sorted && this._sortColumn && (this._filteredRows.length !== 0)) {
                if (!this._compareRowsDelegate) {
                    this._compareRowsDelegate = Function.createDelegate(this, this.compareRows);
                }
                this._filteredRows.sort(this._compareRowsDelegate);
                for (var i = this._filteredRows.length - 1; i >= 0; i--) {
                    this._filteredRows[i]._set_index(i);
                }
                this._sorted = true;
                this._paginatedRows = null;
                this._filteredTable = null;
            }
            if ((this._pageSize > 0) && !this._paginatedRows) {
                this._paginatedRows = [];
                this._filteredTable = null;
                var len = this._filteredRows.length;
                var start = this._pageSize * this._pageIndex;
                if (len && (start >= len)) {
                    this._pageIndex = Math.floor(len / this._pageSize) - 1;
                    start = this._pageSize * this._pageIndex;
                }
                var end = start + this._pageSize;
                for(var i = start; (i < end) && (i < len); i++) {
                    this._filteredRows[i]._set_index(i);
                    Array.add(this._paginatedRows, this._filteredRows[i]);
                }
            }
            else {
                this._paginatedRows = this._filteredRows;
            }
            if (!this._filteredTable) {
                this._filteredTable = new Sys.Preview.Data.DataRowCollection(this._paginatedRows, this._data);
                this._filteredTable.initialize();
            }
        }
        this.triggerChangeEvents(oldState, true);
        this._updating = false;
    }
    function Sys$Preview$Data$DataView$isValidAfterFiltering(row, filters, filterLength) {
                                        for (var j = 0; j < filterLength; j++) {
            if (!filters[j].filter(row)) {
                return false;
            }
        }
        return true;
    }
    function Sys$Preview$Data$DataView$triggerChangeEvents(oldState, lengthCanChange) {
                        var count;
        var pageIndex = this.get_pageIndex();
        if (lengthCanChange) {
            if (this.get_pageCount() !== oldState.pageCount) {
                this.raisePropertyChanged('pageCount');
            }
            if (this.get_length() !== oldState.length) {
                this.raisePropertyChanged('length');
            }
            count = this.get_pageCount();
            if (pageIndex >= count) {
                pageIndex = (count > 0 ? count - 1 : 0);
                this.set_pageIndex(pageIndex);
            }
        }
        else {
            count = oldState.pageCount;
        }
        if (pageIndex !== oldState.pageIndex) {
            this.raisePropertyChanged('pageIndex');
        }
        if ((pageIndex < count - 1) !== oldState.hasNextPage) {
            this.raisePropertyChanged('hasNextPage');
        }
        if ((pageIndex > 0) !== oldState.hasPreviousPage) {
            this.raisePropertyChanged('hasPreviousPage');
        }
    }
    function Sys$Preview$Data$DataView$prepareChange() {
                return {pageCount: this.get_pageCount(),
                pageIndex: this.get_pageIndex(),
                length: this.get_length(),
                hasNextPage: this.get_hasNextPage(),
                hasPreviousPage: this.get_hasPreviousPage()};
    }
Sys.Preview.Data.DataView.prototype = {
        _data: null,
    _filteredTable: null,
    _filteredRows: null,
    _paginatedRows: null,
    _pageSize: 0,
    _pageIndex: 0,
    _sorted: false,
    _sortColumn: '',
    _sortDirection: Sys.Preview.Data.SortDirection.Ascending,
    _filters: null,
    _dataChangedDelegate: null,
    _compareRowsDelegate: null,
    _updating: false,
    get_data: Sys$Preview$Data$DataView$get_data,
    set_data: Sys$Preview$Data$DataView$set_data,
    get_filteredData: Sys$Preview$Data$DataView$get_filteredData,
    get_filters: Sys$Preview$Data$DataView$get_filters,
    get_hasNextPage: Sys$Preview$Data$DataView$get_hasNextPage,
    get_hasPreviousPage: Sys$Preview$Data$DataView$get_hasPreviousPage,
    get_length: Sys$Preview$Data$DataView$get_length,
    get_pageCount: Sys$Preview$Data$DataView$get_pageCount,
    get_pageIndex: Sys$Preview$Data$DataView$get_pageIndex,
            set_pageIndex: Sys$Preview$Data$DataView$set_pageIndex,
    get_pageSize: Sys$Preview$Data$DataView$get_pageSize,
    set_pageSize: Sys$Preview$Data$DataView$set_pageSize,
    get_sortColumn: Sys$Preview$Data$DataView$get_sortColumn,
    set_sortColumn: Sys$Preview$Data$DataView$set_sortColumn,
    get_sortDirection: Sys$Preview$Data$DataView$get_sortDirection,
    set_sortDirection: Sys$Preview$Data$DataView$set_sortDirection,
    dispose: Sys$Preview$Data$DataView$dispose,
    getItem: Sys$Preview$Data$DataView$getItem,
    initialize: Sys$Preview$Data$DataView$initialize,
    sort: Sys$Preview$Data$DataView$sort,
    _raiseFilterChanged: Sys$Preview$Data$DataView$_raiseFilterChanged,
    compareRows: Sys$Preview$Data$DataView$compareRows,
        onDataChanged: Sys$Preview$Data$DataView$onDataChanged,
    ensureFilteredData: Sys$Preview$Data$DataView$ensureFilteredData,
    isValidAfterFiltering: Sys$Preview$Data$DataView$isValidAfterFiltering,
    triggerChangeEvents: Sys$Preview$Data$DataView$triggerChangeEvents,
    prepareChange: Sys$Preview$Data$DataView$prepareChange
}
Sys.Preview.Data.DataView.registerClass('Sys.Preview.Data.DataView', Sys.Component);
Sys.Preview.Data.DataView.descriptor = {
    properties: [ { name: 'data', type: Sys.Preview.Data.DataTable },
                  { name: 'filteredData', type: Sys.Preview.Data.DataTable, readOnly: true },
                  { name: 'filters', type: Array, readOnly: true },
                  { name: 'hasNextPage', type: Boolean, readOnly: true},
                  { name: 'hasPreviousPage', type: Boolean, readOnly: true },
                  { name: 'length', type: Number, readOnly: true },
                  { name: 'pageCount', type: Number, readOnly: true },
                  { name: 'pageIndex', type: Number },
                  { name: 'pageSize', type: Number },
                  { name: 'sortColumn', type: String },
                  { name: 'sortDirection', type: Sys.Preview.Data.SortDirection } ],
    methods: [ { name: 'sort', params: [ {name: 'sortColumn', type: String},
                                         {name: 'sortDirection', type: Sys.Preview.Data.SortDirection} ] } ]
}
Sys.Preview.Data.DataFilter = function Sys$Preview$Data$DataFilter() {
    Sys.Preview.Data.DataFilter.initializeBase(this);
}
    function Sys$Preview$Data$DataFilter$filter(value) {
                        throw Error.notImplemented();
    }
    function Sys$Preview$Data$DataFilter$get_dataContext() {
    if (arguments.length !== 0) throw Error.parameterCount();
                var dc = Sys.Preview.Data.DataFilter.callBaseMethod(this, 'get_dataContext');
        if (!dc) {
            if (this.owner) {
                dc = this.owner.get_dataContext();
            }
        }
        return dc;
    }
    function Sys$Preview$Data$DataFilter$dispose() {
        this.owner = null;
        Sys.Preview.Data.DataFilter.callBaseMethod(this, 'dispose');
    }
    function Sys$Preview$Data$DataFilter$raisePropertyChanged(propertyName) {
                Sys.Preview.Data.DataFilter.callBaseMethod(this, 'raisePropertyChanged', [propertyName]);
        if (this.owner) {
            this.owner._raiseFilterChanged(this);
        }
    }
    function Sys$Preview$Data$DataFilter$setOwner(owner) {
                this.owner = owner;
    }
Sys.Preview.Data.DataFilter.prototype = {
        filter: Sys$Preview$Data$DataFilter$filter,
    get_dataContext: Sys$Preview$Data$DataFilter$get_dataContext,
    dispose: Sys$Preview$Data$DataFilter$dispose,
    raisePropertyChanged: Sys$Preview$Data$DataFilter$raisePropertyChanged,
    setOwner: Sys$Preview$Data$DataFilter$setOwner
}
Sys.Preview.Data.DataFilter.registerClass('Sys.Preview.Data.DataFilter', Sys.Component);
Sys.Preview.Data.PropertyFilter = function Sys$Preview$Data$PropertyFilter() {
    Sys.Preview.Data.PropertyFilter.initializeBase(this);
}
    function Sys$Preview$Data$PropertyFilter$get_property() {
    if (arguments.length !== 0) throw Error.parameterCount();
                return this._property;
    }
    function Sys$Preview$Data$PropertyFilter$set_property(name) {
        this._property = name;
        this.raisePropertyChanged('property');
    }
    function Sys$Preview$Data$PropertyFilter$get_value() {
    if (arguments.length !== 0) throw Error.parameterCount();
                return this._value;
    }
    function Sys$Preview$Data$PropertyFilter$set_value(value) {
        this._value = value;
        this.raisePropertyChanged('value');
    }
    function Sys$Preview$Data$PropertyFilter$filter(item) {
                        return Sys.Preview.TypeDescriptor.getProperty(item, this._property) === this._value;
    }
Sys.Preview.Data.PropertyFilter.prototype = {
    _property: null,
    _value: null,
    get_property: Sys$Preview$Data$PropertyFilter$get_property,
    set_property: Sys$Preview$Data$PropertyFilter$set_property,
    get_value: Sys$Preview$Data$PropertyFilter$get_value,
    set_value: Sys$Preview$Data$PropertyFilter$set_value,
    filter: Sys$Preview$Data$PropertyFilter$filter
}
Sys.Preview.Data.PropertyFilter.registerClass('Sys.Preview.Data.PropertyFilter', Sys.Preview.Data.DataFilter);
Sys.Preview.Data.PropertyFilter.descriptor = {
    properties: [ { name: 'property', type: String },
                  { name: 'value' } ]
}
Sys.Preview.Data.DataSource = function Sys$Preview$Data$DataSource() {
                        Sys.Preview.Data.DataSource.initializeBase(this);
    this._parameters = {};
}
    function Sys$Preview$Data$DataSource$add_dataAvailable(handler) {
    var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
    if (e) throw e;
                this.get_events().addHandler("dataAvailable", handler);
    }
    function Sys$Preview$Data$DataSource$remove_dataAvailable(handler) {
    var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
    if (e) throw e;
                this.get_events().removeHandler("dataAvailable", handler);
    }
    function Sys$Preview$Data$DataSource$_onDataAvailable() {
        var handler = this.get_events().getHandler("dataAvailable");
        if (handler) {
            handler(this, Sys.EventArgs.Empty);
        }
    }
    function Sys$Preview$Data$DataSource$get_autoLoad() {
    if (arguments.length !== 0) throw Error.parameterCount();
                return this._autoLoad;
    }
    function Sys$Preview$Data$DataSource$set_autoLoad(value) {
        this._autoLoad = value;
    }
    function Sys$Preview$Data$DataSource$get_data() {
    if (arguments.length !== 0) throw Error.parameterCount();
                return this._data;
    }
    function Sys$Preview$Data$DataSource$set_data(data) {
                if(data && Object.getTypeName(data) === 'Object') {
            data = Sys.Preview.Data.DataTable.parseFromJson(data);
        }
        
        var oldIsDirtyAndReady = this.get_isDirtyAndReady();
        var oldIsReady = this.get_isReady();
        var oldRowCount = this.get_rowCount();
        if (this._data && this._dataChangedDelegate) {
            this._data.remove_propertyChanged(this._dataChangedDelegate);
        }
        if (data instanceof Array) {
            data = new Sys.Preview.Data.DataTable([], data);
        }
        this._data = data;
        if (this._data) {
            if (!this._dataChangedDelegate) {
                this._dataChangedDelegate = Function.createDelegate(this, this.onDataPropertyChanged);
            }
            this._data.add_propertyChanged(this._dataChangedDelegate);
        }
        this.raisePropertyChanged('data');
        if (oldIsDirtyAndReady !== this.get_isDirtyAndReady()) {
            this.raisePropertyChanged('isDirtyAndReady');
        }
        if (oldIsReady !== this.get_isReady()) {
            this.raisePropertyChanged('isReady');
        }
        if (oldRowCount !== this.get_rowCount()) {
            this.raisePropertyChanged('rowCount');
        }
    }
    function Sys$Preview$Data$DataSource$get_initialData() {
    if (arguments.length !== 0) throw Error.parameterCount();
                return this._initialData;
    }
    function Sys$Preview$Data$DataSource$set_initialData(value) {
        if (!this._data) {
            if (this.get_isInitialized()) {
                var data = null;
                if (value && (value.length)) {
                    data = Sys.Serialization.JavaScriptSerializer.deserialize(value);
                }
                this.set_data(data);
            }
            else {
                this._initialData = value;
            }
        }
    }
    function Sys$Preview$Data$DataSource$get_isDirtyAndReady() {
    if (arguments.length !== 0) throw Error.parameterCount();
                return this._isReady && this._data && this._data.get_isDirty();
    }
    function Sys$Preview$Data$DataSource$get_isReady() {
    if (arguments.length !== 0) throw Error.parameterCount();
                return this._isReady;
    }
    function Sys$Preview$Data$DataSource$_set_isReady(value) {
        if (this._isReady !== value) {
            var oldDirtyAndReady = this.get_isDirtyAndReady();
            this._isReady = value;
            this.raisePropertyChanged("isReady");
            if (this.get_isDirtyAndReady() !== oldDirtyAndReady) {
                this.raisePropertyChanged("isDirtyAndReady");
            }
        }
    }
    function Sys$Preview$Data$DataSource$get_loadMethod() {
    if (arguments.length !== 0) throw Error.parameterCount();
                return this._loadMethod;
    }
    function Sys$Preview$Data$DataSource$set_loadMethod(value) {
        this._loadMethod = value;
    }
    function Sys$Preview$Data$DataSource$get_parameters() {
    if (arguments.length !== 0) throw Error.parameterCount();
                return this._parameters;
    }
    function Sys$Preview$Data$DataSource$get_serviceURL() {
    if (arguments.length !== 0) throw Error.parameterCount();
                return this._serviceURL;
    }
    function Sys$Preview$Data$DataSource$set_serviceURL(url) {
        this._serviceURL = url;
    }
    function Sys$Preview$Data$DataSource$get_serviceType() {
    if (arguments.length !== 0) throw Error.parameterCount();
                return this._serviceType;
    }
    function Sys$Preview$Data$DataSource$set_serviceType(value) {
        this._serviceType = value;
    }
    function Sys$Preview$Data$DataSource$get_rowCount() {
    if (arguments.length !== 0) throw Error.parameterCount();
                if (this._data) {
            return this._data.get_length();
        }
        return 0;
    }
    function Sys$Preview$Data$DataSource$dispose() {
        if (this._data) {
            this._data.dispose();
        }
        this._data = null;
        Sys.Preview.Data.DataSource.callBaseMethod(this, 'dispose');
    }
    function Sys$Preview$Data$DataSource$initialize() {
        Sys.Preview.Data.DataSource.callBaseMethod(this, 'initialize');
        if (this._autoLoad || this._initialData) {
            this.load();
        }
    }
    function Sys$Preview$Data$DataSource$onDataPropertyChanged(sender, args) {
                        switch(args.get_propertyName()) {
            case "isDirty":
                this.raisePropertyChanged("isDirtyAndReady");
                break;
            case "length":
                this.raisePropertyChanged("rowCount");
                break;
        }
    }
    function Sys$Preview$Data$DataSource$onRequestComplete(response, eventArgs) {
                        this.onLoadComplete(response.get_object());
    }
    function Sys$Preview$Data$DataSource$onLoadComplete(rawData, userContext, methodName) {
                                var oldDirtyAndReady = this.get_isDirtyAndReady();
        this.set_data(eval(rawData));
        this._isReady = true;
        this.raisePropertyChanged("isReady");
        if (this.get_isDirtyAndReady() !== oldDirtyAndReady) {
            this.raisePropertyChanged("isDirtyAndReady");
        }
        this._onDataAvailable();
    }
    function Sys$Preview$Data$DataSource$ready() {
        this._set_isReady(true);
    }
    function Sys$Preview$Data$DataSource$load() {
                                                        if (this._initialData) {
            this.set_data(Sys.Serialization.JavaScriptSerializer.deserialize(this._initialData));
            this._initialData = null;
            return;
        }
        this._set_isReady(false);
        if (this._serviceType === Sys.Preview.Data.ServiceType.DataService) {
            var method = "GetData";
            var params = {parameters: this._parameters, loadMethod: this._loadMethod};
            var onComplete = Function.createDelegate(this, this.onLoadComplete);
            var onError = Function.createDelegate(this, this.ready); 
            this._request = Sys.Net.WebServiceProxy.invoke(this._serviceURL, method, false, params, onComplete, onError, this, this._timeout);
        }
        else {
            var onComplete = Function.createDelegate(this, this.onRequestComplete);
            var onErrorOrTimeout = Function.createDelegate(this, this.ready);             var url = Sys.Net.WebRequest._createUrl(this._serviceURL, this._parameters);
            var request = new Sys.Net.WebRequest();
            request.set_url(url);
            request.add_completed(function(response, eventArgs) {
                if (response.get_responseAvailable()) {
                    var statusCode = response.get_statusCode();
                    if (statusCode >= 200 || statusCode < 300) {
                        onComplete(response, eventArgs);
                    }
                    else {
                        onErrorOrTimeout();
                    }
                }
            });
            request.invoke();
        }
    }
    function Sys$Preview$Data$DataSource$save() {
        if (this._data && this._data.get_isDirty()) {
            var changes = this._data.getChanges();
            this._set_isReady(false);
            if (this._serviceType === Sys.Preview.Data.ServiceType.DataService) {
                var method = "SaveData";
                var params = {changeList: changes, parameters: this._parameters, loadMethod: this._loadMethod};
                var onComplete = Function.createDelegate(this, this.onLoadComplete);
                var onError = Function.createDelegate(this, this.ready);                 this._request = Sys.Net.WebServiceProxy.invoke(this._serviceURL, method, false, params, onComplete, onError, this, this._timeout);
            }
            else {
                
            }
        }
    }
Sys.Preview.Data.DataSource.prototype = {
    _data: null,
    _initialData: null,
    _autoLoad: false,
    _serviceURL: "",
    _loadMethod: "",
    _serviceType: Sys.Preview.Data.ServiceType.DataService,
    _isReady: true,
    _dataChangedDelegate: null,
    _request: null,
    _timeout: 0,
    add_dataAvailable: Sys$Preview$Data$DataSource$add_dataAvailable,
    remove_dataAvailable: Sys$Preview$Data$DataSource$remove_dataAvailable,
    _onDataAvailable: Sys$Preview$Data$DataSource$_onDataAvailable,
    get_autoLoad: Sys$Preview$Data$DataSource$get_autoLoad,
    set_autoLoad: Sys$Preview$Data$DataSource$set_autoLoad,
    get_data: Sys$Preview$Data$DataSource$get_data,
    set_data: Sys$Preview$Data$DataSource$set_data,
    get_initialData: Sys$Preview$Data$DataSource$get_initialData,
    set_initialData: Sys$Preview$Data$DataSource$set_initialData,
    get_isDirtyAndReady: Sys$Preview$Data$DataSource$get_isDirtyAndReady,
    get_isReady: Sys$Preview$Data$DataSource$get_isReady,
    _set_isReady: Sys$Preview$Data$DataSource$_set_isReady,
    get_loadMethod: Sys$Preview$Data$DataSource$get_loadMethod,
    set_loadMethod: Sys$Preview$Data$DataSource$set_loadMethod,
    get_parameters: Sys$Preview$Data$DataSource$get_parameters,
    get_serviceURL: Sys$Preview$Data$DataSource$get_serviceURL,
    set_serviceURL: Sys$Preview$Data$DataSource$set_serviceURL,
    get_serviceType: Sys$Preview$Data$DataSource$get_serviceType,
    set_serviceType: Sys$Preview$Data$DataSource$set_serviceType,
    get_rowCount: Sys$Preview$Data$DataSource$get_rowCount,
    dispose: Sys$Preview$Data$DataSource$dispose,
    initialize: Sys$Preview$Data$DataSource$initialize,
    onDataPropertyChanged: Sys$Preview$Data$DataSource$onDataPropertyChanged,
    onRequestComplete: Sys$Preview$Data$DataSource$onRequestComplete,
    onLoadComplete: Sys$Preview$Data$DataSource$onLoadComplete,
    ready: Sys$Preview$Data$DataSource$ready,
    load: Sys$Preview$Data$DataSource$load,
    save: Sys$Preview$Data$DataSource$save
}
Sys.Preview.Data.DataSource.registerClass('Sys.Preview.Data.DataSource', Sys.Component);
Sys.Preview.Data.DataSource.descriptor = {
    properties: [ { name: 'data', type: Object },
                  { name: 'autoLoad', type: Boolean },
                  { name: 'initialData', type: String },
                  { name: 'isDirtyAndReady', type: Boolean, readOnly: true },
                  { name: 'isReady', type: Boolean, readOnly: true },
                  { name: 'loadMethod', type: String },
                  { name: 'rowCount', type: Number, readOnly: true },
                  { name: 'serviceURL', type: String },
                  { name: 'parameters', type: Object, readOnly: true },
                  { name: 'serviceType', type: Sys.Preview.Data.ServiceType } ],
    methods: [ { name: 'load' },
               { name: 'save' } ],
    events: [ { name: 'dataAvailable', readOnly: true } ]
}
Sys.Preview.Data.XMLDataSource = function Sys$Preview$Data$XMLDataSource() {
    Sys.Preview.Data.XMLDataSource.initializeBase(this);
}
    function Sys$Preview$Data$XMLDataSource$add_documentAvailable(handler) {
    var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
    if (e) throw e;
                this.get_events().addHandler("documentAvailable", handler);
    }
    function Sys$Preview$Data$XMLDataSource$remove_documentAvailable(handler) {
    var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
    if (e) throw e;
                this.get_events().removeHandler("documentAvailable", handler);
    }
    function Sys$Preview$Data$XMLDataSource$_onDocumentAvailable() {
        var handler = this.get_events().getHandler("documentAvailable");
        if (handler) {
            handler(this, Sys.EventArgs.Empty);
        }
    }
    function Sys$Preview$Data$XMLDataSource$get_autoLoad() {
    if (arguments.length !== 0) throw Error.parameterCount();
                return this._autoLoad;
    }
    function Sys$Preview$Data$XMLDataSource$set_autoLoad(value) {
                this._autoLoad = value;
    }
    function Sys$Preview$Data$XMLDataSource$get_document() {
    if (arguments.length !== 0) throw Error.parameterCount();
                return this._document;
    }
    function Sys$Preview$Data$XMLDataSource$get_data() {
    if (arguments.length !== 0) throw Error.parameterCount();
                return this._data;
    }
    function Sys$Preview$Data$XMLDataSource$get_initialDocument() {
    if (arguments.length !== 0) throw Error.parameterCount();
                return this._initialDocument;
    }
    function Sys$Preview$Data$XMLDataSource$set_initialDocument(value) {
                if (!this._document) {
            var document;
            if (Sys.Net.XMLDOM) {
                document = new Sys.Net.XMLDOM(value.trim());
            }
            else {
                                document = new XMLDOM(value.trim());
            }
            if (this.get_isInitialized()) {
                this._setDocument(document);
            }
            else {
                this._initialDocument = document;
            }
        }
    }
    function Sys$Preview$Data$XMLDataSource$get_isReady() {
    if (arguments.length !== 0) throw Error.parameterCount();
                return this._isReady;
    }
    function Sys$Preview$Data$XMLDataSource$get_parameters() {
    if (arguments.length !== 0) throw Error.parameterCount();
                if (this._parameters === null) {
            this._parameters = {};
        }
        return this._parameters;
    }
    function Sys$Preview$Data$XMLDataSource$get_serviceURL() {
    if (arguments.length !== 0) throw Error.parameterCount();
                return this._serviceURL;
    }
    function Sys$Preview$Data$XMLDataSource$set_serviceURL(value) {
                this._serviceURL = value;
    }
    function Sys$Preview$Data$XMLDataSource$get_xpath() {
    if (arguments.length !== 0) throw Error.parameterCount();
                return this._xpath;
    }
    function Sys$Preview$Data$XMLDataSource$set_xpath(value) {
                if (this._xpath !== value) {
            this._xpath = value;
            if (this._document) {
                this._updateData();
            }
        }
    }
    function Sys$Preview$Data$XMLDataSource$dispose() {
        this._document = null;
        this._initialDocument = null;
        this._data = null;
        Sys.Preview.Data.XMLDataSource.callBaseMethod(this, 'dispose');
    }
    function Sys$Preview$Data$XMLDataSource$initialize() {
        Sys.Preview.Data.XMLDataSource.callBaseMethod(this, 'initialize');
        if (this._autoLoad) {
            this.load();
        }
    }
    function Sys$Preview$Data$XMLDataSource$load() {
        if (this._initialDocument) {
            var document = this._initialDocument;
            this._initialDocument = null;
            this._setDocument(document);
            this._updateReady(true);
        }
        else {
            this._invokeService();
        }
    }
    function Sys$Preview$Data$XMLDataSource$_invokeService() {
        var onComplete = Function.createDelegate(this, this._serviceCompleted);
        var onErrorOrTimeout = Function.createDelegate(this, this._serviceTimeout);
        var url = Sys.Net.WebRequest._createUrl(this._serviceURL, this.get_parameters());
        var request = new Sys.Net.WebRequest();
        request.set_url(url);
        request.add_completed(function(response, eventArgs) {
            if (response.get_responseAvailable()) {
                var statusCode = response.get_statusCode();
                if (statusCode >= 200 || statusCode < 300) {
                    onComplete(response, eventArgs);
                }
                else {
                    onErrorOrTimeout();
                }
            }
        });
        request.invoke();
        this._updateReady(false);
    }
    function Sys$Preview$Data$XMLDataSource$_serviceCompleted(sender, eventArgs) {
                        if (sender.get_statusCode() === 200) {
            this._setDocument(sender.get_xml());
        }
        this._updateReady(true);
    }
    function Sys$Preview$Data$XMLDataSource$_serviceTimeout(sender, eventArgs) {
                        this._updateReady(true);
    }
    function Sys$Preview$Data$XMLDataSource$_setDocument(document) {
                this._document = document;
        this._updateData();
        this.raisePropertyChanged('document');
        this._onDocumentAvailable();
    }
    function Sys$Preview$Data$XMLDataSource$_updateData() {
        var xpath = this._xpath;
        if (!xpath || !xpath.length) {
                        xpath = '*/*';         }
        var nodes = this._document.selectNodes(xpath);
                                var data = [];
        for (var i = 0; i < nodes.length; i++) {
            var node = nodes[i];
            if (!node || (node.nodeType !== 1)) {
                continue;
            }
            Array.add(data, node);
        }
        this._data = data;
        this.raisePropertyChanged('data');
    }
    function Sys$Preview$Data$XMLDataSource$_updateReady(ready) {
                this._isReady = ready;
        this.raisePropertyChanged('isReady');
    }
Sys.Preview.Data.XMLDataSource.prototype = {
    _document: null,
    _initialDocument: null,
    _data: null,
    _xpath: '',
    _serviceURL: null,
    _parameters: null,
    _isReady: false,
    _autoLoad: false,
    add_documentAvailable: Sys$Preview$Data$XMLDataSource$add_documentAvailable,
    remove_documentAvailable: Sys$Preview$Data$XMLDataSource$remove_documentAvailable,
    _onDocumentAvailable: Sys$Preview$Data$XMLDataSource$_onDocumentAvailable,
    get_autoLoad: Sys$Preview$Data$XMLDataSource$get_autoLoad,
    set_autoLoad: Sys$Preview$Data$XMLDataSource$set_autoLoad,
    get_document: Sys$Preview$Data$XMLDataSource$get_document,
    get_data: Sys$Preview$Data$XMLDataSource$get_data,
    get_initialDocument: Sys$Preview$Data$XMLDataSource$get_initialDocument,
    set_initialDocument: Sys$Preview$Data$XMLDataSource$set_initialDocument,
    get_isReady: Sys$Preview$Data$XMLDataSource$get_isReady,
    get_parameters: Sys$Preview$Data$XMLDataSource$get_parameters,
    get_serviceURL: Sys$Preview$Data$XMLDataSource$get_serviceURL,
    set_serviceURL: Sys$Preview$Data$XMLDataSource$set_serviceURL,
    get_xpath: Sys$Preview$Data$XMLDataSource$get_xpath,
    set_xpath: Sys$Preview$Data$XMLDataSource$set_xpath,
    dispose: Sys$Preview$Data$XMLDataSource$dispose,
    initialize: Sys$Preview$Data$XMLDataSource$initialize,
    load: Sys$Preview$Data$XMLDataSource$load,
    _invokeService: Sys$Preview$Data$XMLDataSource$_invokeService,
    _serviceCompleted: Sys$Preview$Data$XMLDataSource$_serviceCompleted,
    _serviceTimeout: Sys$Preview$Data$XMLDataSource$_serviceTimeout,
    _setDocument: Sys$Preview$Data$XMLDataSource$_setDocument,
    _updateData: Sys$Preview$Data$XMLDataSource$_updateData,
    _updateReady: Sys$Preview$Data$XMLDataSource$_updateReady
}
Sys.Preview.Data.XMLDataSource.registerClass('Sys.Preview.Data.XMLDataSource', Sys.Component);
Sys.Preview.Data.XMLDataSource.descriptor = {
    properties: [ { name: 'autoLoad', type: Boolean },
                  { name: 'data', type: Object, readOnly: true },
                  { name: 'document', type: Object, readOnly: true },
                  { name: 'initialDocument', type: String },
                  { name: 'isReady', type: Boolean, readOnly: true },
                  { name: 'parameters', type: Object, readOnly: true },
                  { name: 'serviceURL', type: String },
                  { name: 'xpath', type: String } ],
    events: [ { name: 'documentAvailable', readOnly: true } ],
    methods: [ { name: 'load' } ]
}
Type.registerNamespace('Sys.Preview.UI');
Sys.Preview.UI.CommandEventArgs = function Sys$Preview$UI$CommandEventArgs(commandName, argument) {
    /// <param name="commandName" type="String" mayBeNull="true"></param>
    /// <param name="argument" type="String" mayBeNull="true" optional="true"></param>
    var e = Function._validateParams(arguments, [
        {name: "commandName", type: String, mayBeNull: true},
        {name: "argument", type: String, mayBeNull: true, optional: true}
    ]);
    if (e) throw e;
    Sys.Preview.UI.CommandEventArgs.initializeBase(this);
    
    this._commandName = commandName;
    this._argument = argument;
}
    function Sys$Preview$UI$CommandEventArgs$get_argument() {
        /// <value tyep="String" mayBeNull="true"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._argument;
    }
    function Sys$Preview$UI$CommandEventArgs$get_commandName() {
        /// <value type="String" mayBeNull="true"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._commandName;
    }
Sys.Preview.UI.CommandEventArgs.prototype = {
    get_argument: Sys$Preview$UI$CommandEventArgs$get_argument,    
    get_commandName: Sys$Preview$UI$CommandEventArgs$get_commandName
}
Sys.Preview.UI.CommandEventArgs.registerClass('Sys.Preview.UI.CommandEventArgs', Sys.EventArgs);
Sys.Preview.UI.CommandEventArgs.descriptor = {
    properties: [   {name: 'argument', type: String, readOnly: true},
                    {name: 'commandName', type: String, readOnly: true} ]
}
Sys.Preview.UI.ITemplate = function Sys$Preview$UI$ITemplate() {
    throw Error.notImplemented();
}
    function Sys$Preview$UI$ITemplate$createInstance() {
        throw Error.notImplemented();
    }
    function Sys$Preview$UI$ITemplate$initialize() {
        throw Error.notImplemented();
    }
Sys.Preview.UI.ITemplate.prototype = {
    createInstance: Sys$Preview$UI$ITemplate$createInstance,
    
    initialize: Sys$Preview$UI$ITemplate$initialize
}
Sys.Preview.UI.ITemplate.registerInterface('Sys.Preview.UI.ITemplate');
Sys.Preview.UI.ITemplate.disposeInstance = function Sys$Preview$UI$ITemplate$disposeInstance(container) {
    /// <param name="container"></param>
    var e = Function._validateParams(arguments, [
        {name: "container"}
    ]);
    if (e) throw e;
    if (container.markupContext) {
        container.markupContext.dispose();
        container.markupContext = null;
    }
}
Sys.Preview.UI.Template = function Sys$Preview$UI$Template(layoutElement, scriptNode, parentMarkupContext) {
                Sys.Preview.UI.Template.initializeBase(this);
    this._layoutElement = layoutElement;
    this._scriptNode = scriptNode;
    this._parentMarkupContext = parentMarkupContext;
}
    function Sys$Preview$UI$Template$createInstance(containerElement, dataContext, instanceElementCreatedCallback, callbackContext) {
                                                var result = new Sys.Preview.UI.TemplateInstance();
        result.instanceElement = this._layoutElement.cloneNode(true);
        var documentFragment = document.createDocumentFragment();
        documentFragment.appendChild(result.instanceElement);
        var markupContext = Sys.Preview.MarkupContext.createLocalContext(documentFragment, this._parentMarkupContext, dataContext);
        markupContext.open();
        Sys.Preview.MarkupParser.parseNodes(this._scriptNode.childNodes, markupContext);
        if (instanceElementCreatedCallback) {
            result.callbackResult = instanceElementCreatedCallback(result.instanceElement, markupContext, callbackContext);
        }
        result.instanceElement.markupContext = markupContext;
        containerElement.appendChild(result.instanceElement);
        markupContext.close();
        return result;
    }
    function Sys$Preview$UI$Template$dispose() {
        this._layoutElement = null;
        this._scriptNode = null;
        this._parentMarkupContext = null;
    }
    function Sys$Preview$UI$Template$initialize() {
                if (this._layoutElement.parentNode) {
            this._layoutElement.parentNode.removeChild(this._layoutElement);
        }
    }
Sys.Preview.UI.Template.prototype = {
    createInstance: Sys$Preview$UI$Template$createInstance,
    dispose: Sys$Preview$UI$Template$dispose,
    initialize: Sys$Preview$UI$Template$initialize
}
Sys.Preview.UI.Template.registerClass('Sys.Preview.UI.Template', null, Sys.Preview.UI.ITemplate, Sys.IDisposable);
Sys.Preview.UI.Template.parseFromMarkup = function Sys$Preview$UI$Template$parseFromMarkup(type, node, markupContext) {
                    var layoutElementAttribute = node.attributes.getNamedItem('layoutElement');
    
    var layoutElementID = layoutElementAttribute.nodeValue;
    var layoutElement = markupContext.findElement(layoutElementID);
    
    return new Sys.Preview.UI.Template(layoutElement, node, markupContext);
}
Sys.Preview.UI.ClickBehavior = function Sys$Preview$UI$ClickBehavior(element) {
    /// <param name="element" domElement="true"></param>
    var e = Function._validateParams(arguments, [
        {name: "element", domElement: true}
    ]);
    if (e) throw e;
    Sys.Preview.UI.ClickBehavior.initializeBase(this, [element]);
}
    function Sys$Preview$UI$ClickBehavior$add_click(handler) {
        var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
        if (e) throw e;
        this.get_events().addHandler('click', handler);
    }
    function Sys$Preview$UI$ClickBehavior$remove_click(handler) {
        var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
        if (e) throw e;
        this.get_events().removeHandler('click', handler);
    }
    function Sys$Preview$UI$ClickBehavior$dispose() {
        if (this._clickHandler) {
            Sys.UI.DomEvent.removeHandler(this.get_element(), 'click', this._clickHandler);
        }
        Sys.Preview.UI.ClickBehavior.callBaseMethod(this, 'dispose');
    }
    function Sys$Preview$UI$ClickBehavior$initialize() {
        Sys.Preview.UI.ClickBehavior.callBaseMethod(this, 'initialize');
        this._clickHandler = Function.createDelegate(this, this._onClick);
        Sys.UI.DomEvent.addHandler(this.get_element(), 'click', this._clickHandler);
    }
    function Sys$Preview$UI$ClickBehavior$_onClick() {
        var handler = this.get_events().getHandler('click');
        if(handler) {
            handler(this, Sys.EventArgs.Empty);
        }
    }
Sys.Preview.UI.ClickBehavior.prototype = {
    
    _clickHandler: null,
    add_click: Sys$Preview$UI$ClickBehavior$add_click,
    
    remove_click: Sys$Preview$UI$ClickBehavior$remove_click,
    dispose: Sys$Preview$UI$ClickBehavior$dispose,
    initialize: Sys$Preview$UI$ClickBehavior$initialize,
    _onClick: Sys$Preview$UI$ClickBehavior$_onClick
}
Sys.Preview.UI.ClickBehavior.descriptor = {
    events: [ {name: 'click'} ]
}
Sys.Preview.UI.ClickBehavior.registerClass('Sys.Preview.UI.ClickBehavior', Sys.UI.Behavior);
Sys.Preview.UI.Label = function Sys$Preview$UI$Label(associatedElement) {
    /// <param name="associatedElement" domElement="true"></param>
    var e = Function._validateParams(arguments, [
        {name: "associatedElement", domElement: true}
    ]);
    if (e) throw e;
    Sys.Preview.UI.Label.initializeBase(this, [associatedElement]);
}
    function Sys$Preview$UI$Label$get_htmlEncode() {
        /// <value type="Boolean"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._htmlEncode;
    }
    function Sys$Preview$UI$Label$set_htmlEncode(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: Boolean}]);
        if (e) throw e;
        this._htmlEncode = value;
    }
    function Sys$Preview$UI$Label$get_text() {
        /// <value mayBeNull="true"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        var element = this.get_element();
        if (this._htmlEncode) {
            return element.textContent || element.innerText || '';
        }
        else {
            return element.innerHTML;
        }
    }
    function Sys$Preview$UI$Label$set_text(value) {
        var e = Function._validateParams(arguments, [{name: "value", mayBeNull: true}]);
        if (e) throw e;
        if (!value) value = "";
        var element = this.get_element();
        var hasTextContent = element.hasAttribute && element.hasAttribute('textContent');
        var previous = this._htmlEncode ? 
            (hasTextContent ?
                element.textContent : element.innerText
            ) : element.innerHTML;
        if (previous !== value) {
            if (this._htmlEncode) {
                if (hasTextContent) {
                    var sb = [],
                        prevCh,
                        current = 0;
                    for (var i = 0, l = value.length; i < l; i++) {
                        var ch = value.charAt(i), b;
                        switch (ch) {
                            case "<":
                                b = "&lt;";
                                break;
                            case ">":
                                b = "&gt;";
                                break;
                            case "\"":
                                b = "&quot;";
                                break;
                            case "&":
                                b = "&amp;";
                                break;
                            case " ":
                                if (prevCh == " ") {
                                    b = "&nbsp;";
                                }
                                break;
                            case "\n":
                                                                b = "\n<br />";
                                break;
                        }
                        if (b) {
                            sb[sb.length] = value.substring(current, i) + b;
                            current = i;
                        }
                        prevCh = ch;
                    }
                    element.innerHTML = (current === 0) ? value : sb.join();
                }
                else {
                    element.innerText = value;
                }
            }
            else {
                element.innerHTML = value;
            }
            this.raisePropertyChanged('text');
        }
    }
Sys.Preview.UI.Label.prototype = {
    _htmlEncode: false,
    get_htmlEncode: Sys$Preview$UI$Label$get_htmlEncode,
    
    set_htmlEncode: Sys$Preview$UI$Label$set_htmlEncode,
    get_text: Sys$Preview$UI$Label$get_text,
        set_text: Sys$Preview$UI$Label$set_text
    
}
Sys.Preview.UI.Label.descriptor = {
    properties: [ { name: 'htmlEncode', type: Boolean },
                  { name: 'text', type: String } ]
}
Sys.Preview.UI.Label.registerClass('Sys.Preview.UI.Label', Sys.UI.Control);
Sys.Preview.UI.Image = function Sys$Preview$UI$Image(associatedElement) {
    /// <param name="associatedElement" domElement="true"></param>
    var e = Function._validateParams(arguments, [
        {name: "associatedElement", domElement: true}
    ]);
    if (e) throw e;
    Sys.Preview.UI.Image.initializeBase(this, [associatedElement]);
}
    function Sys$Preview$UI$Image$get_alternateText() {
        /// <value type="String" mayBeNull="true"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this.get_element().alt;
    }
    function Sys$Preview$UI$Image$set_alternateText(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: String, mayBeNull: true}]);
        if (e) throw e;
        this.get_element().alt = value;
    }
    function Sys$Preview$UI$Image$get_height() {
        /// <value></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this.get_element().height;
    }
    function Sys$Preview$UI$Image$set_height(value) {
        var e = Function._validateParams(arguments, [{name: "value"}]);
        if (e) throw e;
        this.get_element().height = value;
    }
    function Sys$Preview$UI$Image$get_imageURL() {
        /// <value type="String" mayBeNull="true"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this.get_element().src;
    }
    function Sys$Preview$UI$Image$set_imageURL(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: String, mayBeNull: true}]);
        if (e) throw e;
        this.get_element().src = value;
    }
    function Sys$Preview$UI$Image$get_width() {
        /// <value></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this.get_element().width;
    }
    function Sys$Preview$UI$Image$set_width(value) {
        var e = Function._validateParams(arguments, [{name: "value"}]);
        if (e) throw e;
        this.get_element().width = value;
    }
Sys.Preview.UI.Image.prototype = {
    
    get_alternateText: Sys$Preview$UI$Image$get_alternateText,
    
    set_alternateText: Sys$Preview$UI$Image$set_alternateText,
    
    get_height: Sys$Preview$UI$Image$get_height,
    
    set_height: Sys$Preview$UI$Image$set_height,
    
    get_imageURL: Sys$Preview$UI$Image$get_imageURL,
    
    set_imageURL: Sys$Preview$UI$Image$set_imageURL,
    
    get_width: Sys$Preview$UI$Image$get_width,
    
    set_width: Sys$Preview$UI$Image$set_width
    
}
Sys.Preview.UI.Image.descriptor = {
    properties: [ { name: 'alternateText', type: String },
                  { name: 'height' },
                  { name: 'imageURL', type: String },
                  { name: 'width' } ]
}
Sys.Preview.UI.Image.registerClass('Sys.Preview.UI.Image', Sys.UI.Control);
if(Sys.Browser.agent === Sys.Browser.Safari) {
    Sys.Preview.UI.Image_ = function Sys$Preview$UI$Image_(element) { Sys.Preview.UI.Image_.initializeBase(this,[element]); }
    Sys.Preview.UI.Image_.registerClass('Sys.Preview.UI.Image_', Sys.Preview.UI.Image);
}
Sys.Preview.UI.HyperLink = function Sys$Preview$UI$HyperLink(associatedElement) {
    /// <param name="associatedElement" domElement="true"></param>
    var e = Function._validateParams(arguments, [
        {name: "associatedElement", domElement: true}
    ]);
    if (e) throw e;
    Sys.Preview.UI.HyperLink.initializeBase(this, [associatedElement]);
}
    function Sys$Preview$UI$HyperLink$get_navigateURL() {
        /// <value type="String" mayBeNull="true"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this.get_element().href;
    }
    function Sys$Preview$UI$HyperLink$set_navigateURL(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: String, mayBeNull: true}]);
        if (e) throw e;
        this.get_element().href = value? value : "";     }
    function Sys$Preview$UI$HyperLink$initialize() {
        Sys.Preview.UI.HyperLink.callBaseMethod(this, 'initialize');
        this._clickHandler = Function.createDelegate(this, this._onClick);
        Sys.UI.DomEvent.addHandler(this.get_element(), "click", this._clickHandler);
    }
    function Sys$Preview$UI$HyperLink$dispose() {
        if (this._clickHandler) {
            Sys.UI.DomEvent.removeHandler(this.get_element(), "click", this._clickHandler);
        }
        Sys.Preview.UI.HyperLink.callBaseMethod(this, 'dispose');
    }
    function Sys$Preview$UI$HyperLink$add_click(handler) {
        var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
        if (e) throw e;
        this.get_events().addHandler("click", handler);
    }
    function Sys$Preview$UI$HyperLink$remove_click(handler) {
        var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
        if (e) throw e;
        this.get_events().removeHandler("click", handler);
    }
    function Sys$Preview$UI$HyperLink$_onClick() {
        var handler = this.get_events().getHandler("click");
        if (handler) {
            handler(this, Sys.EventArgs.Empty);
        }
    }
Sys.Preview.UI.HyperLink.prototype = {
    
    _clickHandler: null,
        get_navigateURL: Sys$Preview$UI$HyperLink$get_navigateURL,
    
    set_navigateURL: Sys$Preview$UI$HyperLink$set_navigateURL,
    
    initialize: Sys$Preview$UI$HyperLink$initialize,
    dispose: Sys$Preview$UI$HyperLink$dispose,
    
    add_click: Sys$Preview$UI$HyperLink$add_click,
    remove_click: Sys$Preview$UI$HyperLink$remove_click,
    
    _onClick: Sys$Preview$UI$HyperLink$_onClick
}
Sys.Preview.UI.HyperLink.descriptor = {
    properties: [ { name: 'navigateURL', type: String } ],
    events: [ { name: 'click' } ]
}
Sys.Preview.UI.HyperLink.registerClass('Sys.Preview.UI.HyperLink', Sys.Preview.UI.Label);
Sys.Preview.UI.Button = function Sys$Preview$UI$Button(associatedElement) {
    /// <param name="associatedElement" domElement="true"></param>
    var e = Function._validateParams(arguments, [
        {name: "associatedElement", domElement: true}
    ]);
    if (e) throw e;
    Sys.Preview.UI.Button.initializeBase(this, [associatedElement]);
}
    function Sys$Preview$UI$Button$get_argument() {
        /// <value type="String" mayBeNull="true"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._arg;
    }
    function Sys$Preview$UI$Button$set_argument(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: String, mayBeNull: true}]);
        if (e) throw e;
        if (this._arg !== value) {
            this._arg = value;
            this.raisePropertyChanged('argument');
        }
    }
    function Sys$Preview$UI$Button$get_command() {
        /// <value type="String" mayBeNull="true"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._command;
    }
    function Sys$Preview$UI$Button$set_command(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: String, mayBeNull: true}]);
        if (e) throw e;
        if (this._command !== value) {
            this._command = value;
            this.raisePropertyChanged('command');
        }
    }
    function Sys$Preview$UI$Button$initialize() {
        Sys.Preview.UI.Button.callBaseMethod(this, 'initialize');
        this._clickHandler = Function.createDelegate(this, this._onClick);
        Sys.UI.DomEvent.addHandler(this.get_element(), "click", this._clickHandler);
    }
    function Sys$Preview$UI$Button$dispose() {
        if (this._clickHandler) {
            Sys.UI.DomEvent.removeHandler(this.get_element(), "click", this._clickHandler);
        }
        Sys.Preview.UI.Button.callBaseMethod(this, 'dispose');
    }
    function Sys$Preview$UI$Button$add_click(handler) {
        var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
        if (e) throw e;
        this.get_events().addHandler("click", handler);
    }
    function Sys$Preview$UI$Button$remove_click(handler) {
        var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
        if (e) throw e;
        this.get_events().removeHandler("click", handler);
    }
    function Sys$Preview$UI$Button$_onClick() {
        var handler = this.get_events().getHandler("click");
        if (handler) {
            handler(this, Sys.EventArgs.Empty);
        }
        if (this._command) {
            this.raiseBubbleEvent(this, new Sys.Preview.UI.CommandEventArgs(this._command, this._arg));
        }
    }
Sys.Preview.UI.Button.prototype = {
    _command: null,
    _arg: null,
    _clickHandler: null,
    
    get_argument: Sys$Preview$UI$Button$get_argument,
    
    set_argument: Sys$Preview$UI$Button$set_argument,
    
    get_command: Sys$Preview$UI$Button$get_command,
    
    set_command: Sys$Preview$UI$Button$set_command,
    
    initialize: Sys$Preview$UI$Button$initialize,
    dispose: Sys$Preview$UI$Button$dispose,
    
    add_click: Sys$Preview$UI$Button$add_click,
    remove_click: Sys$Preview$UI$Button$remove_click,
    
    _onClick: Sys$Preview$UI$Button$_onClick
}
    
Sys.Preview.UI.Button.descriptor = {
    properties: [ { name: 'command', type: String },
                  { name: 'argument', type: String } ],
    events: [ { name: 'click' } ]
}
Sys.Preview.UI.Button.registerClass('Sys.Preview.UI.Button', Sys.UI.Control);
Sys.Preview.UI.CheckBox = function Sys$Preview$UI$CheckBox(associatedElement) {
    /// <param name="associatedElement" domElement="true"></param>
    var e = Function._validateParams(arguments, [
        {name: "associatedElement", domElement: true}
    ]);
    if (e) throw e;
    Sys.Preview.UI.CheckBox.initializeBase(this, [associatedElement]);
}
    function Sys$Preview$UI$CheckBox$get_checked() {
        /// <value mayBeNull="true" optional="false"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
                        return !!(this.get_element().checked);
    }
    function Sys$Preview$UI$CheckBox$set_checked(value) {
        var e = Function._validateParams(arguments, [{name: "value", mayBeNull: true}]);
        if (e) throw e;
       value = !!value;         if (value !== this.get_checked()) {
            this.get_element().checked = value;
            this.raisePropertyChanged('checked');
        }
    }
    function Sys$Preview$UI$CheckBox$initialize() {
        Sys.Preview.UI.CheckBox.callBaseMethod(this, 'initialize');
        this._clickHandler = Function.createDelegate(this, this._onClick);
        Sys.UI.DomEvent.addHandler(this.get_element(), "click", this._clickHandler);
    }
    function Sys$Preview$UI$CheckBox$dispose() {
        if (this._clickHandler) {
            Sys.UI.DomEvent.removeHandler(this.get_element(), "click", this._clickHandler);
        }
        Sys.Preview.UI.CheckBox.callBaseMethod(this, 'dispose');
    }
    function Sys$Preview$UI$CheckBox$add_click(handler) {
        var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
        if (e) throw e;
        this.get_events().addHandler("click", handler);
    }
    function Sys$Preview$UI$CheckBox$remove_click(handler) {
        var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
        if (e) throw e;
        this.get_events().removeHandler("click", handler);
    }
    function Sys$Preview$UI$CheckBox$_onClick() {
        this.raisePropertyChanged('checked');
        var handler = this.get_events().getHandler("click");
        if (handler) {
            handler(this, Sys.EventArgs.Empty);
        }
    }
Sys.Preview.UI.CheckBox.prototype = {
    _clickHandler: null,
    
    get_checked: Sys$Preview$UI$CheckBox$get_checked,
    
    set_checked: Sys$Preview$UI$CheckBox$set_checked,
    
    initialize: Sys$Preview$UI$CheckBox$initialize,
    
    dispose: Sys$Preview$UI$CheckBox$dispose,
    
    add_click: Sys$Preview$UI$CheckBox$add_click,
    remove_click: Sys$Preview$UI$CheckBox$remove_click,
    
    _onClick: Sys$Preview$UI$CheckBox$_onClick
}
Sys.Preview.UI.CheckBox.descriptor = {
    properties: [ { name: 'checked' } ],
    events: [ { name: 'click' } ]
}
Sys.Preview.UI.CheckBox.registerClass('Sys.Preview.UI.CheckBox', Sys.UI.Control);
Sys.Preview.UI.TextBox = function Sys$Preview$UI$TextBox(associatedElement) {
    /// <param name="associatedElement" domElement="true"></param>
    var e = Function._validateParams(arguments, [
        {name: "associatedElement", domElement: true}
    ]);
    if (e) throw e;
    Sys.Preview.UI.TextBox.initializeBase(this, [associatedElement]);
}
    function Sys$Preview$UI$TextBox$get_text() {
        /// <value type="String" mayBeNull="true"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this.get_element().value;
    }
    function Sys$Preview$UI$TextBox$set_text(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: String, mayBeNull: true}]);
        if (e) throw e;
        var element = this.get_element();
        if(!value) value = "";
        if (element.value !== value) {
            element.value = value;
            this.raisePropertyChanged('text');
        }
    }
    function Sys$Preview$UI$TextBox$dispose() {
        if (this._changeHandler) {
            Sys.UI.DomEvent.removeHandler(this.get_element(), "change", this._changeHandler);
            this._changeHandler = null;
        }
        if (this._keyPressHandler) {
            Sys.UI.DomEvent.removeHandler(this.get_element(), "keypress", this._keyPressHandler);
            this._keyPressHandler = null;
        }
        Sys.Preview.UI.TextBox.callBaseMethod(this, 'dispose');
    }
    function Sys$Preview$UI$TextBox$_onChange() {
        var value = this.get_element().value;
        if (value !== this._text) {
            this._text = value;
            this.raisePropertyChanged('text');
        }
    }
    function Sys$Preview$UI$TextBox$_onKeyPress(e) {
                var key = e.keyCode ? e.keyCode : e.rawEvent.keyCode;
        if (key === Sys.UI.Key.enter) {
            var value = this.get_element().value;
            if (value !== this._text) {
                this._text = value;
                this.raisePropertyChanged('text');
            }
        }
    }
    function Sys$Preview$UI$TextBox$initialize() {
        Sys.Preview.UI.TextBox.callBaseMethod(this, 'initialize');
        var element = this.get_element();
        this._text = element.value;
        this._changeHandler = Function.createDelegate(this, this._onChange);
        Sys.UI.DomEvent.addHandler(element, "change", this._changeHandler);
        this._keyPressHandler = Function.createDelegate(this, this._onKeyPress);
        Sys.UI.DomEvent.addHandler(element, "keypress", this._keyPressHandler);
    }
Sys.Preview.UI.TextBox.prototype = {
    _text: null,
    _changeHandler: null,
    _keyPressHandler: null,
    get_text: Sys$Preview$UI$TextBox$get_text,
    set_text: Sys$Preview$UI$TextBox$set_text,
    dispose: Sys$Preview$UI$TextBox$dispose,
    _onChange: Sys$Preview$UI$TextBox$_onChange,
    _onKeyPress: Sys$Preview$UI$TextBox$_onKeyPress,
    initialize: Sys$Preview$UI$TextBox$initialize
}
Sys.Preview.UI.TextBox.descriptor = {
    properties: [ { name: 'text', type: String } ]
}
Sys.Preview.UI.TextBox.registerClass('Sys.Preview.UI.TextBox', Sys.UI.Control);
Sys.Preview.UI.Selector = function Sys$Preview$UI$Selector(associatedElement) {
    Sys.Preview.UI.Selector.initializeBase(this, [associatedElement]);
    this._dataChangedDelegate = Function.createDelegate(this, this.dataBind);
}
    function Sys$Preview$UI$Selector$add_selectionChanged(handler) {
        var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
        if (e) throw e;
        this.get_events().addHandler("selectionChanged", handler);
    }
    function Sys$Preview$UI$Selector$remove_selectionChanged(handler) {
        var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
        if (e) throw e;
        this.get_events().removeHandler("selectionChanged", handler);
    }
    function Sys$Preview$UI$Selector$_onSelectionChanged() {
        this.raisePropertyChanged('selectedValue');
        var handler = this.get_events().getHandler("selectionChanged");
        if (handler) {
            handler(this, Sys.EventArgs.Empty);
        }
    }
    function Sys$Preview$UI$Selector$get_data() {
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._data;
    }
    function Sys$Preview$UI$Selector$set_data(value) {
        if (this._data && Sys.Preview.INotifyCollectionChanged.isImplementedBy(this._data)) {
            this._data.remove_collectionChanged(this._dataChangedDelegate);
        }
        this._data = value;
        if (this._data) {
            if (!Sys.Preview.Data.DataTable.isInstanceOfType(this._data)) {
                if (this._data instanceof Array) {
                    this._data = new Sys.Preview.Data.DataTable([], this._data);
                }
                else if (typeof(this._data) === 'object') {
                    this._data = Sys.Preview.Data.DataTable.parseFromJson(this._data);
                }
            }
            this._data.add_collectionChanged(this._dataChangedDelegate);
        }
        this.dataBind();
        this.raisePropertyChanged('data');
    }
    function Sys$Preview$UI$Selector$get_firstItemText() {
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._firstItemText;
    }
    function Sys$Preview$UI$Selector$set_firstItemText(value) {
        if (this._firstItemText != value) {
            this._firstItemText = value;
            this.raisePropertyChanged('firstItemText');
            this.dataBind();
        }
    }
    function Sys$Preview$UI$Selector$get_selectedValue() {
        if (arguments.length !== 0) throw Error.parameterCount();
        return this.get_element().value;
    }
    function Sys$Preview$UI$Selector$set_selectedValue(value) {
        this.get_element().value = value;
    }
    function Sys$Preview$UI$Selector$get_textProperty() {
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._textProperty;
    }
    function Sys$Preview$UI$Selector$set_textProperty(value) {
        this._textProperty = value;
        this.raisePropertyChanged('textProperty');
    }
    function Sys$Preview$UI$Selector$get_valueProperty() {
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._valueProperty;
    }
    function Sys$Preview$UI$Selector$set_valueProperty(value) {
        this._valueProperty = value;
        this.raisePropertyChanged('valueProperty');
    }
    function Sys$Preview$UI$Selector$dataBind() {
        var options = this.get_element().options;
        var selectedValues = [];
        var i;
        for (i = options.length - 1; i >= 0; i--) {
            if (options[i].selected) {
                Array.add(selectedValues, options[i].value);
            }
            options[i] = null;
        }
        var option;
        if (this._firstItemText && (this._firstItemText.length != 0)) {
            option = new Option(this._firstItemText, "");
            options[this.get_element().length] = option;
        }
        if (this._data) {
            var length = this._data.get_length();
            for (i = 0; i < length; i++) {
                var item = this._data.getItem(i);
                option = new Option(Sys.Preview.TypeDescriptor.getProperty(item, this._textProperty),
                    Sys.Preview.TypeDescriptor.getProperty(item, this._valueProperty));
                option.selected = Array.contains(selectedValues, option.value);
                options[this.get_element().length] = option;
            }
        }
    }
    function Sys$Preview$UI$Selector$dispose() {
        if (this._selectionChangedHandler) {
            Sys.UI.DomEvent.removeHandler(this.get_element(), "change", this._selectionChangedHandler);
            this._selectionChangedHandler = null;
        }
        Sys.Preview.UI.Selector.callBaseMethod(this, 'dispose');
    }
    function Sys$Preview$UI$Selector$initialize() {
        Sys.Preview.UI.Selector.callBaseMethod(this, 'initialize');
        this._selectionChangedHandler = Function.createDelegate(this, this._onSelectionChanged);
        Sys.UI.DomEvent.addHandler(this.get_element(), "change", this._selectionChangedHandler);
    }
Sys.Preview.UI.Selector.prototype = {
    _selectionChangedHandler: null,
    _data: null,
    _textProperty: null,
    _valueProperty: null,
    _firstItemText: null,
    add_selectionChanged: Sys$Preview$UI$Selector$add_selectionChanged,
    remove_selectionChanged: Sys$Preview$UI$Selector$remove_selectionChanged,
    _onSelectionChanged: Sys$Preview$UI$Selector$_onSelectionChanged,
    get_data: Sys$Preview$UI$Selector$get_data,
    set_data: Sys$Preview$UI$Selector$set_data,
    get_firstItemText: Sys$Preview$UI$Selector$get_firstItemText,
    set_firstItemText: Sys$Preview$UI$Selector$set_firstItemText,
    get_selectedValue: Sys$Preview$UI$Selector$get_selectedValue,
    set_selectedValue: Sys$Preview$UI$Selector$set_selectedValue,
    get_textProperty: Sys$Preview$UI$Selector$get_textProperty,
    set_textProperty: Sys$Preview$UI$Selector$set_textProperty,
    get_valueProperty: Sys$Preview$UI$Selector$get_valueProperty,
    set_valueProperty: Sys$Preview$UI$Selector$set_valueProperty,
    dataBind: Sys$Preview$UI$Selector$dataBind,
    dispose: Sys$Preview$UI$Selector$dispose,
    initialize: Sys$Preview$UI$Selector$initialize
}
Sys.Preview.UI.Selector.registerClass('Sys.Preview.UI.Selector', Sys.UI.Control);
Sys.Preview.UI.Selector.descriptor = {
    properties: [ { name: 'data', type: Sys.Preview.Data.DataTable },
                  { name: 'firstItemText', type: String },
                  { name: 'selectedValue', type: String },
                  { name: 'textProperty', type: String },
                  { name: 'valueProperty', type: String } ],
    events: [ { name: 'selectionChanged', readOnly: true } ]
}
Sys.Preview.UI.DialogResult = function Sys$Preview$UI$DialogResult() {
    throw Error.invalidOperation();
}
Sys.Preview.UI.DialogResult.prototype = {
    OK: 0,
    Cancel: 1
}
Sys.Preview.UI.DialogResult.registerEnum('Sys.Preview.UI.DialogResult');
Sys.Preview.UI.Color = function Sys$Preview$UI$Color(r, g, b) {
    /// <param name="r" type="Number"></param>
    /// <param name="g" type="Number"></param>
    /// <param name="b" type="Number"></param>
    var e = Function._validateParams(arguments, [
        {name: "r", type: Number},
        {name: "g", type: Number},
        {name: "b", type: Number}
    ]);
    if (e) throw e;
    Sys.Preview.UI.Color.initializeBase(this);
    
    this._r = r;
    this._g = g;
    this._b = b;
}
    function Sys$Preview$UI$Color$get_blue() {
        /// <value type="Number"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._b;
    }
    function Sys$Preview$UI$Color$get_green() {
        /// <value type="Number"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._g;
    }
    function Sys$Preview$UI$Color$get_red() {
        /// <value type="Number"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._r;
    }
    function Sys$Preview$UI$Color$toString() {
        /// <returns type="String"></returns>
        if (arguments.length !== 0) throw Error.parameterCount();
        var red = this._r.toString(16);
        if (this._r < 16) {
            red = '0' + red;
        }
        var green = this._g.toString(16);
        if (this._g < 16) {
            green = '0' + green;
        }
        var blue = this._b.toString(16);
        if (this._b < 16) {
            blue = '0' + blue;
        }
        return "#" + red + green + blue;
    }
Sys.Preview.UI.Color.prototype = {
    get_blue: Sys$Preview$UI$Color$get_blue,
    
    get_green: Sys$Preview$UI$Color$get_green,
    
    get_red: Sys$Preview$UI$Color$get_red,
    
    toString: Sys$Preview$UI$Color$toString
}
Sys.Preview.UI.Color.registerClass('Sys.Preview.UI.Color');
Sys.Preview.UI.Color.parse = function Sys$Preview$UI$Color$parse(value) {
    /// <param name="value" type="String"></param>
    /// <returns type="Sys.Preview.UI.Color"></returns>
    var e = Function._validateParams(arguments, [
        {name: "value", type: String}
    ]);
    if (e) throw e;
    if (value && (value.length === 7) && value.startsWith("#")) {
        var red = parseInt('0x' + value.substr(1, 2));
        var green = parseInt('0x' + value.substr(3, 2));
        var blue = parseInt('0x' + value.substr(5, 2));
        
        return new Sys.Preview.UI.Color(red, green, blue);
    }
    return null;
}
Sys.Preview.UI.MessageBoxStyle = function Sys$Preview$UI$MessageBoxStyle() {
    throw Error.invalidOperation();
}
Sys.Preview.UI.MessageBoxStyle.prototype = {
    OK: 0,
    OKCancel: 1
}
Sys.Preview.UI.MessageBoxStyle.registerEnum('Sys.Preview.UI.MessageBoxStyle');
Sys.Preview.UI.Window = function Sys$Preview$UI$Window() {
    throw Error.invalidOperation();
}
Sys.Preview.UI.Window.messageBox = function Sys$Preview$UI$Window$messageBox(text, style) {
    if (!style) {
        style = Sys.Preview.UI.MessageBoxStyle.OK;
    }
    var result = Sys.Preview.UI.DialogResult.OK;
    switch (style) {
        case Sys.Preview.UI.MessageBoxStyle.OK:
            window.alert(text);
            break;
        case Sys.Preview.UI.MessageBoxStyle.OKCancel:
            if (window.confirm(text) === false) {
                result = Sys.Preview.UI.DialogResult.Cancel;
            }
            break;
    }
    return result;
}
Sys.Preview.UI.Window.inputBox = function Sys$Preview$UI$Window$inputBox(promptText, defaultValue) {
    if (!defaultValue) {
        defaultValue = '';
    }
    return window.prompt(promptText, defaultValue);
}
Sys.Preview.UI.TemplateInstance = function Sys$Preview$UI$TemplateInstance() {
    this.instanceElement = null;
    this.callbackResult = null;
}
Type.registerNamespace('Sys.Preview.UI.Data');
Sys.Preview.UI.Data.DataControl = function Sys$Preview$UI$Data$DataControl(associatedElement) {
        Sys.Preview.UI.Data.DataControl.initializeBase(this, [associatedElement]);
    this._dataIndex = 0; }
    function Sys$Preview$UI$Data$DataControl$prepareChange() {
                return { dataIndex: this.get_dataIndex(), canMoveNext: this.get_canMoveNext(), canMovePrevious: this.get_canMovePrevious() };
    }
    function Sys$Preview$UI$Data$DataControl$triggerChangeEvents(oldState) {
                var dataIndex = this.get_dataIndex();
        if (oldState.dataIndex !== dataIndex) {
            this.raisePropertyChanged('dataIndex');
            this.raisePropertyChanged('dataItem');
            oldState.dataIndex = dataIndex;
        }
        var canMoveNext = this.get_canMoveNext();
        if (oldState.canMoveNext !== canMoveNext) {
            this.raisePropertyChanged('canMoveNext');
            oldState.canMoveNext = canMoveNext;
        }
        var canMovePrevious = this.get_canMovePrevious();
        if (oldState.canMovePrevious !== canMovePrevious) {
            this.raisePropertyChanged('canMovePrevious');
            oldState.canMovePrevious = canMovePrevious;
        }
    }
    function Sys$Preview$UI$Data$DataControl$get_canMoveNext() {
    if (arguments.length !== 0) throw Error.parameterCount();
                if (!this._data) return false;
        return (this._dataIndex < this.get_length() - 1);
    }
    function Sys$Preview$UI$Data$DataControl$get_canMovePrevious() {
    if (arguments.length !== 0) throw Error.parameterCount();
                if (!this._data) return false;
        return (this._dataIndex > 0);
    }
    function Sys$Preview$UI$Data$DataControl$get_data() {
    if (arguments.length !== 0) throw Error.parameterCount();
                return this._data;
    }
    function Sys$Preview$UI$Data$DataControl$set_data(value) {
                var oldState = this.prepareChange();
        if (this._data && Sys.Preview.INotifyCollectionChanged.isImplementedBy(this._data)) {
            this._data.remove_collectionChanged(this._dataChangedDelegate);
            this._dataChangedDelegate = null;
        }
        this._data = value;
        if (this._data && Sys.Preview.INotifyCollectionChanged.isImplementedBy(this._data)) {
            this._dataChangedDelegate = Function.createDelegate(this, this.onDataChanged);
            this._data.add_collectionChanged(this._dataChangedDelegate);
        }
        if (this._dataIndex >= this.get_length()) {
            this.set_dataIndex(0);
        }
        if (!this.get_isUpdating()) {
            this.render();
        }
        this.raisePropertyChanged('data');
        this.triggerChangeEvents(oldState);
    }
    function Sys$Preview$UI$Data$DataControl$get_dataContext() {
    if (arguments.length !== 0) throw Error.parameterCount();
                return this.get_dataItem();
    }
    function Sys$Preview$UI$Data$DataControl$get_dataIndex() {
    if (arguments.length !== 0) throw Error.parameterCount();
                return this._dataIndex;
    }
    function Sys$Preview$UI$Data$DataControl$set_dataIndex(value) {
        if (this._dataIndex !== value) {
            var oldState = this.prepareChange();
            this._dataIndex = value;
            if (!this._suspendChangeNotifications) {
                this.triggerChangeEvents(oldState);
            }
        }
    }
    function Sys$Preview$UI$Data$DataControl$get_dataItem() {
    if (arguments.length !== 0) throw Error.parameterCount();
                if (this._data && (this._dataIndex >= 0)) {
            if (Sys.Preview.Data.IData.isImplementedBy(this._data)) {
                return this._data.getItem(this._dataIndex);
            }
            if (this._data instanceof Array) {
                return this._data[this._dataIndex];
            }
        }
        return null;
    }
    function Sys$Preview$UI$Data$DataControl$get_length() {
    if (arguments.length !== 0) throw Error.parameterCount();
                if (!this._data) return 0;
        if (Sys.Preview.Data.IData.isImplementedBy(this._data)) {
            return this._data.get_length();
        }
        if (this._data instanceof Array) {
            return this._data.length;
        }
        return 0;
    }
    function Sys$Preview$UI$Data$DataControl$addItem() {
        if (this._data) {
            var oldState = this.prepareChange();
            if (Sys.Preview.Data.IData.isImplementedBy(this._data)) {
                this._data.add({});
            }
            else if (this._data instanceof Array) {
                if (typeof (this._data.add) === "function") {
                    this._data.add({});
                }
                else {
                    Array.add(this._data, {});
                }
            }
            this.set_dataIndex(this.get_length() - 1);
            this.triggerChangeEvents(oldState);
        }
    }
    function Sys$Preview$UI$Data$DataControl$deleteCurrentItem() {
        if (this._data) {
            var oldState = this.prepareChange();
            this._suspendChangeNotifications = true;
            var item = this.get_dataItem();
            if (this.get_dataIndex() === this.get_length() - 1) {
                this.set_dataIndex(Math.max(0, this.get_length() - 2));
            }
            if (Sys.Preview.Data.IData.isImplementedBy(this._data)) {
                this._data.remove(item);
            }
            else if (this._data instanceof Array) {
                if (typeof (this._data.remove) === "function") {
                    this._data.remove(item);
                }
                else {
                    Array.remove(this._data, item);
                }
            }
            this._suspendChangeNotifications = false;
            this.triggerChangeEvents(oldState);
        }
    }
    function Sys$Preview$UI$Data$DataControl$getItem(index) {
                        if (this._data) {
            if (Sys.Preview.Data.IData.isImplementedBy(this._data)) {
                return this._data.getItem(index);
            }
            if (this._data instanceof Array) {
                return this._data[index];
            }
        }
        return null;
    }
    function Sys$Preview$UI$Data$DataControl$moveNext() {
        if (this._data) {
            var oldState = this.prepareChange();
            var newIndex = this.get_dataIndex() + 1;
            if (newIndex < this.get_length()) {
                this.set_dataIndex(newIndex);
            }
            this.triggerChangeEvents(oldState);
        }
    }
    function Sys$Preview$UI$Data$DataControl$movePrevious() {
        if (this._data) {
            var oldState = this.prepareChange();
            var newIndex = this.get_dataIndex() - 1;
            if (newIndex >= 0) {
                this.set_dataIndex(newIndex);
            }
            this.triggerChangeEvents(oldState);
        }
    }
    function Sys$Preview$UI$Data$DataControl$onBubbleEvent(source, args) {
                                if (args.get_commandName() === "select") {
            var arg = args.get_argument();
            if (!arg && arg !== 0) {
                var dataContext = source.get_dataContext();
                if (dataContext) {
                    arg = dataContext.get_index();
                }
            }
            if (arg && String.isInstanceOfType(arg)) {
                arg = Number.parseInvariant(arg);
            }
            if (arg || arg === 0) {
                this.set_dataIndex(arg);
                return true;
            }
        }
        return false;
    }
    function Sys$Preview$UI$Data$DataControl$onDataChanged(sender, args) {
                        this.render();
    }
Sys.Preview.UI.Data.DataControl.prototype = {
    _data: null,
    _suspendChangeNotifications: false,
    _dataChangedDelegate: null,
    prepareChange: Sys$Preview$UI$Data$DataControl$prepareChange,
    triggerChangeEvents: Sys$Preview$UI$Data$DataControl$triggerChangeEvents,
    get_canMoveNext: Sys$Preview$UI$Data$DataControl$get_canMoveNext,
    get_canMovePrevious: Sys$Preview$UI$Data$DataControl$get_canMovePrevious,
    get_data: Sys$Preview$UI$Data$DataControl$get_data,
    set_data: Sys$Preview$UI$Data$DataControl$set_data,
    get_dataContext: Sys$Preview$UI$Data$DataControl$get_dataContext,
    get_dataIndex: Sys$Preview$UI$Data$DataControl$get_dataIndex,
    set_dataIndex: Sys$Preview$UI$Data$DataControl$set_dataIndex,
    get_dataItem: Sys$Preview$UI$Data$DataControl$get_dataItem,
    get_length: Sys$Preview$UI$Data$DataControl$get_length,
    addItem: Sys$Preview$UI$Data$DataControl$addItem,
    deleteCurrentItem: Sys$Preview$UI$Data$DataControl$deleteCurrentItem,
    getItem: Sys$Preview$UI$Data$DataControl$getItem,
    moveNext: Sys$Preview$UI$Data$DataControl$moveNext,
    movePrevious: Sys$Preview$UI$Data$DataControl$movePrevious,
    onBubbleEvent: Sys$Preview$UI$Data$DataControl$onBubbleEvent,
    onDataChanged: Sys$Preview$UI$Data$DataControl$onDataChanged
}
Sys.Preview.UI.Data.DataControl.registerClass('Sys.Preview.UI.Data.DataControl', Sys.UI.Control);
Sys.Preview.UI.Data.DataControl.descriptor = {
    properties: [ { name: 'canMoveNext', type: Boolean, readOnly: true },
                  { name: 'canMovePrevious', type: Boolean, readOnly: true },
                  { name: 'data', type: Sys.Preview.Data.DataTable },
                  { name: 'dataIndex', type: Number },
                  { name: 'dataItem', type: Object, readOnly: true },
                  { name: 'length', type: Number, readOnly: true } ],
    methods: [ { name: 'addItem' },
               { name: 'deleteCurrentItem' },
               { name: 'moveNext' },
               { name: 'movePrevious' } ]
}
Sys.Preview.UI.Data.DataNavigator = function Sys$Preview$UI$Data$DataNavigator(associatedElement) {
        Sys.Preview.UI.Data.DataNavigator.initializeBase(this, [associatedElement]);
}
    function Sys$Preview$UI$Data$DataNavigator$get_dataView() {
    if (arguments.length !== 0) throw Error.parameterCount();
                return this._data;
    }
    function Sys$Preview$UI$Data$DataNavigator$set_dataView(value) {
        
        this._data = value;
        this.raisePropertyChanged('dataView');
    }
    function Sys$Preview$UI$Data$DataNavigator$get_dataContext() {
    if (arguments.length !== 0) throw Error.parameterCount();
                return this.get_dataView();
    }
    function Sys$Preview$UI$Data$DataNavigator$onBubbleEvent(source, args) {
                                if (!this._data) return false;
        var cmd = args.get_commandName().toLowerCase();
        switch(cmd) {
        case "page":
            var arg = args.get_argument();
            if (arg && String.isInstanceOfType(arg)) {
                arg = Number.parseInvariant(arg);
            }
            if (arg || arg === 0) {
                this._data.set_pageIndex(arg);
                return true;
            }
            break;
        case "nextpage":
            this._data.set_pageIndex(this._data.get_pageIndex() + 1);
            return true;
        case "previouspage":
            var idx = this._data.get_pageIndex() - 1;
            if (idx >= 0) {
                this._data.set_pageIndex(idx);
            }
            return true;
        case "firstpage":
            this._data.set_pageIndex(0);
            return true;
        case "lastpage":
            this._data.set_pageIndex(this._data.get_pageCount() - 1);
            return true;
        }
        return false;
    }
Sys.Preview.UI.Data.DataNavigator.prototype = {
    _data: null,
    get_dataView: Sys$Preview$UI$Data$DataNavigator$get_dataView,
    set_dataView: Sys$Preview$UI$Data$DataNavigator$set_dataView,
    get_dataContext: Sys$Preview$UI$Data$DataNavigator$get_dataContext,
    onBubbleEvent: Sys$Preview$UI$Data$DataNavigator$onBubbleEvent
}
Sys.Preview.UI.Data.DataNavigator.registerClass('Sys.Preview.UI.Data.DataNavigator', Sys.UI.Control);
Sys.Preview.UI.Data.DataNavigator.descriptor = {
    properties: [ { name: 'dataView', type: Object } ]
}
Sys.Preview.UI.Data.ItemView = function Sys$Preview$UI$Data$ItemView(associatedElement) {
        Sys.Preview.UI.Data.ItemView.initializeBase(this, [associatedElement]);
}
    function Sys$Preview$UI$Data$ItemView$set_dataIndex(value) {
                if (this.get_dataIndex() !== value) {
            Sys.Preview.UI.Data.ItemView.callBaseMethod(this, 'set_dataIndex', [value]);
            if (!this.get_isUpdating()) {
                this.render();
            }
        }
    }
    function Sys$Preview$UI$Data$ItemView$get_emptyTemplate() {
    if (arguments.length !== 0) throw Error.parameterCount();
                return this._emptyTemplate;
    }
    function Sys$Preview$UI$Data$ItemView$set_emptyTemplate(value) {
        if (this._emptyTemplate) {
            this._emptyTemplate.dispose();
        }
        this._emptyTemplate = value;
        if (!this.get_isUpdating()) {
            this.render();
        }
        this.raisePropertyChanged('emptyTemplate');
    }
    function Sys$Preview$UI$Data$ItemView$get_itemTemplate() {
    if (arguments.length !== 0) throw Error.parameterCount();
                return this._itemTemplate;
    }
    function Sys$Preview$UI$Data$ItemView$set_itemTemplate(value) {
        if (this._itemTemplate) {
            this._itemTemplate.dispose();
        }
        this._itemTemplate = value;
        if (!this.get_isUpdating()) {
            this.render();
        }
        this.raisePropertyChanged('itemTemplate');
    }
    function Sys$Preview$UI$Data$ItemView$dispose() {
        var element = this.get_element();
        if (element) {
            if (this._keyDownHandler) {
                Sys.UI.DomEvent.removeHandler(element, "keydown", this._keyDownHandler);
            }
            if (element.childNodes.length) {
                element.markupContext = null;
                Sys.Preview.UI.ITemplate.disposeInstance(element.firstChild);
            }
        }
        if (this._itemTemplate) {
            this._itemTemplate.dispose();
            this._itemTemplate = null;
        }
        if (this._emptyTemplate) {
            this._emptyTemplate.dispose();
            this._emptyTemplate = null;
        }
        this._layoutTemplateElement = null;
        Sys.Preview.UI.Data.ItemView.callBaseMethod(this, 'dispose');
    }
    function Sys$Preview$UI$Data$ItemView$initialize() {
        this._keyDownHandler = Function.createDelegate(this, this._onKeyDown);
        Sys.Preview.UI.Data.ItemView.callBaseMethod(this, 'initialize');
        Sys.UI.DomEvent.addHandler(this.get_element(), "keydown", this._keyDownHandler);
        if (this._itemTemplate) {
            this._itemTemplate.initialize();
        }
        if (this._emptyTemplate) {
            this._emptyTemplate.initialize();
        }
        this.render();
    }
    function Sys$Preview$UI$Data$ItemView$_onKeyDown(ev) {
        if (ev.target === this.get_element()) {
            var k = ev.keyCode ? ev.keyCode : ev.rawEvent.keyCode;
            if ((k === Sys.UI.Key.up) || (k === Sys.UI.Key.left)) {
                this.movePrevious();
                ev.preventDefault();
            }
            else if ((k === Sys.UI.Key.down) || (k === Sys.UI.Key.right)) {
                this.moveNext();
                ev.preventDefault();
            }
        }
    }
    function Sys$Preview$UI$Data$ItemView$render() {
        var element = this.get_element();
        if (element.childNodes.length) {
            if (this._layoutTemplateElement) {
                Sys.Preview.UI.ITemplate.disposeInstance(this._layoutTemplateElement);
            }
        }
        element.innerHTML = '';
        var template;
        var data = this.get_data();
        if (data && data.get_length()) {
            template = this._itemTemplate;
        }
        else {
            template = this._emptyTemplate;
        }
        if (template) {
            var instance = template.createInstance(element, this.get_dataContext()).instanceElement;
            element.markupContext = instance.markupContext;
            this._layoutTemplateElement = instance;
        }
    }
    function Sys$Preview$UI$Data$ItemView$findObject(id) {
                        var object;
        var element = this.get_element();
        if (element.markupContext) {
            object = element.markupContext.findComponent(id);
        }
        if (!object) {
            var parent = this.get_parent();
            if (parent) {
                object = parent.findObject(id);
            }
            else {
                object = Sys.Application.findComponent(id);
            }
        }
        return object;
    }
Sys.Preview.UI.Data.ItemView.prototype = {
    _itemTemplate: null,
    _emptyTemplate: null,
    _keyDownHandler: null,
    _layoutTemplateElement: null,
    set_dataIndex: Sys$Preview$UI$Data$ItemView$set_dataIndex,
    get_emptyTemplate: Sys$Preview$UI$Data$ItemView$get_emptyTemplate,
    set_emptyTemplate: Sys$Preview$UI$Data$ItemView$set_emptyTemplate,
    get_itemTemplate: Sys$Preview$UI$Data$ItemView$get_itemTemplate,
    set_itemTemplate: Sys$Preview$UI$Data$ItemView$set_itemTemplate,
    dispose: Sys$Preview$UI$Data$ItemView$dispose,
    initialize: Sys$Preview$UI$Data$ItemView$initialize,
    _onKeyDown: Sys$Preview$UI$Data$ItemView$_onKeyDown,
    render: Sys$Preview$UI$Data$ItemView$render,
    findObject: Sys$Preview$UI$Data$ItemView$findObject
}
Sys.Preview.UI.Data.ItemView.registerClass('Sys.Preview.UI.Data.ItemView', Sys.Preview.UI.Data.DataControl, Sys.IContainer);
Sys.Preview.UI.Data.ItemView.descriptor = {
    properties: [ { name: 'itemTemplate', type: Sys.Preview.UI.ITemplate },
                  { name: 'emptyTemplate', type: Sys.Preview.UI.ITemplate } ]
}
Sys.Preview.UI.Data.ListView = function Sys$Preview$UI$Data$ListView(associatedElement) {
        Sys.Preview.UI.Data.ListView.initializeBase(this, [associatedElement]);
    this._itemElements = [];
    this._separatorElements = [];
}
    function Sys$Preview$UI$Data$ListView$get_alternatingItemCssClass() {
    if (arguments.length !== 0) throw Error.parameterCount();
                return this._alternatingItemClass;
    }
    function Sys$Preview$UI$Data$ListView$set_alternatingItemCssClass(value) {
        if (value !== this._alternatingItemClass) {
            this._alternatingItemClass = value;
            this.render();
            this.raisePropertyChanged('alternatingItemCssClass');
        }
    }
    function Sys$Preview$UI$Data$ListView$set_dataIndex(value) {
                var oldIndex = this.get_dataIndex();
        if (oldIndex !== value) {
            var sel = this.getItemElement(oldIndex);
            if (sel && this._selectedItemClass) {
                Sys.UI.DomElement.removeCssClass(sel, this._selectedItemClass);
            }
            Sys.Preview.UI.Data.ListView.callBaseMethod(this, 'set_dataIndex', [value]);
            sel = this.getItemElement(value);
            if (sel && this._selectedItemClass) {
                Sys.UI.DomElement.addCssClass(sel, this._selectedItemClass);
            }
        }
    }
    function Sys$Preview$UI$Data$ListView$get_emptyTemplate() {
    if (arguments.length !== 0) throw Error.parameterCount();
                return this._emptyTemplate;
    }
    function Sys$Preview$UI$Data$ListView$set_emptyTemplate(value) {
        if (this._emptyTemplate) {
            this._emptyTemplate.dispose();
        }
        this._emptyTemplate = value;
        if (!this.get_isUpdating()) {
            this.render();
        }
        this.raisePropertyChanged('emptyTemplate');
    }
    function Sys$Preview$UI$Data$ListView$get_itemCssClass() {
    if (arguments.length !== 0) throw Error.parameterCount();
                return this._itemClass;
    }
    function Sys$Preview$UI$Data$ListView$set_itemCssClass(value) {
        if (value !== this._itemClass) {
            this._itemClass = value;
            this.render();
            this.raisePropertyChanged('itemCssClass');
        }
    }
    function Sys$Preview$UI$Data$ListView$get_itemTemplate() {
    if (arguments.length !== 0) throw Error.parameterCount();
                return this._itemTemplate;
    }
    function Sys$Preview$UI$Data$ListView$set_itemTemplate(value) {
        if (this._itemTemplate) {
            this._itemTemplate.dispose();
        }
        this._itemTemplate = value;
        if (!this.get_isUpdating()) {
            this.render();
        }
        this.raisePropertyChanged('itemTemplate');
    }
    function Sys$Preview$UI$Data$ListView$get_itemTemplateParentElementId() {
    if (arguments.length !== 0) throw Error.parameterCount();
                return this._itemTemplateParentElementId;
    }
    function Sys$Preview$UI$Data$ListView$set_itemTemplateParentElementId(value) {
        this._itemTemplateParentElementId = value;
        this.raisePropertyChanged('itemTemplateParentElementId');
    }
    function Sys$Preview$UI$Data$ListView$get_layoutTemplate() {
    if (arguments.length !== 0) throw Error.parameterCount();
                return this._layoutTemplate;
    }
    function Sys$Preview$UI$Data$ListView$set_layoutTemplate(value) {
        if (this._layoutTemplate) {
            this._layoutTemplate.dispose();
        }
        this._layoutTemplate = value;
        if (!this.get_isUpdating()) {
            this.render();
        }
        this.raisePropertyChanged('layoutTemplate');
    }
    function Sys$Preview$UI$Data$ListView$get_selectedItemCssClass() {
    if (arguments.length !== 0) throw Error.parameterCount();
                return this._selectedItemClass;
    }
    function Sys$Preview$UI$Data$ListView$set_selectedItemCssClass(value) {
        if (value !== this._selectedItemClass) {
            this._selectedItemClass = value;
            this.render();
            this.raisePropertyChanged('selectedItemCssClass');
        }
    }
    function Sys$Preview$UI$Data$ListView$get_separatorCssClass() {
    if (arguments.length !== 0) throw Error.parameterCount();
                return this._separatorClass;
    }
    function Sys$Preview$UI$Data$ListView$set_separatorCssClass(value) {
        if (value !== this._separatorClass) {
            this._separatorClass = value;
            this.render();
            this.raisePropertyChanged('separatorCssClass');
        }
    }
    function Sys$Preview$UI$Data$ListView$get_separatorTemplate() {
    if (arguments.length !== 0) throw Error.parameterCount();
                return this._separatorTemplate;
    }
    function Sys$Preview$UI$Data$ListView$set_separatorTemplate(value) {
        if (this._separatorTemplate) {
            this._separatorTemplate.dispose();
        }
        this._separatorTemplate = value;
        if (!this.get_isUpdating()) {
            this.render();
        }
        this.raisePropertyChanged('separatorTemplate');
    }
    function Sys$Preview$UI$Data$ListView$getItemElement(index) {
                        return this._itemElements[index];
    }
    function Sys$Preview$UI$Data$ListView$add_renderComplete(handler) {
    var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
    if (e) throw e;
        this.get_events().addHandler("renderComplete", handler);
    }
    function Sys$Preview$UI$Data$ListView$remove_renderComplete(handler) {
    var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
    if (e) throw e;
        this.get_events().removeHandler("renderComplete", handler);
    }
    function Sys$Preview$UI$Data$ListView$initialize() {
        var element = this.get_element();
        this._focusHandler = Function.createDelegate(this, this._onGotFocus);
        this._keyDownHandler = Function.createDelegate(this, this._onKeyDown);
        this._itemFocusHandler = Function.createDelegate(this, this._onItemFocus);
        this._itemClickHandler = Function.createDelegate(this, this._onItemClick);
        Sys.Preview.UI.Data.ListView.callBaseMethod(this, 'initialize');
        Sys.UI.DomEvent.addHandler(element, "keydown", this._keyDownHandler);
        Sys.UI.DomEvent.addHandler(element, "focus", this._focusHandler);
        if (this._itemTemplate) {
            this._itemTemplate.initialize();
        }
        if (this._separatorTemplate) {
            this._separatorTemplate.initialize();
        }
        if (this._emptyTemplate) {
            this._emptyTemplate.initialize();
        }
        if (this._layoutTemplate) {
            this._layoutTemplate.initialize();
        }
        if (!element.tabIndex) {
            element.tabIndex = 0;
        }
        this.render();
    }
    function Sys$Preview$UI$Data$ListView$dispose() {
        if (this._disposed) return;
        var element = this.get_element();
        if (element) {
            if (this._focusHandler) {
                Sys.UI.DomEvent.removeHandler(element, "focus", this._focusHandler);
            }
            if (this._keyDownHandler) {
                Sys.UI.DomEvent.removeHandler(element, "keydown", this._keyDownHandler);
            }
        }
        if (this._itemElements) {
            for (var i = this._itemElements.length - 1; i >= 0; i--) {
                if (this._itemFocusHandler) {
                    Sys.UI.DomEvent.removeHandler(this._itemElements[i], "focus", this._itemFocusHandler);
                }
                if (this._itemClickHandler) {
                    Sys.UI.DomEvent.removeHandler(this._itemElements[i], "click", this._itemClickHandler);
                }
            }
        }
        if (this._layoutTemplate) {
            this._layoutTemplate.dispose();
            this._layoutTemplate = null;
        }
        if (this._itemTemplate) {
            this._itemTemplate.dispose();
            this._itemTemplate = null;
        }
        if (this._separatorTemplate) {
            this._separatorTemplate.dispose();
            this._separatorTemplate = null;
        }
        if (this._emptyTemplate) {
            this._emptyTemplate.dispose();
            this._emptyTemplate = null;
        }
        this._itemElements = null;
        this._separatorElements = null;
        this._layoutTemplateElement = null;
        this._disposed = true;
        Sys.Preview.UI.Data.ListView.callBaseMethod(this, 'dispose');
    }
    function Sys$Preview$UI$Data$ListView$_onGotFocus(ev) {
        if (ev.target === this.get_element()) {
            this.setFocus(this, this.getItemElement(this.get_dataIndex()));
        }
    }
    function Sys$Preview$UI$Data$ListView$_onKeyDown(ev) {
        if (ev.target === this.getItemElement(this._focusIndex)) {
            var k = ev.keyCode ? ev.keyCode : ev.rawEvent.keyCode;
            if ((k === Sys.UI.Key.up) || (k === Sys.UI.Key.left)) {
                if (this._focusIndex > 0) {
                    this.setFocus(this, this.getItemElement(this._focusIndex - 1));
                    ev.preventDefault();
                }
            }
            else if ((k === Sys.UI.Key.down) || (k === Sys.UI.Key.right)) {
                if (this._focusIndex < (this.get_length() - 1)) {
                    this.setFocus(this, this.getItemElement(this._focusIndex + 1));
                    ev.preventDefault();
                }
            }
            else if ((k === Sys.UI.Key.enter) || (k === Sys.UI.Key.space)) {
                if (this._focusIndex !== -1) {
                    this.set_dataIndex(this._focusIndex);
                    ev.preventDefaut();
                }
            }
        }
    }
    function Sys$Preview$UI$Data$ListView$_onItemFocus(ev) {
        if (typeof (ev.target.dataIndex) !== "undefined") {
            this._focusIndex = ev.target.dataIndex;
        }
    }
    function Sys$Preview$UI$Data$ListView$_onItemClick(ev) {
        var s = ev.target;
        var srcTag = s.tagName.toUpperCase();
        while (s && (typeof (s.dataIndex) === 'undefined')) {
            s = s.parentNode;
        }
        if (s) {
            var idx = s.dataIndex;
            sel = this.getItemElement(idx);
            if (sel) {
                this.set_dataIndex(idx);
                if ((srcTag !== "INPUT") && (srcTag !== "TEXTAREA") &&
                    (srcTag !== "SELECT") && (srcTag !== "BUTTON") && (srcTag !== "A")) {
                    this.setFocus(this, sel);
                }
            }
        }
    }
    function Sys$Preview$UI$Data$ListView$render() {
        var associatedElement = this.get_element();
        var i, element;
        for (i = this._itemElements.length - 1; i >= 0; i--) {
            element = this._itemElements[i];
            if (element) {
                Sys.Preview.UI.ITemplate.disposeInstance(element);
            }
        }
        this._itemElements = [];
        for (i = this._separatorElements.length - 1; i >= 0; i--) {
            element = this._separatorElements[i];
            if (element) {
                Sys.Preview.UI.ITemplate.disposeInstance(element);
            }
        }
        this._separatorElements = [];
        if (associatedElement.childNodes.length) {
            if (this._layoutTemplateElement) {
                Sys.Preview.UI.ITemplate.disposeInstance(this._layoutTemplateElement);
            }
        }
        associatedElement.innerHTML = '';
        var tasksPending = false;
        var items = this.get_data();
        var itemLength = items ? (items.get_length ? items.get_length() : items.length) : (0);
                if (itemLength && itemLength > 0) {
            var template = this.get_layoutTemplate();
            if (template) {
                var itemTemplate = this.get_itemTemplate();
                var separatorTemplate = this.get_separatorTemplate();
                var layoutTemplateInstance = template.createInstance(associatedElement, null, this.findItemTemplateParentCallback, this._itemTemplateParentElementId);
                var itemTemplateParent = layoutTemplateInstance.callbackResult;
                this._layoutTemplateElement = layoutTemplateInstance.instanceElement;
                tasksPending = true;
                this._pendingTasks++;
                var renderTask = new Sys.Preview.UI.Data.ListViewRenderTask(this, items, itemTemplate, itemTemplateParent, separatorTemplate, this._itemElements, this._separatorElements, this._itemClass, this._alternatingItemClass, this._separatorClass, this._itemFocusHandler, this._itemClickHandler);
                Sys.Preview.TaskManager.addTask(renderTask);
            }
        }
        else {
            var emptyTemplate = this.get_emptyTemplate();
            if (emptyTemplate) {
                emptyTemplate.createInstance(associatedElement);
            }
            var handler = this.get_events().getHandler('renderComplete');
            if (handler) handler(this, Sys.EventArgs.Empty);
        }
    }
    function Sys$Preview$UI$Data$ListView$_renderTaskComplete(renderTask) {
        this._pendingTasks--;
        if (this._pendingTasks <= 0) {
            this._pendingTasks = 0;
            var handler = this.get_events().getHandler('renderComplete');
            if (handler) handler(this, Sys.EventArgs.Empty);
        }
    }
    function Sys$Preview$UI$Data$ListView$findItemTemplateParentCallback(instanceElement, markupContext, id) {
                                        return markupContext.findElement(id);
    }
    function Sys$Preview$UI$Data$ListView$setFocus(owner, element) {
                        if (element.focus) {
            for (var i = owner.get_length() - 1; i >= 0; i--) {
                var sel = owner.getItemElement(i);
                if (sel) {
                    sel.tabIndex = -1;
                }
            }
            var ownerElement = owner.get_element();
            var t = ownerElement.tabIndex;
            if (t === -1) {
                t = ownerElement.__tabIndex;
            }
            element.tabIndex = t;
            setTimeout(Function.createCallback(this.focus, element), 0);
            ownerElement.__tabIndex = t;
            ownerElement.tabIndex = -1;
        }
    }
    function Sys$Preview$UI$Data$ListView$focus(element) {
                try {
            element.focus();
        }
        catch (e) { }
    }
Sys.Preview.UI.Data.ListView.prototype = {
    _itemClass: null,
    _alternatingItemClass: null,
    _separatorClass: null,
    _selectedItemClass: null,
    _focusHandler: null,
    _keyDownHandler: null,
    _itemFocusHandler: null,
    _itemClickHandler: null,
    _focusIndex: null,
        _layoutTemplate: null,
    _itemTemplate: null,
    _separatorTemplate: null,
    _emptyTemplate: null,
    _itemTemplateParentElementId: null,
    _layoutTemplateElement: null,
    _pendingTasks: 0,
    get_alternatingItemCssClass: Sys$Preview$UI$Data$ListView$get_alternatingItemCssClass,
    set_alternatingItemCssClass: Sys$Preview$UI$Data$ListView$set_alternatingItemCssClass,
    set_dataIndex: Sys$Preview$UI$Data$ListView$set_dataIndex,
    get_emptyTemplate: Sys$Preview$UI$Data$ListView$get_emptyTemplate,
    set_emptyTemplate: Sys$Preview$UI$Data$ListView$set_emptyTemplate,
    get_itemCssClass: Sys$Preview$UI$Data$ListView$get_itemCssClass,
    set_itemCssClass: Sys$Preview$UI$Data$ListView$set_itemCssClass,
    get_itemTemplate: Sys$Preview$UI$Data$ListView$get_itemTemplate,
    set_itemTemplate: Sys$Preview$UI$Data$ListView$set_itemTemplate,
    get_itemTemplateParentElementId: Sys$Preview$UI$Data$ListView$get_itemTemplateParentElementId,
    set_itemTemplateParentElementId: Sys$Preview$UI$Data$ListView$set_itemTemplateParentElementId,
    get_layoutTemplate: Sys$Preview$UI$Data$ListView$get_layoutTemplate,
    set_layoutTemplate: Sys$Preview$UI$Data$ListView$set_layoutTemplate,
    get_selectedItemCssClass: Sys$Preview$UI$Data$ListView$get_selectedItemCssClass,
    set_selectedItemCssClass: Sys$Preview$UI$Data$ListView$set_selectedItemCssClass,
    get_separatorCssClass: Sys$Preview$UI$Data$ListView$get_separatorCssClass,
    set_separatorCssClass: Sys$Preview$UI$Data$ListView$set_separatorCssClass,
    get_separatorTemplate: Sys$Preview$UI$Data$ListView$get_separatorTemplate,
    set_separatorTemplate: Sys$Preview$UI$Data$ListView$set_separatorTemplate,
    getItemElement: Sys$Preview$UI$Data$ListView$getItemElement,
    add_renderComplete: Sys$Preview$UI$Data$ListView$add_renderComplete,
    remove_renderComplete: Sys$Preview$UI$Data$ListView$remove_renderComplete,
    initialize: Sys$Preview$UI$Data$ListView$initialize,
    dispose: Sys$Preview$UI$Data$ListView$dispose,
    _onGotFocus: Sys$Preview$UI$Data$ListView$_onGotFocus,
    _onKeyDown: Sys$Preview$UI$Data$ListView$_onKeyDown,
    _onItemFocus: Sys$Preview$UI$Data$ListView$_onItemFocus,
    _onItemClick: Sys$Preview$UI$Data$ListView$_onItemClick,
    render: Sys$Preview$UI$Data$ListView$render,
    _renderTaskComplete: Sys$Preview$UI$Data$ListView$_renderTaskComplete,
    findItemTemplateParentCallback: Sys$Preview$UI$Data$ListView$findItemTemplateParentCallback,
    setFocus: Sys$Preview$UI$Data$ListView$setFocus,
    focus: Sys$Preview$UI$Data$ListView$focus
}
Sys.Preview.UI.Data.ListView.registerClass('Sys.Preview.UI.Data.ListView', Sys.Preview.UI.Data.DataControl);
Sys.Preview.UI.Data.ListView.descriptor = {
    properties: [ { name: 'alternatingItemCssClass', type: String },
                  { name: 'layoutTemplate', type: Sys.Preview.UI.ITemplate },
                  { name: 'itemCssClass', type: String },
                  { name: 'itemTemplate', type: Sys.Preview.UI.ITemplate },
                  { name: 'itemTemplateParentElementId', type: String },
                  { name: 'selectedItemCssClass', type: String },
                  { name: 'separatorCssClass', type: String },
                  { name: 'separatorTemplate', type: Sys.Preview.UI.ITemplate },
                  { name: 'emptyTemplate', type: Sys.Preview.UI.ITemplate } ],
    events: [ {name: 'renderComplete'} ]
}
Sys.Preview.UI.Data.ListViewRenderTask = function Sys$Preview$UI$Data$ListViewRenderTask(listView, data, itemTemplate, itemTemplateParent, separatorTemplate, itemElements, separatorElements, itemClass, alternatingItemClass, separatorClass, itemFocusHandler, itemClickHandler) {
                                                    this._listView = listView;
    this._data = data;
    this._itemTemplate = itemTemplate;
    this._itemTemplateParent = itemTemplateParent;
    this._separatorTemplate = separatorTemplate;
    this._itemElements = itemElements;
    this._separatorElements = separatorElements;
    this._itemClass = itemClass;
    this._alternatingItemClass = alternatingItemClass;
    this._separatorClass = separatorClass;
    this._itemFocusHandler = itemFocusHandler;
    this._itemClickHandler = itemClickHandler;
    this._currentIndex = 0;
}
    function Sys$Preview$UI$Data$ListViewRenderTask$dispose() {
        this._listView = null;
        this._data = null;
        this._itemTemplate = null;
        this._itemTemplateParent = null;
        this._separatorTemplate = null;
        this._itemElements = null;
        this._separatorElements = null;
        this._itemClass = null;
        this._alternatingItemClass = null;
        this._separatorClass = null;
        this._itemFocusHandler = null;
        this._itemClickHandler = null;
    }
    function Sys$Preview$UI$Data$ListViewRenderTask$execute() {
                var isArray = Array.isInstanceOfType(this._data);
        var itemLength = isArray? this._data.length : (this._data ? (this._data.get_length ? this._data.get_length() : 0) : 0);
        var lengthm1 = itemLength - 1;
                var lastElementToRender = Math.min(itemLength, this._currentIndex + 5);
        for (; this._currentIndex < lastElementToRender; this._currentIndex++) {
            var item = isArray? this._data[this._currentIndex] : this._data.getItem(this._currentIndex);
            if (this._itemTemplate) {
                var element = this._itemTemplate.createInstance(this._itemTemplateParent, item).instanceElement;
                if (this._itemClass) {
                    if ((this._currentIndex % 2 === 1) && (this._alternatingItemClass)) {
                        element.className = this._alternatingItemClass;
                    }
                    else {
                        element.className = this._itemClass;
                    }
                }
                this._itemElements[this._currentIndex] = element;
                element.tabIndex = -1;
                element.dataIndex = this._currentIndex;
                Sys.UI.DomEvent.addHandler(element, "focus", this._itemFocusHandler);
                Sys.UI.DomEvent.addHandler(element, "click", this._itemClickHandler);
                                            }
            if (this._separatorTemplate && (this._currentIndex !== lengthm1) && this._itemTemplateParent) {
                var sep = this._separatorTemplate.createInstance(this._itemTemplateParent).instanceElement;
                if (this._separatorClass) {
                    sep.className = this._separatorClass;
                }
                this._separatorElements[this._currentIndex] = sep;
            }
        }
        if (this._currentIndex === itemLength) {
                        this._listView._renderTaskComplete(this);
                        return true;
        }
        else {
                        return false;
        }
    }
Sys.Preview.UI.Data.ListViewRenderTask.prototype = {
    dispose: Sys$Preview$UI$Data$ListViewRenderTask$dispose,
    execute: Sys$Preview$UI$Data$ListViewRenderTask$execute
}
Sys.Preview.UI.Data.ListViewRenderTask.registerClass('Sys.Preview.UI.Data.ListViewRenderTask', null, Sys.Preview.ITask, Sys.IDisposable);
Sys.Preview.UI.Data.SortBehavior = function Sys$Preview$UI$Data$SortBehavior(element) {
        Sys.Preview.UI.Data.SortBehavior.initializeBase(this,[element]);
}
    function Sys$Preview$UI$Data$SortBehavior$get_sortAscendingCssClass() {
    if (arguments.length !== 0) throw Error.parameterCount();
                return this._sortAscendingCssClass;
    }
    function Sys$Preview$UI$Data$SortBehavior$set_sortAscendingCssClass(value) {
        this._sortAscendingCssClass = value;
    }
    function Sys$Preview$UI$Data$SortBehavior$get_sortColumn() {
    if (arguments.length !== 0) throw Error.parameterCount();
                return this._sortColumn;
    }
    function Sys$Preview$UI$Data$SortBehavior$set_sortColumn(value) {
        if (value !== this._sortColumn) {
            this._sortColumn = value;
            this.raisePropertyChanged('sortColumn');
        }
    }
    function Sys$Preview$UI$Data$SortBehavior$get_sortDescendingCssClass() {
    if (arguments.length !== 0) throw Error.parameterCount();
                return this._sortDescendingCssClass;
    }
    function Sys$Preview$UI$Data$SortBehavior$set_sortDescendingCssClass(value) {
        this._sortDescendingCssClass = value;
    }
    function Sys$Preview$UI$Data$SortBehavior$get_dataView() {
    if (arguments.length !== 0) throw Error.parameterCount();
                return this._dataView;
    }
    function Sys$Preview$UI$Data$SortBehavior$set_dataView(value) {
        
        if (this._dataView && this._sortChangedDelegate) {
            this._dataView.remove_propertyChanged(this._sortChangedDelegate);
        }
        this._dataView = value;
        if (this.get_isInitialized()) {
            this._dataView.add_propertyChanged(this._sortChangedDelegate);
            this.update();
        }
    }
    function Sys$Preview$UI$Data$SortBehavior$dispose() {
        if (this._dataView && !this._dataView._disposed && this._sortChangedDelegate) {
            this._dataView.remove_propertyChanged(this._sortChangedDelegate);
            this._sortChangedDelegate = null;
        }
        this._dataView = null;
        if (this._clickHandler) {
            Sys.UI.DomEvent.removeHandler(this.get_element(), "click", this._clickHandler);
            this._clickHandler = null;
        }
        Sys.Preview.UI.Data.SortBehavior.callBaseMethod(this, 'dispose');
    }
    function Sys$Preview$UI$Data$SortBehavior$initialize() {
        Sys.Preview.UI.Data.SortBehavior.callBaseMethod(this, 'initialize');
        this._clickHandler = Function.createDelegate(this, this.clickHandler);
        Sys.UI.DomEvent.addHandler(this.get_element(), "click", this._clickHandler);
        this._sortChangedDelegate = Function.createDelegate(this, this.sortChanged);
        if (this._dataView) {
            this._dataView.add_propertyChanged(this._sortChangedDelegate);
            this.update();
        }
    }
    function Sys$Preview$UI$Data$SortBehavior$clickHandler() {
        var view = this.get_dataView();
        if (view) {
            if (view.get_sortColumn() === this._sortColumn) {
                view.set_sortDirection(
                    (view.get_sortDirection() === Sys.Preview.Data.SortDirection.Ascending) ?
                    Sys.Preview.Data.SortDirection.Descending :
                    Sys.Preview.Data.SortDirection.Ascending);
            }
            else {
                view.sort(this._sortColumn, Sys.Preview.Data.SortDirection.Ascending);
            }
        }
    }
    function Sys$Preview$UI$Data$SortBehavior$update() {
        var element = this.get_element();
        if (this._dataView && (this._dataView.get_sortColumn() === this._sortColumn)) {
            if (this._dataView.get_sortDirection() === Sys.Preview.Data.SortDirection.Ascending) {
                Sys.UI.DomElement.removeCssClass(element, this._sortDescendingCssClass);
                Sys.UI.DomElement.addCssClass(element, this._sortAscendingCssClass);
            }
            else {
                Sys.UI.DomElement.removeCssClass(element, this._sortAscendingCssClass);
                Sys.UI.DomElement.addCssClass(element, this._sortDescendingCssClass);
            }
        }
        else {
            Sys.UI.DomElement.removeCssClass(element, this._sortAscendingCssClass);
            Sys.UI.DomElement.removeCssClass(element, this._sortDescendingCssClass);
        }
    }
    function Sys$Preview$UI$Data$SortBehavior$sortChanged(sender, args) {
                        var pName = args.get_propertyName();
        if ((pName === 'sortColumn') || (pName === 'sortDirection')) {
            this.update();
        }
    }
Sys.Preview.UI.Data.SortBehavior.prototype = {
    _clickHandler: null,
    _sortChangedDelegate: null,
    _sortColumn: '',
    _sortAscendingCssClass: 'sortAscending',
    _sortDescendingCssClass: 'sortDescending',
    _dataView: null,
    
    get_sortAscendingCssClass: Sys$Preview$UI$Data$SortBehavior$get_sortAscendingCssClass,
    set_sortAscendingCssClass: Sys$Preview$UI$Data$SortBehavior$set_sortAscendingCssClass,
    
    get_sortColumn: Sys$Preview$UI$Data$SortBehavior$get_sortColumn,
    set_sortColumn: Sys$Preview$UI$Data$SortBehavior$set_sortColumn,
    
    get_sortDescendingCssClass: Sys$Preview$UI$Data$SortBehavior$get_sortDescendingCssClass,
    set_sortDescendingCssClass: Sys$Preview$UI$Data$SortBehavior$set_sortDescendingCssClass,
    
    get_dataView: Sys$Preview$UI$Data$SortBehavior$get_dataView,
    set_dataView: Sys$Preview$UI$Data$SortBehavior$set_dataView,
    
    dispose: Sys$Preview$UI$Data$SortBehavior$dispose,
    initialize: Sys$Preview$UI$Data$SortBehavior$initialize,
    
    clickHandler: Sys$Preview$UI$Data$SortBehavior$clickHandler,
    
    update: Sys$Preview$UI$Data$SortBehavior$update,
    
    sortChanged: Sys$Preview$UI$Data$SortBehavior$sortChanged
}
Sys.Preview.UI.Data.SortBehavior.registerClass('Sys.Preview.UI.Data.SortBehavior', Sys.UI.Behavior);
Sys.Preview.UI.Data.SortBehavior.descriptor = {
    properties: [ { name: 'dataView', type: Object },
                  { name: 'sortAscendingCssClass', type: String },
                  { name: 'sortColumn', type: String },
                  { name: 'sortDescendingCssClass', type: String } ]
}
Sys.Preview.UI.Data.XSLTView = function Sys$Preview$UI$Data$XSLTView(associatedElement) {
        Sys.Preview.UI.Data.XSLTView.initializeBase(this, [associatedElement]);
}
    function Sys$Preview$UI$Data$XSLTView$get_document() {
    if (arguments.length !== 0) throw Error.parameterCount();
                return this._document;
    }
    function Sys$Preview$UI$Data$XSLTView$set_document(document) {
                this._document = document;
        if (this.get_isInitialized()) {
            this._render();
        }
    }
    function Sys$Preview$UI$Data$XSLTView$get_parameters() {
    if (arguments.length !== 0) throw Error.parameterCount();
                if (!this._parameters) {
            this._parameters = {};
        }
        return this._parameters;
    }
    function Sys$Preview$UI$Data$XSLTView$get_transform() {
    if (arguments.length !== 0) throw Error.parameterCount();
                return this._transform;
    }
    function Sys$Preview$UI$Data$XSLTView$set_transform(transform) {
                this._transform = transform;
        if (this.get_isInitialized()) {
            this._render();
        }
    }
    function Sys$Preview$UI$Data$XSLTView$dispose() {
        this._document = null;
        this._transform = null;
        Sys.Preview.UI.Data.XSLTView.callBaseMethod(this, 'dispose');
    }
    function Sys$Preview$UI$Data$XSLTView$initialize() {
        Sys.Preview.UI.Data.XSLTView.callBaseMethod(this, 'initialize');
        this._render();
    }
    function Sys$Preview$UI$Data$XSLTView$update() {
        this._render();
    }
    function Sys$Preview$UI$Data$XSLTView$_render() {
        var html = '';
        if (this._document && this._transform) {
            if (this._parameters) {
                if (Sys.Browser.agent === Sys.Browser.InternetExplorer) {
                    this._transform.setProperty('SelectionNamespaces', 'xmlns:xsl="http://www.w3.org/1999/XSL/Transform"');
                }
                for (var paramName in this._parameters) {
                    var paramNode = this._transform.selectSingleNode('//xsl:param[@name="' + paramName + '"]');
                    if (paramNode) {
                        paramNode.text = this._parameters[paramName].toString();
                        paramNode.removeAttribute('select');
                    }
                }
            }
            html = this._document.transformNode(this._transform);
        }
        this.get_element().innerHTML = html;
    }
Sys.Preview.UI.Data.XSLTView.prototype = {
    _document: null,
    _transform: null,
    _parameters: null,
    get_document: Sys$Preview$UI$Data$XSLTView$get_document,
    set_document: Sys$Preview$UI$Data$XSLTView$set_document,
    get_parameters: Sys$Preview$UI$Data$XSLTView$get_parameters,
    get_transform: Sys$Preview$UI$Data$XSLTView$get_transform,
    set_transform: Sys$Preview$UI$Data$XSLTView$set_transform,
    dispose: Sys$Preview$UI$Data$XSLTView$dispose,
    initialize: Sys$Preview$UI$Data$XSLTView$initialize,
    update: Sys$Preview$UI$Data$XSLTView$update,
    _render: Sys$Preview$UI$Data$XSLTView$_render
}
Sys.Preview.UI.Data.XSLTView.registerClass('Sys.Preview.UI.Data.XSLTView', Sys.UI.Control);
Sys.Preview.UI.Data.XSLTView.descriptor = {
    properties: [ { name: 'document', type: Object },
                  { name: 'parameters', type: Object, readOnly: true },
                  { name: 'transform', type: Object } ],
    methods: [ { name: 'update' } ]
}
Sys.Component.createCollection = function Sys$Component$createCollection(component) {
    var collection = [];
    collection._component = component;
    var _events = null;
    collection.get_events = function collection$get_events() {
        if (!_events) {
            _events = new Sys.EventHandlerList();
        }
        return _events;
    }
    collection.add_collectionChanged = function collection$add_collectionChanged(handler) {
        this.get_events().addHandler("collectionChanged", handler);
    }
    collection.remove_collectionChanged = function collection$remove_collectionChanged(handler) {
        this.get_events().removeHandler("collectionChanged", handler);
    }
    collection._onCollectionChanged = function collection$_onCollectionChanged(args) {
        var handler = this.get_events().getHandler("collectionChanged");
        if (handler) {
            handler(this, args);
        }
    }
    collection.add = function collection$add(item) {
        Array.add(this, item);
        if(typeof(item.setOwner) === "function") {
            item.setOwner(this._component);
        }
        this._onCollectionChanged(new Sys.Preview.CollectionChangedEventArgs(Sys.Preview.NotifyCollectionChangedAction.Add, item));
    }
    collection.clear = function collection$clear() {
        for (var i = this.length - 1; i >= 0; i--) {
            this[i].dispose();
            this[i] = null;
        }
        Array.clear(this);
        this._onCollectionChanged(new Sys.Preview.CollectionChangedEventArgs(Sys.Preview.NotifyCollectionChangedAction.Reset, null));
    }
    collection.dispose = function collection$dispose() {
        this.clear();
        delete this._events;
        this._component = null;
        this._disposed = true;
    }
    collection.remove = function collection$remove(item) {
        item.dispose();
        Array.remove(this, item);
        this._onCollectionChanged(new Sys.Preview.CollectionChangedEventArgs(Sys.Preview.NotifyCollectionChangedAction.Remove, item));
    }
    collection.removeAt = function collection$removeAt(index) {
        var item = this[index];
        item.dispose();
        Array.removeAt(this, index);
        this._onCollectionChanged(new Sys.Preview.CollectionChangedEventArgs(Sys.Preview.NotifyCollectionChangedAction.Remove, item));
    }
    return collection;
}
Sys.Component.createMultiple = function Sys$Component$createMultiple(elements, type, properties, events, references) {
    /// <param name="elements" type="Array" elementDomElement="true"></param>
    /// <param name="type" type="Type"></param>
    /// <param name="properties" optional="true" mayBeNull="true"></param>
    /// <param name="events" optional="true" mayBeNull="true"></param>
    /// <param name="references" optional="true" mayBeNull="true"></param>
    var e = Function._validateParams(arguments, [
        {name: "elements", type: Array},
        {name: "type", type: Type},
        {name: "properties", mayBeNull: true, optional: true},
        {name: "events", mayBeNull: true, optional: true},
        {name: "references", mayBeNull: true, optional: true}
    ]);
    if (e) throw e;
    var create = Sys.Component.create;
    for (var i = 0, l = elements.length; i < l; i++) {
        create(type, properties, events, references, elements[i]);
    }
}
Sys.UI.DomElement._contains = function Sys$UI$DomElement$_contains(root, element) {
    while (element) {
        element = element.parentNode;
        if (element === root) return true;
    }
    return false;
}
Sys.UI.DomElement._testTerm = function Sys$UI$DomElement$_testTerm(term, element) {
    return (!term.id || element.id === term.id) &&
        (!term.tagName || element.tagName.toLowerCase() === term.tagName) &&
        ((term.className === '  ') || (' ' + element.className + ' ').indexOf(term.className) !== -1);
}
Sys.UI.DomElement.getElementsByClassName = function Sys$UI$DomElement$getElementsByClassName(className, element) {
    /// <param name="className" type="String"></param>
    /// <param name="element" domElement="true" optional="true"></param>
    /// <returns type="Array" elementDomElement="true"></returns>
    var e = Function._validateParams(arguments, [
        {name: "className", type: String},
        {name: "element", domElement: true, optional: true}
    ]);
    if (e) throw e;
    element = element || document;
    className = ' ' + className + ' ';
    var potentials = element.all || element.getElementsByTagName("*");
    var l = potentials.length, results = [], i;
    for (i = 0; i < l; i++) {
        if ((' ' + potentials[i].className + ' ').indexOf(className) !== -1) {
            results[results.length] = potentials[i];
        }
    }
    return results;
}
Sys.UI.DomElement.selectAllElements = function Sys$UI$DomElement$selectAllElements(selector, element) {
    /// <param name="selector" type="String"></param>
    /// <param name="element" domElement="true" optional="true" mayBeNull="true"></param>
    /// <returns type="Array" elementDomElement="true"></returns>
    var e = Function._validateParams(arguments, [
        {name: "selector", type: String},
        {name: "element", mayBeNull: true, domElement: true, optional: true}
    ]);
    if (e) throw e;
    var cssSelectorExpression = /([^\.#]*)\.?([^#]*)#?(.*)/,
        terms = selector.trim().split(/\s+/),
        root = element || document,
        d = root.body ? root : root.documentElement;
    var l = terms.length;
    if (l === 0) return [];
        for (var i = 0; i < l; i++) {
        terms[i].search(cssSelectorExpression);
        terms[i] = {
            tagName: RegExp.$1.toLowerCase(),
            className: RegExp.$2.toLowerCase(),
            id: RegExp.$3
        };
    }
        var term = terms[0],
        potentials = [],
        nextPotentials = [];
    if (term.id) {
        var elt = d.getElementById(term.id);
        if (elt && 
            ( (root === d) || 
              (root.contains && root.contains(elt)) || 
              this._contains(root, elt) ) ) {
            potentials = [elt];
        }
    }
    else if (term.tagName) {
        potentials = root.getElementsByTagName(term.tagName);
    }
    else if (term.className) {
        potentials = this.getElementsByClassName(term.className, root);
    }
    term.className = ' ' + term.className + ' ';
        for (i = 1; i < l; i++) {
        var m = potentials.length;
        if (m === 0) return [];
        var previousTerm = term;
        term = terms[i];
        term.className = ' ' + term.className + ' ';
        for (var j = 0; j < m; j++) {
            var potential = potentials[j];
            if (!this._testTerm(previousTerm, potential)) continue;
                        if (term.id) {
                elt = d.getElementById(term.id);
                if (elt && 
                    ( (potential === d) ||
                      (potential.contains && potential.contains(elt)) ||
                      this._contains(potential, elt) ) ) {
                    nextPotentials[nextPotentials.length] = elt;
                }
            }
            else if (term.tagName) {
                var elts = potential.getElementsByTagName(term.tagName);
                for (var k = 0, n = elts.length; k < n; k++) {
                    nextPotentials[nextPotentials.length] = elts[k];
                }
            }
            else {
                elts = potential.getElementsByTagName('*');
                n = elts.length;
                for (k = 0; k < n; k++) {
                    elt = elts[k];
                    if ((' ' + elt.className + ' ').indexOf(term.className) !== -1) {
                        nextPotentials[nextPotentials.length] = elt;
                    }
                }
            }
        }
        potentials = nextPotentials;
        nextPotentials = [];
    }
        m = potentials.length;
    for (j = 0; j < m; j++) {
        potential = potentials[j];
        if (this._testTerm(term, potential) && !Array.contains(nextPotentials, potential)) {
            nextPotentials[nextPotentials.length] = potential;
        }
    }
    return nextPotentials;
}
Sys.UI.DomElement.selectElement = function Sys$UI$DomElement$selectElement(selector, element) {
    /// <param name="selector" type="String"></param>
    /// <param name="element" domElement="true" optional="true" mayBeNull="true"></param>
    /// <returns domElement="true" mayBeNull="true"></returns>
    var e = Function._validateParams(arguments, [
        {name: "selector", type: String},
        {name: "element", mayBeNull: true, domElement: true, optional: true}
    ]);
    if (e) throw e;
    var potentials = Sys.UI.DomElement.selectAllElements(selector, element);
    return (potentials.length > 0) ? potentials[0] : null;
}


if(typeof(Sys)!=='undefined')Sys.Application.notifyScriptLoaded();
