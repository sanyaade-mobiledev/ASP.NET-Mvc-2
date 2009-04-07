// Name:        WebParts.debug.js
// Assembly:    Microsoft.Web.Preview
// Version:     2.0.21022.0
// FileVersion: 2.0.21022.0
Type.registerNamespace('Sys.Preview.UI.WebParts');
var __wpm = null;
function Point(x, y) {
    this.x = x;
    this.y = y;
}
function __getOffset(el, ev) {
    if (ev.offsetY && ev.offsetY) {
        return { x: ev.offsetX, y: ev.offsetY };
    }
    var location = Sys.UI.DomElement.getLocation(el);
    var offsetX = window.pageXOffset + ev.clientX - location.x;
    var offsetY = window.pageYOffset + ev.clientY - location.y;
    return { x: offsetX, y: offsetY };
}
function cancelEvent(e) {
    if (e.preventDefault && e.stopPropagation) {
        e.preventDefault();
        e.stopPropagation();
    }
    else if (window.event) {
        window.event.returnValue = false;
        window.event.cancelBubble = true;
    }
        }
function __wpTranslateOffset(x, y, offsetElement, relativeToElement, includeScroll) {
    while ((typeof(offsetElement) != "undefined") && (offsetElement != null) && (offsetElement != relativeToElement)) {
        x += offsetElement.offsetLeft;
        y += offsetElement.offsetTop;
                                                                        var tagName = offsetElement.tagName;
        if ((tagName != "TABLE") && (tagName != "BODY")) {
                        if (offsetElement.clientLeft) {
                x += offsetElement.clientLeft;
            }
                        if (offsetElement.clientTop) {
                y += offsetElement.clientTop;
            }
        }
                                if (includeScroll && (tagName != "BODY")) {
            x -= offsetElement.scrollLeft;
            y -= offsetElement.scrollTop;
        }
        offsetElement = offsetElement.offsetParent;
    }
    return new Point(x, y);
}
function __wpGetPageEventLocation(event, includeScroll) {
    if ((typeof(event) == "undefined") || (event == null)) {
        event = window._event;
    }
    var offset = __getOffset(event.target, event.rawEvent);
    return __wpTranslateOffset(offset.x, offset.y, event.target, null, includeScroll);
}
function WebPart(webPartElement, webPartTitleElement, zone, zoneIndex, allowZoneChange) {
    this.webPartElement = webPartElement;
    this.allowZoneChange = allowZoneChange;
    this.zone = zone;
    this.zoneIndex = zoneIndex;
    this.title = webPartTitleElement ? (webPartTitleElement.textContent || webPartTitleElement.innerText || "") : "";
    webPartElement.__webPart = this;
    if ((typeof(webPartTitleElement) != "undefined") && (webPartTitleElement != null)) {
        webPartTitleElement.style.cursor = "move";
                                            }
    this.UpdatePosition = WebPart_UpdatePosition;
    this.Dispose = WebPart_Dispose;
}
function WebPart_Dispose() {
    this.webPartElement.__webPart = null    
}
function WebPart_UpdatePosition() {
    var location = __wpTranslateOffset(0, 0, this.webPartElement, null, false);
    this.middleX = location.x + this.webPartElement.offsetWidth / 2;
    this.middleY = location.y + this.webPartElement.offsetHeight / 2;
}
function Zone(zoneElement, zoneIndex, uniqueID, isVertical, allowLayoutChange, highlightColor) {
    var webPartTable = null;
    if (zoneElement.rows.length == 1) {
        webPartTableContainer = zoneElement.rows[0].cells[0];
    }
    else {
        webPartTableContainer = zoneElement.rows[1].cells[0];
    }
    var i;
    for (i = 0; i < webPartTableContainer.childNodes.length; i++) {
        var node = webPartTableContainer.childNodes[i];
        if (node.tagName == "TABLE") {
            webPartTable = node;
            break;
        }
    }
    this.zoneElement = zoneElement;
    this.zoneIndex = zoneIndex;
    this.webParts = new Array();
    this.uniqueID = uniqueID;
    this.isVertical = isVertical;
    this.allowLayoutChange = allowLayoutChange;
    this.allowDrop = false;
    this.webPartTable = webPartTable;
    this.highlightColor = highlightColor;
    this.savedBorderColor = (webPartTable != null) ? webPartTable.style.borderColor : null;
    this.dropCueElements = new Array();
    if (webPartTable != null) {
        if (isVertical) {
            for (i = 0; i < webPartTable.rows.length; i += 2) {
                this.dropCueElements[i / 2] = webPartTable.rows[i].cells[0].childNodes[0];
            }
        }
        else {
            for (i = 0; i < webPartTable.rows[0].cells.length; i += 2) {
                this.dropCueElements[i / 2] = webPartTable.rows[0].cells[i].childNodes[0];
            }
        }
    }
    this.AddWebPart = Zone_AddWebPart;
    this.GetWebPartIndex = Zone_GetWebPartIndex;
    this.ToggleDropCues = Zone_ToggleDropCues;
    this.UpdatePosition = Zone_UpdatePosition;
    this.Dispose = Zone_Dispose;
    webPartTable.__zone = this;
        }
function Zone_Dispose() {
    for (var i = 0; i < this.webParts.length; i++) {
        this.webParts[i].Dispose();
    }
    this.webPartTable.__zone = null;
}
function Zone_AddWebPart(webPartElement, webPartTitleElement, allowZoneChange) {
    var webPart = null;
    var zoneIndex = this.webParts.length;
    if (this.allowLayoutChange && __wpm.IsDragDropEnabled()) {
        webPart = new WebPart(webPartElement, webPartTitleElement, this, zoneIndex, allowZoneChange);
    }
    else {
        webPart = new WebPart(webPartElement, null, this, zoneIndex, allowZoneChange);
    }
    this.webParts[zoneIndex] = webPart;
    return webPart;
}
function Zone_ToggleDropCues(show, index, ignoreOutline) {
    if (ignoreOutline == false) {
        this.webPartTable.style.borderColor = (show ? this.highlightColor : this.savedBorderColor);
    }
    if (index == -1) {
        return;
    }
    var dropCue = this.dropCueElements[index];
    if (dropCue && dropCue.style) {
        if (dropCue.style.height == "100%" && !dropCue.webPartZoneHorizontalCueResized) {
                                                                        
                                    var oldParentHeight = dropCue.parentNode.clientHeight;
            
            var realHeight = oldParentHeight - 10;
            dropCue.style.height = realHeight + "px";
            var dropCueVerticalBar = dropCue.getElementsByTagName("DIV")[0];
            if (dropCueVerticalBar && dropCueVerticalBar.style) {
                dropCueVerticalBar.style.height = dropCue.style.height;
                                                var heightDiff = (dropCue.parentNode.clientHeight - oldParentHeight);
                if (heightDiff) {
                    dropCue.style.height = (realHeight - heightDiff) + "px";
                    dropCueVerticalBar.style.height = dropCue.style.height;
                }
            }
            dropCue.webPartZoneHorizontalCueResized = true;
        }
        dropCue.style.visibility = (show ? "visible" : "hidden");
    }
}
function Zone_GetWebPartIndex(location) {
    var x = location.x;
    var y = location.y;
    if ((x < this.webPartTableLeft) || (x > this.webPartTableRight) ||
        (y < this.webPartTableTop) || (y > this.webPartTableBottom)) {
        return -1;
    }
    var vertical = this.isVertical;
    var webParts = this.webParts;
    var webPartsCount = webParts.length;
    for (var i = 0; i < webPartsCount; i++) {
        var webPart = webParts[i];
        if (vertical) {
            if (y < webPart.middleY) {
                return i;
            }
        }
        else {
            if (x < webPart.middleX) {
                return i;
            }
        }
    }
    return webPartsCount;
}
function Zone_UpdatePosition() {
    var topLeft = __wpTranslateOffset(0, 0, this.webPartTable, null, false);
    this.webPartTableLeft = topLeft.x;
    this.webPartTableTop = topLeft.y;
    this.webPartTableRight = (this.webPartTable != null) ? topLeft.x + this.webPartTable.offsetWidth : topLeft.x;
    this.webPartTableBottom = (this.webPartTable != null) ? topLeft.y + this.webPartTable.offsetHeight : topLeft.y;
    for (var i = 0; i < this.webParts.length; i++) {
        this.webParts[i].UpdatePosition();
    }
}
function WebPartMenu(menuLabelElement, menuDropDownElement, menuElement) {
    this.menuLabelElement = menuLabelElement;
    this.menuDropDownElement = menuDropDownElement;
    this.menuElement = menuElement;
    this.menuLabelElement.__menu = this;
    this.menuLabelElement.attachEvent('onclick', WebPartMenu_OnClick);
    this.menuLabelElement.attachEvent('onkeypress', WebPartMenu_OnKeyPress);
    this.menuLabelElement.attachEvent('onmouseenter', WebPartMenu_OnMouseEnter);
    this.menuLabelElement.attachEvent('onmouseleave', WebPartMenu_OnMouseLeave);
    if ((typeof(this.menuDropDownElement) != "undefined") && (this.menuDropDownElement != null)) {
        this.menuDropDownElement.__menu = this;
    }
    this.menuItemStyle = "";
    this.menuItemHoverStyle = "";
    this.popup = null;
    this.hoverClassName = "";
    this.hoverColor = "";
    this.oldColor = this.menuLabelElement.style.color;
    this.oldTextDecoration = this.menuLabelElement.style.textDecoration;
    this.oldClassName = this.menuLabelElement.className;
    this.Show = WebPartMenu_Show;
    this.Hide = WebPartMenu_Hide;
    this.Hover = WebPartMenu_Hover;
    this.Unhover = WebPartMenu_Unhover;
    this.Dispose = WebPartMenu_Dispose;
    var menu = this;
    Sys.Application.add_unload(function() {menu.Dispose();});
    }
function WebPartMenu_Dispose() {
    this.menuLabelElement.__menu = null;
    this.menuDropDownElement.__menu = null;
}
function WebPartMenu_Show() {
    if ((typeof(__wpm.menu) != "undefined") && (__wpm.menu != null)) {
        __wpm.menu.Hide();
    }
    var menuHTML =
        "<html><head><style>" +
        "a.menuItem, a.menuItem:Link { display: block; padding: 1px; text-decoration: none; " + this.itemStyle + " }" +
        "a.menuItem:Hover { " + this.itemHoverStyle + " }" +
        "</style><body scroll=\"no\" style=\"border: none; margin: 0; padding: 0;\" ondragstart=\"window.event.returnValue=false;\" onclick=\"popup.hide()\">" +
        this.menuElement.innerHTML +
        "<body></html>";
    var width = 16;
    var height = 16;
    this.popup = window.createPopup();
    __wpm.menu = this;
    var popupDocument = this.popup.document;
    popupDocument.write(menuHTML);
    this.popup.show(0, 0, width, height);
    var popupBody = popupDocument.body;
    width = popupBody.scrollWidth;
    height = popupBody.scrollHeight;
    if (width < this.menuLabelElement.offsetWidth) {
        width = this.menuLabelElement.offsetWidth + 16;
    }
    if (this.menuElement.innerHTML.indexOf("progid:DXImageTransform.Microsoft.Shadow") != -1) {
        popupBody.style.paddingRight = "4px";
    }
    popupBody.__wpm = __wpm;
    popupBody.__wpmDeleteWarning = __wpmDeleteWarning;
    popupBody.__wpmCloseProviderWarning = __wpmCloseProviderWarning;
    popupBody.popup = this.popup;
    this.popup.hide();
    this.popup.show(0, this.menuLabelElement.offsetHeight, width, height, this.menuLabelElement);
}
function WebPartMenu_Hide() {
    if (__wpm.menu == this) {
        __wpm.menu = null;
        if ((typeof(this.popup) != "undefined") && (this.popup != null)) {
            this.popup.hide();
            this.popup = null;
        }
    }
}
function WebPartMenu_Hover() {
    if (this.labelHoverClassName != "") {
        this.menuLabelElement.className = this.menuLabelElement.className + " " + this.labelHoverClassName;
    }
    if (this.labelHoverColor != "") {
        this.menuLabelElement.style.color = this.labelHoverColor;
    }
}
function WebPartMenu_Unhover() {
    if (this.labelHoverClassName != "") {
        this.menuLabelElement.style.textDecoration = this.oldTextDecoration;
        this.menuLabelElement.className = this.oldClassName;
    }
    if (this.labelHoverColor != "") {
        this.menuLabelElement.style.color = this.oldColor;
    }
}
function WebPartMenu_OnClick() {
    var menu = window.event.srcElement.__menu;
    if ((typeof(menu) != "undefined") && (menu != null)) {
        cancelEvent(window.event);
        menu.Show();
    }
}
function WebPartMenu_OnKeyPress() {
    if (window.event.keyCode == 13) {
        var menu = window.event.srcElement.__menu;
        if ((typeof(menu) != "undefined") && (menu != null)) {
            cancelEvent(window.event);
            menu.Show();
        }
    }
}
function WebPartMenu_OnMouseEnter() {
    var menu = window.event.srcElement.__menu;
    if ((typeof(menu) != "undefined") && (menu != null)) {
        menu.Hover();
    }
}
function WebPartMenu_OnMouseLeave() {
    var menu = window.event.srcElement.__menu;
    if ((typeof(menu) != "undefined") && (menu != null)) {
        menu.Unhover();
    }
}
function WebPartManager() {
    this.overlayContainerElement = null;
    this.zones = new Array();
        this.menu = null;
    this.draggedWebPart = null;
    this.AddZone = WebPartManager_AddZone;
    this.IsDragDropEnabled = WebPartManager_IsDragDropEnabled;
                                this.ShowHelp = WebPartManager_ShowHelp;
        this.Execute = WebPartManager_Execute;
    this.SubmitPage = WebPartManager_SubmitPage;
    this.UpdatePositions = WebPartManager_UpdatePositions;
    Sys.Application.add_unload(WebPartManager_Dispose);
    }
function WebPartManager_Dispose() {
    for (var i = 0; i < __wpm.zones.length; i++) {
        __wpm.zones[i].Dispose();
    }
}
function WebPartManager_AddZone(zoneElement, uniqueID, isVertical, allowLayoutChange, highlightColor) {
    var zoneIndex = this.zones.length;
    var zone = new Zone(zoneElement, zoneIndex, uniqueID, isVertical, allowLayoutChange, highlightColor);
    this.zones[zoneIndex] = zone;
    return zone;
}
function WebPartManager_IsDragDropEnabled() {
    return ((typeof(this.overlayContainerElement) != "undefined") && (this.overlayContainerElement != null));
}
function WebPartManager_Execute(script) {
    if (this.menu) {
        this.menu.Hide();
    }
    var scriptReference = new Function(script);
    return (scriptReference() != false);
}
function WebPartManager_ShowHelp(helpUrl, helpMode) {
    if ((typeof(this.menu) != "undefined") && (this.menu != null)) {
        this.menu.Hide();
    }
    if (helpMode == 0 || helpMode == 1) {
        if (helpMode == 0) {
            var dialogInfo = "edge: Sunken; center: yes; help: no; resizable: yes; status: no";
            window.showModalDialog(helpUrl, null, dialogInfo);
        }
        else {
            window.open(helpUrl, null, "scrollbars=yes,resizable=yes,status=no,toolbar=no,menubar=no,location=no");
        }
    }
    else if (helpMode == 2) {
        window.location = helpUrl;
    }
}
function WebPartManager_UpdatePositions() {
    for (var i = 0; i < this.zones.length; i++) {
        this.zones[i].UpdatePosition();
    }
}
function WebPartManager_SubmitPage(eventTarget, eventArgument) {
    if ((typeof(this.menu) != "undefined") && (this.menu != null)) {
        this.menu.Hide();
    }
    __doPostBack(eventTarget, eventArgument);
}
Sys.Preview.UI.Controls.WebParts.WebPart = function Sys$Preview$UI$Controls$WebParts$WebPart(associatedElement) {
    Sys.Preview.UI.Controls.WebParts.WebPart.initializeBase(this, [associatedElement]);
    var _titleElement;
    var _zone;
    var _zoneIndex;
    var _allowZoneChange = true;
    this.get_allowZoneChange = function this$get_allowZoneChange() {
        return _allowZoneChange;
    }
    this.set_allowZoneChange = function this$set_allowZoneChange(value) {
        _allowZoneChange = value;
    }
    this.get_titleElement = function this$get_titleElement() {
        return _titleElement;
    }
    this.set_titleElement = function this$set_titleElement(value) {
        _titleElement = value;
    }
    this.get_zone = function this$get_zone() {
        return _zone;
    }
    this.set_zone = function this$set_zone(value) {
        _zone = value;
    }
    this.get_zoneIndex = function this$get_zoneIndex() {
        return _zoneIndex;
    }
    this.set_zoneIndex = function this$set_zoneIndex(value) {
        _zoneIndex = value;
    }
    this.initialize = function this$initialize() {
        Sys.Preview.UI.Controls.WebParts.WebPart.callBaseMethod(this, "initialize");
                if (_titleElement && _zone.get_webPartManager().get_allowPageDesign() && _zone.get_allowLayoutChange()) {
                        var element = this.get_element();
                                                
                        Sys.UI.DomEvent.addHandler(_titleElement, "mousedown", Function.createDelegate(this, mouseDownHandler));
            
                                                _titleElement.style.cursor = "move";
        }
    }
    function mouseDownHandler(domEvent) {         window._event = domEvent;
                                _zone.startDragDrop(this);
                domEvent.preventDefault();
    }
}
Sys.Preview.UI.Controls.WebParts.WebPart.registerClass("Sys.Preview.UI.Controls.WebParts.WebPart", Sys.UI.Control);
Sys.Preview.UI.Controls.WebParts.WebPart.descriptor = {
    properties: [ {name: 'titleElement', isDomElement: true},
                  {name: 'zone', type: Object},
                  {name: 'zoneIndex', type: Number},
                  {name: 'allowZoneChange', type: Boolean} ]
}
Sys.Preview.UI.Controls.WebParts.WebPartManager = function Sys$Preview$UI$Controls$WebParts$WebPartManager(associatedElement) {
    Sys.Preview.UI.Controls.WebParts.WebPartManager.initializeBase(this, [associatedElement]);
    var _allowPageDesign;
    this.get_allowPageDesign = function this$get_allowPageDesign() {
        return _allowPageDesign;
    }
    this.set_allowPageDesign = function this$set_allowPageDesign(value) {
        _allowPageDesign = value;
    }
    this.initialize = function this$initialize() {
        Sys.Preview.UI.Controls.WebParts.WebPartManager.callBaseMethod(this, "initialize");
        var baseShowHelp = Function.createDelegate(__wpm, __wpm.ShowHelp);
        __wpm.ShowHelp = function __wpm$ShowHelp(helpUrl, helpMode) {
            var supportedHelpMode;
                        if (helpMode == 0 && !window.showModalDialog) {
                supportedHelpMode = 1;
            }
            else {
                supportedHelpMode = helpMode;
            }
            baseShowHelp(helpUrl, supportedHelpMode);
        }
    }
}
Sys.Preview.UI.Controls.WebParts.WebPartManager.registerClass("Sys.Preview.UI.Controls.WebParts.WebPartManager", Sys.UI.Control);
Sys.Preview.UI.Controls.WebParts.WebPartManager.descriptor = {
    properties: [ {name: 'allowPageDesign', type: Boolean} ]
}
Sys.Preview.UI.Controls.WebParts.WebPartZone = function Sys$Preview$UI$Controls$WebParts$WebPartZone(associatedElement) {
    Sys.Preview.UI.Controls.WebParts.WebPartZone.initializeBase(this, [associatedElement]);
        var _dataType = "WebPart";
    var _allowLayoutChange = true;
    var _uniqueId;
    var _webPartManager;
    var _dropIndex = -1;
    var _dropTargetRegistered = false;
    var _floatContainer;
    var _whidbeyZone;
    this.get_allowLayoutChange = function this$get_allowLayoutChange() {
        return _allowLayoutChange;
    }
    this.set_allowLayoutChange = function this$set_allowLayoutChange(value) {
        _allowLayoutChange = value;
    }
    this.get_uniqueId = function this$get_uniqueId() {
        return _uniqueId;
    }
    this.set_uniqueId = function this$set_uniqueId(value) {
        _uniqueId = value;
    }
    this.get_webPartManager = function this$get_webPartManager() {
        return _webPartManager;
    }
    this.set_webPartManager = function this$set_webPartManager(value) {
        _webPartManager = value;
    }
    function createFloatContainer(webPart) {
                var floatContainer = document.createElement("div");
                                floatContainer.style.filter = "progid:DXImageTransform.Microsoft.BasicImage(opacity=0.75);";
                floatContainer.style.opacity = "0.75";
        floatContainer.style.position = "absolute";
        floatContainer.style.zIndex = 32000;
        var webPartElement = webPart.get_element();
        var currentLocation = Sys.UI.DomElement.getLocation(webPartElement);
        Sys.UI.DomElement.setLocation(floatContainer, currentLocation.x, currentLocation.y);
        floatContainer.style.display = "block";
                floatContainer.style.width = webPartElement.offsetWidth + "px";
        floatContainer.style.height = webPartElement.offsetHeight + "px";
        floatContainer.appendChild(webPartElement.cloneNode(true));
        return floatContainer;
    }
    
    this.dispose = function this$dispose() {
        Sys.Preview.UI.Controls.WebParts.WebPartZone.callBaseMethod(this, "dispose");
        if (_dropTargetRegistered) {
            Sys.Preview.UI.DragDropManager.unregisterDropTarget(this);
        }
    }
    this.initialize = function this$initialize() {
        Sys.Preview.UI.Controls.WebParts.WebPartZone.callBaseMethod(this, "initialize");
        var element = this.get_element();
        for (var i=0; i < __wpm.zones.length; i++) {
            if (__wpm.zones[i].zoneElement == element) {
                _whidbeyZone = __wpm.zones[i];
                break;
            }
        }
                        
        if (_webPartManager.get_allowPageDesign() && _allowLayoutChange) {
            Sys.Preview.UI.DragDropManager.registerDropTarget(this);
            _dropTargetRegistered = true;
        }
    }
    this.startDragDrop = function this$startDragDrop(webPart) {
                __wpm.UpdatePositions();
        _floatContainer = createFloatContainer(webPart);
        document.body.appendChild(_floatContainer);
                Sys.Preview.UI.DragDropManager.startDragDrop(this, _floatContainer, webPart);
    }
            this.get_dragDataType = function this$get_dragDataType() {
        return _dataType;
    }
        this.getDragData = function this$getDragData(context) {
        return context;
    }
        this.get_dragMode = function this$get_dragMode() {
        return Sys.Preview.UI.DragMode.Copy;
    }
        this.onDragStart = function this$onDragStart() {
    }
        this.onDrag = function this$onDrag() {
    }
        this.onDragEnd = function this$onDragEnd(cancelled) {
                        
        document.body.removeChild(_floatContainer);
    }
        this.get_dropTargetElement = function this$get_dropTargetElement() {
        return _whidbeyZone.webPartTable;
    }
        this.canDrop = function this$canDrop(dragMode, dataType, data) {
        var webPart = data;
                        
        return ((dragMode == Sys.Preview.UI.DragMode.Copy) &&
                (dataType == _dataType) &&
                (Sys.Preview.UI.Controls.WebParts.WebPart.isInstanceOfType(webPart)) &&
                (webPart.get_allowZoneChange() || (webPart.get_zone() == this)) &&
                (getDropIndex() != -1));
    }
        this.drop = function this$drop(dragMode, dataType, data) {
                                
                        _whidbeyZone.ToggleDropCues(false, _dropIndex, false);
        var webPart = data;
        if (webPartMoved(webPart, this, _dropIndex)) {
            var eventTarget = _uniqueId;
            var eventArgument = "Drag:" + webPart.get_id() + ":" + _dropIndex;
            __doPostBack(eventTarget, eventArgument);
        }
    }
    function webPartMoved(webPart, dropZone, dropIndex) {
        if (dropZone != webPart.get_zone()) {
            return true;
        }
                        if (dropIndex == webPart.get_zoneIndex() || dropIndex == (webPart.get_zoneIndex() + 1)) {
            return false;
        }
        return true;
    }
        this.onDragEnterTarget = function this$onDragEnterTarget(dragMode, dataType, data) {
        var dropIndex = getDropIndex();
        _whidbeyZone.ToggleDropCues(true, dropIndex, false);
        _dropIndex = dropIndex;
    }
        this.onDragLeaveTarget = function this$onDragLeaveTarget(dragMode, dataType, data) {
        _whidbeyZone.ToggleDropCues(false, _dropIndex, false);
    }
        this.onDragInTarget = function this$onDragInTarget() {
        var dropIndex = getDropIndex();
        if (dropIndex != _dropIndex) {
            _whidbeyZone.ToggleDropCues(false, _dropIndex, true);
            _whidbeyZone.ToggleDropCues(true, dropIndex, true);
            _dropIndex = dropIndex;
        }
    }
    function getDropIndex() {
        var pageLocation = __wpGetPageEventLocation(window._event, false);
        return _whidbeyZone.GetWebPartIndex(pageLocation);
    }
}
Sys.Preview.UI.Controls.WebParts.WebPartZone.registerClass("Sys.Preview.UI.Controls.WebParts.WebPartZone",
    Sys.UI.Control, Sys.Preview.UI.IDragSource, Sys.Preview.UI.IDropTarget);
Sys.Preview.UI.Controls.WebParts.WebPartZone.descriptor = {
    properties: [ {name: 'uniqueId', type: String},
                  {name: 'webPartManager', type: Object},
                  {name: 'allowLayoutChange', type: Boolean} ]
}


if(typeof(Sys)!=='undefined')Sys.Application.notifyScriptLoaded();
