// Name:        PreviewGlitz.debug.js
// Assembly:    Microsoft.Web.Preview
// Version:     2.0.21022.0
// FileVersion: 2.0.21022.0
Type.registerNamespace('Sys.Preview.UI.Effects');
Sys.Preview.UI.Effects.Glitz = function Sys$Preview$UI$Effects$Glitz() {
    throw Error.invalidOperation();
}
Sys.Preview.UI.Effects.Glitz.getElementOpacity = function Sys$Preview$UI$Effects$Glitz$getElementOpacity(element) {
    /// <param name="element" domElement="true"></param>
    /// <returns type="Number"></returns>
    var e = Function._validateParams(arguments, [
        {name: "element", domElement: true}
    ]);
    if (e) throw e;
    var hasOpacity = false;
    var opacity;
    
    if (element.filters) {
        var filters = element.filters;
        if (filters.length !== 0) {
            var alphaFilter = filters['DXImageTransform.Microsoft.Alpha'];
            if (alphaFilter) {
                opacity = alphaFilter.opacity / 100.0;
                hasOpacity = true;
            }
        }
    }
    else {
        var computedStyle = document.defaultView.getComputedStyle;
        opacity = computedStyle(element, null).getPropertyValue('opacity');
        hasOpacity = true;
    }
    
    if (hasOpacity === false) {
        return 1.0;
    }
    return parseFloat(opacity);
}
Sys.Preview.UI.Effects.Glitz.interpolate = function Sys$Preview$UI$Effects$Glitz$interpolate(value1, value2, percentage) {
    /// <param name="value1" type="Number"></param>
    /// <param name="value2" type="Number"></param>
    /// <param name="percentage" type="Number"></param>
    /// <returns type="Number"></returns>
    var e = Function._validateParams(arguments, [
        {name: "value1", type: Number},
        {name: "value2", type: Number},
        {name: "percentage", type: Number}
    ]);
    if (e) throw e;
    return value1 + (value2 - value1) * (percentage / 100);
}
Sys.Preview.UI.Effects.Glitz.setElementOpacity = function Sys$Preview$UI$Effects$Glitz$setElementOpacity(element, value) {
    /// <param name="element" domElement="true"></param>
    /// <param name="value" type="Number"></param>
    var e = Function._validateParams(arguments, [
        {name: "element", domElement: true},
        {name: "value", type: Number}
    ]);
    if (e) throw e;
    if (element.filters) {
        var filters = element.filters;
        var createFilter = true;
        if (filters.length !== 0) {
            var alphaFilter = filters['DXImageTransform.Microsoft.Alpha'];
            if (alphaFilter) {
                createFilter = false;
                alphaFilter.opacity = value * 100;
            }
        }
        if (createFilter) {
            element.style.filter = 'progid:DXImageTransform.Microsoft.Alpha(opacity=' + (value * 100) + ')';
        }
    }
    else {
        element.style.opacity = value;
    }
}
Sys.Preview.UI.Effects.OpacityBehavior = function Sys$Preview$UI$Effects$OpacityBehavior(element) {
    /// <param name="element" domElement="true"></param>
    var e = Function._validateParams(arguments, [
        {name: "element", domElement: true}
    ]);
    if (e) throw e;
    Sys.Preview.UI.Effects.OpacityBehavior.initializeBase(this,[element]);
}
    function Sys$Preview$UI$Effects$OpacityBehavior$get_value() {
        /// <value type="Number"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        var element = this.get_element();
        if (!element) {
            return this._value;
        }
        return Sys.Preview.UI.Effects.Glitz.getElementOpacity(element);
    }
    function Sys$Preview$UI$Effects$OpacityBehavior$set_value(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: Number}]);
        if (e) throw e;
        var element = this.get_element();
        if (!element) {
            this._value = value;
            return;
        }
        Sys.Preview.UI.Effects.Glitz.setElementOpacity(element, value);
    }
    function Sys$Preview$UI$Effects$OpacityBehavior$initialize() {
        Sys.Preview.UI.Effects.OpacityBehavior.callBaseMethod(this, 'initialize');
        if (this._value !== 1) {
            this.set_value(this._value);
        }
    }
Sys.Preview.UI.Effects.OpacityBehavior.prototype = {
    _value: 1,
    
    get_value: Sys$Preview$UI$Effects$OpacityBehavior$get_value,
    
    set_value: Sys$Preview$UI$Effects$OpacityBehavior$set_value,
    
    initialize: Sys$Preview$UI$Effects$OpacityBehavior$initialize
}
Sys.Preview.UI.Effects.OpacityBehavior.registerClass('Sys.Preview.UI.Effects.OpacityBehavior', Sys.UI.Behavior);
Sys.Preview.UI.Effects.OpacityBehavior.descriptor = {
    properties: [ {name: 'value', type: Number} ]
}
Sys.Preview.UI.Effects.Animation = function Sys$Preview$UI$Effects$Animation() {
    Sys.Preview.UI.Effects.Animation.initializeBase(this);
}
    function Sys$Preview$UI$Effects$Animation$get_duration() {
        /// <value type="Number"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._duration;
    }
    function Sys$Preview$UI$Effects$Animation$set_duration(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: Number}]);
        if (e) throw e;
        this._duration = value;
    }
    function Sys$Preview$UI$Effects$Animation$get_fps() {
        /// <value type="Number"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._fps;
    }
    function Sys$Preview$UI$Effects$Animation$set_fps(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: Number}]);
        if (e) throw e;
        this._fps = value;
    }
    function Sys$Preview$UI$Effects$Animation$get_isActive() {
        /// <value type="Boolean"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return (this._timer !== null);
    }
    function Sys$Preview$UI$Effects$Animation$get_isPlaying() {
        /// <value type="Boolean"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return (this._timer !== null) && this._timer.get_enabled();
    }
    function Sys$Preview$UI$Effects$Animation$get_percentComplete() {
        /// <value type="Number"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._percentComplete;
    }
    function Sys$Preview$UI$Effects$Animation$get_target() {
        /// <value></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._target;
    }
    function Sys$Preview$UI$Effects$Animation$set_target(value) {
        var e = Function._validateParams(arguments, [{name: "value"}]);
        if (e) throw e;
        this._target = value;
    }
    function Sys$Preview$UI$Effects$Animation$add_ended(handler) {
        var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
        if (e) throw e;
        this.get_events().addHandler("ended", handler);
    }
    function Sys$Preview$UI$Effects$Animation$remove_ended(handler) {
        var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
        if (e) throw e;
        this.get_events().removeHandler("ended", handler);
    }
    function Sys$Preview$UI$Effects$Animation$add_started(handler) {
        var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
        if (e) throw e;
        this.get_events().addHandler("started", handler);
    }
    function Sys$Preview$UI$Effects$Animation$remove_started(handler) {
        var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
        if (e) throw e;
        this.get_events().removeHandler("started", handler);
    }
    function Sys$Preview$UI$Effects$Animation$dispose() {
        if (this._timer) {
            this._timer.dispose();
            this._timer = null;
        }
        
        this._tickHandler = null;
        this._target = null;
        
        Sys.Preview.UI.Effects.Animation.callBaseMethod(this, 'dispose');
    }
    function Sys$Preview$UI$Effects$Animation$getAnimatedValue() {
        throw Error.notImplemented();
    }
    function Sys$Preview$UI$Effects$Animation$onEnd() {
    }
    function Sys$Preview$UI$Effects$Animation$onStart() {
    }
    function Sys$Preview$UI$Effects$Animation$onStep(percentage) {
        this.setValue(this.getAnimatedValue(percentage));
    }
    function Sys$Preview$UI$Effects$Animation$pause() {
        if (!this._parentAnimation) {
            if (this._timer) {
                this._timer.set_enabled(false);
                
                this.raisePropertyChanged('isPlaying');
            }
        }
    }
    function Sys$Preview$UI$Effects$Animation$play() {
        if (!this._parentAnimation) {
            var resume = true;
            if (!this._timer) {
                resume = false;
                
                if (!this._tickHandler) {
                    this._tickHandler = Function.createDelegate(this, this._onTimerTick);
                }
                
                this._timer = new Sys.Preview.Timer();
                this._timer.set_interval(1000 / this._fps);
                this._timer.add_tick(this._tickHandler);
                this._percentDelta = 100 / (this._duration * this._fps);
                
                this.onStart();
                this._updatePercentComplete(0, true);
            }
            this._timer.set_enabled(true);
            
            this.raisePropertyChanged('isPlaying');
            if (!resume) {
                this.raisePropertyChanged('isActive');
            }
        }
    }
    function Sys$Preview$UI$Effects$Animation$setOwner(owner) {
        /// <param name="owner"></param>
        var e = Function._validateParams(arguments, [
            {name: "owner"}
        ]);
        if (e) throw e;
        this._parentAnimation = owner;
    }
    function Sys$Preview$UI$Effects$Animation$setValue() {
        throw Error.notImplemented();
    }
    function Sys$Preview$UI$Effects$Animation$stop() {
        if (!this._parentAnimation) {
            var t = this._timer;
            this._timer = null;
            if (t) {
                t.dispose();
                
                this._updatePercentComplete(100);
                this.onEnd();
                
                this.raisePropertyChanged('isPlaying');
                this.raisePropertyChanged('isActive');
            }
        }
    }
    function Sys$Preview$UI$Effects$Animation$_onTimerTick() {
        this._updatePercentComplete(this._percentComplete + this._percentDelta, true);
    }
    function Sys$Preview$UI$Effects$Animation$_updatePercentComplete(percentComplete, animate) {
        if (percentComplete > 100) {
            percentComplete = 100;
        }
        
        this._percentComplete = percentComplete;
        this.raisePropertyChanged('percentComplete');
        
        if (animate) {
            this.onStep(percentComplete);
        }
        
        if (percentComplete === 100) {
            this.stop();
        }
    }
Sys.Preview.UI.Effects.Animation.prototype = {
    _duration: 1,
    _fps: 25,
    _target: null,
    _tickHandler: null,
    _timer: null,
    _percentComplete: 0,
    _percentDelta: null,
    _parentAnimation: null,
    
    get_duration: Sys$Preview$UI$Effects$Animation$get_duration,
    set_duration: Sys$Preview$UI$Effects$Animation$set_duration,
    
    get_fps: Sys$Preview$UI$Effects$Animation$get_fps,
    set_fps: Sys$Preview$UI$Effects$Animation$set_fps,
    
    get_isActive: Sys$Preview$UI$Effects$Animation$get_isActive,
    
    get_isPlaying: Sys$Preview$UI$Effects$Animation$get_isPlaying,
    get_percentComplete: Sys$Preview$UI$Effects$Animation$get_percentComplete,
    
    get_target: Sys$Preview$UI$Effects$Animation$get_target,
    set_target: Sys$Preview$UI$Effects$Animation$set_target,
    add_ended: Sys$Preview$UI$Effects$Animation$add_ended,
    remove_ended: Sys$Preview$UI$Effects$Animation$remove_ended,
    
    add_started: Sys$Preview$UI$Effects$Animation$add_started,
    remove_started: Sys$Preview$UI$Effects$Animation$remove_started,
    
    dispose: Sys$Preview$UI$Effects$Animation$dispose,
    
    getAnimatedValue: Sys$Preview$UI$Effects$Animation$getAnimatedValue,
    onEnd: Sys$Preview$UI$Effects$Animation$onEnd,
    
    onStart: Sys$Preview$UI$Effects$Animation$onStart,
    
    onStep: Sys$Preview$UI$Effects$Animation$onStep,
    
    pause: Sys$Preview$UI$Effects$Animation$pause,
    
    play: Sys$Preview$UI$Effects$Animation$play,
    
    setOwner: Sys$Preview$UI$Effects$Animation$setOwner,
    setValue: Sys$Preview$UI$Effects$Animation$setValue,
    
    stop: Sys$Preview$UI$Effects$Animation$stop,
    
    _onTimerTick: Sys$Preview$UI$Effects$Animation$_onTimerTick,
    _updatePercentComplete: Sys$Preview$UI$Effects$Animation$_updatePercentComplete
}
Sys.Preview.UI.Effects.Animation.registerClass('Sys.Preview.UI.Effects.Animation', Sys.Component);
Sys.Preview.UI.Effects.Animation.descriptor = {
    properties: [   {name: 'duration', type: Number},
                    {name: 'fps', type: Number},
                    {name: 'isActive', type: Boolean, readOnly: true},
                    {name: 'isPlaying', type: Boolean, readOnly: true},
                    {name: 'percentComplete', type: Number, readOnly: true},
                    {name: 'target', type: Object} ],
    methods: [  {name: 'play'},
                {name: 'pause'},
                {name: 'stop'} ]
}
Sys.Preview.UI.Effects.AnimationManager = function Sys$Preview$UI$Effects$AnimationManager() {
    Sys.Preview.UI.Effects.AnimationManager.initializeBase(this);
    this._animations = [];
}
    function Sys$Preview$UI$Effects$AnimationManager$get_fps() {
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._fps;
    }
    function Sys$Preview$UI$Effects$AnimationManager$set_fps(value) {
        this._fps = value;
    }
    function Sys$Preview$UI$Effects$AnimationManager$addAnimation(animation) {
        Array.add(this._animations, animation);
        if (!this._timer) {
            this._timer = new Sys.Preview.Timer();
            this._timer.set_interval(1000 / this._fps);
            this._timer.add_tick(this._onTickHandler);
        }
        this._timer.set_enabled(true);
    }
    function Sys$Preview$UI$Effects$AnimationManager$dispose() {
        if (this._timer) {
            this._timer.dispose();
            this._timer = null;
        }
        Array.clear(this._animations);
    }
    function Sys$Preview$UI$Effects$AnimationManager$removeAnimation(animation) {
        Array.remove(this._animations, animation);
        if (this._animations.length === 0) {
            this._timer.set_enabled(false);
        }
    }
    function Sys$Preview$UI$Effects$AnimationManager$_onTickHandler(sender, eventArgs) {
        for (var i = this._animations.length - 1; i >= 0; i--) {
            this._animations[i]._onTick();
        }
    }
Sys.Preview.UI.Effects.AnimationManager.prototype = {
    _fps: 25,
    _timer: null,
    get_fps: Sys$Preview$UI$Effects$AnimationManager$get_fps,
    set_fps: Sys$Preview$UI$Effects$AnimationManager$set_fps,
    addAnimation: Sys$Preview$UI$Effects$AnimationManager$addAnimation,
    dispose: Sys$Preview$UI$Effects$AnimationManager$dispose,
    removeAnimation: Sys$Preview$UI$Effects$AnimationManager$removeAnimation,
    _onTickHandler: Sys$Preview$UI$Effects$AnimationManager$_onTickHandler
}
Sys.Preview.UI.Effects.AnimationManager.descriptor = {
    properties: [ {name: 'fps', type: Number} ]
}
Sys.Preview.UI.Effects.AnimationManager.registerClass('Sys.Preview.UI.Effects.AnimationManager', null, Sys.IDisposable);
Sys.Preview.UI.Effects.AnimationManager.instance = new Sys.Preview.UI.Effects.AnimationManager();
Sys.Preview.UI.Effects.AnimationManager.parseFromMarkup = function Sys$Preview$UI$Effects$AnimationManager$parseFromMarkup(type, node, markupContext) {
    if (!markupContext.get_isGlobal()) {
        return null;
    }
    Sys.Preview.MarkupParser.initializeObject(Sys.Preview.UI.Effects.AnimationManager.instance, node, markupContext);
    return Sys.Preview.UI.Effects.AnimationManager.instance;
}
Sys.Preview.UI.Effects.PropertyAnimation = function Sys$Preview$UI$Effects$PropertyAnimation() {
    Sys.Preview.UI.Effects.PropertyAnimation.initializeBase(this);
}
    function Sys$Preview$UI$Effects$PropertyAnimation$get_property() {
        /// <value type="String" mayBeNull="true"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._property;
    }
    function Sys$Preview$UI$Effects$PropertyAnimation$set_property(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: String, mayBeNull: true}]);
        if (e) throw e;
        this._property = value;
    }
    function Sys$Preview$UI$Effects$PropertyAnimation$get_propertyKey() {
        /// <value mayBeNull="true"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._propertyKey;
    }
    function Sys$Preview$UI$Effects$PropertyAnimation$set_propertyKey(value) {
        var e = Function._validateParams(arguments, [{name: "value", mayBeNull: true}]);
        if (e) throw e;
        this._propertyKey = value;
    }
    function Sys$Preview$UI$Effects$PropertyAnimation$add_ended(handler) {
        var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
        if (e) throw e;
        this.get_events().addHandler("ended", handler);
    }
    function Sys$Preview$UI$Effects$PropertyAnimation$remove_ended(handler) {
        var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
        if (e) throw e;
        this.get_events().removeHandler("ended", handler);
    }
    function Sys$Preview$UI$Effects$PropertyAnimation$add_started(handler) {
        var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
        if (e) throw e;
        this.get_events().addHandler("started", handler);
    }
    function Sys$Preview$UI$Effects$PropertyAnimation$remove_started(handler) {
        var e = Function._validateParams(arguments, [{name: "handler", type: Function}]);
        if (e) throw e;
        this.get_events().removeHandler("started", handler);
    }
    function Sys$Preview$UI$Effects$PropertyAnimation$setValue(value) {
        /// <param name="value"></param>
        var e = Function._validateParams(arguments, [
            {name: "value"}
        ]);
        if (e) throw e;
        Sys.Preview.TypeDescriptor.setProperty(this.get_target(), this._property, value, this._propertyKey);
    }
Sys.Preview.UI.Effects.PropertyAnimation.prototype = {    
    _property: null,
    _propertyKey: null,
    
    get_property: Sys$Preview$UI$Effects$PropertyAnimation$get_property,
    set_property: Sys$Preview$UI$Effects$PropertyAnimation$set_property,
    
    get_propertyKey: Sys$Preview$UI$Effects$PropertyAnimation$get_propertyKey,
    set_propertyKey: Sys$Preview$UI$Effects$PropertyAnimation$set_propertyKey,
    add_ended: Sys$Preview$UI$Effects$PropertyAnimation$add_ended,
    remove_ended: Sys$Preview$UI$Effects$PropertyAnimation$remove_ended,
    add_started: Sys$Preview$UI$Effects$PropertyAnimation$add_started,
    remove_started: Sys$Preview$UI$Effects$PropertyAnimation$remove_started,
    setValue: Sys$Preview$UI$Effects$PropertyAnimation$setValue
}
Sys.Preview.UI.Effects.PropertyAnimation.registerClass('Sys.Preview.UI.Effects.PropertyAnimation', Sys.Preview.UI.Effects.Animation);
Sys.Preview.UI.Effects.PropertyAnimation.descriptor = {
    properties: [   {name: 'property', type: String},
                    {name: 'propertyKey' } ]
}
Sys.Preview.UI.Effects.InterpolatedAnimation = function Sys$Preview$UI$Effects$InterpolatedAnimation() {
    Sys.Preview.UI.Effects.InterpolatedAnimation.initializeBase(this);
}
    function Sys$Preview$UI$Effects$InterpolatedAnimation$get_endValue() {
        /// <value></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._endValue;
    }
    function Sys$Preview$UI$Effects$InterpolatedAnimation$set_endValue(value) {
        var e = Function._validateParams(arguments, [{name: "value"}]);
        if (e) throw e;
        this._endValue = value;
    }
    function Sys$Preview$UI$Effects$InterpolatedAnimation$get_startValue() {
        /// <value></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._startValue;
    }
    function Sys$Preview$UI$Effects$InterpolatedAnimation$set_startValue(value) {
        var e = Function._validateParams(arguments, [{name: "value"}]);
        if (e) throw e;
        this._startValue = value;
    }
Sys.Preview.UI.Effects.InterpolatedAnimation.prototype = {
    _startValue: null,
    _endValue: null,
    
    get_endValue: Sys$Preview$UI$Effects$InterpolatedAnimation$get_endValue,
    set_endValue: Sys$Preview$UI$Effects$InterpolatedAnimation$set_endValue,
    
    get_startValue: Sys$Preview$UI$Effects$InterpolatedAnimation$get_startValue,
    set_startValue: Sys$Preview$UI$Effects$InterpolatedAnimation$set_startValue
}
Sys.Preview.UI.Effects.InterpolatedAnimation.registerClass('Sys.Preview.UI.Effects.InterpolatedAnimation', Sys.Preview.UI.Effects.PropertyAnimation);
Sys.Preview.UI.Effects.InterpolatedAnimation.descriptor = {
    properties: [   {name: 'endValue', type: Object},
                    {name: 'startValue', type: Object} ]
}
Sys.Preview.UI.Effects.DiscreteAnimation = function Sys$Preview$UI$Effects$DiscreteAnimation() {
    Sys.Preview.UI.Effects.DiscreteAnimation.initializeBase(this);
    this._values = [];
}
    function Sys$Preview$UI$Effects$DiscreteAnimation$get_values() {
        /// <value type="Array"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._values;
    }
    function Sys$Preview$UI$Effects$DiscreteAnimation$set_values(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: Array}]);
        if (e) throw e;
        this._values = value;
    }
    function Sys$Preview$UI$Effects$DiscreteAnimation$getAnimatedValue(percentage) {
        /// <param name="percentage" type="Number"></param>
        /// <returns></returns>
        var e = Function._validateParams(arguments, [
            {name: "percentage", type: Number}
        ]);
        if (e) throw e;
        var index = Math.round((percentage / 100) * (this._values.length - 1));
        return this._values[index];
    }
Sys.Preview.UI.Effects.DiscreteAnimation.prototype = {
    
    get_values: Sys$Preview$UI$Effects$DiscreteAnimation$get_values,
    set_values: Sys$Preview$UI$Effects$DiscreteAnimation$set_values,
    getAnimatedValue: Sys$Preview$UI$Effects$DiscreteAnimation$getAnimatedValue
}
Sys.Preview.UI.Effects.DiscreteAnimation.registerClass('Sys.Preview.UI.Effects.DiscreteAnimation', Sys.Preview.UI.Effects.PropertyAnimation);
Sys.Preview.UI.Effects.DiscreteAnimation.descriptor = {
    properties: [ {name: 'values', type: Array }]
}
Sys.Preview.UI.Effects.NumberAnimation = function Sys$Preview$UI$Effects$NumberAnimation() {
    Sys.Preview.UI.Effects.NumberAnimation.initializeBase(this);
}
    function Sys$Preview$UI$Effects$NumberAnimation$get_integralValues() {
        /// <value type="Boolean"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._integralValues;
    }
    function Sys$Preview$UI$Effects$NumberAnimation$set_integralValues(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: Boolean}]);
        if (e) throw e;
        this._integralValues = value;
    }
    function Sys$Preview$UI$Effects$NumberAnimation$getAnimatedValue(percentage) {
        /// <param name="percentage" type="Number"></param>
        /// <returns type="Number"></returns>
        var e = Function._validateParams(arguments, [
            {name: "percentage", type: Number}
        ]);
        if (e) throw e;
        var value = Sys.Preview.UI.Effects.Glitz.interpolate(this.get_startValue(),
                                                    this.get_endValue(),
                                                    percentage);
        if (this._integralValues) {
            value = Math.round(value);
        }
        return value;
    }
Sys.Preview.UI.Effects.NumberAnimation.prototype = {
    _integralValues: false,
    
    get_integralValues: Sys$Preview$UI$Effects$NumberAnimation$get_integralValues,
    set_integralValues: Sys$Preview$UI$Effects$NumberAnimation$set_integralValues,
    getAnimatedValue: Sys$Preview$UI$Effects$NumberAnimation$getAnimatedValue
}
Sys.Preview.UI.Effects.NumberAnimation.registerClass('Sys.Preview.UI.Effects.NumberAnimation', Sys.Preview.UI.Effects.InterpolatedAnimation);
Sys.Preview.UI.Effects.NumberAnimation.descriptor = {
    properties: [   {name: 'startValue', type: Number},
                    {name: 'endValue', type: Number},
                    {name: 'integralValues', type: Boolean} ]
}
Sys.Preview.UI.Effects.LengthAnimation = function Sys$Preview$UI$Effects$LengthAnimation() {
    Sys.Preview.UI.Effects.LengthAnimation.initializeBase(this);
}
    function Sys$Preview$UI$Effects$LengthAnimation$get_unit() {
        /// <value type="String"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._unit;
    }
    function Sys$Preview$UI$Effects$LengthAnimation$set_unit(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: String}]);
        if (e) throw e;
        this._unit = value;
    }
    function Sys$Preview$UI$Effects$LengthAnimation$getAnimatedValue(percentage) {
        /// <param name="percentage" type="Number"></param>
        /// <returns type="Number"></returns>
        var e = Function._validateParams(arguments, [
            {name: "percentage", type: Number}
        ]);
        if (e) throw e;
        var value = Sys.Preview.UI.Effects.Glitz.interpolate(this.get_startValue(),
                                                    this.get_endValue(),
                                                    percentage);
        return Math.round(value) + this._unit;
    }
Sys.Preview.UI.Effects.LengthAnimation.prototype = {
    _unit: 'px',
    
    get_unit: Sys$Preview$UI$Effects$LengthAnimation$get_unit,
    set_unit: Sys$Preview$UI$Effects$LengthAnimation$set_unit,
    getAnimatedValue: Sys$Preview$UI$Effects$LengthAnimation$getAnimatedValue
}
Sys.Preview.UI.Effects.LengthAnimation.registerClass('Sys.Preview.UI.Effects.LengthAnimation', Sys.Preview.UI.Effects.InterpolatedAnimation);
Sys.Preview.UI.Effects.LengthAnimation.descriptor = {
    properties: [   {name: 'startValue', type: Number},
                    {name: 'endValue', type: Number},
                    {name: 'unit', type: String} ]
}
Sys.Preview.UI.Effects.CompositeAnimation = function Sys$Preview$UI$Effects$CompositeAnimation() {
    Sys.Preview.UI.Effects.CompositeAnimation.initializeBase(this);
    this._animations = Sys.Component.createCollection(this);
}
    function Sys$Preview$UI$Effects$CompositeAnimation$get_animations() {
        /// <value type="Array"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._animations;
    }
    function Sys$Preview$UI$Effects$CompositeAnimation$getAnimatedValue(percentage) {
                throw Error.invalidOperation();
    }
    function Sys$Preview$UI$Effects$CompositeAnimation$dispose() {
        this._animations.dispose();
        this._animations = null;
        
        Sys.Preview.UI.Effects.CompositeAnimation.callBaseMethod(this, 'dispose');
    }
    function Sys$Preview$UI$Effects$CompositeAnimation$onEnd() {
        for (var i = 0; i < this._animations.length; i++) {
            this._animations[i].onEnd();
        }
    }
    function Sys$Preview$UI$Effects$CompositeAnimation$onStart() {
        for (var i = 0; i < this._animations.length; i++) {
            this._animations[i].onStart();
        }
    }
    function Sys$Preview$UI$Effects$CompositeAnimation$onStep(percentage) {
        /// <param name="percentage" type="Number"></param>
        var e = Function._validateParams(arguments, [
            {name: "percentage", type: Number}
        ]);
        if (e) throw e;
        for (var i = 0; i < this._animations.length; i++) {
            this._animations[i].onStep(percentage);
        }
    }
Sys.Preview.UI.Effects.CompositeAnimation.prototype = {
    get_animations: Sys$Preview$UI$Effects$CompositeAnimation$get_animations,
    getAnimatedValue: Sys$Preview$UI$Effects$CompositeAnimation$getAnimatedValue,
    
    dispose: Sys$Preview$UI$Effects$CompositeAnimation$dispose,
    onEnd: Sys$Preview$UI$Effects$CompositeAnimation$onEnd,
    
    onStart: Sys$Preview$UI$Effects$CompositeAnimation$onStart,
    
    onStep: Sys$Preview$UI$Effects$CompositeAnimation$onStep
}
Sys.Preview.UI.Effects.CompositeAnimation.registerClass('Sys.Preview.UI.Effects.CompositeAnimation', Sys.Preview.UI.Effects.Animation);
Sys.Preview.UI.Effects.CompositeAnimation.descriptor = {
    properties: [ {name: 'animations', type: Array, readOnly: true} ]
}
Sys.Preview.UI.Effects.FadeEffect = function Sys$Preview$UI$Effects$FadeEffect() {
    throw Error.invalidOperation();
}
Sys.Preview.UI.Effects.FadeEffect.prototype = {
    FadeIn: 0,
    FadeOut: 1
}
Sys.Preview.UI.Effects.FadeEffect.registerEnum('Sys.Preview.UI.Effects.FadeEffect');
Sys.Preview.UI.Effects.FadeAnimation = function Sys$Preview$UI$Effects$FadeAnimation() {
    Sys.Preview.UI.Effects.FadeAnimation.initializeBase(this);
}
    function Sys$Preview$UI$Effects$FadeAnimation$get_effect() {
        /// <value type="Sys.Preview.UI.Effects.FadeEffect"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._effect;
    }
    function Sys$Preview$UI$Effects$FadeAnimation$set_effect(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: Sys.Preview.UI.Effects.FadeEffect}]);
        if (e) throw e;
        this._effect = value;
    }
    function Sys$Preview$UI$Effects$FadeAnimation$getAnimatedValue(percentage) {
        /// <param name="percentage" type="Number"></param>
        /// <returns type="Number"></returns>
        var e = Function._validateParams(arguments, [
            {name: "percentage", type: Number}
        ]);
        if (e) throw e;
        var startValue = 0;
        var endValue = 1;
        if (this._effect === Sys.Preview.UI.Effects.FadeEffect.FadeOut) {
            startValue = 1;
            endValue = 0;
        }
        return Sys.Preview.UI.Effects.Glitz.interpolate(startValue, endValue, percentage);
    }
    function Sys$Preview$UI$Effects$FadeAnimation$onStart() {
        var opacity = 0;
        if (this._effect === Sys.Preview.UI.Effects.FadeEffect.FadeOut) {
            opacity = 1;
        }
        
        this.setValue(opacity);
    }
    function Sys$Preview$UI$Effects$FadeAnimation$onEnd() {
        var opacity = 1;
        if (this._effect === Sys.Preview.UI.Effects.FadeEffect.FadeOut) {
            opacity = 0;
        }
        
        this.setValue(opacity);
    }
    function Sys$Preview$UI$Effects$FadeAnimation$setValue(value) {
        /// <param name="value" type="Number"></param>
        var e = Function._validateParams(arguments, [
            {name: "value", type: Number}
        ]);
        if (e) throw e;
        Sys.Preview.UI.Effects.Glitz.setElementOpacity(this.get_target().get_element(), value);
    }
Sys.Preview.UI.Effects.FadeAnimation.prototype = {
    _effect: Sys.Preview.UI.Effects.FadeEffect.FadeIn,
    
    get_effect: Sys$Preview$UI$Effects$FadeAnimation$get_effect,
    set_effect: Sys$Preview$UI$Effects$FadeAnimation$set_effect,
    getAnimatedValue: Sys$Preview$UI$Effects$FadeAnimation$getAnimatedValue,
    
    onStart: Sys$Preview$UI$Effects$FadeAnimation$onStart,
    
    onEnd: Sys$Preview$UI$Effects$FadeAnimation$onEnd,
    
    setValue: Sys$Preview$UI$Effects$FadeAnimation$setValue
}
Sys.Preview.UI.Effects.FadeAnimation.registerClass('Sys.Preview.UI.Effects.FadeAnimation', Sys.Preview.UI.Effects.Animation);
Sys.Preview.UI.Effects.FadeAnimation.descriptor = {
    properties: [ {name: 'effect', type: Sys.Preview.UI.Effects.FadeEffect} ]
}
Sys.Preview.UI.Effects.LayoutBehavior = function Sys$Preview$UI$Effects$LayoutBehavior(element) {
    /// <param name="element" domElement="true"></param>
    var e = Function._validateParams(arguments, [
        {name: "element", domElement: true}
    ]);
    if (e) throw e;
    Sys.Preview.UI.Effects.LayoutBehavior.initializeBase(this,[element]);
}
    function Sys$Preview$UI$Effects$LayoutBehavior$get_height() {
        /// <value type="String" mayBeNull="true"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._height;
    }
    function Sys$Preview$UI$Effects$LayoutBehavior$set_height(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: String, mayBeNull: true}]);
        if (e) throw e;
        this._height = value;
        
        var element = this.get_element();
        if (element) {
            element.style.height = this._height;
        }
    }
    function Sys$Preview$UI$Effects$LayoutBehavior$get_left() {
        /// <value type="String" mayBeNull="true"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._left;
    }
    function Sys$Preview$UI$Effects$LayoutBehavior$set_left(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: String, mayBeNull: true}]);
        if (e) throw e;
        this._left = value;
        
        var element = this.get_element();
        if (element) {
            element.style.left = this._left;
        }
    }
    function Sys$Preview$UI$Effects$LayoutBehavior$get_top() {
        /// <value type="String" mayBeNull="true"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._top;
    }
    function Sys$Preview$UI$Effects$LayoutBehavior$set_top(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: String, mayBeNull: true}]);
        if (e) throw e;
        this._top = value;
        
        var element = this.get_element();
        if (element) {
            element.style.top = this._top;
        }
    }
    function Sys$Preview$UI$Effects$LayoutBehavior$get_width() {
        /// <value type="String" mayBeNull="true"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._width;
    }
    function Sys$Preview$UI$Effects$LayoutBehavior$set_width(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: String, mayBeNull: true}]);
        if (e) throw e;
        this._width = value;
        
        var element = this.get_element();
        if (element) {
            element.style.width = this._width;
        }
    }
    function Sys$Preview$UI$Effects$LayoutBehavior$initialize() {
        Sys.Preview.UI.Effects.LayoutBehavior.callBaseMethod(this, 'initialize');
        if (this._height) {
            this.set_height(this._height);
        }
        if (this._left) {
            this.set_left(this._left);
        }
        if (this._top) {
            this.set_top(this._top);
        }
        if (this._width) {
            this.set_width(this._width);
        }
    }
Sys.Preview.UI.Effects.LayoutBehavior.prototype = {
    _left: null,
    _top: null,
    _width: null,
    _height: null,
    
    get_height: Sys$Preview$UI$Effects$LayoutBehavior$get_height,
    set_height: Sys$Preview$UI$Effects$LayoutBehavior$set_height,
    
    get_left: Sys$Preview$UI$Effects$LayoutBehavior$get_left,
    set_left: Sys$Preview$UI$Effects$LayoutBehavior$set_left,
    
    get_top: Sys$Preview$UI$Effects$LayoutBehavior$get_top,
    set_top: Sys$Preview$UI$Effects$LayoutBehavior$set_top,
    
    get_width: Sys$Preview$UI$Effects$LayoutBehavior$get_width,
    set_width: Sys$Preview$UI$Effects$LayoutBehavior$set_width,
    
    initialize: Sys$Preview$UI$Effects$LayoutBehavior$initialize
}
Sys.Preview.UI.Effects.LayoutBehavior.registerClass('Sys.Preview.UI.Effects.LayoutBehavior', Sys.UI.Behavior);
Sys.Preview.UI.Effects.LayoutBehavior.descriptor = {
    properties: [   {name: 'height', type: String},
                    {name: 'left', type: String},
                    {name: 'top', type: String},
                    {name: 'width', type: String} ]
}


if(typeof(Sys)!=='undefined')Sys.Application.notifyScriptLoaded();
