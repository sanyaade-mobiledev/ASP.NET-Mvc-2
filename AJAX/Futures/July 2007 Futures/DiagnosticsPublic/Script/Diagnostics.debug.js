// Name:        Diagnostics.debug.js
// Assembly:    Microsoft.Web.Preview.Diagnostics
// Version:     1.3.61025.0
// FileVersion: 1.3.61025.0
//-----------------------------------------------------------------------
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------
// ExceptionService.js
// Atlas Diagnostics Framework.

Type.registerNamespace('Sys.Preview.Diagnostics');
Sys.Preview._DiagnosticsService = function Sys$Preview$_DiagnosticsService() {
    Sys.Preview._DiagnosticsService.initializeBase(this);
    this._exceptions = [];
    this._traces = [];
    this._timeout = 2000;
    this._timeoutTimerId = -1;
    this.set_path("DiagnosticsService.asmx");
    this.start();
}

    function Sys$Preview$_DiagnosticsService$get_path() {
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._path || "";
    }
    function Sys$Preview$_DiagnosticsService$set_path(value) {
        this._path = value;
    }
    function Sys$Preview$_DiagnosticsService$_onError(msg, url, lno) { 
        var ex = {};
        this._exceptions.push(ex);
        ex.Reported = false;
        ex.Filename = url;   
        ex.LineNumber = lno;        
        ex.Message = msg;   
        return true;
    }
    function Sys$Preview$_DiagnosticsService$start() {  
        if (arguments.length !== 0) throw Error.parameterCount();
        window.onerror = Function.createDelegate(this,this._onError);
        this.startTimer();
    }
    function Sys$Preview$_DiagnosticsService$dispose() {
        this.stopTimer();
        this.reportExceptions();
        window.onerror = null;
        Sys.Preview._DiagnosticsService.callBaseMethod(this, 'dispose');
    }
    function Sys$Preview$_DiagnosticsService$startTimer() {  
        this._timeoutTimerId = window.setTimeout(Function.createDelegate(this,this._onTimer),this._timeout);
    }
    function Sys$Preview$_DiagnosticsService$stopTimer() {
        window.clearTimeout(this._timeoutTimerId);
    }
    function Sys$Preview$_DiagnosticsService$_onTimer() {
        this.reportExceptions();
    }
    function Sys$Preview$_DiagnosticsService$reportExceptions() {
        var parameters = {},
            methodName = "ReportExceptions",
            exceptionInfos = [],
            targetIndex = 0,
            info, ex;
        for (var i = 0, l = this._exceptions.length; i < l; i++) { 
            ex = this._exceptions[i];   
            if (!ex.Reported) {
                exceptionInfos[targetIndex] = info = {}; 
                info.Filename = ex.Filename;
                info.LineNumber = ex.LineNumber;
                info.Message = ex.Message;
                ex.Reported = true;
                targetIndex++;
            }
        }
        if (exceptionInfos.length > 0)  {
            parameters = { exceptionInfos: exceptionInfos };
            Sys.Net.WebServiceProxy.invoke(this.get_path(),  methodName, false,  parameters, null,
                                            Function.createDelegate(this, this._onReportFailed), null);
        }                                        
    }
    function Sys$Preview$_DiagnosticsService$display() {
        if (arguments.length !== 0) throw Error.parameterCount();
        win2=window.open('','window2','scrollbars=yes');
        win2.document.writeln('<p></p><h1>Exception Report</h1><p></p>');
        
        for (var i = 0, l = this._exceptions.length; i < l; i++) {    
            var ex = this._exceptions[i];  
            win2.document.writeln('<b>Reported: ' + ex.Reported + '<br>');      
            win2.document.writeln('<b>Exception in file:</b> ' + ex.Filename + '<br>');      
            win2.document.writeln('<b>Line number:</b> ' + ex.LineNumber + '<br>');      
            win2.document.writeln('<b>Message:</b> ' + ex.Message + '<p>');   
        }
        win2.document.close();
     }
    function Sys$Preview$_DiagnosticsService$_onReportFailed(err, context, methodName) {
    }
Sys.Preview._DiagnosticsService.prototype = {
    get_path: Sys$Preview$_DiagnosticsService$get_path,
    set_path: Sys$Preview$_DiagnosticsService$set_path,
    _onError: Sys$Preview$_DiagnosticsService$_onError,
    start: Sys$Preview$_DiagnosticsService$start,
    dispose: Sys$Preview$_DiagnosticsService$dispose,    
    startTimer: Sys$Preview$_DiagnosticsService$startTimer,
    stopTimer: Sys$Preview$_DiagnosticsService$stopTimer,
    _onTimer: Sys$Preview$_DiagnosticsService$_onTimer,
    reportExceptions: Sys$Preview$_DiagnosticsService$reportExceptions,
    display: Sys$Preview$_DiagnosticsService$display,
    _onReportFailed: Sys$Preview$_DiagnosticsService$_onReportFailed     
}
Sys.Preview._DiagnosticsService.registerClass('Sys.Preview._DiagnosticsService', Sys.Component);
Sys.Preview.DiagnosticsService = new Sys.Preview._DiagnosticsService();


if(typeof(Sys)!=='undefined')Sys.Application.notifyScriptLoaded();
