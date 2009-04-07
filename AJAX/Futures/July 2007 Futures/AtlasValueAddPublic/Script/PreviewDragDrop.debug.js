// Name:        PreviewDragDrop.debug.js
// Assembly:    Microsoft.Web.Preview
// Version:     2.0.21022.0
// FileVersion: 2.0.21022.0
Sys.Preview.UI._DragDropManager = function Sys$Preview$UI$_DragDropManager() {
}
    function Sys$Preview$UI$_DragDropManager$add_dragStart(handler) {
var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
if (e) throw e;
        this.get_events().addHandler('dragStart', handler);
    }
    function Sys$Preview$UI$_DragDropManager$remove_dragStart(handler) {
var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
if (e) throw e;
        this.get_events().removeHandler('dragStart', handler);
    }
    function Sys$Preview$UI$_DragDropManager$get_events() {
if (arguments.length !== 0) throw Error.parameterCount();
                            if (!this._events) {
            this._events = new Sys.EventHandlerList();
        }
        return this._events;
    }
    function Sys$Preview$UI$_DragDropManager$add_dragStop(handler) {
var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
if (e) throw e;
        this.get_events().addHandler('dragStop', handler);
    }
    function Sys$Preview$UI$_DragDropManager$remove_dragStop(handler) {
var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
if (e) throw e;
        this.get_events().removeHandler('dragStop', handler);
    }
    function Sys$Preview$UI$_DragDropManager$_getInstance() {
        if (!this._instance) {
            if (Sys.Browser.agent === Sys.Browser.InternetExplorer) {
                this._instance = new Sys.Preview.UI.IEDragDropManager();
            }
            else {
                this._instance = new Sys.Preview.UI.GenericDragDropManager();
            }
            this._instance.initialize();
            this._instance.add_dragStart(Function.createDelegate(this, this._raiseDragStart));
            this._instance.add_dragStop(Function.createDelegate(this, this._raiseDragStop));
        }
        return this._instance;
    }
    function Sys$Preview$UI$_DragDropManager$startDragDrop(dragSource, dragVisual, context) {
        this._getInstance().startDragDrop(dragSource, dragVisual, context);
    }
    function Sys$Preview$UI$_DragDropManager$registerDropTarget(target) {
        this._getInstance().registerDropTarget(target);
    }
    function Sys$Preview$UI$_DragDropManager$unregisterDropTarget(target) {
        this._getInstance().unregisterDropTarget(target);
    }
    function Sys$Preview$UI$_DragDropManager$dispose() {
        delete this._events;
        Sys.Application.unregisterDisposableObject(this);
        Sys.Application.removeComponent(this);
    }
    function Sys$Preview$UI$_DragDropManager$_raiseDragStart(sender, eventArgs) {
        var handler = this.get_events().getHandler('dragStart');
        if(handler) {
            handler(this, eventArgs);
        }
    }
    function Sys$Preview$UI$_DragDropManager$_raiseDragStop(sender, eventArgs) {
        var handler = this.get_events().getHandler('dragStop');
        if(handler) {
            handler(this, eventArgs);
        }
    }
Sys.Preview.UI._DragDropManager.prototype = {
    _instance: null,
    _events: null,
    
    add_dragStart: Sys$Preview$UI$_DragDropManager$add_dragStart,
    remove_dragStart: Sys$Preview$UI$_DragDropManager$remove_dragStart,
    
    get_events: Sys$Preview$UI$_DragDropManager$get_events,
    
    add_dragStop: Sys$Preview$UI$_DragDropManager$add_dragStop,
    remove_dragStop: Sys$Preview$UI$_DragDropManager$remove_dragStop,
    
    _getInstance: Sys$Preview$UI$_DragDropManager$_getInstance,
    
    startDragDrop: Sys$Preview$UI$_DragDropManager$startDragDrop,
    
    registerDropTarget: Sys$Preview$UI$_DragDropManager$registerDropTarget,
    
    unregisterDropTarget: Sys$Preview$UI$_DragDropManager$unregisterDropTarget,
    
    dispose: Sys$Preview$UI$_DragDropManager$dispose,
    
    _raiseDragStart: Sys$Preview$UI$_DragDropManager$_raiseDragStart,
    
    _raiseDragStop: Sys$Preview$UI$_DragDropManager$_raiseDragStop
}
Sys.Preview.UI._DragDropManager.registerClass('Sys.Preview.UI._DragDropManager');
Sys.Preview.UI.DragDropManager = new Sys.Preview.UI._DragDropManager();
Sys.Preview.UI.DragDropEventArgs = function Sys$Preview$UI$DragDropEventArgs(dragMode, dragDataType, dragData) {
    this._dragMode = dragMode;
    this._dataType = dragDataType;
    this._data = dragData;
}
    function Sys$Preview$UI$DragDropEventArgs$get_dragMode() {
if (arguments.length !== 0) throw Error.parameterCount();
        return this._dragMode || null;
    }
    function Sys$Preview$UI$DragDropEventArgs$get_dragDataType() {
if (arguments.length !== 0) throw Error.parameterCount();
        return this._dataType || null;
    }
    function Sys$Preview$UI$DragDropEventArgs$get_dragData() {
if (arguments.length !== 0) throw Error.parameterCount();
        return this._data || null;
    }
Sys.Preview.UI.DragDropEventArgs.prototype = {
    get_dragMode: Sys$Preview$UI$DragDropEventArgs$get_dragMode,
    get_dragDataType: Sys$Preview$UI$DragDropEventArgs$get_dragDataType,
    get_dragData: Sys$Preview$UI$DragDropEventArgs$get_dragData
}
Sys.Preview.UI.DragDropEventArgs.registerClass('Sys.Preview.UI.DragDropEventArgs');
Sys.Preview.UI.IDragSource = function Sys$Preview$UI$IDragSource() {
}
    function Sys$Preview$UI$IDragSource$get_dragDataType() { throw Error.notImplemented(); }
    function Sys$Preview$UI$IDragSource$getDragData() { throw Error.notImplemented(); }
    function Sys$Preview$UI$IDragSource$get_dragMode() { throw Error.notImplemented(); }
    function Sys$Preview$UI$IDragSource$onDragStart() { throw Error.notImplemented(); }
    function Sys$Preview$UI$IDragSource$onDrag() { throw Error.notImplemented(); }
    function Sys$Preview$UI$IDragSource$onDragEnd() { throw Error.notImplemented(); }
Sys.Preview.UI.IDragSource.prototype = {
        get_dragDataType: Sys$Preview$UI$IDragSource$get_dragDataType,
        getDragData: Sys$Preview$UI$IDragSource$getDragData,
        get_dragMode: Sys$Preview$UI$IDragSource$get_dragMode,
        onDragStart: Sys$Preview$UI$IDragSource$onDragStart,
        onDrag: Sys$Preview$UI$IDragSource$onDrag,
        onDragEnd: Sys$Preview$UI$IDragSource$onDragEnd
}
Sys.Preview.UI.IDragSource.registerInterface('Sys.Preview.UI.IDragSource');
///////////////////////////////////////////////////////////////////////////////
Sys.Preview.UI.IDropTarget = function Sys$Preview$UI$IDropTarget() {
}
    function Sys$Preview$UI$IDropTarget$get_dropTargetElement() { throw Error.notImplemented(); }
    function Sys$Preview$UI$IDropTarget$canDrop() { throw Error.notImplemented(); }
    function Sys$Preview$UI$IDropTarget$drop() { throw Error.notImplemented(); }
    function Sys$Preview$UI$IDropTarget$onDragEnterTarget() { throw Error.notImplemented(); }
    function Sys$Preview$UI$IDropTarget$onDragLeaveTarget() { throw Error.notImplemented(); }
    function Sys$Preview$UI$IDropTarget$onDragInTarget() { throw Error.notImplemented(); }
Sys.Preview.UI.IDropTarget.prototype = {
    get_dropTargetElement: Sys$Preview$UI$IDropTarget$get_dropTargetElement,
        canDrop: Sys$Preview$UI$IDropTarget$canDrop,
        drop: Sys$Preview$UI$IDropTarget$drop,
        onDragEnterTarget: Sys$Preview$UI$IDropTarget$onDragEnterTarget,
        onDragLeaveTarget: Sys$Preview$UI$IDropTarget$onDragLeaveTarget,
        onDragInTarget: Sys$Preview$UI$IDropTarget$onDragInTarget
}
Sys.Preview.UI.IDropTarget.registerInterface('Sys.Preview.UI.IDropTarget');
Sys.Preview.UI.DragMode = function Sys$Preview$UI$DragMode() {
    throw Error.invalidOperation();
}
Sys.Preview.UI.DragMode.prototype = {
    Copy: 0,
    Move: 1
}
Sys.Preview.UI.DragMode.registerEnum('Sys.Preview.UI.DragMode');
Sys.Preview.UI.IEDragDropManager = function Sys$Preview$UI$IEDragDropManager() {
    Sys.Preview.UI.IEDragDropManager.initializeBase(this);
}
    function Sys$Preview$UI$IEDragDropManager$add_dragStart(handler) {
var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
if (e) throw e;
        this.get_events().addHandler("dragStart", handler);
    }
    function Sys$Preview$UI$IEDragDropManager$remove_dragStart(handler) {
var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
if (e) throw e;
        this.get_events().removeHandler("dragStart", handler);
    }
    function Sys$Preview$UI$IEDragDropManager$add_dragStop(handler) {
var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
if (e) throw e;
        this.get_events().addHandler("dragStop", handler);
    }
    function Sys$Preview$UI$IEDragDropManager$remove_dragStop(handler) {
var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
if (e) throw e;
        this.get_events().removeHandler("dragStop", handler);
    }
    function Sys$Preview$UI$IEDragDropManager$initialize() {
        Sys.Preview.UI.IEDragDropManager.callBaseMethod(this, 'initialize');
        this._mouseUpHandler = Function.createDelegate(this, this.mouseUpHandler);
        this._documentMouseMoveHandler = Function.createDelegate(this, this.documentMouseMoveHandler);
        this._documentDragOverHandler = Function.createDelegate(this, this.documentDragOverHandler);
        this._dragStartHandler = Function.createDelegate(this, this.dragStartHandler);
        this._mouseMoveHandler = Function.createDelegate(this, this.mouseMoveHandler);
        this._dragEnterHandler = Function.createDelegate(this, this.dragEnterHandler);
        this._dragLeaveHandler = Function.createDelegate(this, this.dragLeaveHandler);
        this._dragOverHandler = Function.createDelegate(this, this.dragOverHandler);
        this._dropHandler = Function.createDelegate(this, this.dropHandler);
    }
    function Sys$Preview$UI$IEDragDropManager$dispose() {
        if(this._dropTargets) {
            for (var i = 0; i < this._dropTargets; i++) {
                this.unregisterDropTarget(this._dropTargets[i]);
            }
            this._dropTargets = null;
        }
        Sys.Preview.UI.IEDragDropManager.callBaseMethod(this, 'dispose');
    }
    function Sys$Preview$UI$IEDragDropManager$startDragDrop(dragSource, dragVisual, context) {
        var ev = window._event;
                if (this._isDragging) {
            return;
        }
        
        this._underlyingTarget = null;
        this._activeDragSource = dragSource;
        this._activeDragVisual = dragVisual;
        this._activeContext = context;
        
        var mousePosition = { x: ev.clientX, y: ev.clientY };
                        dragVisual.originalPosition = dragVisual.style.position;
        dragVisual.style.position = "absolute";
        document._lastPosition = mousePosition;
        dragVisual.startingPoint = mousePosition;
        var scrollOffset = this.getScrollOffset(dragVisual,  true);
        dragVisual.startingPoint = this.addPoints(dragVisual.startingPoint, scrollOffset);
        if (dragVisual.style.position == "absolute") {
            dragVisual.startingPoint = this.subtractPoints(dragVisual.startingPoint, Sys.UI.DomElement.getLocation(dragVisual));
        }
        else {
            var left = parseInt(dragVisual.style.left);
            var top = parseInt(dragVisual.style.top);
            if (isNaN(left)) left = "0";
            if (isNaN(top)) top = "0";
            dragVisual.startingPoint = this.subtractPoints(dragVisual.startingPoint, { x: left, y: top });
        }
                this._prepareForDomChanges();
        dragSource.onDragStart();
        var eventArgs = new Sys.Preview.UI.DragDropEventArgs(
            dragSource.get_dragMode(),
            dragSource.get_dragDataType(),
            dragSource.getDragData(context));
        var handler = this.get_events().getHandler('dragStart');
        if(handler) handler(this,eventArgs);
        this._recoverFromDomChanges();
        this._wireEvents();
        this._drag( true);
    }
    function Sys$Preview$UI$IEDragDropManager$_stopDragDrop(cancelled) {
        var ev = window._event;
        if (this._activeDragSource) {
            this._unwireEvents();
            if (!cancelled) {
                                                cancelled = (this._underlyingTarget == null);
            }
            
            if (!cancelled && this._underlyingTarget) {
                this._underlyingTarget.drop(this._activeDragSource.get_dragMode(), this._activeDragSource.get_dragDataType(),
                    this._activeDragSource.getDragData(this._activeContext));
            }
            this._activeDragSource.onDragEnd(cancelled);
            var handler = this.get_events().getHandler('dragStop');
            if(handler) handler(this,Sys.EventArgs.Empty);
            
            this._activeDragVisual.style.position = this._activeDragVisual.originalPosition;
        
            this._activeDragSource = null;
            this._activeContext = null;
            this._activeDragVisual = null;
            this._isDragging = false;
            this._potentialTarget = null;
            ev.preventDefault();
        }
    }
    function Sys$Preview$UI$IEDragDropManager$_drag(isInitialDrag) {
        var ev = window._event;
        var mousePosition = { x: ev.clientX, y: ev.clientY };
                        document._lastPosition = mousePosition;
        
        var scrollOffset = this.getScrollOffset(this._activeDragVisual,  true);
        var position = this.addPoints(this.subtractPoints(mousePosition, this._activeDragVisual.startingPoint), scrollOffset);
                if (!isInitialDrag && parseInt(this._activeDragVisual.style.left) == position.x && parseInt(this._activeDragVisual.style.top) == position.y) {
            return;
        }
        
        Sys.UI.DomElement.setLocation(this._activeDragVisual, position.x, position.y);
        
                this._prepareForDomChanges();
        this._activeDragSource.onDrag();
        this._recoverFromDomChanges();
                this._potentialTarget = this._findPotentialTarget(this._activeDragSource, this._activeDragVisual);
        
        var movedToOtherTarget = (this._potentialTarget != this._underlyingTarget || this._potentialTarget == null);
                if (movedToOtherTarget && this._underlyingTarget != null) {
            this._leaveTarget(this._activeDragSource, this._underlyingTarget);
        }
        
        if (this._potentialTarget != null) {
                        if (movedToOtherTarget) {
                this._underlyingTarget = this._potentialTarget;
                
                                this._enterTarget(this._activeDragSource, this._underlyingTarget);
            }
            else {
                this._moveInTarget(this._activeDragSource, this._underlyingTarget);
            }
        }
        else {
            this._underlyingTarget = null;
        }
    }
    function Sys$Preview$UI$IEDragDropManager$_wireEvents() {
        Sys.UI.DomEvent.addHandler(document, "mouseup", this._mouseUpHandler);
        Sys.UI.DomEvent.addHandler(document, "mousemove", this._documentMouseMoveHandler);
        Sys.UI.DomEvent.addHandler(document.body, "dragover", this._documentDragOverHandler);
        
        Sys.UI.DomEvent.addHandler(this._activeDragVisual, "dragstart", this._dragStartHandler);
        Sys.UI.DomEvent.addHandler(this._activeDragVisual, "dragend", this._mouseUpHandler);
        Sys.UI.DomEvent.addHandler(this._activeDragVisual, "drag", this._mouseMoveHandler);
    }
    function Sys$Preview$UI$IEDragDropManager$_unwireEvents() {
        Sys.UI.DomEvent.removeHandler(this._activeDragVisual, "drag", this._mouseMoveHandler);
        Sys.UI.DomEvent.removeHandler(this._activeDragVisual, "dragend", this._mouseUpHandler);
        Sys.UI.DomEvent.removeHandler(this._activeDragVisual, "dragstart", this._dragStartHandler);
        Sys.UI.DomEvent.removeHandler(document.body, "dragover", this._documentDragOverHandler);
        Sys.UI.DomEvent.removeHandler(document, "mousemove", this._documentMouseMoveHandler);
        Sys.UI.DomEvent.removeHandler(document, "mouseup", this._mouseUpHandler);
    }
    function Sys$Preview$UI$IEDragDropManager$registerDropTarget(dropTarget) {
        if (!this._dropTargets) {
            this._dropTargets = [];
        }
        Array.add(this._dropTargets, dropTarget);
        
        this._wireDropTargetEvents(dropTarget);
    }
    function Sys$Preview$UI$IEDragDropManager$unregisterDropTarget(dropTarget) {
        this._unwireDropTargetEvents(dropTarget);
        if(this._dropTargets) {
            Array.remove(this._dropTargets, dropTarget);
        }
    }
    function Sys$Preview$UI$IEDragDropManager$_wireDropTargetEvents(dropTarget) {
        var associatedElement = dropTarget.get_dropTargetElement();
        associatedElement._dropTarget = dropTarget;
        Sys.UI.DomEvent.addHandler(associatedElement, "dragenter", this._dragEnterHandler);
        Sys.UI.DomEvent.addHandler(associatedElement, "dragleave", this._dragLeaveHandler);
        Sys.UI.DomEvent.addHandler(associatedElement, "dragover", this._dragOverHandler);
        Sys.UI.DomEvent.addHandler(associatedElement, "drop", this._dropHandler);
    }
    function Sys$Preview$UI$IEDragDropManager$_unwireDropTargetEvents(dropTarget) {
        var associatedElement = dropTarget.get_dropTargetElement();
        associatedElement._dropTarget = null;
        Sys.UI.DomEvent.removeHandler(associatedElement, "dragenter", this._dragEnterHandler);
        Sys.UI.DomEvent.removeHandler(associatedElement, "dragleave", this._dragLeaveHandler);
        Sys.UI.DomEvent.removeHandler(associatedElement, "dragover", this._dragOverHandler);
        Sys.UI.DomEvent.removeHandler(associatedElement, "drop", this._dropHandler);
    }
    function Sys$Preview$UI$IEDragDropManager$dragStartHandler(ev) {
        window._event = ev;
        document.selection.empty();
        var dt = ev.dataTransfer;
        if(!dt) dt = ev.rawEvent.dataTransfer;
        
        var dataType = this._activeDragSource.get_dragDataType().toLowerCase();
        var data = this._activeDragSource.getDragData(this._activeContext);
        
        if (data) {
                        if (dataType != "text" && dataType != "url") {
                dataType = "text";
                if (data.innerHTML != null) {
                    data = data.innerHTML;
                }
            }
            dt.effectAllowed = "move";
            dt.setData(dataType, data.toString());
        }
    }
    function Sys$Preview$UI$IEDragDropManager$mouseUpHandler(ev) {
        window._event = ev;
        this._stopDragDrop(false);
    }
    function Sys$Preview$UI$IEDragDropManager$documentMouseMoveHandler(ev) {
        window._event = ev;
        this._dragDrop();
    }
    function Sys$Preview$UI$IEDragDropManager$documentDragOverHandler(ev) {
        window._event = ev;
        if(this._potentialTarget) ev.preventDefault();
    }
    function Sys$Preview$UI$IEDragDropManager$mouseMoveHandler(ev) {
        window._event = ev;
        this._drag();
    }
    function Sys$Preview$UI$IEDragDropManager$dragEnterHandler(ev) {
        window._event = ev;
        if (this._isDragging) {
            ev.preventDefault();
        }
        else {
                        var dataObjects = Sys.Preview.UI.IEDragDropManager._getDataObjectsForDropTarget(this._getDropTarget(ev.target));
            for (var i = 0; i < dataObjects.length; i++) {
                this._dropTarget.onDragEnterTarget(Sys.Preview.UI.DragMode.Copy, dataObjects[i].type, dataObjects[i].value);
            }
        }
    }
    function Sys$Preview$UI$IEDragDropManager$dragLeaveHandler(ev) {
        window._event = ev;
        if (this._isDragging) {
            ev.preventDefault();
        }
        else {
                        var dataObjects = Sys.Preview.UI.IEDragDropManager._getDataObjectsForDropTarget(this._getDropTarget(ev.target));
            for (var i = 0; i < dataObjects.length; i++) {
                this._dropTarget.onDragLeaveTarget(Sys.Preview.UI.DragMode.Copy, dataObjects[i].type, dataObjects[i].value);
            }
        }
    }
    function Sys$Preview$UI$IEDragDropManager$dragOverHandler(ev) {
        window._event = ev;
        if (this._isDragging) {
            ev.preventDefault();
        }
        else {
                        var dataObjects = Sys.Preview.UI.IEDragDropManager._getDataObjectsForDropTarget(this._getDropTarget(ev.target));
            for (var i = 0; i < dataObjects.length; i++) {
                this._dropTarget.onDragInTarget(Sys.Preview.UI.DragMode.Copy, dataObjects[i].type, dataObjects[i].value);
            }
        }
    }
    function Sys$Preview$UI$IEDragDropManager$dropHandler(ev) {
        window._event = ev;
        if (!this._isDragging) {
                        var dataObjects = Sys.Preview.UI.IEDragDropManager._getDataObjectsForDropTarget(this._getDropTarget(ev.target));
            for (var i = 0; i < dataObjects.length; i++) {
                this._dropTarget.drop(Sys.Preview.UI.DragMode.Copy, dataObjects[i].type, dataObjects[i].value);
            }
        }
        ev.preventDefault();
    }
    function Sys$Preview$UI$IEDragDropManager$_getDropTarget(element) {
        while (element) {
            if (element._dropTarget != null) {
                return element._dropTarget;
            }
            element = element.parentNode;
        }
        return null;
    }
    function Sys$Preview$UI$IEDragDropManager$_dragDrop() {
        if (this._isDragging) {
            return;
        }
        
        this._isDragging = true;
        this._activeDragVisual.dragDrop();
        document.selection.empty();
    }
    function Sys$Preview$UI$IEDragDropManager$_moveInTarget(dragSource, dropTarget) {
                this._prepareForDomChanges();
        dropTarget.onDragInTarget(dragSource.get_dragMode(), dragSource.get_dragDataType(), dragSource.getDragData(this._activeContext));
        this._recoverFromDomChanges();
    }
    function Sys$Preview$UI$IEDragDropManager$_enterTarget(dragSource, dropTarget) {
                this._prepareForDomChanges();
        dropTarget.onDragEnterTarget(dragSource.get_dragMode(), dragSource.get_dragDataType(), dragSource.getDragData(this._activeContext));
        this._recoverFromDomChanges();
    }
    function Sys$Preview$UI$IEDragDropManager$_leaveTarget(dragSource, dropTarget) {
                this._prepareForDomChanges();
        dropTarget.onDragLeaveTarget(dragSource.get_dragMode(), dragSource.get_dragDataType(), dragSource.getDragData(this._activeContext));
        this._recoverFromDomChanges();
    }
    function Sys$Preview$UI$IEDragDropManager$_findPotentialTarget(dragSource, dragVisual) {
        var ev = window._event;
        if (!this._dropTargets) {
            return null;
        }
        var type = dragSource.get_dragDataType();
        var mode = dragSource.get_dragMode();
        var data = dragSource.getDragData(this._activeContext);
                var scrollOffset = this.getScrollOffset(document.body,  true);
        var x = ev.clientX + scrollOffset.x;
        var y = ev.clientY + scrollOffset.y;
        var cursorRect = { x: x - this._radius, y: y - this._radius, width: this._radius * 2, height: this._radius * 2 };
        
                for (var i = 0; i < this._dropTargets.length; i++) {
            var dt = this._dropTargets[i];
            var canDrop = dt.canDrop(mode, type, data);
            if(!canDrop) continue;
            var el = dt.get_dropTargetElement();
            var targetRect = Sys.UI.DomElement.getBounds(el);
            var overlaps = Sys.UI.Control.overlaps(cursorRect, targetRect);
                        if (overlaps || el === document.body) {
                return dt;
            }
        }        
        return null;
    }
    function Sys$Preview$UI$IEDragDropManager$_prepareForDomChanges() {
        this._oldOffset = Sys.UI.DomElement.getLocation(this._activeDragVisual);
    }
    function Sys$Preview$UI$IEDragDropManager$_recoverFromDomChanges() {
        var newOffset = Sys.UI.DomElement.getLocation(this._activeDragVisual);
        if (this._oldOffset.x != newOffset.x || this._oldOffset.y != newOffset.y) {
            this._activeDragVisual.startingPoint = this.subtractPoints(this._activeDragVisual.startingPoint, this.subtractPoints(this._oldOffset, newOffset));
            scrollOffset = this.getScrollOffset(this._activeDragVisual,  true);
            var position = this.addPoints(this.subtractPoints(document._lastPosition, this._activeDragVisual.startingPoint), scrollOffset);
            Sys.UI.DomElement.setLocation(this._activeDragVisual, position.x, position.y);
        }
    }
    function Sys$Preview$UI$IEDragDropManager$addPoints(p1, p2) {
        return { x: p1.x + p2.x, y: p1.y + p2.y };
    }
    function Sys$Preview$UI$IEDragDropManager$subtractPoints(p1, p2) {
        return { x: p1.x - p2.x, y: p1.y - p2.y };
    }
    function Sys$Preview$UI$IEDragDropManager$getScrollOffset(element, recursive) {
        var left = element.scrollLeft;
        var top = element.scrollTop;
        if (recursive) {
            var parent = element.parentNode;
            while (parent != null && parent.scrollLeft != null) {
                left += parent.scrollLeft;
                top += parent.scrollTop;
                                if (parent == document.body && (left != 0 && top != 0))
                    break;
                parent = parent.parentNode;
            }
        }
        return { x: left, y: top };
    }
    function Sys$Preview$UI$IEDragDropManager$getBrowserRectangle() {
        var width = window.innerWidth;
        var height = window.innerHeight;
        if (width == null) {
            width = document.body.clientWidth;
        }
        if (height == null) {
            height = document.body.clientHeight;
        }
        return { x: 0, y: 0, width: width, height: height };
    }
    function Sys$Preview$UI$IEDragDropManager$getNextSibling(item) {
        for (item = item.nextSibling; item != null; item = item.nextSibling) {
            if (item.innerHTML != null) {
                return item;
            }
        }
        return null;
    }
    function Sys$Preview$UI$IEDragDropManager$hasParent(element) {
        return (element.parentNode != null && element.parentNode.tagName != null);
    }
Sys.Preview.UI.IEDragDropManager.prototype = {
    _dropTargets: null,
                _radius: 10,
    _activeDragVisual: null,
    _activeContext: null,
    _activeDragSource: null,
    _underlyingTarget: null,
    _oldOffset: null,
    _potentialTarget: null,
    _isDragging: false,
    _mouseUpHandler: null,
    _documentMouseMoveHandler: null,
    _documentDragOverHandler: null,
    _dragStartHandler: null,
    _mouseMoveHandler: null,
    _dragEnterHandler: null,
    _dragLeaveHandler: null,
    _dragOverHandler: null,
    _dropHandler: null,
    
    add_dragStart: Sys$Preview$UI$IEDragDropManager$add_dragStart,
    remove_dragStart: Sys$Preview$UI$IEDragDropManager$remove_dragStart,
    add_dragStop: Sys$Preview$UI$IEDragDropManager$add_dragStop,
    remove_dragStop: Sys$Preview$UI$IEDragDropManager$remove_dragStop,
    
    initialize: Sys$Preview$UI$IEDragDropManager$initialize,
    
    dispose: Sys$Preview$UI$IEDragDropManager$dispose,
    startDragDrop: Sys$Preview$UI$IEDragDropManager$startDragDrop,
    
    _stopDragDrop: Sys$Preview$UI$IEDragDropManager$_stopDragDrop,
    
    _drag: Sys$Preview$UI$IEDragDropManager$_drag,
    
    _wireEvents: Sys$Preview$UI$IEDragDropManager$_wireEvents,
    
    _unwireEvents: Sys$Preview$UI$IEDragDropManager$_unwireEvents,
    
    registerDropTarget: Sys$Preview$UI$IEDragDropManager$registerDropTarget,
    
    unregisterDropTarget: Sys$Preview$UI$IEDragDropManager$unregisterDropTarget,
    
    _wireDropTargetEvents: Sys$Preview$UI$IEDragDropManager$_wireDropTargetEvents,
        
    _unwireDropTargetEvents: Sys$Preview$UI$IEDragDropManager$_unwireDropTargetEvents,
    
    dragStartHandler: Sys$Preview$UI$IEDragDropManager$dragStartHandler,
    
    mouseUpHandler: Sys$Preview$UI$IEDragDropManager$mouseUpHandler,
    
    documentMouseMoveHandler: Sys$Preview$UI$IEDragDropManager$documentMouseMoveHandler,
    documentDragOverHandler: Sys$Preview$UI$IEDragDropManager$documentDragOverHandler,
    
    mouseMoveHandler: Sys$Preview$UI$IEDragDropManager$mouseMoveHandler,
    
    dragEnterHandler: Sys$Preview$UI$IEDragDropManager$dragEnterHandler,
    
    dragLeaveHandler: Sys$Preview$UI$IEDragDropManager$dragLeaveHandler,
    
    dragOverHandler: Sys$Preview$UI$IEDragDropManager$dragOverHandler,
    
    dropHandler: Sys$Preview$UI$IEDragDropManager$dropHandler,
    
    _getDropTarget: Sys$Preview$UI$IEDragDropManager$_getDropTarget,
    
    _dragDrop: Sys$Preview$UI$IEDragDropManager$_dragDrop,
    
    _moveInTarget: Sys$Preview$UI$IEDragDropManager$_moveInTarget,
    
    _enterTarget: Sys$Preview$UI$IEDragDropManager$_enterTarget,
    
    _leaveTarget: Sys$Preview$UI$IEDragDropManager$_leaveTarget,
    
    _findPotentialTarget: Sys$Preview$UI$IEDragDropManager$_findPotentialTarget,
    
    _prepareForDomChanges: Sys$Preview$UI$IEDragDropManager$_prepareForDomChanges,
    
    _recoverFromDomChanges: Sys$Preview$UI$IEDragDropManager$_recoverFromDomChanges,
    
    addPoints: Sys$Preview$UI$IEDragDropManager$addPoints,
    
    subtractPoints: Sys$Preview$UI$IEDragDropManager$subtractPoints,
    
        getScrollOffset: Sys$Preview$UI$IEDragDropManager$getScrollOffset,
    
    getBrowserRectangle: Sys$Preview$UI$IEDragDropManager$getBrowserRectangle,
    
    getNextSibling: Sys$Preview$UI$IEDragDropManager$getNextSibling,
    
    hasParent: Sys$Preview$UI$IEDragDropManager$hasParent
}
Sys.Preview.UI.IEDragDropManager.registerClass('Sys.Preview.UI.IEDragDropManager', Sys.Component);
Sys.Preview.UI.IEDragDropManager._getDataObjectsForDropTarget = function Sys$Preview$UI$IEDragDropManager$_getDataObjectsForDropTarget(dropTarget) {
    if (dropTarget == null) {
        return [];
    }
    var ev = window._event;
    var dataObjects = [];
    var dataTypes = [ "URL", "Text" ];
    var data;
    for (var i = 0; i < dataTypes.length; i++) {
        var dt = ev.dataTransfer;
        if(!dt) dt = ev.rawEvent.dataTransfer;
        data = dt.getData(dataTypes[i]);
        if (dropTarget.canDrop(Sys.Preview.UI.DragMode.Copy, dataTypes[i], data)) {
            if (data) {
                Array.add(dataObjects, { type : dataTypes[i], value : data });
            }
        }
    }
    return dataObjects;
}
Sys.Preview.UI.GenericDragDropManager = function Sys$Preview$UI$GenericDragDropManager() {
    Sys.Preview.UI.GenericDragDropManager.initializeBase(this);
}
    function Sys$Preview$UI$GenericDragDropManager$initialize() {
        Sys.Preview.UI.GenericDragDropManager.callBaseMethod(this, "initialize");
        
        this._mouseUpHandler = Function.createDelegate(this, this.mouseUpHandler);
        this._mouseMoveHandler = Function.createDelegate(this, this.mouseMoveHandler);
        this._keyPressHandler = Function.createDelegate(this, this.keyPressHandler);
        if (Sys.Browser.agent === Sys.Browser.Safari) {
            Sys.Preview.UI.GenericDragDropManager.__loadSafariCompatLayer(this);
        }
        this._scroller = new Sys.Preview.Timer();
        this._scroller.set_interval(10);
        this._scroller.add_tick(Function.createDelegate(this,this.scrollerTickHandler));
    }
    function Sys$Preview$UI$GenericDragDropManager$startDragDrop(dragSource, dragVisual, context) {
        this._activeDragSource = dragSource;
        this._activeDragVisual = dragVisual;
        this._activeContext = context;
        
        Sys.Preview.UI.GenericDragDropManager.callBaseMethod(this, "startDragDrop", [dragSource, dragVisual, context]);
    }
    function Sys$Preview$UI$GenericDragDropManager$_stopDragDrop(cancelled) {
        this._scroller.set_enabled(false);
        
        Sys.Preview.UI.GenericDragDropManager.callBaseMethod(this, "_stopDragDrop", [cancelled]);
    }
    function Sys$Preview$UI$GenericDragDropManager$_drag(isInitialDrag) {
        Sys.Preview.UI.GenericDragDropManager.callBaseMethod(this, "_drag", [isInitialDrag]);
        
        this._autoScroll();
    }
    function Sys$Preview$UI$GenericDragDropManager$_wireEvents() {
        Sys.UI.DomEvent.addHandler(document, "mouseup", this._mouseUpHandler);
        Sys.UI.DomEvent.addHandler(document, "mousemove", this._mouseMoveHandler);
        Sys.UI.DomEvent.addHandler(document, "keypress", this._keyPressHandler);
    }
    function Sys$Preview$UI$GenericDragDropManager$_unwireEvents() {
        Sys.UI.DomEvent.removeHandler(document, "keypress", this._keyPressHandler);
        Sys.UI.DomEvent.removeHandler(document, "mousemove", this._mouseMoveHandler);
        Sys.UI.DomEvent.removeHandler(document, "mouseup", this._mouseUpHandler);
    }
    function Sys$Preview$UI$GenericDragDropManager$_wireDropTargetEvents(dropTarget) {
            }
    function Sys$Preview$UI$GenericDragDropManager$_unwireDropTargetEvents(dropTarget) {
            }
    function Sys$Preview$UI$GenericDragDropManager$mouseUpHandler(e) {
        window._event = e;
        this._stopDragDrop(false);
    }
    function Sys$Preview$UI$GenericDragDropManager$mouseMoveHandler(e) {
        window._event = e;
        this._drag();
    }
    function Sys$Preview$UI$GenericDragDropManager$keyPressHandler(e) {
        window._event = e;
                var k = e.keyCode ? e.keyCode : e.rawEvent.keyCode;
        if (k == 27) {
            this._stopDragDrop( true);
        }
    }
    function Sys$Preview$UI$GenericDragDropManager$_autoScroll() {
        var ev = window._event;
        var browserRect = this.getBrowserRectangle();
        if (browserRect.width > 0) {
            this._scrollDeltaX = this._scrollDeltaY = 0;
            if (ev.clientX < browserRect.x + this._scrollEdgeConst) this._scrollDeltaX = -this._scrollByConst;
            else if (ev.clientX > browserRect.width - this._scrollEdgeConst) this._scrollDeltaX = this._scrollByConst;
            if (ev.clientY < browserRect.y + this._scrollEdgeConst) this._scrollDeltaY = -this._scrollByConst;
            else if (ev.clientY > browserRect.height - this._scrollEdgeConst) this._scrollDeltaY = this._scrollByConst;
            if (this._scrollDeltaX != 0 || this._scrollDeltaY != 0) {
                this._scroller.set_enabled(true);
            }
            else {
                this._scroller.set_enabled(false);
            }
        }
    }
    function Sys$Preview$UI$GenericDragDropManager$scrollerTickHandler() {
        var oldLeft = document.body.scrollLeft;
        var oldTop = document.body.scrollTop;
        window.scrollBy(this._scrollDeltaX, this._scrollDeltaY);
        var newLeft = document.body.scrollLeft;
        var newTop = document.body.scrollTop;
        
        var dragVisual = this._activeDragVisual;
        var position = { x: parseInt(dragVisual.style.left) + (newLeft - oldLeft), y: parseInt(dragVisual.style.top) + (newTop - oldTop) };
        Sys.UI.DomElement.setLocation(dragVisual, position.x, position.y);
    }
Sys.Preview.UI.GenericDragDropManager.prototype = {
                    _scrollEdgeConst: 40,
    _scrollByConst: 10,
    _scroller: null,
    _scrollDeltaX: null,
    _scrollDeltaY: null,
    _activeDragVisual: null,
    _activeContext: null,
    _activeDragSource: null,
            _mouseUpHandler: null,
    _mouseMoveHandler: null,
    _keyPressHandler: null,
    
    initialize: Sys$Preview$UI$GenericDragDropManager$initialize,
    startDragDrop: Sys$Preview$UI$GenericDragDropManager$startDragDrop,
    
    _stopDragDrop: Sys$Preview$UI$GenericDragDropManager$_stopDragDrop,
    
    _drag: Sys$Preview$UI$GenericDragDropManager$_drag,
    
    _wireEvents: Sys$Preview$UI$GenericDragDropManager$_wireEvents,
    
    _unwireEvents: Sys$Preview$UI$GenericDragDropManager$_unwireEvents,
    
    _wireDropTargetEvents: Sys$Preview$UI$GenericDragDropManager$_wireDropTargetEvents,
    
    _unwireDropTargetEvents: Sys$Preview$UI$GenericDragDropManager$_unwireDropTargetEvents,
    
    mouseUpHandler: Sys$Preview$UI$GenericDragDropManager$mouseUpHandler,
    
    mouseMoveHandler: Sys$Preview$UI$GenericDragDropManager$mouseMoveHandler,
    
    keyPressHandler: Sys$Preview$UI$GenericDragDropManager$keyPressHandler,
    
    _autoScroll: Sys$Preview$UI$GenericDragDropManager$_autoScroll,
    
    scrollerTickHandler: Sys$Preview$UI$GenericDragDropManager$scrollerTickHandler
}
Sys.Preview.UI.GenericDragDropManager.registerClass('Sys.Preview.UI.GenericDragDropManager', Sys.Preview.UI.IEDragDropManager);
if (Sys.Browser.agent === Sys.Browser.Safari) {
    Sys.Preview.UI.GenericDragDropManager.__loadSafariCompatLayer = function Sys$Preview$UI$GenericDragDropManager$__loadSafariCompatLayer(ddm) {
        ddm._getScrollOffset = ddm.getScrollOffset;
        ddm.getScrollOffset = function ddm$getScrollOffset(element, recursive) {
            return { x: 0, y: 0 };
        }
        ddm._getBrowserRectangle = ddm.getBrowserRectangle;
        ddm.getBrowserRectangle = function ddm$getBrowserRectangle() {
            var browserRect = ddm._getBrowserRectangle();
            
            var offset = ddm._getScrollOffset(document.body, true);
            return { x: browserRect.x + offset.x, y: browserRect.y + offset.y,
                width: browserRect.width + offset.x, height: browserRect.height + offset.y };
        }
    }
}
Sys.Preview.UI.RepeatDirection = function Sys$Preview$UI$RepeatDirection() {
    throw Error.invalidOperation();
}
Sys.Preview.UI.RepeatDirection.prototype = {
    Horizontal: 0,
    Vertical: 1
}
Sys.Preview.UI.RepeatDirection.registerEnum('Sys.Preview.UI.RepeatDirection');
Sys.Preview.UI.DragDropList = function Sys$Preview$UI$DragDropList(associatedElement) {
    Sys.Preview.UI.DragDropList.initializeBase(this, [associatedElement]);
    this._acceptedDataTypes = [];
}
    function Sys$Preview$UI$DragDropList$get_data() {
if (arguments.length !== 0) throw Error.parameterCount();
        return this._data;
    }
    function Sys$Preview$UI$DragDropList$set_data(value) {
        this._data = value;
    }
    function Sys$Preview$UI$DragDropList$initialize() {
        Sys.Preview.UI.DragDropList.callBaseMethod(this, 'initialize');
        this.get_element().__dragDropList = this;
        Sys.Preview.UI.DragDropManager.registerDropTarget(this);
    }
    function Sys$Preview$UI$DragDropList$startDragDrop(dragObject, context, dragVisual) {
        if (!this._isDragging) {
            this._isDragging = true;
            this._currentContext = context;
            if (!dragVisual) {
                dragVisual = this.createDragVisual(dragObject);
                            }
            else {
                this._dragVisual = dragVisual;
                            }
            Sys.Preview.UI.DragDropManager.startDragDrop(this, dragVisual, context);
        }
    }
    function Sys$Preview$UI$DragDropList$createDragVisual(dragObject) {
        if (this._dragMode === Sys.Preview.UI.DragMode.Copy) {
            this._dragVisual = dragObject.cloneNode(true);
        }
        else {
            this._dragVisual = dragObject;
        }
        var oldOffset = Sys.Preview.UI.DragDropManager._getInstance().getScrollOffset(dragObject, true);
        this._dragVisual.style.width = dragObject.offsetWidth + "px";
        this._dragVisual.style.height = dragObject.offsetHeight + "px";
        this._dragVisual.style.opacity = "0.4";
        this._dragVisual.style.filter = "progid:DXImageTransform.Microsoft.BasicImage(opacity=0.4);";
        this._originalZIndex = this._dragVisual.style.zIndex;
        this._dragVisual.style.zIndex = 99999;
        this._originalParent = this._dragVisual.parentNode;
        this._originalNextSibling = Sys.Preview.UI.DragDropManager._getInstance().getNextSibling(this._dragVisual);
        var ddm = Sys.Preview.UI.DragDropManager._getInstance();
        var currentLocation = Sys.UI.DomElement.getLocation(dragObject);
                var dragVisualContainer = this._getFloatContainer();
        Sys.UI.DomElement.setLocation(dragVisualContainer, currentLocation.x, currentLocation.y);
        
        if (Sys.Preview.UI.DragDropManager._getInstance().hasParent(this._dragVisual)) {
            this._dragVisual.parentNode.removeChild(this._dragVisual);
        }
        dragVisualContainer.appendChild(this._dragVisual);
        var newOffset = ddm.getScrollOffset(dragObject, true);
        if (oldOffset.x !== newOffset.x || oldOffset.y !== newOffset.y) {
            var diff = ddm.subtractPoints(oldOffset, newOffset);
            var location = ddm.subtractPoints(currentLocation, diff);
            Sys.UI.DomElement.setLocation(dragVisualContainer, location.x, location.y);
        }
        return dragVisualContainer;
    }
    function Sys$Preview$UI$DragDropList$get_emptyTemplate() {
if (arguments.length !== 0) throw Error.parameterCount();
        return this._emptyTemplate;
    }
    function Sys$Preview$UI$DragDropList$set_emptyTemplate(value) {
        this._emptyTemplate = value;
    }
    function Sys$Preview$UI$DragDropList$get_dragDataType() {
if (arguments.length !== 0) throw Error.parameterCount();
        return this._dataType;
    }
    function Sys$Preview$UI$DragDropList$set_dragDataType(value) {
        this._dataType = value;
    }
    function Sys$Preview$UI$DragDropList$getDragData(context) {
        return context;
    }
    function Sys$Preview$UI$DragDropList$get_dragMode() {
if (arguments.length !== 0) throw Error.parameterCount();
        return this._dragMode;
    }
    function Sys$Preview$UI$DragDropList$set_dragMode(value) {
        this._dragMode = value;
    }
    function Sys$Preview$UI$DragDropList$dispose() {
        this.get_element().__dragDropList = null;
        Sys.Preview.UI.DragDropList.callBaseMethod(this, 'dispose');
    }
    function Sys$Preview$UI$DragDropList$onDragStart() {
        this._validate();
    }
    function Sys$Preview$UI$DragDropList$onDrag() {
            }
    function Sys$Preview$UI$DragDropList$onDragEnd(cancelled) {
        if (this._floatContainerInstance) {
            if (this._dragMode === Sys.Preview.UI.DragMode.Copy) {
                this._floatContainerInstance.removeChild(this._dragVisual);
            }
            else {
                                                this._dragVisual.style.opacity = "0.999";
                                this._dragVisual.style.filter = "";
                this._dragVisual.style.zIndex = this._originalZIndex ? this._originalZIndex : 0;
                if (cancelled) {
                                        this._dragVisual.parentNode.removeChild(this._dragVisual);
                    if (this._originalNextSibling != null) {
                        this._originalParent.insertBefore(this._dragVisual, this._originalNextSibling);
                    }
                    else {
                        this._originalParent.appendChild(this._dragVisual);
                    }
                }
                else {
                    if (this._dragVisual.parentNode === this._floatContainerInstance) {
                        this._dragVisual.parentNode.removeChild(this._dragVisual);
                    }
                }
            }
                        document.body.removeChild(this._floatContainerInstance);
        }
        else {
            this._dragVisual.parentNode.removeChild(this._dragVisual);
        }
        if (!cancelled && this._data && this._dragMode === Sys.Preview.UI.DragMode.Move) {
            var data = this.getDragData(this._currentContext);
            if (this._data && data) {
                if (Sys.Preview.Data.IData.isImplementedBy(this._data)) {
                    this._data.remove(data);
                }
                else if (this._data instanceof Array) {
                    if(typeof(this._data.remove) === "function") {
                        this._data.remove(data);
                    }
                    else {
                        Array.remove(this._data, data);
                    }
                }
            }
        }
        this._isDragging = false;
        this._validate();
    }
    function Sys$Preview$UI$DragDropList$get_direction() {
if (arguments.length !== 0) throw Error.parameterCount();
        return this._direction;
    }
    function Sys$Preview$UI$DragDropList$set_direction(value) {
        this._direction = value;
    }
    function Sys$Preview$UI$DragDropList$get_acceptedDataTypes() {
if (arguments.length !== 0) throw Error.parameterCount();
        return this._acceptedDataTypes;
    }
    function Sys$Preview$UI$DragDropList$set_acceptedDataTypes(value) {
        this._acceptedDataTypes = value;
    }
    function Sys$Preview$UI$DragDropList$get_dropCueTemplate() {
if (arguments.length !== 0) throw Error.parameterCount();
        return this._dropCueTemplate;
    }
    function Sys$Preview$UI$DragDropList$set_dropCueTemplate(value) {
        this._dropCueTemplate = value;
    }
    function Sys$Preview$UI$DragDropList$get_dropTargetElement() {
if (arguments.length !== 0) throw Error.parameterCount();
        return this.get_element();
    }
    function Sys$Preview$UI$DragDropList$canDrop(dragMode, dataType, data) {
        for (var i = 0; i < this._acceptedDataTypes.length; i++) {
            if (this._acceptedDataTypes[i] === dataType) {
                return true;
            }
        }
        return false;
    }
    function Sys$Preview$UI$DragDropList$drop(dragMode, dataType, data) {
        if (dataType === "HTML" && dragMode === Sys.Preview.UI.DragMode.Move) {
                        dragVisual = data;
            var potentialNextSibling = this._findPotentialNextSibling(dragVisual);
            this._setDropCueVisible(false, dragVisual);
            dragVisual.parentNode.removeChild(dragVisual);
            if (potentialNextSibling) {
                this.get_element().insertBefore(dragVisual, potentialNextSibling);
            }
            else {
                this.get_element().appendChild(dragVisual);
            }
        }
        else {
            this._setDropCueVisible(false);
        }
        if (this._data && data) {
            var newRow = data;
            if (Sys.Preview.Data.DataRow.isInstanceOfType(data) && Sys.Preview.Data.DataTable.isInstanceOfType(this._data)) {
                var src = data.get_table();
                if (src) {
                    newRow = this._data.createRow(data);
                }
            }
            if (Sys.Preview.Data.IData.isImplementedBy(this._data)) {
                this._data.add(newRow);
            }
            else if (this._data instanceof Array) {
                if(typeof(this._data.add) === "function") {
                    this._data.add(newRow);
                }
                else {
                    Array.add(this._data, newRow);
                }
            }
        }
    }
    function Sys$Preview$UI$DragDropList$onDragEnterTarget(dragMode, dataType, data) {
        if (dataType === "HTML") {
            this._setDropCueVisible(true, data);
            this._validate();
        }
    }
    function Sys$Preview$UI$DragDropList$onDragLeaveTarget(dragMode, dataType, data) {
        if (dataType === "HTML") {
            this._setDropCueVisible(false);
            this._validate();
        }
    }
    function Sys$Preview$UI$DragDropList$onDragInTarget(dragMode, dataType, data) {
        if (dataType === "HTML") {
            this._setDropCueVisible(true, data);
        }
    }
    function Sys$Preview$UI$DragDropList$_setDropCueVisible(visible, dragVisual) {
        if (this._dropCueTemplate) {
            if (visible) {
                if (!this._dropCueTemplateInstance) {
                    var documentContext = document.createDocumentFragment();
                    this._dropCueTemplateInstance = this._dropCueTemplate.createInstance(documentContext).instanceElement;
                }
                var potentialNextSibling = this._findPotentialNextSibling(dragVisual);
                if (!Sys.Preview.UI.DragDropManager._getInstance().hasParent(this._dropCueTemplateInstance)) {
                                        if (potentialNextSibling) {
                        this.get_element().insertBefore(this._dropCueTemplateInstance, potentialNextSibling);
                    }
                    else {
                        this.get_element().appendChild(this._dropCueTemplateInstance);
                    }
                    this._dropCueTemplateInstance.style.width = dragVisual.offsetWidth + "px";
                    this._dropCueTemplateInstance.style.height = dragVisual.offsetHeight + "px";
                }
                else {
                                        if (Sys.Preview.UI.DragDropManager._getInstance().getNextSibling(this._dropCueTemplateInstance) !== potentialNextSibling) {
                        this.get_element().removeChild(this._dropCueTemplateInstance);
                        if (potentialNextSibling) {
                            this.get_element().insertBefore(this._dropCueTemplateInstance, potentialNextSibling);
                        }
                        else {
                            this.get_element().appendChild(this._dropCueTemplateInstance);
                        }
                    }
                }
            }
            else {
                if (this._dropCueTemplateInstance && Sys.Preview.UI.DragDropManager._getInstance().hasParent(this._dropCueTemplateInstance)) {
                    this.get_element().removeChild(this._dropCueTemplateInstance);
                }
            }
        }
    }
    function Sys$Preview$UI$DragDropList$_findPotentialNextSibling(dragVisual) {
        var dragVisualRect = Sys.UI.DomElement.getBounds(dragVisual);
        var isVertical = (this._direction === Sys.Preview.UI.RepeatDirection.Vertical);
        var nodeRect;
        for (var node = this.get_element().firstChild; node !== null; node = node.nextSibling) {
            if (node.innerHTML && node !== this._dropCueTemplateInstance && node !== this._emptyTemplateInstance) {
                nodeRect = Sys.UI.DomElement.getBounds(node);
                if ((!isVertical && dragVisualRect.x <= nodeRect.x) || (isVertical && dragVisualRect.y <= nodeRect.y)) {
                    return node;
                }
            }
        }
        return null;
    }
    function Sys$Preview$UI$DragDropList$_validate() {
        var visible = (this._dropCueTemplateInstance == null || !Sys.Preview.UI.DragDropManager._getInstance().hasParent(this._dropCueTemplateInstance));
                var count = 0;
        for (var node = this.get_element().firstChild; node !== null; node = node.nextSibling) {
            if (node.innerHTML && node !== this._emptyTemplateInstance && node !== this._dropCueTemplateInstance) {
                count++;
            }
        }
        if (count > 0) {
            visible = false;
        }
        this._setEmptyTemplateVisible(visible);
    }
    function Sys$Preview$UI$DragDropList$_setEmptyTemplateVisible(visible) {
        if (this._emptyTemplate) {
            if (visible) {
                if (!this._emptyTemplateInstance) {
                    this._emptyTemplateInstance = this._emptyTemplate.createInstance(this.get_element()).instanceElement;
                }
                else if (!Sys.Preview.UI.DragDropManager._getInstance().hasParent(this._emptyTemplateInstance)) {
                    this.get_element().appendChild(this._emptyTemplateInstance);
                }
            }
            else {
                if (this._emptyTemplateInstance && Sys.Preview.UI.DragDropManager._getInstance().hasParent(this._emptyTemplateInstance)) {
                    this.get_element().removeChild(this._emptyTemplateInstance);
                }
            }
        }
    }
    function Sys$Preview$UI$DragDropList$_getFloatContainer() {
        if (!this._floatContainerInstance) {
            this._floatContainerInstance = document.createElement(this.get_element().tagName);
            var none = "0px 0px 0px 0px";
            this._floatContainerInstance.style.position = "absolute";
            this._floatContainerInstance.style.padding = none;
            this._floatContainerInstance.style.margin = none;
            document.body.appendChild(this._floatContainerInstance);
        }
        else if (!Sys.Preview.UI.DragDropManager._getInstance().hasParent(this._floatContainerInstance)) {
            document.body.appendChild(this._floatContainerInstance);
        }
        return this._floatContainerInstance;
    }
Sys.Preview.UI.DragDropList.prototype = {
    _isDragging: null,
    _dataType: null,
    _dragMode: null,
    _dragVisual: null,
    _direction: Sys.Preview.UI.RepeatDirection.Vertical,
    _emptyTemplate: null,
    _emptyTemplateInstance: null,
    _dropCueTemplate: null,
    _dropCueTemplateInstance: null,
    _floatContainerInstance: null,
    _originalParent: null,
    _originalNextSibling: null,
    _originalZIndex: null,
    _currentContext: null,
    _data: null,
    get_data: Sys$Preview$UI$DragDropList$get_data,
    set_data: Sys$Preview$UI$DragDropList$set_data,
    initialize: Sys$Preview$UI$DragDropList$initialize,
    
    startDragDrop: Sys$Preview$UI$DragDropList$startDragDrop,
    createDragVisual: Sys$Preview$UI$DragDropList$createDragVisual,
    get_emptyTemplate: Sys$Preview$UI$DragDropList$get_emptyTemplate,
    set_emptyTemplate: Sys$Preview$UI$DragDropList$set_emptyTemplate,
        get_dragDataType: Sys$Preview$UI$DragDropList$get_dragDataType,
    set_dragDataType: Sys$Preview$UI$DragDropList$set_dragDataType,
        getDragData: Sys$Preview$UI$DragDropList$getDragData,
        get_dragMode: Sys$Preview$UI$DragDropList$get_dragMode,
    set_dragMode: Sys$Preview$UI$DragDropList$set_dragMode,
    dispose: Sys$Preview$UI$DragDropList$dispose,
        onDragStart: Sys$Preview$UI$DragDropList$onDragStart,
        onDrag: Sys$Preview$UI$DragDropList$onDrag,
        onDragEnd: Sys$Preview$UI$DragDropList$onDragEnd,
    
    get_direction: Sys$Preview$UI$DragDropList$get_direction,
    set_direction: Sys$Preview$UI$DragDropList$set_direction,
    get_acceptedDataTypes: Sys$Preview$UI$DragDropList$get_acceptedDataTypes,
    set_acceptedDataTypes: Sys$Preview$UI$DragDropList$set_acceptedDataTypes,
    get_dropCueTemplate: Sys$Preview$UI$DragDropList$get_dropCueTemplate,
    set_dropCueTemplate: Sys$Preview$UI$DragDropList$set_dropCueTemplate,
    get_dropTargetElement: Sys$Preview$UI$DragDropList$get_dropTargetElement,
        canDrop: Sys$Preview$UI$DragDropList$canDrop,
        drop: Sys$Preview$UI$DragDropList$drop,
        onDragEnterTarget: Sys$Preview$UI$DragDropList$onDragEnterTarget,
        onDragLeaveTarget: Sys$Preview$UI$DragDropList$onDragLeaveTarget,
        onDragInTarget: Sys$Preview$UI$DragDropList$onDragInTarget,
    _setDropCueVisible: Sys$Preview$UI$DragDropList$_setDropCueVisible,
    _findPotentialNextSibling: Sys$Preview$UI$DragDropList$_findPotentialNextSibling,
    _validate: Sys$Preview$UI$DragDropList$_validate,
    _setEmptyTemplateVisible: Sys$Preview$UI$DragDropList$_setEmptyTemplateVisible,
    _getFloatContainer: Sys$Preview$UI$DragDropList$_getFloatContainer
}
Sys.Preview.UI.DragDropList.registerClass('Sys.Preview.UI.DragDropList', Sys.UI.Behavior, Sys.Preview.UI.IDragSource, Sys.Preview.UI.IDropTarget, Sys.IDisposable);
Sys.Preview.UI.DragDropList.descriptor = {
    properties: [   {name: 'acceptedDataTypes', type: Array},
                    {name: 'data', type: Object},
                    {name: 'dragDataType', type: String},
                    {name: 'emptyTemplate', type: Sys.Preview.UI.ITemplate},
                    {name: 'dropCueTemplate', type: Sys.Preview.UI.ITemplate},
                    {name: 'dropTargetElement', type: Object, readOnly: true},
                    {name: 'direction', type: Sys.Preview.UI.RepeatDirection},
                    {name: 'dragMode', type: Sys.Preview.UI.DragMode} ]
}
Sys.Preview.UI.DataSourceDropTarget = function Sys$Preview$UI$DataSourceDropTarget(associatedElement) {
    Sys.Preview.UI.DataSourceDropTarget.initializeBase(this, [associatedElement]);
}
    function Sys$Preview$UI$DataSourceDropTarget$get_append() {
if (arguments.length !== 0) throw Error.parameterCount();
        return this._append;
    }
    function Sys$Preview$UI$DataSourceDropTarget$set_append(value) {
        this._append = value;
    }
    function Sys$Preview$UI$DataSourceDropTarget$get_target() {
if (arguments.length !== 0) throw Error.parameterCount();
        return this._target;
    }
    function Sys$Preview$UI$DataSourceDropTarget$set_target(value) {
        this._target = value;
    }
    function Sys$Preview$UI$DataSourceDropTarget$get_property() {
if (arguments.length !== 0) throw Error.parameterCount();
        return this._property;
    }
    function Sys$Preview$UI$DataSourceDropTarget$set_property(value) {
        this._property = value;
    }
    function Sys$Preview$UI$DataSourceDropTarget$get_acceptedDataTypes() {
if (arguments.length !== 0) throw Error.parameterCount();
        return this._acceptedDataTypes;
    }
    function Sys$Preview$UI$DataSourceDropTarget$set_acceptedDataTypes(value) {
        this._acceptedDataTypes = value;
    }
    function Sys$Preview$UI$DataSourceDropTarget$initialize() {
        Sys.Preview.UI.DataSourceDropTarget.callBaseMethod(this, 'initialize');
        this._control = Sys.Application.findComponent(this.get_element().id);         Sys.Preview.UI.DragDropManager.registerDropTarget(this);
    }
    function Sys$Preview$UI$DataSourceDropTarget$get_dropTargetElement() {
if (arguments.length !== 0) throw Error.parameterCount();
        return this.get_element();
    }
    function Sys$Preview$UI$DataSourceDropTarget$canDrop(dragMode, dataType, data) {
        for (var i = 0; i < this._acceptedDataTypes.length; i++) {
            if (this._acceptedDataTypes[i] === dataType) {
                return true;
            }
        }
        return false;
    }
    function Sys$Preview$UI$DataSourceDropTarget$drop(dragMode, type, data) {
        if (data) {
            var p;
                                                var target = this._target ? this._target : this._control;
            if (this._append) {
                p = target["get_" + this._property];
                if (p) {
                    var targetData = p.call(target);
                    if (targetData) {
                        if (Sys.Preview.Data.IData.isImplementedBy(targetData)) {
                            targetData.add(data);
                        }
                        else if (targetData instanceof Array) {
                            if(typeof(targetData.add) === "function") {
                                targetData.add(data);
                            }
                            else {
                                Array.add(targetData, data);
                            }
                        }
                    } else {
                                                p = target["set_" + this._property];
                        if (p) {
                            p.call(target, data);
                        }
                    }
                }
            }
            else {
                p = target["set_" + this._property];
                if (p) {
                    p.call(target, data);
                }
            }
        }
    }
    function Sys$Preview$UI$DataSourceDropTarget$onDragEnterTarget(dragMode, type, data) {
            }
    function Sys$Preview$UI$DataSourceDropTarget$onDragLeaveTarget(dragMode, type, data) {
            }
    function Sys$Preview$UI$DataSourceDropTarget$onDragInTarget(dragMode, type, data) {
            }
Sys.Preview.UI.DataSourceDropTarget.prototype = {
    _control: null,
    _acceptedDataTypes: null,
    _append: true,
    _target: null,
    _property: "data",
    get_append: Sys$Preview$UI$DataSourceDropTarget$get_append,
    set_append: Sys$Preview$UI$DataSourceDropTarget$set_append,
    get_target: Sys$Preview$UI$DataSourceDropTarget$get_target,
    set_target: Sys$Preview$UI$DataSourceDropTarget$set_target,
    get_property: Sys$Preview$UI$DataSourceDropTarget$get_property,
    set_property: Sys$Preview$UI$DataSourceDropTarget$set_property,
    get_acceptedDataTypes: Sys$Preview$UI$DataSourceDropTarget$get_acceptedDataTypes,
    set_acceptedDataTypes: Sys$Preview$UI$DataSourceDropTarget$set_acceptedDataTypes,
    initialize: Sys$Preview$UI$DataSourceDropTarget$initialize,
    get_dropTargetElement: Sys$Preview$UI$DataSourceDropTarget$get_dropTargetElement,
        canDrop: Sys$Preview$UI$DataSourceDropTarget$canDrop,
        drop: Sys$Preview$UI$DataSourceDropTarget$drop,
        onDragEnterTarget: Sys$Preview$UI$DataSourceDropTarget$onDragEnterTarget,
        onDragLeaveTarget: Sys$Preview$UI$DataSourceDropTarget$onDragLeaveTarget,
        onDragInTarget: Sys$Preview$UI$DataSourceDropTarget$onDragInTarget
}
Sys.Preview.UI.DataSourceDropTarget.registerClass('Sys.Preview.UI.DataSourceDropTarget', Sys.UI.Behavior, Sys.Preview.UI.IDropTarget);
Sys.Preview.UI.DataSourceDropTarget.descriptor = {
    properties: [   {name: 'acceptedDataTypes', type: Array},
                    {name: 'append', type: Boolean},
                    {name: 'dropTargetElement', type: Object, readOnly: true},
                    {name: 'target', type: Object},
                    {name: 'property', type: String} ]
}
Sys.Preview.UI.DraggableListItem = function Sys$Preview$UI$DraggableListItem(element) {
    Sys.Preview.UI.DraggableListItem.initializeBase(this,[element]);
    
    var _data;
    var _handle;
    var _dragVisualTemplate;
    var _dragVisualTemplateInstance;
    
    this.get_data = function this$get_data() {
        if (_data == null) {
            var dragSource = this._findDragSource();
            if (dragSource != null && dragSource.get_dragDataType() == "HTML") {
                return this.get_element();
            }
        }
        
        return _data;
    }
    
    this.set_data = function this$set_data(value) {
        _data = value;
    }
    
    this.get_handle = function this$get_handle() {
        return _handle;
    }
    
    this.set_handle = function this$set_handle(value) {
        if (_handle != null) {
            Sys.UI.DomEvent.removeHandler(_handle, "mousedown", this._handleMouseDown);
            _handle.__draggableBehavior = null;
        }
        if (value.element) {
            value = value.element;
        }
        _handle = value;
        _handle.__draggableBehavior = this;
        
        Sys.UI.DomEvent.addHandler(_handle, "mousedown", this._handleMouseDown);
        _handle.__draggableBehavior = this;
    }
    
    this.get_dragVisualTemplate = function this$get_dragVisualTemplate() {
        return _dragVisualTemplate;
    }
    
    this.set_dragVisualTemplate = function this$set_dragVisualTemplate(value) {
        _dragVisualTemplate = value;
    }
       
    this._handleMouseDown = function this$_handleMouseDown(e) {
        window._event = e;
        _handle.__draggableBehavior._handleMouseDownInternal();
    }
    
    this._handleMouseDownInternal = function this$_handleMouseDownInternal() {
        var ev = window._event;
        if (ev.button <= 1) {
            var dragSource = this._findDragSource();
            if (dragSource != null) {
                var dragVisual = this._createDragVisual();
                dragSource.startDragDrop(this.get_element(), this.get_data(), dragVisual);
                ev.preventDefault();
            }
        }
    }
    
    this._createDragVisual = function this$_createDragVisual() {
        var ev = window._event;
        if (_dragVisualTemplate != null) {
            if (_dragVisualTemplateInstance == null) {
                _dragVisualTemplateInstance = _dragVisualTemplate.createInstance(this.get_element()).instanceElement;
            }
            else if (!Sys.Preview.UI.DragDropManager._getInstance().hasParent(_dragVisualTemplateInstance)) {
                this.get_element().appendChild(_dragVisualTemplateInstance);
            }
            
            var location = { x: ev.clientX, y: ev.clientY };
            location = Sys.Preview.UI.DragDropManager._getInstance().addPoints(location, Sys.Preview.UI.DragDropManager._getInstance().getScrollOffset(document.body, true));
            Sys.UI.DomElement.setLocation(_dragVisualTemplateInstance, location.x, location.y);
        }
        return _dragVisualTemplateInstance;
    }
    
    this._findDragSource = function this$_findDragSource() {
        var element = this.get_element();
        while (element != null) {
            if (element.__dragDropList != null) {
                return element.__dragDropList;
            }
            element = element.parentNode;
        }
        return null;
    }
}
Sys.Preview.UI.DraggableListItem.registerClass('Sys.Preview.UI.DraggableListItem', Sys.UI.Behavior);
Sys.Preview.UI.DraggableListItem.descriptor = {
    properties: [   {name: 'data', type: Object},
                    {name: 'handle', isDomElement: true},
                    {name: 'dragVisualTemplate', type: Sys.Preview.UI.ITemplate} ]
}
Sys.Preview.UI.FloatingBehavior = function Sys$Preview$UI$FloatingBehavior(element) {
    Sys.Preview.UI.FloatingBehavior.initializeBase(this,[element]);
    this._mouseDownHandler = Function.createDelegate(this, this.mouseDownHandler);
}
    function Sys$Preview$UI$FloatingBehavior$add_move(handler) {
var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
if (e) throw e;
        this.get_events().addHandler('move', handler);
    }
    function Sys$Preview$UI$FloatingBehavior$remove_move(handler) {
var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
if (e) throw e;
        this.get_events().removeHandler('move', handler);
    }
    function Sys$Preview$UI$FloatingBehavior$get_handle() {
if (arguments.length !== 0) throw Error.parameterCount();
        return this._handle;
    }
    function Sys$Preview$UI$FloatingBehavior$set_handle(value) {
        if (this._handle) {
            Sys.UI.DomEvent.removeHandler(this._handle, "mousedown", this._mouseDownHandler);
        }
        this._handle = value;
        Sys.UI.DomEvent.addHandler(this._handle, "mousedown", this._mouseDownHandler);
    }
    function Sys$Preview$UI$FloatingBehavior$get_profileProperty() {
if (arguments.length !== 0) throw Error.parameterCount();
        return this._profileProperty;
    }
    function Sys$Preview$UI$FloatingBehavior$set_profileProperty(value) {
        
        this._profileProperty = value;
    }
    function Sys$Preview$UI$FloatingBehavior$get_profileComponent() {
if (arguments.length !== 0) throw Error.parameterCount();
        return this._profileComponent;
    }
    function Sys$Preview$UI$FloatingBehavior$set_profileComponent(value) {
        this._profileComponent = value;
    }
    function Sys$Preview$UI$FloatingBehavior$get_location() {
if (arguments.length !== 0) throw Error.parameterCount();
        return this._location;
    }
    function Sys$Preview$UI$FloatingBehavior$set_location(value) {
        if (this._location != value) {
            this._location = value;
            if (this.get_isInitialized()) {
                var numbers = this._location.split(',');
                var location = { x : parseInt(numbers[0]), y : parseInt(numbers[1]) };
                Sys.UI.DomElement.setLocation(this.get_element(), location.x, location.y);
            }
            this.raisePropertyChanged('location');
        }
    }
    function Sys$Preview$UI$FloatingBehavior$initialize() {
        Sys.Preview.UI.FloatingBehavior.callBaseMethod(this, 'initialize');
        Sys.Preview.UI.DragDropManager.registerDropTarget(this);
        var el = this.get_element();
        var location;
        if (this._location) {
            var numbers = this._location.split(',');
            location = { x : parseInt(numbers[0]), y : parseInt(numbers[1]) };
        }
        else {
            location = Sys.UI.DomElement.getLocation(el);
        }
        if(el.offsetWidth) {
            el.style.width = el.offsetWidth + "px";
        }
        if(el.offsetHeight) {
            el.style.height = el.offsetHeight + "px";
        }
        el.style.position = "absolute";
        Sys.UI.DomElement.setLocation(el, location.x, location.y);
        var p = this.get_profileProperty();
        if(p) {
            var b = new Sys.Preview.Binding();
            b.beginUpdate();
            b.set_target(this);
            b.set_property("location");
            var profile = this.get_profileComponent();
            if(!profile) profile = Sys.Preview.Services.Components.Profile.instance;
            b.set_dataContext(profile);
            b.set_dataPath(p);
            b.set_direction(Sys.Preview.BindingDirection.InOut);
                                    var a = new Sys.Preview.InvokeMethodAction();
            a.beginUpdate();
            a.set_eventSource(profile);
            a.set_eventName("loadComplete");
            a.set_target(b);
            a.set_method("evaluateIn");
            a.endUpdate();
            b.endUpdate();
            this._binding = b;
            this._action = a;
        }
    }
    function Sys$Preview$UI$FloatingBehavior$dispose() {
        Sys.Preview.UI.DragDropManager.unregisterDropTarget(this);
        if (this._handle && this._mouseDownHandler) {
            Sys.UI.DomEvent.removeHandler(this._handle, "mousedown", this._mouseDownHandler);
        }
        this._mouseDownHandler = null;
        Sys.Preview.UI.FloatingBehavior.callBaseMethod(this, 'dispose');
    }
    function Sys$Preview$UI$FloatingBehavior$checkCanDrag(element) {
        var undraggableTagNames = ["input", "button", "select", "textarea", "label"];
        var tagName = element.tagName;
        if ((tagName.toLowerCase() == "a") && (element.href != null) && (element.href.length > 0)) {
            return false;
        }
        if (Array.indexOf(undraggableTagNames, tagName.toLowerCase()) > -1) {
            return false;
        }
        return true;
    }
    function Sys$Preview$UI$FloatingBehavior$mouseDownHandler(ev) {
        window._event = ev;
        var el = this.get_element();
        if (this.checkCanDrag(ev.target)) {
            this._dragStartLocation = Sys.UI.DomElement.getLocation(el);
            ev.preventDefault();
            this.startDragDrop(el);
        }
    }
    function Sys$Preview$UI$FloatingBehavior$get_dragDataType() {
if (arguments.length !== 0) throw Error.parameterCount();
        return "_floatingObject";
    }
    function Sys$Preview$UI$FloatingBehavior$getDragData(context) {
        return null;
    }
    function Sys$Preview$UI$FloatingBehavior$get_dragMode() {
if (arguments.length !== 0) throw Error.parameterCount();
        return Sys.Preview.UI.DragMode.Move;
    }
    function Sys$Preview$UI$FloatingBehavior$onDragStart() { }
    function Sys$Preview$UI$FloatingBehavior$onDrag() { }
    function Sys$Preview$UI$FloatingBehavior$onDragEnd(canceled) {
        if (!canceled) {
            var handler = this.get_events().getHandler('move');
            if(handler) {
                var cancelArgs = new Sys.CancelEventArgs();
                handler(this, cancelArgs);
                canceled = cancelArgs.get_cancel();
            }
        }
        var el = this.get_element();
        if (canceled) {
                        Sys.UI.DomElement.setLocation(el, this._dragStartLocation.x, this._dragStartLocation.y);
        }
        else {
            var location = Sys.UI.DomElement.getLocation(el);
            this._location = location.x + ',' + location.y;
            this.raisePropertyChanged('location');
        }
    }
    function Sys$Preview$UI$FloatingBehavior$startDragDrop(dragVisual) {
        Sys.Preview.UI.DragDropManager.startDragDrop(this, dragVisual, null);
    }
    function Sys$Preview$UI$FloatingBehavior$get_dropTargetElement() {
if (arguments.length !== 0) throw Error.parameterCount();
        return document.body;
    }
    function Sys$Preview$UI$FloatingBehavior$canDrop(dragMode, dataType, data) {
        return (dataType === "_floatingObject");
    }
    function Sys$Preview$UI$FloatingBehavior$drop(dragMode, dataType, data) {}
    function Sys$Preview$UI$FloatingBehavior$onDragEnterTarget(dragMode, dataType, data) {}
    function Sys$Preview$UI$FloatingBehavior$onDragLeaveTarget(dragMode, dataType, data) {}
    function Sys$Preview$UI$FloatingBehavior$onDragInTarget(dragMode, dataType, data) {}
Sys.Preview.UI.FloatingBehavior.prototype = {
    _handle: null,
    _location: null,
    _dragStartLocation: null,
    _profileProperty: null,
    _profileComponent: null,
    add_move: Sys$Preview$UI$FloatingBehavior$add_move,
    remove_move: Sys$Preview$UI$FloatingBehavior$remove_move,
    get_handle: Sys$Preview$UI$FloatingBehavior$get_handle,
    set_handle: Sys$Preview$UI$FloatingBehavior$set_handle,
    get_profileProperty: Sys$Preview$UI$FloatingBehavior$get_profileProperty,
    set_profileProperty: Sys$Preview$UI$FloatingBehavior$set_profileProperty,
    get_profileComponent: Sys$Preview$UI$FloatingBehavior$get_profileComponent,
    set_profileComponent: Sys$Preview$UI$FloatingBehavior$set_profileComponent,
    get_location: Sys$Preview$UI$FloatingBehavior$get_location,
    set_location: Sys$Preview$UI$FloatingBehavior$set_location,
    initialize: Sys$Preview$UI$FloatingBehavior$initialize,
    dispose: Sys$Preview$UI$FloatingBehavior$dispose,
    checkCanDrag: Sys$Preview$UI$FloatingBehavior$checkCanDrag,
    mouseDownHandler: Sys$Preview$UI$FloatingBehavior$mouseDownHandler,
        get_dragDataType: Sys$Preview$UI$FloatingBehavior$get_dragDataType,
        getDragData: Sys$Preview$UI$FloatingBehavior$getDragData,
        get_dragMode: Sys$Preview$UI$FloatingBehavior$get_dragMode,
        onDragStart: Sys$Preview$UI$FloatingBehavior$onDragStart,
        onDrag: Sys$Preview$UI$FloatingBehavior$onDrag,
        onDragEnd: Sys$Preview$UI$FloatingBehavior$onDragEnd,
    startDragDrop: Sys$Preview$UI$FloatingBehavior$startDragDrop,
    get_dropTargetElement: Sys$Preview$UI$FloatingBehavior$get_dropTargetElement,
        canDrop: Sys$Preview$UI$FloatingBehavior$canDrop,
        drop: Sys$Preview$UI$FloatingBehavior$drop,
        onDragEnterTarget: Sys$Preview$UI$FloatingBehavior$onDragEnterTarget,
        onDragLeaveTarget: Sys$Preview$UI$FloatingBehavior$onDragLeaveTarget,
        onDragInTarget: Sys$Preview$UI$FloatingBehavior$onDragInTarget
}
Sys.Preview.UI.FloatingBehavior.descriptor = {
    properties: [   {name: "profileProperty", type: String},
                    {name: "profileComponent", type: Object},
                    {name: "dragData", type: Object, readOnly: true},
                    {name: "dragDataType", type: String, readOnly: true},
                    {name: "dragMode", type: Sys.Preview.UI.DragMode, readOnly: true},
                    {name: "dropTargetElement", type: Object, readOnly: true},
                    {name: "handle", isDomElement: true},
                    {name: "location", type: String} ],
    events: [   {name: "move"} ]
}
Sys.Preview.UI.FloatingBehavior.registerClass('Sys.Preview.UI.FloatingBehavior', Sys.UI.Behavior, Sys.Preview.UI.IDragSource, Sys.Preview.UI.IDropTarget, Sys.IDisposable);
Sys.UI.Control.overlaps = function Sys$UI$Control$overlaps(r1, r2) {
    var xLeft = (r1.x >= r2.x && r1.x <= (r2.x + r2.width));
    var xRight = ((r1.x + r1.width) >= r2.x && (r1.x + r1.width) <= r2.x + r2.width);
    var xComplete = ((r1.x < r2.x) && ((r1.x + r1.height) > (r2.x + r2.height)));
    
    var yLeft = (r1.y >= r2.y && r1.y <= (r2.y + r2.height));
    var yRight = ((r1.y + r1.height) >= r2.y && (r1.y + r1.height) <= r2.y + r2.height);
    var yComplete = ((r1.y < r2.y) && ((r1.y + r1.height) > (r2.y + r2.height)));
    if ((xLeft || xRight || xComplete) && (yLeft || yRight || yComplete)) {
        return true;
    }
   
    return false;
}


if(typeof(Sys)!=='undefined')Sys.Application.notifyScriptLoaded();
