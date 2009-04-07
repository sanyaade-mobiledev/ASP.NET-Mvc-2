// Name:        PreviewWebParts.debug.js
// Assembly:    Microsoft.Web.Preview
// Version:     2.0.21022.0
// FileVersion: 2.0.21022.0
Type.registerNamespace('Sys.Preview.UI.Controls.WebParts');
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
