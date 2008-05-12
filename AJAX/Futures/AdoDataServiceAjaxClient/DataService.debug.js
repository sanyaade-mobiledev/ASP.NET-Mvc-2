//-----------------------------------------------------------------------
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------
// DataService.js
// Microsoft Data Services Framework
Type.registerNamespace('Sys.Data');
Sys.Data.ActionResult = function Sys$Data$ActionResult(result, error, actionContext, operation) {
    /// <summary locid="M:J#Sys.Data.ActionResult.#ctor" />
    /// <param name="result" mayBeNull="true"></param>
    /// <param name="error" mayBeNull="true" type="Sys.Data.DataServiceError"></param>
    /// <param name="actionContext" mayBeNull="true"></param>
    /// <param name="operation" type="String"></param>
    var e = Function._validateParams(arguments, [
        {name: "result", mayBeNull: true},
        {name: "error", type: Sys.Data.DataServiceError, mayBeNull: true},
        {name: "actionContext", mayBeNull: true},
        {name: "operation", type: String}
    ]);
    if (e) throw e;
    this._result = result;
    this._error = error;
    this._actionContext = actionContext;
    this._operation = operation;
}
    function Sys$Data$ActionResult$get_result() {
        /// <value mayBeNull="true" locid="P:J#Sys.Data.ActionResult.result"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._result;
    }
    function Sys$Data$ActionResult$get_error() {
        /// <value mayBeNull="true" type="Sys.Data.DataServiceError" locid="P:J#Sys.Data.ActionResult.error"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._error;
    }
    function Sys$Data$ActionResult$get_actionContext() {
        /// <value mayBeNull="true" locid="P:J#Sys.Data.ActionResult.actionContext"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._actionContext;
    }
    function Sys$Data$ActionResult$get_operation() {
        /// <value type="String" locid="P:J#Sys.Data.ActionResult.operation"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._operation;
    }
Sys.Data.ActionResult.prototype = {
    
    get_result: Sys$Data$ActionResult$get_result,
    get_error: Sys$Data$ActionResult$get_error,
    get_actionContext: Sys$Data$ActionResult$get_actionContext,
    get_operation: Sys$Data$ActionResult$get_operation
};
Sys.Data.ActionResult.registerClass("Sys.Data.ActionResult");
Sys.Data.ActionSequence = function Sys$Data$ActionSequence(dataService) {
    /// <summary locid="M:J#Sys.Data.ActionSequence.#ctor" />
    /// <param name="dataService" type="Sys.Data.DataService"></param>
    var e = Function._validateParams(arguments, [
        {name: "dataService", type: Sys.Data.DataService}
    ]);
    if (e) throw e;
    this._dataService = dataService;
    this._actionQueue = new Array();
}
    function Sys$Data$ActionSequence$get_service() {
        /// <value type="Sys.Data.DataService" locid="P:J#Sys.Data.ActionSequence.service"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._dataService;
    }
    function Sys$Data$ActionSequence$addInsertAction(item, resourceSetUri, actionContext) {
        /// <summary locid="M:J#Sys.Data.ActionSequence.addInsertAction" />
        /// <param name="item" type="Object"></param>
        /// <param name="resourceSetUri" type="String" mayBeNull="true"></param>
        /// <param name="actionContext" mayBeNull="true" optional="true"></param>
        var e = Function._validateParams(arguments, [
            {name: "item", type: Object},
            {name: "resourceSetUri", type: String, mayBeNull: true},
            {name: "actionContext", mayBeNull: true, optional: true}
        ]);
        if (e) throw e;
        
        var dataService = this._dataService;
        Array.enqueue(this._actionQueue, function(o) { dataService.insert(
            item,
            resourceSetUri,
            Sys.Data.ActionSequence._genSuccessCallback(o),
            Sys.Data.ActionSequence._genFailureCallback(o),
            actionContext 
        )});
    }
    function Sys$Data$ActionSequence$addUpdateAction(item, resourceUri, actionContext) {
        /// <summary locid="M:J#Sys.Data.ActionSequence.addUpdateAction" />
        /// <param name="item" type="Object"></param>
        /// <param name="resourceUri" type="String" mayBeNull="true" optional="true"></param>
        /// <param name="actionContext" mayBeNull="true" optional="true"></param>
        var e = Function._validateParams(arguments, [
            {name: "item", type: Object},
            {name: "resourceUri", type: String, mayBeNull: true, optional: true},
            {name: "actionContext", mayBeNull: true, optional: true}
        ]);
        if (e) throw e;
        
        var dataService = this._dataService;
        Array.enqueue(this._actionQueue, function(o) { dataService.update(
            item,
            resourceUri,
            Sys.Data.ActionSequence._genSuccessCallback(o),
            Sys.Data.ActionSequence._genFailureCallback(o),
            actionContext 
        )});
    }
    function Sys$Data$ActionSequence$addRemoveAction(item, resourceUri, actionContext) {
        /// <summary locid="M:J#Sys.Data.ActionSequence.addRemoveAction" />
        /// <param name="item" type="Object" mayBeNull="true"></param>
        /// <param name="resourceUri" type="String" mayBeNull="true" optional="true"></param>
        /// <param name="actionContext" mayBeNull="true" optional="true"></param>
        var e = Function._validateParams(arguments, [
            {name: "item", type: Object, mayBeNull: true},
            {name: "resourceUri", type: String, mayBeNull: true, optional: true},
            {name: "actionContext", mayBeNull: true, optional: true}
        ]);
        if (e) throw e;
        
        var dataService = this._dataService;
        Array.enqueue(this._actionQueue, function(o) { dataService.remove(
            item,
            resourceUri,
            Sys.Data.ActionSequence._genSuccessCallback(o),
            Sys.Data.ActionSequence._genFailureCallback(o),
            actionContext 
        )});
    }
    function Sys$Data$ActionSequence$clearActions() {
        /// <summary locid="M:J#Sys.Data.ActionSequence.clearActions" />
        if (arguments.length !== 0) throw Error.parameterCount();
        Array.clear(this._actionQueue);
    }
    function Sys$Data$ActionSequence$executeActions(actionsCallback, userContext) {
        /// <summary locid="M:J#Sys.Data.ActionSequence.executeActions" />
        /// <param name="actionsCallback" type="Function" mayBeNull="true" optional="true"></param>
        /// <param name="userContext" mayBeNull="true" optional="true"></param>
        var e = Function._validateParams(arguments, [
            {name: "actionsCallback", type: Function, mayBeNull: true, optional: true},
            {name: "userContext", mayBeNull: true, optional: true}
        ]);
        if (e) throw e;
    
        var o = {
            actionResultQueue: new Array(),
            hasError: false,
            remainingActions: this._actionQueue
        };
        
        this._actionQueue = new Array();
        
        Array.enqueue(o.remainingActions, function(o) {
            if (actionsCallback) {
                window.setTimeout(function() { actionsCallback(o.actionResultQueue, o.hasError, userContext); }, 0);
            }
        });
        Array.dequeue(o.remainingActions)(o);
    }
Sys.Data.ActionSequence.prototype = {
    
    get_service: Sys$Data$ActionSequence$get_service,
    
    addInsertAction: Sys$Data$ActionSequence$addInsertAction,
    addUpdateAction: Sys$Data$ActionSequence$addUpdateAction,
    addRemoveAction: Sys$Data$ActionSequence$addRemoveAction,
    clearActions: Sys$Data$ActionSequence$clearActions,
    executeActions: Sys$Data$ActionSequence$executeActions
}

Sys.Data.ActionSequence._genSuccessCallback = function Sys$Data$ActionSequence$_genSuccessCallback(o) {
    return function(result, actionContext, operation) {
        var newAR = new Sys.Data.ActionResult(result, null, actionContext, operation);
        Array.enqueue(o.actionResultQueue, newAR);
        Array.dequeue(o.remainingActions)(o); 
    };
};
Sys.Data.ActionSequence._genFailureCallback = function Sys$Data$ActionSequence$_genFailureCallback(o) {
    return function(error, actionContext, operation) {
        o.hasError = true;
        var newAR = new Sys.Data.ActionResult(null, error, actionContext, operation);
        Array.enqueue(o.actionResultQueue, newAR);
        Array.dequeue(o.remainingActions)(o); 
    };
};
Sys.Data.ActionSequence.registerClass("Sys.Data.ActionSequence");
Sys.Data.DataService = function Sys$Data$DataService(serviceUri) {
    /// <summary locid="M:J#Sys.Data.DataService.#ctor" />
    /// <param name="serviceUri" type="String"></param>
    var e = Function._validateParams(arguments, [
        {name: "serviceUri", type: String}
    ]);
    if (e) throw e;
    this._serviceUri = serviceUri;
    this._timeout = 0;
    this._defaultUserContext = null;
    this._defaultSucceededCallback = null;
    this._defaultFailedCallback = null;
}
    function Sys$Data$DataService$get_serviceUri() {
        /// <value type="String" locid="P:J#Sys.Data.DataService.serviceUri"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._serviceUri;
    }
    function Sys$Data$DataService$get_timeout() {
        /// <value type="Number" integer="true" locid="P:J#Sys.Data.DataService.timeout"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        if (this._timeout === 0) {
            return Sys.Net.WebRequestManager.get_defaultTimeout();
        }
        return this._timeout;
    }
    function Sys$Data$DataService$set_timeout(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: Number, integer: true}]);
        if (e) throw e;
        this._timeout = value;
    }
    function Sys$Data$DataService$get_defaultUserContext() {
        /// <value mayBeNull="true" locid="P:J#Sys.Data.DataService.defaultUserContext"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._defaultUserContext;
    }
    function Sys$Data$DataService$set_defaultUserContext(value) {
        var e = Function._validateParams(arguments, [{name: "value", mayBeNull: true}]);
        if (e) throw e;
        this._defaultUserContext = value;
    }
    function Sys$Data$DataService$get_defaultSucceededCallback() {
        /// <value type="Function" mayBeNull="true" locid="P:J#Sys.Data.DataService.defaultSucceededCallback"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._defaultSucceededCallback;
    }
    function Sys$Data$DataService$set_defaultSucceededCallback(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: Function, mayBeNull: true}]);
        if (e) throw e;
        this._defaultSucceededCallback = value;
    }
    function Sys$Data$DataService$get_defaultFailedCallback() {
        /// <value type="Function" mayBeNull="true" locid="P:J#Sys.Data.DataService.defaultFailedCallback"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._defaultFailedCallback;
    }
    function Sys$Data$DataService$set_defaultFailedCallback(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: Function, mayBeNull: true}]);
        if (e) throw e;
        this._defaultFailedCallback = value;
    }
    function Sys$Data$DataService$query(query, succeededCallback, failedCallback, userContext, webRequest) {
        /// <summary locid="M:J#Sys.Data.DataService.query" />
        /// <param name="query" type="String" mayBeNull="true" optional="true"></param>
        /// <param name="succeededCallback" type="Function" mayBeNull="true" optional="true"></param>
        /// <param name="failedCallback" type="Function" mayBeNull="true" optional="true"></param>
        /// <param name="userContext" mayBeNull="true" optional="true"></param>
        /// <param name="webRequest" type="Sys.Net.WebRequest" mayBeNull="true" optional="true"></param>
        var e = Function._validateParams(arguments, [
            {name: "query", type: String, mayBeNull: true, optional: true},
            {name: "succeededCallback", type: Function, mayBeNull: true, optional: true},
            {name: "failedCallback", type: Function, mayBeNull: true, optional: true},
            {name: "userContext", mayBeNull: true, optional: true},
            {name: "webRequest", type: Sys.Net.WebRequest, mayBeNull: true, optional: true}
        ]);
        if (e) throw e;
        
        var wRequest = this._prepWebRequest(null, query, "GET", succeededCallback, failedCallback, userContext, query, webRequest);
        wRequest.invoke();
    }
    function Sys$Data$DataService$loadProperty(item, property, succeededCallback, failedCallback, userContext, webRequest) {
        /// <summary locid="M:J#Sys.Data.DataService.loadProperty" />
        /// <param name="item" type="Object"></param>
        /// <param name="property" type="String"></param>
        /// <param name="succeededCallback" type="Function" mayBeNull="true" optional="true"></param>
        /// <param name="failedCallback" type="Function" mayBeNull="true" optional="true"></param>
        /// <param name="userContext" mayBeNull="true" optional="true"></param>
        /// <param name="webRequest" type="Sys.Net.WebRequest" mayBeNull="true" optional="true"></param>
        var e = Function._validateParams(arguments, [
            {name: "item", type: Object},
            {name: "property", type: String},
            {name: "succeededCallback", type: Function, mayBeNull: true, optional: true},
            {name: "failedCallback", type: Function, mayBeNull: true, optional: true},
            {name: "userContext", mayBeNull: true, optional: true},
            {name: "webRequest", type: Sys.Net.WebRequest, mayBeNull: true, optional: true}
        ]);
        if (e) throw e;
        
        var succeededHelper = function(result, context, operation) {
            item[operation] = result;
            if (!succeededCallback) {
                succeededCallback = this._defaultSucceededCallback;
            }
            if (succeededCallback) {
                succeededCallback(item, context, operation);
            }
        };
        
        var uri;
        if (item[property] && item[property].__metadata && item[property].__metadata.uri) {
            uri = item[property].__metadata.uri;
        }
        else if (item.__metadata && item.__metadata.uri) {
            uri = item.__metadata.uri + '/' + property;
        }
        else {
            throw Error.create(Sys.Data.Res.dataServiceLoadPropertyUriNotPresent);
        }
        
        var wRequest = this._prepWebRequest(null, uri, "GET", succeededHelper, failedCallback, userContext, property, webRequest);
        wRequest.invoke();
    }
    function Sys$Data$DataService$insert(item, resourceSetUri, succeededCallback, failedCallback, userContext, webRequest) {
        /// <summary locid="M:J#succeededHelper" />
        /// <param name="item" type="Object"></param>
        /// <param name="resourceSetUri" type="String" mayBeNull="true"></param>
        /// <param name="succeededCallback" type="Function" mayBeNull="true" optional="true"></param>
        /// <param name="failedCallback" type="Function" mayBeNull="true" optional="true"></param>
        /// <param name="userContext" mayBeNull="true" optional="true"></param>
        /// <param name="webRequest" type="Sys.Net.WebRequest" mayBeNull="true" optional="true"></param>
        var e = Function._validateParams(arguments, [
            {name: "item", type: Object},
            {name: "resourceSetUri", type: String, mayBeNull: true},
            {name: "succeededCallback", type: Function, mayBeNull: true, optional: true},
            {name: "failedCallback", type: Function, mayBeNull: true, optional: true},
            {name: "userContext", mayBeNull: true, optional: true},
            {name: "webRequest", type: Sys.Net.WebRequest, mayBeNull: true, optional: true}
        ]);
        if (e) throw e;
        
        var wRequest = this._prepWebRequest(null, resourceSetUri, "POST", succeededCallback, failedCallback, userContext, "insert", webRequest);
        
        wRequest.set_body(Sys.Serialization.JavaScriptSerializer.serialize(item));                
        wRequest.get_headers()["Content-Type"] = "application/json";
        wRequest.invoke();
    }
    function Sys$Data$DataService$update(item, resourceUri, succeededCallback, failedCallback, userContext, webRequest) {
        /// <summary locid="M:J#succeededHelper" />
        /// <param name="item" type="Object"></param>
        /// <param name="resourceUri" type="String" mayBeNull="true"></param>
        /// <param name="succeededCallback" type="Function" mayBeNull="true" optional="true"></param>
        /// <param name="failedCallback" type="Function" mayBeNull="true" optional="true"></param>
        /// <param name="userContext" mayBeNull="true" optional="true"></param>
        /// <param name="webRequest" type="Sys.Net.WebRequest" mayBeNull="true" optional="true"></param>
        var e = Function._validateParams(arguments, [
            {name: "item", type: Object},
            {name: "resourceUri", type: String, mayBeNull: true},
            {name: "succeededCallback", type: Function, mayBeNull: true, optional: true},
            {name: "failedCallback", type: Function, mayBeNull: true, optional: true},
            {name: "userContext", mayBeNull: true, optional: true},
            {name: "webRequest", type: Sys.Net.WebRequest, mayBeNull: true, optional: true}
        ]);
        if (e) throw e;
        
        var wRequest = this._prepWebRequest(item, resourceUri, "PUT", succeededCallback, failedCallback, userContext, "update", webRequest);
        wRequest.get_headers()["Content-Type"] = "application/json";
        wRequest.invoke();
    }
    function Sys$Data$DataService$remove(item, resourceUri, succeededCallback, failedCallback, userContext, webRequest) {
        /// <summary locid="M:J#succeededHelper" />
        /// <param name="item" type="Object" mayBeNull="true"></param>
        /// <param name="resourceUri" type="String" mayBeNull="true" optional="true"></param>
        /// <param name="succeededCallback" type="Function" mayBeNull="true" optional="true"></param>
        /// <param name="failedCallback" type="Function" mayBeNull="true" optional="true"></param>
        /// <param name="userContext" mayBeNull="true" optional="true"></param>
        /// <param name="webRequest" type="Sys.Net.WebRequest" mayBeNull="true" optional="true"></param>
        var e = Function._validateParams(arguments, [
            {name: "item", type: Object, mayBeNull: true},
            {name: "resourceUri", type: String, mayBeNull: true, optional: true},
            {name: "succeededCallback", type: Function, mayBeNull: true, optional: true},
            {name: "failedCallback", type: Function, mayBeNull: true, optional: true},
            {name: "userContext", mayBeNull: true, optional: true},
            {name: "webRequest", type: Sys.Net.WebRequest, mayBeNull: true, optional: true}
        ]);
        if (e) throw e;
        
        if (!((item && item.__metadata && item.__metadata.uri) || resourceUri)) {
            throw Error.create(Sys.Data.Res.dataServiceRemoveUriNotPresent);
        }
        
        var wRequest = this._prepWebRequest(item, resourceUri, "DELETE",
            this._cbReplaceResult(succeededCallback, null) ,
            failedCallback, userContext, "remove", webRequest);
        
        delete wRequest.get_headers()["Content-Type"];
        wRequest.set_body(null);
        
        wRequest.invoke();
    }
    function Sys$Data$DataService$invoke(operationUri, httpVerb, parameters, succeededCallback, failedCallback, userContext, webRequest) {
        /// <summary locid="M:J#succeededHelper" />
        /// <param name="operationUri" type="String"></param>
        /// <param name="httpVerb" type="String" maybeNull="true" optional="true"></param>
        /// <param name="parameters" type="Object" maybeNull="true" optional="true"></param>
        /// <param name="succeededCallback" type="Function" mayBeNull="true" optional="true"></param>
        /// <param name="failedCallback" type="Function" mayBeNull="true" optional="true"></param>
        /// <param name="userContext" mayBeNull="true" optional="true"></param>
        /// <param name="webRequest" type="Sys.Net.WebRequest" mayBeNull="true" optional="true"></param>
        var e = Function._validateParams(arguments, [
            {name: "operationUri", type: String},
            {name: "httpVerb", type: String, optional: true},
            {name: "parameters", type: Object, optional: true},
            {name: "succeededCallback", type: Function, mayBeNull: true, optional: true},
            {name: "failedCallback", type: Function, mayBeNull: true, optional: true},
            {name: "userContext", mayBeNull: true, optional: true},
            {name: "webRequest", type: Sys.Net.WebRequest, mayBeNull: true, optional: true}
        ]);
        if (e) throw e;
        
        var qb = new Sys.Data.QueryBuilder(operationUri);
        for (key in parameters) {
            qb.get_queryParams()[encodeURIComponent(key)] = encodeURIComponent(parameters[key]);
        }
        
        var wRequest = this._prepWebRequest(null, qb.toString(), httpVerb, succeededCallback, failedCallback, userContext, operationUri, webRequest);
        if (httpVerb == "POST") {
            wRequest.get_headers()["X-Service-Post"] = "true";
        }
        wRequest.invoke();        
    }
    function Sys$Data$DataService$createActionSequence() {
        /// <summary locid="M:J#succeededHelper" />
        /// <returns type="Sys.Data.ActionSequence"></returns>
        if (arguments.length !== 0) throw Error.parameterCount();
        return new Sys.Data.ActionSequence(this);
    }
    function Sys$Data$DataService$_prepWebRequest(item, relUri, verb, onSuccess, onFailure, context, operation, wRequest) {
    
        if (!relUri) {
            relUri = "";
        }
        if (!wRequest) {
            wRequest = new Sys.Net.WebRequest();
        }
    
        wRequest.set_url(Sys.Data.DataService._concatUris(this._serviceUri, relUri));
        wRequest.set_httpVerb(verb);
        wRequest.set_timeout(this.get_timeout());
        
        var headers = wRequest.get_headers();
        headers["Accept"] = "application/json";
        
        if (item) {
            wRequest.set_body(Sys.Serialization.JavaScriptSerializer.serialize(item));                
            headers["Content-Type"] = "application/json";
            if (item.__metadata && item.__metadata.uri)
                wRequest.set_url(item.__metadata.uri);
        }
        
        if (!onSuccess) {
            onSuccess = this._defaultSucceededCallback;    
        }
        if (!onFailure) {
            onFailure = this._defaultFailedCallback;
        }
        if (!context) {
            context = this._defaultUserContext;
        }
        
        wRequest.add_completed(function(executor, eventArgs) {
            Sys.Data.DataService._callbackHelper(executor, eventArgs, onSuccess, onFailure, context, operation);
        });
        
        return wRequest;
    
    }
    function Sys$Data$DataService$_cbReplaceResult(cb, retVal) {
        if (!cb) {
            cb = this._defaultSucceededCallback;
        }
        return (cb)
            ? function(result, context, operation) { cb(retVal, context, operation); }
            : null;
    }
Sys.Data.DataService.prototype = {
    
    get_serviceUri: Sys$Data$DataService$get_serviceUri,
    get_timeout: Sys$Data$DataService$get_timeout,
    set_timeout: Sys$Data$DataService$set_timeout,
    get_defaultUserContext: Sys$Data$DataService$get_defaultUserContext,
    set_defaultUserContext: Sys$Data$DataService$set_defaultUserContext,
    get_defaultSucceededCallback: Sys$Data$DataService$get_defaultSucceededCallback,
    set_defaultSucceededCallback: Sys$Data$DataService$set_defaultSucceededCallback,
    get_defaultFailedCallback: Sys$Data$DataService$get_defaultFailedCallback,
    set_defaultFailedCallback: Sys$Data$DataService$set_defaultFailedCallback,
    
    query: Sys$Data$DataService$query,
    loadProperty: Sys$Data$DataService$loadProperty,
    insert: Sys$Data$DataService$insert,
    update: Sys$Data$DataService$update,
    remove: Sys$Data$DataService$remove,
    invoke: Sys$Data$DataService$invoke,
    createActionSequence: Sys$Data$DataService$createActionSequence,
    
    _prepWebRequest: Sys$Data$DataService$_prepWebRequest,
    _cbReplaceResult: Sys$Data$DataService$_cbReplaceResult
}

Sys.Data.DataService._concatUris = function Sys$Data$DataService$_concatUris(serviceUri, resourceUri) {
    if (serviceUri.endsWith('/')) {
        serviceUri = serviceUri.substr(0, serviceUri.length - 1);
    }
        
    if (resourceUri.startsWith('/')) {
        resourceUri = resourceUri.substr(1);
    }
    
    return serviceUri + '/' + resourceUri;
};
Sys.Data.DataService._callbackHelper = function Sys$Data$DataService$_callbackHelper(executor, eventArgs, onSuccess, onFailure, userContext, operation) {
    if (executor.get_responseAvailable()) {
        var statusCode = executor.get_statusCode();
        if (statusCode == 1223) {
            statusCode = 204;
        }
        var result = null;
       
        try {
            var contentType = executor.getResponseHeader("Content-Type");
            if (contentType.startsWith("application/json")) {
                result = executor.get_object();
            }
            else if (contentType.startsWith("text/xml")) {
                result = executor.get_xml();
            }
            else {
                result = executor.get_responseData();
            }
        } catch (ex) {
        }
        var error = executor.getResponseHeader("jsonerror");
        var errorObj = (error === "true");
        if (errorObj) {
            if (result) {
                result = new Sys.Data.DataServiceError(false, result.Message, result.StackTrace, result.ExceptionType);
            }
        }
        else if (contentType.startsWith("application/json")) {
            if (!result || typeof(result.d) === "undefined") {
                throw Sys.Data.DataService._createFailedError(operation, String.format("The data operation '{0}' returned invalid data. The JSON wrapper is incorrect.", operation));
            }
            result = result.d;
        }
        if (((statusCode < 200) || (statusCode >= 300)) || errorObj) {
            if (onFailure) {
                if (!result || !errorObj) {
                    result = new Sys.Data.DataServiceError(false , String.format("The data operation '{0}' failed.", operation), "", "");
                }
                result._statusCode = statusCode;
                onFailure(result, userContext, operation);
            }
            else {
                if (result && errorObj) {
                    error = result.get_exceptionType() + "-- " + result.get_message();
                }
                else {
                    error = executor.get_responseData();
                }
                throw Sys.Data.DataService._createFailedError(operation, String.format("The data operation '{0}' failed with the following error: {1}", operation, error));
            }
        }
        else if (onSuccess) {
            onSuccess(result, userContext, operation);
        }
    }
    else {
        var msg;
        if (executor.get_timedOut()) {
            msg = String.format("The data operation '{0}' timed out.", operation);
        }
        else {
            msg = String.format("The data operation '{0}' failed.", operation)
        }
        if (onFailure) {
            onFailure(new Sys.Data.DataServiceError(executor.get_timedOut(), msg, "", ""), userContext, operation);
        }
        else {
            throw Sys.Data.DataService._createFailedError(operation, msg);
        }
    }
};
Sys.Data.DataService._createFailedError = function Sys$Data$DataService$_createFailedError(operation, errorMessage) {
    var displayMessage = "Sys.Data.DataServiceFailedException: " + errorMessage;
    var e = Error.create(displayMessage, { 'name': 'Sys.Data.DataServiceFailedException', 'operation': operation });
    e.popStackFrame();
    return e;
}
Sys.Data.DataService.registerClass("Sys.Data.DataService");
Sys.Data.DataServiceError = function Sys$Data$DataServiceError(timedOut, message, stackTrace, exceptionType) {
    /// <summary locid="M:J#Sys.Data.DataServiceError.#ctor" />
    /// <param name="timedOut" type="Boolean"></param>
    /// <param name="message" type="String" mayBeNull="true"></param>
    /// <param name="stackTrace" type="String" mayBeNull="true"></param>
    /// <param name="exceptionType" type="String" mayBeNull="true"></param>
    var e = Function._validateParams(arguments, [
        {name: "timedOut", type: Boolean},
        {name: "message", type: String, mayBeNull: true},
        {name: "stackTrace", type: String, mayBeNull: true},
        {name: "exceptionType", type: String, mayBeNull: true}
    ]);
    if (e) throw e;
    this._timedOut = timedOut;
    this._message = message;
    this._stackTrace = stackTrace;
    this._exceptionType = exceptionType;
    this._statusCode = -1;
}
    function Sys$Data$DataServiceError$get_timedOut() {
        /// <value type="Boolean" locid="P:J#Sys.Data.DataServiceError.timedOut"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._timedOut;
    }
    function Sys$Data$DataServiceError$get_statusCode() {
        /// <value type="Number" locid="P:J#Sys.Data.DataServiceError.statusCode"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._statusCode;
    }
    function Sys$Data$DataServiceError$get_message() {
        /// <value type="String" locid="P:J#Sys.Data.DataServiceError.message"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._message;
    }
    function Sys$Data$DataServiceError$get_stackTrace() {
        /// <value type="String" locid="P:J#Sys.Data.DataServiceError.stackTrace"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._stackTrace;
    }
    function Sys$Data$DataServiceError$get_exceptionType() {
        /// <value type="String" locid="P:J#Sys.Data.DataServiceError.exceptionType"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._exceptionType;
    }
Sys.Data.DataServiceError.prototype = {
    
    get_timedOut: Sys$Data$DataServiceError$get_timedOut,
    get_statusCode: Sys$Data$DataServiceError$get_statusCode,
    get_message: Sys$Data$DataServiceError$get_message,
    get_stackTrace: Sys$Data$DataServiceError$get_stackTrace,
    get_exceptionType: Sys$Data$DataServiceError$get_exceptionType
}
Sys.Data.DataServiceError.registerClass("Sys.Data.DataServiceError");
Sys.Data.QueryBuilder = function Sys$Data$QueryBuilder(uri) {
    /// <summary locid="M:J#Sys.Data.QueryBuilder.#ctor" />
    /// <param name="uri" type="String"></param>
    var e = Function._validateParams(arguments, [
        {name: "uri", type: String}
    ]);
    if (e) throw e;
    this._queryparams = new Object();
    this._uri = uri;
    
    var idxQuery = uri.indexOf('?');
    if (idxQuery >= 0) {
        this._uri = uri.substr(0, idxQuery);
        var params = uri.substr(idxQuery + 1).split('&');
        for (var i in params) {
            param = params[i];
            var idxValue = param.indexOf('=');
            if (idxValue >= 0) {
                this._queryparams[param.substr(0, idxValue)] = param.substr(idxValue + 1);
            }
            else {
                this._queryparams[param] = "";
            }
        }
    }
}
    function Sys$Data$QueryBuilder$get_skip() {
        /// <value type="Number" integer="true" mayBeNull="true" locid="P:J#Sys.Data.QueryBuilder.skip"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._getIntParam("$skip");
    }
    function Sys$Data$QueryBuilder$set_skip(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: Number, mayBeNull: true, integer: true}]);
        if (e) throw e;
        this._setParam("$skip", value);
    }
    function Sys$Data$QueryBuilder$get_top() {
        /// <value type="Number" integer="true" mayBeNull="true" locid="P:J#Sys.Data.QueryBuilder.top"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._getIntParam("$top");
    }
    function Sys$Data$QueryBuilder$set_top(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: Number, mayBeNull: true, integer: true}]);
        if (e) throw e;
        this._setParam("$top", value);
    }
    function Sys$Data$QueryBuilder$get_orderby() {
        /// <value type="String" mayBeNull="true" locid="P:J#Sys.Data.QueryBuilder.orderby"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._getStringParam("$orderby");
    }
    function Sys$Data$QueryBuilder$set_orderby(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: String, mayBeNull: true}]);
        if (e) throw e;
        this._setParam("$orderby", value);
    }
    function Sys$Data$QueryBuilder$get_filter() {
        /// <value type="String" mayBeNull="true" locid="P:J#Sys.Data.QueryBuilder.filter"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._getStringParam("$filter");
    }
    function Sys$Data$QueryBuilder$set_filter(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: String, mayBeNull: true}]);
        if (e) throw e;
        this._setParam("$filter", value);
    }
    function Sys$Data$QueryBuilder$get_expand() {
        /// <value type="String" mayBeNull="true" locid="P:J#Sys.Data.QueryBuilder.expand"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._getStringParam("$expand");
    }
    function Sys$Data$QueryBuilder$set_expand(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: String, mayBeNull: true}]);
        if (e) throw e;
        this._setParam("$expand", value);
    }
    function Sys$Data$QueryBuilder$get_resourcePath() {
        /// <value type="string" locid="P:J#Sys.Data.QueryBuilder.resourcePath"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._uri;
    }
    function Sys$Data$QueryBuilder$get_queryParams() {
        /// <value type="Object" locid="P:J#Sys.Data.QueryBuilder.queryParams"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._queryparams;
    }
    function Sys$Data$QueryBuilder$toString() {
        /// <summary locid="M:J#Sys.Data.QueryBuilder.toString" />
        /// <returns type="string"></returns>
        if (arguments.length !== 0) throw Error.parameterCount();
        var params = new Array();
        for (var key in this._queryparams) {
            if (!Array.contains(Sys.Data.QueryBuilder._queryOptions, key)) {
                var value = this._queryparams[key];
                if (value != null) {
                    Array.add(params, { key: key, value: value });
                }
            }
        }
        for (var i in Sys.Data.QueryBuilder._queryOptions) {
            var key = Sys.Data.QueryBuilder._queryOptions[i];
            var value = this._queryparams[key];
            if (value != null) {
                Array.add(params, { key: key, value: value });
            }
        }
        
        var sb = new Sys.StringBuilder(this._uri);
        var firstElement = true;
        for (var i in params) {
            sb.append((firstElement) ? '?' : '&');
            sb.append(params[i].key);
            sb.append('=');
            sb.append(params[i].value);
            firstElement = false;
        }
        return sb.toString();
    }
    function Sys$Data$QueryBuilder$_setParam(name, value) {
        if (value == null) {
            delete this._queryparams[name];
        } else {
            this._queryparams[name] = value;
        }
    }
    function Sys$Data$QueryBuilder$_getStringParam(name) {
        var value = this._queryparams[name];
        return (value === undefined) ? null : value;
    }
    function Sys$Data$QueryBuilder$_getIntParam(name) {
        var value = parseInt(this._queryparams[name]);
        return (isNaN(value)) ? null : value;
    }
Sys.Data.QueryBuilder.prototype = {
    
    get_skip: Sys$Data$QueryBuilder$get_skip,
    set_skip: Sys$Data$QueryBuilder$set_skip,
    get_top: Sys$Data$QueryBuilder$get_top,
    set_top: Sys$Data$QueryBuilder$set_top,
    get_orderby: Sys$Data$QueryBuilder$get_orderby,
    set_orderby: Sys$Data$QueryBuilder$set_orderby,
    get_filter: Sys$Data$QueryBuilder$get_filter,
    set_filter: Sys$Data$QueryBuilder$set_filter,
    get_expand: Sys$Data$QueryBuilder$get_expand,
    set_expand: Sys$Data$QueryBuilder$set_expand,
    get_resourcePath: Sys$Data$QueryBuilder$get_resourcePath,
    
    get_queryParams: Sys$Data$QueryBuilder$get_queryParams,
    
    toString: Sys$Data$QueryBuilder$toString,
    
    _setParam: Sys$Data$QueryBuilder$_setParam,
    _getStringParam: Sys$Data$QueryBuilder$_getStringParam,
    _getIntParam: Sys$Data$QueryBuilder$_getIntParam
};

Sys.Data.QueryBuilder._queryOptions = new Array("$filter", "$orderby", "$skip", "$top");
Sys.Data.QueryBuilder.registerClass("Sys.Data.QueryBuilder");

