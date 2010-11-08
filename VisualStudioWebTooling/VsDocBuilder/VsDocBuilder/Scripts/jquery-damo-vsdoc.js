/*
* This file has been generated to support Visual Studio IntelliSense.
* You should not use this file at runtime inside the browser--it is only
* intended to be used only for design-time IntelliSense.  Please use the
* standard jQuery library for all production use.
*
* Comment version: 1.4.2
*/

/*!
* jQuery JavaScript Library v1.4.1
* http://jquery.com/
*
* Distributed in whole under the terms of the MIT
*
* Copyright 2010, John Resig
*
* Permission is hereby granted, free of charge, to any person obtaining
* a copy of this software and associated documentation files (the
* "Software"), to deal in the Software without restriction, including
* without limitation the rights to use, copy, modify, merge, publish,
* distribute, sublicense, and/or sell copies of the Software, and to
* permit persons to whom the Software is furnished to do so, subject to
* the following conditions:
*
* The above copyright notice and this permission notice shall be
* included in all copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
* EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
* MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
* NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
* LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
* OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
* WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*
* Includes Sizzle.js
* http://sizzlejs.com/
* Copyright 2010, The Dojo Foundation
* Released under the MIT, BSD, and GPL Licenses.
*
* Date: Mon Jan 25 19:43:33 2010 -0500
*/

(function ( window, undefined ) {
var jQuery = function( selector, context ) {
/// <summary>
///     1: Accepts a string containing a CSS selector which is then used to match a set of elements.
///         1.1 - $(selector, context) 
///         1.2 - $(element) 
///         1.3 - $(elementArray) 
///         1.4 - $(jQuery object) 
///         1.5 - $()
///     2: Creates DOM elements on the fly from the provided string of raw HTML.
///         2.1 - $(html, ownerDocument) 
///         2.2 - $(html, props)
///     3: Binds a function to be executed when the DOM has finished loading.
///         3.1 - $(callback)
/// </summary>
/// <returns type="jQuery" />
/// <param name="selector" type="String">
///     A string containing a selector expression
/// </param>
/// <param name="context" type="jQuery">
///     A DOM Element, Document, or jQuery to use as context
/// </param>

		// The jQuery object is actually just the init constructor 'enhanced'
		return new jQuery.fn.init( selector, context );
	};
function access (elems, key, value, exec, fn, pass) {
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
        };
jQuery.Event = function( src ) {

	// Allow instantiation without the 'new' keyword
	if ( !this.preventDefault ) {
		return new jQuery.Event( src );
	}

	// Event object
	if ( src && src.type ) {
		this.originalEvent = src;
		this.type = src.type;
	// Event type
	} else {
		this.type = src;
	}

	// timeStamp is buggy for some events on Firefox(#3843)
	// So we won't rely on the native value
	this.timeStamp = now();

	// Mark it as fixed
	this[ expando ] = true;
};
jQuery.active = SrliZe = 0;;
jQuery.ajax = function( origSettings ) {
/// <summary>
///     Perform an asynchronous HTTP (Ajax) request.
///     
/// </summary>
/// <returns type="XMLHttpRequest" />
/// <param name="origSettings" type="Object">
///     
///                 A set of key/value pairs that configure the Ajax request. All options are optional. A default can be set for any option with $.ajaxSetup().
///               
/// </param>

		var s = jQuery.extend(true, {}, jQuery.ajaxSettings, origSettings);
		
		var jsonp, status, data,
			callbackContext = origSettings && origSettings.context || s,
			type = s.type.toUpperCase();

		// convert data if not already a string
		if ( s.data && s.processData && typeof s.data !== "string" ) {
			s.data = jQuery.param( s.data, s.traditional );
		}

		// Handle JSONP Parameter Callbacks
		if ( s.dataType === "jsonp" ) {
			if ( type === "GET" ) {
				if ( !jsre.test( s.url ) ) {
					s.url += (rquery.test( s.url ) ? "&" : "?") + (s.jsonp || "callback") + "=?";
				}
			} else if ( !s.data || !jsre.test(s.data) ) {
				s.data = (s.data ? s.data + "&" : "") + (s.jsonp || "callback") + "=?";
			}
			s.dataType = "json";
		}

		// Build temporary JSONP function
		if ( s.dataType === "json" && (s.data && jsre.test(s.data) || jsre.test(s.url)) ) {
			jsonp = s.jsonpCallback || ("jsonp" + jsc++);

			// Replace the =? sequence both in the query string and the data
			if ( s.data ) {
				s.data = (s.data + "").replace(jsre, "=" + jsonp + "$1");
			}

			s.url = s.url.replace(jsre, "=" + jsonp + "$1");

			// We need to make sure
			// that a JSONP style response is executed properly
			s.dataType = "script";

			// Handle JSONP-style loading
			window[ jsonp ] = window[ jsonp ] || function( tmp ) {
				data = tmp;
				success();
				complete();
				// Garbage collect
				window[ jsonp ] = undefined;

				try {
					delete window[ jsonp ];
				} catch(e) {}

				if ( head ) {
					head.removeChild( script );
				}
			};
		}

		if ( s.dataType === "script" && s.cache === null ) {
			s.cache = false;
		}

		if ( s.cache === false && type === "GET" ) {
			var ts = now();

			// try replacing _= if it is there
			var ret = s.url.replace(rts, "$1_=" + ts + "$2");

			// if nothing was replaced, add timestamp to the end
			s.url = ret + ((ret === s.url) ? (rquery.test(s.url) ? "&" : "?") + "_=" + ts : "");
		}

		// If data is available, append data to url for get requests
		if ( s.data && type === "GET" ) {
			s.url += (rquery.test(s.url) ? "&" : "?") + s.data;
		}

		// Watch for a new set of requests
		if ( s.global && ! jQuery.active++ ) {
			jQuery.event.trigger( "ajaxStart" );
		}

		// Matches an absolute URL, and saves the domain
		var parts = rurl.exec( s.url ),
			remote = parts && (parts[1] && parts[1] !== location.protocol || parts[2] !== location.host);

		// If we're requesting a remote document
		// and trying to load JSON or Script with a GET
		if ( s.dataType === "script" && type === "GET" && remote ) {
			var head = document.getElementsByTagName("head")[0] || document.documentElement;
			var script = document.createElement("script");
			script.src = s.url;
			if ( s.scriptCharset ) {
				script.charset = s.scriptCharset;
			}

			// Handle Script loading
			if ( !jsonp ) {
				var done = false;

				// Attach handlers for all browsers
				script.onload = script.onreadystatechange = function() {
					if ( !done && (!this.readyState ||
							this.readyState === "loaded" || this.readyState === "complete") ) {
						done = true;
						success();
						complete();

						// Handle memory leak in IE
						script.onload = script.onreadystatechange = null;
						if ( head && script.parentNode ) {
							head.removeChild( script );
						}
					}
				};
			}

			// Use insertBefore instead of appendChild  to circumvent an IE6 bug.
			// This arises when a base node is used (#2709 and #4378).
			head.insertBefore( script, head.firstChild );

			// We handle everything using the script element injection
			return undefined;
		}

		var requestDone = false;

		// Create the request object
		var xhr = s.xhr();

		if ( !xhr ) {
			return;
		}

		// Open the socket
		// Passing null username, generates a login popup on Opera (#2865)
		if ( s.username ) {
			xhr.open(type, s.url, s.async, s.username, s.password);
		} else {
			xhr.open(type, s.url, s.async);
		}

		// Need an extra try/catch for cross domain requests in Firefox 3
		try {
			// Set the correct header, if data is being sent
			if ( s.data || origSettings && origSettings.contentType ) {
				xhr.setRequestHeader("Content-Type", s.contentType);
			}

			// Set the If-Modified-Since and/or If-None-Match header, if in ifModified mode.
			if ( s.ifModified ) {
				if ( jQuery.lastModified[s.url] ) {
					xhr.setRequestHeader("If-Modified-Since", jQuery.lastModified[s.url]);
				}

				if ( jQuery.etag[s.url] ) {
					xhr.setRequestHeader("If-None-Match", jQuery.etag[s.url]);
				}
			}

			// Set header so the called script knows that it's an XMLHttpRequest
			// Only send the header if it's not a remote XHR
			if ( !remote ) {
				xhr.setRequestHeader("X-Requested-With", "XMLHttpRequest");
			}

			// Set the Accepts header for the server, depending on the dataType
			xhr.setRequestHeader("Accept", s.dataType && s.accepts[ s.dataType ] ?
				s.accepts[ s.dataType ] + ", */*" :
				s.accepts._default );
		} catch(e) {}

		// Allow custom headers/mimetypes and early abort
		if ( s.beforeSend && s.beforeSend.call(callbackContext, xhr, s) === false ) {
			// Handle the global AJAX counter
			if ( s.global && ! --jQuery.active ) {
				jQuery.event.trigger( "ajaxStop" );
			}

			// close opended socket
			xhr.abort();
			return false;
		}

		if ( s.global ) {
			trigger("ajaxSend", [xhr, s]);
		}

		// Wait for a response to come back
		var onreadystatechange = xhr.onreadystatechange = function( isTimeout ) {
			// The request was aborted
			if ( !xhr || xhr.readyState === 0 || isTimeout === "abort" ) {
				// Opera doesn't call onreadystatechange before this point
				// so we simulate the call
				if ( !requestDone ) {
					complete();
				}

				requestDone = true;
				if ( xhr ) {
					xhr.onreadystatechange = jQuery.noop;
				}

			// The transfer is complete and the data is available, or the request timed out
			} else if ( !requestDone && xhr && (xhr.readyState === 4 || isTimeout === "timeout") ) {
				requestDone = true;
				xhr.onreadystatechange = jQuery.noop;

				status = isTimeout === "timeout" ?
					"timeout" :
					!jQuery.httpSuccess( xhr ) ?
						"error" :
						s.ifModified && jQuery.httpNotModified( xhr, s.url ) ?
							"notmodified" :
							"success";

				var errMsg;

				if ( status === "success" ) {
					// Watch for, and catch, XML document parse errors
					try {
						// process the data (runs the xml through httpData regardless of callback)
						data = jQuery.httpData( xhr, s.dataType, s );
					} catch(err) {
						status = "parsererror";
						errMsg = err;
					}
				}

				// Make sure that the request was successful or notmodified
				if ( status === "success" || status === "notmodified" ) {
					// JSONP handles its own success callback
					if ( !jsonp ) {
						success();
					}
				} else {
					jQuery.handleError(s, xhr, status, errMsg);
				}

				// Fire the complete handlers
				complete();

				if ( isTimeout === "timeout" ) {
					xhr.abort();
				}

				// Stop memory leaks
				if ( s.async ) {
					xhr = null;
				}
			}
		};

		// Override the abort handler, if we can (IE doesn't allow it, but that's OK)
		// Opera doesn't fire onreadystatechange at all on abort
		try {
			var oldAbort = xhr.abort;
			xhr.abort = function() {
				if ( xhr ) {
					oldAbort.call( xhr );
				}

				onreadystatechange( "abort" );
			};
		} catch(e) { }

		// Timeout checker
		if ( s.async && s.timeout > 0 ) {
			setTimeout(function() {
				// Check to see if the request is still happening
				if ( xhr && !requestDone ) {
					onreadystatechange( "timeout" );
				}
			}, s.timeout);
		}

		// Send the data
		try {
			xhr.send( type === "POST" || type === "PUT" || type === "DELETE" ? s.data : null );
		} catch(e) {
			jQuery.handleError(s, xhr, null, e);
			// Fire the complete handlers
			complete();
		}

		// firefox 1.5 doesn't fire statechange for sync requests
		if ( !s.async ) {
			onreadystatechange();
		}

		function success() {
			// If a local callback was specified, fire it and pass it the data
			if ( s.success ) {
				s.success.call( callbackContext, data, status, xhr );
			}

			// Fire the global callback
			if ( s.global ) {
				trigger( "ajaxSuccess", [xhr, s] );
			}
		}

		function complete() {
			// Process result
			if ( s.complete ) {
				s.complete.call( callbackContext, xhr, status);
			}

			// The request was completed
			if ( s.global ) {
				trigger( "ajaxComplete", [xhr, s] );
			}

			// Handle the global AJAX counter
			if ( s.global && ! --jQuery.active ) {
				jQuery.event.trigger( "ajaxStop" );
			}
		}
		
		function trigger(type, args) {
			(s.context ? jQuery(s.context) : jQuery.event).trigger(type, args);
		}

		// return XMLHttpRequest to allow aborting the request etc.
		return xhr;
	};
jQuery.ajaxSettings = SrliZe = new Object;SrliZe.url = 'http://localhost:25812/';SrliZe.global = true;SrliZe.type = 'GET';SrliZe.contentType = 'application/x-www-form-urlencoded';SrliZe.processData = true;SrliZe.async = true;SrliZe.xhr = function() {
				return new window.XMLHttpRequest();
			};SrliZe.accepts = new Object;SrliZe.accepts.xml = 'application/xml, text/xml';SrliZe.accepts.html = 'text/html';SrliZe.accepts.script = 'text/javascript, application/javascript';SrliZe.accepts.json = 'application/json, text/javascript';SrliZe.accepts.text = 'text/plain';SrliZe.accepts._default = '*/*';;
jQuery.ajaxSetup = function( settings ) {
/// <summary>
///     Set default values for future Ajax requests.
///     
/// </summary>/// <param name="settings" type="Object">
///     A set of key/value pairs that configure the default Ajax request. All options are optional. 
/// </param>

		jQuery.extend( jQuery.ajaxSettings, settings );
	};
jQuery.attr = function( elem, name, value, pass ) {

		// don't set attributes on text and comment nodes
		if ( !elem || elem.nodeType === 3 || elem.nodeType === 8 ) {
			return undefined;
		}

		if ( pass && name in jQuery.attrFn ) {
			return jQuery(elem)[name](value);
		}

		var notxml = elem.nodeType !== 1 || !jQuery.isXMLDoc( elem ),
			// Whether we are setting (or getting)
			set = value !== undefined;

		// Try to normalize/fix the name
		name = notxml && jQuery.props[ name ] || name;

		// Only do all the following if this is a node (faster for style)
		if ( elem.nodeType === 1 ) {
			// These attributes require special treatment
			var special = rspecialurl.test( name );

			// Safari mis-reports the default selected property of an option
			// Accessing the parent's selectedIndex property fixes it
			if ( name === "selected" && !jQuery.support.optSelected ) {
				var parent = elem.parentNode;
				if ( parent ) {
					parent.selectedIndex;
	
					// Make sure that it also works with optgroups, see #5701
					if ( parent.parentNode ) {
						parent.parentNode.selectedIndex;
					}
				}
			}

			// If applicable, access the attribute via the DOM 0 way
			if ( name in elem && notxml && !special ) {
				if ( set ) {
					// We can't allow the type property to be changed (since it causes problems in IE)
					if ( name === "type" && rtype.test( elem.nodeName ) && elem.parentNode ) {
						jQuery.error( "type property can't be changed" );
					}

					elem[ name ] = value;
				}

				// browsers index elements by id/name on forms, give priority to attributes.
				if ( jQuery.nodeName( elem, "form" ) && elem.getAttributeNode(name) ) {
					return elem.getAttributeNode( name ).nodeValue;
				}

				// elem.tabIndex doesn't always return the correct value when it hasn't been explicitly set
				// http://fluidproject.org/blog/2008/01/09/getting-setting-and-removing-tabindex-values-with-javascript/
				if ( name === "tabIndex" ) {
					var attributeNode = elem.getAttributeNode( "tabIndex" );

					return attributeNode && attributeNode.specified ?
						attributeNode.value :
						rfocusable.test( elem.nodeName ) || rclickable.test( elem.nodeName ) && elem.href ?
							0 :
							undefined;
				}

				return elem[ name ];
			}

			if ( !jQuery.support.style && notxml && name === "style" ) {
				if ( set ) {
					elem.style.cssText = "" + value;
				}

				return elem.style.cssText;
			}

			if ( set ) {
				// convert the value to a string (all browsers do this but IE) see #1070
				elem.setAttribute( name, "" + value );
			}

			var attr = !jQuery.support.hrefNormalized && notxml && special ?
					// Some attributes require a special call on IE
					elem.getAttribute( name, 2 ) :
					elem.getAttribute( name );

			// Non-existent attributes return null, we normalize to undefined
			return attr === null ? undefined : attr;
		}

		// elem is actually elem.style ... set the style
		// Using attr for specific style information is now deprecated. Use style instead.
		return jQuery.style( elem, name, value );
	};
jQuery.attrFn = SrliZe = new Object;SrliZe.val = true;SrliZe.css = true;SrliZe.html = true;SrliZe.text = true;SrliZe.data = true;SrliZe.width = true;SrliZe.height = true;SrliZe.offset = true;SrliZe.blur = true;SrliZe.focus = true;SrliZe.focusin = true;SrliZe.focusout = true;SrliZe.load = true;SrliZe.resize = true;SrliZe.scroll = true;SrliZe.unload = true;SrliZe.click = true;SrliZe.dblclick = true;SrliZe.mousedown = true;SrliZe.mouseup = true;SrliZe.mousemove = true;SrliZe.mouseover = true;SrliZe.mouseout = true;SrliZe.mouseenter = true;SrliZe.mouseleave = true;SrliZe.change = true;SrliZe.select = true;SrliZe.submit = true;SrliZe.keydown = true;SrliZe.keypress = true;SrliZe.keyup = true;SrliZe.error = true;;
jQuery.bindReady = function() {

		if ( readyBound ) {
			return;
		}

		readyBound = true;

		// Catch cases where $(document).ready() is called after the
		// browser event has already occurred.
		if ( document.readyState === "complete" ) {
			return jQuery.ready();
		}

		// Mozilla, Opera and webkit nightlies currently support this event
		if ( document.addEventListener ) {
			// Use the handy event callback
			document.addEventListener( "DOMContentLoaded", DOMContentLoaded, false );
			
			// A fallback to window.onload, that will always work
			window.addEventListener( "load", jQuery.ready, false );

		// If IE event model is used
		} else if ( document.attachEvent ) {
			// ensure firing before onload,
			// maybe late but safe also for iframes
			document.attachEvent("onreadystatechange", DOMContentLoaded);
			
			// A fallback to window.onload, that will always work
			window.attachEvent( "onload", jQuery.ready );

			// If IE and not a frame
			// continually check to see if the document is ready
			var toplevel = false;

			try {
				toplevel = window.frameElement == null;
			} catch(e) {}

			if ( document.documentElement.doScroll && toplevel ) {
				doScrollCheck();
			}
		}
	};
jQuery.boxModel = SrliZe = true;;
jQuery.browser = SrliZe = new Object;SrliZe.msie = true;SrliZe.version = '8.0';;
jQuery.cache = SrliZe = new Object;SrliZe[1] = new Object;SrliZe[1].m = new Object;SrliZe[1].m.name = 'jQuery';SrliZe[1].m.aliases = '$';SrliZe[1].m.ref = function( selector, context ) {
		// The jQuery object is actually just the init constructor 'enhanced'
		return new jQuery.fn.init( selector, context );
	};SrliZe[1].m.doc = '/// <summary>\r\n///     1: Accepts a string containing a CSS selector which is then used to match a set of elements.\r\n///         1.1 - $(selector, context) \r\n///         1.2 - $(element) \r\n///         1.3 - $(elementArray) \r\n///         1.4 - $(jQuery object) \r\n///         1.5 - $()\r\n///     2: Creates DOM elements on the fly from the provided string of raw HTML.\r\n///         2.1 - $(html, ownerDocument) \r\n///         2.2 - $(html, props)\r\n///     3: Binds a function to be executed when the DOM has finished loading.\r\n///         3.1 - $(callback)\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"selector\" type=\"String\">\r\n///     A string containing a selector expression\r\n/// </param>\r\n/// <param name=\"context\" type=\"jQuery\">\r\n///     A DOM Element, Document, or jQuery to use as context\r\n/// </param>\r\n';SrliZe[2] = new Object;SrliZe[2].m = new Object;SrliZe[2].m.name = 'jQuery.Event';SrliZe[2].m.aliases = '';SrliZe[2].m.ref = function( src ) {
	// Allow instantiation without the 'new' keyword
	if ( !this.preventDefault ) {
		return new jQuery.Event( src );
	}

	// Event object
	if ( src && src.type ) {
		this.originalEvent = src;
		this.type = src.type;
	// Event type
	} else {
		this.type = src;
	}

	// timeStamp is buggy for some events on Firefox(#3843)
	// So we won't rely on the native value
	this.timeStamp = now();

	// Mark it as fixed
	this[ expando ] = true;
};SrliZe[2].m.doc = '';SrliZe[3] = new Object;SrliZe[3].m = new Object;SrliZe[3].m.name = 'jQuery.active';SrliZe[3].m.aliases = '';SrliZe[3].m.ref = 0;SrliZe[3].m.doc = '';SrliZe[4] = new Object;SrliZe[4].m = new Object;SrliZe[4].m.name = 'jQuery.ajax';SrliZe[4].m.aliases = '';SrliZe[4].m.ref = function( origSettings ) {
		var s = jQuery.extend(true, {}, jQuery.ajaxSettings, origSettings);
		
		var jsonp, status, data,
			callbackContext = origSettings && origSettings.context || s,
			type = s.type.toUpperCase();

		// convert data if not already a string
		if ( s.data && s.processData && typeof s.data !== "string" ) {
			s.data = jQuery.param( s.data, s.traditional );
		}

		// Handle JSONP Parameter Callbacks
		if ( s.dataType === "jsonp" ) {
			if ( type === "GET" ) {
				if ( !jsre.test( s.url ) ) {
					s.url += (rquery.test( s.url ) ? "&" : "?") + (s.jsonp || "callback") + "=?";
				}
			} else if ( !s.data || !jsre.test(s.data) ) {
				s.data = (s.data ? s.data + "&" : "") + (s.jsonp || "callback") + "=?";
			}
			s.dataType = "json";
		}

		// Build temporary JSONP function
		if ( s.dataType === "json" && (s.data && jsre.test(s.data) || jsre.test(s.url)) ) {
			jsonp = s.jsonpCallback || ("jsonp" + jsc++);

			// Replace the =? sequence both in the query string and the data
			if ( s.data ) {
				s.data = (s.data + "").replace(jsre, "=" + jsonp + "$1");
			}

			s.url = s.url.replace(jsre, "=" + jsonp + "$1");

			// We need to make sure
			// that a JSONP style response is executed properly
			s.dataType = "script";

			// Handle JSONP-style loading
			window[ jsonp ] = window[ jsonp ] || function( tmp ) {
				data = tmp;
				success();
				complete();
				// Garbage collect
				window[ jsonp ] = undefined;

				try {
					delete window[ jsonp ];
				} catch(e) {}

				if ( head ) {
					head.removeChild( script );
				}
			};
		}

		if ( s.dataType === "script" && s.cache === null ) {
			s.cache = false;
		}

		if ( s.cache === false && type === "GET" ) {
			var ts = now();

			// try replacing _= if it is there
			var ret = s.url.replace(rts, "$1_=" + ts + "$2");

			// if nothing was replaced, add timestamp to the end
			s.url = ret + ((ret === s.url) ? (rquery.test(s.url) ? "&" : "?") + "_=" + ts : "");
		}

		// If data is available, append data to url for get requests
		if ( s.data && type === "GET" ) {
			s.url += (rquery.test(s.url) ? "&" : "?") + s.data;
		}

		// Watch for a new set of requests
		if ( s.global && ! jQuery.active++ ) {
			jQuery.event.trigger( "ajaxStart" );
		}

		// Matches an absolute URL, and saves the domain
		var parts = rurl.exec( s.url ),
			remote = parts && (parts[1] && parts[1] !== location.protocol || parts[2] !== location.host);

		// If we're requesting a remote document
		// and trying to load JSON or Script with a GET
		if ( s.dataType === "script" && type === "GET" && remote ) {
			var head = document.getElementsByTagName("head")[0] || document.documentElement;
			var script = document.createElement("script");
			script.src = s.url;
			if ( s.scriptCharset ) {
				script.charset = s.scriptCharset;
			}

			// Handle Script loading
			if ( !jsonp ) {
				var done = false;

				// Attach handlers for all browsers
				script.onload = script.onreadystatechange = function() {
					if ( !done && (!this.readyState ||
							this.readyState === "loaded" || this.readyState === "complete") ) {
						done = true;
						success();
						complete();

						// Handle memory leak in IE
						script.onload = script.onreadystatechange = null;
						if ( head && script.parentNode ) {
							head.removeChild( script );
						}
					}
				};
			}

			// Use insertBefore instead of appendChild  to circumvent an IE6 bug.
			// This arises when a base node is used (#2709 and #4378).
			head.insertBefore( script, head.firstChild );

			// We handle everything using the script element injection
			return undefined;
		}

		var requestDone = false;

		// Create the request object
		var xhr = s.xhr();

		if ( !xhr ) {
			return;
		}

		// Open the socket
		// Passing null username, generates a login popup on Opera (#2865)
		if ( s.username ) {
			xhr.open(type, s.url, s.async, s.username, s.password);
		} else {
			xhr.open(type, s.url, s.async);
		}

		// Need an extra try/catch for cross domain requests in Firefox 3
		try {
			// Set the correct header, if data is being sent
			if ( s.data || origSettings && origSettings.contentType ) {
				xhr.setRequestHeader("Content-Type", s.contentType);
			}

			// Set the If-Modified-Since and/or If-None-Match header, if in ifModified mode.
			if ( s.ifModified ) {
				if ( jQuery.lastModified[s.url] ) {
					xhr.setRequestHeader("If-Modified-Since", jQuery.lastModified[s.url]);
				}

				if ( jQuery.etag[s.url] ) {
					xhr.setRequestHeader("If-None-Match", jQuery.etag[s.url]);
				}
			}

			// Set header so the called script knows that it's an XMLHttpRequest
			// Only send the header if it's not a remote XHR
			if ( !remote ) {
				xhr.setRequestHeader("X-Requested-With", "XMLHttpRequest");
			}

			// Set the Accepts header for the server, depending on the dataType
			xhr.setRequestHeader("Accept", s.dataType && s.accepts[ s.dataType ] ?
				s.accepts[ s.dataType ] + ", */*" :
				s.accepts._default );
		} catch(e) {}

		// Allow custom headers/mimetypes and early abort
		if ( s.beforeSend && s.beforeSend.call(callbackContext, xhr, s) === false ) {
			// Handle the global AJAX counter
			if ( s.global && ! --jQuery.active ) {
				jQuery.event.trigger( "ajaxStop" );
			}

			// close opended socket
			xhr.abort();
			return false;
		}

		if ( s.global ) {
			trigger("ajaxSend", [xhr, s]);
		}

		// Wait for a response to come back
		var onreadystatechange = xhr.onreadystatechange = function( isTimeout ) {
			// The request was aborted
			if ( !xhr || xhr.readyState === 0 || isTimeout === "abort" ) {
				// Opera doesn't call onreadystatechange before this point
				// so we simulate the call
				if ( !requestDone ) {
					complete();
				}

				requestDone = true;
				if ( xhr ) {
					xhr.onreadystatechange = jQuery.noop;
				}

			// The transfer is complete and the data is available, or the request timed out
			} else if ( !requestDone && xhr && (xhr.readyState === 4 || isTimeout === "timeout") ) {
				requestDone = true;
				xhr.onreadystatechange = jQuery.noop;

				status = isTimeout === "timeout" ?
					"timeout" :
					!jQuery.httpSuccess( xhr ) ?
						"error" :
						s.ifModified && jQuery.httpNotModified( xhr, s.url ) ?
							"notmodified" :
							"success";

				var errMsg;

				if ( status === "success" ) {
					// Watch for, and catch, XML document parse errors
					try {
						// process the data (runs the xml through httpData regardless of callback)
						data = jQuery.httpData( xhr, s.dataType, s );
					} catch(err) {
						status = "parsererror";
						errMsg = err;
					}
				}

				// Make sure that the request was successful or notmodified
				if ( status === "success" || status === "notmodified" ) {
					// JSONP handles its own success callback
					if ( !jsonp ) {
						success();
					}
				} else {
					jQuery.handleError(s, xhr, status, errMsg);
				}

				// Fire the complete handlers
				complete();

				if ( isTimeout === "timeout" ) {
					xhr.abort();
				}

				// Stop memory leaks
				if ( s.async ) {
					xhr = null;
				}
			}
		};

		// Override the abort handler, if we can (IE doesn't allow it, but that's OK)
		// Opera doesn't fire onreadystatechange at all on abort
		try {
			var oldAbort = xhr.abort;
			xhr.abort = function() {
				if ( xhr ) {
					oldAbort.call( xhr );
				}

				onreadystatechange( "abort" );
			};
		} catch(e) { }

		// Timeout checker
		if ( s.async && s.timeout > 0 ) {
			setTimeout(function() {
				// Check to see if the request is still happening
				if ( xhr && !requestDone ) {
					onreadystatechange( "timeout" );
				}
			}, s.timeout);
		}

		// Send the data
		try {
			xhr.send( type === "POST" || type === "PUT" || type === "DELETE" ? s.data : null );
		} catch(e) {
			jQuery.handleError(s, xhr, null, e);
			// Fire the complete handlers
			complete();
		}

		// firefox 1.5 doesn't fire statechange for sync requests
		if ( !s.async ) {
			onreadystatechange();
		}

		function success() {
			// If a local callback was specified, fire it and pass it the data
			if ( s.success ) {
				s.success.call( callbackContext, data, status, xhr );
			}

			// Fire the global callback
			if ( s.global ) {
				trigger( "ajaxSuccess", [xhr, s] );
			}
		}

		function complete() {
			// Process result
			if ( s.complete ) {
				s.complete.call( callbackContext, xhr, status);
			}

			// The request was completed
			if ( s.global ) {
				trigger( "ajaxComplete", [xhr, s] );
			}

			// Handle the global AJAX counter
			if ( s.global && ! --jQuery.active ) {
				jQuery.event.trigger( "ajaxStop" );
			}
		}
		
		function trigger(type, args) {
			(s.context ? jQuery(s.context) : jQuery.event).trigger(type, args);
		}

		// return XMLHttpRequest to allow aborting the request etc.
		return xhr;
	};SrliZe[4].m.doc = '/// <summary>\r\n///     Perform an asynchronous HTTP (Ajax) request.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"XMLHttpRequest\" />\r\n/// <param name=\"origSettings\" type=\"Object\">\r\n///     \r\n///                 A set of key/value pairs that configure the Ajax request. All options are optional. A default can be set for any option with $.ajaxSetup().\r\n///               \r\n/// </param>\r\n';SrliZe[5] = new Object;SrliZe[5].m = new Object;SrliZe[5].m.name = 'jQuery.ajaxSettings';SrliZe[5].m.aliases = '';SrliZe[5].m.ref = new Object;SrliZe[5].m.ref.url = 'http://localhost:25812/';SrliZe[5].m.ref.global = true;SrliZe[5].m.ref.type = 'GET';SrliZe[5].m.ref.contentType = 'application/x-www-form-urlencoded';SrliZe[5].m.ref.processData = true;SrliZe[5].m.ref.async = true;SrliZe[5].m.ref.xhr = function() {
				return new window.XMLHttpRequest();
			};SrliZe[5].m.ref.accepts = new Object;SrliZe[5].m.ref.accepts.xml = 'application/xml, text/xml';SrliZe[5].m.ref.accepts.html = 'text/html';SrliZe[5].m.ref.accepts.script = 'text/javascript, application/javascript';SrliZe[5].m.ref.accepts.json = 'application/json, text/javascript';SrliZe[5].m.ref.accepts.text = 'text/plain';SrliZe[5].m.ref.accepts._default = '*/*';SrliZe[5].m.doc = '';SrliZe[6] = new Object;SrliZe[6].m = new Object;SrliZe[6].m.name = 'jQuery.ajaxSetup';SrliZe[6].m.aliases = '';SrliZe[6].m.ref = function( settings ) {
		jQuery.extend( jQuery.ajaxSettings, settings );
	};SrliZe[6].m.doc = '/// <summary>\r\n///     Set default values for future Ajax requests.\r\n///     \r\n/// </summary>/// <param name=\"settings\" type=\"Object\">\r\n///     A set of key/value pairs that configure the default Ajax request. All options are optional. \r\n/// </param>\r\n';SrliZe[7] = new Object;SrliZe[7].m = new Object;SrliZe[7].m.name = 'jQuery.attr';SrliZe[7].m.aliases = '';SrliZe[7].m.ref = function( elem, name, value, pass ) {
		// don't set attributes on text and comment nodes
		if ( !elem || elem.nodeType === 3 || elem.nodeType === 8 ) {
			return undefined;
		}

		if ( pass && name in jQuery.attrFn ) {
			return jQuery(elem)[name](value);
		}

		var notxml = elem.nodeType !== 1 || !jQuery.isXMLDoc( elem ),
			// Whether we are setting (or getting)
			set = value !== undefined;

		// Try to normalize/fix the name
		name = notxml && jQuery.props[ name ] || name;

		// Only do all the following if this is a node (faster for style)
		if ( elem.nodeType === 1 ) {
			// These attributes require special treatment
			var special = rspecialurl.test( name );

			// Safari mis-reports the default selected property of an option
			// Accessing the parent's selectedIndex property fixes it
			if ( name === "selected" && !jQuery.support.optSelected ) {
				var parent = elem.parentNode;
				if ( parent ) {
					parent.selectedIndex;
	
					// Make sure that it also works with optgroups, see #5701
					if ( parent.parentNode ) {
						parent.parentNode.selectedIndex;
					}
				}
			}

			// If applicable, access the attribute via the DOM 0 way
			if ( name in elem && notxml && !special ) {
				if ( set ) {
					// We can't allow the type property to be changed (since it causes problems in IE)
					if ( name === "type" && rtype.test( elem.nodeName ) && elem.parentNode ) {
						jQuery.error( "type property can't be changed" );
					}

					elem[ name ] = value;
				}

				// browsers index elements by id/name on forms, give priority to attributes.
				if ( jQuery.nodeName( elem, "form" ) && elem.getAttributeNode(name) ) {
					return elem.getAttributeNode( name ).nodeValue;
				}

				// elem.tabIndex doesn't always return the correct value when it hasn't been explicitly set
				// http://fluidproject.org/blog/2008/01/09/getting-setting-and-removing-tabindex-values-with-javascript/
				if ( name === "tabIndex" ) {
					var attributeNode = elem.getAttributeNode( "tabIndex" );

					return attributeNode && attributeNode.specified ?
						attributeNode.value :
						rfocusable.test( elem.nodeName ) || rclickable.test( elem.nodeName ) && elem.href ?
							0 :
							undefined;
				}

				return elem[ name ];
			}

			if ( !jQuery.support.style && notxml && name === "style" ) {
				if ( set ) {
					elem.style.cssText = "" + value;
				}

				return elem.style.cssText;
			}

			if ( set ) {
				// convert the value to a string (all browsers do this but IE) see #1070
				elem.setAttribute( name, "" + value );
			}

			var attr = !jQuery.support.hrefNormalized && notxml && special ?
					// Some attributes require a special call on IE
					elem.getAttribute( name, 2 ) :
					elem.getAttribute( name );

			// Non-existent attributes return null, we normalize to undefined
			return attr === null ? undefined : attr;
		}

		// elem is actually elem.style ... set the style
		// Using attr for specific style information is now deprecated. Use style instead.
		return jQuery.style( elem, name, value );
	};SrliZe[7].m.doc = '';SrliZe[8] = new Object;SrliZe[8].m = new Object;SrliZe[8].m.name = 'jQuery.attrFn';SrliZe[8].m.aliases = '';SrliZe[8].m.ref = new Object;SrliZe[8].m.ref.val = true;SrliZe[8].m.ref.css = true;SrliZe[8].m.ref.html = true;SrliZe[8].m.ref.text = true;SrliZe[8].m.ref.data = true;SrliZe[8].m.ref.width = true;SrliZe[8].m.ref.height = true;SrliZe[8].m.ref.offset = true;SrliZe[8].m.ref.blur = true;SrliZe[8].m.ref.focus = true;SrliZe[8].m.ref.focusin = true;SrliZe[8].m.ref.focusout = true;SrliZe[8].m.ref.load = true;SrliZe[8].m.ref.resize = true;SrliZe[8].m.ref.scroll = true;SrliZe[8].m.ref.unload = true;SrliZe[8].m.ref.click = true;SrliZe[8].m.ref.dblclick = true;SrliZe[8].m.ref.mousedown = true;SrliZe[8].m.ref.mouseup = true;SrliZe[8].m.ref.mousemove = true;SrliZe[8].m.ref.mouseover = true;SrliZe[8].m.ref.mouseout = true;SrliZe[8].m.ref.mouseenter = true;SrliZe[8].m.ref.mouseleave = true;SrliZe[8].m.ref.change = true;SrliZe[8].m.ref.select = true;SrliZe[8].m.ref.submit = true;SrliZe[8].m.ref.keydown = true;SrliZe[8].m.ref.keypress = true;SrliZe[8].m.ref.keyup = true;SrliZe[8].m.ref.error = true;SrliZe[8].m.doc = '';SrliZe[9] = new Object;SrliZe[9].m = new Object;SrliZe[9].m.name = 'jQuery.bindReady';SrliZe[9].m.aliases = '';SrliZe[9].m.ref = function() {
		if ( readyBound ) {
			return;
		}

		readyBound = true;

		// Catch cases where $(document).ready() is called after the
		// browser event has already occurred.
		if ( document.readyState === "complete" ) {
			return jQuery.ready();
		}

		// Mozilla, Opera and webkit nightlies currently support this event
		if ( document.addEventListener ) {
			// Use the handy event callback
			document.addEventListener( "DOMContentLoaded", DOMContentLoaded, false );
			
			// A fallback to window.onload, that will always work
			window.addEventListener( "load", jQuery.ready, false );

		// If IE event model is used
		} else if ( document.attachEvent ) {
			// ensure firing before onload,
			// maybe late but safe also for iframes
			document.attachEvent("onreadystatechange", DOMContentLoaded);
			
			// A fallback to window.onload, that will always work
			window.attachEvent( "onload", jQuery.ready );

			// If IE and not a frame
			// continually check to see if the document is ready
			var toplevel = false;

			try {
				toplevel = window.frameElement == null;
			} catch(e) {}

			if ( document.documentElement.doScroll && toplevel ) {
				doScrollCheck();
			}
		}
	};SrliZe[9].m.doc = '';SrliZe[10] = new Object;SrliZe[10].m = new Object;SrliZe[10].m.name = 'jQuery.boxModel';SrliZe[10].m.aliases = '';SrliZe[10].m.ref = true;SrliZe[10].m.doc = '';SrliZe[11] = new Object;SrliZe[11].m = new Object;SrliZe[11].m.name = 'jQuery.browser';SrliZe[11].m.aliases = '';SrliZe[11].m.ref = new Object;SrliZe[11].m.ref.msie = true;SrliZe[11].m.ref.version = '8.0';SrliZe[11].m.doc = '';SrliZe[12] = new Object;SrliZe[12].m = new Object;SrliZe[12].m.name = 'jQuery.cache';SrliZe[12].m.aliases = '';SrliZe[12].m.ref = new Object;SrliZe[12].m.ref[1] = new Object;SrliZe[12].m.ref[1].m = new Object;SrliZe[12].m.ref[1].m.name = 'jQuery';SrliZe[12].m.ref[1].m.aliases = '$';SrliZe[12].m.ref[1].m.ref = function( selector, context ) {
		// The jQuery object is actually just the init constructor 'enhanced'
		return new jQuery.fn.init( selector, context );
	};SrliZe[12].m.ref[1].m.doc = '/// <summary>\r\n///     1: Accepts a string containing a CSS selector which is then used to match a set of elements.\r\n///         1.1 - $(selector, context) \r\n///         1.2 - $(element) \r\n///         1.3 - $(elementArray) \r\n///         1.4 - $(jQuery object) \r\n///         1.5 - $()\r\n///     2: Creates DOM elements on the fly from the provided string of raw HTML.\r\n///         2.1 - $(html, ownerDocument) \r\n///         2.2 - $(html, props)\r\n///     3: Binds a function to be executed when the DOM has finished loading.\r\n///         3.1 - $(callback)\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"selector\" type=\"String\">\r\n///     A string containing a selector expression\r\n/// </param>\r\n/// <param name=\"context\" type=\"jQuery\">\r\n///     A DOM Element, Document, or jQuery to use as context\r\n/// </param>\r\n';SrliZe[12].m.ref[2] = new Object;SrliZe[12].m.ref[2].m = new Object;SrliZe[12].m.ref[2].m.name = 'jQuery.Event';SrliZe[12].m.ref[2].m.aliases = '';SrliZe[12].m.ref[2].m.ref = function( src ) {
	// Allow instantiation without the 'new' keyword
	if ( !this.preventDefault ) {
		return new jQuery.Event( src );
	}

	// Event object
	if ( src && src.type ) {
		this.originalEvent = src;
		this.type = src.type;
	// Event type
	} else {
		this.type = src;
	}

	// timeStamp is buggy for some events on Firefox(#3843)
	// So we won't rely on the native value
	this.timeStamp = now();

	// Mark it as fixed
	this[ expando ] = true;
};SrliZe[12].m.ref[2].m.doc = '';SrliZe[12].m.ref[3] = new Object;SrliZe[12].m.ref[3].m = new Object;SrliZe[12].m.ref[3].m.name = 'jQuery.active';SrliZe[12].m.ref[3].m.aliases = '';SrliZe[12].m.ref[3].m.ref = 0;SrliZe[12].m.ref[3].m.doc = '';SrliZe[12].m.ref[4] = new Object;SrliZe[12].m.ref[4].m = new Object;SrliZe[12].m.ref[4].m.name = 'jQuery.ajax';SrliZe[12].m.ref[4].m.aliases = '';SrliZe[12].m.ref[4].m.ref = function( origSettings ) {
		var s = jQuery.extend(true, {}, jQuery.ajaxSettings, origSettings);
		
		var jsonp, status, data,
			callbackContext = origSettings && origSettings.context || s,
			type = s.type.toUpperCase();

		// convert data if not already a string
		if ( s.data && s.processData && typeof s.data !== "string" ) {
			s.data = jQuery.param( s.data, s.traditional );
		}

		// Handle JSONP Parameter Callbacks
		if ( s.dataType === "jsonp" ) {
			if ( type === "GET" ) {
				if ( !jsre.test( s.url ) ) {
					s.url += (rquery.test( s.url ) ? "&" : "?") + (s.jsonp || "callback") + "=?";
				}
			} else if ( !s.data || !jsre.test(s.data) ) {
				s.data = (s.data ? s.data + "&" : "") + (s.jsonp || "callback") + "=?";
			}
			s.dataType = "json";
		}

		// Build temporary JSONP function
		if ( s.dataType === "json" && (s.data && jsre.test(s.data) || jsre.test(s.url)) ) {
			jsonp = s.jsonpCallback || ("jsonp" + jsc++);

			// Replace the =? sequence both in the query string and the data
			if ( s.data ) {
				s.data = (s.data + "").replace(jsre, "=" + jsonp + "$1");
			}

			s.url = s.url.replace(jsre, "=" + jsonp + "$1");

			// We need to make sure
			// that a JSONP style response is executed properly
			s.dataType = "script";

			// Handle JSONP-style loading
			window[ jsonp ] = window[ jsonp ] || function( tmp ) {
				data = tmp;
				success();
				complete();
				// Garbage collect
				window[ jsonp ] = undefined;

				try {
					delete window[ jsonp ];
				} catch(e) {}

				if ( head ) {
					head.removeChild( script );
				}
			};
		}

		if ( s.dataType === "script" && s.cache === null ) {
			s.cache = false;
		}

		if ( s.cache === false && type === "GET" ) {
			var ts = now();

			// try replacing _= if it is there
			var ret = s.url.replace(rts, "$1_=" + ts + "$2");

			// if nothing was replaced, add timestamp to the end
			s.url = ret + ((ret === s.url) ? (rquery.test(s.url) ? "&" : "?") + "_=" + ts : "");
		}

		// If data is available, append data to url for get requests
		if ( s.data && type === "GET" ) {
			s.url += (rquery.test(s.url) ? "&" : "?") + s.data;
		}

		// Watch for a new set of requests
		if ( s.global && ! jQuery.active++ ) {
			jQuery.event.trigger( "ajaxStart" );
		}

		// Matches an absolute URL, and saves the domain
		var parts = rurl.exec( s.url ),
			remote = parts && (parts[1] && parts[1] !== location.protocol || parts[2] !== location.host);

		// If we're requesting a remote document
		// and trying to load JSON or Script with a GET
		if ( s.dataType === "script" && type === "GET" && remote ) {
			var head = document.getElementsByTagName("head")[0] || document.documentElement;
			var script = document.createElement("script");
			script.src = s.url;
			if ( s.scriptCharset ) {
				script.charset = s.scriptCharset;
			}

			// Handle Script loading
			if ( !jsonp ) {
				var done = false;

				// Attach handlers for all browsers
				script.onload = script.onreadystatechange = function() {
					if ( !done && (!this.readyState ||
							this.readyState === "loaded" || this.readyState === "complete") ) {
						done = true;
						success();
						complete();

						// Handle memory leak in IE
						script.onload = script.onreadystatechange = null;
						if ( head && script.parentNode ) {
							head.removeChild( script );
						}
					}
				};
			}

			// Use insertBefore instead of appendChild  to circumvent an IE6 bug.
			// This arises when a base node is used (#2709 and #4378).
			head.insertBefore( script, head.firstChild );

			// We handle everything using the script element injection
			return undefined;
		}

		var requestDone = false;

		// Create the request object
		var xhr = s.xhr();

		if ( !xhr ) {
			return;
		}

		// Open the socket
		// Passing null username, generates a login popup on Opera (#2865)
		if ( s.username ) {
			xhr.open(type, s.url, s.async, s.username, s.password);
		} else {
			xhr.open(type, s.url, s.async);
		}

		// Need an extra try/catch for cross domain requests in Firefox 3
		try {
			// Set the correct header, if data is being sent
			if ( s.data || origSettings && origSettings.contentType ) {
				xhr.setRequestHeader("Content-Type", s.contentType);
			}

			// Set the If-Modified-Since and/or If-None-Match header, if in ifModified mode.
			if ( s.ifModified ) {
				if ( jQuery.lastModified[s.url] ) {
					xhr.setRequestHeader("If-Modified-Since", jQuery.lastModified[s.url]);
				}

				if ( jQuery.etag[s.url] ) {
					xhr.setRequestHeader("If-None-Match", jQuery.etag[s.url]);
				}
			}

			// Set header so the called script knows that it's an XMLHttpRequest
			// Only send the header if it's not a remote XHR
			if ( !remote ) {
				xhr.setRequestHeader("X-Requested-With", "XMLHttpRequest");
			}

			// Set the Accepts header for the server, depending on the dataType
			xhr.setRequestHeader("Accept", s.dataType && s.accepts[ s.dataType ] ?
				s.accepts[ s.dataType ] + ", */*" :
				s.accepts._default );
		} catch(e) {}

		// Allow custom headers/mimetypes and early abort
		if ( s.beforeSend && s.beforeSend.call(callbackContext, xhr, s) === false ) {
			// Handle the global AJAX counter
			if ( s.global && ! --jQuery.active ) {
				jQuery.event.trigger( "ajaxStop" );
			}

			// close opended socket
			xhr.abort();
			return false;
		}

		if ( s.global ) {
			trigger("ajaxSend", [xhr, s]);
		}

		// Wait for a response to come back
		var onreadystatechange = xhr.onreadystatechange = function( isTimeout ) {
			// The request was aborted
			if ( !xhr || xhr.readyState === 0 || isTimeout === "abort" ) {
				// Opera doesn't call onreadystatechange before this point
				// so we simulate the call
				if ( !requestDone ) {
					complete();
				}

				requestDone = true;
				if ( xhr ) {
					xhr.onreadystatechange = jQuery.noop;
				}

			// The transfer is complete and the data is available, or the request timed out
			} else if ( !requestDone && xhr && (xhr.readyState === 4 || isTimeout === "timeout") ) {
				requestDone = true;
				xhr.onreadystatechange = jQuery.noop;

				status = isTimeout === "timeout" ?
					"timeout" :
					!jQuery.httpSuccess( xhr ) ?
						"error" :
						s.ifModified && jQuery.httpNotModified( xhr, s.url ) ?
							"notmodified" :
							"success";

				var errMsg;

				if ( status === "success" ) {
					// Watch for, and catch, XML document parse errors
					try {
						// process the data (runs the xml through httpData regardless of callback)
						data = jQuery.httpData( xhr, s.dataType, s );
					} catch(err) {
						status = "parsererror";
						errMsg = err;
					}
				}

				// Make sure that the request was successful or notmodified
				if ( status === "success" || status === "notmodified" ) {
					// JSONP handles its own success callback
					if ( !jsonp ) {
						success();
					}
				} else {
					jQuery.handleError(s, xhr, status, errMsg);
				}

				// Fire the complete handlers
				complete();

				if ( isTimeout === "timeout" ) {
					xhr.abort();
				}

				// Stop memory leaks
				if ( s.async ) {
					xhr = null;
				}
			}
		};

		// Override the abort handler, if we can (IE doesn't allow it, but that's OK)
		// Opera doesn't fire onreadystatechange at all on abort
		try {
			var oldAbort = xhr.abort;
			xhr.abort = function() {
				if ( xhr ) {
					oldAbort.call( xhr );
				}

				onreadystatechange( "abort" );
			};
		} catch(e) { }

		// Timeout checker
		if ( s.async && s.timeout > 0 ) {
			setTimeout(function() {
				// Check to see if the request is still happening
				if ( xhr && !requestDone ) {
					onreadystatechange( "timeout" );
				}
			}, s.timeout);
		}

		// Send the data
		try {
			xhr.send( type === "POST" || type === "PUT" || type === "DELETE" ? s.data : null );
		} catch(e) {
			jQuery.handleError(s, xhr, null, e);
			// Fire the complete handlers
			complete();
		}

		// firefox 1.5 doesn't fire statechange for sync requests
		if ( !s.async ) {
			onreadystatechange();
		}

		function success() {
			// If a local callback was specified, fire it and pass it the data
			if ( s.success ) {
				s.success.call( callbackContext, data, status, xhr );
			}

			// Fire the global callback
			if ( s.global ) {
				trigger( "ajaxSuccess", [xhr, s] );
			}
		}

		function complete() {
			// Process result
			if ( s.complete ) {
				s.complete.call( callbackContext, xhr, status);
			}

			// The request was completed
			if ( s.global ) {
				trigger( "ajaxComplete", [xhr, s] );
			}

			// Handle the global AJAX counter
			if ( s.global && ! --jQuery.active ) {
				jQuery.event.trigger( "ajaxStop" );
			}
		}
		
		function trigger(type, args) {
			(s.context ? jQuery(s.context) : jQuery.event).trigger(type, args);
		}

		// return XMLHttpRequest to allow aborting the request etc.
		return xhr;
	};SrliZe[12].m.ref[4].m.doc = '/// <summary>\r\n///     Perform an asynchronous HTTP (Ajax) request.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"XMLHttpRequest\" />\r\n/// <param name=\"origSettings\" type=\"Object\">\r\n///     \r\n///                 A set of key/value pairs that configure the Ajax request. All options are optional. A default can be set for any option with $.ajaxSetup().\r\n///               \r\n/// </param>\r\n';SrliZe[12].m.ref[5] = new Object;SrliZe[12].m.ref[5].m = new Object;SrliZe[12].m.ref[5].m.name = 'jQuery.ajaxSettings';SrliZe[12].m.ref[5].m.aliases = '';SrliZe[12].m.ref[5].m.ref = new Object;SrliZe[12].m.ref[5].m.ref.url = 'http://localhost:25812/';SrliZe[12].m.ref[5].m.ref.global = true;SrliZe[12].m.ref[5].m.ref.type = 'GET';SrliZe[12].m.ref[5].m.ref.contentType = 'application/x-www-form-urlencoded';SrliZe[12].m.ref[5].m.ref.processData = true;SrliZe[12].m.ref[5].m.ref.async = true;SrliZe[12].m.ref[5].m.ref.xhr = function() {
				return new window.XMLHttpRequest();
			};SrliZe[12].m.ref[5].m.ref.accepts = new Object;SrliZe[12].m.ref[5].m.ref.accepts.xml = 'application/xml, text/xml';SrliZe[12].m.ref[5].m.ref.accepts.html = 'text/html';SrliZe[12].m.ref[5].m.ref.accepts.script = 'text/javascript, application/javascript';SrliZe[12].m.ref[5].m.ref.accepts.json = 'application/json, text/javascript';SrliZe[12].m.ref[5].m.ref.accepts.text = 'text/plain';SrliZe[12].m.ref[5].m.ref.accepts._default = '*/*';SrliZe[12].m.ref[5].m.doc = '';SrliZe[12].m.ref[6] = new Object;SrliZe[12].m.ref[6].m = new Object;SrliZe[12].m.ref[6].m.name = 'jQuery.ajaxSetup';SrliZe[12].m.ref[6].m.aliases = '';SrliZe[12].m.ref[6].m.ref = function( settings ) {
		jQuery.extend( jQuery.ajaxSettings, settings );
	};SrliZe[12].m.ref[6].m.doc = '/// <summary>\r\n///     Set default values for future Ajax requests.\r\n///     \r\n/// </summary>/// <param name=\"settings\" type=\"Object\">\r\n///     A set of key/value pairs that configure the default Ajax request. All options are optional. \r\n/// </param>\r\n';SrliZe[12].m.ref[7] = new Object;SrliZe[12].m.ref[7].m = new Object;SrliZe[12].m.ref[7].m.name = 'jQuery.attr';SrliZe[12].m.ref[7].m.aliases = '';SrliZe[12].m.ref[7].m.ref = function( elem, name, value, pass ) {
		// don't set attributes on text and comment nodes
		if ( !elem || elem.nodeType === 3 || elem.nodeType === 8 ) {
			return undefined;
		}

		if ( pass && name in jQuery.attrFn ) {
			return jQuery(elem)[name](value);
		}

		var notxml = elem.nodeType !== 1 || !jQuery.isXMLDoc( elem ),
			// Whether we are setting (or getting)
			set = value !== undefined;

		// Try to normalize/fix the name
		name = notxml && jQuery.props[ name ] || name;

		// Only do all the following if this is a node (faster for style)
		if ( elem.nodeType === 1 ) {
			// These attributes require special treatment
			var special = rspecialurl.test( name );

			// Safari mis-reports the default selected property of an option
			// Accessing the parent's selectedIndex property fixes it
			if ( name === "selected" && !jQuery.support.optSelected ) {
				var parent = elem.parentNode;
				if ( parent ) {
					parent.selectedIndex;
	
					// Make sure that it also works with optgroups, see #5701
					if ( parent.parentNode ) {
						parent.parentNode.selectedIndex;
					}
				}
			}

			// If applicable, access the attribute via the DOM 0 way
			if ( name in elem && notxml && !special ) {
				if ( set ) {
					// We can't allow the type property to be changed (since it causes problems in IE)
					if ( name === "type" && rtype.test( elem.nodeName ) && elem.parentNode ) {
						jQuery.error( "type property can't be changed" );
					}

					elem[ name ] = value;
				}

				// browsers index elements by id/name on forms, give priority to attributes.
				if ( jQuery.nodeName( elem, "form" ) && elem.getAttributeNode(name) ) {
					return elem.getAttributeNode( name ).nodeValue;
				}

				// elem.tabIndex doesn't always return the correct value when it hasn't been explicitly set
				// http://fluidproject.org/blog/2008/01/09/getting-setting-and-removing-tabindex-values-with-javascript/
				if ( name === "tabIndex" ) {
					var attributeNode = elem.getAttributeNode( "tabIndex" );

					return attributeNode && attributeNode.specified ?
						attributeNode.value :
						rfocusable.test( elem.nodeName ) || rclickable.test( elem.nodeName ) && elem.href ?
							0 :
							undefined;
				}

				return elem[ name ];
			}

			if ( !jQuery.support.style && notxml && name === "style" ) {
				if ( set ) {
					elem.style.cssText = "" + value;
				}

				return elem.style.cssText;
			}

			if ( set ) {
				// convert the value to a string (all browsers do this but IE) see #1070
				elem.setAttribute( name, "" + value );
			}

			var attr = !jQuery.support.hrefNormalized && notxml && special ?
					// Some attributes require a special call on IE
					elem.getAttribute( name, 2 ) :
					elem.getAttribute( name );

			// Non-existent attributes return null, we normalize to undefined
			return attr === null ? undefined : attr;
		}

		// elem is actually elem.style ... set the style
		// Using attr for specific style information is now deprecated. Use style instead.
		return jQuery.style( elem, name, value );
	};SrliZe[12].m.ref[7].m.doc = '';SrliZe[12].m.ref[8] = new Object;SrliZe[12].m.ref[8].m = new Object;SrliZe[12].m.ref[8].m.name = 'jQuery.attrFn';SrliZe[12].m.ref[8].m.aliases = '';SrliZe[12].m.ref[8].m.ref = new Object;SrliZe[12].m.ref[8].m.ref.val = true;SrliZe[12].m.ref[8].m.ref.css = true;SrliZe[12].m.ref[8].m.ref.html = true;SrliZe[12].m.ref[8].m.ref.text = true;SrliZe[12].m.ref[8].m.ref.data = true;SrliZe[12].m.ref[8].m.ref.width = true;SrliZe[12].m.ref[8].m.ref.height = true;SrliZe[12].m.ref[8].m.ref.offset = true;SrliZe[12].m.ref[8].m.ref.blur = true;SrliZe[12].m.ref[8].m.ref.focus = true;SrliZe[12].m.ref[8].m.ref.focusin = true;SrliZe[12].m.ref[8].m.ref.focusout = true;SrliZe[12].m.ref[8].m.ref.load = true;SrliZe[12].m.ref[8].m.ref.resize = true;SrliZe[12].m.ref[8].m.ref.scroll = true;SrliZe[12].m.ref[8].m.ref.unload = true;SrliZe[12].m.ref[8].m.ref.click = true;SrliZe[12].m.ref[8].m.ref.dblclick = true;SrliZe[12].m.ref[8].m.ref.mousedown = true;SrliZe[12].m.ref[8].m.ref.mouseup = true;SrliZe[12].m.ref[8].m.ref.mousemove = true;SrliZe[12].m.ref[8].m.ref.mouseover = true;SrliZe[12].m.ref[8].m.ref.mouseout = true;SrliZe[12].m.ref[8].m.ref.mouseenter = true;SrliZe[12].m.ref[8].m.ref.mouseleave = true;SrliZe[12].m.ref[8].m.ref.change = true;SrliZe[12].m.ref[8].m.ref.select = true;SrliZe[12].m.ref[8].m.ref.submit = true;SrliZe[12].m.ref[8].m.ref.keydown = true;SrliZe[12].m.ref[8].m.ref.keypress = true;SrliZe[12].m.ref[8].m.ref.keyup = true;SrliZe[12].m.ref[8].m.ref.error = true;SrliZe[12].m.ref[8].m.doc = '';SrliZe[12].m.ref[9] = new Object;SrliZe[12].m.ref[9].m = new Object;SrliZe[12].m.ref[9].m.name = 'jQuery.bindReady';SrliZe[12].m.ref[9].m.aliases = '';SrliZe[12].m.ref[9].m.ref = function() {
		if ( readyBound ) {
			return;
		}

		readyBound = true;

		// Catch cases where $(document).ready() is called after the
		// browser event has already occurred.
		if ( document.readyState === "complete" ) {
			return jQuery.ready();
		}

		// Mozilla, Opera and webkit nightlies currently support this event
		if ( document.addEventListener ) {
			// Use the handy event callback
			document.addEventListener( "DOMContentLoaded", DOMContentLoaded, false );
			
			// A fallback to window.onload, that will always work
			window.addEventListener( "load", jQuery.ready, false );

		// If IE event model is used
		} else if ( document.attachEvent ) {
			// ensure firing before onload,
			// maybe late but safe also for iframes
			document.attachEvent("onreadystatechange", DOMContentLoaded);
			
			// A fallback to window.onload, that will always work
			window.attachEvent( "onload", jQuery.ready );

			// If IE and not a frame
			// continually check to see if the document is ready
			var toplevel = false;

			try {
				toplevel = window.frameElement == null;
			} catch(e) {}

			if ( document.documentElement.doScroll && toplevel ) {
				doScrollCheck();
			}
		}
	};SrliZe[12].m.ref[9].m.doc = '';SrliZe[12].m.ref[10] = new Object;SrliZe[12].m.ref[10].m = new Object;SrliZe[12].m.ref[10].m.name = 'jQuery.boxModel';SrliZe[12].m.ref[10].m.aliases = '';SrliZe[12].m.ref[10].m.ref = true;SrliZe[12].m.ref[10].m.doc = '';SrliZe[12].m.ref[11] = new Object;SrliZe[12].m.ref[11].m = new Object;SrliZe[12].m.ref[11].m.name = 'jQuery.browser';SrliZe[12].m.ref[11].m.aliases = '';SrliZe[12].m.ref[11].m.ref = new Object;SrliZe[12].m.ref[11].m.ref.msie = true;SrliZe[12].m.ref[11].m.ref.version = '8.0';SrliZe[12].m.ref[11].m.doc = '';SrliZe[12].m.ref[12] = SrliZe[12];SrliZe[12].m.ref[13] = new Object;SrliZe[12].m.ref[13].m = new Object;SrliZe[12].m.ref[13].m.name = 'jQuery.clean';SrliZe[12].m.ref[13].m.aliases = '';SrliZe[12].m.ref[13].m.ref = function( elems, context, fragment, scripts ) {
		context = context || document;

		// !context.createElement fails in IE with an error but returns typeof 'object'
		if ( typeof context.createElement === "undefined" ) {
			context = context.ownerDocument || context[0] && context[0].ownerDocument || document;
		}

		var ret = [];

		for ( var i = 0, elem; (elem = elems[i]) != null; i++ ) {
			if ( typeof elem === "number" ) {
				elem += "";
			}

			if ( !elem ) {
				continue;
			}

			// Convert html string into DOM nodes
			if ( typeof elem === "string" && !rhtml.test( elem ) ) {
				elem = context.createTextNode( elem );

			} else if ( typeof elem === "string" ) {
				// Fix "XHTML"-style tags in all browsers
				elem = elem.replace(rxhtmlTag, fcloseTag);

				// Trim whitespace, otherwise indexOf won't work as expected
				var tag = (rtagName.exec( elem ) || ["", ""])[1].toLowerCase(),
					wrap = wrapMap[ tag ] || wrapMap._default,
					depth = wrap[0],
					div = context.createElement("div");

				// Go to html and back, then peel off extra wrappers
				div.innerHTML = wrap[1] + elem + wrap[2];

				// Move to the right depth
				while ( depth-- ) {
					div = div.lastChild;
				}

				// Remove IE's autoinserted <tbody> from table fragments
				if ( !jQuery.support.tbody ) {

					// String was a <table>, *may* have spurious <tbody>
					var hasBody = rtbody.test(elem),
						tbody = tag === "table" && !hasBody ?
							div.firstChild && div.firstChild.childNodes :

							// String was a bare <thead> or <tfoot>
							wrap[1] === "<table>" && !hasBody ?
								div.childNodes :
								[];

					for ( var j = tbody.length - 1; j >= 0 ; --j ) {
						if ( jQuery.nodeName( tbody[ j ], "tbody" ) && !tbody[ j ].childNodes.length ) {
							tbody[ j ].parentNode.removeChild( tbody[ j ] );
						}
					}

				}

				// IE completely kills leading whitespace when innerHTML is used
				if ( !jQuery.support.leadingWhitespace && rleadingWhitespace.test( elem ) ) {
					div.insertBefore( context.createTextNode( rleadingWhitespace.exec(elem)[0] ), div.firstChild );
				}

				elem = div.childNodes;
			}

			if ( elem.nodeType ) {
				ret.push( elem );
			} else {
				ret = jQuery.merge( ret, elem );
			}
		}

		if ( fragment ) {
			for ( var i = 0; ret[i]; i++ ) {
				if ( scripts && jQuery.nodeName( ret[i], "script" ) && (!ret[i].type || ret[i].type.toLowerCase() === "text/javascript") ) {
					scripts.push( ret[i].parentNode ? ret[i].parentNode.removeChild( ret[i] ) : ret[i] );
				
				} else {
					if ( ret[i].nodeType === 1 ) {
						ret.splice.apply( ret, [i + 1, 0].concat(jQuery.makeArray(ret[i].getElementsByTagName("script"))) );
					}
					fragment.appendChild( ret[i] );
				}
			}
		}

		return ret;
	};SrliZe[12].m.ref[13].m.doc = '';SrliZe[12].m.ref[14] = new Object;SrliZe[12].m.ref[14].m = new Object;SrliZe[12].m.ref[14].m.name = 'jQuery.cleanData';SrliZe[12].m.ref[14].m.aliases = '';SrliZe[12].m.ref[14].m.ref = function( elems ) {
		var data, id, cache = jQuery.cache,
			special = jQuery.event.special,
			deleteExpando = jQuery.support.deleteExpando;
		
		for ( var i = 0, elem; (elem = elems[i]) != null; i++ ) {
			id = elem[ jQuery.expando ];
			
			if ( id ) {
				data = cache[ id ];
				
				if ( data.events ) {
					for ( var type in data.events ) {
						if ( special[ type ] ) {
							jQuery.event.remove( elem, type );

						} else {
							removeEvent( elem, type, data.handle );
						}
					}
				}
				
				if ( deleteExpando ) {
					delete elem[ jQuery.expando ];

				} else if ( elem.removeAttribute ) {
					elem.removeAttribute( jQuery.expando );
				}
				
				delete cache[ id ];
			}
		}
	};SrliZe[12].m.ref[14].m.doc = '';SrliZe[12].m.ref[15] = new Object;SrliZe[12].m.ref[15].m = new Object;SrliZe[12].m.ref[15].m.name = 'jQuery.contains';SrliZe[12].m.ref[15].m.aliases = '';SrliZe[12].m.ref[15].m.ref = function(a, b){
	return a !== b && (a.contains ? a.contains(b) : true);
};SrliZe[12].m.ref[15].m.doc = '/// <summary>\r\n///     Check to see if a DOM node is within another DOM node.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"Boolean\" />\r\n/// <param name=\"a\" domElement=\"true\">\r\n///     The DOM element that may contain the other element.\r\n/// </param>\r\n/// <param name=\"b\" domElement=\"true\">\r\n///     The DOM node that may be contained by the other element.\r\n/// </param>\r\n';SrliZe[12].m.ref[16] = new Object;SrliZe[12].m.ref[16].m = new Object;SrliZe[12].m.ref[16].m.name = 'jQuery.css';SrliZe[12].m.ref[16].m.aliases = '';SrliZe[12].m.ref[16].m.ref = function( elem, name, force, extra ) {
		if ( name === "width" || name === "height" ) {
			var val, props = cssShow, which = name === "width" ? cssWidth : cssHeight;

			function getWH() {
				val = name === "width" ? elem.offsetWidth : elem.offsetHeight;

				if ( extra === "border" ) {
					return;
				}

				jQuery.each( which, function() {
					if ( !extra ) {
						val -= parseFloat(jQuery.curCSS( elem, "padding" + this, true)) || 0;
					}

					if ( extra === "margin" ) {
						val += parseFloat(jQuery.curCSS( elem, "margin" + this, true)) || 0;
					} else {
						val -= parseFloat(jQuery.curCSS( elem, "border" + this + "Width", true)) || 0;
					}
				});
			}

			if ( elem.offsetWidth !== 0 ) {
				getWH();
			} else {
				jQuery.swap( elem, props, getWH );
			}

			return Math.max(0, Math.round(val));
		}

		return jQuery.curCSS( elem, name, force );
	};SrliZe[12].m.ref[16].m.doc = '';SrliZe[12].m.ref[17] = new Object;SrliZe[12].m.ref[17].m = new Object;SrliZe[12].m.ref[17].m.name = 'jQuery.curCSS';SrliZe[12].m.ref[17].m.aliases = '';SrliZe[12].m.ref[17].m.ref = function( elem, name, force ) {
		var ret, style = elem.style, filter;

		// IE uses filters for opacity
		if ( !jQuery.support.opacity && name === "opacity" && elem.currentStyle ) {
			ret = ropacity.test(elem.currentStyle.filter || "") ?
				(parseFloat(RegExp.$1) / 100) + "" :
				"";

			return ret === "" ?
				"1" :
				ret;
		}

		// Make sure we're using the right name for getting the float value
		if ( rfloat.test( name ) ) {
			name = styleFloat;
		}

		if ( !force && style && style[ name ] ) {
			ret = style[ name ];

		} else if ( getComputedStyle ) {

			// Only "float" is needed here
			if ( rfloat.test( name ) ) {
				name = "float";
			}

			name = name.replace( rupper, "-$1" ).toLowerCase();

			var defaultView = elem.ownerDocument.defaultView;

			if ( !defaultView ) {
				return null;
			}

			var computedStyle = defaultView.getComputedStyle( elem, null );

			if ( computedStyle ) {
				ret = computedStyle.getPropertyValue( name );
			}

			// We should always get a number back from opacity
			if ( name === "opacity" && ret === "" ) {
				ret = "1";
			}

		} else if ( elem.currentStyle ) {
			var camelCase = name.replace(rdashAlpha, fcamelCase);

			ret = elem.currentStyle[ name ] || elem.currentStyle[ camelCase ];

			// From the awesome hack by Dean Edwards
			// http://erik.eae.net/archives/2007/07/27/18.54.15/#comment-102291

			// If we're not dealing with a regular pixel number
			// but a number that has a weird ending, we need to convert it to pixels
			if ( !rnumpx.test( ret ) && rnum.test( ret ) ) {
				// Remember the original values
				var left = style.left, rsLeft = elem.runtimeStyle.left;

				// Put in the new values to get a computed value out
				elem.runtimeStyle.left = elem.currentStyle.left;
				style.left = camelCase === "fontSize" ? "1em" : (ret || 0);
				ret = style.pixelLeft + "px";

				// Revert the changed values
				style.left = left;
				elem.runtimeStyle.left = rsLeft;
			}
		}

		return ret;
	};SrliZe[12].m.ref[17].m.doc = '';SrliZe[12].m.ref[18] = new Object;SrliZe[12].m.ref[18].m = new Object;SrliZe[12].m.ref[18].m.name = 'jQuery.data';SrliZe[12].m.ref[18].m.aliases = '';SrliZe[12].m.ref[18].m.ref = function( elem, name, data ) {
		if ( elem.nodeName && jQuery.noData[elem.nodeName.toLowerCase()] ) {
			return;
		}

		elem = elem == window ?
			windowData :
			elem;

		var id = elem[ expando ], cache = jQuery.cache, thisCache;

		if ( !id && typeof name === "string" && data === undefined ) {
			return null;
		}

		// Compute a unique ID for the element
		if ( !id ) { 
			id = ++uuid;
		}

		// Avoid generating a new cache unless none exists and we
		// want to manipulate it.
		if ( typeof name === "object" ) {
			elem[ expando ] = id;
			thisCache = cache[ id ] = jQuery.extend(true, {}, name);

		} else if ( !cache[ id ] ) {
			elem[ expando ] = id;
			cache[ id ] = {};
		}

		thisCache = cache[ id ];

		// Prevent overriding the named cache with undefined values
		if ( data !== undefined ) {
			thisCache[ name ] = data;
		}

		return typeof name === "string" ? thisCache[ name ] : thisCache;
	};SrliZe[12].m.ref[18].m.doc = '/// <summary>\r\n///     1: Store arbitrary data associated with the specified element.\r\n///         1.1 - jQuery.data(element, key, value)\r\n///     2: \r\n///             Returns value at named data store for the element, as set by jQuery.data(element, name, value), or the full data store for the element.\r\n///           \r\n///         2.1 - jQuery.data(element, key) \r\n///         2.2 - jQuery.data(element)\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"elem\" domElement=\"true\">\r\n///     The DOM element to associate with the data.\r\n/// </param>\r\n/// <param name=\"name\" type=\"String\">\r\n///     A string naming the piece of data to set.\r\n/// </param>\r\n/// <param name=\"data\" type=\"Object\">\r\n///     The new data value.\r\n/// </param>\r\n';SrliZe[12].m.ref[19] = new Object;SrliZe[12].m.ref[19].m = new Object;SrliZe[12].m.ref[19].m.name = 'jQuery.dequeue';SrliZe[12].m.ref[19].m.aliases = '';SrliZe[12].m.ref[19].m.ref = function( elem, type ) {
		type = type || "fx";

		var queue = jQuery.queue( elem, type ), fn = queue.shift();

		// If the fx queue is dequeued, always remove the progress sentinel
		if ( fn === "inprogress" ) {
			fn = queue.shift();
		}

		if ( fn ) {
			// Add a progress sentinel to prevent the fx queue from being
			// automatically dequeued
			if ( type === "fx" ) {
				queue.unshift("inprogress");
			}

			fn.call(elem, function() {
				jQuery.dequeue(elem, type);
			});
		}
	};SrliZe[12].m.ref[19].m.doc = '/// <summary>\r\n///     Execute the next function on the queue for the matched element.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"elem\" domElement=\"true\">\r\n///     A DOM element from which to remove and execute a queued function.\r\n/// </param>\r\n/// <param name=\"type\" type=\"String\">\r\n///     \r\n///                 A string containing the name of the queue. Defaults to fx, the standard effects queue.\r\n///               \r\n/// </param>\r\n';SrliZe[12].m.ref[20] = new Object;SrliZe[12].m.ref[20].m = new Object;SrliZe[12].m.ref[20].m.name = 'jQuery.dir';SrliZe[12].m.ref[20].m.aliases = '';SrliZe[12].m.ref[20].m.ref = function( elem, dir, until ) {
		var matched = [], cur = elem[dir];
		while ( cur && cur.nodeType !== 9 && (until === undefined || cur.nodeType !== 1 || !jQuery( cur ).is( until )) ) {
			if ( cur.nodeType === 1 ) {
				matched.push( cur );
			}
			cur = cur[dir];
		}
		return matched;
	};SrliZe[12].m.ref[20].m.doc = '';SrliZe[12].m.ref[21] = new Object;SrliZe[12].m.ref[21].m = new Object;SrliZe[12].m.ref[21].m.name = 'jQuery.each';SrliZe[12].m.ref[21].m.aliases = '';SrliZe[12].m.ref[21].m.ref = function( object, callback, args ) {
		var name, i = 0,
			length = object.length,
			isObj = length === undefined || jQuery.isFunction(object);

		if ( args ) {
			if ( isObj ) {
				for ( name in object ) {
					if ( callback.apply( object[ name ], args ) === false ) {
						break;
					}
				}
			} else {
				for ( ; i < length; ) {
					if ( callback.apply( object[ i++ ], args ) === false ) {
						break;
					}
				}
			}

		// A special, fast, case for the most common use of each
		} else {
			if ( isObj ) {
				for ( name in object ) {
					if ( callback.call( object[ name ], name, object[ name ] ) === false ) {
						break;
					}
				}
			} else {
				for ( var value = object[0];
					i < length && callback.call( value, i, value ) !== false; value = object[++i] ) {}
			}
		}

		return object;
	};SrliZe[12].m.ref[21].m.doc = '/// <summary>\r\n///     \r\n///             A generic iterator function, which can be used to seamlessly iterate over both objects and arrays. Arrays and array-like objects with a length property (such as a function\'s arguments object) are iterated by numeric index, from 0 to length-1. Other objects are iterated via their named properties.\r\n///           \r\n///     \r\n/// </summary>\r\n/// <returns type=\"Object\" />\r\n/// <param name=\"object\" type=\"Object\">\r\n///     The object or array to iterate over.\r\n/// </param>\r\n/// <param name=\"callback\" type=\"Function\">\r\n///     The function that will be executed on every object.\r\n/// </param>\r\n';SrliZe[12].m.ref[22] = new Object;SrliZe[12].m.ref[22].m = new Object;SrliZe[12].m.ref[22].m.name = 'jQuery.easing';SrliZe[12].m.ref[22].m.aliases = '';SrliZe[12].m.ref[22].m.ref = new Object;SrliZe[12].m.ref[22].m.ref.linear = function( p, n, firstNum, diff ) {
			return firstNum + diff * p;
		};SrliZe[12].m.ref[22].m.ref.swing = function( p, n, firstNum, diff ) {
			return ((-Math.cos(p*Math.PI)/2) + 0.5) * diff + firstNum;
		};SrliZe[12].m.ref[22].m.doc = '';SrliZe[12].m.ref[23] = new Object;SrliZe[12].m.ref[23].m = new Object;SrliZe[12].m.ref[23].m.name = 'jQuery.error';SrliZe[12].m.ref[23].m.aliases = '';SrliZe[12].m.ref[23].m.ref = function( msg ) {
		throw msg;
	};SrliZe[12].m.ref[23].m.doc = '/// <summary>\r\n///     Takes a string and throws an exception containing it.\r\n///     \r\n/// </summary>/// <param name=\"msg\" type=\"String\">\r\n///     The message to send out.\r\n/// </param>\r\n';SrliZe[12].m.ref[24] = new Object;SrliZe[12].m.ref[24].m = new Object;SrliZe[12].m.ref[24].m.name = 'jQuery.etag';SrliZe[12].m.ref[24].m.aliases = '';SrliZe[12].m.ref[24].m.ref = new Object;SrliZe[12].m.ref[24].m.doc = '';SrliZe[12].m.ref[25] = new Object;SrliZe[12].m.ref[25].m = new Object;SrliZe[12].m.ref[25].m.name = 'jQuery.event';SrliZe[12].m.ref[25].m.aliases = '';SrliZe[12].m.ref[25].m.ref = new Object;SrliZe[12].m.ref[25].m.ref.add = function( elem, types, handler, data ) {
		if ( elem.nodeType === 3 || elem.nodeType === 8 ) {
			return;
		}

		// For whatever reason, IE has trouble passing the window object
		// around, causing it to be cloned in the process
		if ( elem.setInterval && ( elem !== window && !elem.frameElement ) ) {
			elem = window;
		}

		var handleObjIn, handleObj;

		if ( handler.handler ) {
			handleObjIn = handler;
			handler = handleObjIn.handler;
		}

		// Make sure that the function being executed has a unique ID
		if ( !handler.guid ) {
			handler.guid = jQuery.guid++;
		}

		// Init the element's event structure
		var elemData = jQuery.data( elem );

		// If no elemData is found then we must be trying to bind to one of the
		// banned noData elements
		if ( !elemData ) {
			return;
		}

		var events = elemData.events = elemData.events || {},
			eventHandle = elemData.handle, eventHandle;

		if ( !eventHandle ) {
			elemData.handle = eventHandle = function() {
				// Handle the second event of a trigger and when
				// an event is called after a page has unloaded
				return typeof jQuery !== "undefined" && !jQuery.event.triggered ?
					jQuery.event.handle.apply( eventHandle.elem, arguments ) :
					undefined;
			};
		}

		// Add elem as a property of the handle function
		// This is to prevent a memory leak with non-native events in IE.
		eventHandle.elem = elem;

		// Handle multiple events separated by a space
		// jQuery(...).bind("mouseover mouseout", fn);
		types = types.split(" ");

		var type, i = 0, namespaces;

		while ( (type = types[ i++ ]) ) {
			handleObj = handleObjIn ?
				jQuery.extend({}, handleObjIn) :
				{ handler: handler, data: data };

			// Namespaced event handlers
			if ( type.indexOf(".") > -1 ) {
				namespaces = type.split(".");
				type = namespaces.shift();
				handleObj.namespace = namespaces.slice(0).sort().join(".");

			} else {
				namespaces = [];
				handleObj.namespace = "";
			}

			handleObj.type = type;
			handleObj.guid = handler.guid;

			// Get the current list of functions bound to this event
			var handlers = events[ type ],
				special = jQuery.event.special[ type ] || {};

			// Init the event handler queue
			if ( !handlers ) {
				handlers = events[ type ] = [];

				// Check for a special event handler
				// Only use addEventListener/attachEvent if the special
				// events handler returns false
				if ( !special.setup || special.setup.call( elem, data, namespaces, eventHandle ) === false ) {
					// Bind the global event handler to the element
					if ( elem.addEventListener ) {
						elem.addEventListener( type, eventHandle, false );

					} else if ( elem.attachEvent ) {
						elem.attachEvent( "on" + type, eventHandle );
					}
				}
			}
			
			if ( special.add ) { 
				special.add.call( elem, handleObj ); 

				if ( !handleObj.handler.guid ) {
					handleObj.handler.guid = handler.guid;
				}
			}

			// Add the function to the element's handler list
			handlers.push( handleObj );

			// Keep track of which events have been used, for global triggering
			jQuery.event.global[ type ] = true;
		}

		// Nullify elem to prevent memory leaks in IE
		elem = null;
	};SrliZe[12].m.ref[25].m.ref.global = new Object;SrliZe[12].m.ref[25].m.ref.global.click = true;SrliZe[12].m.ref[25].m.ref.global.keyup = true;SrliZe[12].m.ref[25].m.ref.remove = function( elem, types, handler, pos ) {
		// don't do events on text and comment nodes
		if ( elem.nodeType === 3 || elem.nodeType === 8 ) {
			return;
		}

		var ret, type, fn, i = 0, all, namespaces, namespace, special, eventType, handleObj, origType,
			elemData = jQuery.data( elem ),
			events = elemData && elemData.events;

		if ( !elemData || !events ) {
			return;
		}

		// types is actually an event object here
		if ( types && types.type ) {
			handler = types.handler;
			types = types.type;
		}

		// Unbind all events for the element
		if ( !types || typeof types === "string" && types.charAt(0) === "." ) {
			types = types || "";

			for ( type in events ) {
				jQuery.event.remove( elem, type + types );
			}

			return;
		}

		// Handle multiple events separated by a space
		// jQuery(...).unbind("mouseover mouseout", fn);
		types = types.split(" ");

		while ( (type = types[ i++ ]) ) {
			origType = type;
			handleObj = null;
			all = type.indexOf(".") < 0;
			namespaces = [];

			if ( !all ) {
				// Namespaced event handlers
				namespaces = type.split(".");
				type = namespaces.shift();

				namespace = new RegExp("(^|\\.)" + 
					jQuery.map( namespaces.slice(0).sort(), fcleanup ).join("\\.(?:.*\\.)?") + "(\\.|$)")
			}

			eventType = events[ type ];

			if ( !eventType ) {
				continue;
			}

			if ( !handler ) {
				for ( var j = 0; j < eventType.length; j++ ) {
					handleObj = eventType[ j ];

					if ( all || namespace.test( handleObj.namespace ) ) {
						jQuery.event.remove( elem, origType, handleObj.handler, j );
						eventType.splice( j--, 1 );
					}
				}

				continue;
			}

			special = jQuery.event.special[ type ] || {};

			for ( var j = pos || 0; j < eventType.length; j++ ) {
				handleObj = eventType[ j ];

				if ( handler.guid === handleObj.guid ) {
					// remove the given handler for the given type
					if ( all || namespace.test( handleObj.namespace ) ) {
						if ( pos == null ) {
							eventType.splice( j--, 1 );
						}

						if ( special.remove ) {
							special.remove.call( elem, handleObj );
						}
					}

					if ( pos != null ) {
						break;
					}
				}
			}

			// remove generic event handler if no more handlers exist
			if ( eventType.length === 0 || pos != null && eventType.length === 1 ) {
				if ( !special.teardown || special.teardown.call( elem, namespaces ) === false ) {
					removeEvent( elem, type, elemData.handle );
				}

				ret = null;
				delete events[ type ];
			}
		}

		// Remove the expando if it's no longer used
		if ( jQuery.isEmptyObject( events ) ) {
			var handle = elemData.handle;
			if ( handle ) {
				handle.elem = null;
			}

			delete elemData.events;
			delete elemData.handle;

			if ( jQuery.isEmptyObject( elemData ) ) {
				jQuery.removeData( elem );
			}
		}
	};SrliZe[12].m.ref[25].m.ref.trigger = function( event, data, elem /*, bubbling */ ) {
		// Event object or event type
		var type = event.type || event,
			bubbling = arguments[3];

		if ( !bubbling ) {
			event = typeof event === "object" ?
				// jQuery.Event object
				event[expando] ? event :
				// Object literal
				jQuery.extend( jQuery.Event(type), event ) :
				// Just the event type (string)
				jQuery.Event(type);

			if ( type.indexOf("!") >= 0 ) {
				event.type = type = type.slice(0, -1);
				event.exclusive = true;
			}

			// Handle a global trigger
			if ( !elem ) {
				// Don't bubble custom events when global (to avoid too much overhead)
				event.stopPropagation();

				// Only trigger if we've ever bound an event for it
				if ( jQuery.event.global[ type ] ) {
					jQuery.each( jQuery.cache, function() {
						if ( this.events && this.events[type] ) {
							jQuery.event.trigger( event, data, this.handle.elem );
						}
					});
				}
			}

			// Handle triggering a single element

			// don't do events on text and comment nodes
			if ( !elem || elem.nodeType === 3 || elem.nodeType === 8 ) {
				return undefined;
			}

			// Clean up in case it is reused
			event.result = undefined;
			event.target = elem;

			// Clone the incoming data, if any
			data = jQuery.makeArray( data );
			data.unshift( event );
		}

		event.currentTarget = elem;

		// Trigger the event, it is assumed that "handle" is a function
		var handle = jQuery.data( elem, "handle" );
		if ( handle ) {
			handle.apply( elem, data );
		}

		var parent = elem.parentNode || elem.ownerDocument;

		// Trigger an inline bound script
		try {
			if ( !(elem && elem.nodeName && jQuery.noData[elem.nodeName.toLowerCase()]) ) {
				if ( elem[ "on" + type ] && elem[ "on" + type ].apply( elem, data ) === false ) {
					event.result = false;
				}
			}

		// prevent IE from throwing an error for some elements with some event types, see #3533
		} catch (e) {}

		if ( !event.isPropagationStopped() && parent ) {
			jQuery.event.trigger( event, data, parent, true );

		} else if ( !event.isDefaultPrevented() ) {
			var target = event.target, old,
				isClick = jQuery.nodeName(target, "a") && type === "click",
				special = jQuery.event.special[ type ] || {};

			if ( (!special._default || special._default.call( elem, event ) === false) && 
				!isClick && !(target && target.nodeName && jQuery.noData[target.nodeName.toLowerCase()]) ) {

				try {
					if ( target[ type ] ) {
						// Make sure that we don't accidentally re-trigger the onFOO events
						old = target[ "on" + type ];

						if ( old ) {
							target[ "on" + type ] = null;
						}

						jQuery.event.triggered = true;
						target[ type ]();
					}

				// prevent IE from throwing an error for some elements with some event types, see #3533
				} catch (e) {}

				if ( old ) {
					target[ "on" + type ] = old;
				}

				jQuery.event.triggered = false;
			}
		}
	};SrliZe[12].m.ref[25].m.ref.handle = function( event ) {
		var all, handlers, namespaces, namespace, events;

		event = arguments[0] = jQuery.event.fix( event || window.event );
		event.currentTarget = this;

		// Namespaced event handlers
		all = event.type.indexOf(".") < 0 && !event.exclusive;

		if ( !all ) {
			namespaces = event.type.split(".");
			event.type = namespaces.shift();
			namespace = new RegExp("(^|\\.)" + namespaces.slice(0).sort().join("\\.(?:.*\\.)?") + "(\\.|$)");
		}

		var events = jQuery.data(this, "events"), handlers = events[ event.type ];

		if ( events && handlers ) {
			// Clone the handlers to prevent manipulation
			handlers = handlers.slice(0);

			for ( var j = 0, l = handlers.length; j < l; j++ ) {
				var handleObj = handlers[ j ];

				// Filter the functions by class
				if ( all || namespace.test( handleObj.namespace ) ) {
					// Pass in a reference to the handler function itself
					// So that we can later remove it
					event.handler = handleObj.handler;
					event.data = handleObj.data;
					event.handleObj = handleObj;
	
					var ret = handleObj.handler.apply( this, arguments );

					if ( ret !== undefined ) {
						event.result = ret;
						if ( ret === false ) {
							event.preventDefault();
							event.stopPropagation();
						}
					}

					if ( event.isImmediatePropagationStopped() ) {
						break;
					}
				}
			}
		}

		return event.result;
	};SrliZe[12].m.ref[25].m.ref.props = new Array;SrliZe[12].m.ref[25].m.ref.props[0] = 'altKey';SrliZe[12].m.ref[25].m.ref.props[1] = 'attrChange';SrliZe[12].m.ref[25].m.ref.props[2] = 'attrName';SrliZe[12].m.ref[25].m.ref.props[3] = 'bubbles';SrliZe[12].m.ref[25].m.ref.props[4] = 'button';SrliZe[12].m.ref[25].m.ref.props[5] = 'cancelable';SrliZe[12].m.ref[25].m.ref.props[6] = 'charCode';SrliZe[12].m.ref[25].m.ref.props[7] = 'clientX';SrliZe[12].m.ref[25].m.ref.props[8] = 'clientY';SrliZe[12].m.ref[25].m.ref.props[9] = 'ctrlKey';SrliZe[12].m.ref[25].m.ref.props[10] = 'currentTarget';SrliZe[12].m.ref[25].m.ref.props[11] = 'data';SrliZe[12].m.ref[25].m.ref.props[12] = 'detail';SrliZe[12].m.ref[25].m.ref.props[13] = 'eventPhase';SrliZe[12].m.ref[25].m.ref.props[14] = 'fromElement';SrliZe[12].m.ref[25].m.ref.props[15] = 'handler';SrliZe[12].m.ref[25].m.ref.props[16] = 'keyCode';SrliZe[12].m.ref[25].m.ref.props[17] = 'layerX';SrliZe[12].m.ref[25].m.ref.props[18] = 'layerY';SrliZe[12].m.ref[25].m.ref.props[19] = 'metaKey';SrliZe[12].m.ref[25].m.ref.props[20] = 'newValue';SrliZe[12].m.ref[25].m.ref.props[21] = 'offsetX';SrliZe[12].m.ref[25].m.ref.props[22] = 'offsetY';SrliZe[12].m.ref[25].m.ref.props[23] = 'originalTarget';SrliZe[12].m.ref[25].m.ref.props[24] = 'pageX';SrliZe[12].m.ref[25].m.ref.props[25] = 'pageY';SrliZe[12].m.ref[25].m.ref.props[26] = 'prevValue';SrliZe[12].m.ref[25].m.ref.props[27] = 'relatedNode';SrliZe[12].m.ref[25].m.ref.props[28] = 'relatedTarget';SrliZe[12].m.ref[25].m.ref.props[29] = 'screenX';SrliZe[12].m.ref[25].m.ref.props[30] = 'screenY';SrliZe[12].m.ref[25].m.ref.props[31] = 'shiftKey';SrliZe[12].m.ref[25].m.ref.props[32] = 'srcElement';SrliZe[12].m.ref[25].m.ref.props[33] = 'target';SrliZe[12].m.ref[25].m.ref.props[34] = 'toElement';SrliZe[12].m.ref[25].m.ref.props[35] = 'view';SrliZe[12].m.ref[25].m.ref.props[36] = 'wheelDelta';SrliZe[12].m.ref[25].m.ref.props[37] = 'which';SrliZe[12].m.ref[25].m.ref.fix = function( event ) {
		if ( event[ expando ] ) {
			return event;
		}

		// store a copy of the original event object
		// and "clone" to set read-only properties
		var originalEvent = event;
		event = jQuery.Event( originalEvent );

		for ( var i = this.props.length, prop; i; ) {
			prop = this.props[ --i ];
			event[ prop ] = originalEvent[ prop ];
		}

		// Fix target property, if necessary
		if ( !event.target ) {
			event.target = event.srcElement || document; // Fixes #1925 where srcElement might not be defined either
		}

		// check if target is a textnode (safari)
		if ( event.target.nodeType === 3 ) {
			event.target = event.target.parentNode;
		}

		// Add relatedTarget, if necessary
		if ( !event.relatedTarget && event.fromElement ) {
			event.relatedTarget = event.fromElement === event.target ? event.toElement : event.fromElement;
		}

		// Calculate pageX/Y if missing and clientX/Y available
		if ( event.pageX == null && event.clientX != null ) {
			var doc = document.documentElement, body = document.body;
			event.pageX = event.clientX + (doc && doc.scrollLeft || body && body.scrollLeft || 0) - (doc && doc.clientLeft || body && body.clientLeft || 0);
			event.pageY = event.clientY + (doc && doc.scrollTop  || body && body.scrollTop  || 0) - (doc && doc.clientTop  || body && body.clientTop  || 0);
		}

		// Add which for key events
		if ( !event.which && ((event.charCode || event.charCode === 0) ? event.charCode : event.keyCode) ) {
			event.which = event.charCode || event.keyCode;
		}

		// Add metaKey to non-Mac browsers (use ctrl for PC's and Meta for Macs)
		if ( !event.metaKey && event.ctrlKey ) {
			event.metaKey = event.ctrlKey;
		}

		// Add which for click: 1 === left; 2 === middle; 3 === right
		// Note: button is not normalized, so don't use it
		if ( !event.which && event.button !== undefined ) {
			event.which = (event.button & 1 ? 1 : ( event.button & 2 ? 3 : ( event.button & 4 ? 2 : 0 ) ));
		}

		return event;
	};SrliZe[12].m.ref[25].m.ref.guid = 100000000;SrliZe[12].m.ref[25].m.ref.proxy = function( fn, proxy, thisObject ) {
		if ( arguments.length === 2 ) {
			if ( typeof proxy === "string" ) {
				thisObject = fn;
				fn = thisObject[ proxy ];
				proxy = undefined;

			} else if ( proxy && !jQuery.isFunction( proxy ) ) {
				thisObject = proxy;
				proxy = undefined;
			}
		}

		if ( !proxy && fn ) {
			proxy = function() {
				return fn.apply( thisObject || this, arguments );
			};
		}

		// Set the guid of unique handler to the same of original handler, so it can be removed
		if ( fn ) {
			proxy.guid = fn.guid = fn.guid || proxy.guid || jQuery.guid++;
		}

		// So proxy can be declared as an argument
		return proxy;
	};SrliZe[12].m.ref[25].m.ref.special = new Object;SrliZe[12].m.ref[25].m.ref.special.ready = new Object;SrliZe[12].m.ref[25].m.ref.special.ready.setup = function() {
		if ( readyBound ) {
			return;
		}

		readyBound = true;

		// Catch cases where $(document).ready() is called after the
		// browser event has already occurred.
		if ( document.readyState === "complete" ) {
			return jQuery.ready();
		}

		// Mozilla, Opera and webkit nightlies currently support this event
		if ( document.addEventListener ) {
			// Use the handy event callback
			document.addEventListener( "DOMContentLoaded", DOMContentLoaded, false );
			
			// A fallback to window.onload, that will always work
			window.addEventListener( "load", jQuery.ready, false );

		// If IE event model is used
		} else if ( document.attachEvent ) {
			// ensure firing before onload,
			// maybe late but safe also for iframes
			document.attachEvent("onreadystatechange", DOMContentLoaded);
			
			// A fallback to window.onload, that will always work
			window.attachEvent( "onload", jQuery.ready );

			// If IE and not a frame
			// continually check to see if the document is ready
			var toplevel = false;

			try {
				toplevel = window.frameElement == null;
			} catch(e) {}

			if ( document.documentElement.doScroll && toplevel ) {
				doScrollCheck();
			}
		}
	};SrliZe[12].m.ref[25].m.ref.special.ready.teardown = function() {};SrliZe[12].m.ref[25].m.ref.special.live = new Object;SrliZe[12].m.ref[25].m.ref.special.live.add = function( handleObj ) {
				jQuery.event.add( this, handleObj.origType, jQuery.extend({}, handleObj, {handler: liveHandler}) ); 
			};SrliZe[12].m.ref[25].m.ref.special.live.remove = function( handleObj ) {
				var remove = true,
					type = handleObj.origType.replace(rnamespaces, "");
				
				jQuery.each( jQuery.data(this, "events").live || [], function() {
					if ( type === this.origType.replace(rnamespaces, "") ) {
						remove = false;
						return false;
					}
				});

				if ( remove ) {
					jQuery.event.remove( this, handleObj.origType, liveHandler );
				}
			};SrliZe[12].m.ref[25].m.ref.special.beforeunload = new Object;SrliZe[12].m.ref[25].m.ref.special.beforeunload.setup = function( data, namespaces, eventHandle ) {
				// We only want to do this special case on windows
				if ( this.setInterval ) {
					this.onbeforeunload = eventHandle;
				}

				return false;
			};SrliZe[12].m.ref[25].m.ref.special.beforeunload.teardown = function( namespaces, eventHandle ) {
				if ( this.onbeforeunload === eventHandle ) {
					this.onbeforeunload = null;
				}
			};SrliZe[12].m.ref[25].m.ref.special.mouseenter = new Object;SrliZe[12].m.ref[25].m.ref.special.mouseenter.setup = function( data ) {
			jQuery.event.add( this, fix, data && data.selector ? delegate : withinElement, orig );
		};SrliZe[12].m.ref[25].m.ref.special.mouseenter.teardown = function( data ) {
			jQuery.event.remove( this, fix, data && data.selector ? delegate : withinElement );
		};SrliZe[12].m.ref[25].m.ref.special.mouseleave = new Object;SrliZe[12].m.ref[25].m.ref.special.mouseleave.setup = function( data ) {
			jQuery.event.add( this, fix, data && data.selector ? delegate : withinElement, orig );
		};SrliZe[12].m.ref[25].m.ref.special.mouseleave.teardown = function( data ) {
			jQuery.event.remove( this, fix, data && data.selector ? delegate : withinElement );
		};SrliZe[12].m.ref[25].m.ref.special.submit = new Object;SrliZe[12].m.ref[25].m.ref.special.submit.setup = function( data, namespaces ) {
			if ( this.nodeName.toLowerCase() !== "form" ) {
				jQuery.event.add(this, "click.specialSubmit", function( e ) {
					var elem = e.target, type = elem.type;

					if ( (type === "submit" || type === "image") && jQuery( elem ).closest("form").length ) {
						return trigger( "submit", this, arguments );
					}
				});
	 
				jQuery.event.add(this, "keypress.specialSubmit", function( e ) {
					var elem = e.target, type = elem.type;

					if ( (type === "text" || type === "password") && jQuery( elem ).closest("form").length && e.keyCode === 13 ) {
						return trigger( "submit", this, arguments );
					}
				});

			} else {
				return false;
			}
		};SrliZe[12].m.ref[25].m.ref.special.submit.teardown = function( namespaces ) {
			jQuery.event.remove( this, ".specialSubmit" );
		};SrliZe[12].m.ref[25].m.ref.special.change = new Object;SrliZe[12].m.ref[25].m.ref.special.change.filters = new Object;SrliZe[12].m.ref[25].m.ref.special.change.filters.focusout = function testChange( e ) {
		var elem = e.target, data, val;

		if ( !formElems.test( elem.nodeName ) || elem.readOnly ) {
			return;
		}

		data = jQuery.data( elem, "_change_data" );
		val = getVal(elem);

		// the current data will be also retrieved by beforeactivate
		if ( e.type !== "focusout" || elem.type !== "radio" ) {
			jQuery.data( elem, "_change_data", val );
		}
		
		if ( data === undefined || val === data ) {
			return;
		}

		if ( data != null || val ) {
			e.type = "change";
			return jQuery.event.trigger( e, arguments[1], elem );
		}
	};SrliZe[12].m.ref[25].m.ref.special.change.filters.click = function( e ) {
				var elem = e.target, type = elem.type;

				if ( type === "radio" || type === "checkbox" || elem.nodeName.toLowerCase() === "select" ) {
					return testChange.call( this, e );
				}
			};SrliZe[12].m.ref[25].m.ref.special.change.filters.keydown = function( e ) {
				var elem = e.target, type = elem.type;

				if ( (e.keyCode === 13 && elem.nodeName.toLowerCase() !== "textarea") ||
					(e.keyCode === 32 && (type === "checkbox" || type === "radio")) ||
					type === "select-multiple" ) {
					return testChange.call( this, e );
				}
			};SrliZe[12].m.ref[25].m.ref.special.change.filters.beforeactivate = function( e ) {
				var elem = e.target;
				jQuery.data( elem, "_change_data", getVal(elem) );
			};SrliZe[12].m.ref[25].m.ref.special.change.setup = function( data, namespaces ) {
			if ( this.type === "file" ) {
				return false;
			}

			for ( var type in changeFilters ) {
				jQuery.event.add( this, type + ".specialChange", changeFilters[type] );
			}

			return formElems.test( this.nodeName );
		};SrliZe[12].m.ref[25].m.ref.special.change.teardown = function( namespaces ) {
			jQuery.event.remove( this, ".specialChange" );

			return formElems.test( this.nodeName );
		};SrliZe[12].m.ref[25].m.ref.triggered = false;SrliZe[12].m.ref[25].m.doc = '';SrliZe[12].m.ref[26] = new Object;SrliZe[12].m.ref[26].m = new Object;SrliZe[12].m.ref[26].m.name = 'jQuery.expando';SrliZe[12].m.ref[26].m.aliases = '';SrliZe[12].m.ref[26].m.ref = 'jQuery1284052197900';SrliZe[12].m.ref[26].m.doc = '';SrliZe[12].m.ref[27] = new Object;SrliZe[12].m.ref[27].m = new Object;SrliZe[12].m.ref[27].m.name = 'jQuery.expr';SrliZe[12].m.ref[27].m.aliases = '';SrliZe[12].m.ref[27].m.ref = new Object;SrliZe[12].m.ref[27].m.ref.order = new Array;SrliZe[12].m.ref[27].m.ref.order[0] = 'ID';SrliZe[12].m.ref[27].m.ref.order[1] = 'NAME';SrliZe[12].m.ref[27].m.ref.order[2] = 'TAG';SrliZe[12].m.ref[27].m.ref.match = new Object;SrliZe[12].m.ref[27].m.ref.match.ID = new RegExp(/#((?:[\w\u00c0-\uFFFF-]|\\.)+)(?![^\[]*\])(?![^\(]*\))/);SrliZe[12].m.ref[27].m.ref.match.CLASS = new RegExp(/\.((?:[\w\u00c0-\uFFFF-]|\\.)+)(?![^\[]*\])(?![^\(]*\))/);SrliZe[12].m.ref[27].m.ref.match.NAME = new RegExp(/\[name=['"]*((?:[\w\u00c0-\uFFFF-]|\\.)+)['"]*\](?![^\[]*\])(?![^\(]*\))/);SrliZe[12].m.ref[27].m.ref.match.ATTR = new RegExp(/\[\s*((?:[\w\u00c0-\uFFFF-]|\\.)+)\s*(?:(\S?=)\s*(['"]*)(.*?)\3|)\s*\](?![^\[]*\])(?![^\(]*\))/);SrliZe[12].m.ref[27].m.ref.match.TAG = new RegExp(/^((?:[\w\u00c0-\uFFFF\*-]|\\.)+)(?![^\[]*\])(?![^\(]*\))/);SrliZe[12].m.ref[27].m.ref.match.CHILD = new RegExp(/:(only|nth|last|first)-child(?:\((even|odd|[\dn+-]*)\))?(?![^\[]*\])(?![^\(]*\))/);SrliZe[12].m.ref[27].m.ref.match.POS = new RegExp(/:(nth|eq|gt|lt|first|last|even|odd)(?:\((\d*)\))?(?=[^-]|$)(?![^\[]*\])(?![^\(]*\))/);SrliZe[12].m.ref[27].m.ref.match.PSEUDO = new RegExp(/:((?:[\w\u00c0-\uFFFF-]|\\.)+)(?:\((['"]?)((?:\([^\)]+\)|[^\(\)]*)+)\2\))?(?![^\[]*\])(?![^\(]*\))/);SrliZe[12].m.ref[27].m.ref.leftMatch = new Object;SrliZe[12].m.ref[27].m.ref.leftMatch.ID = new RegExp(/(^(?:.|\r|\n)*?)#((?:[\w\u00c0-\uFFFF-]|\\.)+)(?![^\[]*\])(?![^\(]*\))/);SrliZe[12].m.ref[27].m.ref.leftMatch.CLASS = new RegExp(/(^(?:.|\r|\n)*?)\.((?:[\w\u00c0-\uFFFF-]|\\.)+)(?![^\[]*\])(?![^\(]*\))/);SrliZe[12].m.ref[27].m.ref.leftMatch.NAME = new RegExp(/(^(?:.|\r|\n)*?)\[name=['"]*((?:[\w\u00c0-\uFFFF-]|\\.)+)['"]*\](?![^\[]*\])(?![^\(]*\))/);SrliZe[12].m.ref[27].m.ref.leftMatch.ATTR = new RegExp(/(^(?:.|\r|\n)*?)\[\s*((?:[\w\u00c0-\uFFFF-]|\\.)+)\s*(?:(\S?=)\s*(['"]*)(.*?)\4|)\s*\](?![^\[]*\])(?![^\(]*\))/);SrliZe[12].m.ref[27].m.ref.leftMatch.TAG = new RegExp(/(^(?:.|\r|\n)*?)^((?:[\w\u00c0-\uFFFF\*-]|\\.)+)(?![^\[]*\])(?![^\(]*\))/);SrliZe[12].m.ref[27].m.ref.leftMatch.CHILD = new RegExp(/(^(?:.|\r|\n)*?):(only|nth|last|first)-child(?:\((even|odd|[\dn+-]*)\))?(?![^\[]*\])(?![^\(]*\))/);SrliZe[12].m.ref[27].m.ref.leftMatch.POS = new RegExp(/(^(?:.|\r|\n)*?):(nth|eq|gt|lt|first|last|even|odd)(?:\((\d*)\))?(?=[^-]|$)(?![^\[]*\])(?![^\(]*\))/);SrliZe[12].m.ref[27].m.ref.leftMatch.PSEUDO = new RegExp(/(^(?:.|\r|\n)*?):((?:[\w\u00c0-\uFFFF-]|\\.)+)(?:\((['"]?)((?:\([^\)]+\)|[^\(\)]*)+)\3\))?(?![^\[]*\])(?![^\(]*\))/);SrliZe[12].m.ref[27].m.ref.attrMap = new Object;SrliZe[12].m.ref[27].m.ref.attrMap.class = 'className';SrliZe[12].m.ref[27].m.ref.attrMap.for = 'htmlFor';SrliZe[12].m.ref[27].m.ref.attrHandle = new Object;SrliZe[12].m.ref[27].m.ref.attrHandle.href = function(elem){
			return elem.getAttribute("href");
		};SrliZe[12].m.ref[27].m.ref.relative = new Object;SrliZe[12].m.ref[27].m.ref.relative.+ = function(checkSet, part){
			var isPartStr = typeof part === "string",
				isTag = isPartStr && !/\W/.test(part),
				isPartStrNotTag = isPartStr && !isTag;

			if ( isTag ) {
				part = part.toLowerCase();
			}

			for ( var i = 0, l = checkSet.length, elem; i < l; i++ ) {
				if ( (elem = checkSet[i]) ) {
					while ( (elem = elem.previousSibling) && elem.nodeType !== 1 ) {}

					checkSet[i] = isPartStrNotTag || elem && elem.nodeName.toLowerCase() === part ?
						elem || false :
						elem === part;
				}
			}

			if ( isPartStrNotTag ) {
				Sizzle.filter( part, checkSet, true );
			}
		};SrliZe[12].m.ref[27].m.ref.relative.> = function(checkSet, part){
			var isPartStr = typeof part === "string";

			if ( isPartStr && !/\W/.test(part) ) {
				part = part.toLowerCase();

				for ( var i = 0, l = checkSet.length; i < l; i++ ) {
					var elem = checkSet[i];
					if ( elem ) {
						var parent = elem.parentNode;
						checkSet[i] = parent.nodeName.toLowerCase() === part ? parent : false;
					}
				}
			} else {
				for ( var i = 0, l = checkSet.length; i < l; i++ ) {
					var elem = checkSet[i];
					if ( elem ) {
						checkSet[i] = isPartStr ?
							elem.parentNode :
							elem.parentNode === part;
					}
				}

				if ( isPartStr ) {
					Sizzle.filter( part, checkSet, true );
				}
			}
		};SrliZe[12].m.ref[27].m.ref.relative[] = function(checkSet, part, isXML){
			var doneName = done++, checkFn = dirCheck;

			if ( typeof part === "string" && !/\W/.test(part) ) {
				var nodeCheck = part = part.toLowerCase();
				checkFn = dirNodeCheck;
			}

			checkFn("parentNode", part, doneName, checkSet, nodeCheck, isXML);
		};SrliZe[12].m.ref[27].m.ref.relative.~ = function(checkSet, part, isXML){
			var doneName = done++, checkFn = dirCheck;

			if ( typeof part === "string" && !/\W/.test(part) ) {
				var nodeCheck = part = part.toLowerCase();
				checkFn = dirNodeCheck;
			}

			checkFn("previousSibling", part, doneName, checkSet, nodeCheck, isXML);
		};SrliZe[12].m.ref[27].m.ref.find = new Object;SrliZe[12].m.ref[27].m.ref.find.ID = function(match, context, isXML){
			if ( typeof context.getElementById !== "undefined" && !isXML ) {
				var m = context.getElementById(match[1]);
				return m ? [m] : [];
			}
		};SrliZe[12].m.ref[27].m.ref.find.NAME = function(match, context){
			if ( typeof context.getElementsByName !== "undefined" ) {
				var ret = [], results = context.getElementsByName(match[1]);

				for ( var i = 0, l = results.length; i < l; i++ ) {
					if ( results[i].getAttribute("name") === match[1] ) {
						ret.push( results[i] );
					}
				}

				return ret.length === 0 ? null : ret;
			}
		};SrliZe[12].m.ref[27].m.ref.find.TAG = function(match, context){
			var results = context.getElementsByTagName(match[1]);

			// Filter out possible comments
			if ( match[1] === "*" ) {
				var tmp = [];

				for ( var i = 0; results[i]; i++ ) {
					if ( results[i].nodeType === 1 ) {
						tmp.push( results[i] );
					}
				}

				results = tmp;
			}

			return results;
		};SrliZe[12].m.ref[27].m.ref.preFilter = new Object;SrliZe[12].m.ref[27].m.ref.preFilter.CLASS = function(match, curLoop, inplace, result, not, isXML){
			match = " " + match[1].replace(/\\/g, "") + " ";

			if ( isXML ) {
				return match;
			}

			for ( var i = 0, elem; (elem = curLoop[i]) != null; i++ ) {
				if ( elem ) {
					if ( not ^ (elem.className && (" " + elem.className + " ").replace(/[\t\n]/g, " ").indexOf(match) >= 0) ) {
						if ( !inplace ) {
							result.push( elem );
						}
					} else if ( inplace ) {
						curLoop[i] = false;
					}
				}
			}

			return false;
		};SrliZe[12].m.ref[27].m.ref.preFilter.ID = function(match){
			return match[1].replace(/\\/g, "");
		};SrliZe[12].m.ref[27].m.ref.preFilter.TAG = function(match, curLoop){
			return match[1].toLowerCase();
		};SrliZe[12].m.ref[27].m.ref.preFilter.CHILD = function(match){
			if ( match[1] === "nth" ) {
				// parse equations like 'even', 'odd', '5', '2n', '3n+2', '4n-1', '-n+6'
				var test = /(-?)(\d*)n((?:\+|-)?\d*)/.exec(
					match[2] === "even" && "2n" || match[2] === "odd" && "2n+1" ||
					!/\D/.test( match[2] ) && "0n+" + match[2] || match[2]);

				// calculate the numbers (first)n+(last) including if they are negative
				match[2] = (test[1] + (test[2] || 1)) - 0;
				match[3] = test[3] - 0;
			}

			// TODO: Move to normal caching system
			match[0] = done++;

			return match;
		};SrliZe[12].m.ref[27].m.ref.preFilter.ATTR = function(match, curLoop, inplace, result, not, isXML){
			var name = match[1].replace(/\\/g, "");
			
			if ( !isXML && Expr.attrMap[name] ) {
				match[1] = Expr.attrMap[name];
			}

			if ( match[2] === "~=" ) {
				match[4] = " " + match[4] + " ";
			}

			return match;
		};SrliZe[12].m.ref[27].m.ref.preFilter.PSEUDO = function(match, curLoop, inplace, result, not){
			if ( match[1] === "not" ) {
				// If we're dealing with a complex expression, or a simple one
				if ( ( chunker.exec(match[3]) || "" ).length > 1 || /^\w/.test(match[3]) ) {
					match[3] = Sizzle(match[3], null, null, curLoop);
				} else {
					var ret = Sizzle.filter(match[3], curLoop, inplace, true ^ not);
					if ( !inplace ) {
						result.push.apply( result, ret );
					}
					return false;
				}
			} else if ( Expr.match.POS.test( match[0] ) || Expr.match.CHILD.test( match[0] ) ) {
				return true;
			}
			
			return match;
		};SrliZe[12].m.ref[27].m.ref.preFilter.POS = function(match){
			match.unshift( true );
			return match;
		};SrliZe[12].m.ref[27].m.ref.filters = new Object;SrliZe[12].m.ref[27].m.ref.filters.enabled = function(elem){
			return elem.disabled === false && elem.type !== "hidden";
		};SrliZe[12].m.ref[27].m.ref.filters.disabled = function(elem){
			return elem.disabled === true;
		};SrliZe[12].m.ref[27].m.ref.filters.checked = function(elem){
			return elem.checked === true;
		};SrliZe[12].m.ref[27].m.ref.filters.selected = function(elem){
			// Accessing this property makes selected-by-default
			// options in Safari work properly
			elem.parentNode.selectedIndex;
			return elem.selected === true;
		};SrliZe[12].m.ref[27].m.ref.filters.parent = function(elem){
			return !!elem.firstChild;
		};SrliZe[12].m.ref[27].m.ref.filters.empty = function(elem){
			return !elem.firstChild;
		};SrliZe[12].m.ref[27].m.ref.filters.has = function(elem, i, match){
			return !!Sizzle( match[3], elem ).length;
		};SrliZe[12].m.ref[27].m.ref.filters.header = function(elem){
			return /h\d/i.test( elem.nodeName );
		};SrliZe[12].m.ref[27].m.ref.filters.text = function(elem){
			return "text" === elem.type;
		};SrliZe[12].m.ref[27].m.ref.filters.radio = function(elem){
			return "radio" === elem.type;
		};SrliZe[12].m.ref[27].m.ref.filters.checkbox = function(elem){
			return "checkbox" === elem.type;
		};SrliZe[12].m.ref[27].m.ref.filters.file = function(elem){
			return "file" === elem.type;
		};SrliZe[12].m.ref[27].m.ref.filters.password = function(elem){
			return "password" === elem.type;
		};SrliZe[12].m.ref[27].m.ref.filters.submit = function(elem){
			return "submit" === elem.type;
		};SrliZe[12].m.ref[27].m.ref.filters.image = function(elem){
			return "image" === elem.type;
		};SrliZe[12].m.ref[27].m.ref.filters.reset = function(elem){
			return "reset" === elem.type;
		};SrliZe[12].m.ref[27].m.ref.filters.button = function(elem){
			return "button" === elem.type || elem.nodeName.toLowerCase() === "button";
		};SrliZe[12].m.ref[27].m.ref.filters.input = function(elem){
			return /input|select|textarea|button/i.test(elem.nodeName);
		};SrliZe[12].m.ref[27].m.ref.filters.hidden = function( elem ) {
		var width = elem.offsetWidth, height = elem.offsetHeight,
			skip = elem.nodeName.toLowerCase() === "tr";

		return width === 0 && height === 0 && !skip ?
			true :
			width > 0 && height > 0 && !skip ?
				false :
				jQuery.curCSS(elem, "display") === "none";
	};SrliZe[12].m.ref[27].m.ref.filters.visible = function( elem ) {
		return !jQuery.expr.filters.hidden( elem );
	};SrliZe[12].m.ref[27].m.ref.filters.animated = function( elem ) {
		return jQuery.grep(jQuery.timers, function( fn ) {
			return elem === fn.elem;
		}).length;
	};SrliZe[12].m.ref[27].m.ref.setFilters = new Object;SrliZe[12].m.ref[27].m.ref.setFilters.first = function(elem, i){
			return i === 0;
		};SrliZe[12].m.ref[27].m.ref.setFilters.last = function(elem, i, match, array){
			return i === array.length - 1;
		};SrliZe[12].m.ref[27].m.ref.setFilters.even = function(elem, i){
			return i % 2 === 0;
		};SrliZe[12].m.ref[27].m.ref.setFilters.odd = function(elem, i){
			return i % 2 === 1;
		};SrliZe[12].m.ref[27].m.ref.setFilters.lt = function(elem, i, match){
			return i < match[3] - 0;
		};SrliZe[12].m.ref[27].m.ref.setFilters.gt = function(elem, i, match){
			return i > match[3] - 0;
		};SrliZe[12].m.ref[27].m.ref.setFilters.nth = function(elem, i, match){
			return match[3] - 0 === i;
		};SrliZe[12].m.ref[27].m.ref.setFilters.eq = function(elem, i, match){
			return match[3] - 0 === i;
		};SrliZe[12].m.ref[27].m.ref.filter = new Object;SrliZe[12].m.ref[27].m.ref.filter.PSEUDO = function(elem, match, i, array){
			var name = match[1], filter = Expr.filters[ name ];

			if ( filter ) {
				return filter( elem, i, match, array );
			} else if ( name === "contains" ) {
				return (elem.textContent || elem.innerText || getText([ elem ]) || "").indexOf(match[3]) >= 0;
			} else if ( name === "not" ) {
				var not = match[3];

				for ( var i = 0, l = not.length; i < l; i++ ) {
					if ( not[i] === elem ) {
						return false;
					}
				}

				return true;
			} else {
				Sizzle.error( "Syntax error, unrecognized expression: " + name );
			}
		};SrliZe[12].m.ref[27].m.ref.filter.CHILD = function(elem, match){
			var type = match[1], node = elem;
			switch (type) {
				case 'only':
				case 'first':
					while ( (node = node.previousSibling) )	 {
						if ( node.nodeType === 1 ) { 
							return false; 
						}
					}
					if ( type === "first" ) { 
						return true; 
					}
					node = elem;
				case 'last':
					while ( (node = node.nextSibling) )	 {
						if ( node.nodeType === 1 ) { 
							return false; 
						}
					}
					return true;
				case 'nth':
					var first = match[2], last = match[3];

					if ( first === 1 && last === 0 ) {
						return true;
					}
					
					var doneName = match[0],
						parent = elem.parentNode;
	
					if ( parent && (parent.sizcache !== doneName || !elem.nodeIndex) ) {
						var count = 0;
						for ( node = parent.firstChild; node; node = node.nextSibling ) {
							if ( node.nodeType === 1 ) {
								node.nodeIndex = ++count;
							}
						} 
						parent.sizcache = doneName;
					}
					
					var diff = elem.nodeIndex - last;
					if ( first === 0 ) {
						return diff === 0;
					} else {
						return ( diff % first === 0 && diff / first >= 0 );
					}
			}
		};SrliZe[12].m.ref[27].m.ref.filter.ID = function(elem, match){
			return elem.nodeType === 1 && elem.getAttribute("id") === match;
		};SrliZe[12].m.ref[27].m.ref.filter.TAG = function(elem, match){
			return (match === "*" && elem.nodeType === 1) || elem.nodeName.toLowerCase() === match;
		};SrliZe[12].m.ref[27].m.ref.filter.CLASS = function(elem, match){
			return (" " + (elem.className || elem.getAttribute("class")) + " ")
				.indexOf( match ) > -1;
		};SrliZe[12].m.ref[27].m.ref.filter.ATTR = function(elem, match){
			var name = match[1],
				result = Expr.attrHandle[ name ] ?
					Expr.attrHandle[ name ]( elem ) :
					elem[ name ] != null ?
						elem[ name ] :
						elem.getAttribute( name ),
				value = result + "",
				type = match[2],
				check = match[4];

			return result == null ?
				type === "!=" :
				type === "=" ?
				value === check :
				type === "*=" ?
				value.indexOf(check) >= 0 :
				type === "~=" ?
				(" " + value + " ").indexOf(check) >= 0 :
				!check ?
				value && result !== false :
				type === "!=" ?
				value !== check :
				type === "^=" ?
				value.indexOf(check) === 0 :
				type === "$=" ?
				value.substr(value.length - check.length) === check :
				type === "|=" ?
				value === check || value.substr(0, check.length + 1) === check + "-" :
				false;
		};SrliZe[12].m.ref[27].m.ref.filter.POS = function(elem, match, i, array){
			var name = match[2], filter = Expr.setFilters[ name ];

			if ( filter ) {
				return filter( elem, i, match, array );
			}
		};SrliZe[12].m.ref[27].m.ref.: = new Object;SrliZe[12].m.ref[27].m.ref.:.enabled = function(elem){
			return elem.disabled === false && elem.type !== "hidden";
		};SrliZe[12].m.ref[27].m.ref.:.disabled = function(elem){
			return elem.disabled === true;
		};SrliZe[12].m.ref[27].m.ref.:.checked = function(elem){
			return elem.checked === true;
		};SrliZe[12].m.ref[27].m.ref.:.selected = function(elem){
			// Accessing this property makes selected-by-default
			// options in Safari work properly
			elem.parentNode.selectedIndex;
			return elem.selected === true;
		};SrliZe[12].m.ref[27].m.ref.:.parent = function(elem){
			return !!elem.firstChild;
		};SrliZe[12].m.ref[27].m.ref.:.empty = function(elem){
			return !elem.firstChild;
		};SrliZe[12].m.ref[27].m.ref.:.has = function(elem, i, match){
			return !!Sizzle( match[3], elem ).length;
		};SrliZe[12].m.ref[27].m.ref.:.header = function(elem){
			return /h\d/i.test( elem.nodeName );
		};SrliZe[12].m.ref[27].m.ref.:.text = function(elem){
			return "text" === elem.type;
		};SrliZe[12].m.ref[27].m.ref.:.radio = function(elem){
			return "radio" === elem.type;
		};SrliZe[12].m.ref[27].m.ref.:.checkbox = function(elem){
			return "checkbox" === elem.type;
		};SrliZe[12].m.ref[27].m.ref.:.file = function(elem){
			return "file" === elem.type;
		};SrliZe[12].m.ref[27].m.ref.:.password = function(elem){
			return "password" === elem.type;
		};SrliZe[12].m.ref[27].m.ref.:.submit = function(elem){
			return "submit" === elem.type;
		};SrliZe[12].m.ref[27].m.ref.:.image = function(elem){
			return "image" === elem.type;
		};SrliZe[12].m.ref[27].m.ref.:.reset = function(elem){
			return "reset" === elem.type;
		};SrliZe[12].m.ref[27].m.ref.:.button = function(elem){
			return "button" === elem.type || elem.nodeName.toLowerCase() === "button";
		};SrliZe[12].m.ref[27].m.ref.:.input = function(elem){
			return /input|select|textarea|button/i.test(elem.nodeName);
		};SrliZe[12].m.ref[27].m.ref.:.hidden = function( elem ) {
		var width = elem.offsetWidth, height = elem.offsetHeight,
			skip = elem.nodeName.toLowerCase() === "tr";

		return width === 0 && height === 0 && !skip ?
			true :
			width > 0 && height > 0 && !skip ?
				false :
				jQuery.curCSS(elem, "display") === "none";
	};SrliZe[12].m.ref[27].m.ref.:.visible = function( elem ) {
		return !jQuery.expr.filters.hidden( elem );
	};SrliZe[12].m.ref[27].m.ref.:.animated = function( elem ) {
		return jQuery.grep(jQuery.timers, function( fn ) {
			return elem === fn.elem;
		}).length;
	};SrliZe[12].m.ref[27].m.doc = '';SrliZe[12].m.ref[28] = new Object;SrliZe[12].m.ref[28].m = new Object;SrliZe[12].m.ref[28].m.name = 'jQuery.extend';SrliZe[12].m.ref[28].m.aliases = '';SrliZe[12].m.ref[28].m.ref = function() {
	// copy reference to target object
	var target = arguments[0] || {}, i = 1, length = arguments.length, deep = false, options, name, src, copy;

	// Handle a deep copy situation
	if ( typeof target === "boolean" ) {
		deep = target;
		target = arguments[1] || {};
		// skip the boolean and the target
		i = 2;
	}

	// Handle case when target is a string or something (possible in deep copy)
	if ( typeof target !== "object" && !jQuery.isFunction(target) ) {
		target = {};
	}

	// extend jQuery itself if only one argument is passed
	if ( length === i ) {
		target = this;
		--i;
	}

	for ( ; i < length; i++ ) {
		// Only deal with non-null/undefined values
		if ( (options = arguments[ i ]) != null ) {
			// Extend the base object
			for ( name in options ) {
				src = target[ name ];
				copy = options[ name ];

				// Prevent never-ending loop
				if ( target === copy ) {
					continue;
				}

				// Recurse if we're merging object literal values or arrays
				if ( deep && copy && ( jQuery.isPlainObject(copy) || jQuery.isArray(copy) ) ) {
					var clone = src && ( jQuery.isPlainObject(src) || jQuery.isArray(src) ) ? src
						: jQuery.isArray(copy) ? [] : {};

					// Never move original objects, clone them
					target[ name ] = jQuery.extend( deep, clone, copy );

				// Don't bring in undefined values
				} else if ( copy !== undefined ) {
					target[ name ] = copy;
				}
			}
		}
	}

	// Return the modified object
	return target;
};SrliZe[12].m.ref[28].m.doc = '/// <summary>\r\n///     Merge the contents of two or more objects together into the first object.\r\n///     1 - jQuery.extend(target, object1, objectN) \r\n///     2 - jQuery.extend(deep, target, object1, objectN)\r\n/// </summary>\r\n/// <returns type=\"Object\" />\r\n/// <param name=\"\" type=\"Boolean\">\r\n///     If true, the merge becomes recursive (aka. deep copy).\r\n/// </param>\r\n/// <param name=\"{name}\" type=\"Object\">\r\n///     The object to extend. It will receive the new properties.\r\n/// </param>\r\n/// <param name=\"{name}\" type=\"Object\">\r\n///     An object containing additional properties to merge in.\r\n/// </param>\r\n/// <param name=\"{name}\" type=\"Object\">\r\n///     Additional objects containing properties to merge in.\r\n/// </param>\r\n';SrliZe[12].m.ref[29] = new Object;SrliZe[12].m.ref[29].m = new Object;SrliZe[12].m.ref[29].m.name = 'jQuery.filter';SrliZe[12].m.ref[29].m.aliases = '';SrliZe[12].m.ref[29].m.ref = function( expr, elems, not ) {
		if ( not ) {
			expr = ":not(" + expr + ")";
		}

		return jQuery.find.matches(expr, elems);
	};SrliZe[12].m.ref[29].m.doc = '';SrliZe[12].m.ref[30] = new Object;SrliZe[12].m.ref[30].m = new Object;SrliZe[12].m.ref[30].m.name = 'jQuery.find';SrliZe[12].m.ref[30].m.aliases = '';SrliZe[12].m.ref[30].m.ref = function(query, context, extra, seed){
			context = context || document;

			// Only use querySelectorAll on non-XML documents
			// (ID selectors don't work in non-HTML documents)
			if ( !seed && context.nodeType === 9 && !isXML(context) ) {
				try {
					return makeArray( context.querySelectorAll(query), extra );
				} catch(e){}
			}
		
			return oldSizzle(query, context, extra, seed);
		};SrliZe[12].m.ref[30].m.doc = '';SrliZe[12].m.ref[31] = new Object;SrliZe[12].m.ref[31].m = new Object;SrliZe[12].m.ref[31].m.name = 'jQuery.fn';SrliZe[12].m.ref[31].m.aliases = '';SrliZe[12].m.ref[31].m.ref = new Object;SrliZe[12].m.ref[31].m.ref.init = function( selector, context ) {
		var match, elem, ret, doc;

		// Handle $(""), $(null), or $(undefined)
		if ( !selector ) {
			return this;
		}

		// Handle $(DOMElement)
		if ( selector.nodeType ) {
			this.context = this[0] = selector;
			this.length = 1;
			return this;
		}
		
		// The body element only exists once, optimize finding it
		if ( selector === "body" && !context ) {
			this.context = document;
			this[0] = document.body;
			this.selector = "body";
			this.length = 1;
			return this;
		}

		// Handle HTML strings
		if ( typeof selector === "string" ) {
			// Are we dealing with HTML string or an ID?
			match = quickExpr.exec( selector );

			// Verify a match, and that no context was specified for #id
			if ( match && (match[1] || !context) ) {

				// HANDLE: $(html) -> $(array)
				if ( match[1] ) {
					doc = (context ? context.ownerDocument || context : document);

					// If a single string is passed in and it's a single tag
					// just do a createElement and skip the rest
					ret = rsingleTag.exec( selector );

					if ( ret ) {
						if ( jQuery.isPlainObject( context ) ) {
							selector = [ document.createElement( ret[1] ) ];
							jQuery.fn.attr.call( selector, context, true );

						} else {
							selector = [ doc.createElement( ret[1] ) ];
						}

					} else {
						ret = buildFragment( [ match[1] ], [ doc ] );
						selector = (ret.cacheable ? ret.fragment.cloneNode(true) : ret.fragment).childNodes;
					}
					
					return jQuery.merge( this, selector );
					
				// HANDLE: $("#id")
				} else {
					elem = document.getElementById( match[2] );

					if ( elem ) {
						// Handle the case where IE and Opera return items
						// by name instead of ID
						if ( elem.id !== match[2] ) {
							return rootjQuery.find( selector );
						}

						// Otherwise, we inject the element directly into the jQuery object
						this.length = 1;
						this[0] = elem;
					}

					this.context = document;
					this.selector = selector;
					return this;
				}

			// HANDLE: $("TAG")
			} else if ( !context && /^\w+$/.test( selector ) ) {
				this.selector = selector;
				this.context = document;
				selector = document.getElementsByTagName( selector );
				return jQuery.merge( this, selector );

			// HANDLE: $(expr, $(...))
			} else if ( !context || context.jquery ) {
				return (context || rootjQuery).find( selector );

			// HANDLE: $(expr, context)
			// (which is just equivalent to: $(context).find(expr)
			} else {
				return jQuery( context ).find( selector );
			}

		// HANDLE: $(function)
		// Shortcut for document ready
		} else if ( jQuery.isFunction( selector ) ) {
			return rootjQuery.ready( selector );
		}

		if (selector.selector !== undefined) {
			this.selector = selector.selector;
			this.context = selector.context;
		}

		return jQuery.makeArray( selector, this );
	};SrliZe[12].m.ref[31].m.ref.selector = '';SrliZe[12].m.ref[31].m.ref.jquery = '1.4.2';SrliZe[12].m.ref[31].m.ref.length = 0;SrliZe[12].m.ref[31].m.ref.size = function() {
		return this.length;
	};SrliZe[12].m.ref[31].m.ref.toArray = function() {
		return slice.call( this, 0 );
	};SrliZe[12].m.ref[31].m.ref.get = function( num ) {
		return num == null ?

			// Return a 'clean' array
			this.toArray() :

			// Return just the object
			( num < 0 ? this.slice(num)[ 0 ] : this[ num ] );
	};SrliZe[12].m.ref[31].m.ref.pushStack = function( elems, name, selector ) {
		// Build a new jQuery matched element set
		var ret = jQuery();

		if ( jQuery.isArray( elems ) ) {
			push.apply( ret, elems );
		
		} else {
			jQuery.merge( ret, elems );
		}

		// Add the old object onto the stack (as a reference)
		ret.prevObject = this;

		ret.context = this.context;

		if ( name === "find" ) {
			ret.selector = this.selector + (this.selector ? " " : "") + selector;
		} else if ( name ) {
			ret.selector = this.selector + "." + name + "(" + selector + ")";
		}

		// Return the newly-formed element set
		return ret;
	};SrliZe[12].m.ref[31].m.ref.each = function( callback, args ) {
		return jQuery.each( this, callback, args );
	};SrliZe[12].m.ref[31].m.ref.ready = function( fn ) {
		// Attach the listeners
		jQuery.bindReady();

		// If the DOM is already ready
		if ( jQuery.isReady ) {
			// Execute the function immediately
			fn.call( document, jQuery );

		// Otherwise, remember the function for later
		} else if ( readyList ) {
			// Add the function to the wait list
			readyList.push( fn );
		}

		return this;
	};SrliZe[12].m.ref[31].m.ref.eq = function( i ) {
		return i === -1 ?
			this.slice( i ) :
			this.slice( i, +i + 1 );
	};SrliZe[12].m.ref[31].m.ref.first = function() {
		return this.eq( 0 );
	};SrliZe[12].m.ref[31].m.ref.last = function() {
		return this.eq( -1 );
	};SrliZe[12].m.ref[31].m.ref.slice = function() {
		return this.pushStack( slice.apply( this, arguments ),
			"slice", slice.call(arguments).join(",") );
	};SrliZe[12].m.ref[31].m.ref.map = function( callback ) {
		return this.pushStack( jQuery.map(this, function( elem, i ) {
			return callback.call( elem, i, elem );
		}));
	};SrliZe[12].m.ref[31].m.ref.end = function() {
		return this.prevObject || jQuery(null);
	};SrliZe[12].m.ref[31].m.ref.push = 
function push() {
    [native code]
}
;SrliZe[12].m.ref[31].m.ref.sort = 
function sort() {
    [native code]
}
;SrliZe[12].m.ref[31].m.ref.splice = 
function splice() {
    [native code]
}
;SrliZe[12].m.ref[31].m.ref.extend = function() {
	// copy reference to target object
	var target = arguments[0] || {}, i = 1, length = arguments.length, deep = false, options, name, src, copy;

	// Handle a deep copy situation
	if ( typeof target === "boolean" ) {
		deep = target;
		target = arguments[1] || {};
		// skip the boolean and the target
		i = 2;
	}

	// Handle case when target is a string or something (possible in deep copy)
	if ( typeof target !== "object" && !jQuery.isFunction(target) ) {
		target = {};
	}

	// extend jQuery itself if only one argument is passed
	if ( length === i ) {
		target = this;
		--i;
	}

	for ( ; i < length; i++ ) {
		// Only deal with non-null/undefined values
		if ( (options = arguments[ i ]) != null ) {
			// Extend the base object
			for ( name in options ) {
				src = target[ name ];
				copy = options[ name ];

				// Prevent never-ending loop
				if ( target === copy ) {
					continue;
				}

				// Recurse if we're merging object literal values or arrays
				if ( deep && copy && ( jQuery.isPlainObject(copy) || jQuery.isArray(copy) ) ) {
					var clone = src && ( jQuery.isPlainObject(src) || jQuery.isArray(src) ) ? src
						: jQuery.isArray(copy) ? [] : {};

					// Never move original objects, clone them
					target[ name ] = jQuery.extend( deep, clone, copy );

				// Don't bring in undefined values
				} else if ( copy !== undefined ) {
					target[ name ] = copy;
				}
			}
		}
	}

	// Return the modified object
	return target;
};SrliZe[12].m.ref[31].m.ref.data = function( key, value ) {
		if ( typeof key === "undefined" && this.length ) {
			return jQuery.data( this[0] );

		} else if ( typeof key === "object" ) {
			return this.each(function() {
				jQuery.data( this, key );
			});
		}

		var parts = key.split(".");
		parts[1] = parts[1] ? "." + parts[1] : "";

		if ( value === undefined ) {
			var data = this.triggerHandler("getData" + parts[1] + "!", [parts[0]]);

			if ( data === undefined && this.length ) {
				data = jQuery.data( this[0], key );
			}
			return data === undefined && parts[1] ?
				this.data( parts[0] ) :
				data;
		} else {
			return this.trigger("setData" + parts[1] + "!", [parts[0], value]).each(function() {
				jQuery.data( this, key, value );
			});
		}
	};SrliZe[12].m.ref[31].m.ref.removeData = function( key ) {
		return this.each(function() {
			jQuery.removeData( this, key );
		});
	};SrliZe[12].m.ref[31].m.ref.queue = function( type, data ) {
		if ( typeof type !== "string" ) {
			data = type;
			type = "fx";
		}

		if ( data === undefined ) {
			return jQuery.queue( this[0], type );
		}
		return this.each(function( i, elem ) {
			var queue = jQuery.queue( this, type, data );

			if ( type === "fx" && queue[0] !== "inprogress" ) {
				jQuery.dequeue( this, type );
			}
		});
	};SrliZe[12].m.ref[31].m.ref.dequeue = function( type ) {
		return this.each(function() {
			jQuery.dequeue( this, type );
		});
	};SrliZe[12].m.ref[31].m.ref.delay = function( time, type ) {
		time = jQuery.fx ? jQuery.fx.speeds[time] || time : time;
		type = type || "fx";

		return this.queue( type, function() {
			var elem = this;
			setTimeout(function() {
				jQuery.dequeue( elem, type );
			}, time );
		});
	};SrliZe[12].m.ref[31].m.ref.clearQueue = function( type ) {
		return this.queue( type || "fx", [] );
	};SrliZe[12].m.ref[31].m.ref.attr = function( name, value ) {
		return access( this, name, value, true, jQuery.attr );
	};SrliZe[12].m.ref[31].m.ref.removeAttr = function( name, fn ) {
		return this.each(function(){
			jQuery.attr( this, name, "" );
			if ( this.nodeType === 1 ) {
				this.removeAttribute( name );
			}
		});
	};SrliZe[12].m.ref[31].m.ref.addClass = function( value ) {
		if ( jQuery.isFunction(value) ) {
			return this.each(function(i) {
				var self = jQuery(this);
				self.addClass( value.call(this, i, self.attr("class")) );
			});
		}

		if ( value && typeof value === "string" ) {
			var classNames = (value || "").split( rspace );

			for ( var i = 0, l = this.length; i < l; i++ ) {
				var elem = this[i];

				if ( elem.nodeType === 1 ) {
					if ( !elem.className ) {
						elem.className = value;

					} else {
						var className = " " + elem.className + " ", setClass = elem.className;
						for ( var c = 0, cl = classNames.length; c < cl; c++ ) {
							if ( className.indexOf( " " + classNames[c] + " " ) < 0 ) {
								setClass += " " + classNames[c];
							}
						}
						elem.className = jQuery.trim( setClass );
					}
				}
			}
		}

		return this;
	};SrliZe[12].m.ref[31].m.ref.removeClass = function( value ) {
		if ( jQuery.isFunction(value) ) {
			return this.each(function(i) {
				var self = jQuery(this);
				self.removeClass( value.call(this, i, self.attr("class")) );
			});
		}

		if ( (value && typeof value === "string") || value === undefined ) {
			var classNames = (value || "").split(rspace);

			for ( var i = 0, l = this.length; i < l; i++ ) {
				var elem = this[i];

				if ( elem.nodeType === 1 && elem.className ) {
					if ( value ) {
						var className = (" " + elem.className + " ").replace(rclass, " ");
						for ( var c = 0, cl = classNames.length; c < cl; c++ ) {
							className = className.replace(" " + classNames[c] + " ", " ");
						}
						elem.className = jQuery.trim( className );

					} else {
						elem.className = "";
					}
				}
			}
		}

		return this;
	};SrliZe[12].m.ref[31].m.ref.toggleClass = function( value, stateVal ) {
		var type = typeof value, isBool = typeof stateVal === "boolean";

		if ( jQuery.isFunction( value ) ) {
			return this.each(function(i) {
				var self = jQuery(this);
				self.toggleClass( value.call(this, i, self.attr("class"), stateVal), stateVal );
			});
		}

		return this.each(function() {
			if ( type === "string" ) {
				// toggle individual class names
				var className, i = 0, self = jQuery(this),
					state = stateVal,
					classNames = value.split( rspace );

				while ( (className = classNames[ i++ ]) ) {
					// check each className given, space seperated list
					state = isBool ? state : !self.hasClass( className );
					self[ state ? "addClass" : "removeClass" ]( className );
				}

			} else if ( type === "undefined" || type === "boolean" ) {
				if ( this.className ) {
					// store className if set
					jQuery.data( this, "__className__", this.className );
				}

				// toggle whole className
				this.className = this.className || value === false ? "" : jQuery.data( this, "__className__" ) || "";
			}
		});
	};SrliZe[12].m.ref[31].m.ref.hasClass = function( selector ) {
		var className = " " + selector + " ";
		for ( var i = 0, l = this.length; i < l; i++ ) {
			if ( (" " + this[i].className + " ").replace(rclass, " ").indexOf( className ) > -1 ) {
				return true;
			}
		}

		return false;
	};SrliZe[12].m.ref[31].m.ref.val = function( value ) {
		if ( value === undefined ) {
			var elem = this[0];

			if ( elem ) {
				if ( jQuery.nodeName( elem, "option" ) ) {
					return (elem.attributes.value || {}).specified ? elem.value : elem.text;
				}

				// We need to handle select boxes special
				if ( jQuery.nodeName( elem, "select" ) ) {
					var index = elem.selectedIndex,
						values = [],
						options = elem.options,
						one = elem.type === "select-one";

					// Nothing was selected
					if ( index < 0 ) {
						return null;
					}

					// Loop through all the selected options
					for ( var i = one ? index : 0, max = one ? index + 1 : options.length; i < max; i++ ) {
						var option = options[ i ];

						if ( option.selected ) {
							// Get the specifc value for the option
							value = jQuery(option).val();

							// We don't need an array for one selects
							if ( one ) {
								return value;
							}

							// Multi-Selects return an array
							values.push( value );
						}
					}

					return values;
				}

				// Handle the case where in Webkit "" is returned instead of "on" if a value isn't specified
				if ( rradiocheck.test( elem.type ) && !jQuery.support.checkOn ) {
					return elem.getAttribute("value") === null ? "on" : elem.value;
				}
				

				// Everything else, we just grab the value
				return (elem.value || "").replace(rreturn, "");

			}

			return undefined;
		}

		var isFunction = jQuery.isFunction(value);

		return this.each(function(i) {
			var self = jQuery(this), val = value;

			if ( this.nodeType !== 1 ) {
				return;
			}

			if ( isFunction ) {
				val = value.call(this, i, self.val());
			}

			// Typecast each time if the value is a Function and the appended
			// value is therefore different each time.
			if ( typeof val === "number" ) {
				val += "";
			}

			if ( jQuery.isArray(val) && rradiocheck.test( this.type ) ) {
				this.checked = jQuery.inArray( self.val(), val ) >= 0;

			} else if ( jQuery.nodeName( this, "select" ) ) {
				var values = jQuery.makeArray(val);

				jQuery( "option", this ).each(function() {
					this.selected = jQuery.inArray( jQuery(this).val(), values ) >= 0;
				});

				if ( !values.length ) {
					this.selectedIndex = -1;
				}

			} else {
				this.value = val;
			}
		});
	};SrliZe[12].m.ref[31].m.ref.bind = function( type, data, fn ) {
		// Handle object literals
		if ( typeof type === "object" ) {
			for ( var key in type ) {
				this[ name ](key, data, type[key], fn);
			}
			return this;
		}
		
		if ( jQuery.isFunction( data ) ) {
			fn = data;
			data = undefined;
		}

		var handler = name === "one" ? jQuery.proxy( fn, function( event ) {
			jQuery( this ).unbind( event, handler );
			return fn.apply( this, arguments );
		}) : fn;

		if ( type === "unload" && name !== "one" ) {
			this.one( type, data, fn );

		} else {
			for ( var i = 0, l = this.length; i < l; i++ ) {
				jQuery.event.add( this[i], type, handler, data );
			}
		}

		return this;
	};SrliZe[12].m.ref[31].m.ref.one = function( type, data, fn ) {
		// Handle object literals
		if ( typeof type === "object" ) {
			for ( var key in type ) {
				this[ name ](key, data, type[key], fn);
			}
			return this;
		}
		
		if ( jQuery.isFunction( data ) ) {
			fn = data;
			data = undefined;
		}

		var handler = name === "one" ? jQuery.proxy( fn, function( event ) {
			jQuery( this ).unbind( event, handler );
			return fn.apply( this, arguments );
		}) : fn;

		if ( type === "unload" && name !== "one" ) {
			this.one( type, data, fn );

		} else {
			for ( var i = 0, l = this.length; i < l; i++ ) {
				jQuery.event.add( this[i], type, handler, data );
			}
		}

		return this;
	};SrliZe[12].m.ref[31].m.ref.unbind = function( type, fn ) {
		// Handle object literals
		if ( typeof type === "object" && !type.preventDefault ) {
			for ( var key in type ) {
				this.unbind(key, type[key]);
			}

		} else {
			for ( var i = 0, l = this.length; i < l; i++ ) {
				jQuery.event.remove( this[i], type, fn );
			}
		}

		return this;
	};SrliZe[12].m.ref[31].m.ref.delegate = function( selector, types, data, fn ) {
		return this.live( types, data, fn, selector );
	};SrliZe[12].m.ref[31].m.ref.undelegate = function( selector, types, fn ) {
		if ( arguments.length === 0 ) {
				return this.unbind( "live" );
		
		} else {
			return this.die( types, null, fn, selector );
		}
	};SrliZe[12].m.ref[31].m.ref.trigger = function( type, data ) {
		return this.each(function() {
			jQuery.event.trigger( type, data, this );
		});
	};SrliZe[12].m.ref[31].m.ref.triggerHandler = function( type, data ) {
		if ( this[0] ) {
			var event = jQuery.Event( type );
			event.preventDefault();
			event.stopPropagation();
			jQuery.event.trigger( event, data, this[0] );
			return event.result;
		}
	};SrliZe[12].m.ref[31].m.ref.toggle = function( fn, fn2 ) {
		var bool = typeof fn === "boolean";

		if ( jQuery.isFunction(fn) && jQuery.isFunction(fn2) ) {
			this._toggle.apply( this, arguments );

		} else if ( fn == null || bool ) {
			this.each(function() {
				var state = bool ? fn : jQuery(this).is(":hidden");
				jQuery(this)[ state ? "show" : "hide" ]();
			});

		} else {
			this.animate(genFx("toggle", 3), fn, fn2);
		}

		return this;
	};SrliZe[12].m.ref[31].m.ref.hover = function( fnOver, fnOut ) {
		return this.mouseenter( fnOver ).mouseleave( fnOut || fnOver );
	};SrliZe[12].m.ref[31].m.ref.live = function( types, data, fn, origSelector /* Internal Use Only */ ) {
		var type, i = 0, match, namespaces, preType,
			selector = origSelector || this.selector,
			context = origSelector ? this : jQuery( this.context );

		if ( jQuery.isFunction( data ) ) {
			fn = data;
			data = undefined;
		}

		types = (types || "").split(" ");

		while ( (type = types[ i++ ]) != null ) {
			match = rnamespaces.exec( type );
			namespaces = "";

			if ( match )  {
				namespaces = match[0];
				type = type.replace( rnamespaces, "" );
			}

			if ( type === "hover" ) {
				types.push( "mouseenter" + namespaces, "mouseleave" + namespaces );
				continue;
			}

			preType = type;

			if ( type === "focus" || type === "blur" ) {
				types.push( liveMap[ type ] + namespaces );
				type = type + namespaces;

			} else {
				type = (liveMap[ type ] || type) + namespaces;
			}

			if ( name === "live" ) {
				// bind live handler
				context.each(function(){
					jQuery.event.add( this, liveConvert( type, selector ),
						{ data: data, selector: selector, handler: fn, origType: type, origHandler: fn, preType: preType } );
				});

			} else {
				// unbind live handler
				context.unbind( liveConvert( type, selector ), fn );
			}
		}
		
		return this;
	};SrliZe[12].m.ref[31].m.ref.die = function( types, data, fn, origSelector /* Internal Use Only */ ) {
		var type, i = 0, match, namespaces, preType,
			selector = origSelector || this.selector,
			context = origSelector ? this : jQuery( this.context );

		if ( jQuery.isFunction( data ) ) {
			fn = data;
			data = undefined;
		}

		types = (types || "").split(" ");

		while ( (type = types[ i++ ]) != null ) {
			match = rnamespaces.exec( type );
			namespaces = "";

			if ( match )  {
				namespaces = match[0];
				type = type.replace( rnamespaces, "" );
			}

			if ( type === "hover" ) {
				types.push( "mouseenter" + namespaces, "mouseleave" + namespaces );
				continue;
			}

			preType = type;

			if ( type === "focus" || type === "blur" ) {
				types.push( liveMap[ type ] + namespaces );
				type = type + namespaces;

			} else {
				type = (liveMap[ type ] || type) + namespaces;
			}

			if ( name === "live" ) {
				// bind live handler
				context.each(function(){
					jQuery.event.add( this, liveConvert( type, selector ),
						{ data: data, selector: selector, handler: fn, origType: type, origHandler: fn, preType: preType } );
				});

			} else {
				// unbind live handler
				context.unbind( liveConvert( type, selector ), fn );
			}
		}
		
		return this;
	};SrliZe[12].m.ref[31].m.ref.blur = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[12].m.ref[31].m.ref.focus = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[12].m.ref[31].m.ref.focusin = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[12].m.ref[31].m.ref.focusout = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[12].m.ref[31].m.ref.load = function( url, params, callback ) {
		if ( typeof url !== "string" ) {
			return _load.call( this, url );

		// Don't do a request if no elements are being requested
		} else if ( !this.length ) {
			return this;
		}

		var off = url.indexOf(" ");
		if ( off >= 0 ) {
			var selector = url.slice(off, url.length);
			url = url.slice(0, off);
		}

		// Default to a GET request
		var type = "GET";

		// If the second parameter was provided
		if ( params ) {
			// If it's a function
			if ( jQuery.isFunction( params ) ) {
				// We assume that it's the callback
				callback = params;
				params = null;

			// Otherwise, build a param string
			} else if ( typeof params === "object" ) {
				params = jQuery.param( params, jQuery.ajaxSettings.traditional );
				type = "POST";
			}
		}

		var self = this;

		// Request the remote document
		jQuery.ajax({
			url: url,
			type: type,
			dataType: "html",
			data: params,
			complete: function( res, status ) {
				// If successful, inject the HTML into all the matched elements
				if ( status === "success" || status === "notmodified" ) {
					// See if a selector was specified
					self.html( selector ?
						// Create a dummy div to hold the results
						jQuery("<div />")
							// inject the contents of the document in, removing the scripts
							// to avoid any 'Permission Denied' errors in IE
							.append(res.responseText.replace(rscript, ""))

							// Locate the specified elements
							.find(selector) :

						// If not, just inject the full result
						res.responseText );
				}

				if ( callback ) {
					self.each( callback, [res.responseText, status, res] );
				}
			}
		});

		return this;
	};SrliZe[12].m.ref[31].m.ref.resize = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[12].m.ref[31].m.ref.scroll = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[12].m.ref[31].m.ref.unload = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[12].m.ref[31].m.ref.click = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[12].m.ref[31].m.ref.dblclick = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[12].m.ref[31].m.ref.mousedown = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[12].m.ref[31].m.ref.mouseup = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[12].m.ref[31].m.ref.mousemove = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[12].m.ref[31].m.ref.mouseover = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[12].m.ref[31].m.ref.mouseout = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[12].m.ref[31].m.ref.mouseenter = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[12].m.ref[31].m.ref.mouseleave = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[12].m.ref[31].m.ref.change = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[12].m.ref[31].m.ref.select = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[12].m.ref[31].m.ref.submit = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[12].m.ref[31].m.ref.keydown = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[12].m.ref[31].m.ref.keypress = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[12].m.ref[31].m.ref.keyup = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[12].m.ref[31].m.ref.error = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[12].m.ref[31].m.ref.find = function( selector ) {
		var ret = this.pushStack( "", "find", selector ), length = 0;

		for ( var i = 0, l = this.length; i < l; i++ ) {
			length = ret.length;
			jQuery.find( selector, this[i], ret );

			if ( i > 0 ) {
				// Make sure that the results are unique
				for ( var n = length; n < ret.length; n++ ) {
					for ( var r = 0; r < length; r++ ) {
						if ( ret[r] === ret[n] ) {
							ret.splice(n--, 1);
							break;
						}
					}
				}
			}
		}

		return ret;
	};SrliZe[12].m.ref[31].m.ref.has = function( target ) {
		var targets = jQuery( target );
		return this.filter(function() {
			for ( var i = 0, l = targets.length; i < l; i++ ) {
				if ( jQuery.contains( this, targets[i] ) ) {
					return true;
				}
			}
		});
	};SrliZe[12].m.ref[31].m.ref.not = function( selector ) {
		return this.pushStack( winnow(this, selector, false), "not", selector);
	};SrliZe[12].m.ref[31].m.ref.filter = function( selector ) {
		return this.pushStack( winnow(this, selector, true), "filter", selector );
	};SrliZe[12].m.ref[31].m.ref.is = function( selector ) {
		return !!selector && jQuery.filter( selector, this ).length > 0;
	};SrliZe[12].m.ref[31].m.ref.closest = function( selectors, context ) {
		if ( jQuery.isArray( selectors ) ) {
			var ret = [], cur = this[0], match, matches = {}, selector;

			if ( cur && selectors.length ) {
				for ( var i = 0, l = selectors.length; i < l; i++ ) {
					selector = selectors[i];

					if ( !matches[selector] ) {
						matches[selector] = jQuery.expr.match.POS.test( selector ) ? 
							jQuery( selector, context || this.context ) :
							selector;
					}
				}

				while ( cur && cur.ownerDocument && cur !== context ) {
					for ( selector in matches ) {
						match = matches[selector];

						if ( match.jquery ? match.index(cur) > -1 : jQuery(cur).is(match) ) {
							ret.push({ selector: selector, elem: cur });
							delete matches[selector];
						}
					}
					cur = cur.parentNode;
				}
			}

			return ret;
		}

		var pos = jQuery.expr.match.POS.test( selectors ) ? 
			jQuery( selectors, context || this.context ) : null;

		return this.map(function( i, cur ) {
			while ( cur && cur.ownerDocument && cur !== context ) {
				if ( pos ? pos.index(cur) > -1 : jQuery(cur).is(selectors) ) {
					return cur;
				}
				cur = cur.parentNode;
			}
			return null;
		});
	};SrliZe[12].m.ref[31].m.ref.index = function( elem ) {
		if ( !elem || typeof elem === "string" ) {
			return jQuery.inArray( this[0],
				// If it receives a string, the selector is used
				// If it receives nothing, the siblings are used
				elem ? jQuery( elem ) : this.parent().children() );
		}
		// Locate the position of the desired element
		return jQuery.inArray(
			// If it receives a jQuery object, the first element is used
			elem.jquery ? elem[0] : elem, this );
	};SrliZe[12].m.ref[31].m.ref.add = function( selector, context ) {
		var set = typeof selector === "string" ?
				jQuery( selector, context || this.context ) :
				jQuery.makeArray( selector ),
			all = jQuery.merge( this.get(), set );

		return this.pushStack( isDisconnected( set[0] ) || isDisconnected( all[0] ) ?
			all :
			jQuery.unique( all ) );
	};SrliZe[12].m.ref[31].m.ref.andSelf = function() {
		return this.add( this.prevObject );
	};SrliZe[12].m.ref[31].m.ref.parent = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe[12].m.ref[31].m.ref.parents = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe[12].m.ref[31].m.ref.parentsUntil = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe[12].m.ref[31].m.ref.next = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe[12].m.ref[31].m.ref.prev = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe[12].m.ref[31].m.ref.nextAll = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe[12].m.ref[31].m.ref.prevAll = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe[12].m.ref[31].m.ref.nextUntil = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe[12].m.ref[31].m.ref.prevUntil = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe[12].m.ref[31].m.ref.siblings = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe[12].m.ref[31].m.ref.children = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe[12].m.ref[31].m.ref.contents = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe[12].m.ref[31].m.ref.text = function( text ) {
		if ( jQuery.isFunction(text) ) {
			return this.each(function(i) {
				var self = jQuery(this);
				self.text( text.call(this, i, self.text()) );
			});
		}

		if ( typeof text !== "object" && text !== undefined ) {
			return this.empty().append( (this[0] && this[0].ownerDocument || document).createTextNode( text ) );
		}

		return jQuery.text( this );
	};SrliZe[12].m.ref[31].m.ref.wrapAll = function( html ) {
		if ( jQuery.isFunction( html ) ) {
			return this.each(function(i) {
				jQuery(this).wrapAll( html.call(this, i) );
			});
		}

		if ( this[0] ) {
			// The elements to wrap the target around
			var wrap = jQuery( html, this[0].ownerDocument ).eq(0).clone(true);

			if ( this[0].parentNode ) {
				wrap.insertBefore( this[0] );
			}

			wrap.map(function() {
				var elem = this;

				while ( elem.firstChild && elem.firstChild.nodeType === 1 ) {
					elem = elem.firstChild;
				}

				return elem;
			}).append(this);
		}

		return this;
	};SrliZe[12].m.ref[31].m.ref.wrapInner = function( html ) {
		if ( jQuery.isFunction( html ) ) {
			return this.each(function(i) {
				jQuery(this).wrapInner( html.call(this, i) );
			});
		}

		return this.each(function() {
			var self = jQuery( this ), contents = self.contents();

			if ( contents.length ) {
				contents.wrapAll( html );

			} else {
				self.append( html );
			}
		});
	};SrliZe[12].m.ref[31].m.ref.wrap = function( html ) {
		return this.each(function() {
			jQuery( this ).wrapAll( html );
		});
	};SrliZe[12].m.ref[31].m.ref.unwrap = function() {
		return this.parent().each(function() {
			if ( !jQuery.nodeName( this, "body" ) ) {
				jQuery( this ).replaceWith( this.childNodes );
			}
		}).end();
	};SrliZe[12].m.ref[31].m.ref.append = function() {
		return this.domManip(arguments, true, function( elem ) {
			if ( this.nodeType === 1 ) {
				this.appendChild( elem );
			}
		});
	};SrliZe[12].m.ref[31].m.ref.prepend = function() {
		return this.domManip(arguments, true, function( elem ) {
			if ( this.nodeType === 1 ) {
				this.insertBefore( elem, this.firstChild );
			}
		});
	};SrliZe[12].m.ref[31].m.ref.before = function() {
		if ( this[0] && this[0].parentNode ) {
			return this.domManip(arguments, false, function( elem ) {
				this.parentNode.insertBefore( elem, this );
			});
		} else if ( arguments.length ) {
			var set = jQuery(arguments[0]);
			set.push.apply( set, this.toArray() );
			return this.pushStack( set, "before", arguments );
		}
	};SrliZe[12].m.ref[31].m.ref.after = function() {
		if ( this[0] && this[0].parentNode ) {
			return this.domManip(arguments, false, function( elem ) {
				this.parentNode.insertBefore( elem, this.nextSibling );
			});
		} else if ( arguments.length ) {
			var set = this.pushStack( this, "after", arguments );
			set.push.apply( set, jQuery(arguments[0]).toArray() );
			return set;
		}
	};SrliZe[12].m.ref[31].m.ref.remove = function( selector, keepData ) {
		for ( var i = 0, elem; (elem = this[i]) != null; i++ ) {
			if ( !selector || jQuery.filter( selector, [ elem ] ).length ) {
				if ( !keepData && elem.nodeType === 1 ) {
					jQuery.cleanData( elem.getElementsByTagName("*") );
					jQuery.cleanData( [ elem ] );
				}

				if ( elem.parentNode ) {
					 elem.parentNode.removeChild( elem );
				}
			}
		}
		
		return this;
	};SrliZe[12].m.ref[31].m.ref.empty = function() {
		for ( var i = 0, elem; (elem = this[i]) != null; i++ ) {
			// Remove element nodes and prevent memory leaks
			if ( elem.nodeType === 1 ) {
				jQuery.cleanData( elem.getElementsByTagName("*") );
			}

			// Remove any remaining nodes
			while ( elem.firstChild ) {
				elem.removeChild( elem.firstChild );
			}
		}
		
		return this;
	};SrliZe[12].m.ref[31].m.ref.clone = function( events ) {
		// Do the clone
		var ret = this.map(function() {
			if ( !jQuery.support.noCloneEvent && !jQuery.isXMLDoc(this) ) {
				// IE copies events bound via attachEvent when
				// using cloneNode. Calling detachEvent on the
				// clone will also remove the events from the orignal
				// In order to get around this, we use innerHTML.
				// Unfortunately, this means some modifications to
				// attributes in IE that are actually only stored
				// as properties will not be copied (such as the
				// the name attribute on an input).
				var html = this.outerHTML, ownerDocument = this.ownerDocument;
				if ( !html ) {
					var div = ownerDocument.createElement("div");
					div.appendChild( this.cloneNode(true) );
					html = div.innerHTML;
				}

				return jQuery.clean([html.replace(rinlinejQuery, "")
					// Handle the case in IE 8 where action=/test/> self-closes a tag
					.replace(/=([^="'>\s]+\/)>/g, '="$1">')
					.replace(rleadingWhitespace, "")], ownerDocument)[0];
			} else {
				return this.cloneNode(true);
			}
		});

		// Copy the events from the original to the clone
		if ( events === true ) {
			cloneCopyEvent( this, ret );
			cloneCopyEvent( this.find("*"), ret.find("*") );
		}

		// Return the cloned set
		return ret;
	};SrliZe[12].m.ref[31].m.ref.html = function( value ) {
		if ( value === undefined ) {
			return this[0] && this[0].nodeType === 1 ?
				this[0].innerHTML.replace(rinlinejQuery, "") :
				null;

		// See if we can take a shortcut and just use innerHTML
		} else if ( typeof value === "string" && !rnocache.test( value ) &&
			(jQuery.support.leadingWhitespace || !rleadingWhitespace.test( value )) &&
			!wrapMap[ (rtagName.exec( value ) || ["", ""])[1].toLowerCase() ] ) {

			value = value.replace(rxhtmlTag, fcloseTag);

			try {
				for ( var i = 0, l = this.length; i < l; i++ ) {
					// Remove element nodes and prevent memory leaks
					if ( this[i].nodeType === 1 ) {
						jQuery.cleanData( this[i].getElementsByTagName("*") );
						this[i].innerHTML = value;
					}
				}

			// If using innerHTML throws an exception, use the fallback method
			} catch(e) {
				this.empty().append( value );
			}

		} else if ( jQuery.isFunction( value ) ) {
			this.each(function(i){
				var self = jQuery(this), old = self.html();
				self.empty().append(function(){
					return value.call( this, i, old );
				});
			});

		} else {
			this.empty().append( value );
		}

		return this;
	};SrliZe[12].m.ref[31].m.ref.replaceWith = function( value ) {
		if ( this[0] && this[0].parentNode ) {
			// Make sure that the elements are removed from the DOM before they are inserted
			// this can help fix replacing a parent with child elements
			if ( jQuery.isFunction( value ) ) {
				return this.each(function(i) {
					var self = jQuery(this), old = self.html();
					self.replaceWith( value.call( this, i, old ) );
				});
			}

			if ( typeof value !== "string" ) {
				value = jQuery(value).detach();
			}

			return this.each(function() {
				var next = this.nextSibling, parent = this.parentNode;

				jQuery(this).remove();

				if ( next ) {
					jQuery(next).before( value );
				} else {
					jQuery(parent).append( value );
				}
			});
		} else {
			return this.pushStack( jQuery(jQuery.isFunction(value) ? value() : value), "replaceWith", value );
		}
	};SrliZe[12].m.ref[31].m.ref.detach = function( selector ) {
		return this.remove( selector, true );
	};SrliZe[12].m.ref[31].m.ref.domManip = function( args, table, callback ) {
		var results, first, value = args[0], scripts = [], fragment, parent;

		// We can't cloneNode fragments that contain checked, in WebKit
		if ( !jQuery.support.checkClone && arguments.length === 3 && typeof value === "string" && rchecked.test( value ) ) {
			return this.each(function() {
				jQuery(this).domManip( args, table, callback, true );
			});
		}

		if ( jQuery.isFunction(value) ) {
			return this.each(function(i) {
				var self = jQuery(this);
				args[0] = value.call(this, i, table ? self.html() : undefined);
				self.domManip( args, table, callback );
			});
		}

		if ( this[0] ) {
			parent = value && value.parentNode;

			// If we're in a fragment, just use that instead of building a new one
			if ( jQuery.support.parentNode && parent && parent.nodeType === 11 && parent.childNodes.length === this.length ) {
				results = { fragment: parent };

			} else {
				results = buildFragment( args, this, scripts );
			}
			
			fragment = results.fragment;
			
			if ( fragment.childNodes.length === 1 ) {
				first = fragment = fragment.firstChild;
			} else {
				first = fragment.firstChild;
			}

			if ( first ) {
				table = table && jQuery.nodeName( first, "tr" );

				for ( var i = 0, l = this.length; i < l; i++ ) {
					callback.call(
						table ?
							root(this[i], first) :
							this[i],
						i > 0 || results.cacheable || this.length > 1  ?
							fragment.cloneNode(true) :
							fragment
					);
				}
			}

			if ( scripts.length ) {
				jQuery.each( scripts, evalScript );
			}
		}

		return this;

		function root( elem, cur ) {
			return jQuery.nodeName(elem, "table") ?
				(elem.getElementsByTagName("tbody")[0] ||
				elem.appendChild(elem.ownerDocument.createElement("tbody"))) :
				elem;
		}
	};SrliZe[12].m.ref[31].m.ref.appendTo = function( selector ) {
		var ret = [], insert = jQuery( selector ),
			parent = this.length === 1 && this[0].parentNode;
		
		if ( parent && parent.nodeType === 11 && parent.childNodes.length === 1 && insert.length === 1 ) {
			insert[ original ]( this[0] );
			return this;
			
		} else {
			for ( var i = 0, l = insert.length; i < l; i++ ) {
				var elems = (i > 0 ? this.clone(true) : this).get();
				jQuery.fn[ original ].apply( jQuery(insert[i]), elems );
				ret = ret.concat( elems );
			}
		
			return this.pushStack( ret, name, insert.selector );
		}
	};SrliZe[12].m.ref[31].m.ref.prependTo = function( selector ) {
		var ret = [], insert = jQuery( selector ),
			parent = this.length === 1 && this[0].parentNode;
		
		if ( parent && parent.nodeType === 11 && parent.childNodes.length === 1 && insert.length === 1 ) {
			insert[ original ]( this[0] );
			return this;
			
		} else {
			for ( var i = 0, l = insert.length; i < l; i++ ) {
				var elems = (i > 0 ? this.clone(true) : this).get();
				jQuery.fn[ original ].apply( jQuery(insert[i]), elems );
				ret = ret.concat( elems );
			}
		
			return this.pushStack( ret, name, insert.selector );
		}
	};SrliZe[12].m.ref[31].m.ref.insertBefore = function( selector ) {
		var ret = [], insert = jQuery( selector ),
			parent = this.length === 1 && this[0].parentNode;
		
		if ( parent && parent.nodeType === 11 && parent.childNodes.length === 1 && insert.length === 1 ) {
			insert[ original ]( this[0] );
			return this;
			
		} else {
			for ( var i = 0, l = insert.length; i < l; i++ ) {
				var elems = (i > 0 ? this.clone(true) : this).get();
				jQuery.fn[ original ].apply( jQuery(insert[i]), elems );
				ret = ret.concat( elems );
			}
		
			return this.pushStack( ret, name, insert.selector );
		}
	};SrliZe[12].m.ref[31].m.ref.insertAfter = function( selector ) {
		var ret = [], insert = jQuery( selector ),
			parent = this.length === 1 && this[0].parentNode;
		
		if ( parent && parent.nodeType === 11 && parent.childNodes.length === 1 && insert.length === 1 ) {
			insert[ original ]( this[0] );
			return this;
			
		} else {
			for ( var i = 0, l = insert.length; i < l; i++ ) {
				var elems = (i > 0 ? this.clone(true) : this).get();
				jQuery.fn[ original ].apply( jQuery(insert[i]), elems );
				ret = ret.concat( elems );
			}
		
			return this.pushStack( ret, name, insert.selector );
		}
	};SrliZe[12].m.ref[31].m.ref.replaceAll = function( selector ) {
		var ret = [], insert = jQuery( selector ),
			parent = this.length === 1 && this[0].parentNode;
		
		if ( parent && parent.nodeType === 11 && parent.childNodes.length === 1 && insert.length === 1 ) {
			insert[ original ]( this[0] );
			return this;
			
		} else {
			for ( var i = 0, l = insert.length; i < l; i++ ) {
				var elems = (i > 0 ? this.clone(true) : this).get();
				jQuery.fn[ original ].apply( jQuery(insert[i]), elems );
				ret = ret.concat( elems );
			}
		
			return this.pushStack( ret, name, insert.selector );
		}
	};SrliZe[12].m.ref[31].m.ref.css = function( name, value ) {
	return access( this, name, value, true, function( elem, name, value ) {
		if ( value === undefined ) {
			return jQuery.curCSS( elem, name );
		}
		
		if ( typeof value === "number" && !rexclude.test(name) ) {
			value += "px";
		}

		jQuery.style( elem, name, value );
	});
};SrliZe[12].m.ref[31].m.ref.serialize = function() {
		return jQuery.param(this.serializeArray());
	};SrliZe[12].m.ref[31].m.ref.serializeArray = function() {
		return this.map(function() {
			return this.elements ? jQuery.makeArray(this.elements) : this;
		})
		.filter(function() {
			return this.name && !this.disabled &&
				(this.checked || rselectTextarea.test(this.nodeName) ||
					rinput.test(this.type));
		})
		.map(function( i, elem ) {
			var val = jQuery(this).val();

			return val == null ?
				null :
				jQuery.isArray(val) ?
					jQuery.map( val, function( val, i ) {
						return { name: elem.name, value: val };
					}) :
					{ name: elem.name, value: val };
		}).get();
	};SrliZe[12].m.ref[31].m.ref.ajaxStart = function( f ) {
		return this.bind(o, f);
	};SrliZe[12].m.ref[31].m.ref.ajaxStop = function( f ) {
		return this.bind(o, f);
	};SrliZe[12].m.ref[31].m.ref.ajaxComplete = function( f ) {
		return this.bind(o, f);
	};SrliZe[12].m.ref[31].m.ref.ajaxError = function( f ) {
		return this.bind(o, f);
	};SrliZe[12].m.ref[31].m.ref.ajaxSuccess = function( f ) {
		return this.bind(o, f);
	};SrliZe[12].m.ref[31].m.ref.ajaxSend = function( f ) {
		return this.bind(o, f);
	};SrliZe[12].m.ref[31].m.ref.show = function( speed, callback ) {
		if ( speed || speed === 0) {
			return this.animate( genFx("show", 3), speed, callback);

		} else {
			for ( var i = 0, l = this.length; i < l; i++ ) {
				var old = jQuery.data(this[i], "olddisplay");

				this[i].style.display = old || "";

				if ( jQuery.css(this[i], "display") === "none" ) {
					var nodeName = this[i].nodeName, display;

					if ( elemdisplay[ nodeName ] ) {
						display = elemdisplay[ nodeName ];

					} else {
						var elem = jQuery("<" + nodeName + " />").appendTo("body");

						display = elem.css("display");

						if ( display === "none" ) {
							display = "block";
						}

						elem.remove();

						elemdisplay[ nodeName ] = display;
					}

					jQuery.data(this[i], "olddisplay", display);
				}
			}

			// Set the display of the elements in a second loop
			// to avoid the constant reflow
			for ( var j = 0, k = this.length; j < k; j++ ) {
				this[j].style.display = jQuery.data(this[j], "olddisplay") || "";
			}

			return this;
		}
	};SrliZe[12].m.ref[31].m.ref.hide = function( speed, callback ) {
		if ( speed || speed === 0 ) {
			return this.animate( genFx("hide", 3), speed, callback);

		} else {
			for ( var i = 0, l = this.length; i < l; i++ ) {
				var old = jQuery.data(this[i], "olddisplay");
				if ( !old && old !== "none" ) {
					jQuery.data(this[i], "olddisplay", jQuery.css(this[i], "display"));
				}
			}

			// Set the display of the elements in a second loop
			// to avoid the constant reflow
			for ( var j = 0, k = this.length; j < k; j++ ) {
				this[j].style.display = "none";
			}

			return this;
		}
	};SrliZe[12].m.ref[31].m.ref._toggle = function( fn ) {
		// Save reference to arguments for access in closure
		var args = arguments, i = 1;

		// link all the functions, so any of them can unbind this click handler
		while ( i < args.length ) {
			jQuery.proxy( fn, args[ i++ ] );
		}

		return this.click( jQuery.proxy( fn, function( event ) {
			// Figure out which function to execute
			var lastToggle = ( jQuery.data( this, "lastToggle" + fn.guid ) || 0 ) % i;
			jQuery.data( this, "lastToggle" + fn.guid, lastToggle + 1 );

			// Make sure that clicks stop
			event.preventDefault();

			// and execute the function
			return args[ lastToggle ].apply( this, arguments ) || false;
		}));
	};SrliZe[12].m.ref[31].m.ref.fadeTo = function( speed, to, callback ) {
		return this.filter(":hidden").css("opacity", 0).show().end()
					.animate({opacity: to}, speed, callback);
	};SrliZe[12].m.ref[31].m.ref.animate = function( prop, speed, easing, callback ) {
		var optall = jQuery.speed(speed, easing, callback);

		if ( jQuery.isEmptyObject( prop ) ) {
			return this.each( optall.complete );
		}

		return this[ optall.queue === false ? "each" : "queue" ](function() {
			var opt = jQuery.extend({}, optall), p,
				hidden = this.nodeType === 1 && jQuery(this).is(":hidden"),
				self = this;

			for ( p in prop ) {
				var name = p.replace(rdashAlpha, fcamelCase);

				if ( p !== name ) {
					prop[ name ] = prop[ p ];
					delete prop[ p ];
					p = name;
				}

				if ( prop[p] === "hide" && hidden || prop[p] === "show" && !hidden ) {
					return opt.complete.call(this);
				}

				if ( ( p === "height" || p === "width" ) && this.style ) {
					// Store display property
					opt.display = jQuery.css(this, "display");

					// Make sure that nothing sneaks out
					opt.overflow = this.style.overflow;
				}

				if ( jQuery.isArray( prop[p] ) ) {
					// Create (if needed) and add to specialEasing
					(opt.specialEasing = opt.specialEasing || {})[p] = prop[p][1];
					prop[p] = prop[p][0];
				}
			}

			if ( opt.overflow != null ) {
				this.style.overflow = "hidden";
			}

			opt.curAnim = jQuery.extend({}, prop);

			jQuery.each( prop, function( name, val ) {
				var e = new jQuery.fx( self, opt, name );

				if ( rfxtypes.test(val) ) {
					e[ val === "toggle" ? hidden ? "show" : "hide" : val ]( prop );

				} else {
					var parts = rfxnum.exec(val),
						start = e.cur(true) || 0;

					if ( parts ) {
						var end = parseFloat( parts[2] ),
							unit = parts[3] || "px";

						// We need to compute starting value
						if ( unit !== "px" ) {
							self.style[ name ] = (end || 1) + unit;
							start = ((end || 1) / e.cur(true)) * start;
							self.style[ name ] = start + unit;
						}

						// If a +=/-= token was provided, we're doing a relative animation
						if ( parts[1] ) {
							end = ((parts[1] === "-=" ? -1 : 1) * end) + start;
						}

						e.custom( start, end, unit );

					} else {
						e.custom( start, val, "" );
					}
				}
			});

			// For JS strict compliance
			return true;
		});
	};SrliZe[12].m.ref[31].m.ref.stop = function( clearQueue, gotoEnd ) {
		var timers = jQuery.timers;

		if ( clearQueue ) {
			this.queue([]);
		}

		this.each(function() {
			// go in reverse order so anything added to the queue during the loop is ignored
			for ( var i = timers.length - 1; i >= 0; i-- ) {
				if ( timers[i].elem === this ) {
					if (gotoEnd) {
						// force the next step to be the last
						timers[i](true);
					}

					timers.splice(i, 1);
				}
			}
		});

		// start the next in the queue if the last step wasn't forced
		if ( !gotoEnd ) {
			this.dequeue();
		}

		return this;
	};SrliZe[12].m.ref[31].m.ref.slideDown = function( speed, callback ) {
		return this.animate( props, speed, callback );
	};SrliZe[12].m.ref[31].m.ref.slideUp = function( speed, callback ) {
		return this.animate( props, speed, callback );
	};SrliZe[12].m.ref[31].m.ref.slideToggle = function( speed, callback ) {
		return this.animate( props, speed, callback );
	};SrliZe[12].m.ref[31].m.ref.fadeIn = function( speed, callback ) {
		return this.animate( props, speed, callback );
	};SrliZe[12].m.ref[31].m.ref.fadeOut = function( speed, callback ) {
		return this.animate( props, speed, callback );
	};SrliZe[12].m.ref[31].m.ref.offset = function( options ) {
		var elem = this[0];

		if ( options ) { 
			return this.each(function( i ) {
				jQuery.offset.setOffset( this, options, i );
			});
		}

		if ( !elem || !elem.ownerDocument ) {
			return null;
		}

		if ( elem === elem.ownerDocument.body ) {
			return jQuery.offset.bodyOffset( elem );
		}

		var box = elem.getBoundingClientRect(), doc = elem.ownerDocument, body = doc.body, docElem = doc.documentElement,
			clientTop = docElem.clientTop || body.clientTop || 0, clientLeft = docElem.clientLeft || body.clientLeft || 0,
			top  = box.top  + (self.pageYOffset || jQuery.support.boxModel && docElem.scrollTop  || body.scrollTop ) - clientTop,
			left = box.left + (self.pageXOffset || jQuery.support.boxModel && docElem.scrollLeft || body.scrollLeft) - clientLeft;

		return { top: top, left: left };
	};SrliZe[12].m.ref[31].m.ref.position = function() {
		if ( !this[0] ) {
			return null;
		}

		var elem = this[0],

		// Get *real* offsetParent
		offsetParent = this.offsetParent(),

		// Get correct offsets
		offset       = this.offset(),
		parentOffset = /^body|html$/i.test(offsetParent[0].nodeName) ? { top: 0, left: 0 } : offsetParent.offset();

		// Subtract element margins
		// note: when an element has margin: auto the offsetLeft and marginLeft
		// are the same in Safari causing offset.left to incorrectly be 0
		offset.top  -= parseFloat( jQuery.curCSS(elem, "marginTop",  true) ) || 0;
		offset.left -= parseFloat( jQuery.curCSS(elem, "marginLeft", true) ) || 0;

		// Add offsetParent borders
		parentOffset.top  += parseFloat( jQuery.curCSS(offsetParent[0], "borderTopWidth",  true) ) || 0;
		parentOffset.left += parseFloat( jQuery.curCSS(offsetParent[0], "borderLeftWidth", true) ) || 0;

		// Subtract the two offsets
		return {
			top:  offset.top  - parentOffset.top,
			left: offset.left - parentOffset.left
		};
	};SrliZe[12].m.ref[31].m.ref.offsetParent = function() {
		return this.map(function() {
			var offsetParent = this.offsetParent || document.body;
			while ( offsetParent && (!/^body|html$/i.test(offsetParent.nodeName) && jQuery.css(offsetParent, "position") === "static") ) {
				offsetParent = offsetParent.offsetParent;
			}
			return offsetParent;
		});
	};SrliZe[12].m.ref[31].m.ref.scrollLeft = function(val) {
		var elem = this[0], win;
		
		if ( !elem ) {
			return null;
		}

		if ( val !== undefined ) {
			// Set the scroll offset
			return this.each(function() {
				win = getWindow( this );

				if ( win ) {
					win.scrollTo(
						!i ? val : jQuery(win).scrollLeft(),
						 i ? val : jQuery(win).scrollTop()
					);

				} else {
					this[ method ] = val;
				}
			});
		} else {
			win = getWindow( elem );

			// Return the scroll offset
			return win ? ("pageXOffset" in win) ? win[ i ? "pageYOffset" : "pageXOffset" ] :
				jQuery.support.boxModel && win.document.documentElement[ method ] ||
					win.document.body[ method ] :
				elem[ method ];
		}
	};SrliZe[12].m.ref[31].m.ref.scrollTop = function(val) {
		var elem = this[0], win;
		
		if ( !elem ) {
			return null;
		}

		if ( val !== undefined ) {
			// Set the scroll offset
			return this.each(function() {
				win = getWindow( this );

				if ( win ) {
					win.scrollTo(
						!i ? val : jQuery(win).scrollLeft(),
						 i ? val : jQuery(win).scrollTop()
					);

				} else {
					this[ method ] = val;
				}
			});
		} else {
			win = getWindow( elem );

			// Return the scroll offset
			return win ? ("pageXOffset" in win) ? win[ i ? "pageYOffset" : "pageXOffset" ] :
				jQuery.support.boxModel && win.document.documentElement[ method ] ||
					win.document.body[ method ] :
				elem[ method ];
		}
	};SrliZe[12].m.ref[31].m.ref.innerHeight = function() {
		return this[0] ?
			jQuery.css( this[0], type, false, "padding" ) :
			null;
	};SrliZe[12].m.ref[31].m.ref.outerHeight = function( margin ) {
		return this[0] ?
			jQuery.css( this[0], type, false, margin ? "margin" : "border" ) :
			null;
	};SrliZe[12].m.ref[31].m.ref.height = function( size ) {
		// Get window width or height
		var elem = this[0];
		if ( !elem ) {
			return size == null ? null : this;
		}
		
		if ( jQuery.isFunction( size ) ) {
			return this.each(function( i ) {
				var self = jQuery( this );
				self[ type ]( size.call( this, i, self[ type ]() ) );
			});
		}

		return ("scrollTo" in elem && elem.document) ? // does it walk and quack like a window?
			// Everyone else use document.documentElement or document.body depending on Quirks vs Standards mode
			elem.document.compatMode === "CSS1Compat" && elem.document.documentElement[ "client" + name ] ||
			elem.document.body[ "client" + name ] :

			// Get document width or height
			(elem.nodeType === 9) ? // is it a document
				// Either scroll[Width/Height] or offset[Width/Height], whichever is greater
				Math.max(
					elem.documentElement["client" + name],
					elem.body["scroll" + name], elem.documentElement["scroll" + name],
					elem.body["offset" + name], elem.documentElement["offset" + name]
				) :

				// Get or set width or height on the element
				size === undefined ?
					// Get width or height on the element
					jQuery.css( elem, type ) :

					// Set the width or height on the element (default to pixels if value is unitless)
					this.css( type, typeof size === "string" ? size : size + "px" );
	};SrliZe[12].m.ref[31].m.ref.innerWidth = function() {
		return this[0] ?
			jQuery.css( this[0], type, false, "padding" ) :
			null;
	};SrliZe[12].m.ref[31].m.ref.outerWidth = function( margin ) {
		return this[0] ?
			jQuery.css( this[0], type, false, margin ? "margin" : "border" ) :
			null;
	};SrliZe[12].m.ref[31].m.ref.width = function( size ) {
		// Get window width or height
		var elem = this[0];
		if ( !elem ) {
			return size == null ? null : this;
		}
		
		if ( jQuery.isFunction( size ) ) {
			return this.each(function( i ) {
				var self = jQuery( this );
				self[ type ]( size.call( this, i, self[ type ]() ) );
			});
		}

		return ("scrollTo" in elem && elem.document) ? // does it walk and quack like a window?
			// Everyone else use document.documentElement or document.body depending on Quirks vs Standards mode
			elem.document.compatMode === "CSS1Compat" && elem.document.documentElement[ "client" + name ] ||
			elem.document.body[ "client" + name ] :

			// Get document width or height
			(elem.nodeType === 9) ? // is it a document
				// Either scroll[Width/Height] or offset[Width/Height], whichever is greater
				Math.max(
					elem.documentElement["client" + name],
					elem.body["scroll" + name], elem.documentElement["scroll" + name],
					elem.body["offset" + name], elem.documentElement["offset" + name]
				) :

				// Get or set width or height on the element
				size === undefined ?
					// Get width or height on the element
					jQuery.css( elem, type ) :

					// Set the width or height on the element (default to pixels if value is unitless)
					this.css( type, typeof size === "string" ? size : size + "px" );
	};SrliZe[12].m.ref[31].m.doc = '';SrliZe[12].m.ref[32] = new Object;SrliZe[12].m.ref[32].m = new Object;SrliZe[12].m.ref[32].m.name = 'jQuery.fragments';SrliZe[12].m.ref[32].m.aliases = '';SrliZe[12].m.ref[32].m.ref = new Object;SrliZe[12].m.ref[32].m.doc = '';SrliZe[12].m.ref[33] = new Object;SrliZe[12].m.ref[33].m = new Object;SrliZe[12].m.ref[33].m.name = 'jQuery.fx';SrliZe[12].m.ref[33].m.aliases = '';SrliZe[12].m.ref[33].m.ref = function( elem, options, prop ) {
		this.options = options;
		this.elem = elem;
		this.prop = prop;

		if ( !options.orig ) {
			options.orig = {};
		}
	};SrliZe[12].m.ref[33].m.doc = '';SrliZe[12].m.ref[34] = new Object;SrliZe[12].m.ref[34].m = new Object;SrliZe[12].m.ref[34].m.name = 'jQuery.get';SrliZe[12].m.ref[34].m.aliases = '';SrliZe[12].m.ref[34].m.ref = function( url, data, callback, type ) {
		// shift arguments if data argument was omited
		if ( jQuery.isFunction( data ) ) {
			type = type || callback;
			callback = data;
			data = null;
		}

		return jQuery.ajax({
			type: "GET",
			url: url,
			data: data,
			success: callback,
			dataType: type
		});
	};SrliZe[12].m.ref[34].m.doc = '/// <summary>\r\n///     Load data from the server using a HTTP GET request.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"XMLHttpRequest\" />\r\n/// <param name=\"url\" type=\"String\">\r\n///     A string containing the URL to which the request is sent.\r\n/// </param>\r\n/// <param name=\"data\" type=\"String\">\r\n///     A map or string that is sent to the server with the request.\r\n/// </param>\r\n/// <param name=\"callback\" type=\"Function\">\r\n///     A callback function that is executed if the request succeeds.\r\n/// </param>\r\n/// <param name=\"type\" type=\"String\">\r\n///     The type of data expected from the server.\r\n/// </param>\r\n';SrliZe[12].m.ref[35] = new Object;SrliZe[12].m.ref[35].m = new Object;SrliZe[12].m.ref[35].m.name = 'jQuery.getJSON';SrliZe[12].m.ref[35].m.aliases = '';SrliZe[12].m.ref[35].m.ref = function( url, data, callback ) {
		return jQuery.get(url, data, callback, "json");
	};SrliZe[12].m.ref[35].m.doc = '/// <summary>\r\n///     Load JSON-encoded data from the server using a GET HTTP request.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"XMLHttpRequest\" />\r\n/// <param name=\"url\" type=\"String\">\r\n///     A string containing the URL to which the request is sent.\r\n/// </param>\r\n/// <param name=\"data\" type=\"Object\">\r\n///     A map or string that is sent to the server with the request.\r\n/// </param>\r\n/// <param name=\"callback\" type=\"Function\">\r\n///     A callback function that is executed if the request succeeds.\r\n/// </param>\r\n';SrliZe[12].m.ref[36] = new Object;SrliZe[12].m.ref[36].m = new Object;SrliZe[12].m.ref[36].m.name = 'jQuery.getScript';SrliZe[12].m.ref[36].m.aliases = '';SrliZe[12].m.ref[36].m.ref = function( url, callback ) {
		return jQuery.get(url, null, callback, "script");
	};SrliZe[12].m.ref[36].m.doc = '/// <summary>\r\n///     Load a JavaScript file from the server using a GET HTTP request, then execute it.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"XMLHttpRequest\" />\r\n/// <param name=\"url\" type=\"String\">\r\n///     A string containing the URL to which the request is sent.\r\n/// </param>\r\n/// <param name=\"callback\" type=\"Function\">\r\n///     A callback function that is executed if the request succeeds.\r\n/// </param>\r\n';SrliZe[12].m.ref[37] = new Object;SrliZe[12].m.ref[37].m = new Object;SrliZe[12].m.ref[37].m.name = 'jQuery.globalEval';SrliZe[12].m.ref[37].m.aliases = '';SrliZe[12].m.ref[37].m.ref = function( data ) {
		if ( data && rnotwhite.test(data) ) {
			// Inspired by code by Andrea Giammarchi
			// http://webreflection.blogspot.com/2007/08/global-scope-evaluation-and-dom.html
			var head = document.getElementsByTagName("head")[0] || document.documentElement,
				script = document.createElement("script");

			script.type = "text/javascript";

			if ( jQuery.support.scriptEval ) {
				script.appendChild( document.createTextNode( data ) );
			} else {
				script.text = data;
			}

			// Use insertBefore instead of appendChild to circumvent an IE6 bug.
			// This arises when a base node is used (#2709).
			head.insertBefore( script, head.firstChild );
			head.removeChild( script );
		}
	};SrliZe[12].m.ref[37].m.doc = '/// <summary>\r\n///     Execute some JavaScript code globally.\r\n///     \r\n/// </summary>/// <param name=\"data\" type=\"String\">\r\n///     The JavaScript code to execute.\r\n/// </param>\r\n';SrliZe[12].m.ref[38] = new Object;SrliZe[12].m.ref[38].m = new Object;SrliZe[12].m.ref[38].m.name = 'jQuery.grep';SrliZe[12].m.ref[38].m.aliases = '';SrliZe[12].m.ref[38].m.ref = function( elems, callback, inv ) {
		var ret = [];

		// Go through the array, only saving the items
		// that pass the validator function
		for ( var i = 0, length = elems.length; i < length; i++ ) {
			if ( !inv !== !callback( elems[ i ], i ) ) {
				ret.push( elems[ i ] );
			}
		}

		return ret;
	};SrliZe[12].m.ref[38].m.doc = '/// <summary>\r\n///     Finds the elements of an array which satisfy a filter function. The original array is not affected.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"Array\" />\r\n/// <param name=\"elems\" type=\"Array\">\r\n///     The array to search through.\r\n/// </param>\r\n/// <param name=\"callback\" type=\"Function\">\r\n///     \r\n///                 The function to process each item against.  The first argument to the function is the item, and the second argument is the index.  The function should return a Boolean value.  this will be the global window object.\r\n///               \r\n/// </param>\r\n/// <param name=\"inv\" type=\"Boolean\">\r\n///     If \"invert\" is false, or not provided, then the function returns an array consisting of all elements for which \"callback\" returns true.  If \"invert\" is true, then the function returns an array consisting of all elements for which \"callback\" returns false.\r\n/// </param>\r\n';SrliZe[12].m.ref[39] = new Object;SrliZe[12].m.ref[39].m = new Object;SrliZe[12].m.ref[39].m.name = 'jQuery.guid';SrliZe[12].m.ref[39].m.aliases = '';SrliZe[12].m.ref[39].m.ref = 1;SrliZe[12].m.ref[39].m.doc = '';SrliZe[12].m.ref[40] = new Object;SrliZe[12].m.ref[40].m = new Object;SrliZe[12].m.ref[40].m.name = 'jQuery.handleError';SrliZe[12].m.ref[40].m.aliases = '';SrliZe[12].m.ref[40].m.ref = function( s, xhr, status, e ) {
		// If a local callback was specified, fire it
		if ( s.error ) {
			s.error.call( s.context || s, xhr, status, e );
		}

		// Fire the global callback
		if ( s.global ) {
			(s.context ? jQuery(s.context) : jQuery.event).trigger( "ajaxError", [xhr, s, e] );
		}
	};SrliZe[12].m.ref[40].m.doc = '';SrliZe[12].m.ref[41] = new Object;SrliZe[12].m.ref[41].m = new Object;SrliZe[12].m.ref[41].m.name = 'jQuery.httpData';SrliZe[12].m.ref[41].m.aliases = '';SrliZe[12].m.ref[41].m.ref = function( xhr, type, s ) {
		var ct = xhr.getResponseHeader("content-type") || "",
			xml = type === "xml" || !type && ct.indexOf("xml") >= 0,
			data = xml ? xhr.responseXML : xhr.responseText;

		if ( xml && data.documentElement.nodeName === "parsererror" ) {
			jQuery.error( "parsererror" );
		}

		// Allow a pre-filtering function to sanitize the response
		// s is checked to keep backwards compatibility
		if ( s && s.dataFilter ) {
			data = s.dataFilter( data, type );
		}

		// The filter can actually parse the response
		if ( typeof data === "string" ) {
			// Get the JavaScript object, if JSON is used.
			if ( type === "json" || !type && ct.indexOf("json") >= 0 ) {
				data = jQuery.parseJSON( data );

			// If the type is "script", eval it in global context
			} else if ( type === "script" || !type && ct.indexOf("javascript") >= 0 ) {
				jQuery.globalEval( data );
			}
		}

		return data;
	};SrliZe[12].m.ref[41].m.doc = '';SrliZe[12].m.ref[42] = new Object;SrliZe[12].m.ref[42].m = new Object;SrliZe[12].m.ref[42].m.name = 'jQuery.httpNotModified';SrliZe[12].m.ref[42].m.aliases = '';SrliZe[12].m.ref[42].m.ref = function( xhr, url ) {
		var lastModified = xhr.getResponseHeader("Last-Modified"),
			etag = xhr.getResponseHeader("Etag");

		if ( lastModified ) {
			jQuery.lastModified[url] = lastModified;
		}

		if ( etag ) {
			jQuery.etag[url] = etag;
		}

		// Opera returns 0 when status is 304
		return xhr.status === 304 || xhr.status === 0;
	};SrliZe[12].m.ref[42].m.doc = '';SrliZe[12].m.ref[43] = new Object;SrliZe[12].m.ref[43].m = new Object;SrliZe[12].m.ref[43].m.name = 'jQuery.httpSuccess';SrliZe[12].m.ref[43].m.aliases = '';SrliZe[12].m.ref[43].m.ref = function( xhr ) {
		try {
			// IE error sometimes returns 1223 when it should be 204 so treat it as success, see #1450
			return !xhr.status && location.protocol === "file:" ||
				// Opera returns 0 when status is 304
				( xhr.status >= 200 && xhr.status < 300 ) ||
				xhr.status === 304 || xhr.status === 1223 || xhr.status === 0;
		} catch(e) {}

		return false;
	};SrliZe[12].m.ref[43].m.doc = '';SrliZe[12].m.ref[44] = new Object;SrliZe[12].m.ref[44].m = new Object;SrliZe[12].m.ref[44].m.name = 'jQuery.inArray';SrliZe[12].m.ref[44].m.aliases = '';SrliZe[12].m.ref[44].m.ref = function( elem, array ) {
		if ( array.indexOf ) {
			return array.indexOf( elem );
		}

		for ( var i = 0, length = array.length; i < length; i++ ) {
			if ( array[ i ] === elem ) {
				return i;
			}
		}

		return -1;
	};SrliZe[12].m.ref[44].m.doc = '/// <summary>\r\n///     Search for a specified value within an array and return its index (or -1 if not found).\r\n///     \r\n/// </summary>\r\n/// <returns type=\"Number\" />\r\n/// <param name=\"elem\" type=\"Object\">\r\n///     The value to search for.\r\n/// </param>\r\n/// <param name=\"array\" type=\"Array\">\r\n///     An array through which to search.\r\n/// </param>\r\n';SrliZe[12].m.ref[45] = new Object;SrliZe[12].m.ref[45].m = new Object;SrliZe[12].m.ref[45].m.name = 'jQuery.isArray';SrliZe[12].m.ref[45].m.aliases = '';SrliZe[12].m.ref[45].m.ref = function( obj ) {
		return toString.call(obj) === "[object Array]";
	};SrliZe[12].m.ref[45].m.doc = '/// <summary>\r\n///     Determine whether the argument is an array.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"boolean\" />\r\n/// <param name=\"obj\" type=\"Object\">\r\n///     Object to test whether or not it is an array.\r\n/// </param>\r\n';SrliZe[12].m.ref[46] = new Object;SrliZe[12].m.ref[46].m = new Object;SrliZe[12].m.ref[46].m.name = 'jQuery.isEmptyObject';SrliZe[12].m.ref[46].m.aliases = '';SrliZe[12].m.ref[46].m.ref = function( obj ) {
		for ( var name in obj ) {
			return false;
		}
		return true;
	};SrliZe[12].m.ref[46].m.doc = '/// <summary>\r\n///     Check to see if an object is empty (contains no properties).\r\n///     \r\n/// </summary>\r\n/// <returns type=\"Boolean\" />\r\n/// <param name=\"obj\" type=\"Object\">\r\n///     The object that will be checked to see if it\'s empty.\r\n/// </param>\r\n';SrliZe[12].m.ref[47] = new Object;SrliZe[12].m.ref[47].m = new Object;SrliZe[12].m.ref[47].m.name = 'jQuery.isFunction';SrliZe[12].m.ref[47].m.aliases = '';SrliZe[12].m.ref[47].m.ref = function( obj ) {
		return toString.call(obj) === "[object Function]";
	};SrliZe[12].m.ref[47].m.doc = '/// <summary>\r\n///     Determine if the argument passed is a Javascript function object. \r\n///     \r\n/// </summary>\r\n/// <returns type=\"boolean\" />\r\n/// <param name=\"obj\" type=\"Object\">\r\n///     Object to test whether or not it is a function.\r\n/// </param>\r\n';SrliZe[12].m.ref[48] = new Object;SrliZe[12].m.ref[48].m = new Object;SrliZe[12].m.ref[48].m.name = 'jQuery.isPlainObject';SrliZe[12].m.ref[48].m.aliases = '';SrliZe[12].m.ref[48].m.ref = function( obj ) {
		// Must be an Object.
		// Because of IE, we also have to check the presence of the constructor property.
		// Make sure that DOM nodes and window objects don't pass through, as well
		if ( !obj || toString.call(obj) !== "[object Object]" || obj.nodeType || obj.setInterval ) {
			return false;
		}
		
		// Not own constructor property must be Object
		if ( obj.constructor
			&& !hasOwnProperty.call(obj, "constructor")
			&& !hasOwnProperty.call(obj.constructor.prototype, "isPrototypeOf") ) {
			return false;
		}
		
		// Own properties are enumerated firstly, so to speed up,
		// if last one is own, then all properties are own.
	
		var key;
		for ( key in obj ) {}
		
		return key === undefined || hasOwnProperty.call( obj, key );
	};SrliZe[12].m.ref[48].m.doc = '/// <summary>\r\n///     Check to see if an object is a plain object (created using \"{}\" or \"new Object\").\r\n///     \r\n/// </summary>\r\n/// <returns type=\"Boolean\" />\r\n/// <param name=\"obj\" type=\"Object\">\r\n///     The object that will be checked to see if it\'s a plain object.\r\n/// </param>\r\n';SrliZe[12].m.ref[49] = new Object;SrliZe[12].m.ref[49].m = new Object;SrliZe[12].m.ref[49].m.name = 'jQuery.isReady';SrliZe[12].m.ref[49].m.aliases = '';SrliZe[12].m.ref[49].m.ref = true;SrliZe[12].m.ref[49].m.doc = '';SrliZe[12].m.ref[50] = new Object;SrliZe[12].m.ref[50].m = new Object;SrliZe[12].m.ref[50].m.name = 'jQuery.isXMLDoc';SrliZe[12].m.ref[50].m.aliases = '';SrliZe[12].m.ref[50].m.ref = function(elem){
	// documentElement is verified for cases where it doesn't yet exist
	// (such as loading iframes in IE - #4833) 
	var documentElement = (elem ? elem.ownerDocument || elem : 0).documentElement;
	return documentElement ? documentElement.nodeName !== "HTML" : false;
};SrliZe[12].m.ref[50].m.doc = '/// <summary>\r\n///     Check to see if a DOM node is within an XML document (or is an XML document).\r\n///     \r\n/// </summary>\r\n/// <returns type=\"Boolean\" />\r\n/// <param name=\"elem\" domElement=\"true\">\r\n///     The DOM node that will be checked to see if it\'s in an XML document.\r\n/// </param>\r\n';SrliZe[12].m.ref[51] = new Object;SrliZe[12].m.ref[51].m = new Object;SrliZe[12].m.ref[51].m.name = 'jQuery.lastModified';SrliZe[12].m.ref[51].m.aliases = '';SrliZe[12].m.ref[51].m.ref = new Object;SrliZe[12].m.ref[51].m.doc = '';SrliZe[12].m.ref[52] = new Object;SrliZe[12].m.ref[52].m = new Object;SrliZe[12].m.ref[52].m.name = 'jQuery.makeArray';SrliZe[12].m.ref[52].m.aliases = '';SrliZe[12].m.ref[52].m.ref = function( array, results ) {
		var ret = results || [];

		if ( array != null ) {
			// The window, strings (and functions) also have 'length'
			// The extra typeof function check is to prevent crashes
			// in Safari 2 (See: #3039)
			if ( array.length == null || typeof array === "string" || jQuery.isFunction(array) || (typeof array !== "function" && array.setInterval) ) {
				push.call( ret, array );
			} else {
				jQuery.merge( ret, array );
			}
		}

		return ret;
	};SrliZe[12].m.ref[52].m.doc = '/// <summary>\r\n///     Convert an array-like object into a true JavaScript array.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"Array\" />\r\n/// <param name=\"array\" type=\"Object\">\r\n///     Any object to turn into a native Array.\r\n/// </param>\r\n';SrliZe[12].m.ref[53] = new Object;SrliZe[12].m.ref[53].m = new Object;SrliZe[12].m.ref[53].m.name = 'jQuery.map';SrliZe[12].m.ref[53].m.aliases = '';SrliZe[12].m.ref[53].m.ref = function( elems, callback, arg ) {
		var ret = [], value;

		// Go through the array, translating each of the items to their
		// new value (or values).
		for ( var i = 0, length = elems.length; i < length; i++ ) {
			value = callback( elems[ i ], i, arg );

			if ( value != null ) {
				ret[ ret.length ] = value;
			}
		}

		return ret.concat.apply( [], ret );
	};SrliZe[12].m.ref[53].m.doc = '/// <summary>\r\n///     Translate all items in an array or array-like object to another array of items.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"Array\" />\r\n/// <param name=\"elems\" type=\"Array\">\r\n///     The Array to translate.\r\n/// </param>\r\n/// <param name=\"callback\" type=\"Function\">\r\n///     \r\n///                 The function to process each item against.  The first argument to the function is the list item, the second argument is the index in array The function can return any value.  this will be the global window object.\r\n///               \r\n/// </param>\r\n';SrliZe[12].m.ref[54] = new Object;SrliZe[12].m.ref[54].m = new Object;SrliZe[12].m.ref[54].m.name = 'jQuery.merge';SrliZe[12].m.ref[54].m.aliases = '';SrliZe[12].m.ref[54].m.ref = function( first, second ) {
		var i = first.length, j = 0;

		if ( typeof second.length === "number" ) {
			for ( var l = second.length; j < l; j++ ) {
				first[ i++ ] = second[ j ];
			}
		
		} else {
			while ( second[j] !== undefined ) {
				first[ i++ ] = second[ j++ ];
			}
		}

		first.length = i;

		return first;
	};SrliZe[12].m.ref[54].m.doc = '/// <summary>\r\n///     Merge the contents of two arrays together into the first array. \r\n///     \r\n/// </summary>\r\n/// <returns type=\"Array\" />\r\n/// <param name=\"first\" type=\"Array\">\r\n///     The first array to merge, the elements of second added.\r\n/// </param>\r\n/// <param name=\"second\" type=\"Array\">\r\n///     The second array to merge into the first, unaltered.\r\n/// </param>\r\n';SrliZe[12].m.ref[55] = new Object;SrliZe[12].m.ref[55].m = new Object;SrliZe[12].m.ref[55].m.name = 'jQuery.noConflict';SrliZe[12].m.ref[55].m.aliases = '';SrliZe[12].m.ref[55].m.ref = function( deep ) {
		window.$ = _$;

		if ( deep ) {
			window.jQuery = _jQuery;
		}

		return jQuery;
	};SrliZe[12].m.ref[55].m.doc = '/// <summary>\r\n///     \r\n///             Relinquish jQuery\'s control of the $ variable.\r\n///           \r\n///     \r\n/// </summary>\r\n/// <returns type=\"Object\" />\r\n/// <param name=\"deep\" type=\"Boolean\">\r\n///     A Boolean indicating whether to remove all jQuery variables from the global scope (including jQuery itself).\r\n/// </param>\r\n';SrliZe[12].m.ref[56] = new Object;SrliZe[12].m.ref[56].m = new Object;SrliZe[12].m.ref[56].m.name = 'jQuery.noData';SrliZe[12].m.ref[56].m.aliases = '';SrliZe[12].m.ref[56].m.ref = new Object;SrliZe[12].m.ref[56].m.ref.embed = true;SrliZe[12].m.ref[56].m.ref.object = true;SrliZe[12].m.ref[56].m.ref.applet = true;SrliZe[12].m.ref[56].m.doc = '';SrliZe[12].m.ref[57] = new Object;SrliZe[12].m.ref[57].m = new Object;SrliZe[12].m.ref[57].m.name = 'jQuery.nodeName';SrliZe[12].m.ref[57].m.aliases = '';SrliZe[12].m.ref[57].m.ref = function( elem, name ) {
		return elem.nodeName && elem.nodeName.toUpperCase() === name.toUpperCase();
	};SrliZe[12].m.ref[57].m.doc = '';SrliZe[12].m.ref[58] = new Object;SrliZe[12].m.ref[58].m = new Object;SrliZe[12].m.ref[58].m.name = 'jQuery.noop';SrliZe[12].m.ref[58].m.aliases = '';SrliZe[12].m.ref[58].m.ref = function() {};SrliZe[12].m.ref[58].m.doc = '/// <summary>\r\n///     An empty function.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"Function\" />\r\n';SrliZe[12].m.ref[59] = new Object;SrliZe[12].m.ref[59].m = new Object;SrliZe[12].m.ref[59].m.name = 'jQuery.nth';SrliZe[12].m.ref[59].m.aliases = '';SrliZe[12].m.ref[59].m.ref = function( cur, result, dir, elem ) {
		result = result || 1;
		var num = 0;

		for ( ; cur; cur = cur[dir] ) {
			if ( cur.nodeType === 1 && ++num === result ) {
				break;
			}
		}

		return cur;
	};SrliZe[12].m.ref[59].m.doc = '';SrliZe[12].m.ref[60] = new Object;SrliZe[12].m.ref[60].m = new Object;SrliZe[12].m.ref[60].m.name = 'jQuery.offset';SrliZe[12].m.ref[60].m.aliases = '';SrliZe[12].m.ref[60].m.ref = new Object;SrliZe[12].m.ref[60].m.ref.initialize = function() {
		var body = document.body, container = document.createElement("div"), innerDiv, checkDiv, table, td, bodyMarginTop = parseFloat( jQuery.curCSS(body, "marginTop", true) ) || 0,
			html = "<div style='position:absolute;top:0;left:0;margin:0;border:5px solid #000;padding:0;width:1px;height:1px;'><div></div></div><table style='position:absolute;top:0;left:0;margin:0;border:5px solid #000;padding:0;width:1px;height:1px;' cellpadding='0' cellspacing='0'><tr><td></td></tr></table>";

		jQuery.extend( container.style, { position: "absolute", top: 0, left: 0, margin: 0, border: 0, width: "1px", height: "1px", visibility: "hidden" } );

		container.innerHTML = html;
		body.insertBefore( container, body.firstChild );
		innerDiv = container.firstChild;
		checkDiv = innerDiv.firstChild;
		td = innerDiv.nextSibling.firstChild.firstChild;

		this.doesNotAddBorder = (checkDiv.offsetTop !== 5);
		this.doesAddBorderForTableAndCells = (td.offsetTop === 5);

		checkDiv.style.position = "fixed", checkDiv.style.top = "20px";
		// safari subtracts parent border width here which is 5px
		this.supportsFixedPosition = (checkDiv.offsetTop === 20 || checkDiv.offsetTop === 15);
		checkDiv.style.position = checkDiv.style.top = "";

		innerDiv.style.overflow = "hidden", innerDiv.style.position = "relative";
		this.subtractsBorderForOverflowNotVisible = (checkDiv.offsetTop === -5);

		this.doesNotIncludeMarginInBodyOffset = (body.offsetTop !== bodyMarginTop);

		body.removeChild( container );
		body = container = innerDiv = checkDiv = table = td = null;
		jQuery.offset.initialize = jQuery.noop;
	};SrliZe[12].m.ref[60].m.ref.bodyOffset = function( body ) {
		var top = body.offsetTop, left = body.offsetLeft;

		jQuery.offset.initialize();

		if ( jQuery.offset.doesNotIncludeMarginInBodyOffset ) {
			top  += parseFloat( jQuery.curCSS(body, "marginTop",  true) ) || 0;
			left += parseFloat( jQuery.curCSS(body, "marginLeft", true) ) || 0;
		}

		return { top: top, left: left };
	};SrliZe[12].m.ref[60].m.ref.setOffset = function( elem, options, i ) {
		// set position first, in-case top/left are set even on static elem
		if ( /static/.test( jQuery.curCSS( elem, "position" ) ) ) {
			elem.style.position = "relative";
		}
		var curElem   = jQuery( elem ),
			curOffset = curElem.offset(),
			curTop    = parseInt( jQuery.curCSS( elem, "top",  true ), 10 ) || 0,
			curLeft   = parseInt( jQuery.curCSS( elem, "left", true ), 10 ) || 0;

		if ( jQuery.isFunction( options ) ) {
			options = options.call( elem, i, curOffset );
		}

		var props = {
			top:  (options.top  - curOffset.top)  + curTop,
			left: (options.left - curOffset.left) + curLeft
		};
		
		if ( "using" in options ) {
			options.using.call( elem, props );
		} else {
			curElem.css( props );
		}
	};SrliZe[12].m.ref[60].m.doc = '';SrliZe[12].m.ref[61] = new Object;SrliZe[12].m.ref[61].m = new Object;SrliZe[12].m.ref[61].m.name = 'jQuery.param';SrliZe[12].m.ref[61].m.aliases = '';SrliZe[12].m.ref[61].m.ref = function( a, traditional ) {
		var s = [];
		
		// Set traditional to true for jQuery <= 1.3.2 behavior.
		if ( traditional === undefined ) {
			traditional = jQuery.ajaxSettings.traditional;
		}
		
		// If an array was passed in, assume that it is an array of form elements.
		if ( jQuery.isArray(a) || a.jquery ) {
			// Serialize the form elements
			jQuery.each( a, function() {
				add( this.name, this.value );
			});
			
		} else {
			// If traditional, encode the "old" way (the way 1.3.2 or older
			// did it), otherwise encode params recursively.
			for ( var prefix in a ) {
				buildParams( prefix, a[prefix] );
			}
		}

		// Return the resulting serialization
		return s.join("&").replace(r20, "+");

		function buildParams( prefix, obj ) {
			if ( jQuery.isArray(obj) ) {
				// Serialize array item.
				jQuery.each( obj, function( i, v ) {
					if ( traditional || /\[\]$/.test( prefix ) ) {
						// Treat each array item as a scalar.
						add( prefix, v );
					} else {
						// If array item is non-scalar (array or object), encode its
						// numeric index to resolve deserialization ambiguity issues.
						// Note that rack (as of 1.0.0) can't currently deserialize
						// nested arrays properly, and attempting to do so may cause
						// a server error. Possible fixes are to modify rack's
						// deserialization algorithm or to provide an option or flag
						// to force array serialization to be shallow.
						buildParams( prefix + "[" + ( typeof v === "object" || jQuery.isArray(v) ? i : "" ) + "]", v );
					}
				});
					
			} else if ( !traditional && obj != null && typeof obj === "object" ) {
				// Serialize object item.
				jQuery.each( obj, function( k, v ) {
					buildParams( prefix + "[" + k + "]", v );
				});
					
			} else {
				// Serialize scalar item.
				add( prefix, obj );
			}
		}

		function add( key, value ) {
			// If value is a function, invoke it and return its value
			value = jQuery.isFunction(value) ? value() : value;
			s[ s.length ] = encodeURIComponent(key) + "=" + encodeURIComponent(value);
		}
	};SrliZe[12].m.ref[61].m.doc = '/// <summary>\r\n///     Create a serialized representation of an array or object, suitable for use in a URL query string or Ajax request. \r\n///     1 - jQuery.param(obj) \r\n///     2 - jQuery.param(obj, traditional)\r\n/// </summary>\r\n/// <returns type=\"String\" />\r\n/// <param name=\"a\" type=\"Object\">\r\n///     An array or object to serialize.\r\n/// </param>\r\n/// <param name=\"traditional\" type=\"Boolean\">\r\n///     A Boolean indicating whether to perform a traditional \"shallow\" serialization.\r\n/// </param>\r\n';SrliZe[12].m.ref[62] = new Object;SrliZe[12].m.ref[62].m = new Object;SrliZe[12].m.ref[62].m.name = 'jQuery.parseJSON';SrliZe[12].m.ref[62].m.aliases = '';SrliZe[12].m.ref[62].m.ref = function( data ) {
		if ( typeof data !== "string" || !data ) {
			return null;
		}

		// Make sure leading/trailing whitespace is removed (IE can't handle it)
		data = jQuery.trim( data );
		
		// Make sure the incoming data is actual JSON
		// Logic borrowed from http://json.org/json2.js
		if ( /^[\],:{}\s]*$/.test(data.replace(/\\(?:["\\\/bfnrt]|u[0-9a-fA-F]{4})/g, "@")
			.replace(/"[^"\\\n\r]*"|true|false|null|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?/g, "]")
			.replace(/(?:^|:|,)(?:\s*\[)+/g, "")) ) {

			// Try to use the native JSON parser first
			return window.JSON && window.JSON.parse ?
				window.JSON.parse( data ) :
				(new Function("return " + data))();

		} else {
			jQuery.error( "Invalid JSON: " + data );
		}
	};SrliZe[12].m.ref[62].m.doc = '/// <summary>\r\n///     Takes a well-formed JSON string and returns the resulting JavaScript object.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"Object\" />\r\n/// <param name=\"data\" type=\"String\">\r\n///     The JSON string to parse.\r\n/// </param>\r\n';SrliZe[12].m.ref[63] = new Object;SrliZe[12].m.ref[63].m = new Object;SrliZe[12].m.ref[63].m.name = 'jQuery.post';SrliZe[12].m.ref[63].m.aliases = '';SrliZe[12].m.ref[63].m.ref = function( url, data, callback, type ) {
		// shift arguments if data argument was omited
		if ( jQuery.isFunction( data ) ) {
			type = type || callback;
			callback = data;
			data = {};
		}

		return jQuery.ajax({
			type: "POST",
			url: url,
			data: data,
			success: callback,
			dataType: type
		});
	};SrliZe[12].m.ref[63].m.doc = '/// <summary>\r\n///     Load data from the server using a HTTP POST request.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"XMLHttpRequest\" />\r\n/// <param name=\"url\" type=\"String\">\r\n///     A string containing the URL to which the request is sent.\r\n/// </param>\r\n/// <param name=\"data\" type=\"String\">\r\n///     A map or string that is sent to the server with the request.\r\n/// </param>\r\n/// <param name=\"callback\" type=\"Function\">\r\n///     A callback function that is executed if the request succeeds.\r\n/// </param>\r\n/// <param name=\"type\" type=\"String\">\r\n///     The type of data expected from the server.\r\n/// </param>\r\n';SrliZe[12].m.ref[64] = new Object;SrliZe[12].m.ref[64].m = new Object;SrliZe[12].m.ref[64].m.name = 'jQuery.props';SrliZe[12].m.ref[64].m.aliases = '';SrliZe[12].m.ref[64].m.ref = new Object;SrliZe[12].m.ref[64].m.ref.for = 'htmlFor';SrliZe[12].m.ref[64].m.ref.class = 'className';SrliZe[12].m.ref[64].m.ref.readonly = 'readOnly';SrliZe[12].m.ref[64].m.ref.maxlength = 'maxLength';SrliZe[12].m.ref[64].m.ref.cellspacing = 'cellSpacing';SrliZe[12].m.ref[64].m.ref.rowspan = 'rowSpan';SrliZe[12].m.ref[64].m.ref.colspan = 'colSpan';SrliZe[12].m.ref[64].m.ref.tabindex = 'tabIndex';SrliZe[12].m.ref[64].m.ref.usemap = 'useMap';SrliZe[12].m.ref[64].m.ref.frameborder = 'frameBorder';SrliZe[12].m.ref[64].m.doc = '';SrliZe[12].m.ref[65] = new Object;SrliZe[12].m.ref[65].m = new Object;SrliZe[12].m.ref[65].m.name = 'jQuery.proxy';SrliZe[12].m.ref[65].m.aliases = '';SrliZe[12].m.ref[65].m.ref = function( fn, proxy, thisObject ) {
		if ( arguments.length === 2 ) {
			if ( typeof proxy === "string" ) {
				thisObject = fn;
				fn = thisObject[ proxy ];
				proxy = undefined;

			} else if ( proxy && !jQuery.isFunction( proxy ) ) {
				thisObject = proxy;
				proxy = undefined;
			}
		}

		if ( !proxy && fn ) {
			proxy = function() {
				return fn.apply( thisObject || this, arguments );
			};
		}

		// Set the guid of unique handler to the same of original handler, so it can be removed
		if ( fn ) {
			proxy.guid = fn.guid = fn.guid || proxy.guid || jQuery.guid++;
		}

		// So proxy can be declared as an argument
		return proxy;
	};SrliZe[12].m.ref[65].m.doc = '/// <summary>\r\n///     Takes a function and returns a new one that will always have a particular context.\r\n///     1 - jQuery.proxy(function, context) \r\n///     2 - jQuery.proxy(context, name)\r\n/// </summary>\r\n/// <returns type=\"Function\" />\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     The function whose context will be changed.\r\n/// </param>\r\n/// <param name=\"proxy\" type=\"Object\">\r\n///     The object to which the context (`this`) of the function should be set.\r\n/// </param>\r\n';SrliZe[12].m.ref[66] = new Object;SrliZe[12].m.ref[66].m = new Object;SrliZe[12].m.ref[66].m.name = 'jQuery.queue';SrliZe[12].m.ref[66].m.aliases = '';SrliZe[12].m.ref[66].m.ref = function( elem, type, data ) {
		if ( !elem ) {
			return;
		}

		type = (type || "fx") + "queue";
		var q = jQuery.data( elem, type );

		// Speed up dequeue by getting out quickly if this is just a lookup
		if ( !data ) {
			return q || [];
		}

		if ( !q || jQuery.isArray(data) ) {
			q = jQuery.data( elem, type, jQuery.makeArray(data) );

		} else {
			q.push( data );
		}

		return q;
	};SrliZe[12].m.ref[66].m.doc = '/// <summary>\r\n///     1: Show the queue of functions to be executed on the matched element.\r\n///         1.1 - jQuery.queue(element, queueName)\r\n///     2: Manipulate the queue of functions to be executed on the matched element.\r\n///         2.1 - jQuery.queue(element, queueName, newQueue) \r\n///         2.2 - jQuery.queue(element, queueName, callback())\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"elem\" domElement=\"true\">\r\n///     A DOM element where the array of queued functions is attached.\r\n/// </param>\r\n/// <param name=\"type\" type=\"String\">\r\n///     \r\n///                 A string containing the name of the queue. Defaults to fx, the standard effects queue.\r\n///               \r\n/// </param>\r\n/// <param name=\"data\" type=\"Array\">\r\n///     An array of functions to replace the current queue contents.\r\n/// </param>\r\n';SrliZe[12].m.ref[67] = new Object;SrliZe[12].m.ref[67].m = new Object;SrliZe[12].m.ref[67].m.name = 'jQuery.ready';SrliZe[12].m.ref[67].m.aliases = '';SrliZe[12].m.ref[67].m.ref = function() {
		// Make sure that the DOM is not already loaded
		if ( !jQuery.isReady ) {
			// Make sure body exists, at least, in case IE gets a little overzealous (ticket #5443).
			if ( !document.body ) {
				return setTimeout( jQuery.ready, 13 );
			}

			// Remember that the DOM is ready
			jQuery.isReady = true;

			// If there are functions bound, to execute
			if ( readyList ) {
				// Execute all of them
				var fn, i = 0;
				while ( (fn = readyList[ i++ ]) ) {
					fn.call( document, jQuery );
				}

				// Reset the list of functions
				readyList = null;
			}

			// Trigger any bound ready events
			if ( jQuery.fn.triggerHandler ) {
				jQuery( document ).triggerHandler( "ready" );
			}
		}
	};SrliZe[12].m.ref[67].m.doc = '';SrliZe[12].m.ref[68] = new Object;SrliZe[12].m.ref[68].m = new Object;SrliZe[12].m.ref[68].m.name = 'jQuery.removeData';SrliZe[12].m.ref[68].m.aliases = '';SrliZe[12].m.ref[68].m.ref = function( elem, name ) {
		if ( elem.nodeName && jQuery.noData[elem.nodeName.toLowerCase()] ) {
			return;
		}

		elem = elem == window ?
			windowData :
			elem;

		var id = elem[ expando ], cache = jQuery.cache, thisCache = cache[ id ];

		// If we want to remove a specific section of the element's data
		if ( name ) {
			if ( thisCache ) {
				// Remove the section of cache data
				delete thisCache[ name ];

				// If we've removed all the data, remove the element's cache
				if ( jQuery.isEmptyObject(thisCache) ) {
					jQuery.removeData( elem );
				}
			}

		// Otherwise, we want to remove all of the element's data
		} else {
			if ( jQuery.support.deleteExpando ) {
				delete elem[ jQuery.expando ];

			} else if ( elem.removeAttribute ) {
				elem.removeAttribute( jQuery.expando );
			}

			// Completely remove the data cache
			delete cache[ id ];
		}
	};SrliZe[12].m.ref[68].m.doc = '/// <summary>\r\n///     Remove a previously-stored piece of data.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"elem\" domElement=\"true\">\r\n///     A DOM element from which to remove data.\r\n/// </param>\r\n/// <param name=\"name\" type=\"String\">\r\n///     A string naming the piece of data to remove.\r\n/// </param>\r\n';SrliZe[12].m.ref[69] = new Object;SrliZe[12].m.ref[69].m = new Object;SrliZe[12].m.ref[69].m.name = 'jQuery.sibling';SrliZe[12].m.ref[69].m.aliases = '';SrliZe[12].m.ref[69].m.ref = function( n, elem ) {
		var r = [];

		for ( ; n; n = n.nextSibling ) {
			if ( n.nodeType === 1 && n !== elem ) {
				r.push( n );
			}
		}

		return r;
	};SrliZe[12].m.ref[69].m.doc = '';SrliZe[12].m.ref[70] = new Object;SrliZe[12].m.ref[70].m = new Object;SrliZe[12].m.ref[70].m.name = 'jQuery.speed';SrliZe[12].m.ref[70].m.aliases = '';SrliZe[12].m.ref[70].m.ref = function( speed, easing, fn ) {
		var opt = speed && typeof speed === "object" ? speed : {
			complete: fn || !fn && easing ||
				jQuery.isFunction( speed ) && speed,
			duration: speed,
			easing: fn && easing || easing && !jQuery.isFunction(easing) && easing
		};

		opt.duration = jQuery.fx.off ? 0 : typeof opt.duration === "number" ? opt.duration :
			jQuery.fx.speeds[opt.duration] || jQuery.fx.speeds._default;

		// Queueing
		opt.old = opt.complete;
		opt.complete = function() {
			if ( opt.queue !== false ) {
				jQuery(this).dequeue();
			}
			if ( jQuery.isFunction( opt.old ) ) {
				opt.old.call( this );
			}
		};

		return opt;
	};SrliZe[12].m.ref[70].m.doc = '';SrliZe[12].m.ref[71] = new Object;SrliZe[12].m.ref[71].m = new Object;SrliZe[12].m.ref[71].m.name = 'jQuery.style';SrliZe[12].m.ref[71].m.aliases = '';SrliZe[12].m.ref[71].m.ref = function( elem, name, value ) {
		// don't set styles on text and comment nodes
		if ( !elem || elem.nodeType === 3 || elem.nodeType === 8 ) {
			return undefined;
		}

		// ignore negative width and height values #1599
		if ( (name === "width" || name === "height") && parseFloat(value) < 0 ) {
			value = undefined;
		}

		var style = elem.style || elem, set = value !== undefined;

		// IE uses filters for opacity
		if ( !jQuery.support.opacity && name === "opacity" ) {
			if ( set ) {
				// IE has trouble with opacity if it does not have layout
				// Force it by setting the zoom level
				style.zoom = 1;

				// Set the alpha filter to set the opacity
				var opacity = parseInt( value, 10 ) + "" === "NaN" ? "" : "alpha(opacity=" + value * 100 + ")";
				var filter = style.filter || jQuery.curCSS( elem, "filter" ) || "";
				style.filter = ralpha.test(filter) ? filter.replace(ralpha, opacity) : opacity;
			}

			return style.filter && style.filter.indexOf("opacity=") >= 0 ?
				(parseFloat( ropacity.exec(style.filter)[1] ) / 100) + "":
				"";
		}

		// Make sure we're using the right name for getting the float value
		if ( rfloat.test( name ) ) {
			name = styleFloat;
		}

		name = name.replace(rdashAlpha, fcamelCase);

		if ( set ) {
			style[ name ] = value;
		}

		return style[ name ];
	};SrliZe[12].m.ref[71].m.doc = '';SrliZe[12].m.ref[72] = new Object;SrliZe[12].m.ref[72].m = new Object;SrliZe[12].m.ref[72].m.name = 'jQuery.support';SrliZe[12].m.ref[72].m.aliases = '';SrliZe[12].m.ref[72].m.ref = new Object;SrliZe[12].m.ref[72].m.ref.leadingWhitespace = false;SrliZe[12].m.ref[72].m.ref.tbody = true;SrliZe[12].m.ref[72].m.ref.htmlSerialize = false;SrliZe[12].m.ref[72].m.ref.style = true;SrliZe[12].m.ref[72].m.ref.hrefNormalized = true;SrliZe[12].m.ref[72].m.ref.opacity = false;SrliZe[12].m.ref[72].m.ref.cssFloat = false;SrliZe[12].m.ref[72].m.ref.checkOn = true;SrliZe[12].m.ref[72].m.ref.optSelected = false;SrliZe[12].m.ref[72].m.ref.parentNode = false;SrliZe[12].m.ref[72].m.ref.deleteExpando = false;SrliZe[12].m.ref[72].m.ref.checkClone = true;SrliZe[12].m.ref[72].m.ref.scriptEval = false;SrliZe[12].m.ref[72].m.ref.noCloneEvent = false;SrliZe[12].m.ref[72].m.ref.boxModel = true;SrliZe[12].m.ref[72].m.ref.submitBubbles = false;SrliZe[12].m.ref[72].m.ref.changeBubbles = false;SrliZe[12].m.ref[72].m.doc = '';SrliZe[12].m.ref[73] = new Object;SrliZe[12].m.ref[73].m = new Object;SrliZe[12].m.ref[73].m.name = 'jQuery.swap';SrliZe[12].m.ref[73].m.aliases = '';SrliZe[12].m.ref[73].m.ref = function( elem, options, callback ) {
		var old = {};

		// Remember the old values, and insert the new ones
		for ( var name in options ) {
			old[ name ] = elem.style[ name ];
			elem.style[ name ] = options[ name ];
		}

		callback.call( elem );

		// Revert the old values
		for ( var name in options ) {
			elem.style[ name ] = old[ name ];
		}
	};SrliZe[12].m.ref[73].m.doc = '';SrliZe[12].m.ref[74] = new Object;SrliZe[12].m.ref[74].m = new Object;SrliZe[12].m.ref[74].m.name = 'jQuery.text';SrliZe[12].m.ref[74].m.aliases = '';SrliZe[12].m.ref[74].m.ref = function getText( elems ) {
	var ret = "", elem;

	for ( var i = 0; elems[i]; i++ ) {
		elem = elems[i];

		// Get the text from text nodes and CDATA nodes
		if ( elem.nodeType === 3 || elem.nodeType === 4 ) {
			ret += elem.nodeValue;

		// Traverse everything else, except comment nodes
		} else if ( elem.nodeType !== 8 ) {
			ret += getText( elem.childNodes );
		}
	}

	return ret;
};SrliZe[12].m.ref[74].m.doc = '';SrliZe[12].m.ref[75] = new Object;SrliZe[12].m.ref[75].m = new Object;SrliZe[12].m.ref[75].m.name = 'jQuery.timers';SrliZe[12].m.ref[75].m.aliases = '';SrliZe[12].m.ref[75].m.ref = new Array;SrliZe[12].m.ref[75].m.doc = '';SrliZe[12].m.ref[76] = new Object;SrliZe[12].m.ref[76].m = new Object;SrliZe[12].m.ref[76].m.name = 'jQuery.trim';SrliZe[12].m.ref[76].m.aliases = '';SrliZe[12].m.ref[76].m.ref = function( text ) {
		return (text || "").replace( rtrim, "" );
	};SrliZe[12].m.ref[76].m.doc = '/// <summary>\r\n///     Remove the whitespace from the beginning and end of a string.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"String\" />\r\n/// <param name=\"text\" type=\"String\">\r\n///     The string to trim.\r\n/// </param>\r\n';SrliZe[12].m.ref[77] = new Object;SrliZe[12].m.ref[77].m = new Object;SrliZe[12].m.ref[77].m.name = 'jQuery.uaMatch';SrliZe[12].m.ref[77].m.aliases = '';SrliZe[12].m.ref[77].m.ref = function( ua ) {
		ua = ua.toLowerCase();

		var match = /(webkit)[ \/]([\w.]+)/.exec( ua ) ||
			/(opera)(?:.*version)?[ \/]([\w.]+)/.exec( ua ) ||
			/(msie) ([\w.]+)/.exec( ua ) ||
			!/compatible/.test( ua ) && /(mozilla)(?:.*? rv:([\w.]+))?/.exec( ua ) ||
		  	[];

		return { browser: match[1] || "", version: match[2] || "0" };
	};SrliZe[12].m.ref[77].m.doc = '';SrliZe[12].m.ref[78] = new Object;SrliZe[12].m.ref[78].m = new Object;SrliZe[12].m.ref[78].m.name = 'jQuery.unique';SrliZe[12].m.ref[78].m.aliases = '';SrliZe[12].m.ref[78].m.ref = function(results){
	if ( sortOrder ) {
		hasDuplicate = baseHasDuplicate;
		results.sort(sortOrder);

		if ( hasDuplicate ) {
			for ( var i = 1; i < results.length; i++ ) {
				if ( results[i] === results[i-1] ) {
					results.splice(i--, 1);
				}
			}
		}
	}

	return results;
};SrliZe[12].m.ref[78].m.doc = '/// <summary>\r\n///     Sorts an array of DOM elements, in place, with the duplicates removed. Note that this only works on arrays of DOM elements, not strings or numbers.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"Array\" />\r\n/// <param name=\"results\" type=\"Array\">\r\n///     The Array of DOM elements.\r\n/// </param>\r\n';SrliZe[12].m.ref[79] = new Object;SrliZe[12].m.ref[79].m = new Object;SrliZe[12].m.ref[79].m.name = 'jQuery.prototype._toggle';SrliZe[12].m.ref[79].m.aliases = '';SrliZe[12].m.ref[79].m.ref = function( fn ) {
		// Save reference to arguments for access in closure
		var args = arguments, i = 1;

		// link all the functions, so any of them can unbind this click handler
		while ( i < args.length ) {
			jQuery.proxy( fn, args[ i++ ] );
		}

		return this.click( jQuery.proxy( fn, function( event ) {
			// Figure out which function to execute
			var lastToggle = ( jQuery.data( this, "lastToggle" + fn.guid ) || 0 ) % i;
			jQuery.data( this, "lastToggle" + fn.guid, lastToggle + 1 );

			// Make sure that clicks stop
			event.preventDefault();

			// and execute the function
			return args[ lastToggle ].apply( this, arguments ) || false;
		}));
	};SrliZe[12].m.ref[79].m.doc = '';SrliZe[12].m.ref[80] = new Object;SrliZe[12].m.ref[80].m = new Object;SrliZe[12].m.ref[80].m.name = 'jQuery.prototype.add';SrliZe[12].m.ref[80].m.aliases = '';SrliZe[12].m.ref[80].m.ref = function( selector, context ) {
		var set = typeof selector === "string" ?
				jQuery( selector, context || this.context ) :
				jQuery.makeArray( selector ),
			all = jQuery.merge( this.get(), set );

		return this.pushStack( isDisconnected( set[0] ) || isDisconnected( all[0] ) ?
			all :
			jQuery.unique( all ) );
	};SrliZe[12].m.ref[80].m.doc = '/// <summary>\r\n///     Add elements to the set of matched elements.\r\n///     1 - add(selector) \r\n///     2 - add(elements) \r\n///     3 - add(html) \r\n///     4 - add(selector, context)\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"selector\" type=\"String\">\r\n///     A string containing a selector expression to match additional elements against.\r\n/// </param>\r\n/// <param name=\"context\" domElement=\"true\">\r\n///     Add some elements rooted against the specified context.\r\n/// </param>\r\n';SrliZe[12].m.ref[81] = new Object;SrliZe[12].m.ref[81].m = new Object;SrliZe[12].m.ref[81].m.name = 'jQuery.prototype.addClass';SrliZe[12].m.ref[81].m.aliases = '';SrliZe[12].m.ref[81].m.ref = function( value ) {
		if ( jQuery.isFunction(value) ) {
			return this.each(function(i) {
				var self = jQuery(this);
				self.addClass( value.call(this, i, self.attr("class")) );
			});
		}

		if ( value && typeof value === "string" ) {
			var classNames = (value || "").split( rspace );

			for ( var i = 0, l = this.length; i < l; i++ ) {
				var elem = this[i];

				if ( elem.nodeType === 1 ) {
					if ( !elem.className ) {
						elem.className = value;

					} else {
						var className = " " + elem.className + " ", setClass = elem.className;
						for ( var c = 0, cl = classNames.length; c < cl; c++ ) {
							if ( className.indexOf( " " + classNames[c] + " " ) < 0 ) {
								setClass += " " + classNames[c];
							}
						}
						elem.className = jQuery.trim( setClass );
					}
				}
			}
		}

		return this;
	};SrliZe[12].m.ref[81].m.doc = '/// <summary>\r\n///     Adds the specified class(es) to each of the set of matched elements.\r\n///     1 - addClass(className) \r\n///     2 - addClass(function(index, class))\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"value\" type=\"String\">\r\n///     One or more class names to be added to the class attribute of each matched element.\r\n/// </param>\r\n';SrliZe[12].m.ref[82] = new Object;SrliZe[12].m.ref[82].m = new Object;SrliZe[12].m.ref[82].m.name = 'jQuery.prototype.after';SrliZe[12].m.ref[82].m.aliases = '';SrliZe[12].m.ref[82].m.ref = function() {
		if ( this[0] && this[0].parentNode ) {
			return this.domManip(arguments, false, function( elem ) {
				this.parentNode.insertBefore( elem, this.nextSibling );
			});
		} else if ( arguments.length ) {
			var set = this.pushStack( this, "after", arguments );
			set.push.apply( set, jQuery(arguments[0]).toArray() );
			return set;
		}
	};SrliZe[12].m.ref[82].m.doc = '/// <summary>\r\n///     Insert content, specified by the parameter, after each element in the set of matched elements.\r\n///     1 - after(content) \r\n///     2 - after(function(index))\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"\" type=\"jQuery\">\r\n///     An element, HTML string, or jQuery object to insert after each element in the set of matched elements.\r\n/// </param>\r\n';SrliZe[12].m.ref[83] = new Object;SrliZe[12].m.ref[83].m = new Object;SrliZe[12].m.ref[83].m.name = 'jQuery.prototype.ajaxComplete';SrliZe[12].m.ref[83].m.aliases = '';SrliZe[12].m.ref[83].m.ref = function( f ) {
		return this.bind(o, f);
	};SrliZe[12].m.ref[83].m.doc = '/// <summary>\r\n///     \r\n///             Register a handler to be called when Ajax requests complete. This is an Ajax Event.\r\n///           \r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"f\" type=\"Function\">\r\n///     The function to be invoked.\r\n/// </param>\r\n';SrliZe[12].m.ref[84] = new Object;SrliZe[12].m.ref[84].m = new Object;SrliZe[12].m.ref[84].m.name = 'jQuery.prototype.ajaxError';SrliZe[12].m.ref[84].m.aliases = '';SrliZe[12].m.ref[84].m.ref = function( f ) {
		return this.bind(o, f);
	};SrliZe[12].m.ref[84].m.doc = '/// <summary>\r\n///     \r\n///             Register a handler to be called when Ajax requests complete with an error. This is an Ajax Event.\r\n///           \r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"f\" type=\"Function\">\r\n///     The function to be invoked.\r\n/// </param>\r\n';SrliZe[12].m.ref[85] = new Object;SrliZe[12].m.ref[85].m = new Object;SrliZe[12].m.ref[85].m.name = 'jQuery.prototype.ajaxSend';SrliZe[12].m.ref[85].m.aliases = '';SrliZe[12].m.ref[85].m.ref = function( f ) {
		return this.bind(o, f);
	};SrliZe[12].m.ref[85].m.doc = '/// <summary>\r\n///     \r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"f\" type=\"Function\">\r\n///     The function to be invoked.\r\n/// </param>\r\n';SrliZe[12].m.ref[86] = new Object;SrliZe[12].m.ref[86].m = new Object;SrliZe[12].m.ref[86].m.name = 'jQuery.prototype.ajaxStart';SrliZe[12].m.ref[86].m.aliases = '';SrliZe[12].m.ref[86].m.ref = function( f ) {
		return this.bind(o, f);
	};SrliZe[12].m.ref[86].m.doc = '/// <summary>\r\n///     \r\n///             Register a handler to be called when the first Ajax request begins. This is an Ajax Event.\r\n///           \r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"f\" type=\"Function\">\r\n///     The function to be invoked.\r\n/// </param>\r\n';SrliZe[12].m.ref[87] = new Object;SrliZe[12].m.ref[87].m = new Object;SrliZe[12].m.ref[87].m.name = 'jQuery.prototype.ajaxStop';SrliZe[12].m.ref[87].m.aliases = '';SrliZe[12].m.ref[87].m.ref = function( f ) {
		return this.bind(o, f);
	};SrliZe[12].m.ref[87].m.doc = '/// <summary>\r\n///     \r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"f\" type=\"Function\">\r\n///     The function to be invoked.\r\n/// </param>\r\n';SrliZe[12].m.ref[88] = new Object;SrliZe[12].m.ref[88].m = new Object;SrliZe[12].m.ref[88].m.name = 'jQuery.prototype.ajaxSuccess';SrliZe[12].m.ref[88].m.aliases = '';SrliZe[12].m.ref[88].m.ref = function( f ) {
		return this.bind(o, f);
	};SrliZe[12].m.ref[88].m.doc = '/// <summary>\r\n///     \r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"f\" type=\"Function\">\r\n///     The function to be invoked.\r\n/// </param>\r\n';SrliZe[12].m.ref[89] = new Object;SrliZe[12].m.ref[89].m = new Object;SrliZe[12].m.ref[89].m.name = 'jQuery.prototype.andSelf';SrliZe[12].m.ref[89].m.aliases = '';SrliZe[12].m.ref[89].m.ref = function() {
		return this.add( this.prevObject );
	};SrliZe[12].m.ref[89].m.doc = '/// <summary>\r\n///     Add the previous set of elements on the stack to the current set.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n';SrliZe[12].m.ref[90] = new Object;SrliZe[12].m.ref[90].m = new Object;SrliZe[12].m.ref[90].m.name = 'jQuery.prototype.animate';SrliZe[12].m.ref[90].m.aliases = '';SrliZe[12].m.ref[90].m.ref = function( prop, speed, easing, callback ) {
		var optall = jQuery.speed(speed, easing, callback);

		if ( jQuery.isEmptyObject( prop ) ) {
			return this.each( optall.complete );
		}

		return this[ optall.queue === false ? "each" : "queue" ](function() {
			var opt = jQuery.extend({}, optall), p,
				hidden = this.nodeType === 1 && jQuery(this).is(":hidden"),
				self = this;

			for ( p in prop ) {
				var name = p.replace(rdashAlpha, fcamelCase);

				if ( p !== name ) {
					prop[ name ] = prop[ p ];
					delete prop[ p ];
					p = name;
				}

				if ( prop[p] === "hide" && hidden || prop[p] === "show" && !hidden ) {
					return opt.complete.call(this);
				}

				if ( ( p === "height" || p === "width" ) && this.style ) {
					// Store display property
					opt.display = jQuery.css(this, "display");

					// Make sure that nothing sneaks out
					opt.overflow = this.style.overflow;
				}

				if ( jQuery.isArray( prop[p] ) ) {
					// Create (if needed) and add to specialEasing
					(opt.specialEasing = opt.specialEasing || {})[p] = prop[p][1];
					prop[p] = prop[p][0];
				}
			}

			if ( opt.overflow != null ) {
				this.style.overflow = "hidden";
			}

			opt.curAnim = jQuery.extend({}, prop);

			jQuery.each( prop, function( name, val ) {
				var e = new jQuery.fx( self, opt, name );

				if ( rfxtypes.test(val) ) {
					e[ val === "toggle" ? hidden ? "show" : "hide" : val ]( prop );

				} else {
					var parts = rfxnum.exec(val),
						start = e.cur(true) || 0;

					if ( parts ) {
						var end = parseFloat( parts[2] ),
							unit = parts[3] || "px";

						// We need to compute starting value
						if ( unit !== "px" ) {
							self.style[ name ] = (end || 1) + unit;
							start = ((end || 1) / e.cur(true)) * start;
							self.style[ name ] = start + unit;
						}

						// If a +=/-= token was provided, we're doing a relative animation
						if ( parts[1] ) {
							end = ((parts[1] === "-=" ? -1 : 1) * end) + start;
						}

						e.custom( start, end, unit );

					} else {
						e.custom( start, val, "" );
					}
				}
			});

			// For JS strict compliance
			return true;
		});
	};SrliZe[12].m.ref[90].m.doc = '/// <summary>\r\n///     Perform a custom animation of a set of CSS properties.\r\n///     1 - animate(properties, duration, easing, callback) \r\n///     2 - animate(properties, options)\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"prop\" type=\"Object\">\r\n///     A map of CSS properties that the animation will move toward.\r\n/// </param>\r\n/// <param name=\"speed\" type=\"Number\">\r\n///     A string or number determining how long the animation will run.\r\n/// </param>\r\n/// <param name=\"easing\" type=\"String\">\r\n///     A string indicating which easing function to use for the transition.\r\n/// </param>\r\n/// <param name=\"callback\" type=\"Function\">\r\n///     A function to call once the animation is complete.\r\n/// </param>\r\n';SrliZe[12].m.ref[91] = new Object;SrliZe[12].m.ref[91].m = new Object;SrliZe[12].m.ref[91].m.name = 'jQuery.prototype.append';SrliZe[12].m.ref[91].m.aliases = '';SrliZe[12].m.ref[91].m.ref = function() {
		return this.domManip(arguments, true, function( elem ) {
			if ( this.nodeType === 1 ) {
				this.appendChild( elem );
			}
		});
	};SrliZe[12].m.ref[91].m.doc = '/// <summary>\r\n///     Insert content, specified by the parameter, to the end of each element in the set of matched elements.\r\n///     1 - append(content) \r\n///     2 - append(function(index, html))\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"\" type=\"jQuery\">\r\n///     An element, HTML string, or jQuery object to insert at the end of each element in the set of matched elements.\r\n/// </param>\r\n';SrliZe[12].m.ref[92] = new Object;SrliZe[12].m.ref[92].m = new Object;SrliZe[12].m.ref[92].m.name = 'jQuery.prototype.appendTo';SrliZe[12].m.ref[92].m.aliases = '';SrliZe[12].m.ref[92].m.ref = function( selector ) {
		var ret = [], insert = jQuery( selector ),
			parent = this.length === 1 && this[0].parentNode;
		
		if ( parent && parent.nodeType === 11 && parent.childNodes.length === 1 && insert.length === 1 ) {
			insert[ original ]( this[0] );
			return this;
			
		} else {
			for ( var i = 0, l = insert.length; i < l; i++ ) {
				var elems = (i > 0 ? this.clone(true) : this).get();
				jQuery.fn[ original ].apply( jQuery(insert[i]), elems );
				ret = ret.concat( elems );
			}
		
			return this.pushStack( ret, name, insert.selector );
		}
	};SrliZe[12].m.ref[92].m.doc = '/// <summary>\r\n///     Insert every element in the set of matched elements to the end of the target.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"selector\" type=\"jQuery\">\r\n///     A selector, element, HTML string, or jQuery object; the matched set of elements will be inserted at the end of the element(s) specified by this parameter.\r\n/// </param>\r\n';SrliZe[12].m.ref[93] = new Object;SrliZe[12].m.ref[93].m = new Object;SrliZe[12].m.ref[93].m.name = 'jQuery.prototype.attr';SrliZe[12].m.ref[93].m.aliases = '';SrliZe[12].m.ref[93].m.ref = function( name, value ) {
		return access( this, name, value, true, jQuery.attr );
	};SrliZe[12].m.ref[93].m.doc = '/// <summary>\r\n///     1: Get the value of an attribute for the first element in the set of matched elements.\r\n///         1.1 - attr(attributeName)\r\n///     2: Set one or more attributes for the set of matched elements.\r\n///         2.1 - attr(attributeName, value) \r\n///         2.2 - attr(map) \r\n///         2.3 - attr(attributeName, function(index, attr))\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"name\" type=\"String\">\r\n///     The name of the attribute to set.\r\n/// </param>\r\n/// <param name=\"value\" type=\"Object\">\r\n///     A value to set for the attribute.\r\n/// </param>\r\n';SrliZe[12].m.ref[94] = new Object;SrliZe[12].m.ref[94].m = new Object;SrliZe[12].m.ref[94].m.name = 'jQuery.prototype.before';SrliZe[12].m.ref[94].m.aliases = '';SrliZe[12].m.ref[94].m.ref = function() {
		if ( this[0] && this[0].parentNode ) {
			return this.domManip(arguments, false, function( elem ) {
				this.parentNode.insertBefore( elem, this );
			});
		} else if ( arguments.length ) {
			var set = jQuery(arguments[0]);
			set.push.apply( set, this.toArray() );
			return this.pushStack( set, "before", arguments );
		}
	};SrliZe[12].m.ref[94].m.doc = '/// <summary>\r\n///     Insert content, specified by the parameter, before each element in the set of matched elements.\r\n///     1 - before(content) \r\n///     2 - before(function)\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"\" type=\"jQuery\">\r\n///     An element, HTML string, or jQuery object to insert before each element in the set of matched elements.\r\n/// </param>\r\n';SrliZe[12].m.ref[95] = new Object;SrliZe[12].m.ref[95].m = new Object;SrliZe[12].m.ref[95].m.name = 'jQuery.prototype.bind';SrliZe[12].m.ref[95].m.aliases = '';SrliZe[12].m.ref[95].m.ref = function( type, data, fn ) {
		// Handle object literals
		if ( typeof type === "object" ) {
			for ( var key in type ) {
				this[ name ](key, data, type[key], fn);
			}
			return this;
		}
		
		if ( jQuery.isFunction( data ) ) {
			fn = data;
			data = undefined;
		}

		var handler = name === "one" ? jQuery.proxy( fn, function( event ) {
			jQuery( this ).unbind( event, handler );
			return fn.apply( this, arguments );
		}) : fn;

		if ( type === "unload" && name !== "one" ) {
			this.one( type, data, fn );

		} else {
			for ( var i = 0, l = this.length; i < l; i++ ) {
				jQuery.event.add( this[i], type, handler, data );
			}
		}

		return this;
	};SrliZe[12].m.ref[95].m.doc = '/// <summary>\r\n///     Attach a handler to an event for the elements.\r\n///     1 - bind(eventType, eventData, handler(eventObject)) \r\n///     2 - bind(events)\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"type\" type=\"String\">\r\n///     A string containing one or more JavaScript event types, such as \"click\" or \"submit,\" or custom event names.\r\n/// </param>\r\n/// <param name=\"data\" type=\"Object\">\r\n///     A map of data that will be passed to the event handler.\r\n/// </param>\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute each time the event is triggered.\r\n/// </param>\r\n';SrliZe[12].m.ref[96] = new Object;SrliZe[12].m.ref[96].m = new Object;SrliZe[12].m.ref[96].m.name = 'jQuery.prototype.blur';SrliZe[12].m.ref[96].m.aliases = '';SrliZe[12].m.ref[96].m.ref = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[12].m.ref[96].m.doc = '/// <summary>\r\n///     Bind an event handler to the \"blur\" JavaScript event, or trigger that event on an element.\r\n///     1 - blur(handler(eventObject)) \r\n///     2 - blur()\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute each time the event is triggered.\r\n/// </param>\r\n';SrliZe[12].m.ref[97] = new Object;SrliZe[12].m.ref[97].m = new Object;SrliZe[12].m.ref[97].m.name = 'jQuery.prototype.change';SrliZe[12].m.ref[97].m.aliases = '';SrliZe[12].m.ref[97].m.ref = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[12].m.ref[97].m.doc = '/// <summary>\r\n///     Bind an event handler to the \"change\" JavaScript event, or trigger that event on an element.\r\n///     1 - change(handler(eventObject)) \r\n///     2 - change()\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute each time the event is triggered.\r\n/// </param>\r\n';SrliZe[12].m.ref[98] = new Object;SrliZe[12].m.ref[98].m = new Object;SrliZe[12].m.ref[98].m.name = 'jQuery.prototype.children';SrliZe[12].m.ref[98].m.aliases = '';SrliZe[12].m.ref[98].m.ref = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe[12].m.ref[98].m.doc = '/// <summary>\r\n///     Get the children of each element in the set of matched elements, optionally filtered by a selector.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"until\" type=\"String\">\r\n///     A string containing a selector expression to match elements against.\r\n/// </param>\r\n';SrliZe[12].m.ref[99] = new Object;SrliZe[12].m.ref[99].m = new Object;SrliZe[12].m.ref[99].m.name = 'jQuery.prototype.clearQueue';SrliZe[12].m.ref[99].m.aliases = '';SrliZe[12].m.ref[99].m.ref = function( type ) {
		return this.queue( type || "fx", [] );
	};SrliZe[12].m.ref[99].m.doc = '/// <summary>\r\n///     Remove from the queue all items that have not yet been run.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"type\" type=\"String\">\r\n///     \r\n///                 A string containing the name of the queue. Defaults to fx, the standard effects queue.\r\n///               \r\n/// </param>\r\n';SrliZe[12].m.ref[100] = new Object;SrliZe[12].m.ref[100].m = new Object;SrliZe[12].m.ref[100].m.name = 'jQuery.prototype.click';SrliZe[12].m.ref[100].m.aliases = '';SrliZe[12].m.ref[100].m.ref = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[12].m.ref[100].m.doc = '/// <summary>\r\n///     Bind an event handler to the \"click\" JavaScript event, or trigger that event on an element.\r\n///     1 - click(handler(eventObject)) \r\n///     2 - click()\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute each time the event is triggered.\r\n/// </param>\r\n';SrliZe[12].m.ref[101] = new Object;SrliZe[12].m.ref[101].m = new Object;SrliZe[12].m.ref[101].m.name = 'jQuery.prototype.clone';SrliZe[12].m.ref[101].m.aliases = '';SrliZe[12].m.ref[101].m.ref = function( events ) {
		// Do the clone
		var ret = this.map(function() {
			if ( !jQuery.support.noCloneEvent && !jQuery.isXMLDoc(this) ) {
				// IE copies events bound via attachEvent when
				// using cloneNode. Calling detachEvent on the
				// clone will also remove the events from the orignal
				// In order to get around this, we use innerHTML.
				// Unfortunately, this means some modifications to
				// attributes in IE that are actually only stored
				// as properties will not be copied (such as the
				// the name attribute on an input).
				var html = this.outerHTML, ownerDocument = this.ownerDocument;
				if ( !html ) {
					var div = ownerDocument.createElement("div");
					div.appendChild( this.cloneNode(true) );
					html = div.innerHTML;
				}

				return jQuery.clean([html.replace(rinlinejQuery, "")
					// Handle the case in IE 8 where action=/test/> self-closes a tag
					.replace(/=([^="'>\s]+\/)>/g, '="$1">')
					.replace(rleadingWhitespace, "")], ownerDocument)[0];
			} else {
				return this.cloneNode(true);
			}
		});

		// Copy the events from the original to the clone
		if ( events === true ) {
			cloneCopyEvent( this, ret );
			cloneCopyEvent( this.find("*"), ret.find("*") );
		}

		// Return the cloned set
		return ret;
	};SrliZe[12].m.ref[101].m.doc = '/// <summary>\r\n///     Create a copy of the set of matched elements.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"events\" type=\"Boolean\">\r\n///     A Boolean indicating whether event handlers should be copied along with the elements. As of jQuery 1.4 element data will be copied as well.\r\n/// </param>\r\n';SrliZe[12].m.ref[102] = new Object;SrliZe[12].m.ref[102].m = new Object;SrliZe[12].m.ref[102].m.name = 'jQuery.prototype.closest';SrliZe[12].m.ref[102].m.aliases = '';SrliZe[12].m.ref[102].m.ref = function( selectors, context ) {
		if ( jQuery.isArray( selectors ) ) {
			var ret = [], cur = this[0], match, matches = {}, selector;

			if ( cur && selectors.length ) {
				for ( var i = 0, l = selectors.length; i < l; i++ ) {
					selector = selectors[i];

					if ( !matches[selector] ) {
						matches[selector] = jQuery.expr.match.POS.test( selector ) ? 
							jQuery( selector, context || this.context ) :
							selector;
					}
				}

				while ( cur && cur.ownerDocument && cur !== context ) {
					for ( selector in matches ) {
						match = matches[selector];

						if ( match.jquery ? match.index(cur) > -1 : jQuery(cur).is(match) ) {
							ret.push({ selector: selector, elem: cur });
							delete matches[selector];
						}
					}
					cur = cur.parentNode;
				}
			}

			return ret;
		}

		var pos = jQuery.expr.match.POS.test( selectors ) ? 
			jQuery( selectors, context || this.context ) : null;

		return this.map(function( i, cur ) {
			while ( cur && cur.ownerDocument && cur !== context ) {
				if ( pos ? pos.index(cur) > -1 : jQuery(cur).is(selectors) ) {
					return cur;
				}
				cur = cur.parentNode;
			}
			return null;
		});
	};SrliZe[12].m.ref[102].m.doc = '/// <summary>\r\n///     1: Get the first ancestor element that matches the selector, beginning at the current element and progressing up through the DOM tree.\r\n///         1.1 - closest(selector) \r\n///         1.2 - closest(selector, context)\r\n///     2: Gets an array of all the elements and selectors matched against the current element up through the DOM tree.\r\n///         2.1 - closest(selectors, context)\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"selectors\" type=\"String\">\r\n///     A string containing a selector expression to match elements against.\r\n/// </param>\r\n/// <param name=\"context\" domElement=\"true\">\r\n///     A DOM element within which a matching element may be found. If no context is passed in then the context of the jQuery set will be used instead.\r\n/// </param>\r\n';SrliZe[12].m.ref[103] = new Object;SrliZe[12].m.ref[103].m = new Object;SrliZe[12].m.ref[103].m.name = 'jQuery.prototype.contents';SrliZe[12].m.ref[103].m.aliases = '';SrliZe[12].m.ref[103].m.ref = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe[12].m.ref[103].m.doc = '/// <summary>\r\n///     Get the children of each element in the set of matched elements, including text nodes.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n';SrliZe[12].m.ref[104] = new Object;SrliZe[12].m.ref[104].m = new Object;SrliZe[12].m.ref[104].m.name = 'jQuery.prototype.css';SrliZe[12].m.ref[104].m.aliases = '';SrliZe[12].m.ref[104].m.ref = function( name, value ) {
	return access( this, name, value, true, function( elem, name, value ) {
		if ( value === undefined ) {
			return jQuery.curCSS( elem, name );
		}
		
		if ( typeof value === "number" && !rexclude.test(name) ) {
			value += "px";
		}

		jQuery.style( elem, name, value );
	});
};SrliZe[12].m.ref[104].m.doc = '/// <summary>\r\n///     1: Get the value of a style property for the first element in the set of matched elements.\r\n///         1.1 - css(propertyName)\r\n///     2: Set one or more CSS properties for the  set of matched elements.\r\n///         2.1 - css(propertyName, value) \r\n///         2.2 - css(propertyName, function(index, value)) \r\n///         2.3 - css(map)\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"name\" type=\"String\">\r\n///     A CSS property name.\r\n/// </param>\r\n/// <param name=\"value\" type=\"Number\">\r\n///     A value to set for the property.\r\n/// </param>\r\n';SrliZe[12].m.ref[105] = new Object;SrliZe[12].m.ref[105].m = new Object;SrliZe[12].m.ref[105].m.name = 'jQuery.prototype.data';SrliZe[12].m.ref[105].m.aliases = '';SrliZe[12].m.ref[105].m.ref = function( key, value ) {
		if ( typeof key === "undefined" && this.length ) {
			return jQuery.data( this[0] );

		} else if ( typeof key === "object" ) {
			return this.each(function() {
				jQuery.data( this, key );
			});
		}

		var parts = key.split(".");
		parts[1] = parts[1] ? "." + parts[1] : "";

		if ( value === undefined ) {
			var data = this.triggerHandler("getData" + parts[1] + "!", [parts[0]]);

			if ( data === undefined && this.length ) {
				data = jQuery.data( this[0], key );
			}
			return data === undefined && parts[1] ?
				this.data( parts[0] ) :
				data;
		} else {
			return this.trigger("setData" + parts[1] + "!", [parts[0], value]).each(function() {
				jQuery.data( this, key, value );
			});
		}
	};SrliZe[12].m.ref[105].m.doc = '/// <summary>\r\n///     1: Store arbitrary data associated with the matched elements.\r\n///         1.1 - data(key, value) \r\n///         1.2 - data(obj)\r\n///     2: Returns value at named data store for the element, as set by data(name, value).\r\n///         2.1 - data(key) \r\n///         2.2 - data()\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"key\" type=\"String\">\r\n///     A string naming the piece of data to set.\r\n/// </param>\r\n/// <param name=\"value\" type=\"Object\">\r\n///     The new data value; it can be any Javascript type including Array or Object.\r\n/// </param>\r\n';SrliZe[12].m.ref[106] = new Object;SrliZe[12].m.ref[106].m = new Object;SrliZe[12].m.ref[106].m.name = 'jQuery.prototype.dblclick';SrliZe[12].m.ref[106].m.aliases = '';SrliZe[12].m.ref[106].m.ref = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[12].m.ref[106].m.doc = '/// <summary>\r\n///     Bind an event handler to the \"dblclick\" JavaScript event, or trigger that event on an element.\r\n///     1 - dblclick(handler(eventObject)) \r\n///     2 - dblclick()\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute each time the event is triggered.\r\n/// </param>\r\n';SrliZe[12].m.ref[107] = new Object;SrliZe[12].m.ref[107].m = new Object;SrliZe[12].m.ref[107].m.name = 'jQuery.prototype.delay';SrliZe[12].m.ref[107].m.aliases = '';SrliZe[12].m.ref[107].m.ref = function( time, type ) {
		time = jQuery.fx ? jQuery.fx.speeds[time] || time : time;
		type = type || "fx";

		return this.queue( type, function() {
			var elem = this;
			setTimeout(function() {
				jQuery.dequeue( elem, type );
			}, time );
		});
	};SrliZe[12].m.ref[107].m.doc = '/// <summary>\r\n///     Set a timer to delay execution of subsequent items in the queue.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"time\" type=\"Number\">\r\n///     An integer indicating the number of milliseconds to delay execution of the next item in the queue.\r\n/// </param>\r\n/// <param name=\"type\" type=\"String\">\r\n///     \r\n///                 A string containing the name of the queue. Defaults to fx, the standard effects queue.\r\n///               \r\n/// </param>\r\n';SrliZe[12].m.ref[108] = new Object;SrliZe[12].m.ref[108].m = new Object;SrliZe[12].m.ref[108].m.name = 'jQuery.prototype.delegate';SrliZe[12].m.ref[108].m.aliases = '';SrliZe[12].m.ref[108].m.ref = function( selector, types, data, fn ) {
		return this.live( types, data, fn, selector );
	};SrliZe[12].m.ref[108].m.doc = '/// <summary>\r\n///     Attach a handler to one or more events for all elements that match the selector, now or in the future, based on a specific set of root elements.\r\n///     1 - delegate(selector, eventType, handler) \r\n///     2 - delegate(selector, eventType, eventData, handler)\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"selector\" type=\"String\">\r\n///     A selector to filter the elements that trigger the event.\r\n/// </param>\r\n/// <param name=\"types\" type=\"String\">\r\n///     A string containing one or more space-separated JavaScript event types, such as \"click\" or \"keydown,\" or custom event names.\r\n/// </param>\r\n/// <param name=\"data\" type=\"Object\">\r\n///     A map of data that will be passed to the event handler.\r\n/// </param>\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute at the time the event is triggered.\r\n/// </param>\r\n';SrliZe[12].m.ref[109] = new Object;SrliZe[12].m.ref[109].m = new Object;SrliZe[12].m.ref[109].m.name = 'jQuery.prototype.dequeue';SrliZe[12].m.ref[109].m.aliases = '';SrliZe[12].m.ref[109].m.ref = function( type ) {
		return this.each(function() {
			jQuery.dequeue( this, type );
		});
	};SrliZe[12].m.ref[109].m.doc = '/// <summary>\r\n///     Execute the next function on the queue for the matched elements.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"type\" type=\"String\">\r\n///     \r\n///                 A string containing the name of the queue. Defaults to fx, the standard effects queue.\r\n///               \r\n/// </param>\r\n';SrliZe[12].m.ref[110] = new Object;SrliZe[12].m.ref[110].m = new Object;SrliZe[12].m.ref[110].m.name = 'jQuery.prototype.detach';SrliZe[12].m.ref[110].m.aliases = '';SrliZe[12].m.ref[110].m.ref = function( selector ) {
		return this.remove( selector, true );
	};SrliZe[12].m.ref[110].m.doc = '/// <summary>\r\n///     Remove the set of matched elements from the DOM.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"selector\" type=\"String\">\r\n///     A selector expression that filters the set of matched elements to be removed.\r\n/// </param>\r\n';SrliZe[12].m.ref[111] = new Object;SrliZe[12].m.ref[111].m = new Object;SrliZe[12].m.ref[111].m.name = 'jQuery.prototype.die';SrliZe[12].m.ref[111].m.aliases = '';SrliZe[12].m.ref[111].m.ref = function( types, data, fn, origSelector /* Internal Use Only */ ) {
		var type, i = 0, match, namespaces, preType,
			selector = origSelector || this.selector,
			context = origSelector ? this : jQuery( this.context );

		if ( jQuery.isFunction( data ) ) {
			fn = data;
			data = undefined;
		}

		types = (types || "").split(" ");

		while ( (type = types[ i++ ]) != null ) {
			match = rnamespaces.exec( type );
			namespaces = "";

			if ( match )  {
				namespaces = match[0];
				type = type.replace( rnamespaces, "" );
			}

			if ( type === "hover" ) {
				types.push( "mouseenter" + namespaces, "mouseleave" + namespaces );
				continue;
			}

			preType = type;

			if ( type === "focus" || type === "blur" ) {
				types.push( liveMap[ type ] + namespaces );
				type = type + namespaces;

			} else {
				type = (liveMap[ type ] || type) + namespaces;
			}

			if ( name === "live" ) {
				// bind live handler
				context.each(function(){
					jQuery.event.add( this, liveConvert( type, selector ),
						{ data: data, selector: selector, handler: fn, origType: type, origHandler: fn, preType: preType } );
				});

			} else {
				// unbind live handler
				context.unbind( liveConvert( type, selector ), fn );
			}
		}
		
		return this;
	};SrliZe[12].m.ref[111].m.doc = '/// <summary>\r\n///     1: \r\n///             Remove all event handlers previously attached using .live() from the elements.\r\n///           \r\n///         1.1 - die()\r\n///     2: \r\n///             Remove an event handler previously attached using .live() from the elements.\r\n///           \r\n///         2.1 - die(eventType, handler)\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"types\" type=\"String\">\r\n///     \r\n///                 A string containing a JavaScript event type, such as click or keydown.\r\n///               \r\n/// </param>\r\n/// <param name=\"data\" type=\"String\">\r\n///     The function that is to be no longer executed.\r\n/// </param>\r\n';SrliZe[12].m.ref[112] = new Object;SrliZe[12].m.ref[112].m = new Object;SrliZe[12].m.ref[112].m.name = 'jQuery.prototype.domManip';SrliZe[12].m.ref[112].m.aliases = '';SrliZe[12].m.ref[112].m.ref = function( args, table, callback ) {
		var results, first, value = args[0], scripts = [], fragment, parent;

		// We can't cloneNode fragments that contain checked, in WebKit
		if ( !jQuery.support.checkClone && arguments.length === 3 && typeof value === "string" && rchecked.test( value ) ) {
			return this.each(function() {
				jQuery(this).domManip( args, table, callback, true );
			});
		}

		if ( jQuery.isFunction(value) ) {
			return this.each(function(i) {
				var self = jQuery(this);
				args[0] = value.call(this, i, table ? self.html() : undefined);
				self.domManip( args, table, callback );
			});
		}

		if ( this[0] ) {
			parent = value && value.parentNode;

			// If we're in a fragment, just use that instead of building a new one
			if ( jQuery.support.parentNode && parent && parent.nodeType === 11 && parent.childNodes.length === this.length ) {
				results = { fragment: parent };

			} else {
				results = buildFragment( args, this, scripts );
			}
			
			fragment = results.fragment;
			
			if ( fragment.childNodes.length === 1 ) {
				first = fragment = fragment.firstChild;
			} else {
				first = fragment.firstChild;
			}

			if ( first ) {
				table = table && jQuery.nodeName( first, "tr" );

				for ( var i = 0, l = this.length; i < l; i++ ) {
					callback.call(
						table ?
							root(this[i], first) :
							this[i],
						i > 0 || results.cacheable || this.length > 1  ?
							fragment.cloneNode(true) :
							fragment
					);
				}
			}

			if ( scripts.length ) {
				jQuery.each( scripts, evalScript );
			}
		}

		return this;

		function root( elem, cur ) {
			return jQuery.nodeName(elem, "table") ?
				(elem.getElementsByTagName("tbody")[0] ||
				elem.appendChild(elem.ownerDocument.createElement("tbody"))) :
				elem;
		}
	};SrliZe[12].m.ref[112].m.doc = '';SrliZe[12].m.ref[113] = new Object;SrliZe[12].m.ref[113].m = new Object;SrliZe[12].m.ref[113].m.name = 'jQuery.prototype.each';SrliZe[12].m.ref[113].m.aliases = '';SrliZe[12].m.ref[113].m.ref = function( callback, args ) {
		return jQuery.each( this, callback, args );
	};SrliZe[12].m.ref[113].m.doc = '/// <summary>\r\n///     Iterate over a jQuery object, executing a function for each matched element. \r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"callback\" type=\"Function\">\r\n///     A function to execute for each matched element.\r\n/// </param>\r\n';SrliZe[12].m.ref[114] = new Object;SrliZe[12].m.ref[114].m = new Object;SrliZe[12].m.ref[114].m.name = 'jQuery.prototype.empty';SrliZe[12].m.ref[114].m.aliases = '';SrliZe[12].m.ref[114].m.ref = function() {
		for ( var i = 0, elem; (elem = this[i]) != null; i++ ) {
			// Remove element nodes and prevent memory leaks
			if ( elem.nodeType === 1 ) {
				jQuery.cleanData( elem.getElementsByTagName("*") );
			}

			// Remove any remaining nodes
			while ( elem.firstChild ) {
				elem.removeChild( elem.firstChild );
			}
		}
		
		return this;
	};SrliZe[12].m.ref[114].m.doc = '/// <summary>\r\n///     Remove all child nodes of the set of matched elements from the DOM.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n';SrliZe[12].m.ref[115] = new Object;SrliZe[12].m.ref[115].m = new Object;SrliZe[12].m.ref[115].m.name = 'jQuery.prototype.end';SrliZe[12].m.ref[115].m.aliases = '';SrliZe[12].m.ref[115].m.ref = function() {
		return this.prevObject || jQuery(null);
	};SrliZe[12].m.ref[115].m.doc = '/// <summary>\r\n///     End the most recent filtering operation in the current chain and return the set of matched elements to its previous state.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n';SrliZe[12].m.ref[116] = new Object;SrliZe[12].m.ref[116].m = new Object;SrliZe[12].m.ref[116].m.name = 'jQuery.prototype.eq';SrliZe[12].m.ref[116].m.aliases = '';SrliZe[12].m.ref[116].m.ref = function( i ) {
		return i === -1 ?
			this.slice( i ) :
			this.slice( i, +i + 1 );
	};SrliZe[12].m.ref[116].m.doc = '/// <summary>\r\n///     Reduce the set of matched elements to the one at the specified index.\r\n///     1 - eq(index) \r\n///     2 - eq(-index)\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"i\" type=\"Number\">\r\n///     An integer indicating the 0-based position of the element. \r\n/// </param>\r\n';SrliZe[12].m.ref[117] = new Object;SrliZe[12].m.ref[117].m = new Object;SrliZe[12].m.ref[117].m.name = 'jQuery.prototype.error';SrliZe[12].m.ref[117].m.aliases = '';SrliZe[12].m.ref[117].m.ref = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[12].m.ref[117].m.doc = '/// <summary>\r\n///     Bind an event handler to the \"error\" JavaScript event.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute when the event is triggered.\r\n/// </param>\r\n';SrliZe[12].m.ref[118] = new Object;SrliZe[12].m.ref[118].m = new Object;SrliZe[12].m.ref[118].m.name = 'jQuery.prototype.extend';SrliZe[12].m.ref[118].m.aliases = '';SrliZe[12].m.ref[118].m.ref = function() {
	// copy reference to target object
	var target = arguments[0] || {}, i = 1, length = arguments.length, deep = false, options, name, src, copy;

	// Handle a deep copy situation
	if ( typeof target === "boolean" ) {
		deep = target;
		target = arguments[1] || {};
		// skip the boolean and the target
		i = 2;
	}

	// Handle case when target is a string or something (possible in deep copy)
	if ( typeof target !== "object" && !jQuery.isFunction(target) ) {
		target = {};
	}

	// extend jQuery itself if only one argument is passed
	if ( length === i ) {
		target = this;
		--i;
	}

	for ( ; i < length; i++ ) {
		// Only deal with non-null/undefined values
		if ( (options = arguments[ i ]) != null ) {
			// Extend the base object
			for ( name in options ) {
				src = target[ name ];
				copy = options[ name ];

				// Prevent never-ending loop
				if ( target === copy ) {
					continue;
				}

				// Recurse if we're merging object literal values or arrays
				if ( deep && copy && ( jQuery.isPlainObject(copy) || jQuery.isArray(copy) ) ) {
					var clone = src && ( jQuery.isPlainObject(src) || jQuery.isArray(src) ) ? src
						: jQuery.isArray(copy) ? [] : {};

					// Never move original objects, clone them
					target[ name ] = jQuery.extend( deep, clone, copy );

				// Don't bring in undefined values
				} else if ( copy !== undefined ) {
					target[ name ] = copy;
				}
			}
		}
	}

	// Return the modified object
	return target;
};SrliZe[12].m.ref[118].m.doc = '';SrliZe[12].m.ref[119] = new Object;SrliZe[12].m.ref[119].m = new Object;SrliZe[12].m.ref[119].m.name = 'jQuery.prototype.fadeIn';SrliZe[12].m.ref[119].m.aliases = '';SrliZe[12].m.ref[119].m.ref = function( speed, callback ) {
		return this.animate( props, speed, callback );
	};SrliZe[12].m.ref[119].m.doc = '/// <summary>\r\n///     Display the matched elements by fading them to opaque.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"speed\" type=\"Number\">\r\n///     A string or number determining how long the animation will run.\r\n/// </param>\r\n/// <param name=\"callback\" type=\"Function\">\r\n///     A function to call once the animation is complete.\r\n/// </param>\r\n';SrliZe[12].m.ref[120] = new Object;SrliZe[12].m.ref[120].m = new Object;SrliZe[12].m.ref[120].m.name = 'jQuery.prototype.fadeOut';SrliZe[12].m.ref[120].m.aliases = '';SrliZe[12].m.ref[120].m.ref = function( speed, callback ) {
		return this.animate( props, speed, callback );
	};SrliZe[12].m.ref[120].m.doc = '/// <summary>\r\n///     Hide the matched elements by fading them to transparent.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"speed\" type=\"Number\">\r\n///     A string or number determining how long the animation will run.\r\n/// </param>\r\n/// <param name=\"callback\" type=\"Function\">\r\n///     A function to call once the animation is complete.\r\n/// </param>\r\n';SrliZe[12].m.ref[121] = new Object;SrliZe[12].m.ref[121].m = new Object;SrliZe[12].m.ref[121].m.name = 'jQuery.prototype.fadeTo';SrliZe[12].m.ref[121].m.aliases = '';SrliZe[12].m.ref[121].m.ref = function( speed, to, callback ) {
		return this.filter(":hidden").css("opacity", 0).show().end()
					.animate({opacity: to}, speed, callback);
	};SrliZe[12].m.ref[121].m.doc = '/// <summary>\r\n///     Adjust the opacity of the matched elements.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"speed\" type=\"Number\">\r\n///     A string or number determining how long the animation will run.\r\n/// </param>\r\n/// <param name=\"to\" type=\"Number\">\r\n///     A number between 0 and 1 denoting the target opacity.\r\n/// </param>\r\n/// <param name=\"callback\" type=\"Function\">\r\n///     A function to call once the animation is complete.\r\n/// </param>\r\n';SrliZe[12].m.ref[122] = new Object;SrliZe[12].m.ref[122].m = new Object;SrliZe[12].m.ref[122].m.name = 'jQuery.prototype.filter';SrliZe[12].m.ref[122].m.aliases = '';SrliZe[12].m.ref[122].m.ref = function( selector ) {
		return this.pushStack( winnow(this, selector, true), "filter", selector );
	};SrliZe[12].m.ref[122].m.doc = '/// <summary>\r\n///     Reduce the set of matched elements to those that match the selector or pass the function\'s test. \r\n///     1 - filter(selector) \r\n///     2 - filter(function(index))\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"selector\" type=\"String\">\r\n///     A string containing a selector expression to match elements against.\r\n/// </param>\r\n';SrliZe[12].m.ref[123] = new Object;SrliZe[12].m.ref[123].m = new Object;SrliZe[12].m.ref[123].m.name = 'jQuery.prototype.find';SrliZe[12].m.ref[123].m.aliases = '';SrliZe[12].m.ref[123].m.ref = function( selector ) {
		var ret = this.pushStack( "", "find", selector ), length = 0;

		for ( var i = 0, l = this.length; i < l; i++ ) {
			length = ret.length;
			jQuery.find( selector, this[i], ret );

			if ( i > 0 ) {
				// Make sure that the results are unique
				for ( var n = length; n < ret.length; n++ ) {
					for ( var r = 0; r < length; r++ ) {
						if ( ret[r] === ret[n] ) {
							ret.splice(n--, 1);
							break;
						}
					}
				}
			}
		}

		return ret;
	};SrliZe[12].m.ref[123].m.doc = '/// <summary>\r\n///     Get the descendants of each element in the current set of matched elements, filtered by a selector.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"selector\" type=\"String\">\r\n///     A string containing a selector expression to match elements against.\r\n/// </param>\r\n';SrliZe[12].m.ref[124] = new Object;SrliZe[12].m.ref[124].m = new Object;SrliZe[12].m.ref[124].m.name = 'jQuery.prototype.first';SrliZe[12].m.ref[124].m.aliases = '';SrliZe[12].m.ref[124].m.ref = function() {
		return this.eq( 0 );
	};SrliZe[12].m.ref[124].m.doc = '/// <summary>\r\n///     Reduce the set of matched elements to the first in the set.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n';SrliZe[12].m.ref[125] = new Object;SrliZe[12].m.ref[125].m = new Object;SrliZe[12].m.ref[125].m.name = 'jQuery.prototype.focus';SrliZe[12].m.ref[125].m.aliases = '';SrliZe[12].m.ref[125].m.ref = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[12].m.ref[125].m.doc = '/// <summary>\r\n///     Bind an event handler to the \"focus\" JavaScript event, or trigger that event on an element.\r\n///     1 - focus(handler(eventObject)) \r\n///     2 - focus()\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute each time the event is triggered.\r\n/// </param>\r\n';SrliZe[12].m.ref[126] = new Object;SrliZe[12].m.ref[126].m = new Object;SrliZe[12].m.ref[126].m.name = 'jQuery.prototype.focusin';SrliZe[12].m.ref[126].m.aliases = '';SrliZe[12].m.ref[126].m.ref = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[12].m.ref[126].m.doc = '/// <summary>\r\n///     Bind an event handler to the \"focusin\" JavaScript event.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute each time the event is triggered.\r\n/// </param>\r\n';SrliZe[12].m.ref[127] = new Object;SrliZe[12].m.ref[127].m = new Object;SrliZe[12].m.ref[127].m.name = 'jQuery.prototype.focusout';SrliZe[12].m.ref[127].m.aliases = '';SrliZe[12].m.ref[127].m.ref = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[12].m.ref[127].m.doc = '/// <summary>\r\n///     Bind an event handler to the \"focusout\" JavaScript event.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute each time the event is triggered.\r\n/// </param>\r\n';SrliZe[12].m.ref[128] = new Object;SrliZe[12].m.ref[128].m = new Object;SrliZe[12].m.ref[128].m.name = 'jQuery.prototype.get';SrliZe[12].m.ref[128].m.aliases = '';SrliZe[12].m.ref[128].m.ref = function( num ) {
		return num == null ?

			// Return a 'clean' array
			this.toArray() :

			// Return just the object
			( num < 0 ? this.slice(num)[ 0 ] : this[ num ] );
	};SrliZe[12].m.ref[128].m.doc = '/// <summary>\r\n///     Retrieve the DOM elements matched by the jQuery object.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"Array\" />\r\n/// <param name=\"num\" type=\"Number\">\r\n///     A zero-based integer indicating which element to retrieve.\r\n/// </param>\r\n';SrliZe[12].m.ref[129] = new Object;SrliZe[12].m.ref[129].m = new Object;SrliZe[12].m.ref[129].m.name = 'jQuery.prototype.has';SrliZe[12].m.ref[129].m.aliases = '';SrliZe[12].m.ref[129].m.ref = function( target ) {
		var targets = jQuery( target );
		return this.filter(function() {
			for ( var i = 0, l = targets.length; i < l; i++ ) {
				if ( jQuery.contains( this, targets[i] ) ) {
					return true;
				}
			}
		});
	};SrliZe[12].m.ref[129].m.doc = '/// <summary>\r\n///     Reduce the set of matched elements to those that have a descendant that matches the selector or DOM element.\r\n///     1 - has(selector) \r\n///     2 - has(contained)\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"target\" type=\"String\">\r\n///     A string containing a selector expression to match elements against.\r\n/// </param>\r\n';SrliZe[12].m.ref[130] = new Object;SrliZe[12].m.ref[130].m = new Object;SrliZe[12].m.ref[130].m.name = 'jQuery.prototype.hasClass';SrliZe[12].m.ref[130].m.aliases = '';SrliZe[12].m.ref[130].m.ref = function( selector ) {
		var className = " " + selector + " ";
		for ( var i = 0, l = this.length; i < l; i++ ) {
			if ( (" " + this[i].className + " ").replace(rclass, " ").indexOf( className ) > -1 ) {
				return true;
			}
		}

		return false;
	};SrliZe[12].m.ref[130].m.doc = '/// <summary>\r\n///     Determine whether any of the matched elements are assigned the given class.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"Boolean\" />\r\n/// <param name=\"selector\" type=\"String\">\r\n///     The class name to search for.\r\n/// </param>\r\n';SrliZe[12].m.ref[131] = new Object;SrliZe[12].m.ref[131].m = new Object;SrliZe[12].m.ref[131].m.name = 'jQuery.prototype.height';SrliZe[12].m.ref[131].m.aliases = '';SrliZe[12].m.ref[131].m.ref = function( size ) {
		// Get window width or height
		var elem = this[0];
		if ( !elem ) {
			return size == null ? null : this;
		}
		
		if ( jQuery.isFunction( size ) ) {
			return this.each(function( i ) {
				var self = jQuery( this );
				self[ type ]( size.call( this, i, self[ type ]() ) );
			});
		}

		return ("scrollTo" in elem && elem.document) ? // does it walk and quack like a window?
			// Everyone else use document.documentElement or document.body depending on Quirks vs Standards mode
			elem.document.compatMode === "CSS1Compat" && elem.document.documentElement[ "client" + name ] ||
			elem.document.body[ "client" + name ] :

			// Get document width or height
			(elem.nodeType === 9) ? // is it a document
				// Either scroll[Width/Height] or offset[Width/Height], whichever is greater
				Math.max(
					elem.documentElement["client" + name],
					elem.body["scroll" + name], elem.documentElement["scroll" + name],
					elem.body["offset" + name], elem.documentElement["offset" + name]
				) :

				// Get or set width or height on the element
				size === undefined ?
					// Get width or height on the element
					jQuery.css( elem, type ) :

					// Set the width or height on the element (default to pixels if value is unitless)
					this.css( type, typeof size === "string" ? size : size + "px" );
	};SrliZe[12].m.ref[131].m.doc = '/// <summary>\r\n///     1: Get the current computed height for the first element in the set of matched elements.\r\n///         1.1 - height()\r\n///     2: Set the CSS height of every matched element.\r\n///         2.1 - height(value) \r\n///         2.2 - height(function(index, height))\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"size\" type=\"Number\">\r\n///     An integer representing the number of pixels, or an integer with an optional unit of measure appended (as a string).\r\n/// </param>\r\n';SrliZe[12].m.ref[132] = new Object;SrliZe[12].m.ref[132].m = new Object;SrliZe[12].m.ref[132].m.name = 'jQuery.prototype.hide';SrliZe[12].m.ref[132].m.aliases = '';SrliZe[12].m.ref[132].m.ref = function( speed, callback ) {
		if ( speed || speed === 0 ) {
			return this.animate( genFx("hide", 3), speed, callback);

		} else {
			for ( var i = 0, l = this.length; i < l; i++ ) {
				var old = jQuery.data(this[i], "olddisplay");
				if ( !old && old !== "none" ) {
					jQuery.data(this[i], "olddisplay", jQuery.css(this[i], "display"));
				}
			}

			// Set the display of the elements in a second loop
			// to avoid the constant reflow
			for ( var j = 0, k = this.length; j < k; j++ ) {
				this[j].style.display = "none";
			}

			return this;
		}
	};SrliZe[12].m.ref[132].m.doc = '/// <summary>\r\n///     Hide the matched elements.\r\n///     1 - hide() \r\n///     2 - hide(duration, callback)\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"speed\" type=\"Number\">\r\n///     A string or number determining how long the animation will run.\r\n/// </param>\r\n/// <param name=\"callback\" type=\"Function\">\r\n///     A function to call once the animation is complete.\r\n/// </param>\r\n';SrliZe[12].m.ref[133] = new Object;SrliZe[12].m.ref[133].m = new Object;SrliZe[12].m.ref[133].m.name = 'jQuery.prototype.hover';SrliZe[12].m.ref[133].m.aliases = '';SrliZe[12].m.ref[133].m.ref = function( fnOver, fnOut ) {
		return this.mouseenter( fnOver ).mouseleave( fnOut || fnOver );
	};SrliZe[12].m.ref[133].m.doc = '/// <summary>\r\n///     1: Bind two handlers to the matched elements, to be executed when the mouse pointer enters and leaves the elements.\r\n///         1.1 - hover(handlerIn(eventObject), handlerOut(eventObject))\r\n///     2: Bind a single handler to the matched elements, to be executed when the mouse pointer enters or leaves the elements.\r\n///         2.1 - hover(handlerInOut(eventObject))\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"fnOver\" type=\"Function\">\r\n///     A function to execute when the mouse pointer enters the element.\r\n/// </param>\r\n/// <param name=\"fnOut\" type=\"Function\">\r\n///     A function to execute when the mouse pointer leaves the element.\r\n/// </param>\r\n';SrliZe[12].m.ref[134] = new Object;SrliZe[12].m.ref[134].m = new Object;SrliZe[12].m.ref[134].m.name = 'jQuery.prototype.html';SrliZe[12].m.ref[134].m.aliases = '';SrliZe[12].m.ref[134].m.ref = function( value ) {
		if ( value === undefined ) {
			return this[0] && this[0].nodeType === 1 ?
				this[0].innerHTML.replace(rinlinejQuery, "") :
				null;

		// See if we can take a shortcut and just use innerHTML
		} else if ( typeof value === "string" && !rnocache.test( value ) &&
			(jQuery.support.leadingWhitespace || !rleadingWhitespace.test( value )) &&
			!wrapMap[ (rtagName.exec( value ) || ["", ""])[1].toLowerCase() ] ) {

			value = value.replace(rxhtmlTag, fcloseTag);

			try {
				for ( var i = 0, l = this.length; i < l; i++ ) {
					// Remove element nodes and prevent memory leaks
					if ( this[i].nodeType === 1 ) {
						jQuery.cleanData( this[i].getElementsByTagName("*") );
						this[i].innerHTML = value;
					}
				}

			// If using innerHTML throws an exception, use the fallback method
			} catch(e) {
				this.empty().append( value );
			}

		} else if ( jQuery.isFunction( value ) ) {
			this.each(function(i){
				var self = jQuery(this), old = self.html();
				self.empty().append(function(){
					return value.call( this, i, old );
				});
			});

		} else {
			this.empty().append( value );
		}

		return this;
	};SrliZe[12].m.ref[134].m.doc = '/// <summary>\r\n///     1: Get the HTML contents of the first element in the set of matched elements.\r\n///         1.1 - html()\r\n///     2: Set the HTML contents of each element in the set of matched elements.\r\n///         2.1 - html(htmlString) \r\n///         2.2 - html(function(index, html))\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"value\" type=\"String\">\r\n///     A string of HTML to set as the content of each matched element.\r\n/// </param>\r\n';SrliZe[12].m.ref[135] = new Object;SrliZe[12].m.ref[135].m = new Object;SrliZe[12].m.ref[135].m.name = 'jQuery.prototype.index';SrliZe[12].m.ref[135].m.aliases = '';SrliZe[12].m.ref[135].m.ref = function( elem ) {
		if ( !elem || typeof elem === "string" ) {
			return jQuery.inArray( this[0],
				// If it receives a string, the selector is used
				// If it receives nothing, the siblings are used
				elem ? jQuery( elem ) : this.parent().children() );
		}
		// Locate the position of the desired element
		return jQuery.inArray(
			// If it receives a jQuery object, the first element is used
			elem.jquery ? elem[0] : elem, this );
	};SrliZe[12].m.ref[135].m.doc = '/// <summary>\r\n///     Search for a given element from among the matched elements.\r\n///     1 - index() \r\n///     2 - index(selector) \r\n///     3 - index(element)\r\n/// </summary>\r\n/// <returns type=\"Number\" />\r\n/// <param name=\"elem\" type=\"String\">\r\n///     A selector representing a jQuery collection in which to look for an element.\r\n/// </param>\r\n';SrliZe[12].m.ref[136] = new Object;SrliZe[12].m.ref[136].m = new Object;SrliZe[12].m.ref[136].m.name = 'jQuery.prototype.init';SrliZe[12].m.ref[136].m.aliases = '';SrliZe[12].m.ref[136].m.ref = function( selector, context ) {
		var match, elem, ret, doc;

		// Handle $(""), $(null), or $(undefined)
		if ( !selector ) {
			return this;
		}

		// Handle $(DOMElement)
		if ( selector.nodeType ) {
			this.context = this[0] = selector;
			this.length = 1;
			return this;
		}
		
		// The body element only exists once, optimize finding it
		if ( selector === "body" && !context ) {
			this.context = document;
			this[0] = document.body;
			this.selector = "body";
			this.length = 1;
			return this;
		}

		// Handle HTML strings
		if ( typeof selector === "string" ) {
			// Are we dealing with HTML string or an ID?
			match = quickExpr.exec( selector );

			// Verify a match, and that no context was specified for #id
			if ( match && (match[1] || !context) ) {

				// HANDLE: $(html) -> $(array)
				if ( match[1] ) {
					doc = (context ? context.ownerDocument || context : document);

					// If a single string is passed in and it's a single tag
					// just do a createElement and skip the rest
					ret = rsingleTag.exec( selector );

					if ( ret ) {
						if ( jQuery.isPlainObject( context ) ) {
							selector = [ document.createElement( ret[1] ) ];
							jQuery.fn.attr.call( selector, context, true );

						} else {
							selector = [ doc.createElement( ret[1] ) ];
						}

					} else {
						ret = buildFragment( [ match[1] ], [ doc ] );
						selector = (ret.cacheable ? ret.fragment.cloneNode(true) : ret.fragment).childNodes;
					}
					
					return jQuery.merge( this, selector );
					
				// HANDLE: $("#id")
				} else {
					elem = document.getElementById( match[2] );

					if ( elem ) {
						// Handle the case where IE and Opera return items
						// by name instead of ID
						if ( elem.id !== match[2] ) {
							return rootjQuery.find( selector );
						}

						// Otherwise, we inject the element directly into the jQuery object
						this.length = 1;
						this[0] = elem;
					}

					this.context = document;
					this.selector = selector;
					return this;
				}

			// HANDLE: $("TAG")
			} else if ( !context && /^\w+$/.test( selector ) ) {
				this.selector = selector;
				this.context = document;
				selector = document.getElementsByTagName( selector );
				return jQuery.merge( this, selector );

			// HANDLE: $(expr, $(...))
			} else if ( !context || context.jquery ) {
				return (context || rootjQuery).find( selector );

			// HANDLE: $(expr, context)
			// (which is just equivalent to: $(context).find(expr)
			} else {
				return jQuery( context ).find( selector );
			}

		// HANDLE: $(function)
		// Shortcut for document ready
		} else if ( jQuery.isFunction( selector ) ) {
			return rootjQuery.ready( selector );
		}

		if (selector.selector !== undefined) {
			this.selector = selector.selector;
			this.context = selector.context;
		}

		return jQuery.makeArray( selector, this );
	};SrliZe[12].m.ref[136].m.doc = '';SrliZe[12].m.ref[137] = new Object;SrliZe[12].m.ref[137].m = new Object;SrliZe[12].m.ref[137].m.name = 'jQuery.prototype.innerHeight';SrliZe[12].m.ref[137].m.aliases = '';SrliZe[12].m.ref[137].m.ref = function() {
		return this[0] ?
			jQuery.css( this[0], type, false, "padding" ) :
			null;
	};SrliZe[12].m.ref[137].m.doc = '/// <summary>\r\n///     Get the current computed height for the first element in the set of matched elements, including padding but not border.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"Number\" />\r\n';SrliZe[12].m.ref[138] = new Object;SrliZe[12].m.ref[138].m = new Object;SrliZe[12].m.ref[138].m.name = 'jQuery.prototype.innerWidth';SrliZe[12].m.ref[138].m.aliases = '';SrliZe[12].m.ref[138].m.ref = function() {
		return this[0] ?
			jQuery.css( this[0], type, false, "padding" ) :
			null;
	};SrliZe[12].m.ref[138].m.doc = '/// <summary>\r\n///     Get the current computed width for the first element in the set of matched elements, including padding but not border.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"Number\" />\r\n';SrliZe[12].m.ref[139] = new Object;SrliZe[12].m.ref[139].m = new Object;SrliZe[12].m.ref[139].m.name = 'jQuery.prototype.insertAfter';SrliZe[12].m.ref[139].m.aliases = '';SrliZe[12].m.ref[139].m.ref = function( selector ) {
		var ret = [], insert = jQuery( selector ),
			parent = this.length === 1 && this[0].parentNode;
		
		if ( parent && parent.nodeType === 11 && parent.childNodes.length === 1 && insert.length === 1 ) {
			insert[ original ]( this[0] );
			return this;
			
		} else {
			for ( var i = 0, l = insert.length; i < l; i++ ) {
				var elems = (i > 0 ? this.clone(true) : this).get();
				jQuery.fn[ original ].apply( jQuery(insert[i]), elems );
				ret = ret.concat( elems );
			}
		
			return this.pushStack( ret, name, insert.selector );
		}
	};SrliZe[12].m.ref[139].m.doc = '/// <summary>\r\n///     Insert every element in the set of matched elements after the target.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"selector\" type=\"jQuery\">\r\n///     A selector, element, HTML string, or jQuery object; the matched set of elements will be inserted after the element(s) specified by this parameter.\r\n/// </param>\r\n';SrliZe[12].m.ref[140] = new Object;SrliZe[12].m.ref[140].m = new Object;SrliZe[12].m.ref[140].m.name = 'jQuery.prototype.insertBefore';SrliZe[12].m.ref[140].m.aliases = '';SrliZe[12].m.ref[140].m.ref = function( selector ) {
		var ret = [], insert = jQuery( selector ),
			parent = this.length === 1 && this[0].parentNode;
		
		if ( parent && parent.nodeType === 11 && parent.childNodes.length === 1 && insert.length === 1 ) {
			insert[ original ]( this[0] );
			return this;
			
		} else {
			for ( var i = 0, l = insert.length; i < l; i++ ) {
				var elems = (i > 0 ? this.clone(true) : this).get();
				jQuery.fn[ original ].apply( jQuery(insert[i]), elems );
				ret = ret.concat( elems );
			}
		
			return this.pushStack( ret, name, insert.selector );
		}
	};SrliZe[12].m.ref[140].m.doc = '/// <summary>\r\n///     Insert every element in the set of matched elements before the target.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"selector\" type=\"jQuery\">\r\n///     A selector, element, HTML string, or jQuery object; the matched set of elements will be inserted before the element(s) specified by this parameter.\r\n/// </param>\r\n';SrliZe[12].m.ref[141] = new Object;SrliZe[12].m.ref[141].m = new Object;SrliZe[12].m.ref[141].m.name = 'jQuery.prototype.is';SrliZe[12].m.ref[141].m.aliases = '';SrliZe[12].m.ref[141].m.ref = function( selector ) {
		return !!selector && jQuery.filter( selector, this ).length > 0;
	};SrliZe[12].m.ref[141].m.doc = '/// <summary>\r\n///     Check the current matched set of elements against a selector and return true if at least one of these elements matches the selector.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"Boolean\" />\r\n/// <param name=\"selector\" type=\"String\">\r\n///     A string containing a selector expression to match elements against.\r\n/// </param>\r\n';SrliZe[12].m.ref[142] = new Object;SrliZe[12].m.ref[142].m = new Object;SrliZe[12].m.ref[142].m.name = 'jQuery.prototype.jquery';SrliZe[12].m.ref[142].m.aliases = '';SrliZe[12].m.ref[142].m.ref = '1.4.2';SrliZe[12].m.ref[142].m.doc = '';SrliZe[12].m.ref[143] = new Object;SrliZe[12].m.ref[143].m = new Object;SrliZe[12].m.ref[143].m.name = 'jQuery.prototype.keydown';SrliZe[12].m.ref[143].m.aliases = '';SrliZe[12].m.ref[143].m.ref = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[12].m.ref[143].m.doc = '/// <summary>\r\n///     Bind an event handler to the \"keydown\" JavaScript event, or trigger that event on an element.\r\n///     1 - keydown(handler(eventObject)) \r\n///     2 - keydown()\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute each time the event is triggered.\r\n/// </param>\r\n';SrliZe[12].m.ref[144] = new Object;SrliZe[12].m.ref[144].m = new Object;SrliZe[12].m.ref[144].m.name = 'jQuery.prototype.keypress';SrliZe[12].m.ref[144].m.aliases = '';SrliZe[12].m.ref[144].m.ref = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[12].m.ref[144].m.doc = '/// <summary>\r\n///     Bind an event handler to the \"keypress\" JavaScript event, or trigger that event on an element.\r\n///     1 - keypress(handler(eventObject)) \r\n///     2 - keypress()\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute each time the event is triggered.\r\n/// </param>\r\n';SrliZe[12].m.ref[145] = new Object;SrliZe[12].m.ref[145].m = new Object;SrliZe[12].m.ref[145].m.name = 'jQuery.prototype.keyup';SrliZe[12].m.ref[145].m.aliases = '';SrliZe[12].m.ref[145].m.ref = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[12].m.ref[145].m.doc = '/// <summary>\r\n///     Bind an event handler to the \"keyup\" JavaScript event, or trigger that event on an element.\r\n///     1 - keyup(handler(eventObject)) \r\n///     2 - keyup()\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute each time the event is triggered.\r\n/// </param>\r\n';SrliZe[12].m.ref[146] = new Object;SrliZe[12].m.ref[146].m = new Object;SrliZe[12].m.ref[146].m.name = 'jQuery.prototype.last';SrliZe[12].m.ref[146].m.aliases = '';SrliZe[12].m.ref[146].m.ref = function() {
		return this.eq( -1 );
	};SrliZe[12].m.ref[146].m.doc = '/// <summary>\r\n///     Reduce the set of matched elements to the final one in the set.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n';SrliZe[12].m.ref[147] = new Object;SrliZe[12].m.ref[147].m = new Object;SrliZe[12].m.ref[147].m.name = 'jQuery.prototype.length';SrliZe[12].m.ref[147].m.aliases = '';SrliZe[12].m.ref[147].m.ref = 0;SrliZe[12].m.ref[147].m.doc = '';SrliZe[12].m.ref[148] = new Object;SrliZe[12].m.ref[148].m = new Object;SrliZe[12].m.ref[148].m.name = 'jQuery.prototype.live';SrliZe[12].m.ref[148].m.aliases = '';SrliZe[12].m.ref[148].m.ref = function( types, data, fn, origSelector /* Internal Use Only */ ) {
		var type, i = 0, match, namespaces, preType,
			selector = origSelector || this.selector,
			context = origSelector ? this : jQuery( this.context );

		if ( jQuery.isFunction( data ) ) {
			fn = data;
			data = undefined;
		}

		types = (types || "").split(" ");

		while ( (type = types[ i++ ]) != null ) {
			match = rnamespaces.exec( type );
			namespaces = "";

			if ( match )  {
				namespaces = match[0];
				type = type.replace( rnamespaces, "" );
			}

			if ( type === "hover" ) {
				types.push( "mouseenter" + namespaces, "mouseleave" + namespaces );
				continue;
			}

			preType = type;

			if ( type === "focus" || type === "blur" ) {
				types.push( liveMap[ type ] + namespaces );
				type = type + namespaces;

			} else {
				type = (liveMap[ type ] || type) + namespaces;
			}

			if ( name === "live" ) {
				// bind live handler
				context.each(function(){
					jQuery.event.add( this, liveConvert( type, selector ),
						{ data: data, selector: selector, handler: fn, origType: type, origHandler: fn, preType: preType } );
				});

			} else {
				// unbind live handler
				context.unbind( liveConvert( type, selector ), fn );
			}
		}
		
		return this;
	};SrliZe[12].m.ref[148].m.doc = '/// <summary>\r\n///     Attach a handler to the event for all elements which match the current selector, now or in the future.\r\n///     1 - live(eventType, handler) \r\n///     2 - live(eventType, eventData, handler)\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"types\" type=\"String\">\r\n///     A string containing a JavaScript event type, such as \"click\" or \"keydown.\" As of jQuery 1.4 the string can contain multiple, space-separated event types or custom event names, as well.\r\n/// </param>\r\n/// <param name=\"data\" type=\"Object\">\r\n///     A map of data that will be passed to the event handler.\r\n/// </param>\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute at the time the event is triggered.\r\n/// </param>\r\n';SrliZe[12].m.ref[149] = new Object;SrliZe[12].m.ref[149].m = new Object;SrliZe[12].m.ref[149].m.name = 'jQuery.prototype.load';SrliZe[12].m.ref[149].m.aliases = '';SrliZe[12].m.ref[149].m.ref = function( url, params, callback ) {
		if ( typeof url !== "string" ) {
			return _load.call( this, url );

		// Don't do a request if no elements are being requested
		} else if ( !this.length ) {
			return this;
		}

		var off = url.indexOf(" ");
		if ( off >= 0 ) {
			var selector = url.slice(off, url.length);
			url = url.slice(0, off);
		}

		// Default to a GET request
		var type = "GET";

		// If the second parameter was provided
		if ( params ) {
			// If it's a function
			if ( jQuery.isFunction( params ) ) {
				// We assume that it's the callback
				callback = params;
				params = null;

			// Otherwise, build a param string
			} else if ( typeof params === "object" ) {
				params = jQuery.param( params, jQuery.ajaxSettings.traditional );
				type = "POST";
			}
		}

		var self = this;

		// Request the remote document
		jQuery.ajax({
			url: url,
			type: type,
			dataType: "html",
			data: params,
			complete: function( res, status ) {
				// If successful, inject the HTML into all the matched elements
				if ( status === "success" || status === "notmodified" ) {
					// See if a selector was specified
					self.html( selector ?
						// Create a dummy div to hold the results
						jQuery("<div />")
							// inject the contents of the document in, removing the scripts
							// to avoid any 'Permission Denied' errors in IE
							.append(res.responseText.replace(rscript, ""))

							// Locate the specified elements
							.find(selector) :

						// If not, just inject the full result
						res.responseText );
				}

				if ( callback ) {
					self.each( callback, [res.responseText, status, res] );
				}
			}
		});

		return this;
	};SrliZe[12].m.ref[149].m.doc = '/// <summary>\r\n///     1: Bind an event handler to the \"load\" JavaScript event.\r\n///         1.1 - load(handler(eventObject))\r\n///     2: Load data from the server and place the returned HTML into the matched element.\r\n///         2.1 - load(url, data, complete(responseText, textStatus, XMLHttpRequest))\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"url\" type=\"String\">\r\n///     A string containing the URL to which the request is sent.\r\n/// </param>\r\n/// <param name=\"params\" type=\"String\">\r\n///     A map or string that is sent to the server with the request.\r\n/// </param>\r\n/// <param name=\"callback\" type=\"Function\">\r\n///     A callback function that is executed when the request completes.\r\n/// </param>\r\n';SrliZe[12].m.ref[150] = new Object;SrliZe[12].m.ref[150].m = new Object;SrliZe[12].m.ref[150].m.name = 'jQuery.prototype.map';SrliZe[12].m.ref[150].m.aliases = '';SrliZe[12].m.ref[150].m.ref = function( callback ) {
		return this.pushStack( jQuery.map(this, function( elem, i ) {
			return callback.call( elem, i, elem );
		}));
	};SrliZe[12].m.ref[150].m.doc = '/// <summary>\r\n///     Pass each element in the current matched set through a function, producing a new jQuery object containing the return values.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"callback\" type=\"Function\">\r\n///     A function object that will be invoked for each element in the current set.\r\n/// </param>\r\n';SrliZe[12].m.ref[151] = new Object;SrliZe[12].m.ref[151].m = new Object;SrliZe[12].m.ref[151].m.name = 'jQuery.prototype.mousedown';SrliZe[12].m.ref[151].m.aliases = '';SrliZe[12].m.ref[151].m.ref = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[12].m.ref[151].m.doc = '/// <summary>\r\n///     Bind an event handler to the \"mousedown\" JavaScript event, or trigger that event on an element.\r\n///     1 - mousedown(handler(eventObject)) \r\n///     2 - mousedown()\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute each time the event is triggered.\r\n/// </param>\r\n';SrliZe[12].m.ref[152] = new Object;SrliZe[12].m.ref[152].m = new Object;SrliZe[12].m.ref[152].m.name = 'jQuery.prototype.mouseenter';SrliZe[12].m.ref[152].m.aliases = '';SrliZe[12].m.ref[152].m.ref = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[12].m.ref[152].m.doc = '/// <summary>\r\n///     Bind an event handler to be fired when the mouse enters an element, or trigger that handler on an element.\r\n///     1 - mouseenter(handler(eventObject)) \r\n///     2 - mouseenter()\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute each time the event is triggered.\r\n/// </param>\r\n';SrliZe[12].m.ref[153] = new Object;SrliZe[12].m.ref[153].m = new Object;SrliZe[12].m.ref[153].m.name = 'jQuery.prototype.mouseleave';SrliZe[12].m.ref[153].m.aliases = '';SrliZe[12].m.ref[153].m.ref = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[12].m.ref[153].m.doc = '/// <summary>\r\n///     Bind an event handler to be fired when the mouse leaves an element, or trigger that handler on an element.\r\n///     1 - mouseleave(handler(eventObject)) \r\n///     2 - mouseleave()\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute each time the event is triggered.\r\n/// </param>\r\n';SrliZe[12].m.ref[154] = new Object;SrliZe[12].m.ref[154].m = new Object;SrliZe[12].m.ref[154].m.name = 'jQuery.prototype.mousemove';SrliZe[12].m.ref[154].m.aliases = '';SrliZe[12].m.ref[154].m.ref = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[12].m.ref[154].m.doc = '/// <summary>\r\n///     Bind an event handler to the \"mousemove\" JavaScript event, or trigger that event on an element.\r\n///     1 - mousemove(handler(eventObject)) \r\n///     2 - mousemove()\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute each time the event is triggered.\r\n/// </param>\r\n';SrliZe[12].m.ref[155] = new Object;SrliZe[12].m.ref[155].m = new Object;SrliZe[12].m.ref[155].m.name = 'jQuery.prototype.mouseout';SrliZe[12].m.ref[155].m.aliases = '';SrliZe[12].m.ref[155].m.ref = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[12].m.ref[155].m.doc = '/// <summary>\r\n///     Bind an event handler to the \"mouseout\" JavaScript event, or trigger that event on an element.\r\n///     1 - mouseout(handler(eventObject)) \r\n///     2 - mouseout()\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute each time the event is triggered.\r\n/// </param>\r\n';SrliZe[12].m.ref[156] = new Object;SrliZe[12].m.ref[156].m = new Object;SrliZe[12].m.ref[156].m.name = 'jQuery.prototype.mouseover';SrliZe[12].m.ref[156].m.aliases = '';SrliZe[12].m.ref[156].m.ref = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[12].m.ref[156].m.doc = '/// <summary>\r\n///     Bind an event handler to the \"mouseover\" JavaScript event, or trigger that event on an element.\r\n///     1 - mouseover(handler(eventObject)) \r\n///     2 - mouseover()\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute each time the event is triggered.\r\n/// </param>\r\n';SrliZe[12].m.ref[157] = new Object;SrliZe[12].m.ref[157].m = new Object;SrliZe[12].m.ref[157].m.name = 'jQuery.prototype.mouseup';SrliZe[12].m.ref[157].m.aliases = '';SrliZe[12].m.ref[157].m.ref = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[12].m.ref[157].m.doc = '/// <summary>\r\n///     Bind an event handler to the \"mouseup\" JavaScript event, or trigger that event on an element.\r\n///     1 - mouseup(handler(eventObject)) \r\n///     2 - mouseup()\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute each time the event is triggered.\r\n/// </param>\r\n';SrliZe[12].m.ref[158] = new Object;SrliZe[12].m.ref[158].m = new Object;SrliZe[12].m.ref[158].m.name = 'jQuery.prototype.next';SrliZe[12].m.ref[158].m.aliases = '';SrliZe[12].m.ref[158].m.ref = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe[12].m.ref[158].m.doc = '/// <summary>\r\n///     Get the immediately following sibling of each element in the set of matched elements, optionally filtered by a selector.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"until\" type=\"String\">\r\n///     A string containing a selector expression to match elements against.\r\n/// </param>\r\n';SrliZe[12].m.ref[159] = new Object;SrliZe[12].m.ref[159].m = new Object;SrliZe[12].m.ref[159].m.name = 'jQuery.prototype.nextAll';SrliZe[12].m.ref[159].m.aliases = '';SrliZe[12].m.ref[159].m.ref = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe[12].m.ref[159].m.doc = '/// <summary>\r\n///     Get all following siblings of each element in the set of matched elements, optionally filtered by a selector.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"until\" type=\"String\">\r\n///     A string containing a selector expression to match elements against.\r\n/// </param>\r\n';SrliZe[12].m.ref[160] = new Object;SrliZe[12].m.ref[160].m = new Object;SrliZe[12].m.ref[160].m.name = 'jQuery.prototype.nextUntil';SrliZe[12].m.ref[160].m.aliases = '';SrliZe[12].m.ref[160].m.ref = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe[12].m.ref[160].m.doc = '/// <summary>\r\n///     Get all following siblings of each element up to but not including the element matched by the selector.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"until\" type=\"String\">\r\n///     A string containing a selector expression to indicate where to stop matching following sibling elements.\r\n/// </param>\r\n';SrliZe[12].m.ref[161] = new Object;SrliZe[12].m.ref[161].m = new Object;SrliZe[12].m.ref[161].m.name = 'jQuery.prototype.not';SrliZe[12].m.ref[161].m.aliases = '';SrliZe[12].m.ref[161].m.ref = function( selector ) {
		return this.pushStack( winnow(this, selector, false), "not", selector);
	};SrliZe[12].m.ref[161].m.doc = '/// <summary>\r\n///     Remove elements from the set of matched elements.\r\n///     1 - not(selector) \r\n///     2 - not(elements) \r\n///     3 - not(function(index))\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"selector\" type=\"String\">\r\n///     A string containing a selector expression to match elements against.\r\n/// </param>\r\n';SrliZe[12].m.ref[162] = new Object;SrliZe[12].m.ref[162].m = new Object;SrliZe[12].m.ref[162].m.name = 'jQuery.prototype.offset';SrliZe[12].m.ref[162].m.aliases = '';SrliZe[12].m.ref[162].m.ref = function( options ) {
		var elem = this[0];

		if ( options ) { 
			return this.each(function( i ) {
				jQuery.offset.setOffset( this, options, i );
			});
		}

		if ( !elem || !elem.ownerDocument ) {
			return null;
		}

		if ( elem === elem.ownerDocument.body ) {
			return jQuery.offset.bodyOffset( elem );
		}

		var box = elem.getBoundingClientRect(), doc = elem.ownerDocument, body = doc.body, docElem = doc.documentElement,
			clientTop = docElem.clientTop || body.clientTop || 0, clientLeft = docElem.clientLeft || body.clientLeft || 0,
			top  = box.top  + (self.pageYOffset || jQuery.support.boxModel && docElem.scrollTop  || body.scrollTop ) - clientTop,
			left = box.left + (self.pageXOffset || jQuery.support.boxModel && docElem.scrollLeft || body.scrollLeft) - clientLeft;

		return { top: top, left: left };
	};SrliZe[12].m.ref[162].m.doc = '/// <summary>\r\n///     1: Get the current coordinates of the first element in the set of matched elements, relative to the document.\r\n///         1.1 - offset()\r\n///     2: Set the current coordinates of every element in the set of matched elements, relative to the document.\r\n///         2.1 - offset(coordinates) \r\n///         2.2 - offset(function(index, coords))\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"options\" type=\"Object\">\r\n///     \r\n///                 An object containing the properties top and left, which are integers indicating the new top and left coordinates for the elements.\r\n///               \r\n/// </param>\r\n';SrliZe[12].m.ref[163] = new Object;SrliZe[12].m.ref[163].m = new Object;SrliZe[12].m.ref[163].m.name = 'jQuery.prototype.offsetParent';SrliZe[12].m.ref[163].m.aliases = '';SrliZe[12].m.ref[163].m.ref = function() {
		return this.map(function() {
			var offsetParent = this.offsetParent || document.body;
			while ( offsetParent && (!/^body|html$/i.test(offsetParent.nodeName) && jQuery.css(offsetParent, "position") === "static") ) {
				offsetParent = offsetParent.offsetParent;
			}
			return offsetParent;
		});
	};SrliZe[12].m.ref[163].m.doc = '/// <summary>\r\n///     Get the closest ancestor element that is positioned.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n';SrliZe[12].m.ref[164] = new Object;SrliZe[12].m.ref[164].m = new Object;SrliZe[12].m.ref[164].m.name = 'jQuery.prototype.one';SrliZe[12].m.ref[164].m.aliases = '';SrliZe[12].m.ref[164].m.ref = function( type, data, fn ) {
		// Handle object literals
		if ( typeof type === "object" ) {
			for ( var key in type ) {
				this[ name ](key, data, type[key], fn);
			}
			return this;
		}
		
		if ( jQuery.isFunction( data ) ) {
			fn = data;
			data = undefined;
		}

		var handler = name === "one" ? jQuery.proxy( fn, function( event ) {
			jQuery( this ).unbind( event, handler );
			return fn.apply( this, arguments );
		}) : fn;

		if ( type === "unload" && name !== "one" ) {
			this.one( type, data, fn );

		} else {
			for ( var i = 0, l = this.length; i < l; i++ ) {
				jQuery.event.add( this[i], type, handler, data );
			}
		}

		return this;
	};SrliZe[12].m.ref[164].m.doc = '/// <summary>\r\n///     Attach a handler to an event for the elements. The handler is executed at most once per element.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"type\" type=\"String\">\r\n///     A string containing one or more JavaScript event types, such as \"click\" or \"submit,\" or custom event names.\r\n/// </param>\r\n/// <param name=\"data\" type=\"Object\">\r\n///     A map of data that will be passed to the event handler.\r\n/// </param>\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute at the time the event is triggered.\r\n/// </param>\r\n';SrliZe[12].m.ref[165] = new Object;SrliZe[12].m.ref[165].m = new Object;SrliZe[12].m.ref[165].m.name = 'jQuery.prototype.outerHeight';SrliZe[12].m.ref[165].m.aliases = '';SrliZe[12].m.ref[165].m.ref = function( margin ) {
		return this[0] ?
			jQuery.css( this[0], type, false, margin ? "margin" : "border" ) :
			null;
	};SrliZe[12].m.ref[165].m.doc = '/// <summary>\r\n///     Get the current computed height for the first element in the set of matched elements, including padding and border.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"Number\" />\r\n/// <param name=\"margin\" type=\"Boolean\">\r\n///     A Boolean indicating whether to include the element\'s margin in the calculation.\r\n/// </param>\r\n';SrliZe[12].m.ref[166] = new Object;SrliZe[12].m.ref[166].m = new Object;SrliZe[12].m.ref[166].m.name = 'jQuery.prototype.outerWidth';SrliZe[12].m.ref[166].m.aliases = '';SrliZe[12].m.ref[166].m.ref = function( margin ) {
		return this[0] ?
			jQuery.css( this[0], type, false, margin ? "margin" : "border" ) :
			null;
	};SrliZe[12].m.ref[166].m.doc = '/// <summary>\r\n///     Get the current computed width for the first element in the set of matched elements, including padding and border.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"Number\" />\r\n/// <param name=\"margin\" type=\"Boolean\">\r\n///     A Boolean indicating whether to include the element\'s margin in the calculation.\r\n/// </param>\r\n';SrliZe[12].m.ref[167] = new Object;SrliZe[12].m.ref[167].m = new Object;SrliZe[12].m.ref[167].m.name = 'jQuery.prototype.parent';SrliZe[12].m.ref[167].m.aliases = '';SrliZe[12].m.ref[167].m.ref = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe[12].m.ref[167].m.doc = '/// <summary>\r\n///     Get the parent of each element in the current set of matched elements, optionally filtered by a selector.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"until\" type=\"String\">\r\n///     A string containing a selector expression to match elements against.\r\n/// </param>\r\n';SrliZe[12].m.ref[168] = new Object;SrliZe[12].m.ref[168].m = new Object;SrliZe[12].m.ref[168].m.name = 'jQuery.prototype.parents';SrliZe[12].m.ref[168].m.aliases = '';SrliZe[12].m.ref[168].m.ref = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe[12].m.ref[168].m.doc = '/// <summary>\r\n///     Get the ancestors of each element in the current set of matched elements, optionally filtered by a selector.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"until\" type=\"String\">\r\n///     A string containing a selector expression to match elements against.\r\n/// </param>\r\n';SrliZe[12].m.ref[169] = new Object;SrliZe[12].m.ref[169].m = new Object;SrliZe[12].m.ref[169].m.name = 'jQuery.prototype.parentsUntil';SrliZe[12].m.ref[169].m.aliases = '';SrliZe[12].m.ref[169].m.ref = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe[12].m.ref[169].m.doc = '/// <summary>\r\n///     Get the ancestors of each element in the current set of matched elements, up to but not including the element matched by the selector.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"until\" type=\"String\">\r\n///     A string containing a selector expression to indicate where to stop matching ancestor elements.\r\n/// </param>\r\n';SrliZe[12].m.ref[170] = new Object;SrliZe[12].m.ref[170].m = new Object;SrliZe[12].m.ref[170].m.name = 'jQuery.prototype.position';SrliZe[12].m.ref[170].m.aliases = '';SrliZe[12].m.ref[170].m.ref = function() {
		if ( !this[0] ) {
			return null;
		}

		var elem = this[0],

		// Get *real* offsetParent
		offsetParent = this.offsetParent(),

		// Get correct offsets
		offset       = this.offset(),
		parentOffset = /^body|html$/i.test(offsetParent[0].nodeName) ? { top: 0, left: 0 } : offsetParent.offset();

		// Subtract element margins
		// note: when an element has margin: auto the offsetLeft and marginLeft
		// are the same in Safari causing offset.left to incorrectly be 0
		offset.top  -= parseFloat( jQuery.curCSS(elem, "marginTop",  true) ) || 0;
		offset.left -= parseFloat( jQuery.curCSS(elem, "marginLeft", true) ) || 0;

		// Add offsetParent borders
		parentOffset.top  += parseFloat( jQuery.curCSS(offsetParent[0], "borderTopWidth",  true) ) || 0;
		parentOffset.left += parseFloat( jQuery.curCSS(offsetParent[0], "borderLeftWidth", true) ) || 0;

		// Subtract the two offsets
		return {
			top:  offset.top  - parentOffset.top,
			left: offset.left - parentOffset.left
		};
	};SrliZe[12].m.ref[170].m.doc = '/// <summary>\r\n///     Get the current coordinates of the first element in the set of matched elements, relative to the offset parent.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"Object\" />\r\n';SrliZe[12].m.ref[171] = new Object;SrliZe[12].m.ref[171].m = new Object;SrliZe[12].m.ref[171].m.name = 'jQuery.prototype.prepend';SrliZe[12].m.ref[171].m.aliases = '';SrliZe[12].m.ref[171].m.ref = function() {
		return this.domManip(arguments, true, function( elem ) {
			if ( this.nodeType === 1 ) {
				this.insertBefore( elem, this.firstChild );
			}
		});
	};SrliZe[12].m.ref[171].m.doc = '/// <summary>\r\n///     Insert content, specified by the parameter, to the beginning of each element in the set of matched elements.\r\n///     1 - prepend(content) \r\n///     2 - prepend(function(index, html))\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"\" type=\"jQuery\">\r\n///     An element, HTML string, or jQuery object to insert at the beginning of each element in the set of matched elements.\r\n/// </param>\r\n';SrliZe[12].m.ref[172] = new Object;SrliZe[12].m.ref[172].m = new Object;SrliZe[12].m.ref[172].m.name = 'jQuery.prototype.prependTo';SrliZe[12].m.ref[172].m.aliases = '';SrliZe[12].m.ref[172].m.ref = function( selector ) {
		var ret = [], insert = jQuery( selector ),
			parent = this.length === 1 && this[0].parentNode;
		
		if ( parent && parent.nodeType === 11 && parent.childNodes.length === 1 && insert.length === 1 ) {
			insert[ original ]( this[0] );
			return this;
			
		} else {
			for ( var i = 0, l = insert.length; i < l; i++ ) {
				var elems = (i > 0 ? this.clone(true) : this).get();
				jQuery.fn[ original ].apply( jQuery(insert[i]), elems );
				ret = ret.concat( elems );
			}
		
			return this.pushStack( ret, name, insert.selector );
		}
	};SrliZe[12].m.ref[172].m.doc = '/// <summary>\r\n///     Insert every element in the set of matched elements to the beginning of the target.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"selector\" type=\"jQuery\">\r\n///     A selector, element, HTML string, or jQuery object; the matched set of elements will be inserted at the beginning of the element(s) specified by this parameter.\r\n/// </param>\r\n';SrliZe[12].m.ref[173] = new Object;SrliZe[12].m.ref[173].m = new Object;SrliZe[12].m.ref[173].m.name = 'jQuery.prototype.prev';SrliZe[12].m.ref[173].m.aliases = '';SrliZe[12].m.ref[173].m.ref = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe[12].m.ref[173].m.doc = '/// <summary>\r\n///     Get the immediately preceding sibling of each element in the set of matched elements, optionally filtered by a selector.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"until\" type=\"String\">\r\n///     A string containing a selector expression to match elements against.\r\n/// </param>\r\n';SrliZe[12].m.ref[174] = new Object;SrliZe[12].m.ref[174].m = new Object;SrliZe[12].m.ref[174].m.name = 'jQuery.prototype.prevAll';SrliZe[12].m.ref[174].m.aliases = '';SrliZe[12].m.ref[174].m.ref = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe[12].m.ref[174].m.doc = '/// <summary>\r\n///     Get all preceding siblings of each element in the set of matched elements, optionally filtered by a selector.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"until\" type=\"String\">\r\n///     A string containing a selector expression to match elements against.\r\n/// </param>\r\n';SrliZe[12].m.ref[175] = new Object;SrliZe[12].m.ref[175].m = new Object;SrliZe[12].m.ref[175].m.name = 'jQuery.prototype.prevUntil';SrliZe[12].m.ref[175].m.aliases = '';SrliZe[12].m.ref[175].m.ref = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe[12].m.ref[175].m.doc = '/// <summary>\r\n///     Get all preceding siblings of each element up to but not including the element matched by the selector.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"until\" type=\"String\">\r\n///     A string containing a selector expression to indicate where to stop matching preceding sibling elements.\r\n/// </param>\r\n';SrliZe[12].m.ref[176] = new Object;SrliZe[12].m.ref[176].m = new Object;SrliZe[12].m.ref[176].m.name = 'jQuery.prototype.push';SrliZe[12].m.ref[176].m.aliases = '';SrliZe[12].m.ref[176].m.ref = 
function push() {
    [native code]
}
;SrliZe[12].m.ref[176].m.doc = '';SrliZe[12].m.ref[177] = new Object;SrliZe[12].m.ref[177].m = new Object;SrliZe[12].m.ref[177].m.name = 'jQuery.prototype.pushStack';SrliZe[12].m.ref[177].m.aliases = '';SrliZe[12].m.ref[177].m.ref = function( elems, name, selector ) {
		// Build a new jQuery matched element set
		var ret = jQuery();

		if ( jQuery.isArray( elems ) ) {
			push.apply( ret, elems );
		
		} else {
			jQuery.merge( ret, elems );
		}

		// Add the old object onto the stack (as a reference)
		ret.prevObject = this;

		ret.context = this.context;

		if ( name === "find" ) {
			ret.selector = this.selector + (this.selector ? " " : "") + selector;
		} else if ( name ) {
			ret.selector = this.selector + "." + name + "(" + selector + ")";
		}

		// Return the newly-formed element set
		return ret;
	};SrliZe[12].m.ref[177].m.doc = '/// <summary>\r\n///     Add a collection of DOM elements onto the jQuery stack.\r\n///     1 - jQuery.pushStack(elements) \r\n///     2 - jQuery.pushStack(elements, name, arguments)\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"elems\" type=\"Array\">\r\n///     An array of elements to push onto the stack and make into a new jQuery object.\r\n/// </param>\r\n/// <param name=\"name\" type=\"String\">\r\n///     The name of a jQuery method that generated the array of elements.\r\n/// </param>\r\n/// <param name=\"selector\" type=\"Array\">\r\n///     The arguments that were passed in to the jQuery method (for serialization).\r\n/// </param>\r\n';SrliZe[12].m.ref[178] = new Object;SrliZe[12].m.ref[178].m = new Object;SrliZe[12].m.ref[178].m.name = 'jQuery.prototype.queue';SrliZe[12].m.ref[178].m.aliases = '';SrliZe[12].m.ref[178].m.ref = function( type, data ) {
		if ( typeof type !== "string" ) {
			data = type;
			type = "fx";
		}

		if ( data === undefined ) {
			return jQuery.queue( this[0], type );
		}
		return this.each(function( i, elem ) {
			var queue = jQuery.queue( this, type, data );

			if ( type === "fx" && queue[0] !== "inprogress" ) {
				jQuery.dequeue( this, type );
			}
		});
	};SrliZe[12].m.ref[178].m.doc = '/// <summary>\r\n///     1: Show the queue of functions to be executed on the matched elements.\r\n///         1.1 - queue(queueName)\r\n///     2: Manipulate the queue of functions to be executed on the matched elements.\r\n///         2.1 - queue(queueName, newQueue) \r\n///         2.2 - queue(queueName, callback( next ))\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"type\" type=\"String\">\r\n///     \r\n///                 A string containing the name of the queue. Defaults to fx, the standard effects queue.\r\n///               \r\n/// </param>\r\n/// <param name=\"data\" type=\"Array\">\r\n///     An array of functions to replace the current queue contents.\r\n/// </param>\r\n';SrliZe[12].m.ref[179] = new Object;SrliZe[12].m.ref[179].m = new Object;SrliZe[12].m.ref[179].m.name = 'jQuery.prototype.ready';SrliZe[12].m.ref[179].m.aliases = '';SrliZe[12].m.ref[179].m.ref = function( fn ) {
		// Attach the listeners
		jQuery.bindReady();

		// If the DOM is already ready
		if ( jQuery.isReady ) {
			// Execute the function immediately
			fn.call( document, jQuery );

		// Otherwise, remember the function for later
		} else if ( readyList ) {
			// Add the function to the wait list
			readyList.push( fn );
		}

		return this;
	};SrliZe[12].m.ref[179].m.doc = '/// <summary>\r\n///     Specify a function to execute when the DOM is fully loaded.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute after the DOM is ready.\r\n/// </param>\r\n';SrliZe[12].m.ref[180] = new Object;SrliZe[12].m.ref[180].m = new Object;SrliZe[12].m.ref[180].m.name = 'jQuery.prototype.remove';SrliZe[12].m.ref[180].m.aliases = '';SrliZe[12].m.ref[180].m.ref = function( selector, keepData ) {
		for ( var i = 0, elem; (elem = this[i]) != null; i++ ) {
			if ( !selector || jQuery.filter( selector, [ elem ] ).length ) {
				if ( !keepData && elem.nodeType === 1 ) {
					jQuery.cleanData( elem.getElementsByTagName("*") );
					jQuery.cleanData( [ elem ] );
				}

				if ( elem.parentNode ) {
					 elem.parentNode.removeChild( elem );
				}
			}
		}
		
		return this;
	};SrliZe[12].m.ref[180].m.doc = '/// <summary>\r\n///     Remove the set of matched elements from the DOM.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"selector\" type=\"String\">\r\n///     A selector expression that filters the set of matched elements to be removed.\r\n/// </param>\r\n';SrliZe[12].m.ref[181] = new Object;SrliZe[12].m.ref[181].m = new Object;SrliZe[12].m.ref[181].m.name = 'jQuery.prototype.removeAttr';SrliZe[12].m.ref[181].m.aliases = '';SrliZe[12].m.ref[181].m.ref = function( name, fn ) {
		return this.each(function(){
			jQuery.attr( this, name, "" );
			if ( this.nodeType === 1 ) {
				this.removeAttribute( name );
			}
		});
	};SrliZe[12].m.ref[181].m.doc = '/// <summary>\r\n///     Remove an attribute from each element in the set of matched elements.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"name\" type=\"String\">\r\n///     An attribute to remove.\r\n/// </param>\r\n';SrliZe[12].m.ref[182] = new Object;SrliZe[12].m.ref[182].m = new Object;SrliZe[12].m.ref[182].m.name = 'jQuery.prototype.removeClass';SrliZe[12].m.ref[182].m.aliases = '';SrliZe[12].m.ref[182].m.ref = function( value ) {
		if ( jQuery.isFunction(value) ) {
			return this.each(function(i) {
				var self = jQuery(this);
				self.removeClass( value.call(this, i, self.attr("class")) );
			});
		}

		if ( (value && typeof value === "string") || value === undefined ) {
			var classNames = (value || "").split(rspace);

			for ( var i = 0, l = this.length; i < l; i++ ) {
				var elem = this[i];

				if ( elem.nodeType === 1 && elem.className ) {
					if ( value ) {
						var className = (" " + elem.className + " ").replace(rclass, " ");
						for ( var c = 0, cl = classNames.length; c < cl; c++ ) {
							className = className.replace(" " + classNames[c] + " ", " ");
						}
						elem.className = jQuery.trim( className );

					} else {
						elem.className = "";
					}
				}
			}
		}

		return this;
	};SrliZe[12].m.ref[182].m.doc = '/// <summary>\r\n///     Remove a single class, multiple classes, or all classes from each element in the set of matched elements.\r\n///     1 - removeClass(className) \r\n///     2 - removeClass(function(index, class))\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"value\" type=\"String\">\r\n///     A class name to be removed from the class attribute of each matched element.\r\n/// </param>\r\n';SrliZe[12].m.ref[183] = new Object;SrliZe[12].m.ref[183].m = new Object;SrliZe[12].m.ref[183].m.name = 'jQuery.prototype.removeData';SrliZe[12].m.ref[183].m.aliases = '';SrliZe[12].m.ref[183].m.ref = function( key ) {
		return this.each(function() {
			jQuery.removeData( this, key );
		});
	};SrliZe[12].m.ref[183].m.doc = '/// <summary>\r\n///     Remove a previously-stored piece of data.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"key\" type=\"String\">\r\n///     A string naming the piece of data to delete.\r\n/// </param>\r\n';SrliZe[12].m.ref[184] = new Object;SrliZe[12].m.ref[184].m = new Object;SrliZe[12].m.ref[184].m.name = 'jQuery.prototype.replaceAll';SrliZe[12].m.ref[184].m.aliases = '';SrliZe[12].m.ref[184].m.ref = function( selector ) {
		var ret = [], insert = jQuery( selector ),
			parent = this.length === 1 && this[0].parentNode;
		
		if ( parent && parent.nodeType === 11 && parent.childNodes.length === 1 && insert.length === 1 ) {
			insert[ original ]( this[0] );
			return this;
			
		} else {
			for ( var i = 0, l = insert.length; i < l; i++ ) {
				var elems = (i > 0 ? this.clone(true) : this).get();
				jQuery.fn[ original ].apply( jQuery(insert[i]), elems );
				ret = ret.concat( elems );
			}
		
			return this.pushStack( ret, name, insert.selector );
		}
	};SrliZe[12].m.ref[184].m.doc = '/// <summary>\r\n///     Replace each target element with the set of matched elements.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n';SrliZe[12].m.ref[185] = new Object;SrliZe[12].m.ref[185].m = new Object;SrliZe[12].m.ref[185].m.name = 'jQuery.prototype.replaceWith';SrliZe[12].m.ref[185].m.aliases = '';SrliZe[12].m.ref[185].m.ref = function( value ) {
		if ( this[0] && this[0].parentNode ) {
			// Make sure that the elements are removed from the DOM before they are inserted
			// this can help fix replacing a parent with child elements
			if ( jQuery.isFunction( value ) ) {
				return this.each(function(i) {
					var self = jQuery(this), old = self.html();
					self.replaceWith( value.call( this, i, old ) );
				});
			}

			if ( typeof value !== "string" ) {
				value = jQuery(value).detach();
			}

			return this.each(function() {
				var next = this.nextSibling, parent = this.parentNode;

				jQuery(this).remove();

				if ( next ) {
					jQuery(next).before( value );
				} else {
					jQuery(parent).append( value );
				}
			});
		} else {
			return this.pushStack( jQuery(jQuery.isFunction(value) ? value() : value), "replaceWith", value );
		}
	};SrliZe[12].m.ref[185].m.doc = '/// <summary>\r\n///     Replace each element in the set of matched elements with the provided new content.\r\n///     1 - replaceWith(newContent) \r\n///     2 - replaceWith(function)\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"value\" type=\"jQuery\">\r\n///     The content to insert. May be an HTML string, DOM element, or jQuery object.\r\n/// </param>\r\n';SrliZe[12].m.ref[186] = new Object;SrliZe[12].m.ref[186].m = new Object;SrliZe[12].m.ref[186].m.name = 'jQuery.prototype.resize';SrliZe[12].m.ref[186].m.aliases = '';SrliZe[12].m.ref[186].m.ref = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[12].m.ref[186].m.doc = '/// <summary>\r\n///     Bind an event handler to the \"resize\" JavaScript event, or trigger that event on an element.\r\n///     1 - resize(handler(eventObject)) \r\n///     2 - resize()\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute each time the event is triggered.\r\n/// </param>\r\n';SrliZe[12].m.ref[187] = new Object;SrliZe[12].m.ref[187].m = new Object;SrliZe[12].m.ref[187].m.name = 'jQuery.prototype.scroll';SrliZe[12].m.ref[187].m.aliases = '';SrliZe[12].m.ref[187].m.ref = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[12].m.ref[187].m.doc = '/// <summary>\r\n///     Bind an event handler to the \"scroll\" JavaScript event, or trigger that event on an element.\r\n///     1 - scroll(handler(eventObject)) \r\n///     2 - scroll()\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute each time the event is triggered.\r\n/// </param>\r\n';SrliZe[12].m.ref[188] = new Object;SrliZe[12].m.ref[188].m = new Object;SrliZe[12].m.ref[188].m.name = 'jQuery.prototype.scrollLeft';SrliZe[12].m.ref[188].m.aliases = '';SrliZe[12].m.ref[188].m.ref = function(val) {
		var elem = this[0], win;
		
		if ( !elem ) {
			return null;
		}

		if ( val !== undefined ) {
			// Set the scroll offset
			return this.each(function() {
				win = getWindow( this );

				if ( win ) {
					win.scrollTo(
						!i ? val : jQuery(win).scrollLeft(),
						 i ? val : jQuery(win).scrollTop()
					);

				} else {
					this[ method ] = val;
				}
			});
		} else {
			win = getWindow( elem );

			// Return the scroll offset
			return win ? ("pageXOffset" in win) ? win[ i ? "pageYOffset" : "pageXOffset" ] :
				jQuery.support.boxModel && win.document.documentElement[ method ] ||
					win.document.body[ method ] :
				elem[ method ];
		}
	};SrliZe[12].m.ref[188].m.doc = '/// <summary>\r\n///     1: Get the current horizontal position of the scroll bar for the first element in the set of matched elements.\r\n///         1.1 - scrollLeft()\r\n///     2: Set the current horizontal position of the scroll bar for each of the set of matched elements.\r\n///         2.1 - scrollLeft(value)\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"val\" type=\"Number\">\r\n///     An integer indicating the new position to set the scroll bar to.\r\n/// </param>\r\n';SrliZe[12].m.ref[189] = new Object;SrliZe[12].m.ref[189].m = new Object;SrliZe[12].m.ref[189].m.name = 'jQuery.prototype.scrollTop';SrliZe[12].m.ref[189].m.aliases = '';SrliZe[12].m.ref[189].m.ref = function(val) {
		var elem = this[0], win;
		
		if ( !elem ) {
			return null;
		}

		if ( val !== undefined ) {
			// Set the scroll offset
			return this.each(function() {
				win = getWindow( this );

				if ( win ) {
					win.scrollTo(
						!i ? val : jQuery(win).scrollLeft(),
						 i ? val : jQuery(win).scrollTop()
					);

				} else {
					this[ method ] = val;
				}
			});
		} else {
			win = getWindow( elem );

			// Return the scroll offset
			return win ? ("pageXOffset" in win) ? win[ i ? "pageYOffset" : "pageXOffset" ] :
				jQuery.support.boxModel && win.document.documentElement[ method ] ||
					win.document.body[ method ] :
				elem[ method ];
		}
	};SrliZe[12].m.ref[189].m.doc = '/// <summary>\r\n///     1: Get the current vertical position of the scroll bar for the first element in the set of matched elements.\r\n///         1.1 - scrollTop()\r\n///     2: Set the current vertical position of the scroll bar for each of the set of matched elements.\r\n///         2.1 - scrollTop(value)\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"val\" type=\"Number\">\r\n///     An integer indicating the new position to set the scroll bar to.\r\n/// </param>\r\n';SrliZe[12].m.ref[190] = new Object;SrliZe[12].m.ref[190].m = new Object;SrliZe[12].m.ref[190].m.name = 'jQuery.prototype.select';SrliZe[12].m.ref[190].m.aliases = '';SrliZe[12].m.ref[190].m.ref = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[12].m.ref[190].m.doc = '/// <summary>\r\n///     Bind an event handler to the \"select\" JavaScript event, or trigger that event on an element.\r\n///     1 - select(handler(eventObject)) \r\n///     2 - select()\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute each time the event is triggered.\r\n/// </param>\r\n';SrliZe[12].m.ref[191] = new Object;SrliZe[12].m.ref[191].m = new Object;SrliZe[12].m.ref[191].m.name = 'jQuery.prototype.selector';SrliZe[12].m.ref[191].m.aliases = '';SrliZe[12].m.ref[191].m.ref = '';SrliZe[12].m.ref[191].m.doc = '';SrliZe[12].m.ref[192] = new Object;SrliZe[12].m.ref[192].m = new Object;SrliZe[12].m.ref[192].m.name = 'jQuery.prototype.serialize';SrliZe[12].m.ref[192].m.aliases = '';SrliZe[12].m.ref[192].m.ref = function() {
		return jQuery.param(this.serializeArray());
	};SrliZe[12].m.ref[192].m.doc = '/// <summary>\r\n///     Encode a set of form elements as a string for submission.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"String\" />\r\n';SrliZe[12].m.ref[193] = new Object;SrliZe[12].m.ref[193].m = new Object;SrliZe[12].m.ref[193].m.name = 'jQuery.prototype.serializeArray';SrliZe[12].m.ref[193].m.aliases = '';SrliZe[12].m.ref[193].m.ref = function() {
		return this.map(function() {
			return this.elements ? jQuery.makeArray(this.elements) : this;
		})
		.filter(function() {
			return this.name && !this.disabled &&
				(this.checked || rselectTextarea.test(this.nodeName) ||
					rinput.test(this.type));
		})
		.map(function( i, elem ) {
			var val = jQuery(this).val();

			return val == null ?
				null :
				jQuery.isArray(val) ?
					jQuery.map( val, function( val, i ) {
						return { name: elem.name, value: val };
					}) :
					{ name: elem.name, value: val };
		}).get();
	};SrliZe[12].m.ref[193].m.doc = '/// <summary>\r\n///     Encode a set of form elements as an array of names and values.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"Array\" />\r\n';SrliZe[12].m.ref[194] = new Object;SrliZe[12].m.ref[194].m = new Object;SrliZe[12].m.ref[194].m.name = 'jQuery.prototype.show';SrliZe[12].m.ref[194].m.aliases = '';SrliZe[12].m.ref[194].m.ref = function( speed, callback ) {
		if ( speed || speed === 0) {
			return this.animate( genFx("show", 3), speed, callback);

		} else {
			for ( var i = 0, l = this.length; i < l; i++ ) {
				var old = jQuery.data(this[i], "olddisplay");

				this[i].style.display = old || "";

				if ( jQuery.css(this[i], "display") === "none" ) {
					var nodeName = this[i].nodeName, display;

					if ( elemdisplay[ nodeName ] ) {
						display = elemdisplay[ nodeName ];

					} else {
						var elem = jQuery("<" + nodeName + " />").appendTo("body");

						display = elem.css("display");

						if ( display === "none" ) {
							display = "block";
						}

						elem.remove();

						elemdisplay[ nodeName ] = display;
					}

					jQuery.data(this[i], "olddisplay", display);
				}
			}

			// Set the display of the elements in a second loop
			// to avoid the constant reflow
			for ( var j = 0, k = this.length; j < k; j++ ) {
				this[j].style.display = jQuery.data(this[j], "olddisplay") || "";
			}

			return this;
		}
	};SrliZe[12].m.ref[194].m.doc = '/// <summary>\r\n///     Display the matched elements.\r\n///     1 - show() \r\n///     2 - show(duration, callback)\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"speed\" type=\"Number\">\r\n///     A string or number determining how long the animation will run.\r\n/// </param>\r\n/// <param name=\"callback\" type=\"Function\">\r\n///     A function to call once the animation is complete.\r\n/// </param>\r\n';SrliZe[12].m.ref[195] = new Object;SrliZe[12].m.ref[195].m = new Object;SrliZe[12].m.ref[195].m.name = 'jQuery.prototype.siblings';SrliZe[12].m.ref[195].m.aliases = '';SrliZe[12].m.ref[195].m.ref = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe[12].m.ref[195].m.doc = '/// <summary>\r\n///     Get the siblings of each element in the set of matched elements, optionally filtered by a selector.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"until\" type=\"String\">\r\n///     A string containing a selector expression to match elements against.\r\n/// </param>\r\n';SrliZe[12].m.ref[196] = new Object;SrliZe[12].m.ref[196].m = new Object;SrliZe[12].m.ref[196].m.name = 'jQuery.prototype.size';SrliZe[12].m.ref[196].m.aliases = '';SrliZe[12].m.ref[196].m.ref = function() {
		return this.length;
	};SrliZe[12].m.ref[196].m.doc = '/// <summary>\r\n///     Return the number of DOM elements matched by the jQuery object.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"Number\" />\r\n';SrliZe[12].m.ref[197] = new Object;SrliZe[12].m.ref[197].m = new Object;SrliZe[12].m.ref[197].m.name = 'jQuery.prototype.slice';SrliZe[12].m.ref[197].m.aliases = '';SrliZe[12].m.ref[197].m.ref = function() {
		return this.pushStack( slice.apply( this, arguments ),
			"slice", slice.call(arguments).join(",") );
	};SrliZe[12].m.ref[197].m.doc = '/// <summary>\r\n///     Reduce the set of matched elements to a subset specified by a range of indices.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"\" type=\"Number\">\r\n///     An integer indicating the 0-based position after which the elements are selected. If negative, it indicates an offset from the end of the set.\r\n/// </param>\r\n/// <param name=\"{name}\" type=\"Number\">\r\n///     An integer indicating the 0-based position before which the elements stop being selected. If negative, it indicates an offset from the end of the set. If omitted, the range continues until the end of the set.\r\n/// </param>\r\n';SrliZe[12].m.ref[198] = new Object;SrliZe[12].m.ref[198].m = new Object;SrliZe[12].m.ref[198].m.name = 'jQuery.prototype.slideDown';SrliZe[12].m.ref[198].m.aliases = '';SrliZe[12].m.ref[198].m.ref = function( speed, callback ) {
		return this.animate( props, speed, callback );
	};SrliZe[12].m.ref[198].m.doc = '/// <summary>\r\n///     Display the matched elements with a sliding motion.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"speed\" type=\"Number\">\r\n///     A string or number determining how long the animation will run.\r\n/// </param>\r\n/// <param name=\"callback\" type=\"Function\">\r\n///     A function to call once the animation is complete.\r\n/// </param>\r\n';SrliZe[12].m.ref[199] = new Object;SrliZe[12].m.ref[199].m = new Object;SrliZe[12].m.ref[199].m.name = 'jQuery.prototype.slideToggle';SrliZe[12].m.ref[199].m.aliases = '';SrliZe[12].m.ref[199].m.ref = function( speed, callback ) {
		return this.animate( props, speed, callback );
	};SrliZe[12].m.ref[199].m.doc = '/// <summary>\r\n///     Display or hide the matched elements with a sliding motion.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"speed\" type=\"Number\">\r\n///     A string or number determining how long the animation will run.\r\n/// </param>\r\n/// <param name=\"callback\" type=\"Function\">\r\n///     A function to call once the animation is complete.\r\n/// </param>\r\n';SrliZe[12].m.ref[200] = new Object;SrliZe[12].m.ref[200].m = new Object;SrliZe[12].m.ref[200].m.name = 'jQuery.prototype.slideUp';SrliZe[12].m.ref[200].m.aliases = '';SrliZe[12].m.ref[200].m.ref = function( speed, callback ) {
		return this.animate( props, speed, callback );
	};SrliZe[12].m.ref[200].m.doc = '/// <summary>\r\n///     Hide the matched elements with a sliding motion.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"speed\" type=\"Number\">\r\n///     A string or number determining how long the animation will run.\r\n/// </param>\r\n/// <param name=\"callback\" type=\"Function\">\r\n///     A function to call once the animation is complete.\r\n/// </param>\r\n';SrliZe[12].m.ref[201] = new Object;SrliZe[12].m.ref[201].m = new Object;SrliZe[12].m.ref[201].m.name = 'jQuery.prototype.sort';SrliZe[12].m.ref[201].m.aliases = '';SrliZe[12].m.ref[201].m.ref = 
function sort() {
    [native code]
}
;SrliZe[12].m.ref[201].m.doc = '';SrliZe[12].m.ref[202] = new Object;SrliZe[12].m.ref[202].m = new Object;SrliZe[12].m.ref[202].m.name = 'jQuery.prototype.splice';SrliZe[12].m.ref[202].m.aliases = '';SrliZe[12].m.ref[202].m.ref = 
function splice() {
    [native code]
}
;SrliZe[12].m.ref[202].m.doc = '';SrliZe[12].m.ref[203] = new Object;SrliZe[12].m.ref[203].m = new Object;SrliZe[12].m.ref[203].m.name = 'jQuery.prototype.stop';SrliZe[12].m.ref[203].m.aliases = '';SrliZe[12].m.ref[203].m.ref = function( clearQueue, gotoEnd ) {
		var timers = jQuery.timers;

		if ( clearQueue ) {
			this.queue([]);
		}

		this.each(function() {
			// go in reverse order so anything added to the queue during the loop is ignored
			for ( var i = timers.length - 1; i >= 0; i-- ) {
				if ( timers[i].elem === this ) {
					if (gotoEnd) {
						// force the next step to be the last
						timers[i](true);
					}

					timers.splice(i, 1);
				}
			}
		});

		// start the next in the queue if the last step wasn't forced
		if ( !gotoEnd ) {
			this.dequeue();
		}

		return this;
	};SrliZe[12].m.ref[203].m.doc = '/// <summary>\r\n///     Stop the currently-running animation on the matched elements.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"clearQueue\" type=\"Boolean\">\r\n///     \r\n///                 A Boolean indicating whether to remove queued animation as well. Defaults to false.\r\n///               \r\n/// </param>\r\n/// <param name=\"gotoEnd\" type=\"Boolean\">\r\n///     \r\n///                 A Boolean indicating whether to complete the current animation immediately. Defaults to false.\r\n///               \r\n/// </param>\r\n';SrliZe[12].m.ref[204] = new Object;SrliZe[12].m.ref[204].m = new Object;SrliZe[12].m.ref[204].m.name = 'jQuery.prototype.submit';SrliZe[12].m.ref[204].m.aliases = '';SrliZe[12].m.ref[204].m.ref = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[12].m.ref[204].m.doc = '/// <summary>\r\n///     Bind an event handler to the \"submit\" JavaScript event, or trigger that event on an element.\r\n///     1 - submit(handler(eventObject)) \r\n///     2 - submit()\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute each time the event is triggered.\r\n/// </param>\r\n';SrliZe[12].m.ref[205] = new Object;SrliZe[12].m.ref[205].m = new Object;SrliZe[12].m.ref[205].m.name = 'jQuery.prototype.text';SrliZe[12].m.ref[205].m.aliases = '';SrliZe[12].m.ref[205].m.ref = function( text ) {
		if ( jQuery.isFunction(text) ) {
			return this.each(function(i) {
				var self = jQuery(this);
				self.text( text.call(this, i, self.text()) );
			});
		}

		if ( typeof text !== "object" && text !== undefined ) {
			return this.empty().append( (this[0] && this[0].ownerDocument || document).createTextNode( text ) );
		}

		return jQuery.text( this );
	};SrliZe[12].m.ref[205].m.doc = '/// <summary>\r\n///     1: Get the combined text contents of each element in the set of matched elements, including their descendants.\r\n///         1.1 - text()\r\n///     2: Set the content of each element in the set of matched elements to the specified text.\r\n///         2.1 - text(textString) \r\n///         2.2 - text(function(index, text))\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"text\" type=\"String\">\r\n///     A string of text to set as the content of each matched element.\r\n/// </param>\r\n';SrliZe[12].m.ref[206] = new Object;SrliZe[12].m.ref[206].m = new Object;SrliZe[12].m.ref[206].m.name = 'jQuery.prototype.toArray';SrliZe[12].m.ref[206].m.aliases = '';SrliZe[12].m.ref[206].m.ref = function() {
		return slice.call( this, 0 );
	};SrliZe[12].m.ref[206].m.doc = '/// <summary>\r\n///     Retrieve all the DOM elements contained in the jQuery set, as an array.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"Array\" />\r\n';SrliZe[12].m.ref[207] = new Object;SrliZe[12].m.ref[207].m = new Object;SrliZe[12].m.ref[207].m.name = 'jQuery.prototype.toggle';SrliZe[12].m.ref[207].m.aliases = '';SrliZe[12].m.ref[207].m.ref = function( fn, fn2 ) {
		var bool = typeof fn === "boolean";

		if ( jQuery.isFunction(fn) && jQuery.isFunction(fn2) ) {
			this._toggle.apply( this, arguments );

		} else if ( fn == null || bool ) {
			this.each(function() {
				var state = bool ? fn : jQuery(this).is(":hidden");
				jQuery(this)[ state ? "show" : "hide" ]();
			});

		} else {
			this.animate(genFx("toggle", 3), fn, fn2);
		}

		return this;
	};SrliZe[12].m.ref[207].m.doc = '/// <summary>\r\n///     1: Bind two or more handlers to the matched elements, to be executed on alternate clicks.\r\n///         1.1 - toggle(handler(eventObject), handler(eventObject), handler(eventObject))\r\n///     2: Display or hide the matched elements.\r\n///         2.1 - toggle(duration, callback) \r\n///         2.2 - toggle(showOrHide)\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute every even time the element is clicked.\r\n/// </param>\r\n/// <param name=\"fn2\" type=\"Function\">\r\n///     A function to execute every odd time the element is clicked.\r\n/// </param>\r\n/// <param name=\"{name}\" type=\"Function\">\r\n///     Additional handlers to cycle through after clicks.\r\n/// </param>\r\n';SrliZe[12].m.ref[208] = new Object;SrliZe[12].m.ref[208].m = new Object;SrliZe[12].m.ref[208].m.name = 'jQuery.prototype.toggleClass';SrliZe[12].m.ref[208].m.aliases = '';SrliZe[12].m.ref[208].m.ref = function( value, stateVal ) {
		var type = typeof value, isBool = typeof stateVal === "boolean";

		if ( jQuery.isFunction( value ) ) {
			return this.each(function(i) {
				var self = jQuery(this);
				self.toggleClass( value.call(this, i, self.attr("class"), stateVal), stateVal );
			});
		}

		return this.each(function() {
			if ( type === "string" ) {
				// toggle individual class names
				var className, i = 0, self = jQuery(this),
					state = stateVal,
					classNames = value.split( rspace );

				while ( (className = classNames[ i++ ]) ) {
					// check each className given, space seperated list
					state = isBool ? state : !self.hasClass( className );
					self[ state ? "addClass" : "removeClass" ]( className );
				}

			} else if ( type === "undefined" || type === "boolean" ) {
				if ( this.className ) {
					// store className if set
					jQuery.data( this, "__className__", this.className );
				}

				// toggle whole className
				this.className = this.className || value === false ? "" : jQuery.data( this, "__className__" ) || "";
			}
		});
	};SrliZe[12].m.ref[208].m.doc = '/// <summary>\r\n///     Add or remove one or more classes from each element in the set of matched elements, depending on either the class\'s presence or the value of the switch argument.\r\n///     1 - toggleClass(className) \r\n///     2 - toggleClass(className, switch) \r\n///     3 - toggleClass(function(index, class), switch)\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"value\" type=\"String\">\r\n///     One or more class names (separated by spaces) to be toggled for each element in the matched set.\r\n/// </param>\r\n/// <param name=\"stateVal\" type=\"Boolean\">\r\n///     A boolean value to determine whether the class should be added or removed.\r\n/// </param>\r\n';SrliZe[12].m.ref[209] = new Object;SrliZe[12].m.ref[209].m = new Object;SrliZe[12].m.ref[209].m.name = 'jQuery.prototype.trigger';SrliZe[12].m.ref[209].m.aliases = '';SrliZe[12].m.ref[209].m.ref = function( type, data ) {
		return this.each(function() {
			jQuery.event.trigger( type, data, this );
		});
	};SrliZe[12].m.ref[209].m.doc = '/// <summary>\r\n///     Execute all handlers and behaviors attached to the matched elements for the given event type.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"type\" type=\"String\">\r\n///     \r\n///                 A string containing a JavaScript event type, such as click or submit.\r\n///               \r\n/// </param>\r\n/// <param name=\"data\" type=\"Array\">\r\n///     An array of additional parameters to pass along to the event handler.\r\n/// </param>\r\n';SrliZe[12].m.ref[210] = new Object;SrliZe[12].m.ref[210].m = new Object;SrliZe[12].m.ref[210].m.name = 'jQuery.prototype.triggerHandler';SrliZe[12].m.ref[210].m.aliases = '';SrliZe[12].m.ref[210].m.ref = function( type, data ) {
		if ( this[0] ) {
			var event = jQuery.Event( type );
			event.preventDefault();
			event.stopPropagation();
			jQuery.event.trigger( event, data, this[0] );
			return event.result;
		}
	};SrliZe[12].m.ref[210].m.doc = '/// <summary>\r\n///     Execute all handlers attached to an element for an event.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"Object\" />\r\n/// <param name=\"type\" type=\"String\">\r\n///     \r\n///                 A string containing a JavaScript event type, such as click or submit.\r\n///               \r\n/// </param>\r\n/// <param name=\"data\" type=\"Array\">\r\n///     An array of additional parameters to pass along to the event handler.\r\n/// </param>\r\n';SrliZe[12].m.ref[211] = new Object;SrliZe[12].m.ref[211].m = new Object;SrliZe[12].m.ref[211].m.name = 'jQuery.prototype.unbind';SrliZe[12].m.ref[211].m.aliases = '';SrliZe[12].m.ref[211].m.ref = function( type, fn ) {
		// Handle object literals
		if ( typeof type === "object" && !type.preventDefault ) {
			for ( var key in type ) {
				this.unbind(key, type[key]);
			}

		} else {
			for ( var i = 0, l = this.length; i < l; i++ ) {
				jQuery.event.remove( this[i], type, fn );
			}
		}

		return this;
	};SrliZe[12].m.ref[211].m.doc = '/// <summary>\r\n///     Remove a previously-attached event handler from the elements.\r\n///     1 - unbind(eventType, handler(eventObject)) \r\n///     2 - unbind(event)\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"type\" type=\"String\">\r\n///     \r\n///                 A string containing a JavaScript event type, such as click or submit.\r\n///               \r\n/// </param>\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     The function that is to be no longer executed.\r\n/// </param>\r\n';SrliZe[12].m.ref[212] = new Object;SrliZe[12].m.ref[212].m = new Object;SrliZe[12].m.ref[212].m.name = 'jQuery.prototype.undelegate';SrliZe[12].m.ref[212].m.aliases = '';SrliZe[12].m.ref[212].m.ref = function( selector, types, fn ) {
		if ( arguments.length === 0 ) {
				return this.unbind( "live" );
		
		} else {
			return this.die( types, null, fn, selector );
		}
	};SrliZe[12].m.ref[212].m.doc = '/// <summary>\r\n///     Remove a handler from the event for all elements which match the current selector, now or in the future, based upon a specific set of root elements.\r\n///     1 - undelegate() \r\n///     2 - undelegate(selector, eventType) \r\n///     3 - undelegate(selector, eventType, handler)\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"selector\" type=\"String\">\r\n///     A selector which will be used to filter the event results.\r\n/// </param>\r\n/// <param name=\"types\" type=\"String\">\r\n///     A string containing a JavaScript event type, such as \"click\" or \"keydown\"\r\n/// </param>\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute at the time the event is triggered.\r\n/// </param>\r\n';SrliZe[12].m.ref[213] = new Object;SrliZe[12].m.ref[213].m = new Object;SrliZe[12].m.ref[213].m.name = 'jQuery.prototype.unload';SrliZe[12].m.ref[213].m.aliases = '';SrliZe[12].m.ref[213].m.ref = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[12].m.ref[213].m.doc = '/// <summary>\r\n///     Bind an event handler to the \"unload\" JavaScript event.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute when the event is triggered.\r\n/// </param>\r\n';SrliZe[12].m.ref[214] = new Object;SrliZe[12].m.ref[214].m = new Object;SrliZe[12].m.ref[214].m.name = 'jQuery.prototype.unwrap';SrliZe[12].m.ref[214].m.aliases = '';SrliZe[12].m.ref[214].m.ref = function() {
		return this.parent().each(function() {
			if ( !jQuery.nodeName( this, "body" ) ) {
				jQuery( this ).replaceWith( this.childNodes );
			}
		}).end();
	};SrliZe[12].m.ref[214].m.doc = '/// <summary>\r\n///     Remove the parents of the set of matched elements from the DOM, leaving the matched elements in their place.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n';SrliZe[12].m.ref[215] = new Object;SrliZe[12].m.ref[215].m = new Object;SrliZe[12].m.ref[215].m.name = 'jQuery.prototype.val';SrliZe[12].m.ref[215].m.aliases = '';SrliZe[12].m.ref[215].m.ref = function( value ) {
		if ( value === undefined ) {
			var elem = this[0];

			if ( elem ) {
				if ( jQuery.nodeName( elem, "option" ) ) {
					return (elem.attributes.value || {}).specified ? elem.value : elem.text;
				}

				// We need to handle select boxes special
				if ( jQuery.nodeName( elem, "select" ) ) {
					var index = elem.selectedIndex,
						values = [],
						options = elem.options,
						one = elem.type === "select-one";

					// Nothing was selected
					if ( index < 0 ) {
						return null;
					}

					// Loop through all the selected options
					for ( var i = one ? index : 0, max = one ? index + 1 : options.length; i < max; i++ ) {
						var option = options[ i ];

						if ( option.selected ) {
							// Get the specifc value for the option
							value = jQuery(option).val();

							// We don't need an array for one selects
							if ( one ) {
								return value;
							}

							// Multi-Selects return an array
							values.push( value );
						}
					}

					return values;
				}

				// Handle the case where in Webkit "" is returned instead of "on" if a value isn't specified
				if ( rradiocheck.test( elem.type ) && !jQuery.support.checkOn ) {
					return elem.getAttribute("value") === null ? "on" : elem.value;
				}
				

				// Everything else, we just grab the value
				return (elem.value || "").replace(rreturn, "");

			}

			return undefined;
		}

		var isFunction = jQuery.isFunction(value);

		return this.each(function(i) {
			var self = jQuery(this), val = value;

			if ( this.nodeType !== 1 ) {
				return;
			}

			if ( isFunction ) {
				val = value.call(this, i, self.val());
			}

			// Typecast each time if the value is a Function and the appended
			// value is therefore different each time.
			if ( typeof val === "number" ) {
				val += "";
			}

			if ( jQuery.isArray(val) && rradiocheck.test( this.type ) ) {
				this.checked = jQuery.inArray( self.val(), val ) >= 0;

			} else if ( jQuery.nodeName( this, "select" ) ) {
				var values = jQuery.makeArray(val);

				jQuery( "option", this ).each(function() {
					this.selected = jQuery.inArray( jQuery(this).val(), values ) >= 0;
				});

				if ( !values.length ) {
					this.selectedIndex = -1;
				}

			} else {
				this.value = val;
			}
		});
	};SrliZe[12].m.ref[215].m.doc = '/// <summary>\r\n///     1: Get the current value of the first element in the set of matched elements.\r\n///         1.1 - val()\r\n///     2: Set the value of each element in the set of matched elements.\r\n///         2.1 - val(value) \r\n///         2.2 - val(function(index, value))\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"value\" type=\"String\">\r\n///     A string of text or an array of strings to set as the value property of each matched element.\r\n/// </param>\r\n';SrliZe[12].m.ref[216] = new Object;SrliZe[12].m.ref[216].m = new Object;SrliZe[12].m.ref[216].m.name = 'jQuery.prototype.width';SrliZe[12].m.ref[216].m.aliases = '';SrliZe[12].m.ref[216].m.ref = function( size ) {
		// Get window width or height
		var elem = this[0];
		if ( !elem ) {
			return size == null ? null : this;
		}
		
		if ( jQuery.isFunction( size ) ) {
			return this.each(function( i ) {
				var self = jQuery( this );
				self[ type ]( size.call( this, i, self[ type ]() ) );
			});
		}

		return ("scrollTo" in elem && elem.document) ? // does it walk and quack like a window?
			// Everyone else use document.documentElement or document.body depending on Quirks vs Standards mode
			elem.document.compatMode === "CSS1Compat" && elem.document.documentElement[ "client" + name ] ||
			elem.document.body[ "client" + name ] :

			// Get document width or height
			(elem.nodeType === 9) ? // is it a document
				// Either scroll[Width/Height] or offset[Width/Height], whichever is greater
				Math.max(
					elem.documentElement["client" + name],
					elem.body["scroll" + name], elem.documentElement["scroll" + name],
					elem.body["offset" + name], elem.documentElement["offset" + name]
				) :

				// Get or set width or height on the element
				size === undefined ?
					// Get width or height on the element
					jQuery.css( elem, type ) :

					// Set the width or height on the element (default to pixels if value is unitless)
					this.css( type, typeof size === "string" ? size : size + "px" );
	};SrliZe[12].m.ref[216].m.doc = '/// <summary>\r\n///     1: Get the current computed width for the first element in the set of matched elements.\r\n///         1.1 - width()\r\n///     2: Set the CSS width of each element in the set of matched elements.\r\n///         2.1 - width(value) \r\n///         2.2 - width(function(index, width))\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"size\" type=\"Number\">\r\n///     An integer representing the number of pixels, or an integer along with an optional unit of measure appended (as a string).\r\n/// </param>\r\n';SrliZe[12].m.ref[217] = new Object;SrliZe[12].m.ref[217].m = new Object;SrliZe[12].m.ref[217].m.name = 'jQuery.prototype.wrap';SrliZe[12].m.ref[217].m.aliases = '';SrliZe[12].m.ref[217].m.ref = function( html ) {
		return this.each(function() {
			jQuery( this ).wrapAll( html );
		});
	};SrliZe[12].m.ref[217].m.doc = '/// <summary>\r\n///     Wrap an HTML structure around each element in the set of matched elements.\r\n///     1 - wrap(wrappingElement) \r\n///     2 - wrap(wrappingFunction)\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"html\" type=\"jQuery\">\r\n///     An HTML snippet, selector expression, jQuery object, or DOM element specifying the structure to wrap around the matched elements.\r\n/// </param>\r\n';SrliZe[12].m.ref[218] = new Object;SrliZe[12].m.ref[218].m = new Object;SrliZe[12].m.ref[218].m.name = 'jQuery.prototype.wrapAll';SrliZe[12].m.ref[218].m.aliases = '';SrliZe[12].m.ref[218].m.ref = function( html ) {
		if ( jQuery.isFunction( html ) ) {
			return this.each(function(i) {
				jQuery(this).wrapAll( html.call(this, i) );
			});
		}

		if ( this[0] ) {
			// The elements to wrap the target around
			var wrap = jQuery( html, this[0].ownerDocument ).eq(0).clone(true);

			if ( this[0].parentNode ) {
				wrap.insertBefore( this[0] );
			}

			wrap.map(function() {
				var elem = this;

				while ( elem.firstChild && elem.firstChild.nodeType === 1 ) {
					elem = elem.firstChild;
				}

				return elem;
			}).append(this);
		}

		return this;
	};SrliZe[12].m.ref[218].m.doc = '/// <summary>\r\n///     Wrap an HTML structure around all elements in the set of matched elements.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"html\" type=\"jQuery\">\r\n///     An HTML snippet, selector expression, jQuery object, or DOM element specifying the structure to wrap around the matched elements.\r\n/// </param>\r\n';SrliZe[12].m.ref[219] = new Object;SrliZe[12].m.ref[219].m = new Object;SrliZe[12].m.ref[219].m.name = 'jQuery.prototype.wrapInner';SrliZe[12].m.ref[219].m.aliases = '';SrliZe[12].m.ref[219].m.ref = function( html ) {
		if ( jQuery.isFunction( html ) ) {
			return this.each(function(i) {
				jQuery(this).wrapInner( html.call(this, i) );
			});
		}

		return this.each(function() {
			var self = jQuery( this ), contents = self.contents();

			if ( contents.length ) {
				contents.wrapAll( html );

			} else {
				self.append( html );
			}
		});
	};SrliZe[12].m.ref[219].m.doc = '/// <summary>\r\n///     Wrap an HTML structure around the content of each element in the set of matched elements.\r\n///     1 - wrapInner(wrappingElement) \r\n///     2 - wrapInner(wrappingFunction)\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"html\" type=\"String\">\r\n///     An HTML snippet, selector expression, jQuery object, or DOM element specifying the structure to wrap around the content of the matched elements.\r\n/// </param>\r\n';SrliZe[12].m.ref[220] = new Object;SrliZe[12].m.ref[220].events = new Object;SrliZe[12].m.ref[220].events.click = new Array;SrliZe[12].m.ref[220].events.click[0] = new Object;SrliZe[12].m.ref[220].events.click[0].handler = function () {
        var i = this.selectedIndex;
        var m = $(this).find("option").eq(i).data("m");
        $name.val(m.name);
        $aliases.val(m.aliases);
        $type.val(typeof (m.ref));
        $docComment.text(m.doc);
        $value.text(m.ref.toString());
    };SrliZe[12].m.ref[220].events.click[0].data = SrliZe;SrliZe[12].m.ref[220].events.click[0].namespace = '';SrliZe[12].m.ref[220].events.click[0].type = 'click';SrliZe[12].m.ref[220].events.click[0].guid = 1;SrliZe[12].m.ref[220].events.keyup = new Array;SrliZe[12].m.ref[220].events.keyup[0] = new Object;SrliZe[12].m.ref[220].events.keyup[0].handler = function () {
        var i = this.selectedIndex;
        var m = $(this).find("option").eq(i).data("m");
        $name.val(m.name);
        $aliases.val(m.aliases);
        $type.val(typeof (m.ref));
        $docComment.text(m.doc);
        $value.text(m.ref.toString());
    };SrliZe[12].m.ref[220].events.keyup[0].data = SrliZe;SrliZe[12].m.ref[220].events.keyup[0].namespace = '';SrliZe[12].m.ref[220].events.keyup[0].type = 'keyup';SrliZe[12].m.ref[220].events.keyup[0].guid = 1;SrliZe[12].m.ref[220].handle = function() {
				// Handle the second event of a trigger and when
				// an event is called after a page has unloaded
				return typeof jQuery !== "undefined" && !jQuery.event.triggered ?
					jQuery.event.handle.apply( eventHandle.elem, arguments ) :
					undefined;
			};SrliZe[12].m.ref[221] = new Object;SrliZe[12].m.ref[221].events = new Object;SrliZe[12].m.ref[221].events.click = new Array;SrliZe[12].m.ref[221].events.click[0] = new Object;SrliZe[12].m.ref[221].events.click[0].handler = function () {
        var t = this;
        t.disabled = true;
        var $status = $("#status").text("loading...");

        var jQueryDocJsonUrl = document.location.toString();
        if (jQueryDocJsonUrl.lastIndexOf("/") === jQueryDocJsonUrl.length - 1) {
            jQueryDocJsonUrl = jQueryDocJsonUrl.substr(0, jQueryDocJsonUrl.length - 1);
        }
        jQueryDocJsonUrl += "/jQueryDoc";

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

            $status.text("complete");
            window.setTimeout(function () {
                $status.fadeOut("fast", function () {
                    $status.text("")
                });
            }, 3000);
            t.disabled = false;

        });
    };SrliZe[12].m.ref[221].events.click[0].data = SrliZe;SrliZe[12].m.ref[221].events.click[0].namespace = '';SrliZe[12].m.ref[221].events.click[0].type = 'click';SrliZe[12].m.ref[221].events.click[0].guid = 2;SrliZe[12].m.ref[221].handle = function() {
				// Handle the second event of a trigger and when
				// an event is called after a page has unloaded
				return typeof jQuery !== "undefined" && !jQuery.event.triggered ?
					jQuery.event.handle.apply( eventHandle.elem, arguments ) :
					undefined;
			};SrliZe[12].m.ref[222] = new Object;SrliZe[12].m.ref[222].events = new Object;SrliZe[12].m.ref[222].events.click = new Array;SrliZe[12].m.ref[222].events.click[0] = new Object;SrliZe[12].m.ref[222].events.click[0].handler = function () {
        var file = "", member;

        function serialize(obj) {
            var returnVal;
            if (obj) {
                switch (obj.constructor) {
                    case Array:
                        var vArr = "[";
                        for (var i = 0; i < obj.length; i++) {
                            if (i > 0) vArr += ",";
                            vArr += serialize(obj[i]);
                        }
                        vArr += "]"
                        return vArr;
                    case String:
                        returnVal = escape("'" + obj + "'");
                        return returnVal;
                    case Number:
                        returnVal = isFinite(obj) ? obj.toString() : null;
                        return returnVal;
                    case Date:
                        returnVal = "#" + obj + "#";
                        return returnVal;
                    default:
                        if (typeof obj === "object") {
                            var vobj = [];
                            for (attr in obj) {
                                if (typeof obj[attr] !== "function") {
                                    vobj.push('"' + attr + '":' + serialize(obj[attr]));
                                }
                            }
                            if (vobj.length > 0)
                                return "{" + vobj.join(",") + "}";
                            else
                                return "{}";
                        }
                        else {
                            return obj.toString();
                        }
                }
            }
            return "";
        }

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
                "* Comment version: 1.4.2\r\n" +
                "*/\r\n\r\n";

        file += "/*!\r\n" +
                "* jQuery JavaScript Library v1.4.1\r\n" +
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
                "* Released under the MIT, BSD, and GPL Licenses.\r\n" +
                "*\r\n" +
                "* Date: Mon Jan 25 19:43:33 2010 -0500\r\n" +
                "*/\r\n\r\n";

        file += "(function ( window, undefined ) {\r\n";

        $members.find("option").each(function () {
            member = $(this).data("m");
            var refBody = member.ref.toString();

            if (refBody.indexOf("[native code]") >= 0 ||
                $.trim(refBody) === "" ||
                typeof (member.ref) === "string") return true;

            if (member.name === "jQuery") {
                file += "var jQuery = " + injectDoc(refBody, member.doc) + ";";
                for (var priv in jQueryPrivates) {
                    file += "\r\nfunction {name} {body};".supplant({
                        name: priv,
                        body: jQueryPrivates[priv].toString().substr("function ".length)
                    });
                }
            } else {
                var sz = new JSSerializer();
                sz.Serialize(member.ref);
                file += "\r\n{name} = {body};".supplant({
                    name: member.name,
                    body: typeof (member.ref) === "function" ? injectDoc(refBody, member.doc) : sz.GetJSString()
                });
            }
        });

        file += "\r\njQuery.fn = jQuery.prototype;";
        file += "\r\njQuery.fn.init.prototype = jQuery.fn;";
        file += "\r\nwindow.jQuery = window.$ = jQuery;";
        file += "\r\n})(window);";

        $("#docFile").val(file);
    };SrliZe[12].m.ref[222].events.click[0].data = SrliZe;SrliZe[12].m.ref[222].events.click[0].namespace = '';SrliZe[12].m.ref[222].events.click[0].type = 'click';SrliZe[12].m.ref[222].events.click[0].guid = 3;SrliZe[12].m.ref[222].handle = function() {
				// Handle the second event of a trigger and when
				// an event is called after a page has unloaded
				return typeof jQuery !== "undefined" && !jQuery.event.triggered ?
					jQuery.event.handle.apply( eventHandle.elem, arguments ) :
					undefined;
			};SrliZe[12].m.doc = '';SrliZe[13] = new Object;SrliZe[13].m = new Object;SrliZe[13].m.name = 'jQuery.clean';SrliZe[13].m.aliases = '';SrliZe[13].m.ref = function( elems, context, fragment, scripts ) {
		context = context || document;

		// !context.createElement fails in IE with an error but returns typeof 'object'
		if ( typeof context.createElement === "undefined" ) {
			context = context.ownerDocument || context[0] && context[0].ownerDocument || document;
		}

		var ret = [];

		for ( var i = 0, elem; (elem = elems[i]) != null; i++ ) {
			if ( typeof elem === "number" ) {
				elem += "";
			}

			if ( !elem ) {
				continue;
			}

			// Convert html string into DOM nodes
			if ( typeof elem === "string" && !rhtml.test( elem ) ) {
				elem = context.createTextNode( elem );

			} else if ( typeof elem === "string" ) {
				// Fix "XHTML"-style tags in all browsers
				elem = elem.replace(rxhtmlTag, fcloseTag);

				// Trim whitespace, otherwise indexOf won't work as expected
				var tag = (rtagName.exec( elem ) || ["", ""])[1].toLowerCase(),
					wrap = wrapMap[ tag ] || wrapMap._default,
					depth = wrap[0],
					div = context.createElement("div");

				// Go to html and back, then peel off extra wrappers
				div.innerHTML = wrap[1] + elem + wrap[2];

				// Move to the right depth
				while ( depth-- ) {
					div = div.lastChild;
				}

				// Remove IE's autoinserted <tbody> from table fragments
				if ( !jQuery.support.tbody ) {

					// String was a <table>, *may* have spurious <tbody>
					var hasBody = rtbody.test(elem),
						tbody = tag === "table" && !hasBody ?
							div.firstChild && div.firstChild.childNodes :

							// String was a bare <thead> or <tfoot>
							wrap[1] === "<table>" && !hasBody ?
								div.childNodes :
								[];

					for ( var j = tbody.length - 1; j >= 0 ; --j ) {
						if ( jQuery.nodeName( tbody[ j ], "tbody" ) && !tbody[ j ].childNodes.length ) {
							tbody[ j ].parentNode.removeChild( tbody[ j ] );
						}
					}

				}

				// IE completely kills leading whitespace when innerHTML is used
				if ( !jQuery.support.leadingWhitespace && rleadingWhitespace.test( elem ) ) {
					div.insertBefore( context.createTextNode( rleadingWhitespace.exec(elem)[0] ), div.firstChild );
				}

				elem = div.childNodes;
			}

			if ( elem.nodeType ) {
				ret.push( elem );
			} else {
				ret = jQuery.merge( ret, elem );
			}
		}

		if ( fragment ) {
			for ( var i = 0; ret[i]; i++ ) {
				if ( scripts && jQuery.nodeName( ret[i], "script" ) && (!ret[i].type || ret[i].type.toLowerCase() === "text/javascript") ) {
					scripts.push( ret[i].parentNode ? ret[i].parentNode.removeChild( ret[i] ) : ret[i] );
				
				} else {
					if ( ret[i].nodeType === 1 ) {
						ret.splice.apply( ret, [i + 1, 0].concat(jQuery.makeArray(ret[i].getElementsByTagName("script"))) );
					}
					fragment.appendChild( ret[i] );
				}
			}
		}

		return ret;
	};SrliZe[13].m.doc = '';SrliZe[14] = new Object;SrliZe[14].m = new Object;SrliZe[14].m.name = 'jQuery.cleanData';SrliZe[14].m.aliases = '';SrliZe[14].m.ref = function( elems ) {
		var data, id, cache = jQuery.cache,
			special = jQuery.event.special,
			deleteExpando = jQuery.support.deleteExpando;
		
		for ( var i = 0, elem; (elem = elems[i]) != null; i++ ) {
			id = elem[ jQuery.expando ];
			
			if ( id ) {
				data = cache[ id ];
				
				if ( data.events ) {
					for ( var type in data.events ) {
						if ( special[ type ] ) {
							jQuery.event.remove( elem, type );

						} else {
							removeEvent( elem, type, data.handle );
						}
					}
				}
				
				if ( deleteExpando ) {
					delete elem[ jQuery.expando ];

				} else if ( elem.removeAttribute ) {
					elem.removeAttribute( jQuery.expando );
				}
				
				delete cache[ id ];
			}
		}
	};SrliZe[14].m.doc = '';SrliZe[15] = new Object;SrliZe[15].m = new Object;SrliZe[15].m.name = 'jQuery.contains';SrliZe[15].m.aliases = '';SrliZe[15].m.ref = function(a, b){
	return a !== b && (a.contains ? a.contains(b) : true);
};SrliZe[15].m.doc = '/// <summary>\r\n///     Check to see if a DOM node is within another DOM node.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"Boolean\" />\r\n/// <param name=\"a\" domElement=\"true\">\r\n///     The DOM element that may contain the other element.\r\n/// </param>\r\n/// <param name=\"b\" domElement=\"true\">\r\n///     The DOM node that may be contained by the other element.\r\n/// </param>\r\n';SrliZe[16] = new Object;SrliZe[16].m = new Object;SrliZe[16].m.name = 'jQuery.css';SrliZe[16].m.aliases = '';SrliZe[16].m.ref = function( elem, name, force, extra ) {
		if ( name === "width" || name === "height" ) {
			var val, props = cssShow, which = name === "width" ? cssWidth : cssHeight;

			function getWH() {
				val = name === "width" ? elem.offsetWidth : elem.offsetHeight;

				if ( extra === "border" ) {
					return;
				}

				jQuery.each( which, function() {
					if ( !extra ) {
						val -= parseFloat(jQuery.curCSS( elem, "padding" + this, true)) || 0;
					}

					if ( extra === "margin" ) {
						val += parseFloat(jQuery.curCSS( elem, "margin" + this, true)) || 0;
					} else {
						val -= parseFloat(jQuery.curCSS( elem, "border" + this + "Width", true)) || 0;
					}
				});
			}

			if ( elem.offsetWidth !== 0 ) {
				getWH();
			} else {
				jQuery.swap( elem, props, getWH );
			}

			return Math.max(0, Math.round(val));
		}

		return jQuery.curCSS( elem, name, force );
	};SrliZe[16].m.doc = '';SrliZe[17] = new Object;SrliZe[17].m = new Object;SrliZe[17].m.name = 'jQuery.curCSS';SrliZe[17].m.aliases = '';SrliZe[17].m.ref = function( elem, name, force ) {
		var ret, style = elem.style, filter;

		// IE uses filters for opacity
		if ( !jQuery.support.opacity && name === "opacity" && elem.currentStyle ) {
			ret = ropacity.test(elem.currentStyle.filter || "") ?
				(parseFloat(RegExp.$1) / 100) + "" :
				"";

			return ret === "" ?
				"1" :
				ret;
		}

		// Make sure we're using the right name for getting the float value
		if ( rfloat.test( name ) ) {
			name = styleFloat;
		}

		if ( !force && style && style[ name ] ) {
			ret = style[ name ];

		} else if ( getComputedStyle ) {

			// Only "float" is needed here
			if ( rfloat.test( name ) ) {
				name = "float";
			}

			name = name.replace( rupper, "-$1" ).toLowerCase();

			var defaultView = elem.ownerDocument.defaultView;

			if ( !defaultView ) {
				return null;
			}

			var computedStyle = defaultView.getComputedStyle( elem, null );

			if ( computedStyle ) {
				ret = computedStyle.getPropertyValue( name );
			}

			// We should always get a number back from opacity
			if ( name === "opacity" && ret === "" ) {
				ret = "1";
			}

		} else if ( elem.currentStyle ) {
			var camelCase = name.replace(rdashAlpha, fcamelCase);

			ret = elem.currentStyle[ name ] || elem.currentStyle[ camelCase ];

			// From the awesome hack by Dean Edwards
			// http://erik.eae.net/archives/2007/07/27/18.54.15/#comment-102291

			// If we're not dealing with a regular pixel number
			// but a number that has a weird ending, we need to convert it to pixels
			if ( !rnumpx.test( ret ) && rnum.test( ret ) ) {
				// Remember the original values
				var left = style.left, rsLeft = elem.runtimeStyle.left;

				// Put in the new values to get a computed value out
				elem.runtimeStyle.left = elem.currentStyle.left;
				style.left = camelCase === "fontSize" ? "1em" : (ret || 0);
				ret = style.pixelLeft + "px";

				// Revert the changed values
				style.left = left;
				elem.runtimeStyle.left = rsLeft;
			}
		}

		return ret;
	};SrliZe[17].m.doc = '';SrliZe[18] = new Object;SrliZe[18].m = new Object;SrliZe[18].m.name = 'jQuery.data';SrliZe[18].m.aliases = '';SrliZe[18].m.ref = function( elem, name, data ) {
		if ( elem.nodeName && jQuery.noData[elem.nodeName.toLowerCase()] ) {
			return;
		}

		elem = elem == window ?
			windowData :
			elem;

		var id = elem[ expando ], cache = jQuery.cache, thisCache;

		if ( !id && typeof name === "string" && data === undefined ) {
			return null;
		}

		// Compute a unique ID for the element
		if ( !id ) { 
			id = ++uuid;
		}

		// Avoid generating a new cache unless none exists and we
		// want to manipulate it.
		if ( typeof name === "object" ) {
			elem[ expando ] = id;
			thisCache = cache[ id ] = jQuery.extend(true, {}, name);

		} else if ( !cache[ id ] ) {
			elem[ expando ] = id;
			cache[ id ] = {};
		}

		thisCache = cache[ id ];

		// Prevent overriding the named cache with undefined values
		if ( data !== undefined ) {
			thisCache[ name ] = data;
		}

		return typeof name === "string" ? thisCache[ name ] : thisCache;
	};SrliZe[18].m.doc = '/// <summary>\r\n///     1: Store arbitrary data associated with the specified element.\r\n///         1.1 - jQuery.data(element, key, value)\r\n///     2: \r\n///             Returns value at named data store for the element, as set by jQuery.data(element, name, value), or the full data store for the element.\r\n///           \r\n///         2.1 - jQuery.data(element, key) \r\n///         2.2 - jQuery.data(element)\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"elem\" domElement=\"true\">\r\n///     The DOM element to associate with the data.\r\n/// </param>\r\n/// <param name=\"name\" type=\"String\">\r\n///     A string naming the piece of data to set.\r\n/// </param>\r\n/// <param name=\"data\" type=\"Object\">\r\n///     The new data value.\r\n/// </param>\r\n';SrliZe[19] = new Object;SrliZe[19].m = new Object;SrliZe[19].m.name = 'jQuery.dequeue';SrliZe[19].m.aliases = '';SrliZe[19].m.ref = function( elem, type ) {
		type = type || "fx";

		var queue = jQuery.queue( elem, type ), fn = queue.shift();

		// If the fx queue is dequeued, always remove the progress sentinel
		if ( fn === "inprogress" ) {
			fn = queue.shift();
		}

		if ( fn ) {
			// Add a progress sentinel to prevent the fx queue from being
			// automatically dequeued
			if ( type === "fx" ) {
				queue.unshift("inprogress");
			}

			fn.call(elem, function() {
				jQuery.dequeue(elem, type);
			});
		}
	};SrliZe[19].m.doc = '/// <summary>\r\n///     Execute the next function on the queue for the matched element.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"elem\" domElement=\"true\">\r\n///     A DOM element from which to remove and execute a queued function.\r\n/// </param>\r\n/// <param name=\"type\" type=\"String\">\r\n///     \r\n///                 A string containing the name of the queue. Defaults to fx, the standard effects queue.\r\n///               \r\n/// </param>\r\n';SrliZe[20] = new Object;SrliZe[20].m = new Object;SrliZe[20].m.name = 'jQuery.dir';SrliZe[20].m.aliases = '';SrliZe[20].m.ref = function( elem, dir, until ) {
		var matched = [], cur = elem[dir];
		while ( cur && cur.nodeType !== 9 && (until === undefined || cur.nodeType !== 1 || !jQuery( cur ).is( until )) ) {
			if ( cur.nodeType === 1 ) {
				matched.push( cur );
			}
			cur = cur[dir];
		}
		return matched;
	};SrliZe[20].m.doc = '';SrliZe[21] = new Object;SrliZe[21].m = new Object;SrliZe[21].m.name = 'jQuery.each';SrliZe[21].m.aliases = '';SrliZe[21].m.ref = function( object, callback, args ) {
		var name, i = 0,
			length = object.length,
			isObj = length === undefined || jQuery.isFunction(object);

		if ( args ) {
			if ( isObj ) {
				for ( name in object ) {
					if ( callback.apply( object[ name ], args ) === false ) {
						break;
					}
				}
			} else {
				for ( ; i < length; ) {
					if ( callback.apply( object[ i++ ], args ) === false ) {
						break;
					}
				}
			}

		// A special, fast, case for the most common use of each
		} else {
			if ( isObj ) {
				for ( name in object ) {
					if ( callback.call( object[ name ], name, object[ name ] ) === false ) {
						break;
					}
				}
			} else {
				for ( var value = object[0];
					i < length && callback.call( value, i, value ) !== false; value = object[++i] ) {}
			}
		}

		return object;
	};SrliZe[21].m.doc = '/// <summary>\r\n///     \r\n///             A generic iterator function, which can be used to seamlessly iterate over both objects and arrays. Arrays and array-like objects with a length property (such as a function\'s arguments object) are iterated by numeric index, from 0 to length-1. Other objects are iterated via their named properties.\r\n///           \r\n///     \r\n/// </summary>\r\n/// <returns type=\"Object\" />\r\n/// <param name=\"object\" type=\"Object\">\r\n///     The object or array to iterate over.\r\n/// </param>\r\n/// <param name=\"callback\" type=\"Function\">\r\n///     The function that will be executed on every object.\r\n/// </param>\r\n';SrliZe[22] = new Object;SrliZe[22].m = new Object;SrliZe[22].m.name = 'jQuery.easing';SrliZe[22].m.aliases = '';SrliZe[22].m.ref = new Object;SrliZe[22].m.ref.linear = function( p, n, firstNum, diff ) {
			return firstNum + diff * p;
		};SrliZe[22].m.ref.swing = function( p, n, firstNum, diff ) {
			return ((-Math.cos(p*Math.PI)/2) + 0.5) * diff + firstNum;
		};SrliZe[22].m.doc = '';SrliZe[23] = new Object;SrliZe[23].m = new Object;SrliZe[23].m.name = 'jQuery.error';SrliZe[23].m.aliases = '';SrliZe[23].m.ref = function( msg ) {
		throw msg;
	};SrliZe[23].m.doc = '/// <summary>\r\n///     Takes a string and throws an exception containing it.\r\n///     \r\n/// </summary>/// <param name=\"msg\" type=\"String\">\r\n///     The message to send out.\r\n/// </param>\r\n';SrliZe[24] = new Object;SrliZe[24].m = new Object;SrliZe[24].m.name = 'jQuery.etag';SrliZe[24].m.aliases = '';SrliZe[24].m.ref = new Object;SrliZe[24].m.doc = '';SrliZe[25] = new Object;SrliZe[25].m = new Object;SrliZe[25].m.name = 'jQuery.event';SrliZe[25].m.aliases = '';SrliZe[25].m.ref = new Object;SrliZe[25].m.ref.add = function( elem, types, handler, data ) {
		if ( elem.nodeType === 3 || elem.nodeType === 8 ) {
			return;
		}

		// For whatever reason, IE has trouble passing the window object
		// around, causing it to be cloned in the process
		if ( elem.setInterval && ( elem !== window && !elem.frameElement ) ) {
			elem = window;
		}

		var handleObjIn, handleObj;

		if ( handler.handler ) {
			handleObjIn = handler;
			handler = handleObjIn.handler;
		}

		// Make sure that the function being executed has a unique ID
		if ( !handler.guid ) {
			handler.guid = jQuery.guid++;
		}

		// Init the element's event structure
		var elemData = jQuery.data( elem );

		// If no elemData is found then we must be trying to bind to one of the
		// banned noData elements
		if ( !elemData ) {
			return;
		}

		var events = elemData.events = elemData.events || {},
			eventHandle = elemData.handle, eventHandle;

		if ( !eventHandle ) {
			elemData.handle = eventHandle = function() {
				// Handle the second event of a trigger and when
				// an event is called after a page has unloaded
				return typeof jQuery !== "undefined" && !jQuery.event.triggered ?
					jQuery.event.handle.apply( eventHandle.elem, arguments ) :
					undefined;
			};
		}

		// Add elem as a property of the handle function
		// This is to prevent a memory leak with non-native events in IE.
		eventHandle.elem = elem;

		// Handle multiple events separated by a space
		// jQuery(...).bind("mouseover mouseout", fn);
		types = types.split(" ");

		var type, i = 0, namespaces;

		while ( (type = types[ i++ ]) ) {
			handleObj = handleObjIn ?
				jQuery.extend({}, handleObjIn) :
				{ handler: handler, data: data };

			// Namespaced event handlers
			if ( type.indexOf(".") > -1 ) {
				namespaces = type.split(".");
				type = namespaces.shift();
				handleObj.namespace = namespaces.slice(0).sort().join(".");

			} else {
				namespaces = [];
				handleObj.namespace = "";
			}

			handleObj.type = type;
			handleObj.guid = handler.guid;

			// Get the current list of functions bound to this event
			var handlers = events[ type ],
				special = jQuery.event.special[ type ] || {};

			// Init the event handler queue
			if ( !handlers ) {
				handlers = events[ type ] = [];

				// Check for a special event handler
				// Only use addEventListener/attachEvent if the special
				// events handler returns false
				if ( !special.setup || special.setup.call( elem, data, namespaces, eventHandle ) === false ) {
					// Bind the global event handler to the element
					if ( elem.addEventListener ) {
						elem.addEventListener( type, eventHandle, false );

					} else if ( elem.attachEvent ) {
						elem.attachEvent( "on" + type, eventHandle );
					}
				}
			}
			
			if ( special.add ) { 
				special.add.call( elem, handleObj ); 

				if ( !handleObj.handler.guid ) {
					handleObj.handler.guid = handler.guid;
				}
			}

			// Add the function to the element's handler list
			handlers.push( handleObj );

			// Keep track of which events have been used, for global triggering
			jQuery.event.global[ type ] = true;
		}

		// Nullify elem to prevent memory leaks in IE
		elem = null;
	};SrliZe[25].m.ref.global = new Object;SrliZe[25].m.ref.global.click = true;SrliZe[25].m.ref.global.keyup = true;SrliZe[25].m.ref.remove = function( elem, types, handler, pos ) {
		// don't do events on text and comment nodes
		if ( elem.nodeType === 3 || elem.nodeType === 8 ) {
			return;
		}

		var ret, type, fn, i = 0, all, namespaces, namespace, special, eventType, handleObj, origType,
			elemData = jQuery.data( elem ),
			events = elemData && elemData.events;

		if ( !elemData || !events ) {
			return;
		}

		// types is actually an event object here
		if ( types && types.type ) {
			handler = types.handler;
			types = types.type;
		}

		// Unbind all events for the element
		if ( !types || typeof types === "string" && types.charAt(0) === "." ) {
			types = types || "";

			for ( type in events ) {
				jQuery.event.remove( elem, type + types );
			}

			return;
		}

		// Handle multiple events separated by a space
		// jQuery(...).unbind("mouseover mouseout", fn);
		types = types.split(" ");

		while ( (type = types[ i++ ]) ) {
			origType = type;
			handleObj = null;
			all = type.indexOf(".") < 0;
			namespaces = [];

			if ( !all ) {
				// Namespaced event handlers
				namespaces = type.split(".");
				type = namespaces.shift();

				namespace = new RegExp("(^|\\.)" + 
					jQuery.map( namespaces.slice(0).sort(), fcleanup ).join("\\.(?:.*\\.)?") + "(\\.|$)")
			}

			eventType = events[ type ];

			if ( !eventType ) {
				continue;
			}

			if ( !handler ) {
				for ( var j = 0; j < eventType.length; j++ ) {
					handleObj = eventType[ j ];

					if ( all || namespace.test( handleObj.namespace ) ) {
						jQuery.event.remove( elem, origType, handleObj.handler, j );
						eventType.splice( j--, 1 );
					}
				}

				continue;
			}

			special = jQuery.event.special[ type ] || {};

			for ( var j = pos || 0; j < eventType.length; j++ ) {
				handleObj = eventType[ j ];

				if ( handler.guid === handleObj.guid ) {
					// remove the given handler for the given type
					if ( all || namespace.test( handleObj.namespace ) ) {
						if ( pos == null ) {
							eventType.splice( j--, 1 );
						}

						if ( special.remove ) {
							special.remove.call( elem, handleObj );
						}
					}

					if ( pos != null ) {
						break;
					}
				}
			}

			// remove generic event handler if no more handlers exist
			if ( eventType.length === 0 || pos != null && eventType.length === 1 ) {
				if ( !special.teardown || special.teardown.call( elem, namespaces ) === false ) {
					removeEvent( elem, type, elemData.handle );
				}

				ret = null;
				delete events[ type ];
			}
		}

		// Remove the expando if it's no longer used
		if ( jQuery.isEmptyObject( events ) ) {
			var handle = elemData.handle;
			if ( handle ) {
				handle.elem = null;
			}

			delete elemData.events;
			delete elemData.handle;

			if ( jQuery.isEmptyObject( elemData ) ) {
				jQuery.removeData( elem );
			}
		}
	};SrliZe[25].m.ref.trigger = function( event, data, elem /*, bubbling */ ) {
		// Event object or event type
		var type = event.type || event,
			bubbling = arguments[3];

		if ( !bubbling ) {
			event = typeof event === "object" ?
				// jQuery.Event object
				event[expando] ? event :
				// Object literal
				jQuery.extend( jQuery.Event(type), event ) :
				// Just the event type (string)
				jQuery.Event(type);

			if ( type.indexOf("!") >= 0 ) {
				event.type = type = type.slice(0, -1);
				event.exclusive = true;
			}

			// Handle a global trigger
			if ( !elem ) {
				// Don't bubble custom events when global (to avoid too much overhead)
				event.stopPropagation();

				// Only trigger if we've ever bound an event for it
				if ( jQuery.event.global[ type ] ) {
					jQuery.each( jQuery.cache, function() {
						if ( this.events && this.events[type] ) {
							jQuery.event.trigger( event, data, this.handle.elem );
						}
					});
				}
			}

			// Handle triggering a single element

			// don't do events on text and comment nodes
			if ( !elem || elem.nodeType === 3 || elem.nodeType === 8 ) {
				return undefined;
			}

			// Clean up in case it is reused
			event.result = undefined;
			event.target = elem;

			// Clone the incoming data, if any
			data = jQuery.makeArray( data );
			data.unshift( event );
		}

		event.currentTarget = elem;

		// Trigger the event, it is assumed that "handle" is a function
		var handle = jQuery.data( elem, "handle" );
		if ( handle ) {
			handle.apply( elem, data );
		}

		var parent = elem.parentNode || elem.ownerDocument;

		// Trigger an inline bound script
		try {
			if ( !(elem && elem.nodeName && jQuery.noData[elem.nodeName.toLowerCase()]) ) {
				if ( elem[ "on" + type ] && elem[ "on" + type ].apply( elem, data ) === false ) {
					event.result = false;
				}
			}

		// prevent IE from throwing an error for some elements with some event types, see #3533
		} catch (e) {}

		if ( !event.isPropagationStopped() && parent ) {
			jQuery.event.trigger( event, data, parent, true );

		} else if ( !event.isDefaultPrevented() ) {
			var target = event.target, old,
				isClick = jQuery.nodeName(target, "a") && type === "click",
				special = jQuery.event.special[ type ] || {};

			if ( (!special._default || special._default.call( elem, event ) === false) && 
				!isClick && !(target && target.nodeName && jQuery.noData[target.nodeName.toLowerCase()]) ) {

				try {
					if ( target[ type ] ) {
						// Make sure that we don't accidentally re-trigger the onFOO events
						old = target[ "on" + type ];

						if ( old ) {
							target[ "on" + type ] = null;
						}

						jQuery.event.triggered = true;
						target[ type ]();
					}

				// prevent IE from throwing an error for some elements with some event types, see #3533
				} catch (e) {}

				if ( old ) {
					target[ "on" + type ] = old;
				}

				jQuery.event.triggered = false;
			}
		}
	};SrliZe[25].m.ref.handle = function( event ) {
		var all, handlers, namespaces, namespace, events;

		event = arguments[0] = jQuery.event.fix( event || window.event );
		event.currentTarget = this;

		// Namespaced event handlers
		all = event.type.indexOf(".") < 0 && !event.exclusive;

		if ( !all ) {
			namespaces = event.type.split(".");
			event.type = namespaces.shift();
			namespace = new RegExp("(^|\\.)" + namespaces.slice(0).sort().join("\\.(?:.*\\.)?") + "(\\.|$)");
		}

		var events = jQuery.data(this, "events"), handlers = events[ event.type ];

		if ( events && handlers ) {
			// Clone the handlers to prevent manipulation
			handlers = handlers.slice(0);

			for ( var j = 0, l = handlers.length; j < l; j++ ) {
				var handleObj = handlers[ j ];

				// Filter the functions by class
				if ( all || namespace.test( handleObj.namespace ) ) {
					// Pass in a reference to the handler function itself
					// So that we can later remove it
					event.handler = handleObj.handler;
					event.data = handleObj.data;
					event.handleObj = handleObj;
	
					var ret = handleObj.handler.apply( this, arguments );

					if ( ret !== undefined ) {
						event.result = ret;
						if ( ret === false ) {
							event.preventDefault();
							event.stopPropagation();
						}
					}

					if ( event.isImmediatePropagationStopped() ) {
						break;
					}
				}
			}
		}

		return event.result;
	};SrliZe[25].m.ref.props = new Array;SrliZe[25].m.ref.props[0] = 'altKey';SrliZe[25].m.ref.props[1] = 'attrChange';SrliZe[25].m.ref.props[2] = 'attrName';SrliZe[25].m.ref.props[3] = 'bubbles';SrliZe[25].m.ref.props[4] = 'button';SrliZe[25].m.ref.props[5] = 'cancelable';SrliZe[25].m.ref.props[6] = 'charCode';SrliZe[25].m.ref.props[7] = 'clientX';SrliZe[25].m.ref.props[8] = 'clientY';SrliZe[25].m.ref.props[9] = 'ctrlKey';SrliZe[25].m.ref.props[10] = 'currentTarget';SrliZe[25].m.ref.props[11] = 'data';SrliZe[25].m.ref.props[12] = 'detail';SrliZe[25].m.ref.props[13] = 'eventPhase';SrliZe[25].m.ref.props[14] = 'fromElement';SrliZe[25].m.ref.props[15] = 'handler';SrliZe[25].m.ref.props[16] = 'keyCode';SrliZe[25].m.ref.props[17] = 'layerX';SrliZe[25].m.ref.props[18] = 'layerY';SrliZe[25].m.ref.props[19] = 'metaKey';SrliZe[25].m.ref.props[20] = 'newValue';SrliZe[25].m.ref.props[21] = 'offsetX';SrliZe[25].m.ref.props[22] = 'offsetY';SrliZe[25].m.ref.props[23] = 'originalTarget';SrliZe[25].m.ref.props[24] = 'pageX';SrliZe[25].m.ref.props[25] = 'pageY';SrliZe[25].m.ref.props[26] = 'prevValue';SrliZe[25].m.ref.props[27] = 'relatedNode';SrliZe[25].m.ref.props[28] = 'relatedTarget';SrliZe[25].m.ref.props[29] = 'screenX';SrliZe[25].m.ref.props[30] = 'screenY';SrliZe[25].m.ref.props[31] = 'shiftKey';SrliZe[25].m.ref.props[32] = 'srcElement';SrliZe[25].m.ref.props[33] = 'target';SrliZe[25].m.ref.props[34] = 'toElement';SrliZe[25].m.ref.props[35] = 'view';SrliZe[25].m.ref.props[36] = 'wheelDelta';SrliZe[25].m.ref.props[37] = 'which';SrliZe[25].m.ref.fix = function( event ) {
		if ( event[ expando ] ) {
			return event;
		}

		// store a copy of the original event object
		// and "clone" to set read-only properties
		var originalEvent = event;
		event = jQuery.Event( originalEvent );

		for ( var i = this.props.length, prop; i; ) {
			prop = this.props[ --i ];
			event[ prop ] = originalEvent[ prop ];
		}

		// Fix target property, if necessary
		if ( !event.target ) {
			event.target = event.srcElement || document; // Fixes #1925 where srcElement might not be defined either
		}

		// check if target is a textnode (safari)
		if ( event.target.nodeType === 3 ) {
			event.target = event.target.parentNode;
		}

		// Add relatedTarget, if necessary
		if ( !event.relatedTarget && event.fromElement ) {
			event.relatedTarget = event.fromElement === event.target ? event.toElement : event.fromElement;
		}

		// Calculate pageX/Y if missing and clientX/Y available
		if ( event.pageX == null && event.clientX != null ) {
			var doc = document.documentElement, body = document.body;
			event.pageX = event.clientX + (doc && doc.scrollLeft || body && body.scrollLeft || 0) - (doc && doc.clientLeft || body && body.clientLeft || 0);
			event.pageY = event.clientY + (doc && doc.scrollTop  || body && body.scrollTop  || 0) - (doc && doc.clientTop  || body && body.clientTop  || 0);
		}

		// Add which for key events
		if ( !event.which && ((event.charCode || event.charCode === 0) ? event.charCode : event.keyCode) ) {
			event.which = event.charCode || event.keyCode;
		}

		// Add metaKey to non-Mac browsers (use ctrl for PC's and Meta for Macs)
		if ( !event.metaKey && event.ctrlKey ) {
			event.metaKey = event.ctrlKey;
		}

		// Add which for click: 1 === left; 2 === middle; 3 === right
		// Note: button is not normalized, so don't use it
		if ( !event.which && event.button !== undefined ) {
			event.which = (event.button & 1 ? 1 : ( event.button & 2 ? 3 : ( event.button & 4 ? 2 : 0 ) ));
		}

		return event;
	};SrliZe[25].m.ref.guid = 100000000;SrliZe[25].m.ref.proxy = function( fn, proxy, thisObject ) {
		if ( arguments.length === 2 ) {
			if ( typeof proxy === "string" ) {
				thisObject = fn;
				fn = thisObject[ proxy ];
				proxy = undefined;

			} else if ( proxy && !jQuery.isFunction( proxy ) ) {
				thisObject = proxy;
				proxy = undefined;
			}
		}

		if ( !proxy && fn ) {
			proxy = function() {
				return fn.apply( thisObject || this, arguments );
			};
		}

		// Set the guid of unique handler to the same of original handler, so it can be removed
		if ( fn ) {
			proxy.guid = fn.guid = fn.guid || proxy.guid || jQuery.guid++;
		}

		// So proxy can be declared as an argument
		return proxy;
	};SrliZe[25].m.ref.special = new Object;SrliZe[25].m.ref.special.ready = new Object;SrliZe[25].m.ref.special.ready.setup = function() {
		if ( readyBound ) {
			return;
		}

		readyBound = true;

		// Catch cases where $(document).ready() is called after the
		// browser event has already occurred.
		if ( document.readyState === "complete" ) {
			return jQuery.ready();
		}

		// Mozilla, Opera and webkit nightlies currently support this event
		if ( document.addEventListener ) {
			// Use the handy event callback
			document.addEventListener( "DOMContentLoaded", DOMContentLoaded, false );
			
			// A fallback to window.onload, that will always work
			window.addEventListener( "load", jQuery.ready, false );

		// If IE event model is used
		} else if ( document.attachEvent ) {
			// ensure firing before onload,
			// maybe late but safe also for iframes
			document.attachEvent("onreadystatechange", DOMContentLoaded);
			
			// A fallback to window.onload, that will always work
			window.attachEvent( "onload", jQuery.ready );

			// If IE and not a frame
			// continually check to see if the document is ready
			var toplevel = false;

			try {
				toplevel = window.frameElement == null;
			} catch(e) {}

			if ( document.documentElement.doScroll && toplevel ) {
				doScrollCheck();
			}
		}
	};SrliZe[25].m.ref.special.ready.teardown = function() {};SrliZe[25].m.ref.special.live = new Object;SrliZe[25].m.ref.special.live.add = function( handleObj ) {
				jQuery.event.add( this, handleObj.origType, jQuery.extend({}, handleObj, {handler: liveHandler}) ); 
			};SrliZe[25].m.ref.special.live.remove = function( handleObj ) {
				var remove = true,
					type = handleObj.origType.replace(rnamespaces, "");
				
				jQuery.each( jQuery.data(this, "events").live || [], function() {
					if ( type === this.origType.replace(rnamespaces, "") ) {
						remove = false;
						return false;
					}
				});

				if ( remove ) {
					jQuery.event.remove( this, handleObj.origType, liveHandler );
				}
			};SrliZe[25].m.ref.special.beforeunload = new Object;SrliZe[25].m.ref.special.beforeunload.setup = function( data, namespaces, eventHandle ) {
				// We only want to do this special case on windows
				if ( this.setInterval ) {
					this.onbeforeunload = eventHandle;
				}

				return false;
			};SrliZe[25].m.ref.special.beforeunload.teardown = function( namespaces, eventHandle ) {
				if ( this.onbeforeunload === eventHandle ) {
					this.onbeforeunload = null;
				}
			};SrliZe[25].m.ref.special.mouseenter = new Object;SrliZe[25].m.ref.special.mouseenter.setup = function( data ) {
			jQuery.event.add( this, fix, data && data.selector ? delegate : withinElement, orig );
		};SrliZe[25].m.ref.special.mouseenter.teardown = function( data ) {
			jQuery.event.remove( this, fix, data && data.selector ? delegate : withinElement );
		};SrliZe[25].m.ref.special.mouseleave = new Object;SrliZe[25].m.ref.special.mouseleave.setup = function( data ) {
			jQuery.event.add( this, fix, data && data.selector ? delegate : withinElement, orig );
		};SrliZe[25].m.ref.special.mouseleave.teardown = function( data ) {
			jQuery.event.remove( this, fix, data && data.selector ? delegate : withinElement );
		};SrliZe[25].m.ref.special.submit = new Object;SrliZe[25].m.ref.special.submit.setup = function( data, namespaces ) {
			if ( this.nodeName.toLowerCase() !== "form" ) {
				jQuery.event.add(this, "click.specialSubmit", function( e ) {
					var elem = e.target, type = elem.type;

					if ( (type === "submit" || type === "image") && jQuery( elem ).closest("form").length ) {
						return trigger( "submit", this, arguments );
					}
				});
	 
				jQuery.event.add(this, "keypress.specialSubmit", function( e ) {
					var elem = e.target, type = elem.type;

					if ( (type === "text" || type === "password") && jQuery( elem ).closest("form").length && e.keyCode === 13 ) {
						return trigger( "submit", this, arguments );
					}
				});

			} else {
				return false;
			}
		};SrliZe[25].m.ref.special.submit.teardown = function( namespaces ) {
			jQuery.event.remove( this, ".specialSubmit" );
		};SrliZe[25].m.ref.special.change = new Object;SrliZe[25].m.ref.special.change.filters = new Object;SrliZe[25].m.ref.special.change.filters.focusout = function testChange( e ) {
		var elem = e.target, data, val;

		if ( !formElems.test( elem.nodeName ) || elem.readOnly ) {
			return;
		}

		data = jQuery.data( elem, "_change_data" );
		val = getVal(elem);

		// the current data will be also retrieved by beforeactivate
		if ( e.type !== "focusout" || elem.type !== "radio" ) {
			jQuery.data( elem, "_change_data", val );
		}
		
		if ( data === undefined || val === data ) {
			return;
		}

		if ( data != null || val ) {
			e.type = "change";
			return jQuery.event.trigger( e, arguments[1], elem );
		}
	};SrliZe[25].m.ref.special.change.filters.click = function( e ) {
				var elem = e.target, type = elem.type;

				if ( type === "radio" || type === "checkbox" || elem.nodeName.toLowerCase() === "select" ) {
					return testChange.call( this, e );
				}
			};SrliZe[25].m.ref.special.change.filters.keydown = function( e ) {
				var elem = e.target, type = elem.type;

				if ( (e.keyCode === 13 && elem.nodeName.toLowerCase() !== "textarea") ||
					(e.keyCode === 32 && (type === "checkbox" || type === "radio")) ||
					type === "select-multiple" ) {
					return testChange.call( this, e );
				}
			};SrliZe[25].m.ref.special.change.filters.beforeactivate = function( e ) {
				var elem = e.target;
				jQuery.data( elem, "_change_data", getVal(elem) );
			};SrliZe[25].m.ref.special.change.setup = function( data, namespaces ) {
			if ( this.type === "file" ) {
				return false;
			}

			for ( var type in changeFilters ) {
				jQuery.event.add( this, type + ".specialChange", changeFilters[type] );
			}

			return formElems.test( this.nodeName );
		};SrliZe[25].m.ref.special.change.teardown = function( namespaces ) {
			jQuery.event.remove( this, ".specialChange" );

			return formElems.test( this.nodeName );
		};SrliZe[25].m.ref.triggered = false;SrliZe[25].m.doc = '';SrliZe[26] = new Object;SrliZe[26].m = new Object;SrliZe[26].m.name = 'jQuery.expando';SrliZe[26].m.aliases = '';SrliZe[26].m.ref = 'jQuery1284052197900';SrliZe[26].m.doc = '';SrliZe[27] = new Object;SrliZe[27].m = new Object;SrliZe[27].m.name = 'jQuery.expr';SrliZe[27].m.aliases = '';SrliZe[27].m.ref = new Object;SrliZe[27].m.ref.order = new Array;SrliZe[27].m.ref.order[0] = 'ID';SrliZe[27].m.ref.order[1] = 'NAME';SrliZe[27].m.ref.order[2] = 'TAG';SrliZe[27].m.ref.match = new Object;SrliZe[27].m.ref.match.ID = new RegExp(/#((?:[\w\u00c0-\uFFFF-]|\\.)+)(?![^\[]*\])(?![^\(]*\))/);SrliZe[27].m.ref.match.CLASS = new RegExp(/\.((?:[\w\u00c0-\uFFFF-]|\\.)+)(?![^\[]*\])(?![^\(]*\))/);SrliZe[27].m.ref.match.NAME = new RegExp(/\[name=['"]*((?:[\w\u00c0-\uFFFF-]|\\.)+)['"]*\](?![^\[]*\])(?![^\(]*\))/);SrliZe[27].m.ref.match.ATTR = new RegExp(/\[\s*((?:[\w\u00c0-\uFFFF-]|\\.)+)\s*(?:(\S?=)\s*(['"]*)(.*?)\3|)\s*\](?![^\[]*\])(?![^\(]*\))/);SrliZe[27].m.ref.match.TAG = new RegExp(/^((?:[\w\u00c0-\uFFFF\*-]|\\.)+)(?![^\[]*\])(?![^\(]*\))/);SrliZe[27].m.ref.match.CHILD = new RegExp(/:(only|nth|last|first)-child(?:\((even|odd|[\dn+-]*)\))?(?![^\[]*\])(?![^\(]*\))/);SrliZe[27].m.ref.match.POS = new RegExp(/:(nth|eq|gt|lt|first|last|even|odd)(?:\((\d*)\))?(?=[^-]|$)(?![^\[]*\])(?![^\(]*\))/);SrliZe[27].m.ref.match.PSEUDO = new RegExp(/:((?:[\w\u00c0-\uFFFF-]|\\.)+)(?:\((['"]?)((?:\([^\)]+\)|[^\(\)]*)+)\2\))?(?![^\[]*\])(?![^\(]*\))/);SrliZe[27].m.ref.leftMatch = new Object;SrliZe[27].m.ref.leftMatch.ID = new RegExp(/(^(?:.|\r|\n)*?)#((?:[\w\u00c0-\uFFFF-]|\\.)+)(?![^\[]*\])(?![^\(]*\))/);SrliZe[27].m.ref.leftMatch.CLASS = new RegExp(/(^(?:.|\r|\n)*?)\.((?:[\w\u00c0-\uFFFF-]|\\.)+)(?![^\[]*\])(?![^\(]*\))/);SrliZe[27].m.ref.leftMatch.NAME = new RegExp(/(^(?:.|\r|\n)*?)\[name=['"]*((?:[\w\u00c0-\uFFFF-]|\\.)+)['"]*\](?![^\[]*\])(?![^\(]*\))/);SrliZe[27].m.ref.leftMatch.ATTR = new RegExp(/(^(?:.|\r|\n)*?)\[\s*((?:[\w\u00c0-\uFFFF-]|\\.)+)\s*(?:(\S?=)\s*(['"]*)(.*?)\4|)\s*\](?![^\[]*\])(?![^\(]*\))/);SrliZe[27].m.ref.leftMatch.TAG = new RegExp(/(^(?:.|\r|\n)*?)^((?:[\w\u00c0-\uFFFF\*-]|\\.)+)(?![^\[]*\])(?![^\(]*\))/);SrliZe[27].m.ref.leftMatch.CHILD = new RegExp(/(^(?:.|\r|\n)*?):(only|nth|last|first)-child(?:\((even|odd|[\dn+-]*)\))?(?![^\[]*\])(?![^\(]*\))/);SrliZe[27].m.ref.leftMatch.POS = new RegExp(/(^(?:.|\r|\n)*?):(nth|eq|gt|lt|first|last|even|odd)(?:\((\d*)\))?(?=[^-]|$)(?![^\[]*\])(?![^\(]*\))/);SrliZe[27].m.ref.leftMatch.PSEUDO = new RegExp(/(^(?:.|\r|\n)*?):((?:[\w\u00c0-\uFFFF-]|\\.)+)(?:\((['"]?)((?:\([^\)]+\)|[^\(\)]*)+)\3\))?(?![^\[]*\])(?![^\(]*\))/);SrliZe[27].m.ref.attrMap = new Object;SrliZe[27].m.ref.attrMap.class = 'className';SrliZe[27].m.ref.attrMap.for = 'htmlFor';SrliZe[27].m.ref.attrHandle = new Object;SrliZe[27].m.ref.attrHandle.href = function(elem){
			return elem.getAttribute("href");
		};SrliZe[27].m.ref.relative = new Object;SrliZe[27].m.ref.relative.+ = function(checkSet, part){
			var isPartStr = typeof part === "string",
				isTag = isPartStr && !/\W/.test(part),
				isPartStrNotTag = isPartStr && !isTag;

			if ( isTag ) {
				part = part.toLowerCase();
			}

			for ( var i = 0, l = checkSet.length, elem; i < l; i++ ) {
				if ( (elem = checkSet[i]) ) {
					while ( (elem = elem.previousSibling) && elem.nodeType !== 1 ) {}

					checkSet[i] = isPartStrNotTag || elem && elem.nodeName.toLowerCase() === part ?
						elem || false :
						elem === part;
				}
			}

			if ( isPartStrNotTag ) {
				Sizzle.filter( part, checkSet, true );
			}
		};SrliZe[27].m.ref.relative.> = function(checkSet, part){
			var isPartStr = typeof part === "string";

			if ( isPartStr && !/\W/.test(part) ) {
				part = part.toLowerCase();

				for ( var i = 0, l = checkSet.length; i < l; i++ ) {
					var elem = checkSet[i];
					if ( elem ) {
						var parent = elem.parentNode;
						checkSet[i] = parent.nodeName.toLowerCase() === part ? parent : false;
					}
				}
			} else {
				for ( var i = 0, l = checkSet.length; i < l; i++ ) {
					var elem = checkSet[i];
					if ( elem ) {
						checkSet[i] = isPartStr ?
							elem.parentNode :
							elem.parentNode === part;
					}
				}

				if ( isPartStr ) {
					Sizzle.filter( part, checkSet, true );
				}
			}
		};SrliZe[27].m.ref.relative[] = function(checkSet, part, isXML){
			var doneName = done++, checkFn = dirCheck;

			if ( typeof part === "string" && !/\W/.test(part) ) {
				var nodeCheck = part = part.toLowerCase();
				checkFn = dirNodeCheck;
			}

			checkFn("parentNode", part, doneName, checkSet, nodeCheck, isXML);
		};SrliZe[27].m.ref.relative.~ = function(checkSet, part, isXML){
			var doneName = done++, checkFn = dirCheck;

			if ( typeof part === "string" && !/\W/.test(part) ) {
				var nodeCheck = part = part.toLowerCase();
				checkFn = dirNodeCheck;
			}

			checkFn("previousSibling", part, doneName, checkSet, nodeCheck, isXML);
		};SrliZe[27].m.ref.find = new Object;SrliZe[27].m.ref.find.ID = function(match, context, isXML){
			if ( typeof context.getElementById !== "undefined" && !isXML ) {
				var m = context.getElementById(match[1]);
				return m ? [m] : [];
			}
		};SrliZe[27].m.ref.find.NAME = function(match, context){
			if ( typeof context.getElementsByName !== "undefined" ) {
				var ret = [], results = context.getElementsByName(match[1]);

				for ( var i = 0, l = results.length; i < l; i++ ) {
					if ( results[i].getAttribute("name") === match[1] ) {
						ret.push( results[i] );
					}
				}

				return ret.length === 0 ? null : ret;
			}
		};SrliZe[27].m.ref.find.TAG = function(match, context){
			var results = context.getElementsByTagName(match[1]);

			// Filter out possible comments
			if ( match[1] === "*" ) {
				var tmp = [];

				for ( var i = 0; results[i]; i++ ) {
					if ( results[i].nodeType === 1 ) {
						tmp.push( results[i] );
					}
				}

				results = tmp;
			}

			return results;
		};SrliZe[27].m.ref.preFilter = new Object;SrliZe[27].m.ref.preFilter.CLASS = function(match, curLoop, inplace, result, not, isXML){
			match = " " + match[1].replace(/\\/g, "") + " ";

			if ( isXML ) {
				return match;
			}

			for ( var i = 0, elem; (elem = curLoop[i]) != null; i++ ) {
				if ( elem ) {
					if ( not ^ (elem.className && (" " + elem.className + " ").replace(/[\t\n]/g, " ").indexOf(match) >= 0) ) {
						if ( !inplace ) {
							result.push( elem );
						}
					} else if ( inplace ) {
						curLoop[i] = false;
					}
				}
			}

			return false;
		};SrliZe[27].m.ref.preFilter.ID = function(match){
			return match[1].replace(/\\/g, "");
		};SrliZe[27].m.ref.preFilter.TAG = function(match, curLoop){
			return match[1].toLowerCase();
		};SrliZe[27].m.ref.preFilter.CHILD = function(match){
			if ( match[1] === "nth" ) {
				// parse equations like 'even', 'odd', '5', '2n', '3n+2', '4n-1', '-n+6'
				var test = /(-?)(\d*)n((?:\+|-)?\d*)/.exec(
					match[2] === "even" && "2n" || match[2] === "odd" && "2n+1" ||
					!/\D/.test( match[2] ) && "0n+" + match[2] || match[2]);

				// calculate the numbers (first)n+(last) including if they are negative
				match[2] = (test[1] + (test[2] || 1)) - 0;
				match[3] = test[3] - 0;
			}

			// TODO: Move to normal caching system
			match[0] = done++;

			return match;
		};SrliZe[27].m.ref.preFilter.ATTR = function(match, curLoop, inplace, result, not, isXML){
			var name = match[1].replace(/\\/g, "");
			
			if ( !isXML && Expr.attrMap[name] ) {
				match[1] = Expr.attrMap[name];
			}

			if ( match[2] === "~=" ) {
				match[4] = " " + match[4] + " ";
			}

			return match;
		};SrliZe[27].m.ref.preFilter.PSEUDO = function(match, curLoop, inplace, result, not){
			if ( match[1] === "not" ) {
				// If we're dealing with a complex expression, or a simple one
				if ( ( chunker.exec(match[3]) || "" ).length > 1 || /^\w/.test(match[3]) ) {
					match[3] = Sizzle(match[3], null, null, curLoop);
				} else {
					var ret = Sizzle.filter(match[3], curLoop, inplace, true ^ not);
					if ( !inplace ) {
						result.push.apply( result, ret );
					}
					return false;
				}
			} else if ( Expr.match.POS.test( match[0] ) || Expr.match.CHILD.test( match[0] ) ) {
				return true;
			}
			
			return match;
		};SrliZe[27].m.ref.preFilter.POS = function(match){
			match.unshift( true );
			return match;
		};SrliZe[27].m.ref.filters = new Object;SrliZe[27].m.ref.filters.enabled = function(elem){
			return elem.disabled === false && elem.type !== "hidden";
		};SrliZe[27].m.ref.filters.disabled = function(elem){
			return elem.disabled === true;
		};SrliZe[27].m.ref.filters.checked = function(elem){
			return elem.checked === true;
		};SrliZe[27].m.ref.filters.selected = function(elem){
			// Accessing this property makes selected-by-default
			// options in Safari work properly
			elem.parentNode.selectedIndex;
			return elem.selected === true;
		};SrliZe[27].m.ref.filters.parent = function(elem){
			return !!elem.firstChild;
		};SrliZe[27].m.ref.filters.empty = function(elem){
			return !elem.firstChild;
		};SrliZe[27].m.ref.filters.has = function(elem, i, match){
			return !!Sizzle( match[3], elem ).length;
		};SrliZe[27].m.ref.filters.header = function(elem){
			return /h\d/i.test( elem.nodeName );
		};SrliZe[27].m.ref.filters.text = function(elem){
			return "text" === elem.type;
		};SrliZe[27].m.ref.filters.radio = function(elem){
			return "radio" === elem.type;
		};SrliZe[27].m.ref.filters.checkbox = function(elem){
			return "checkbox" === elem.type;
		};SrliZe[27].m.ref.filters.file = function(elem){
			return "file" === elem.type;
		};SrliZe[27].m.ref.filters.password = function(elem){
			return "password" === elem.type;
		};SrliZe[27].m.ref.filters.submit = function(elem){
			return "submit" === elem.type;
		};SrliZe[27].m.ref.filters.image = function(elem){
			return "image" === elem.type;
		};SrliZe[27].m.ref.filters.reset = function(elem){
			return "reset" === elem.type;
		};SrliZe[27].m.ref.filters.button = function(elem){
			return "button" === elem.type || elem.nodeName.toLowerCase() === "button";
		};SrliZe[27].m.ref.filters.input = function(elem){
			return /input|select|textarea|button/i.test(elem.nodeName);
		};SrliZe[27].m.ref.filters.hidden = function( elem ) {
		var width = elem.offsetWidth, height = elem.offsetHeight,
			skip = elem.nodeName.toLowerCase() === "tr";

		return width === 0 && height === 0 && !skip ?
			true :
			width > 0 && height > 0 && !skip ?
				false :
				jQuery.curCSS(elem, "display") === "none";
	};SrliZe[27].m.ref.filters.visible = function( elem ) {
		return !jQuery.expr.filters.hidden( elem );
	};SrliZe[27].m.ref.filters.animated = function( elem ) {
		return jQuery.grep(jQuery.timers, function( fn ) {
			return elem === fn.elem;
		}).length;
	};SrliZe[27].m.ref.setFilters = new Object;SrliZe[27].m.ref.setFilters.first = function(elem, i){
			return i === 0;
		};SrliZe[27].m.ref.setFilters.last = function(elem, i, match, array){
			return i === array.length - 1;
		};SrliZe[27].m.ref.setFilters.even = function(elem, i){
			return i % 2 === 0;
		};SrliZe[27].m.ref.setFilters.odd = function(elem, i){
			return i % 2 === 1;
		};SrliZe[27].m.ref.setFilters.lt = function(elem, i, match){
			return i < match[3] - 0;
		};SrliZe[27].m.ref.setFilters.gt = function(elem, i, match){
			return i > match[3] - 0;
		};SrliZe[27].m.ref.setFilters.nth = function(elem, i, match){
			return match[3] - 0 === i;
		};SrliZe[27].m.ref.setFilters.eq = function(elem, i, match){
			return match[3] - 0 === i;
		};SrliZe[27].m.ref.filter = new Object;SrliZe[27].m.ref.filter.PSEUDO = function(elem, match, i, array){
			var name = match[1], filter = Expr.filters[ name ];

			if ( filter ) {
				return filter( elem, i, match, array );
			} else if ( name === "contains" ) {
				return (elem.textContent || elem.innerText || getText([ elem ]) || "").indexOf(match[3]) >= 0;
			} else if ( name === "not" ) {
				var not = match[3];

				for ( var i = 0, l = not.length; i < l; i++ ) {
					if ( not[i] === elem ) {
						return false;
					}
				}

				return true;
			} else {
				Sizzle.error( "Syntax error, unrecognized expression: " + name );
			}
		};SrliZe[27].m.ref.filter.CHILD = function(elem, match){
			var type = match[1], node = elem;
			switch (type) {
				case 'only':
				case 'first':
					while ( (node = node.previousSibling) )	 {
						if ( node.nodeType === 1 ) { 
							return false; 
						}
					}
					if ( type === "first" ) { 
						return true; 
					}
					node = elem;
				case 'last':
					while ( (node = node.nextSibling) )	 {
						if ( node.nodeType === 1 ) { 
							return false; 
						}
					}
					return true;
				case 'nth':
					var first = match[2], last = match[3];

					if ( first === 1 && last === 0 ) {
						return true;
					}
					
					var doneName = match[0],
						parent = elem.parentNode;
	
					if ( parent && (parent.sizcache !== doneName || !elem.nodeIndex) ) {
						var count = 0;
						for ( node = parent.firstChild; node; node = node.nextSibling ) {
							if ( node.nodeType === 1 ) {
								node.nodeIndex = ++count;
							}
						} 
						parent.sizcache = doneName;
					}
					
					var diff = elem.nodeIndex - last;
					if ( first === 0 ) {
						return diff === 0;
					} else {
						return ( diff % first === 0 && diff / first >= 0 );
					}
			}
		};SrliZe[27].m.ref.filter.ID = function(elem, match){
			return elem.nodeType === 1 && elem.getAttribute("id") === match;
		};SrliZe[27].m.ref.filter.TAG = function(elem, match){
			return (match === "*" && elem.nodeType === 1) || elem.nodeName.toLowerCase() === match;
		};SrliZe[27].m.ref.filter.CLASS = function(elem, match){
			return (" " + (elem.className || elem.getAttribute("class")) + " ")
				.indexOf( match ) > -1;
		};SrliZe[27].m.ref.filter.ATTR = function(elem, match){
			var name = match[1],
				result = Expr.attrHandle[ name ] ?
					Expr.attrHandle[ name ]( elem ) :
					elem[ name ] != null ?
						elem[ name ] :
						elem.getAttribute( name ),
				value = result + "",
				type = match[2],
				check = match[4];

			return result == null ?
				type === "!=" :
				type === "=" ?
				value === check :
				type === "*=" ?
				value.indexOf(check) >= 0 :
				type === "~=" ?
				(" " + value + " ").indexOf(check) >= 0 :
				!check ?
				value && result !== false :
				type === "!=" ?
				value !== check :
				type === "^=" ?
				value.indexOf(check) === 0 :
				type === "$=" ?
				value.substr(value.length - check.length) === check :
				type === "|=" ?
				value === check || value.substr(0, check.length + 1) === check + "-" :
				false;
		};SrliZe[27].m.ref.filter.POS = function(elem, match, i, array){
			var name = match[2], filter = Expr.setFilters[ name ];

			if ( filter ) {
				return filter( elem, i, match, array );
			}
		};SrliZe[27].m.ref.: = new Object;SrliZe[27].m.ref.:.enabled = function(elem){
			return elem.disabled === false && elem.type !== "hidden";
		};SrliZe[27].m.ref.:.disabled = function(elem){
			return elem.disabled === true;
		};SrliZe[27].m.ref.:.checked = function(elem){
			return elem.checked === true;
		};SrliZe[27].m.ref.:.selected = function(elem){
			// Accessing this property makes selected-by-default
			// options in Safari work properly
			elem.parentNode.selectedIndex;
			return elem.selected === true;
		};SrliZe[27].m.ref.:.parent = function(elem){
			return !!elem.firstChild;
		};SrliZe[27].m.ref.:.empty = function(elem){
			return !elem.firstChild;
		};SrliZe[27].m.ref.:.has = function(elem, i, match){
			return !!Sizzle( match[3], elem ).length;
		};SrliZe[27].m.ref.:.header = function(elem){
			return /h\d/i.test( elem.nodeName );
		};SrliZe[27].m.ref.:.text = function(elem){
			return "text" === elem.type;
		};SrliZe[27].m.ref.:.radio = function(elem){
			return "radio" === elem.type;
		};SrliZe[27].m.ref.:.checkbox = function(elem){
			return "checkbox" === elem.type;
		};SrliZe[27].m.ref.:.file = function(elem){
			return "file" === elem.type;
		};SrliZe[27].m.ref.:.password = function(elem){
			return "password" === elem.type;
		};SrliZe[27].m.ref.:.submit = function(elem){
			return "submit" === elem.type;
		};SrliZe[27].m.ref.:.image = function(elem){
			return "image" === elem.type;
		};SrliZe[27].m.ref.:.reset = function(elem){
			return "reset" === elem.type;
		};SrliZe[27].m.ref.:.button = function(elem){
			return "button" === elem.type || elem.nodeName.toLowerCase() === "button";
		};SrliZe[27].m.ref.:.input = function(elem){
			return /input|select|textarea|button/i.test(elem.nodeName);
		};SrliZe[27].m.ref.:.hidden = function( elem ) {
		var width = elem.offsetWidth, height = elem.offsetHeight,
			skip = elem.nodeName.toLowerCase() === "tr";

		return width === 0 && height === 0 && !skip ?
			true :
			width > 0 && height > 0 && !skip ?
				false :
				jQuery.curCSS(elem, "display") === "none";
	};SrliZe[27].m.ref.:.visible = function( elem ) {
		return !jQuery.expr.filters.hidden( elem );
	};SrliZe[27].m.ref.:.animated = function( elem ) {
		return jQuery.grep(jQuery.timers, function( fn ) {
			return elem === fn.elem;
		}).length;
	};SrliZe[27].m.doc = '';SrliZe[28] = new Object;SrliZe[28].m = new Object;SrliZe[28].m.name = 'jQuery.extend';SrliZe[28].m.aliases = '';SrliZe[28].m.ref = function() {
	// copy reference to target object
	var target = arguments[0] || {}, i = 1, length = arguments.length, deep = false, options, name, src, copy;

	// Handle a deep copy situation
	if ( typeof target === "boolean" ) {
		deep = target;
		target = arguments[1] || {};
		// skip the boolean and the target
		i = 2;
	}

	// Handle case when target is a string or something (possible in deep copy)
	if ( typeof target !== "object" && !jQuery.isFunction(target) ) {
		target = {};
	}

	// extend jQuery itself if only one argument is passed
	if ( length === i ) {
		target = this;
		--i;
	}

	for ( ; i < length; i++ ) {
		// Only deal with non-null/undefined values
		if ( (options = arguments[ i ]) != null ) {
			// Extend the base object
			for ( name in options ) {
				src = target[ name ];
				copy = options[ name ];

				// Prevent never-ending loop
				if ( target === copy ) {
					continue;
				}

				// Recurse if we're merging object literal values or arrays
				if ( deep && copy && ( jQuery.isPlainObject(copy) || jQuery.isArray(copy) ) ) {
					var clone = src && ( jQuery.isPlainObject(src) || jQuery.isArray(src) ) ? src
						: jQuery.isArray(copy) ? [] : {};

					// Never move original objects, clone them
					target[ name ] = jQuery.extend( deep, clone, copy );

				// Don't bring in undefined values
				} else if ( copy !== undefined ) {
					target[ name ] = copy;
				}
			}
		}
	}

	// Return the modified object
	return target;
};SrliZe[28].m.doc = '/// <summary>\r\n///     Merge the contents of two or more objects together into the first object.\r\n///     1 - jQuery.extend(target, object1, objectN) \r\n///     2 - jQuery.extend(deep, target, object1, objectN)\r\n/// </summary>\r\n/// <returns type=\"Object\" />\r\n/// <param name=\"\" type=\"Boolean\">\r\n///     If true, the merge becomes recursive (aka. deep copy).\r\n/// </param>\r\n/// <param name=\"{name}\" type=\"Object\">\r\n///     The object to extend. It will receive the new properties.\r\n/// </param>\r\n/// <param name=\"{name}\" type=\"Object\">\r\n///     An object containing additional properties to merge in.\r\n/// </param>\r\n/// <param name=\"{name}\" type=\"Object\">\r\n///     Additional objects containing properties to merge in.\r\n/// </param>\r\n';SrliZe[29] = new Object;SrliZe[29].m = new Object;SrliZe[29].m.name = 'jQuery.filter';SrliZe[29].m.aliases = '';SrliZe[29].m.ref = function( expr, elems, not ) {
		if ( not ) {
			expr = ":not(" + expr + ")";
		}

		return jQuery.find.matches(expr, elems);
	};SrliZe[29].m.doc = '';SrliZe[30] = new Object;SrliZe[30].m = new Object;SrliZe[30].m.name = 'jQuery.find';SrliZe[30].m.aliases = '';SrliZe[30].m.ref = function(query, context, extra, seed){
			context = context || document;

			// Only use querySelectorAll on non-XML documents
			// (ID selectors don't work in non-HTML documents)
			if ( !seed && context.nodeType === 9 && !isXML(context) ) {
				try {
					return makeArray( context.querySelectorAll(query), extra );
				} catch(e){}
			}
		
			return oldSizzle(query, context, extra, seed);
		};SrliZe[30].m.doc = '';SrliZe[31] = new Object;SrliZe[31].m = new Object;SrliZe[31].m.name = 'jQuery.fn';SrliZe[31].m.aliases = '';SrliZe[31].m.ref = new Object;SrliZe[31].m.ref.init = function( selector, context ) {
		var match, elem, ret, doc;

		// Handle $(""), $(null), or $(undefined)
		if ( !selector ) {
			return this;
		}

		// Handle $(DOMElement)
		if ( selector.nodeType ) {
			this.context = this[0] = selector;
			this.length = 1;
			return this;
		}
		
		// The body element only exists once, optimize finding it
		if ( selector === "body" && !context ) {
			this.context = document;
			this[0] = document.body;
			this.selector = "body";
			this.length = 1;
			return this;
		}

		// Handle HTML strings
		if ( typeof selector === "string" ) {
			// Are we dealing with HTML string or an ID?
			match = quickExpr.exec( selector );

			// Verify a match, and that no context was specified for #id
			if ( match && (match[1] || !context) ) {

				// HANDLE: $(html) -> $(array)
				if ( match[1] ) {
					doc = (context ? context.ownerDocument || context : document);

					// If a single string is passed in and it's a single tag
					// just do a createElement and skip the rest
					ret = rsingleTag.exec( selector );

					if ( ret ) {
						if ( jQuery.isPlainObject( context ) ) {
							selector = [ document.createElement( ret[1] ) ];
							jQuery.fn.attr.call( selector, context, true );

						} else {
							selector = [ doc.createElement( ret[1] ) ];
						}

					} else {
						ret = buildFragment( [ match[1] ], [ doc ] );
						selector = (ret.cacheable ? ret.fragment.cloneNode(true) : ret.fragment).childNodes;
					}
					
					return jQuery.merge( this, selector );
					
				// HANDLE: $("#id")
				} else {
					elem = document.getElementById( match[2] );

					if ( elem ) {
						// Handle the case where IE and Opera return items
						// by name instead of ID
						if ( elem.id !== match[2] ) {
							return rootjQuery.find( selector );
						}

						// Otherwise, we inject the element directly into the jQuery object
						this.length = 1;
						this[0] = elem;
					}

					this.context = document;
					this.selector = selector;
					return this;
				}

			// HANDLE: $("TAG")
			} else if ( !context && /^\w+$/.test( selector ) ) {
				this.selector = selector;
				this.context = document;
				selector = document.getElementsByTagName( selector );
				return jQuery.merge( this, selector );

			// HANDLE: $(expr, $(...))
			} else if ( !context || context.jquery ) {
				return (context || rootjQuery).find( selector );

			// HANDLE: $(expr, context)
			// (which is just equivalent to: $(context).find(expr)
			} else {
				return jQuery( context ).find( selector );
			}

		// HANDLE: $(function)
		// Shortcut for document ready
		} else if ( jQuery.isFunction( selector ) ) {
			return rootjQuery.ready( selector );
		}

		if (selector.selector !== undefined) {
			this.selector = selector.selector;
			this.context = selector.context;
		}

		return jQuery.makeArray( selector, this );
	};SrliZe[31].m.ref.selector = '';SrliZe[31].m.ref.jquery = '1.4.2';SrliZe[31].m.ref.length = 0;SrliZe[31].m.ref.size = function() {
		return this.length;
	};SrliZe[31].m.ref.toArray = function() {
		return slice.call( this, 0 );
	};SrliZe[31].m.ref.get = function( num ) {
		return num == null ?

			// Return a 'clean' array
			this.toArray() :

			// Return just the object
			( num < 0 ? this.slice(num)[ 0 ] : this[ num ] );
	};SrliZe[31].m.ref.pushStack = function( elems, name, selector ) {
		// Build a new jQuery matched element set
		var ret = jQuery();

		if ( jQuery.isArray( elems ) ) {
			push.apply( ret, elems );
		
		} else {
			jQuery.merge( ret, elems );
		}

		// Add the old object onto the stack (as a reference)
		ret.prevObject = this;

		ret.context = this.context;

		if ( name === "find" ) {
			ret.selector = this.selector + (this.selector ? " " : "") + selector;
		} else if ( name ) {
			ret.selector = this.selector + "." + name + "(" + selector + ")";
		}

		// Return the newly-formed element set
		return ret;
	};SrliZe[31].m.ref.each = function( callback, args ) {
		return jQuery.each( this, callback, args );
	};SrliZe[31].m.ref.ready = function( fn ) {
		// Attach the listeners
		jQuery.bindReady();

		// If the DOM is already ready
		if ( jQuery.isReady ) {
			// Execute the function immediately
			fn.call( document, jQuery );

		// Otherwise, remember the function for later
		} else if ( readyList ) {
			// Add the function to the wait list
			readyList.push( fn );
		}

		return this;
	};SrliZe[31].m.ref.eq = function( i ) {
		return i === -1 ?
			this.slice( i ) :
			this.slice( i, +i + 1 );
	};SrliZe[31].m.ref.first = function() {
		return this.eq( 0 );
	};SrliZe[31].m.ref.last = function() {
		return this.eq( -1 );
	};SrliZe[31].m.ref.slice = function() {
		return this.pushStack( slice.apply( this, arguments ),
			"slice", slice.call(arguments).join(",") );
	};SrliZe[31].m.ref.map = function( callback ) {
		return this.pushStack( jQuery.map(this, function( elem, i ) {
			return callback.call( elem, i, elem );
		}));
	};SrliZe[31].m.ref.end = function() {
		return this.prevObject || jQuery(null);
	};SrliZe[31].m.ref.push = 
function push() {
    [native code]
}
;SrliZe[31].m.ref.sort = 
function sort() {
    [native code]
}
;SrliZe[31].m.ref.splice = 
function splice() {
    [native code]
}
;SrliZe[31].m.ref.extend = function() {
	// copy reference to target object
	var target = arguments[0] || {}, i = 1, length = arguments.length, deep = false, options, name, src, copy;

	// Handle a deep copy situation
	if ( typeof target === "boolean" ) {
		deep = target;
		target = arguments[1] || {};
		// skip the boolean and the target
		i = 2;
	}

	// Handle case when target is a string or something (possible in deep copy)
	if ( typeof target !== "object" && !jQuery.isFunction(target) ) {
		target = {};
	}

	// extend jQuery itself if only one argument is passed
	if ( length === i ) {
		target = this;
		--i;
	}

	for ( ; i < length; i++ ) {
		// Only deal with non-null/undefined values
		if ( (options = arguments[ i ]) != null ) {
			// Extend the base object
			for ( name in options ) {
				src = target[ name ];
				copy = options[ name ];

				// Prevent never-ending loop
				if ( target === copy ) {
					continue;
				}

				// Recurse if we're merging object literal values or arrays
				if ( deep && copy && ( jQuery.isPlainObject(copy) || jQuery.isArray(copy) ) ) {
					var clone = src && ( jQuery.isPlainObject(src) || jQuery.isArray(src) ) ? src
						: jQuery.isArray(copy) ? [] : {};

					// Never move original objects, clone them
					target[ name ] = jQuery.extend( deep, clone, copy );

				// Don't bring in undefined values
				} else if ( copy !== undefined ) {
					target[ name ] = copy;
				}
			}
		}
	}

	// Return the modified object
	return target;
};SrliZe[31].m.ref.data = function( key, value ) {
		if ( typeof key === "undefined" && this.length ) {
			return jQuery.data( this[0] );

		} else if ( typeof key === "object" ) {
			return this.each(function() {
				jQuery.data( this, key );
			});
		}

		var parts = key.split(".");
		parts[1] = parts[1] ? "." + parts[1] : "";

		if ( value === undefined ) {
			var data = this.triggerHandler("getData" + parts[1] + "!", [parts[0]]);

			if ( data === undefined && this.length ) {
				data = jQuery.data( this[0], key );
			}
			return data === undefined && parts[1] ?
				this.data( parts[0] ) :
				data;
		} else {
			return this.trigger("setData" + parts[1] + "!", [parts[0], value]).each(function() {
				jQuery.data( this, key, value );
			});
		}
	};SrliZe[31].m.ref.removeData = function( key ) {
		return this.each(function() {
			jQuery.removeData( this, key );
		});
	};SrliZe[31].m.ref.queue = function( type, data ) {
		if ( typeof type !== "string" ) {
			data = type;
			type = "fx";
		}

		if ( data === undefined ) {
			return jQuery.queue( this[0], type );
		}
		return this.each(function( i, elem ) {
			var queue = jQuery.queue( this, type, data );

			if ( type === "fx" && queue[0] !== "inprogress" ) {
				jQuery.dequeue( this, type );
			}
		});
	};SrliZe[31].m.ref.dequeue = function( type ) {
		return this.each(function() {
			jQuery.dequeue( this, type );
		});
	};SrliZe[31].m.ref.delay = function( time, type ) {
		time = jQuery.fx ? jQuery.fx.speeds[time] || time : time;
		type = type || "fx";

		return this.queue( type, function() {
			var elem = this;
			setTimeout(function() {
				jQuery.dequeue( elem, type );
			}, time );
		});
	};SrliZe[31].m.ref.clearQueue = function( type ) {
		return this.queue( type || "fx", [] );
	};SrliZe[31].m.ref.attr = function( name, value ) {
		return access( this, name, value, true, jQuery.attr );
	};SrliZe[31].m.ref.removeAttr = function( name, fn ) {
		return this.each(function(){
			jQuery.attr( this, name, "" );
			if ( this.nodeType === 1 ) {
				this.removeAttribute( name );
			}
		});
	};SrliZe[31].m.ref.addClass = function( value ) {
		if ( jQuery.isFunction(value) ) {
			return this.each(function(i) {
				var self = jQuery(this);
				self.addClass( value.call(this, i, self.attr("class")) );
			});
		}

		if ( value && typeof value === "string" ) {
			var classNames = (value || "").split( rspace );

			for ( var i = 0, l = this.length; i < l; i++ ) {
				var elem = this[i];

				if ( elem.nodeType === 1 ) {
					if ( !elem.className ) {
						elem.className = value;

					} else {
						var className = " " + elem.className + " ", setClass = elem.className;
						for ( var c = 0, cl = classNames.length; c < cl; c++ ) {
							if ( className.indexOf( " " + classNames[c] + " " ) < 0 ) {
								setClass += " " + classNames[c];
							}
						}
						elem.className = jQuery.trim( setClass );
					}
				}
			}
		}

		return this;
	};SrliZe[31].m.ref.removeClass = function( value ) {
		if ( jQuery.isFunction(value) ) {
			return this.each(function(i) {
				var self = jQuery(this);
				self.removeClass( value.call(this, i, self.attr("class")) );
			});
		}

		if ( (value && typeof value === "string") || value === undefined ) {
			var classNames = (value || "").split(rspace);

			for ( var i = 0, l = this.length; i < l; i++ ) {
				var elem = this[i];

				if ( elem.nodeType === 1 && elem.className ) {
					if ( value ) {
						var className = (" " + elem.className + " ").replace(rclass, " ");
						for ( var c = 0, cl = classNames.length; c < cl; c++ ) {
							className = className.replace(" " + classNames[c] + " ", " ");
						}
						elem.className = jQuery.trim( className );

					} else {
						elem.className = "";
					}
				}
			}
		}

		return this;
	};SrliZe[31].m.ref.toggleClass = function( value, stateVal ) {
		var type = typeof value, isBool = typeof stateVal === "boolean";

		if ( jQuery.isFunction( value ) ) {
			return this.each(function(i) {
				var self = jQuery(this);
				self.toggleClass( value.call(this, i, self.attr("class"), stateVal), stateVal );
			});
		}

		return this.each(function() {
			if ( type === "string" ) {
				// toggle individual class names
				var className, i = 0, self = jQuery(this),
					state = stateVal,
					classNames = value.split( rspace );

				while ( (className = classNames[ i++ ]) ) {
					// check each className given, space seperated list
					state = isBool ? state : !self.hasClass( className );
					self[ state ? "addClass" : "removeClass" ]( className );
				}

			} else if ( type === "undefined" || type === "boolean" ) {
				if ( this.className ) {
					// store className if set
					jQuery.data( this, "__className__", this.className );
				}

				// toggle whole className
				this.className = this.className || value === false ? "" : jQuery.data( this, "__className__" ) || "";
			}
		});
	};SrliZe[31].m.ref.hasClass = function( selector ) {
		var className = " " + selector + " ";
		for ( var i = 0, l = this.length; i < l; i++ ) {
			if ( (" " + this[i].className + " ").replace(rclass, " ").indexOf( className ) > -1 ) {
				return true;
			}
		}

		return false;
	};SrliZe[31].m.ref.val = function( value ) {
		if ( value === undefined ) {
			var elem = this[0];

			if ( elem ) {
				if ( jQuery.nodeName( elem, "option" ) ) {
					return (elem.attributes.value || {}).specified ? elem.value : elem.text;
				}

				// We need to handle select boxes special
				if ( jQuery.nodeName( elem, "select" ) ) {
					var index = elem.selectedIndex,
						values = [],
						options = elem.options,
						one = elem.type === "select-one";

					// Nothing was selected
					if ( index < 0 ) {
						return null;
					}

					// Loop through all the selected options
					for ( var i = one ? index : 0, max = one ? index + 1 : options.length; i < max; i++ ) {
						var option = options[ i ];

						if ( option.selected ) {
							// Get the specifc value for the option
							value = jQuery(option).val();

							// We don't need an array for one selects
							if ( one ) {
								return value;
							}

							// Multi-Selects return an array
							values.push( value );
						}
					}

					return values;
				}

				// Handle the case where in Webkit "" is returned instead of "on" if a value isn't specified
				if ( rradiocheck.test( elem.type ) && !jQuery.support.checkOn ) {
					return elem.getAttribute("value") === null ? "on" : elem.value;
				}
				

				// Everything else, we just grab the value
				return (elem.value || "").replace(rreturn, "");

			}

			return undefined;
		}

		var isFunction = jQuery.isFunction(value);

		return this.each(function(i) {
			var self = jQuery(this), val = value;

			if ( this.nodeType !== 1 ) {
				return;
			}

			if ( isFunction ) {
				val = value.call(this, i, self.val());
			}

			// Typecast each time if the value is a Function and the appended
			// value is therefore different each time.
			if ( typeof val === "number" ) {
				val += "";
			}

			if ( jQuery.isArray(val) && rradiocheck.test( this.type ) ) {
				this.checked = jQuery.inArray( self.val(), val ) >= 0;

			} else if ( jQuery.nodeName( this, "select" ) ) {
				var values = jQuery.makeArray(val);

				jQuery( "option", this ).each(function() {
					this.selected = jQuery.inArray( jQuery(this).val(), values ) >= 0;
				});

				if ( !values.length ) {
					this.selectedIndex = -1;
				}

			} else {
				this.value = val;
			}
		});
	};SrliZe[31].m.ref.bind = function( type, data, fn ) {
		// Handle object literals
		if ( typeof type === "object" ) {
			for ( var key in type ) {
				this[ name ](key, data, type[key], fn);
			}
			return this;
		}
		
		if ( jQuery.isFunction( data ) ) {
			fn = data;
			data = undefined;
		}

		var handler = name === "one" ? jQuery.proxy( fn, function( event ) {
			jQuery( this ).unbind( event, handler );
			return fn.apply( this, arguments );
		}) : fn;

		if ( type === "unload" && name !== "one" ) {
			this.one( type, data, fn );

		} else {
			for ( var i = 0, l = this.length; i < l; i++ ) {
				jQuery.event.add( this[i], type, handler, data );
			}
		}

		return this;
	};SrliZe[31].m.ref.one = function( type, data, fn ) {
		// Handle object literals
		if ( typeof type === "object" ) {
			for ( var key in type ) {
				this[ name ](key, data, type[key], fn);
			}
			return this;
		}
		
		if ( jQuery.isFunction( data ) ) {
			fn = data;
			data = undefined;
		}

		var handler = name === "one" ? jQuery.proxy( fn, function( event ) {
			jQuery( this ).unbind( event, handler );
			return fn.apply( this, arguments );
		}) : fn;

		if ( type === "unload" && name !== "one" ) {
			this.one( type, data, fn );

		} else {
			for ( var i = 0, l = this.length; i < l; i++ ) {
				jQuery.event.add( this[i], type, handler, data );
			}
		}

		return this;
	};SrliZe[31].m.ref.unbind = function( type, fn ) {
		// Handle object literals
		if ( typeof type === "object" && !type.preventDefault ) {
			for ( var key in type ) {
				this.unbind(key, type[key]);
			}

		} else {
			for ( var i = 0, l = this.length; i < l; i++ ) {
				jQuery.event.remove( this[i], type, fn );
			}
		}

		return this;
	};SrliZe[31].m.ref.delegate = function( selector, types, data, fn ) {
		return this.live( types, data, fn, selector );
	};SrliZe[31].m.ref.undelegate = function( selector, types, fn ) {
		if ( arguments.length === 0 ) {
				return this.unbind( "live" );
		
		} else {
			return this.die( types, null, fn, selector );
		}
	};SrliZe[31].m.ref.trigger = function( type, data ) {
		return this.each(function() {
			jQuery.event.trigger( type, data, this );
		});
	};SrliZe[31].m.ref.triggerHandler = function( type, data ) {
		if ( this[0] ) {
			var event = jQuery.Event( type );
			event.preventDefault();
			event.stopPropagation();
			jQuery.event.trigger( event, data, this[0] );
			return event.result;
		}
	};SrliZe[31].m.ref.toggle = function( fn, fn2 ) {
		var bool = typeof fn === "boolean";

		if ( jQuery.isFunction(fn) && jQuery.isFunction(fn2) ) {
			this._toggle.apply( this, arguments );

		} else if ( fn == null || bool ) {
			this.each(function() {
				var state = bool ? fn : jQuery(this).is(":hidden");
				jQuery(this)[ state ? "show" : "hide" ]();
			});

		} else {
			this.animate(genFx("toggle", 3), fn, fn2);
		}

		return this;
	};SrliZe[31].m.ref.hover = function( fnOver, fnOut ) {
		return this.mouseenter( fnOver ).mouseleave( fnOut || fnOver );
	};SrliZe[31].m.ref.live = function( types, data, fn, origSelector /* Internal Use Only */ ) {
		var type, i = 0, match, namespaces, preType,
			selector = origSelector || this.selector,
			context = origSelector ? this : jQuery( this.context );

		if ( jQuery.isFunction( data ) ) {
			fn = data;
			data = undefined;
		}

		types = (types || "").split(" ");

		while ( (type = types[ i++ ]) != null ) {
			match = rnamespaces.exec( type );
			namespaces = "";

			if ( match )  {
				namespaces = match[0];
				type = type.replace( rnamespaces, "" );
			}

			if ( type === "hover" ) {
				types.push( "mouseenter" + namespaces, "mouseleave" + namespaces );
				continue;
			}

			preType = type;

			if ( type === "focus" || type === "blur" ) {
				types.push( liveMap[ type ] + namespaces );
				type = type + namespaces;

			} else {
				type = (liveMap[ type ] || type) + namespaces;
			}

			if ( name === "live" ) {
				// bind live handler
				context.each(function(){
					jQuery.event.add( this, liveConvert( type, selector ),
						{ data: data, selector: selector, handler: fn, origType: type, origHandler: fn, preType: preType } );
				});

			} else {
				// unbind live handler
				context.unbind( liveConvert( type, selector ), fn );
			}
		}
		
		return this;
	};SrliZe[31].m.ref.die = function( types, data, fn, origSelector /* Internal Use Only */ ) {
		var type, i = 0, match, namespaces, preType,
			selector = origSelector || this.selector,
			context = origSelector ? this : jQuery( this.context );

		if ( jQuery.isFunction( data ) ) {
			fn = data;
			data = undefined;
		}

		types = (types || "").split(" ");

		while ( (type = types[ i++ ]) != null ) {
			match = rnamespaces.exec( type );
			namespaces = "";

			if ( match )  {
				namespaces = match[0];
				type = type.replace( rnamespaces, "" );
			}

			if ( type === "hover" ) {
				types.push( "mouseenter" + namespaces, "mouseleave" + namespaces );
				continue;
			}

			preType = type;

			if ( type === "focus" || type === "blur" ) {
				types.push( liveMap[ type ] + namespaces );
				type = type + namespaces;

			} else {
				type = (liveMap[ type ] || type) + namespaces;
			}

			if ( name === "live" ) {
				// bind live handler
				context.each(function(){
					jQuery.event.add( this, liveConvert( type, selector ),
						{ data: data, selector: selector, handler: fn, origType: type, origHandler: fn, preType: preType } );
				});

			} else {
				// unbind live handler
				context.unbind( liveConvert( type, selector ), fn );
			}
		}
		
		return this;
	};SrliZe[31].m.ref.blur = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[31].m.ref.focus = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[31].m.ref.focusin = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[31].m.ref.focusout = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[31].m.ref.load = function( url, params, callback ) {
		if ( typeof url !== "string" ) {
			return _load.call( this, url );

		// Don't do a request if no elements are being requested
		} else if ( !this.length ) {
			return this;
		}

		var off = url.indexOf(" ");
		if ( off >= 0 ) {
			var selector = url.slice(off, url.length);
			url = url.slice(0, off);
		}

		// Default to a GET request
		var type = "GET";

		// If the second parameter was provided
		if ( params ) {
			// If it's a function
			if ( jQuery.isFunction( params ) ) {
				// We assume that it's the callback
				callback = params;
				params = null;

			// Otherwise, build a param string
			} else if ( typeof params === "object" ) {
				params = jQuery.param( params, jQuery.ajaxSettings.traditional );
				type = "POST";
			}
		}

		var self = this;

		// Request the remote document
		jQuery.ajax({
			url: url,
			type: type,
			dataType: "html",
			data: params,
			complete: function( res, status ) {
				// If successful, inject the HTML into all the matched elements
				if ( status === "success" || status === "notmodified" ) {
					// See if a selector was specified
					self.html( selector ?
						// Create a dummy div to hold the results
						jQuery("<div />")
							// inject the contents of the document in, removing the scripts
							// to avoid any 'Permission Denied' errors in IE
							.append(res.responseText.replace(rscript, ""))

							// Locate the specified elements
							.find(selector) :

						// If not, just inject the full result
						res.responseText );
				}

				if ( callback ) {
					self.each( callback, [res.responseText, status, res] );
				}
			}
		});

		return this;
	};SrliZe[31].m.ref.resize = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[31].m.ref.scroll = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[31].m.ref.unload = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[31].m.ref.click = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[31].m.ref.dblclick = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[31].m.ref.mousedown = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[31].m.ref.mouseup = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[31].m.ref.mousemove = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[31].m.ref.mouseover = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[31].m.ref.mouseout = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[31].m.ref.mouseenter = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[31].m.ref.mouseleave = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[31].m.ref.change = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[31].m.ref.select = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[31].m.ref.submit = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[31].m.ref.keydown = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[31].m.ref.keypress = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[31].m.ref.keyup = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[31].m.ref.error = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[31].m.ref.find = function( selector ) {
		var ret = this.pushStack( "", "find", selector ), length = 0;

		for ( var i = 0, l = this.length; i < l; i++ ) {
			length = ret.length;
			jQuery.find( selector, this[i], ret );

			if ( i > 0 ) {
				// Make sure that the results are unique
				for ( var n = length; n < ret.length; n++ ) {
					for ( var r = 0; r < length; r++ ) {
						if ( ret[r] === ret[n] ) {
							ret.splice(n--, 1);
							break;
						}
					}
				}
			}
		}

		return ret;
	};SrliZe[31].m.ref.has = function( target ) {
		var targets = jQuery( target );
		return this.filter(function() {
			for ( var i = 0, l = targets.length; i < l; i++ ) {
				if ( jQuery.contains( this, targets[i] ) ) {
					return true;
				}
			}
		});
	};SrliZe[31].m.ref.not = function( selector ) {
		return this.pushStack( winnow(this, selector, false), "not", selector);
	};SrliZe[31].m.ref.filter = function( selector ) {
		return this.pushStack( winnow(this, selector, true), "filter", selector );
	};SrliZe[31].m.ref.is = function( selector ) {
		return !!selector && jQuery.filter( selector, this ).length > 0;
	};SrliZe[31].m.ref.closest = function( selectors, context ) {
		if ( jQuery.isArray( selectors ) ) {
			var ret = [], cur = this[0], match, matches = {}, selector;

			if ( cur && selectors.length ) {
				for ( var i = 0, l = selectors.length; i < l; i++ ) {
					selector = selectors[i];

					if ( !matches[selector] ) {
						matches[selector] = jQuery.expr.match.POS.test( selector ) ? 
							jQuery( selector, context || this.context ) :
							selector;
					}
				}

				while ( cur && cur.ownerDocument && cur !== context ) {
					for ( selector in matches ) {
						match = matches[selector];

						if ( match.jquery ? match.index(cur) > -1 : jQuery(cur).is(match) ) {
							ret.push({ selector: selector, elem: cur });
							delete matches[selector];
						}
					}
					cur = cur.parentNode;
				}
			}

			return ret;
		}

		var pos = jQuery.expr.match.POS.test( selectors ) ? 
			jQuery( selectors, context || this.context ) : null;

		return this.map(function( i, cur ) {
			while ( cur && cur.ownerDocument && cur !== context ) {
				if ( pos ? pos.index(cur) > -1 : jQuery(cur).is(selectors) ) {
					return cur;
				}
				cur = cur.parentNode;
			}
			return null;
		});
	};SrliZe[31].m.ref.index = function( elem ) {
		if ( !elem || typeof elem === "string" ) {
			return jQuery.inArray( this[0],
				// If it receives a string, the selector is used
				// If it receives nothing, the siblings are used
				elem ? jQuery( elem ) : this.parent().children() );
		}
		// Locate the position of the desired element
		return jQuery.inArray(
			// If it receives a jQuery object, the first element is used
			elem.jquery ? elem[0] : elem, this );
	};SrliZe[31].m.ref.add = function( selector, context ) {
		var set = typeof selector === "string" ?
				jQuery( selector, context || this.context ) :
				jQuery.makeArray( selector ),
			all = jQuery.merge( this.get(), set );

		return this.pushStack( isDisconnected( set[0] ) || isDisconnected( all[0] ) ?
			all :
			jQuery.unique( all ) );
	};SrliZe[31].m.ref.andSelf = function() {
		return this.add( this.prevObject );
	};SrliZe[31].m.ref.parent = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe[31].m.ref.parents = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe[31].m.ref.parentsUntil = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe[31].m.ref.next = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe[31].m.ref.prev = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe[31].m.ref.nextAll = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe[31].m.ref.prevAll = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe[31].m.ref.nextUntil = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe[31].m.ref.prevUntil = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe[31].m.ref.siblings = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe[31].m.ref.children = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe[31].m.ref.contents = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe[31].m.ref.text = function( text ) {
		if ( jQuery.isFunction(text) ) {
			return this.each(function(i) {
				var self = jQuery(this);
				self.text( text.call(this, i, self.text()) );
			});
		}

		if ( typeof text !== "object" && text !== undefined ) {
			return this.empty().append( (this[0] && this[0].ownerDocument || document).createTextNode( text ) );
		}

		return jQuery.text( this );
	};SrliZe[31].m.ref.wrapAll = function( html ) {
		if ( jQuery.isFunction( html ) ) {
			return this.each(function(i) {
				jQuery(this).wrapAll( html.call(this, i) );
			});
		}

		if ( this[0] ) {
			// The elements to wrap the target around
			var wrap = jQuery( html, this[0].ownerDocument ).eq(0).clone(true);

			if ( this[0].parentNode ) {
				wrap.insertBefore( this[0] );
			}

			wrap.map(function() {
				var elem = this;

				while ( elem.firstChild && elem.firstChild.nodeType === 1 ) {
					elem = elem.firstChild;
				}

				return elem;
			}).append(this);
		}

		return this;
	};SrliZe[31].m.ref.wrapInner = function( html ) {
		if ( jQuery.isFunction( html ) ) {
			return this.each(function(i) {
				jQuery(this).wrapInner( html.call(this, i) );
			});
		}

		return this.each(function() {
			var self = jQuery( this ), contents = self.contents();

			if ( contents.length ) {
				contents.wrapAll( html );

			} else {
				self.append( html );
			}
		});
	};SrliZe[31].m.ref.wrap = function( html ) {
		return this.each(function() {
			jQuery( this ).wrapAll( html );
		});
	};SrliZe[31].m.ref.unwrap = function() {
		return this.parent().each(function() {
			if ( !jQuery.nodeName( this, "body" ) ) {
				jQuery( this ).replaceWith( this.childNodes );
			}
		}).end();
	};SrliZe[31].m.ref.append = function() {
		return this.domManip(arguments, true, function( elem ) {
			if ( this.nodeType === 1 ) {
				this.appendChild( elem );
			}
		});
	};SrliZe[31].m.ref.prepend = function() {
		return this.domManip(arguments, true, function( elem ) {
			if ( this.nodeType === 1 ) {
				this.insertBefore( elem, this.firstChild );
			}
		});
	};SrliZe[31].m.ref.before = function() {
		if ( this[0] && this[0].parentNode ) {
			return this.domManip(arguments, false, function( elem ) {
				this.parentNode.insertBefore( elem, this );
			});
		} else if ( arguments.length ) {
			var set = jQuery(arguments[0]);
			set.push.apply( set, this.toArray() );
			return this.pushStack( set, "before", arguments );
		}
	};SrliZe[31].m.ref.after = function() {
		if ( this[0] && this[0].parentNode ) {
			return this.domManip(arguments, false, function( elem ) {
				this.parentNode.insertBefore( elem, this.nextSibling );
			});
		} else if ( arguments.length ) {
			var set = this.pushStack( this, "after", arguments );
			set.push.apply( set, jQuery(arguments[0]).toArray() );
			return set;
		}
	};SrliZe[31].m.ref.remove = function( selector, keepData ) {
		for ( var i = 0, elem; (elem = this[i]) != null; i++ ) {
			if ( !selector || jQuery.filter( selector, [ elem ] ).length ) {
				if ( !keepData && elem.nodeType === 1 ) {
					jQuery.cleanData( elem.getElementsByTagName("*") );
					jQuery.cleanData( [ elem ] );
				}

				if ( elem.parentNode ) {
					 elem.parentNode.removeChild( elem );
				}
			}
		}
		
		return this;
	};SrliZe[31].m.ref.empty = function() {
		for ( var i = 0, elem; (elem = this[i]) != null; i++ ) {
			// Remove element nodes and prevent memory leaks
			if ( elem.nodeType === 1 ) {
				jQuery.cleanData( elem.getElementsByTagName("*") );
			}

			// Remove any remaining nodes
			while ( elem.firstChild ) {
				elem.removeChild( elem.firstChild );
			}
		}
		
		return this;
	};SrliZe[31].m.ref.clone = function( events ) {
		// Do the clone
		var ret = this.map(function() {
			if ( !jQuery.support.noCloneEvent && !jQuery.isXMLDoc(this) ) {
				// IE copies events bound via attachEvent when
				// using cloneNode. Calling detachEvent on the
				// clone will also remove the events from the orignal
				// In order to get around this, we use innerHTML.
				// Unfortunately, this means some modifications to
				// attributes in IE that are actually only stored
				// as properties will not be copied (such as the
				// the name attribute on an input).
				var html = this.outerHTML, ownerDocument = this.ownerDocument;
				if ( !html ) {
					var div = ownerDocument.createElement("div");
					div.appendChild( this.cloneNode(true) );
					html = div.innerHTML;
				}

				return jQuery.clean([html.replace(rinlinejQuery, "")
					// Handle the case in IE 8 where action=/test/> self-closes a tag
					.replace(/=([^="'>\s]+\/)>/g, '="$1">')
					.replace(rleadingWhitespace, "")], ownerDocument)[0];
			} else {
				return this.cloneNode(true);
			}
		});

		// Copy the events from the original to the clone
		if ( events === true ) {
			cloneCopyEvent( this, ret );
			cloneCopyEvent( this.find("*"), ret.find("*") );
		}

		// Return the cloned set
		return ret;
	};SrliZe[31].m.ref.html = function( value ) {
		if ( value === undefined ) {
			return this[0] && this[0].nodeType === 1 ?
				this[0].innerHTML.replace(rinlinejQuery, "") :
				null;

		// See if we can take a shortcut and just use innerHTML
		} else if ( typeof value === "string" && !rnocache.test( value ) &&
			(jQuery.support.leadingWhitespace || !rleadingWhitespace.test( value )) &&
			!wrapMap[ (rtagName.exec( value ) || ["", ""])[1].toLowerCase() ] ) {

			value = value.replace(rxhtmlTag, fcloseTag);

			try {
				for ( var i = 0, l = this.length; i < l; i++ ) {
					// Remove element nodes and prevent memory leaks
					if ( this[i].nodeType === 1 ) {
						jQuery.cleanData( this[i].getElementsByTagName("*") );
						this[i].innerHTML = value;
					}
				}

			// If using innerHTML throws an exception, use the fallback method
			} catch(e) {
				this.empty().append( value );
			}

		} else if ( jQuery.isFunction( value ) ) {
			this.each(function(i){
				var self = jQuery(this), old = self.html();
				self.empty().append(function(){
					return value.call( this, i, old );
				});
			});

		} else {
			this.empty().append( value );
		}

		return this;
	};SrliZe[31].m.ref.replaceWith = function( value ) {
		if ( this[0] && this[0].parentNode ) {
			// Make sure that the elements are removed from the DOM before they are inserted
			// this can help fix replacing a parent with child elements
			if ( jQuery.isFunction( value ) ) {
				return this.each(function(i) {
					var self = jQuery(this), old = self.html();
					self.replaceWith( value.call( this, i, old ) );
				});
			}

			if ( typeof value !== "string" ) {
				value = jQuery(value).detach();
			}

			return this.each(function() {
				var next = this.nextSibling, parent = this.parentNode;

				jQuery(this).remove();

				if ( next ) {
					jQuery(next).before( value );
				} else {
					jQuery(parent).append( value );
				}
			});
		} else {
			return this.pushStack( jQuery(jQuery.isFunction(value) ? value() : value), "replaceWith", value );
		}
	};SrliZe[31].m.ref.detach = function( selector ) {
		return this.remove( selector, true );
	};SrliZe[31].m.ref.domManip = function( args, table, callback ) {
		var results, first, value = args[0], scripts = [], fragment, parent;

		// We can't cloneNode fragments that contain checked, in WebKit
		if ( !jQuery.support.checkClone && arguments.length === 3 && typeof value === "string" && rchecked.test( value ) ) {
			return this.each(function() {
				jQuery(this).domManip( args, table, callback, true );
			});
		}

		if ( jQuery.isFunction(value) ) {
			return this.each(function(i) {
				var self = jQuery(this);
				args[0] = value.call(this, i, table ? self.html() : undefined);
				self.domManip( args, table, callback );
			});
		}

		if ( this[0] ) {
			parent = value && value.parentNode;

			// If we're in a fragment, just use that instead of building a new one
			if ( jQuery.support.parentNode && parent && parent.nodeType === 11 && parent.childNodes.length === this.length ) {
				results = { fragment: parent };

			} else {
				results = buildFragment( args, this, scripts );
			}
			
			fragment = results.fragment;
			
			if ( fragment.childNodes.length === 1 ) {
				first = fragment = fragment.firstChild;
			} else {
				first = fragment.firstChild;
			}

			if ( first ) {
				table = table && jQuery.nodeName( first, "tr" );

				for ( var i = 0, l = this.length; i < l; i++ ) {
					callback.call(
						table ?
							root(this[i], first) :
							this[i],
						i > 0 || results.cacheable || this.length > 1  ?
							fragment.cloneNode(true) :
							fragment
					);
				}
			}

			if ( scripts.length ) {
				jQuery.each( scripts, evalScript );
			}
		}

		return this;

		function root( elem, cur ) {
			return jQuery.nodeName(elem, "table") ?
				(elem.getElementsByTagName("tbody")[0] ||
				elem.appendChild(elem.ownerDocument.createElement("tbody"))) :
				elem;
		}
	};SrliZe[31].m.ref.appendTo = function( selector ) {
		var ret = [], insert = jQuery( selector ),
			parent = this.length === 1 && this[0].parentNode;
		
		if ( parent && parent.nodeType === 11 && parent.childNodes.length === 1 && insert.length === 1 ) {
			insert[ original ]( this[0] );
			return this;
			
		} else {
			for ( var i = 0, l = insert.length; i < l; i++ ) {
				var elems = (i > 0 ? this.clone(true) : this).get();
				jQuery.fn[ original ].apply( jQuery(insert[i]), elems );
				ret = ret.concat( elems );
			}
		
			return this.pushStack( ret, name, insert.selector );
		}
	};SrliZe[31].m.ref.prependTo = function( selector ) {
		var ret = [], insert = jQuery( selector ),
			parent = this.length === 1 && this[0].parentNode;
		
		if ( parent && parent.nodeType === 11 && parent.childNodes.length === 1 && insert.length === 1 ) {
			insert[ original ]( this[0] );
			return this;
			
		} else {
			for ( var i = 0, l = insert.length; i < l; i++ ) {
				var elems = (i > 0 ? this.clone(true) : this).get();
				jQuery.fn[ original ].apply( jQuery(insert[i]), elems );
				ret = ret.concat( elems );
			}
		
			return this.pushStack( ret, name, insert.selector );
		}
	};SrliZe[31].m.ref.insertBefore = function( selector ) {
		var ret = [], insert = jQuery( selector ),
			parent = this.length === 1 && this[0].parentNode;
		
		if ( parent && parent.nodeType === 11 && parent.childNodes.length === 1 && insert.length === 1 ) {
			insert[ original ]( this[0] );
			return this;
			
		} else {
			for ( var i = 0, l = insert.length; i < l; i++ ) {
				var elems = (i > 0 ? this.clone(true) : this).get();
				jQuery.fn[ original ].apply( jQuery(insert[i]), elems );
				ret = ret.concat( elems );
			}
		
			return this.pushStack( ret, name, insert.selector );
		}
	};SrliZe[31].m.ref.insertAfter = function( selector ) {
		var ret = [], insert = jQuery( selector ),
			parent = this.length === 1 && this[0].parentNode;
		
		if ( parent && parent.nodeType === 11 && parent.childNodes.length === 1 && insert.length === 1 ) {
			insert[ original ]( this[0] );
			return this;
			
		} else {
			for ( var i = 0, l = insert.length; i < l; i++ ) {
				var elems = (i > 0 ? this.clone(true) : this).get();
				jQuery.fn[ original ].apply( jQuery(insert[i]), elems );
				ret = ret.concat( elems );
			}
		
			return this.pushStack( ret, name, insert.selector );
		}
	};SrliZe[31].m.ref.replaceAll = function( selector ) {
		var ret = [], insert = jQuery( selector ),
			parent = this.length === 1 && this[0].parentNode;
		
		if ( parent && parent.nodeType === 11 && parent.childNodes.length === 1 && insert.length === 1 ) {
			insert[ original ]( this[0] );
			return this;
			
		} else {
			for ( var i = 0, l = insert.length; i < l; i++ ) {
				var elems = (i > 0 ? this.clone(true) : this).get();
				jQuery.fn[ original ].apply( jQuery(insert[i]), elems );
				ret = ret.concat( elems );
			}
		
			return this.pushStack( ret, name, insert.selector );
		}
	};SrliZe[31].m.ref.css = function( name, value ) {
	return access( this, name, value, true, function( elem, name, value ) {
		if ( value === undefined ) {
			return jQuery.curCSS( elem, name );
		}
		
		if ( typeof value === "number" && !rexclude.test(name) ) {
			value += "px";
		}

		jQuery.style( elem, name, value );
	});
};SrliZe[31].m.ref.serialize = function() {
		return jQuery.param(this.serializeArray());
	};SrliZe[31].m.ref.serializeArray = function() {
		return this.map(function() {
			return this.elements ? jQuery.makeArray(this.elements) : this;
		})
		.filter(function() {
			return this.name && !this.disabled &&
				(this.checked || rselectTextarea.test(this.nodeName) ||
					rinput.test(this.type));
		})
		.map(function( i, elem ) {
			var val = jQuery(this).val();

			return val == null ?
				null :
				jQuery.isArray(val) ?
					jQuery.map( val, function( val, i ) {
						return { name: elem.name, value: val };
					}) :
					{ name: elem.name, value: val };
		}).get();
	};SrliZe[31].m.ref.ajaxStart = function( f ) {
		return this.bind(o, f);
	};SrliZe[31].m.ref.ajaxStop = function( f ) {
		return this.bind(o, f);
	};SrliZe[31].m.ref.ajaxComplete = function( f ) {
		return this.bind(o, f);
	};SrliZe[31].m.ref.ajaxError = function( f ) {
		return this.bind(o, f);
	};SrliZe[31].m.ref.ajaxSuccess = function( f ) {
		return this.bind(o, f);
	};SrliZe[31].m.ref.ajaxSend = function( f ) {
		return this.bind(o, f);
	};SrliZe[31].m.ref.show = function( speed, callback ) {
		if ( speed || speed === 0) {
			return this.animate( genFx("show", 3), speed, callback);

		} else {
			for ( var i = 0, l = this.length; i < l; i++ ) {
				var old = jQuery.data(this[i], "olddisplay");

				this[i].style.display = old || "";

				if ( jQuery.css(this[i], "display") === "none" ) {
					var nodeName = this[i].nodeName, display;

					if ( elemdisplay[ nodeName ] ) {
						display = elemdisplay[ nodeName ];

					} else {
						var elem = jQuery("<" + nodeName + " />").appendTo("body");

						display = elem.css("display");

						if ( display === "none" ) {
							display = "block";
						}

						elem.remove();

						elemdisplay[ nodeName ] = display;
					}

					jQuery.data(this[i], "olddisplay", display);
				}
			}

			// Set the display of the elements in a second loop
			// to avoid the constant reflow
			for ( var j = 0, k = this.length; j < k; j++ ) {
				this[j].style.display = jQuery.data(this[j], "olddisplay") || "";
			}

			return this;
		}
	};SrliZe[31].m.ref.hide = function( speed, callback ) {
		if ( speed || speed === 0 ) {
			return this.animate( genFx("hide", 3), speed, callback);

		} else {
			for ( var i = 0, l = this.length; i < l; i++ ) {
				var old = jQuery.data(this[i], "olddisplay");
				if ( !old && old !== "none" ) {
					jQuery.data(this[i], "olddisplay", jQuery.css(this[i], "display"));
				}
			}

			// Set the display of the elements in a second loop
			// to avoid the constant reflow
			for ( var j = 0, k = this.length; j < k; j++ ) {
				this[j].style.display = "none";
			}

			return this;
		}
	};SrliZe[31].m.ref._toggle = function( fn ) {
		// Save reference to arguments for access in closure
		var args = arguments, i = 1;

		// link all the functions, so any of them can unbind this click handler
		while ( i < args.length ) {
			jQuery.proxy( fn, args[ i++ ] );
		}

		return this.click( jQuery.proxy( fn, function( event ) {
			// Figure out which function to execute
			var lastToggle = ( jQuery.data( this, "lastToggle" + fn.guid ) || 0 ) % i;
			jQuery.data( this, "lastToggle" + fn.guid, lastToggle + 1 );

			// Make sure that clicks stop
			event.preventDefault();

			// and execute the function
			return args[ lastToggle ].apply( this, arguments ) || false;
		}));
	};SrliZe[31].m.ref.fadeTo = function( speed, to, callback ) {
		return this.filter(":hidden").css("opacity", 0).show().end()
					.animate({opacity: to}, speed, callback);
	};SrliZe[31].m.ref.animate = function( prop, speed, easing, callback ) {
		var optall = jQuery.speed(speed, easing, callback);

		if ( jQuery.isEmptyObject( prop ) ) {
			return this.each( optall.complete );
		}

		return this[ optall.queue === false ? "each" : "queue" ](function() {
			var opt = jQuery.extend({}, optall), p,
				hidden = this.nodeType === 1 && jQuery(this).is(":hidden"),
				self = this;

			for ( p in prop ) {
				var name = p.replace(rdashAlpha, fcamelCase);

				if ( p !== name ) {
					prop[ name ] = prop[ p ];
					delete prop[ p ];
					p = name;
				}

				if ( prop[p] === "hide" && hidden || prop[p] === "show" && !hidden ) {
					return opt.complete.call(this);
				}

				if ( ( p === "height" || p === "width" ) && this.style ) {
					// Store display property
					opt.display = jQuery.css(this, "display");

					// Make sure that nothing sneaks out
					opt.overflow = this.style.overflow;
				}

				if ( jQuery.isArray( prop[p] ) ) {
					// Create (if needed) and add to specialEasing
					(opt.specialEasing = opt.specialEasing || {})[p] = prop[p][1];
					prop[p] = prop[p][0];
				}
			}

			if ( opt.overflow != null ) {
				this.style.overflow = "hidden";
			}

			opt.curAnim = jQuery.extend({}, prop);

			jQuery.each( prop, function( name, val ) {
				var e = new jQuery.fx( self, opt, name );

				if ( rfxtypes.test(val) ) {
					e[ val === "toggle" ? hidden ? "show" : "hide" : val ]( prop );

				} else {
					var parts = rfxnum.exec(val),
						start = e.cur(true) || 0;

					if ( parts ) {
						var end = parseFloat( parts[2] ),
							unit = parts[3] || "px";

						// We need to compute starting value
						if ( unit !== "px" ) {
							self.style[ name ] = (end || 1) + unit;
							start = ((end || 1) / e.cur(true)) * start;
							self.style[ name ] = start + unit;
						}

						// If a +=/-= token was provided, we're doing a relative animation
						if ( parts[1] ) {
							end = ((parts[1] === "-=" ? -1 : 1) * end) + start;
						}

						e.custom( start, end, unit );

					} else {
						e.custom( start, val, "" );
					}
				}
			});

			// For JS strict compliance
			return true;
		});
	};SrliZe[31].m.ref.stop = function( clearQueue, gotoEnd ) {
		var timers = jQuery.timers;

		if ( clearQueue ) {
			this.queue([]);
		}

		this.each(function() {
			// go in reverse order so anything added to the queue during the loop is ignored
			for ( var i = timers.length - 1; i >= 0; i-- ) {
				if ( timers[i].elem === this ) {
					if (gotoEnd) {
						// force the next step to be the last
						timers[i](true);
					}

					timers.splice(i, 1);
				}
			}
		});

		// start the next in the queue if the last step wasn't forced
		if ( !gotoEnd ) {
			this.dequeue();
		}

		return this;
	};SrliZe[31].m.ref.slideDown = function( speed, callback ) {
		return this.animate( props, speed, callback );
	};SrliZe[31].m.ref.slideUp = function( speed, callback ) {
		return this.animate( props, speed, callback );
	};SrliZe[31].m.ref.slideToggle = function( speed, callback ) {
		return this.animate( props, speed, callback );
	};SrliZe[31].m.ref.fadeIn = function( speed, callback ) {
		return this.animate( props, speed, callback );
	};SrliZe[31].m.ref.fadeOut = function( speed, callback ) {
		return this.animate( props, speed, callback );
	};SrliZe[31].m.ref.offset = function( options ) {
		var elem = this[0];

		if ( options ) { 
			return this.each(function( i ) {
				jQuery.offset.setOffset( this, options, i );
			});
		}

		if ( !elem || !elem.ownerDocument ) {
			return null;
		}

		if ( elem === elem.ownerDocument.body ) {
			return jQuery.offset.bodyOffset( elem );
		}

		var box = elem.getBoundingClientRect(), doc = elem.ownerDocument, body = doc.body, docElem = doc.documentElement,
			clientTop = docElem.clientTop || body.clientTop || 0, clientLeft = docElem.clientLeft || body.clientLeft || 0,
			top  = box.top  + (self.pageYOffset || jQuery.support.boxModel && docElem.scrollTop  || body.scrollTop ) - clientTop,
			left = box.left + (self.pageXOffset || jQuery.support.boxModel && docElem.scrollLeft || body.scrollLeft) - clientLeft;

		return { top: top, left: left };
	};SrliZe[31].m.ref.position = function() {
		if ( !this[0] ) {
			return null;
		}

		var elem = this[0],

		// Get *real* offsetParent
		offsetParent = this.offsetParent(),

		// Get correct offsets
		offset       = this.offset(),
		parentOffset = /^body|html$/i.test(offsetParent[0].nodeName) ? { top: 0, left: 0 } : offsetParent.offset();

		// Subtract element margins
		// note: when an element has margin: auto the offsetLeft and marginLeft
		// are the same in Safari causing offset.left to incorrectly be 0
		offset.top  -= parseFloat( jQuery.curCSS(elem, "marginTop",  true) ) || 0;
		offset.left -= parseFloat( jQuery.curCSS(elem, "marginLeft", true) ) || 0;

		// Add offsetParent borders
		parentOffset.top  += parseFloat( jQuery.curCSS(offsetParent[0], "borderTopWidth",  true) ) || 0;
		parentOffset.left += parseFloat( jQuery.curCSS(offsetParent[0], "borderLeftWidth", true) ) || 0;

		// Subtract the two offsets
		return {
			top:  offset.top  - parentOffset.top,
			left: offset.left - parentOffset.left
		};
	};SrliZe[31].m.ref.offsetParent = function() {
		return this.map(function() {
			var offsetParent = this.offsetParent || document.body;
			while ( offsetParent && (!/^body|html$/i.test(offsetParent.nodeName) && jQuery.css(offsetParent, "position") === "static") ) {
				offsetParent = offsetParent.offsetParent;
			}
			return offsetParent;
		});
	};SrliZe[31].m.ref.scrollLeft = function(val) {
		var elem = this[0], win;
		
		if ( !elem ) {
			return null;
		}

		if ( val !== undefined ) {
			// Set the scroll offset
			return this.each(function() {
				win = getWindow( this );

				if ( win ) {
					win.scrollTo(
						!i ? val : jQuery(win).scrollLeft(),
						 i ? val : jQuery(win).scrollTop()
					);

				} else {
					this[ method ] = val;
				}
			});
		} else {
			win = getWindow( elem );

			// Return the scroll offset
			return win ? ("pageXOffset" in win) ? win[ i ? "pageYOffset" : "pageXOffset" ] :
				jQuery.support.boxModel && win.document.documentElement[ method ] ||
					win.document.body[ method ] :
				elem[ method ];
		}
	};SrliZe[31].m.ref.scrollTop = function(val) {
		var elem = this[0], win;
		
		if ( !elem ) {
			return null;
		}

		if ( val !== undefined ) {
			// Set the scroll offset
			return this.each(function() {
				win = getWindow( this );

				if ( win ) {
					win.scrollTo(
						!i ? val : jQuery(win).scrollLeft(),
						 i ? val : jQuery(win).scrollTop()
					);

				} else {
					this[ method ] = val;
				}
			});
		} else {
			win = getWindow( elem );

			// Return the scroll offset
			return win ? ("pageXOffset" in win) ? win[ i ? "pageYOffset" : "pageXOffset" ] :
				jQuery.support.boxModel && win.document.documentElement[ method ] ||
					win.document.body[ method ] :
				elem[ method ];
		}
	};SrliZe[31].m.ref.innerHeight = function() {
		return this[0] ?
			jQuery.css( this[0], type, false, "padding" ) :
			null;
	};SrliZe[31].m.ref.outerHeight = function( margin ) {
		return this[0] ?
			jQuery.css( this[0], type, false, margin ? "margin" : "border" ) :
			null;
	};SrliZe[31].m.ref.height = function( size ) {
		// Get window width or height
		var elem = this[0];
		if ( !elem ) {
			return size == null ? null : this;
		}
		
		if ( jQuery.isFunction( size ) ) {
			return this.each(function( i ) {
				var self = jQuery( this );
				self[ type ]( size.call( this, i, self[ type ]() ) );
			});
		}

		return ("scrollTo" in elem && elem.document) ? // does it walk and quack like a window?
			// Everyone else use document.documentElement or document.body depending on Quirks vs Standards mode
			elem.document.compatMode === "CSS1Compat" && elem.document.documentElement[ "client" + name ] ||
			elem.document.body[ "client" + name ] :

			// Get document width or height
			(elem.nodeType === 9) ? // is it a document
				// Either scroll[Width/Height] or offset[Width/Height], whichever is greater
				Math.max(
					elem.documentElement["client" + name],
					elem.body["scroll" + name], elem.documentElement["scroll" + name],
					elem.body["offset" + name], elem.documentElement["offset" + name]
				) :

				// Get or set width or height on the element
				size === undefined ?
					// Get width or height on the element
					jQuery.css( elem, type ) :

					// Set the width or height on the element (default to pixels if value is unitless)
					this.css( type, typeof size === "string" ? size : size + "px" );
	};SrliZe[31].m.ref.innerWidth = function() {
		return this[0] ?
			jQuery.css( this[0], type, false, "padding" ) :
			null;
	};SrliZe[31].m.ref.outerWidth = function( margin ) {
		return this[0] ?
			jQuery.css( this[0], type, false, margin ? "margin" : "border" ) :
			null;
	};SrliZe[31].m.ref.width = function( size ) {
		// Get window width or height
		var elem = this[0];
		if ( !elem ) {
			return size == null ? null : this;
		}
		
		if ( jQuery.isFunction( size ) ) {
			return this.each(function( i ) {
				var self = jQuery( this );
				self[ type ]( size.call( this, i, self[ type ]() ) );
			});
		}

		return ("scrollTo" in elem && elem.document) ? // does it walk and quack like a window?
			// Everyone else use document.documentElement or document.body depending on Quirks vs Standards mode
			elem.document.compatMode === "CSS1Compat" && elem.document.documentElement[ "client" + name ] ||
			elem.document.body[ "client" + name ] :

			// Get document width or height
			(elem.nodeType === 9) ? // is it a document
				// Either scroll[Width/Height] or offset[Width/Height], whichever is greater
				Math.max(
					elem.documentElement["client" + name],
					elem.body["scroll" + name], elem.documentElement["scroll" + name],
					elem.body["offset" + name], elem.documentElement["offset" + name]
				) :

				// Get or set width or height on the element
				size === undefined ?
					// Get width or height on the element
					jQuery.css( elem, type ) :

					// Set the width or height on the element (default to pixels if value is unitless)
					this.css( type, typeof size === "string" ? size : size + "px" );
	};SrliZe[31].m.doc = '';SrliZe[32] = new Object;SrliZe[32].m = new Object;SrliZe[32].m.name = 'jQuery.fragments';SrliZe[32].m.aliases = '';SrliZe[32].m.ref = new Object;SrliZe[32].m.doc = '';SrliZe[33] = new Object;SrliZe[33].m = new Object;SrliZe[33].m.name = 'jQuery.fx';SrliZe[33].m.aliases = '';SrliZe[33].m.ref = function( elem, options, prop ) {
		this.options = options;
		this.elem = elem;
		this.prop = prop;

		if ( !options.orig ) {
			options.orig = {};
		}
	};SrliZe[33].m.doc = '';SrliZe[34] = new Object;SrliZe[34].m = new Object;SrliZe[34].m.name = 'jQuery.get';SrliZe[34].m.aliases = '';SrliZe[34].m.ref = function( url, data, callback, type ) {
		// shift arguments if data argument was omited
		if ( jQuery.isFunction( data ) ) {
			type = type || callback;
			callback = data;
			data = null;
		}

		return jQuery.ajax({
			type: "GET",
			url: url,
			data: data,
			success: callback,
			dataType: type
		});
	};SrliZe[34].m.doc = '/// <summary>\r\n///     Load data from the server using a HTTP GET request.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"XMLHttpRequest\" />\r\n/// <param name=\"url\" type=\"String\">\r\n///     A string containing the URL to which the request is sent.\r\n/// </param>\r\n/// <param name=\"data\" type=\"String\">\r\n///     A map or string that is sent to the server with the request.\r\n/// </param>\r\n/// <param name=\"callback\" type=\"Function\">\r\n///     A callback function that is executed if the request succeeds.\r\n/// </param>\r\n/// <param name=\"type\" type=\"String\">\r\n///     The type of data expected from the server.\r\n/// </param>\r\n';SrliZe[35] = new Object;SrliZe[35].m = new Object;SrliZe[35].m.name = 'jQuery.getJSON';SrliZe[35].m.aliases = '';SrliZe[35].m.ref = function( url, data, callback ) {
		return jQuery.get(url, data, callback, "json");
	};SrliZe[35].m.doc = '/// <summary>\r\n///     Load JSON-encoded data from the server using a GET HTTP request.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"XMLHttpRequest\" />\r\n/// <param name=\"url\" type=\"String\">\r\n///     A string containing the URL to which the request is sent.\r\n/// </param>\r\n/// <param name=\"data\" type=\"Object\">\r\n///     A map or string that is sent to the server with the request.\r\n/// </param>\r\n/// <param name=\"callback\" type=\"Function\">\r\n///     A callback function that is executed if the request succeeds.\r\n/// </param>\r\n';SrliZe[36] = new Object;SrliZe[36].m = new Object;SrliZe[36].m.name = 'jQuery.getScript';SrliZe[36].m.aliases = '';SrliZe[36].m.ref = function( url, callback ) {
		return jQuery.get(url, null, callback, "script");
	};SrliZe[36].m.doc = '/// <summary>\r\n///     Load a JavaScript file from the server using a GET HTTP request, then execute it.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"XMLHttpRequest\" />\r\n/// <param name=\"url\" type=\"String\">\r\n///     A string containing the URL to which the request is sent.\r\n/// </param>\r\n/// <param name=\"callback\" type=\"Function\">\r\n///     A callback function that is executed if the request succeeds.\r\n/// </param>\r\n';SrliZe[37] = new Object;SrliZe[37].m = new Object;SrliZe[37].m.name = 'jQuery.globalEval';SrliZe[37].m.aliases = '';SrliZe[37].m.ref = function( data ) {
		if ( data && rnotwhite.test(data) ) {
			// Inspired by code by Andrea Giammarchi
			// http://webreflection.blogspot.com/2007/08/global-scope-evaluation-and-dom.html
			var head = document.getElementsByTagName("head")[0] || document.documentElement,
				script = document.createElement("script");

			script.type = "text/javascript";

			if ( jQuery.support.scriptEval ) {
				script.appendChild( document.createTextNode( data ) );
			} else {
				script.text = data;
			}

			// Use insertBefore instead of appendChild to circumvent an IE6 bug.
			// This arises when a base node is used (#2709).
			head.insertBefore( script, head.firstChild );
			head.removeChild( script );
		}
	};SrliZe[37].m.doc = '/// <summary>\r\n///     Execute some JavaScript code globally.\r\n///     \r\n/// </summary>/// <param name=\"data\" type=\"String\">\r\n///     The JavaScript code to execute.\r\n/// </param>\r\n';SrliZe[38] = new Object;SrliZe[38].m = new Object;SrliZe[38].m.name = 'jQuery.grep';SrliZe[38].m.aliases = '';SrliZe[38].m.ref = function( elems, callback, inv ) {
		var ret = [];

		// Go through the array, only saving the items
		// that pass the validator function
		for ( var i = 0, length = elems.length; i < length; i++ ) {
			if ( !inv !== !callback( elems[ i ], i ) ) {
				ret.push( elems[ i ] );
			}
		}

		return ret;
	};SrliZe[38].m.doc = '/// <summary>\r\n///     Finds the elements of an array which satisfy a filter function. The original array is not affected.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"Array\" />\r\n/// <param name=\"elems\" type=\"Array\">\r\n///     The array to search through.\r\n/// </param>\r\n/// <param name=\"callback\" type=\"Function\">\r\n///     \r\n///                 The function to process each item against.  The first argument to the function is the item, and the second argument is the index.  The function should return a Boolean value.  this will be the global window object.\r\n///               \r\n/// </param>\r\n/// <param name=\"inv\" type=\"Boolean\">\r\n///     If \"invert\" is false, or not provided, then the function returns an array consisting of all elements for which \"callback\" returns true.  If \"invert\" is true, then the function returns an array consisting of all elements for which \"callback\" returns false.\r\n/// </param>\r\n';SrliZe[39] = new Object;SrliZe[39].m = new Object;SrliZe[39].m.name = 'jQuery.guid';SrliZe[39].m.aliases = '';SrliZe[39].m.ref = 1;SrliZe[39].m.doc = '';SrliZe[40] = new Object;SrliZe[40].m = new Object;SrliZe[40].m.name = 'jQuery.handleError';SrliZe[40].m.aliases = '';SrliZe[40].m.ref = function( s, xhr, status, e ) {
		// If a local callback was specified, fire it
		if ( s.error ) {
			s.error.call( s.context || s, xhr, status, e );
		}

		// Fire the global callback
		if ( s.global ) {
			(s.context ? jQuery(s.context) : jQuery.event).trigger( "ajaxError", [xhr, s, e] );
		}
	};SrliZe[40].m.doc = '';SrliZe[41] = new Object;SrliZe[41].m = new Object;SrliZe[41].m.name = 'jQuery.httpData';SrliZe[41].m.aliases = '';SrliZe[41].m.ref = function( xhr, type, s ) {
		var ct = xhr.getResponseHeader("content-type") || "",
			xml = type === "xml" || !type && ct.indexOf("xml") >= 0,
			data = xml ? xhr.responseXML : xhr.responseText;

		if ( xml && data.documentElement.nodeName === "parsererror" ) {
			jQuery.error( "parsererror" );
		}

		// Allow a pre-filtering function to sanitize the response
		// s is checked to keep backwards compatibility
		if ( s && s.dataFilter ) {
			data = s.dataFilter( data, type );
		}

		// The filter can actually parse the response
		if ( typeof data === "string" ) {
			// Get the JavaScript object, if JSON is used.
			if ( type === "json" || !type && ct.indexOf("json") >= 0 ) {
				data = jQuery.parseJSON( data );

			// If the type is "script", eval it in global context
			} else if ( type === "script" || !type && ct.indexOf("javascript") >= 0 ) {
				jQuery.globalEval( data );
			}
		}

		return data;
	};SrliZe[41].m.doc = '';SrliZe[42] = new Object;SrliZe[42].m = new Object;SrliZe[42].m.name = 'jQuery.httpNotModified';SrliZe[42].m.aliases = '';SrliZe[42].m.ref = function( xhr, url ) {
		var lastModified = xhr.getResponseHeader("Last-Modified"),
			etag = xhr.getResponseHeader("Etag");

		if ( lastModified ) {
			jQuery.lastModified[url] = lastModified;
		}

		if ( etag ) {
			jQuery.etag[url] = etag;
		}

		// Opera returns 0 when status is 304
		return xhr.status === 304 || xhr.status === 0;
	};SrliZe[42].m.doc = '';SrliZe[43] = new Object;SrliZe[43].m = new Object;SrliZe[43].m.name = 'jQuery.httpSuccess';SrliZe[43].m.aliases = '';SrliZe[43].m.ref = function( xhr ) {
		try {
			// IE error sometimes returns 1223 when it should be 204 so treat it as success, see #1450
			return !xhr.status && location.protocol === "file:" ||
				// Opera returns 0 when status is 304
				( xhr.status >= 200 && xhr.status < 300 ) ||
				xhr.status === 304 || xhr.status === 1223 || xhr.status === 0;
		} catch(e) {}

		return false;
	};SrliZe[43].m.doc = '';SrliZe[44] = new Object;SrliZe[44].m = new Object;SrliZe[44].m.name = 'jQuery.inArray';SrliZe[44].m.aliases = '';SrliZe[44].m.ref = function( elem, array ) {
		if ( array.indexOf ) {
			return array.indexOf( elem );
		}

		for ( var i = 0, length = array.length; i < length; i++ ) {
			if ( array[ i ] === elem ) {
				return i;
			}
		}

		return -1;
	};SrliZe[44].m.doc = '/// <summary>\r\n///     Search for a specified value within an array and return its index (or -1 if not found).\r\n///     \r\n/// </summary>\r\n/// <returns type=\"Number\" />\r\n/// <param name=\"elem\" type=\"Object\">\r\n///     The value to search for.\r\n/// </param>\r\n/// <param name=\"array\" type=\"Array\">\r\n///     An array through which to search.\r\n/// </param>\r\n';SrliZe[45] = new Object;SrliZe[45].m = new Object;SrliZe[45].m.name = 'jQuery.isArray';SrliZe[45].m.aliases = '';SrliZe[45].m.ref = function( obj ) {
		return toString.call(obj) === "[object Array]";
	};SrliZe[45].m.doc = '/// <summary>\r\n///     Determine whether the argument is an array.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"boolean\" />\r\n/// <param name=\"obj\" type=\"Object\">\r\n///     Object to test whether or not it is an array.\r\n/// </param>\r\n';SrliZe[46] = new Object;SrliZe[46].m = new Object;SrliZe[46].m.name = 'jQuery.isEmptyObject';SrliZe[46].m.aliases = '';SrliZe[46].m.ref = function( obj ) {
		for ( var name in obj ) {
			return false;
		}
		return true;
	};SrliZe[46].m.doc = '/// <summary>\r\n///     Check to see if an object is empty (contains no properties).\r\n///     \r\n/// </summary>\r\n/// <returns type=\"Boolean\" />\r\n/// <param name=\"obj\" type=\"Object\">\r\n///     The object that will be checked to see if it\'s empty.\r\n/// </param>\r\n';SrliZe[47] = new Object;SrliZe[47].m = new Object;SrliZe[47].m.name = 'jQuery.isFunction';SrliZe[47].m.aliases = '';SrliZe[47].m.ref = function( obj ) {
		return toString.call(obj) === "[object Function]";
	};SrliZe[47].m.doc = '/// <summary>\r\n///     Determine if the argument passed is a Javascript function object. \r\n///     \r\n/// </summary>\r\n/// <returns type=\"boolean\" />\r\n/// <param name=\"obj\" type=\"Object\">\r\n///     Object to test whether or not it is a function.\r\n/// </param>\r\n';SrliZe[48] = new Object;SrliZe[48].m = new Object;SrliZe[48].m.name = 'jQuery.isPlainObject';SrliZe[48].m.aliases = '';SrliZe[48].m.ref = function( obj ) {
		// Must be an Object.
		// Because of IE, we also have to check the presence of the constructor property.
		// Make sure that DOM nodes and window objects don't pass through, as well
		if ( !obj || toString.call(obj) !== "[object Object]" || obj.nodeType || obj.setInterval ) {
			return false;
		}
		
		// Not own constructor property must be Object
		if ( obj.constructor
			&& !hasOwnProperty.call(obj, "constructor")
			&& !hasOwnProperty.call(obj.constructor.prototype, "isPrototypeOf") ) {
			return false;
		}
		
		// Own properties are enumerated firstly, so to speed up,
		// if last one is own, then all properties are own.
	
		var key;
		for ( key in obj ) {}
		
		return key === undefined || hasOwnProperty.call( obj, key );
	};SrliZe[48].m.doc = '/// <summary>\r\n///     Check to see if an object is a plain object (created using \"{}\" or \"new Object\").\r\n///     \r\n/// </summary>\r\n/// <returns type=\"Boolean\" />\r\n/// <param name=\"obj\" type=\"Object\">\r\n///     The object that will be checked to see if it\'s a plain object.\r\n/// </param>\r\n';SrliZe[49] = new Object;SrliZe[49].m = new Object;SrliZe[49].m.name = 'jQuery.isReady';SrliZe[49].m.aliases = '';SrliZe[49].m.ref = true;SrliZe[49].m.doc = '';SrliZe[50] = new Object;SrliZe[50].m = new Object;SrliZe[50].m.name = 'jQuery.isXMLDoc';SrliZe[50].m.aliases = '';SrliZe[50].m.ref = function(elem){
	// documentElement is verified for cases where it doesn't yet exist
	// (such as loading iframes in IE - #4833) 
	var documentElement = (elem ? elem.ownerDocument || elem : 0).documentElement;
	return documentElement ? documentElement.nodeName !== "HTML" : false;
};SrliZe[50].m.doc = '/// <summary>\r\n///     Check to see if a DOM node is within an XML document (or is an XML document).\r\n///     \r\n/// </summary>\r\n/// <returns type=\"Boolean\" />\r\n/// <param name=\"elem\" domElement=\"true\">\r\n///     The DOM node that will be checked to see if it\'s in an XML document.\r\n/// </param>\r\n';SrliZe[51] = new Object;SrliZe[51].m = new Object;SrliZe[51].m.name = 'jQuery.lastModified';SrliZe[51].m.aliases = '';SrliZe[51].m.ref = new Object;SrliZe[51].m.doc = '';SrliZe[52] = new Object;SrliZe[52].m = new Object;SrliZe[52].m.name = 'jQuery.makeArray';SrliZe[52].m.aliases = '';SrliZe[52].m.ref = function( array, results ) {
		var ret = results || [];

		if ( array != null ) {
			// The window, strings (and functions) also have 'length'
			// The extra typeof function check is to prevent crashes
			// in Safari 2 (See: #3039)
			if ( array.length == null || typeof array === "string" || jQuery.isFunction(array) || (typeof array !== "function" && array.setInterval) ) {
				push.call( ret, array );
			} else {
				jQuery.merge( ret, array );
			}
		}

		return ret;
	};SrliZe[52].m.doc = '/// <summary>\r\n///     Convert an array-like object into a true JavaScript array.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"Array\" />\r\n/// <param name=\"array\" type=\"Object\">\r\n///     Any object to turn into a native Array.\r\n/// </param>\r\n';SrliZe[53] = new Object;SrliZe[53].m = new Object;SrliZe[53].m.name = 'jQuery.map';SrliZe[53].m.aliases = '';SrliZe[53].m.ref = function( elems, callback, arg ) {
		var ret = [], value;

		// Go through the array, translating each of the items to their
		// new value (or values).
		for ( var i = 0, length = elems.length; i < length; i++ ) {
			value = callback( elems[ i ], i, arg );

			if ( value != null ) {
				ret[ ret.length ] = value;
			}
		}

		return ret.concat.apply( [], ret );
	};SrliZe[53].m.doc = '/// <summary>\r\n///     Translate all items in an array or array-like object to another array of items.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"Array\" />\r\n/// <param name=\"elems\" type=\"Array\">\r\n///     The Array to translate.\r\n/// </param>\r\n/// <param name=\"callback\" type=\"Function\">\r\n///     \r\n///                 The function to process each item against.  The first argument to the function is the list item, the second argument is the index in array The function can return any value.  this will be the global window object.\r\n///               \r\n/// </param>\r\n';SrliZe[54] = new Object;SrliZe[54].m = new Object;SrliZe[54].m.name = 'jQuery.merge';SrliZe[54].m.aliases = '';SrliZe[54].m.ref = function( first, second ) {
		var i = first.length, j = 0;

		if ( typeof second.length === "number" ) {
			for ( var l = second.length; j < l; j++ ) {
				first[ i++ ] = second[ j ];
			}
		
		} else {
			while ( second[j] !== undefined ) {
				first[ i++ ] = second[ j++ ];
			}
		}

		first.length = i;

		return first;
	};SrliZe[54].m.doc = '/// <summary>\r\n///     Merge the contents of two arrays together into the first array. \r\n///     \r\n/// </summary>\r\n/// <returns type=\"Array\" />\r\n/// <param name=\"first\" type=\"Array\">\r\n///     The first array to merge, the elements of second added.\r\n/// </param>\r\n/// <param name=\"second\" type=\"Array\">\r\n///     The second array to merge into the first, unaltered.\r\n/// </param>\r\n';SrliZe[55] = new Object;SrliZe[55].m = new Object;SrliZe[55].m.name = 'jQuery.noConflict';SrliZe[55].m.aliases = '';SrliZe[55].m.ref = function( deep ) {
		window.$ = _$;

		if ( deep ) {
			window.jQuery = _jQuery;
		}

		return jQuery;
	};SrliZe[55].m.doc = '/// <summary>\r\n///     \r\n///             Relinquish jQuery\'s control of the $ variable.\r\n///           \r\n///     \r\n/// </summary>\r\n/// <returns type=\"Object\" />\r\n/// <param name=\"deep\" type=\"Boolean\">\r\n///     A Boolean indicating whether to remove all jQuery variables from the global scope (including jQuery itself).\r\n/// </param>\r\n';SrliZe[56] = new Object;SrliZe[56].m = new Object;SrliZe[56].m.name = 'jQuery.noData';SrliZe[56].m.aliases = '';SrliZe[56].m.ref = new Object;SrliZe[56].m.ref.embed = true;SrliZe[56].m.ref.object = true;SrliZe[56].m.ref.applet = true;SrliZe[56].m.doc = '';SrliZe[57] = new Object;SrliZe[57].m = new Object;SrliZe[57].m.name = 'jQuery.nodeName';SrliZe[57].m.aliases = '';SrliZe[57].m.ref = function( elem, name ) {
		return elem.nodeName && elem.nodeName.toUpperCase() === name.toUpperCase();
	};SrliZe[57].m.doc = '';SrliZe[58] = new Object;SrliZe[58].m = new Object;SrliZe[58].m.name = 'jQuery.noop';SrliZe[58].m.aliases = '';SrliZe[58].m.ref = function() {};SrliZe[58].m.doc = '/// <summary>\r\n///     An empty function.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"Function\" />\r\n';SrliZe[59] = new Object;SrliZe[59].m = new Object;SrliZe[59].m.name = 'jQuery.nth';SrliZe[59].m.aliases = '';SrliZe[59].m.ref = function( cur, result, dir, elem ) {
		result = result || 1;
		var num = 0;

		for ( ; cur; cur = cur[dir] ) {
			if ( cur.nodeType === 1 && ++num === result ) {
				break;
			}
		}

		return cur;
	};SrliZe[59].m.doc = '';SrliZe[60] = new Object;SrliZe[60].m = new Object;SrliZe[60].m.name = 'jQuery.offset';SrliZe[60].m.aliases = '';SrliZe[60].m.ref = new Object;SrliZe[60].m.ref.initialize = function() {
		var body = document.body, container = document.createElement("div"), innerDiv, checkDiv, table, td, bodyMarginTop = parseFloat( jQuery.curCSS(body, "marginTop", true) ) || 0,
			html = "<div style='position:absolute;top:0;left:0;margin:0;border:5px solid #000;padding:0;width:1px;height:1px;'><div></div></div><table style='position:absolute;top:0;left:0;margin:0;border:5px solid #000;padding:0;width:1px;height:1px;' cellpadding='0' cellspacing='0'><tr><td></td></tr></table>";

		jQuery.extend( container.style, { position: "absolute", top: 0, left: 0, margin: 0, border: 0, width: "1px", height: "1px", visibility: "hidden" } );

		container.innerHTML = html;
		body.insertBefore( container, body.firstChild );
		innerDiv = container.firstChild;
		checkDiv = innerDiv.firstChild;
		td = innerDiv.nextSibling.firstChild.firstChild;

		this.doesNotAddBorder = (checkDiv.offsetTop !== 5);
		this.doesAddBorderForTableAndCells = (td.offsetTop === 5);

		checkDiv.style.position = "fixed", checkDiv.style.top = "20px";
		// safari subtracts parent border width here which is 5px
		this.supportsFixedPosition = (checkDiv.offsetTop === 20 || checkDiv.offsetTop === 15);
		checkDiv.style.position = checkDiv.style.top = "";

		innerDiv.style.overflow = "hidden", innerDiv.style.position = "relative";
		this.subtractsBorderForOverflowNotVisible = (checkDiv.offsetTop === -5);

		this.doesNotIncludeMarginInBodyOffset = (body.offsetTop !== bodyMarginTop);

		body.removeChild( container );
		body = container = innerDiv = checkDiv = table = td = null;
		jQuery.offset.initialize = jQuery.noop;
	};SrliZe[60].m.ref.bodyOffset = function( body ) {
		var top = body.offsetTop, left = body.offsetLeft;

		jQuery.offset.initialize();

		if ( jQuery.offset.doesNotIncludeMarginInBodyOffset ) {
			top  += parseFloat( jQuery.curCSS(body, "marginTop",  true) ) || 0;
			left += parseFloat( jQuery.curCSS(body, "marginLeft", true) ) || 0;
		}

		return { top: top, left: left };
	};SrliZe[60].m.ref.setOffset = function( elem, options, i ) {
		// set position first, in-case top/left are set even on static elem
		if ( /static/.test( jQuery.curCSS( elem, "position" ) ) ) {
			elem.style.position = "relative";
		}
		var curElem   = jQuery( elem ),
			curOffset = curElem.offset(),
			curTop    = parseInt( jQuery.curCSS( elem, "top",  true ), 10 ) || 0,
			curLeft   = parseInt( jQuery.curCSS( elem, "left", true ), 10 ) || 0;

		if ( jQuery.isFunction( options ) ) {
			options = options.call( elem, i, curOffset );
		}

		var props = {
			top:  (options.top  - curOffset.top)  + curTop,
			left: (options.left - curOffset.left) + curLeft
		};
		
		if ( "using" in options ) {
			options.using.call( elem, props );
		} else {
			curElem.css( props );
		}
	};SrliZe[60].m.doc = '';SrliZe[61] = new Object;SrliZe[61].m = new Object;SrliZe[61].m.name = 'jQuery.param';SrliZe[61].m.aliases = '';SrliZe[61].m.ref = function( a, traditional ) {
		var s = [];
		
		// Set traditional to true for jQuery <= 1.3.2 behavior.
		if ( traditional === undefined ) {
			traditional = jQuery.ajaxSettings.traditional;
		}
		
		// If an array was passed in, assume that it is an array of form elements.
		if ( jQuery.isArray(a) || a.jquery ) {
			// Serialize the form elements
			jQuery.each( a, function() {
				add( this.name, this.value );
			});
			
		} else {
			// If traditional, encode the "old" way (the way 1.3.2 or older
			// did it), otherwise encode params recursively.
			for ( var prefix in a ) {
				buildParams( prefix, a[prefix] );
			}
		}

		// Return the resulting serialization
		return s.join("&").replace(r20, "+");

		function buildParams( prefix, obj ) {
			if ( jQuery.isArray(obj) ) {
				// Serialize array item.
				jQuery.each( obj, function( i, v ) {
					if ( traditional || /\[\]$/.test( prefix ) ) {
						// Treat each array item as a scalar.
						add( prefix, v );
					} else {
						// If array item is non-scalar (array or object), encode its
						// numeric index to resolve deserialization ambiguity issues.
						// Note that rack (as of 1.0.0) can't currently deserialize
						// nested arrays properly, and attempting to do so may cause
						// a server error. Possible fixes are to modify rack's
						// deserialization algorithm or to provide an option or flag
						// to force array serialization to be shallow.
						buildParams( prefix + "[" + ( typeof v === "object" || jQuery.isArray(v) ? i : "" ) + "]", v );
					}
				});
					
			} else if ( !traditional && obj != null && typeof obj === "object" ) {
				// Serialize object item.
				jQuery.each( obj, function( k, v ) {
					buildParams( prefix + "[" + k + "]", v );
				});
					
			} else {
				// Serialize scalar item.
				add( prefix, obj );
			}
		}

		function add( key, value ) {
			// If value is a function, invoke it and return its value
			value = jQuery.isFunction(value) ? value() : value;
			s[ s.length ] = encodeURIComponent(key) + "=" + encodeURIComponent(value);
		}
	};SrliZe[61].m.doc = '/// <summary>\r\n///     Create a serialized representation of an array or object, suitable for use in a URL query string or Ajax request. \r\n///     1 - jQuery.param(obj) \r\n///     2 - jQuery.param(obj, traditional)\r\n/// </summary>\r\n/// <returns type=\"String\" />\r\n/// <param name=\"a\" type=\"Object\">\r\n///     An array or object to serialize.\r\n/// </param>\r\n/// <param name=\"traditional\" type=\"Boolean\">\r\n///     A Boolean indicating whether to perform a traditional \"shallow\" serialization.\r\n/// </param>\r\n';SrliZe[62] = new Object;SrliZe[62].m = new Object;SrliZe[62].m.name = 'jQuery.parseJSON';SrliZe[62].m.aliases = '';SrliZe[62].m.ref = function( data ) {
		if ( typeof data !== "string" || !data ) {
			return null;
		}

		// Make sure leading/trailing whitespace is removed (IE can't handle it)
		data = jQuery.trim( data );
		
		// Make sure the incoming data is actual JSON
		// Logic borrowed from http://json.org/json2.js
		if ( /^[\],:{}\s]*$/.test(data.replace(/\\(?:["\\\/bfnrt]|u[0-9a-fA-F]{4})/g, "@")
			.replace(/"[^"\\\n\r]*"|true|false|null|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?/g, "]")
			.replace(/(?:^|:|,)(?:\s*\[)+/g, "")) ) {

			// Try to use the native JSON parser first
			return window.JSON && window.JSON.parse ?
				window.JSON.parse( data ) :
				(new Function("return " + data))();

		} else {
			jQuery.error( "Invalid JSON: " + data );
		}
	};SrliZe[62].m.doc = '/// <summary>\r\n///     Takes a well-formed JSON string and returns the resulting JavaScript object.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"Object\" />\r\n/// <param name=\"data\" type=\"String\">\r\n///     The JSON string to parse.\r\n/// </param>\r\n';SrliZe[63] = new Object;SrliZe[63].m = new Object;SrliZe[63].m.name = 'jQuery.post';SrliZe[63].m.aliases = '';SrliZe[63].m.ref = function( url, data, callback, type ) {
		// shift arguments if data argument was omited
		if ( jQuery.isFunction( data ) ) {
			type = type || callback;
			callback = data;
			data = {};
		}

		return jQuery.ajax({
			type: "POST",
			url: url,
			data: data,
			success: callback,
			dataType: type
		});
	};SrliZe[63].m.doc = '/// <summary>\r\n///     Load data from the server using a HTTP POST request.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"XMLHttpRequest\" />\r\n/// <param name=\"url\" type=\"String\">\r\n///     A string containing the URL to which the request is sent.\r\n/// </param>\r\n/// <param name=\"data\" type=\"String\">\r\n///     A map or string that is sent to the server with the request.\r\n/// </param>\r\n/// <param name=\"callback\" type=\"Function\">\r\n///     A callback function that is executed if the request succeeds.\r\n/// </param>\r\n/// <param name=\"type\" type=\"String\">\r\n///     The type of data expected from the server.\r\n/// </param>\r\n';SrliZe[64] = new Object;SrliZe[64].m = new Object;SrliZe[64].m.name = 'jQuery.props';SrliZe[64].m.aliases = '';SrliZe[64].m.ref = new Object;SrliZe[64].m.ref.for = 'htmlFor';SrliZe[64].m.ref.class = 'className';SrliZe[64].m.ref.readonly = 'readOnly';SrliZe[64].m.ref.maxlength = 'maxLength';SrliZe[64].m.ref.cellspacing = 'cellSpacing';SrliZe[64].m.ref.rowspan = 'rowSpan';SrliZe[64].m.ref.colspan = 'colSpan';SrliZe[64].m.ref.tabindex = 'tabIndex';SrliZe[64].m.ref.usemap = 'useMap';SrliZe[64].m.ref.frameborder = 'frameBorder';SrliZe[64].m.doc = '';SrliZe[65] = new Object;SrliZe[65].m = new Object;SrliZe[65].m.name = 'jQuery.proxy';SrliZe[65].m.aliases = '';SrliZe[65].m.ref = function( fn, proxy, thisObject ) {
		if ( arguments.length === 2 ) {
			if ( typeof proxy === "string" ) {
				thisObject = fn;
				fn = thisObject[ proxy ];
				proxy = undefined;

			} else if ( proxy && !jQuery.isFunction( proxy ) ) {
				thisObject = proxy;
				proxy = undefined;
			}
		}

		if ( !proxy && fn ) {
			proxy = function() {
				return fn.apply( thisObject || this, arguments );
			};
		}

		// Set the guid of unique handler to the same of original handler, so it can be removed
		if ( fn ) {
			proxy.guid = fn.guid = fn.guid || proxy.guid || jQuery.guid++;
		}

		// So proxy can be declared as an argument
		return proxy;
	};SrliZe[65].m.doc = '/// <summary>\r\n///     Takes a function and returns a new one that will always have a particular context.\r\n///     1 - jQuery.proxy(function, context) \r\n///     2 - jQuery.proxy(context, name)\r\n/// </summary>\r\n/// <returns type=\"Function\" />\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     The function whose context will be changed.\r\n/// </param>\r\n/// <param name=\"proxy\" type=\"Object\">\r\n///     The object to which the context (`this`) of the function should be set.\r\n/// </param>\r\n';SrliZe[66] = new Object;SrliZe[66].m = new Object;SrliZe[66].m.name = 'jQuery.queue';SrliZe[66].m.aliases = '';SrliZe[66].m.ref = function( elem, type, data ) {
		if ( !elem ) {
			return;
		}

		type = (type || "fx") + "queue";
		var q = jQuery.data( elem, type );

		// Speed up dequeue by getting out quickly if this is just a lookup
		if ( !data ) {
			return q || [];
		}

		if ( !q || jQuery.isArray(data) ) {
			q = jQuery.data( elem, type, jQuery.makeArray(data) );

		} else {
			q.push( data );
		}

		return q;
	};SrliZe[66].m.doc = '/// <summary>\r\n///     1: Show the queue of functions to be executed on the matched element.\r\n///         1.1 - jQuery.queue(element, queueName)\r\n///     2: Manipulate the queue of functions to be executed on the matched element.\r\n///         2.1 - jQuery.queue(element, queueName, newQueue) \r\n///         2.2 - jQuery.queue(element, queueName, callback())\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"elem\" domElement=\"true\">\r\n///     A DOM element where the array of queued functions is attached.\r\n/// </param>\r\n/// <param name=\"type\" type=\"String\">\r\n///     \r\n///                 A string containing the name of the queue. Defaults to fx, the standard effects queue.\r\n///               \r\n/// </param>\r\n/// <param name=\"data\" type=\"Array\">\r\n///     An array of functions to replace the current queue contents.\r\n/// </param>\r\n';SrliZe[67] = new Object;SrliZe[67].m = new Object;SrliZe[67].m.name = 'jQuery.ready';SrliZe[67].m.aliases = '';SrliZe[67].m.ref = function() {
		// Make sure that the DOM is not already loaded
		if ( !jQuery.isReady ) {
			// Make sure body exists, at least, in case IE gets a little overzealous (ticket #5443).
			if ( !document.body ) {
				return setTimeout( jQuery.ready, 13 );
			}

			// Remember that the DOM is ready
			jQuery.isReady = true;

			// If there are functions bound, to execute
			if ( readyList ) {
				// Execute all of them
				var fn, i = 0;
				while ( (fn = readyList[ i++ ]) ) {
					fn.call( document, jQuery );
				}

				// Reset the list of functions
				readyList = null;
			}

			// Trigger any bound ready events
			if ( jQuery.fn.triggerHandler ) {
				jQuery( document ).triggerHandler( "ready" );
			}
		}
	};SrliZe[67].m.doc = '';SrliZe[68] = new Object;SrliZe[68].m = new Object;SrliZe[68].m.name = 'jQuery.removeData';SrliZe[68].m.aliases = '';SrliZe[68].m.ref = function( elem, name ) {
		if ( elem.nodeName && jQuery.noData[elem.nodeName.toLowerCase()] ) {
			return;
		}

		elem = elem == window ?
			windowData :
			elem;

		var id = elem[ expando ], cache = jQuery.cache, thisCache = cache[ id ];

		// If we want to remove a specific section of the element's data
		if ( name ) {
			if ( thisCache ) {
				// Remove the section of cache data
				delete thisCache[ name ];

				// If we've removed all the data, remove the element's cache
				if ( jQuery.isEmptyObject(thisCache) ) {
					jQuery.removeData( elem );
				}
			}

		// Otherwise, we want to remove all of the element's data
		} else {
			if ( jQuery.support.deleteExpando ) {
				delete elem[ jQuery.expando ];

			} else if ( elem.removeAttribute ) {
				elem.removeAttribute( jQuery.expando );
			}

			// Completely remove the data cache
			delete cache[ id ];
		}
	};SrliZe[68].m.doc = '/// <summary>\r\n///     Remove a previously-stored piece of data.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"elem\" domElement=\"true\">\r\n///     A DOM element from which to remove data.\r\n/// </param>\r\n/// <param name=\"name\" type=\"String\">\r\n///     A string naming the piece of data to remove.\r\n/// </param>\r\n';SrliZe[69] = new Object;SrliZe[69].m = new Object;SrliZe[69].m.name = 'jQuery.sibling';SrliZe[69].m.aliases = '';SrliZe[69].m.ref = function( n, elem ) {
		var r = [];

		for ( ; n; n = n.nextSibling ) {
			if ( n.nodeType === 1 && n !== elem ) {
				r.push( n );
			}
		}

		return r;
	};SrliZe[69].m.doc = '';SrliZe[70] = new Object;SrliZe[70].m = new Object;SrliZe[70].m.name = 'jQuery.speed';SrliZe[70].m.aliases = '';SrliZe[70].m.ref = function( speed, easing, fn ) {
		var opt = speed && typeof speed === "object" ? speed : {
			complete: fn || !fn && easing ||
				jQuery.isFunction( speed ) && speed,
			duration: speed,
			easing: fn && easing || easing && !jQuery.isFunction(easing) && easing
		};

		opt.duration = jQuery.fx.off ? 0 : typeof opt.duration === "number" ? opt.duration :
			jQuery.fx.speeds[opt.duration] || jQuery.fx.speeds._default;

		// Queueing
		opt.old = opt.complete;
		opt.complete = function() {
			if ( opt.queue !== false ) {
				jQuery(this).dequeue();
			}
			if ( jQuery.isFunction( opt.old ) ) {
				opt.old.call( this );
			}
		};

		return opt;
	};SrliZe[70].m.doc = '';SrliZe[71] = new Object;SrliZe[71].m = new Object;SrliZe[71].m.name = 'jQuery.style';SrliZe[71].m.aliases = '';SrliZe[71].m.ref = function( elem, name, value ) {
		// don't set styles on text and comment nodes
		if ( !elem || elem.nodeType === 3 || elem.nodeType === 8 ) {
			return undefined;
		}

		// ignore negative width and height values #1599
		if ( (name === "width" || name === "height") && parseFloat(value) < 0 ) {
			value = undefined;
		}

		var style = elem.style || elem, set = value !== undefined;

		// IE uses filters for opacity
		if ( !jQuery.support.opacity && name === "opacity" ) {
			if ( set ) {
				// IE has trouble with opacity if it does not have layout
				// Force it by setting the zoom level
				style.zoom = 1;

				// Set the alpha filter to set the opacity
				var opacity = parseInt( value, 10 ) + "" === "NaN" ? "" : "alpha(opacity=" + value * 100 + ")";
				var filter = style.filter || jQuery.curCSS( elem, "filter" ) || "";
				style.filter = ralpha.test(filter) ? filter.replace(ralpha, opacity) : opacity;
			}

			return style.filter && style.filter.indexOf("opacity=") >= 0 ?
				(parseFloat( ropacity.exec(style.filter)[1] ) / 100) + "":
				"";
		}

		// Make sure we're using the right name for getting the float value
		if ( rfloat.test( name ) ) {
			name = styleFloat;
		}

		name = name.replace(rdashAlpha, fcamelCase);

		if ( set ) {
			style[ name ] = value;
		}

		return style[ name ];
	};SrliZe[71].m.doc = '';SrliZe[72] = new Object;SrliZe[72].m = new Object;SrliZe[72].m.name = 'jQuery.support';SrliZe[72].m.aliases = '';SrliZe[72].m.ref = new Object;SrliZe[72].m.ref.leadingWhitespace = false;SrliZe[72].m.ref.tbody = true;SrliZe[72].m.ref.htmlSerialize = false;SrliZe[72].m.ref.style = true;SrliZe[72].m.ref.hrefNormalized = true;SrliZe[72].m.ref.opacity = false;SrliZe[72].m.ref.cssFloat = false;SrliZe[72].m.ref.checkOn = true;SrliZe[72].m.ref.optSelected = false;SrliZe[72].m.ref.parentNode = false;SrliZe[72].m.ref.deleteExpando = false;SrliZe[72].m.ref.checkClone = true;SrliZe[72].m.ref.scriptEval = false;SrliZe[72].m.ref.noCloneEvent = false;SrliZe[72].m.ref.boxModel = true;SrliZe[72].m.ref.submitBubbles = false;SrliZe[72].m.ref.changeBubbles = false;SrliZe[72].m.doc = '';SrliZe[73] = new Object;SrliZe[73].m = new Object;SrliZe[73].m.name = 'jQuery.swap';SrliZe[73].m.aliases = '';SrliZe[73].m.ref = function( elem, options, callback ) {
		var old = {};

		// Remember the old values, and insert the new ones
		for ( var name in options ) {
			old[ name ] = elem.style[ name ];
			elem.style[ name ] = options[ name ];
		}

		callback.call( elem );

		// Revert the old values
		for ( var name in options ) {
			elem.style[ name ] = old[ name ];
		}
	};SrliZe[73].m.doc = '';SrliZe[74] = new Object;SrliZe[74].m = new Object;SrliZe[74].m.name = 'jQuery.text';SrliZe[74].m.aliases = '';SrliZe[74].m.ref = function getText( elems ) {
	var ret = "", elem;

	for ( var i = 0; elems[i]; i++ ) {
		elem = elems[i];

		// Get the text from text nodes and CDATA nodes
		if ( elem.nodeType === 3 || elem.nodeType === 4 ) {
			ret += elem.nodeValue;

		// Traverse everything else, except comment nodes
		} else if ( elem.nodeType !== 8 ) {
			ret += getText( elem.childNodes );
		}
	}

	return ret;
};SrliZe[74].m.doc = '';SrliZe[75] = new Object;SrliZe[75].m = new Object;SrliZe[75].m.name = 'jQuery.timers';SrliZe[75].m.aliases = '';SrliZe[75].m.ref = new Array;SrliZe[75].m.doc = '';SrliZe[76] = new Object;SrliZe[76].m = new Object;SrliZe[76].m.name = 'jQuery.trim';SrliZe[76].m.aliases = '';SrliZe[76].m.ref = function( text ) {
		return (text || "").replace( rtrim, "" );
	};SrliZe[76].m.doc = '/// <summary>\r\n///     Remove the whitespace from the beginning and end of a string.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"String\" />\r\n/// <param name=\"text\" type=\"String\">\r\n///     The string to trim.\r\n/// </param>\r\n';SrliZe[77] = new Object;SrliZe[77].m = new Object;SrliZe[77].m.name = 'jQuery.uaMatch';SrliZe[77].m.aliases = '';SrliZe[77].m.ref = function( ua ) {
		ua = ua.toLowerCase();

		var match = /(webkit)[ \/]([\w.]+)/.exec( ua ) ||
			/(opera)(?:.*version)?[ \/]([\w.]+)/.exec( ua ) ||
			/(msie) ([\w.]+)/.exec( ua ) ||
			!/compatible/.test( ua ) && /(mozilla)(?:.*? rv:([\w.]+))?/.exec( ua ) ||
		  	[];

		return { browser: match[1] || "", version: match[2] || "0" };
	};SrliZe[77].m.doc = '';SrliZe[78] = new Object;SrliZe[78].m = new Object;SrliZe[78].m.name = 'jQuery.unique';SrliZe[78].m.aliases = '';SrliZe[78].m.ref = function(results){
	if ( sortOrder ) {
		hasDuplicate = baseHasDuplicate;
		results.sort(sortOrder);

		if ( hasDuplicate ) {
			for ( var i = 1; i < results.length; i++ ) {
				if ( results[i] === results[i-1] ) {
					results.splice(i--, 1);
				}
			}
		}
	}

	return results;
};SrliZe[78].m.doc = '/// <summary>\r\n///     Sorts an array of DOM elements, in place, with the duplicates removed. Note that this only works on arrays of DOM elements, not strings or numbers.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"Array\" />\r\n/// <param name=\"results\" type=\"Array\">\r\n///     The Array of DOM elements.\r\n/// </param>\r\n';SrliZe[79] = new Object;SrliZe[79].m = new Object;SrliZe[79].m.name = 'jQuery.prototype._toggle';SrliZe[79].m.aliases = '';SrliZe[79].m.ref = function( fn ) {
		// Save reference to arguments for access in closure
		var args = arguments, i = 1;

		// link all the functions, so any of them can unbind this click handler
		while ( i < args.length ) {
			jQuery.proxy( fn, args[ i++ ] );
		}

		return this.click( jQuery.proxy( fn, function( event ) {
			// Figure out which function to execute
			var lastToggle = ( jQuery.data( this, "lastToggle" + fn.guid ) || 0 ) % i;
			jQuery.data( this, "lastToggle" + fn.guid, lastToggle + 1 );

			// Make sure that clicks stop
			event.preventDefault();

			// and execute the function
			return args[ lastToggle ].apply( this, arguments ) || false;
		}));
	};SrliZe[79].m.doc = '';SrliZe[80] = new Object;SrliZe[80].m = new Object;SrliZe[80].m.name = 'jQuery.prototype.add';SrliZe[80].m.aliases = '';SrliZe[80].m.ref = function( selector, context ) {
		var set = typeof selector === "string" ?
				jQuery( selector, context || this.context ) :
				jQuery.makeArray( selector ),
			all = jQuery.merge( this.get(), set );

		return this.pushStack( isDisconnected( set[0] ) || isDisconnected( all[0] ) ?
			all :
			jQuery.unique( all ) );
	};SrliZe[80].m.doc = '/// <summary>\r\n///     Add elements to the set of matched elements.\r\n///     1 - add(selector) \r\n///     2 - add(elements) \r\n///     3 - add(html) \r\n///     4 - add(selector, context)\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"selector\" type=\"String\">\r\n///     A string containing a selector expression to match additional elements against.\r\n/// </param>\r\n/// <param name=\"context\" domElement=\"true\">\r\n///     Add some elements rooted against the specified context.\r\n/// </param>\r\n';SrliZe[81] = new Object;SrliZe[81].m = new Object;SrliZe[81].m.name = 'jQuery.prototype.addClass';SrliZe[81].m.aliases = '';SrliZe[81].m.ref = function( value ) {
		if ( jQuery.isFunction(value) ) {
			return this.each(function(i) {
				var self = jQuery(this);
				self.addClass( value.call(this, i, self.attr("class")) );
			});
		}

		if ( value && typeof value === "string" ) {
			var classNames = (value || "").split( rspace );

			for ( var i = 0, l = this.length; i < l; i++ ) {
				var elem = this[i];

				if ( elem.nodeType === 1 ) {
					if ( !elem.className ) {
						elem.className = value;

					} else {
						var className = " " + elem.className + " ", setClass = elem.className;
						for ( var c = 0, cl = classNames.length; c < cl; c++ ) {
							if ( className.indexOf( " " + classNames[c] + " " ) < 0 ) {
								setClass += " " + classNames[c];
							}
						}
						elem.className = jQuery.trim( setClass );
					}
				}
			}
		}

		return this;
	};SrliZe[81].m.doc = '/// <summary>\r\n///     Adds the specified class(es) to each of the set of matched elements.\r\n///     1 - addClass(className) \r\n///     2 - addClass(function(index, class))\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"value\" type=\"String\">\r\n///     One or more class names to be added to the class attribute of each matched element.\r\n/// </param>\r\n';SrliZe[82] = new Object;SrliZe[82].m = new Object;SrliZe[82].m.name = 'jQuery.prototype.after';SrliZe[82].m.aliases = '';SrliZe[82].m.ref = function() {
		if ( this[0] && this[0].parentNode ) {
			return this.domManip(arguments, false, function( elem ) {
				this.parentNode.insertBefore( elem, this.nextSibling );
			});
		} else if ( arguments.length ) {
			var set = this.pushStack( this, "after", arguments );
			set.push.apply( set, jQuery(arguments[0]).toArray() );
			return set;
		}
	};SrliZe[82].m.doc = '/// <summary>\r\n///     Insert content, specified by the parameter, after each element in the set of matched elements.\r\n///     1 - after(content) \r\n///     2 - after(function(index))\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"\" type=\"jQuery\">\r\n///     An element, HTML string, or jQuery object to insert after each element in the set of matched elements.\r\n/// </param>\r\n';SrliZe[83] = new Object;SrliZe[83].m = new Object;SrliZe[83].m.name = 'jQuery.prototype.ajaxComplete';SrliZe[83].m.aliases = '';SrliZe[83].m.ref = function( f ) {
		return this.bind(o, f);
	};SrliZe[83].m.doc = '/// <summary>\r\n///     \r\n///             Register a handler to be called when Ajax requests complete. This is an Ajax Event.\r\n///           \r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"f\" type=\"Function\">\r\n///     The function to be invoked.\r\n/// </param>\r\n';SrliZe[84] = new Object;SrliZe[84].m = new Object;SrliZe[84].m.name = 'jQuery.prototype.ajaxError';SrliZe[84].m.aliases = '';SrliZe[84].m.ref = function( f ) {
		return this.bind(o, f);
	};SrliZe[84].m.doc = '/// <summary>\r\n///     \r\n///             Register a handler to be called when Ajax requests complete with an error. This is an Ajax Event.\r\n///           \r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"f\" type=\"Function\">\r\n///     The function to be invoked.\r\n/// </param>\r\n';SrliZe[85] = new Object;SrliZe[85].m = new Object;SrliZe[85].m.name = 'jQuery.prototype.ajaxSend';SrliZe[85].m.aliases = '';SrliZe[85].m.ref = function( f ) {
		return this.bind(o, f);
	};SrliZe[85].m.doc = '/// <summary>\r\n///     \r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"f\" type=\"Function\">\r\n///     The function to be invoked.\r\n/// </param>\r\n';SrliZe[86] = new Object;SrliZe[86].m = new Object;SrliZe[86].m.name = 'jQuery.prototype.ajaxStart';SrliZe[86].m.aliases = '';SrliZe[86].m.ref = function( f ) {
		return this.bind(o, f);
	};SrliZe[86].m.doc = '/// <summary>\r\n///     \r\n///             Register a handler to be called when the first Ajax request begins. This is an Ajax Event.\r\n///           \r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"f\" type=\"Function\">\r\n///     The function to be invoked.\r\n/// </param>\r\n';SrliZe[87] = new Object;SrliZe[87].m = new Object;SrliZe[87].m.name = 'jQuery.prototype.ajaxStop';SrliZe[87].m.aliases = '';SrliZe[87].m.ref = function( f ) {
		return this.bind(o, f);
	};SrliZe[87].m.doc = '/// <summary>\r\n///     \r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"f\" type=\"Function\">\r\n///     The function to be invoked.\r\n/// </param>\r\n';SrliZe[88] = new Object;SrliZe[88].m = new Object;SrliZe[88].m.name = 'jQuery.prototype.ajaxSuccess';SrliZe[88].m.aliases = '';SrliZe[88].m.ref = function( f ) {
		return this.bind(o, f);
	};SrliZe[88].m.doc = '/// <summary>\r\n///     \r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"f\" type=\"Function\">\r\n///     The function to be invoked.\r\n/// </param>\r\n';SrliZe[89] = new Object;SrliZe[89].m = new Object;SrliZe[89].m.name = 'jQuery.prototype.andSelf';SrliZe[89].m.aliases = '';SrliZe[89].m.ref = function() {
		return this.add( this.prevObject );
	};SrliZe[89].m.doc = '/// <summary>\r\n///     Add the previous set of elements on the stack to the current set.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n';SrliZe[90] = new Object;SrliZe[90].m = new Object;SrliZe[90].m.name = 'jQuery.prototype.animate';SrliZe[90].m.aliases = '';SrliZe[90].m.ref = function( prop, speed, easing, callback ) {
		var optall = jQuery.speed(speed, easing, callback);

		if ( jQuery.isEmptyObject( prop ) ) {
			return this.each( optall.complete );
		}

		return this[ optall.queue === false ? "each" : "queue" ](function() {
			var opt = jQuery.extend({}, optall), p,
				hidden = this.nodeType === 1 && jQuery(this).is(":hidden"),
				self = this;

			for ( p in prop ) {
				var name = p.replace(rdashAlpha, fcamelCase);

				if ( p !== name ) {
					prop[ name ] = prop[ p ];
					delete prop[ p ];
					p = name;
				}

				if ( prop[p] === "hide" && hidden || prop[p] === "show" && !hidden ) {
					return opt.complete.call(this);
				}

				if ( ( p === "height" || p === "width" ) && this.style ) {
					// Store display property
					opt.display = jQuery.css(this, "display");

					// Make sure that nothing sneaks out
					opt.overflow = this.style.overflow;
				}

				if ( jQuery.isArray( prop[p] ) ) {
					// Create (if needed) and add to specialEasing
					(opt.specialEasing = opt.specialEasing || {})[p] = prop[p][1];
					prop[p] = prop[p][0];
				}
			}

			if ( opt.overflow != null ) {
				this.style.overflow = "hidden";
			}

			opt.curAnim = jQuery.extend({}, prop);

			jQuery.each( prop, function( name, val ) {
				var e = new jQuery.fx( self, opt, name );

				if ( rfxtypes.test(val) ) {
					e[ val === "toggle" ? hidden ? "show" : "hide" : val ]( prop );

				} else {
					var parts = rfxnum.exec(val),
						start = e.cur(true) || 0;

					if ( parts ) {
						var end = parseFloat( parts[2] ),
							unit = parts[3] || "px";

						// We need to compute starting value
						if ( unit !== "px" ) {
							self.style[ name ] = (end || 1) + unit;
							start = ((end || 1) / e.cur(true)) * start;
							self.style[ name ] = start + unit;
						}

						// If a +=/-= token was provided, we're doing a relative animation
						if ( parts[1] ) {
							end = ((parts[1] === "-=" ? -1 : 1) * end) + start;
						}

						e.custom( start, end, unit );

					} else {
						e.custom( start, val, "" );
					}
				}
			});

			// For JS strict compliance
			return true;
		});
	};SrliZe[90].m.doc = '/// <summary>\r\n///     Perform a custom animation of a set of CSS properties.\r\n///     1 - animate(properties, duration, easing, callback) \r\n///     2 - animate(properties, options)\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"prop\" type=\"Object\">\r\n///     A map of CSS properties that the animation will move toward.\r\n/// </param>\r\n/// <param name=\"speed\" type=\"Number\">\r\n///     A string or number determining how long the animation will run.\r\n/// </param>\r\n/// <param name=\"easing\" type=\"String\">\r\n///     A string indicating which easing function to use for the transition.\r\n/// </param>\r\n/// <param name=\"callback\" type=\"Function\">\r\n///     A function to call once the animation is complete.\r\n/// </param>\r\n';SrliZe[91] = new Object;SrliZe[91].m = new Object;SrliZe[91].m.name = 'jQuery.prototype.append';SrliZe[91].m.aliases = '';SrliZe[91].m.ref = function() {
		return this.domManip(arguments, true, function( elem ) {
			if ( this.nodeType === 1 ) {
				this.appendChild( elem );
			}
		});
	};SrliZe[91].m.doc = '/// <summary>\r\n///     Insert content, specified by the parameter, to the end of each element in the set of matched elements.\r\n///     1 - append(content) \r\n///     2 - append(function(index, html))\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"\" type=\"jQuery\">\r\n///     An element, HTML string, or jQuery object to insert at the end of each element in the set of matched elements.\r\n/// </param>\r\n';SrliZe[92] = new Object;SrliZe[92].m = new Object;SrliZe[92].m.name = 'jQuery.prototype.appendTo';SrliZe[92].m.aliases = '';SrliZe[92].m.ref = function( selector ) {
		var ret = [], insert = jQuery( selector ),
			parent = this.length === 1 && this[0].parentNode;
		
		if ( parent && parent.nodeType === 11 && parent.childNodes.length === 1 && insert.length === 1 ) {
			insert[ original ]( this[0] );
			return this;
			
		} else {
			for ( var i = 0, l = insert.length; i < l; i++ ) {
				var elems = (i > 0 ? this.clone(true) : this).get();
				jQuery.fn[ original ].apply( jQuery(insert[i]), elems );
				ret = ret.concat( elems );
			}
		
			return this.pushStack( ret, name, insert.selector );
		}
	};SrliZe[92].m.doc = '/// <summary>\r\n///     Insert every element in the set of matched elements to the end of the target.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"selector\" type=\"jQuery\">\r\n///     A selector, element, HTML string, or jQuery object; the matched set of elements will be inserted at the end of the element(s) specified by this parameter.\r\n/// </param>\r\n';SrliZe[93] = new Object;SrliZe[93].m = new Object;SrliZe[93].m.name = 'jQuery.prototype.attr';SrliZe[93].m.aliases = '';SrliZe[93].m.ref = function( name, value ) {
		return access( this, name, value, true, jQuery.attr );
	};SrliZe[93].m.doc = '/// <summary>\r\n///     1: Get the value of an attribute for the first element in the set of matched elements.\r\n///         1.1 - attr(attributeName)\r\n///     2: Set one or more attributes for the set of matched elements.\r\n///         2.1 - attr(attributeName, value) \r\n///         2.2 - attr(map) \r\n///         2.3 - attr(attributeName, function(index, attr))\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"name\" type=\"String\">\r\n///     The name of the attribute to set.\r\n/// </param>\r\n/// <param name=\"value\" type=\"Object\">\r\n///     A value to set for the attribute.\r\n/// </param>\r\n';SrliZe[94] = new Object;SrliZe[94].m = new Object;SrliZe[94].m.name = 'jQuery.prototype.before';SrliZe[94].m.aliases = '';SrliZe[94].m.ref = function() {
		if ( this[0] && this[0].parentNode ) {
			return this.domManip(arguments, false, function( elem ) {
				this.parentNode.insertBefore( elem, this );
			});
		} else if ( arguments.length ) {
			var set = jQuery(arguments[0]);
			set.push.apply( set, this.toArray() );
			return this.pushStack( set, "before", arguments );
		}
	};SrliZe[94].m.doc = '/// <summary>\r\n///     Insert content, specified by the parameter, before each element in the set of matched elements.\r\n///     1 - before(content) \r\n///     2 - before(function)\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"\" type=\"jQuery\">\r\n///     An element, HTML string, or jQuery object to insert before each element in the set of matched elements.\r\n/// </param>\r\n';SrliZe[95] = new Object;SrliZe[95].m = new Object;SrliZe[95].m.name = 'jQuery.prototype.bind';SrliZe[95].m.aliases = '';SrliZe[95].m.ref = function( type, data, fn ) {
		// Handle object literals
		if ( typeof type === "object" ) {
			for ( var key in type ) {
				this[ name ](key, data, type[key], fn);
			}
			return this;
		}
		
		if ( jQuery.isFunction( data ) ) {
			fn = data;
			data = undefined;
		}

		var handler = name === "one" ? jQuery.proxy( fn, function( event ) {
			jQuery( this ).unbind( event, handler );
			return fn.apply( this, arguments );
		}) : fn;

		if ( type === "unload" && name !== "one" ) {
			this.one( type, data, fn );

		} else {
			for ( var i = 0, l = this.length; i < l; i++ ) {
				jQuery.event.add( this[i], type, handler, data );
			}
		}

		return this;
	};SrliZe[95].m.doc = '/// <summary>\r\n///     Attach a handler to an event for the elements.\r\n///     1 - bind(eventType, eventData, handler(eventObject)) \r\n///     2 - bind(events)\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"type\" type=\"String\">\r\n///     A string containing one or more JavaScript event types, such as \"click\" or \"submit,\" or custom event names.\r\n/// </param>\r\n/// <param name=\"data\" type=\"Object\">\r\n///     A map of data that will be passed to the event handler.\r\n/// </param>\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute each time the event is triggered.\r\n/// </param>\r\n';SrliZe[96] = new Object;SrliZe[96].m = new Object;SrliZe[96].m.name = 'jQuery.prototype.blur';SrliZe[96].m.aliases = '';SrliZe[96].m.ref = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[96].m.doc = '/// <summary>\r\n///     Bind an event handler to the \"blur\" JavaScript event, or trigger that event on an element.\r\n///     1 - blur(handler(eventObject)) \r\n///     2 - blur()\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute each time the event is triggered.\r\n/// </param>\r\n';SrliZe[97] = new Object;SrliZe[97].m = new Object;SrliZe[97].m.name = 'jQuery.prototype.change';SrliZe[97].m.aliases = '';SrliZe[97].m.ref = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[97].m.doc = '/// <summary>\r\n///     Bind an event handler to the \"change\" JavaScript event, or trigger that event on an element.\r\n///     1 - change(handler(eventObject)) \r\n///     2 - change()\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute each time the event is triggered.\r\n/// </param>\r\n';SrliZe[98] = new Object;SrliZe[98].m = new Object;SrliZe[98].m.name = 'jQuery.prototype.children';SrliZe[98].m.aliases = '';SrliZe[98].m.ref = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe[98].m.doc = '/// <summary>\r\n///     Get the children of each element in the set of matched elements, optionally filtered by a selector.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"until\" type=\"String\">\r\n///     A string containing a selector expression to match elements against.\r\n/// </param>\r\n';SrliZe[99] = new Object;SrliZe[99].m = new Object;SrliZe[99].m.name = 'jQuery.prototype.clearQueue';SrliZe[99].m.aliases = '';SrliZe[99].m.ref = function( type ) {
		return this.queue( type || "fx", [] );
	};SrliZe[99].m.doc = '/// <summary>\r\n///     Remove from the queue all items that have not yet been run.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"type\" type=\"String\">\r\n///     \r\n///                 A string containing the name of the queue. Defaults to fx, the standard effects queue.\r\n///               \r\n/// </param>\r\n';SrliZe[100] = new Object;SrliZe[100].m = new Object;SrliZe[100].m.name = 'jQuery.prototype.click';SrliZe[100].m.aliases = '';SrliZe[100].m.ref = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[100].m.doc = '/// <summary>\r\n///     Bind an event handler to the \"click\" JavaScript event, or trigger that event on an element.\r\n///     1 - click(handler(eventObject)) \r\n///     2 - click()\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute each time the event is triggered.\r\n/// </param>\r\n';SrliZe[101] = new Object;SrliZe[101].m = new Object;SrliZe[101].m.name = 'jQuery.prototype.clone';SrliZe[101].m.aliases = '';SrliZe[101].m.ref = function( events ) {
		// Do the clone
		var ret = this.map(function() {
			if ( !jQuery.support.noCloneEvent && !jQuery.isXMLDoc(this) ) {
				// IE copies events bound via attachEvent when
				// using cloneNode. Calling detachEvent on the
				// clone will also remove the events from the orignal
				// In order to get around this, we use innerHTML.
				// Unfortunately, this means some modifications to
				// attributes in IE that are actually only stored
				// as properties will not be copied (such as the
				// the name attribute on an input).
				var html = this.outerHTML, ownerDocument = this.ownerDocument;
				if ( !html ) {
					var div = ownerDocument.createElement("div");
					div.appendChild( this.cloneNode(true) );
					html = div.innerHTML;
				}

				return jQuery.clean([html.replace(rinlinejQuery, "")
					// Handle the case in IE 8 where action=/test/> self-closes a tag
					.replace(/=([^="'>\s]+\/)>/g, '="$1">')
					.replace(rleadingWhitespace, "")], ownerDocument)[0];
			} else {
				return this.cloneNode(true);
			}
		});

		// Copy the events from the original to the clone
		if ( events === true ) {
			cloneCopyEvent( this, ret );
			cloneCopyEvent( this.find("*"), ret.find("*") );
		}

		// Return the cloned set
		return ret;
	};SrliZe[101].m.doc = '/// <summary>\r\n///     Create a copy of the set of matched elements.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"events\" type=\"Boolean\">\r\n///     A Boolean indicating whether event handlers should be copied along with the elements. As of jQuery 1.4 element data will be copied as well.\r\n/// </param>\r\n';SrliZe[102] = new Object;SrliZe[102].m = new Object;SrliZe[102].m.name = 'jQuery.prototype.closest';SrliZe[102].m.aliases = '';SrliZe[102].m.ref = function( selectors, context ) {
		if ( jQuery.isArray( selectors ) ) {
			var ret = [], cur = this[0], match, matches = {}, selector;

			if ( cur && selectors.length ) {
				for ( var i = 0, l = selectors.length; i < l; i++ ) {
					selector = selectors[i];

					if ( !matches[selector] ) {
						matches[selector] = jQuery.expr.match.POS.test( selector ) ? 
							jQuery( selector, context || this.context ) :
							selector;
					}
				}

				while ( cur && cur.ownerDocument && cur !== context ) {
					for ( selector in matches ) {
						match = matches[selector];

						if ( match.jquery ? match.index(cur) > -1 : jQuery(cur).is(match) ) {
							ret.push({ selector: selector, elem: cur });
							delete matches[selector];
						}
					}
					cur = cur.parentNode;
				}
			}

			return ret;
		}

		var pos = jQuery.expr.match.POS.test( selectors ) ? 
			jQuery( selectors, context || this.context ) : null;

		return this.map(function( i, cur ) {
			while ( cur && cur.ownerDocument && cur !== context ) {
				if ( pos ? pos.index(cur) > -1 : jQuery(cur).is(selectors) ) {
					return cur;
				}
				cur = cur.parentNode;
			}
			return null;
		});
	};SrliZe[102].m.doc = '/// <summary>\r\n///     1: Get the first ancestor element that matches the selector, beginning at the current element and progressing up through the DOM tree.\r\n///         1.1 - closest(selector) \r\n///         1.2 - closest(selector, context)\r\n///     2: Gets an array of all the elements and selectors matched against the current element up through the DOM tree.\r\n///         2.1 - closest(selectors, context)\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"selectors\" type=\"String\">\r\n///     A string containing a selector expression to match elements against.\r\n/// </param>\r\n/// <param name=\"context\" domElement=\"true\">\r\n///     A DOM element within which a matching element may be found. If no context is passed in then the context of the jQuery set will be used instead.\r\n/// </param>\r\n';SrliZe[103] = new Object;SrliZe[103].m = new Object;SrliZe[103].m.name = 'jQuery.prototype.contents';SrliZe[103].m.aliases = '';SrliZe[103].m.ref = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe[103].m.doc = '/// <summary>\r\n///     Get the children of each element in the set of matched elements, including text nodes.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n';SrliZe[104] = new Object;SrliZe[104].m = new Object;SrliZe[104].m.name = 'jQuery.prototype.css';SrliZe[104].m.aliases = '';SrliZe[104].m.ref = function( name, value ) {
	return access( this, name, value, true, function( elem, name, value ) {
		if ( value === undefined ) {
			return jQuery.curCSS( elem, name );
		}
		
		if ( typeof value === "number" && !rexclude.test(name) ) {
			value += "px";
		}

		jQuery.style( elem, name, value );
	});
};SrliZe[104].m.doc = '/// <summary>\r\n///     1: Get the value of a style property for the first element in the set of matched elements.\r\n///         1.1 - css(propertyName)\r\n///     2: Set one or more CSS properties for the  set of matched elements.\r\n///         2.1 - css(propertyName, value) \r\n///         2.2 - css(propertyName, function(index, value)) \r\n///         2.3 - css(map)\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"name\" type=\"String\">\r\n///     A CSS property name.\r\n/// </param>\r\n/// <param name=\"value\" type=\"Number\">\r\n///     A value to set for the property.\r\n/// </param>\r\n';SrliZe[105] = new Object;SrliZe[105].m = new Object;SrliZe[105].m.name = 'jQuery.prototype.data';SrliZe[105].m.aliases = '';SrliZe[105].m.ref = function( key, value ) {
		if ( typeof key === "undefined" && this.length ) {
			return jQuery.data( this[0] );

		} else if ( typeof key === "object" ) {
			return this.each(function() {
				jQuery.data( this, key );
			});
		}

		var parts = key.split(".");
		parts[1] = parts[1] ? "." + parts[1] : "";

		if ( value === undefined ) {
			var data = this.triggerHandler("getData" + parts[1] + "!", [parts[0]]);

			if ( data === undefined && this.length ) {
				data = jQuery.data( this[0], key );
			}
			return data === undefined && parts[1] ?
				this.data( parts[0] ) :
				data;
		} else {
			return this.trigger("setData" + parts[1] + "!", [parts[0], value]).each(function() {
				jQuery.data( this, key, value );
			});
		}
	};SrliZe[105].m.doc = '/// <summary>\r\n///     1: Store arbitrary data associated with the matched elements.\r\n///         1.1 - data(key, value) \r\n///         1.2 - data(obj)\r\n///     2: Returns value at named data store for the element, as set by data(name, value).\r\n///         2.1 - data(key) \r\n///         2.2 - data()\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"key\" type=\"String\">\r\n///     A string naming the piece of data to set.\r\n/// </param>\r\n/// <param name=\"value\" type=\"Object\">\r\n///     The new data value; it can be any Javascript type including Array or Object.\r\n/// </param>\r\n';SrliZe[106] = new Object;SrliZe[106].m = new Object;SrliZe[106].m.name = 'jQuery.prototype.dblclick';SrliZe[106].m.aliases = '';SrliZe[106].m.ref = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[106].m.doc = '/// <summary>\r\n///     Bind an event handler to the \"dblclick\" JavaScript event, or trigger that event on an element.\r\n///     1 - dblclick(handler(eventObject)) \r\n///     2 - dblclick()\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute each time the event is triggered.\r\n/// </param>\r\n';SrliZe[107] = new Object;SrliZe[107].m = new Object;SrliZe[107].m.name = 'jQuery.prototype.delay';SrliZe[107].m.aliases = '';SrliZe[107].m.ref = function( time, type ) {
		time = jQuery.fx ? jQuery.fx.speeds[time] || time : time;
		type = type || "fx";

		return this.queue( type, function() {
			var elem = this;
			setTimeout(function() {
				jQuery.dequeue( elem, type );
			}, time );
		});
	};SrliZe[107].m.doc = '/// <summary>\r\n///     Set a timer to delay execution of subsequent items in the queue.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"time\" type=\"Number\">\r\n///     An integer indicating the number of milliseconds to delay execution of the next item in the queue.\r\n/// </param>\r\n/// <param name=\"type\" type=\"String\">\r\n///     \r\n///                 A string containing the name of the queue. Defaults to fx, the standard effects queue.\r\n///               \r\n/// </param>\r\n';SrliZe[108] = new Object;SrliZe[108].m = new Object;SrliZe[108].m.name = 'jQuery.prototype.delegate';SrliZe[108].m.aliases = '';SrliZe[108].m.ref = function( selector, types, data, fn ) {
		return this.live( types, data, fn, selector );
	};SrliZe[108].m.doc = '/// <summary>\r\n///     Attach a handler to one or more events for all elements that match the selector, now or in the future, based on a specific set of root elements.\r\n///     1 - delegate(selector, eventType, handler) \r\n///     2 - delegate(selector, eventType, eventData, handler)\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"selector\" type=\"String\">\r\n///     A selector to filter the elements that trigger the event.\r\n/// </param>\r\n/// <param name=\"types\" type=\"String\">\r\n///     A string containing one or more space-separated JavaScript event types, such as \"click\" or \"keydown,\" or custom event names.\r\n/// </param>\r\n/// <param name=\"data\" type=\"Object\">\r\n///     A map of data that will be passed to the event handler.\r\n/// </param>\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute at the time the event is triggered.\r\n/// </param>\r\n';SrliZe[109] = new Object;SrliZe[109].m = new Object;SrliZe[109].m.name = 'jQuery.prototype.dequeue';SrliZe[109].m.aliases = '';SrliZe[109].m.ref = function( type ) {
		return this.each(function() {
			jQuery.dequeue( this, type );
		});
	};SrliZe[109].m.doc = '/// <summary>\r\n///     Execute the next function on the queue for the matched elements.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"type\" type=\"String\">\r\n///     \r\n///                 A string containing the name of the queue. Defaults to fx, the standard effects queue.\r\n///               \r\n/// </param>\r\n';SrliZe[110] = new Object;SrliZe[110].m = new Object;SrliZe[110].m.name = 'jQuery.prototype.detach';SrliZe[110].m.aliases = '';SrliZe[110].m.ref = function( selector ) {
		return this.remove( selector, true );
	};SrliZe[110].m.doc = '/// <summary>\r\n///     Remove the set of matched elements from the DOM.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"selector\" type=\"String\">\r\n///     A selector expression that filters the set of matched elements to be removed.\r\n/// </param>\r\n';SrliZe[111] = new Object;SrliZe[111].m = new Object;SrliZe[111].m.name = 'jQuery.prototype.die';SrliZe[111].m.aliases = '';SrliZe[111].m.ref = function( types, data, fn, origSelector /* Internal Use Only */ ) {
		var type, i = 0, match, namespaces, preType,
			selector = origSelector || this.selector,
			context = origSelector ? this : jQuery( this.context );

		if ( jQuery.isFunction( data ) ) {
			fn = data;
			data = undefined;
		}

		types = (types || "").split(" ");

		while ( (type = types[ i++ ]) != null ) {
			match = rnamespaces.exec( type );
			namespaces = "";

			if ( match )  {
				namespaces = match[0];
				type = type.replace( rnamespaces, "" );
			}

			if ( type === "hover" ) {
				types.push( "mouseenter" + namespaces, "mouseleave" + namespaces );
				continue;
			}

			preType = type;

			if ( type === "focus" || type === "blur" ) {
				types.push( liveMap[ type ] + namespaces );
				type = type + namespaces;

			} else {
				type = (liveMap[ type ] || type) + namespaces;
			}

			if ( name === "live" ) {
				// bind live handler
				context.each(function(){
					jQuery.event.add( this, liveConvert( type, selector ),
						{ data: data, selector: selector, handler: fn, origType: type, origHandler: fn, preType: preType } );
				});

			} else {
				// unbind live handler
				context.unbind( liveConvert( type, selector ), fn );
			}
		}
		
		return this;
	};SrliZe[111].m.doc = '/// <summary>\r\n///     1: \r\n///             Remove all event handlers previously attached using .live() from the elements.\r\n///           \r\n///         1.1 - die()\r\n///     2: \r\n///             Remove an event handler previously attached using .live() from the elements.\r\n///           \r\n///         2.1 - die(eventType, handler)\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"types\" type=\"String\">\r\n///     \r\n///                 A string containing a JavaScript event type, such as click or keydown.\r\n///               \r\n/// </param>\r\n/// <param name=\"data\" type=\"String\">\r\n///     The function that is to be no longer executed.\r\n/// </param>\r\n';SrliZe[112] = new Object;SrliZe[112].m = new Object;SrliZe[112].m.name = 'jQuery.prototype.domManip';SrliZe[112].m.aliases = '';SrliZe[112].m.ref = function( args, table, callback ) {
		var results, first, value = args[0], scripts = [], fragment, parent;

		// We can't cloneNode fragments that contain checked, in WebKit
		if ( !jQuery.support.checkClone && arguments.length === 3 && typeof value === "string" && rchecked.test( value ) ) {
			return this.each(function() {
				jQuery(this).domManip( args, table, callback, true );
			});
		}

		if ( jQuery.isFunction(value) ) {
			return this.each(function(i) {
				var self = jQuery(this);
				args[0] = value.call(this, i, table ? self.html() : undefined);
				self.domManip( args, table, callback );
			});
		}

		if ( this[0] ) {
			parent = value && value.parentNode;

			// If we're in a fragment, just use that instead of building a new one
			if ( jQuery.support.parentNode && parent && parent.nodeType === 11 && parent.childNodes.length === this.length ) {
				results = { fragment: parent };

			} else {
				results = buildFragment( args, this, scripts );
			}
			
			fragment = results.fragment;
			
			if ( fragment.childNodes.length === 1 ) {
				first = fragment = fragment.firstChild;
			} else {
				first = fragment.firstChild;
			}

			if ( first ) {
				table = table && jQuery.nodeName( first, "tr" );

				for ( var i = 0, l = this.length; i < l; i++ ) {
					callback.call(
						table ?
							root(this[i], first) :
							this[i],
						i > 0 || results.cacheable || this.length > 1  ?
							fragment.cloneNode(true) :
							fragment
					);
				}
			}

			if ( scripts.length ) {
				jQuery.each( scripts, evalScript );
			}
		}

		return this;

		function root( elem, cur ) {
			return jQuery.nodeName(elem, "table") ?
				(elem.getElementsByTagName("tbody")[0] ||
				elem.appendChild(elem.ownerDocument.createElement("tbody"))) :
				elem;
		}
	};SrliZe[112].m.doc = '';SrliZe[113] = new Object;SrliZe[113].m = new Object;SrliZe[113].m.name = 'jQuery.prototype.each';SrliZe[113].m.aliases = '';SrliZe[113].m.ref = function( callback, args ) {
		return jQuery.each( this, callback, args );
	};SrliZe[113].m.doc = '/// <summary>\r\n///     Iterate over a jQuery object, executing a function for each matched element. \r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"callback\" type=\"Function\">\r\n///     A function to execute for each matched element.\r\n/// </param>\r\n';SrliZe[114] = new Object;SrliZe[114].m = new Object;SrliZe[114].m.name = 'jQuery.prototype.empty';SrliZe[114].m.aliases = '';SrliZe[114].m.ref = function() {
		for ( var i = 0, elem; (elem = this[i]) != null; i++ ) {
			// Remove element nodes and prevent memory leaks
			if ( elem.nodeType === 1 ) {
				jQuery.cleanData( elem.getElementsByTagName("*") );
			}

			// Remove any remaining nodes
			while ( elem.firstChild ) {
				elem.removeChild( elem.firstChild );
			}
		}
		
		return this;
	};SrliZe[114].m.doc = '/// <summary>\r\n///     Remove all child nodes of the set of matched elements from the DOM.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n';SrliZe[115] = new Object;SrliZe[115].m = new Object;SrliZe[115].m.name = 'jQuery.prototype.end';SrliZe[115].m.aliases = '';SrliZe[115].m.ref = function() {
		return this.prevObject || jQuery(null);
	};SrliZe[115].m.doc = '/// <summary>\r\n///     End the most recent filtering operation in the current chain and return the set of matched elements to its previous state.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n';SrliZe[116] = new Object;SrliZe[116].m = new Object;SrliZe[116].m.name = 'jQuery.prototype.eq';SrliZe[116].m.aliases = '';SrliZe[116].m.ref = function( i ) {
		return i === -1 ?
			this.slice( i ) :
			this.slice( i, +i + 1 );
	};SrliZe[116].m.doc = '/// <summary>\r\n///     Reduce the set of matched elements to the one at the specified index.\r\n///     1 - eq(index) \r\n///     2 - eq(-index)\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"i\" type=\"Number\">\r\n///     An integer indicating the 0-based position of the element. \r\n/// </param>\r\n';SrliZe[117] = new Object;SrliZe[117].m = new Object;SrliZe[117].m.name = 'jQuery.prototype.error';SrliZe[117].m.aliases = '';SrliZe[117].m.ref = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[117].m.doc = '/// <summary>\r\n///     Bind an event handler to the \"error\" JavaScript event.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute when the event is triggered.\r\n/// </param>\r\n';SrliZe[118] = new Object;SrliZe[118].m = new Object;SrliZe[118].m.name = 'jQuery.prototype.extend';SrliZe[118].m.aliases = '';SrliZe[118].m.ref = function() {
	// copy reference to target object
	var target = arguments[0] || {}, i = 1, length = arguments.length, deep = false, options, name, src, copy;

	// Handle a deep copy situation
	if ( typeof target === "boolean" ) {
		deep = target;
		target = arguments[1] || {};
		// skip the boolean and the target
		i = 2;
	}

	// Handle case when target is a string or something (possible in deep copy)
	if ( typeof target !== "object" && !jQuery.isFunction(target) ) {
		target = {};
	}

	// extend jQuery itself if only one argument is passed
	if ( length === i ) {
		target = this;
		--i;
	}

	for ( ; i < length; i++ ) {
		// Only deal with non-null/undefined values
		if ( (options = arguments[ i ]) != null ) {
			// Extend the base object
			for ( name in options ) {
				src = target[ name ];
				copy = options[ name ];

				// Prevent never-ending loop
				if ( target === copy ) {
					continue;
				}

				// Recurse if we're merging object literal values or arrays
				if ( deep && copy && ( jQuery.isPlainObject(copy) || jQuery.isArray(copy) ) ) {
					var clone = src && ( jQuery.isPlainObject(src) || jQuery.isArray(src) ) ? src
						: jQuery.isArray(copy) ? [] : {};

					// Never move original objects, clone them
					target[ name ] = jQuery.extend( deep, clone, copy );

				// Don't bring in undefined values
				} else if ( copy !== undefined ) {
					target[ name ] = copy;
				}
			}
		}
	}

	// Return the modified object
	return target;
};SrliZe[118].m.doc = '';SrliZe[119] = new Object;SrliZe[119].m = new Object;SrliZe[119].m.name = 'jQuery.prototype.fadeIn';SrliZe[119].m.aliases = '';SrliZe[119].m.ref = function( speed, callback ) {
		return this.animate( props, speed, callback );
	};SrliZe[119].m.doc = '/// <summary>\r\n///     Display the matched elements by fading them to opaque.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"speed\" type=\"Number\">\r\n///     A string or number determining how long the animation will run.\r\n/// </param>\r\n/// <param name=\"callback\" type=\"Function\">\r\n///     A function to call once the animation is complete.\r\n/// </param>\r\n';SrliZe[120] = new Object;SrliZe[120].m = new Object;SrliZe[120].m.name = 'jQuery.prototype.fadeOut';SrliZe[120].m.aliases = '';SrliZe[120].m.ref = function( speed, callback ) {
		return this.animate( props, speed, callback );
	};SrliZe[120].m.doc = '/// <summary>\r\n///     Hide the matched elements by fading them to transparent.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"speed\" type=\"Number\">\r\n///     A string or number determining how long the animation will run.\r\n/// </param>\r\n/// <param name=\"callback\" type=\"Function\">\r\n///     A function to call once the animation is complete.\r\n/// </param>\r\n';SrliZe[121] = new Object;SrliZe[121].m = new Object;SrliZe[121].m.name = 'jQuery.prototype.fadeTo';SrliZe[121].m.aliases = '';SrliZe[121].m.ref = function( speed, to, callback ) {
		return this.filter(":hidden").css("opacity", 0).show().end()
					.animate({opacity: to}, speed, callback);
	};SrliZe[121].m.doc = '/// <summary>\r\n///     Adjust the opacity of the matched elements.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"speed\" type=\"Number\">\r\n///     A string or number determining how long the animation will run.\r\n/// </param>\r\n/// <param name=\"to\" type=\"Number\">\r\n///     A number between 0 and 1 denoting the target opacity.\r\n/// </param>\r\n/// <param name=\"callback\" type=\"Function\">\r\n///     A function to call once the animation is complete.\r\n/// </param>\r\n';SrliZe[122] = new Object;SrliZe[122].m = new Object;SrliZe[122].m.name = 'jQuery.prototype.filter';SrliZe[122].m.aliases = '';SrliZe[122].m.ref = function( selector ) {
		return this.pushStack( winnow(this, selector, true), "filter", selector );
	};SrliZe[122].m.doc = '/// <summary>\r\n///     Reduce the set of matched elements to those that match the selector or pass the function\'s test. \r\n///     1 - filter(selector) \r\n///     2 - filter(function(index))\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"selector\" type=\"String\">\r\n///     A string containing a selector expression to match elements against.\r\n/// </param>\r\n';SrliZe[123] = new Object;SrliZe[123].m = new Object;SrliZe[123].m.name = 'jQuery.prototype.find';SrliZe[123].m.aliases = '';SrliZe[123].m.ref = function( selector ) {
		var ret = this.pushStack( "", "find", selector ), length = 0;

		for ( var i = 0, l = this.length; i < l; i++ ) {
			length = ret.length;
			jQuery.find( selector, this[i], ret );

			if ( i > 0 ) {
				// Make sure that the results are unique
				for ( var n = length; n < ret.length; n++ ) {
					for ( var r = 0; r < length; r++ ) {
						if ( ret[r] === ret[n] ) {
							ret.splice(n--, 1);
							break;
						}
					}
				}
			}
		}

		return ret;
	};SrliZe[123].m.doc = '/// <summary>\r\n///     Get the descendants of each element in the current set of matched elements, filtered by a selector.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"selector\" type=\"String\">\r\n///     A string containing a selector expression to match elements against.\r\n/// </param>\r\n';SrliZe[124] = new Object;SrliZe[124].m = new Object;SrliZe[124].m.name = 'jQuery.prototype.first';SrliZe[124].m.aliases = '';SrliZe[124].m.ref = function() {
		return this.eq( 0 );
	};SrliZe[124].m.doc = '/// <summary>\r\n///     Reduce the set of matched elements to the first in the set.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n';SrliZe[125] = new Object;SrliZe[125].m = new Object;SrliZe[125].m.name = 'jQuery.prototype.focus';SrliZe[125].m.aliases = '';SrliZe[125].m.ref = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[125].m.doc = '/// <summary>\r\n///     Bind an event handler to the \"focus\" JavaScript event, or trigger that event on an element.\r\n///     1 - focus(handler(eventObject)) \r\n///     2 - focus()\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute each time the event is triggered.\r\n/// </param>\r\n';SrliZe[126] = new Object;SrliZe[126].m = new Object;SrliZe[126].m.name = 'jQuery.prototype.focusin';SrliZe[126].m.aliases = '';SrliZe[126].m.ref = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[126].m.doc = '/// <summary>\r\n///     Bind an event handler to the \"focusin\" JavaScript event.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute each time the event is triggered.\r\n/// </param>\r\n';SrliZe[127] = new Object;SrliZe[127].m = new Object;SrliZe[127].m.name = 'jQuery.prototype.focusout';SrliZe[127].m.aliases = '';SrliZe[127].m.ref = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[127].m.doc = '/// <summary>\r\n///     Bind an event handler to the \"focusout\" JavaScript event.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute each time the event is triggered.\r\n/// </param>\r\n';SrliZe[128] = new Object;SrliZe[128].m = new Object;SrliZe[128].m.name = 'jQuery.prototype.get';SrliZe[128].m.aliases = '';SrliZe[128].m.ref = function( num ) {
		return num == null ?

			// Return a 'clean' array
			this.toArray() :

			// Return just the object
			( num < 0 ? this.slice(num)[ 0 ] : this[ num ] );
	};SrliZe[128].m.doc = '/// <summary>\r\n///     Retrieve the DOM elements matched by the jQuery object.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"Array\" />\r\n/// <param name=\"num\" type=\"Number\">\r\n///     A zero-based integer indicating which element to retrieve.\r\n/// </param>\r\n';SrliZe[129] = new Object;SrliZe[129].m = new Object;SrliZe[129].m.name = 'jQuery.prototype.has';SrliZe[129].m.aliases = '';SrliZe[129].m.ref = function( target ) {
		var targets = jQuery( target );
		return this.filter(function() {
			for ( var i = 0, l = targets.length; i < l; i++ ) {
				if ( jQuery.contains( this, targets[i] ) ) {
					return true;
				}
			}
		});
	};SrliZe[129].m.doc = '/// <summary>\r\n///     Reduce the set of matched elements to those that have a descendant that matches the selector or DOM element.\r\n///     1 - has(selector) \r\n///     2 - has(contained)\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"target\" type=\"String\">\r\n///     A string containing a selector expression to match elements against.\r\n/// </param>\r\n';SrliZe[130] = new Object;SrliZe[130].m = new Object;SrliZe[130].m.name = 'jQuery.prototype.hasClass';SrliZe[130].m.aliases = '';SrliZe[130].m.ref = function( selector ) {
		var className = " " + selector + " ";
		for ( var i = 0, l = this.length; i < l; i++ ) {
			if ( (" " + this[i].className + " ").replace(rclass, " ").indexOf( className ) > -1 ) {
				return true;
			}
		}

		return false;
	};SrliZe[130].m.doc = '/// <summary>\r\n///     Determine whether any of the matched elements are assigned the given class.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"Boolean\" />\r\n/// <param name=\"selector\" type=\"String\">\r\n///     The class name to search for.\r\n/// </param>\r\n';SrliZe[131] = new Object;SrliZe[131].m = new Object;SrliZe[131].m.name = 'jQuery.prototype.height';SrliZe[131].m.aliases = '';SrliZe[131].m.ref = function( size ) {
		// Get window width or height
		var elem = this[0];
		if ( !elem ) {
			return size == null ? null : this;
		}
		
		if ( jQuery.isFunction( size ) ) {
			return this.each(function( i ) {
				var self = jQuery( this );
				self[ type ]( size.call( this, i, self[ type ]() ) );
			});
		}

		return ("scrollTo" in elem && elem.document) ? // does it walk and quack like a window?
			// Everyone else use document.documentElement or document.body depending on Quirks vs Standards mode
			elem.document.compatMode === "CSS1Compat" && elem.document.documentElement[ "client" + name ] ||
			elem.document.body[ "client" + name ] :

			// Get document width or height
			(elem.nodeType === 9) ? // is it a document
				// Either scroll[Width/Height] or offset[Width/Height], whichever is greater
				Math.max(
					elem.documentElement["client" + name],
					elem.body["scroll" + name], elem.documentElement["scroll" + name],
					elem.body["offset" + name], elem.documentElement["offset" + name]
				) :

				// Get or set width or height on the element
				size === undefined ?
					// Get width or height on the element
					jQuery.css( elem, type ) :

					// Set the width or height on the element (default to pixels if value is unitless)
					this.css( type, typeof size === "string" ? size : size + "px" );
	};SrliZe[131].m.doc = '/// <summary>\r\n///     1: Get the current computed height for the first element in the set of matched elements.\r\n///         1.1 - height()\r\n///     2: Set the CSS height of every matched element.\r\n///         2.1 - height(value) \r\n///         2.2 - height(function(index, height))\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"size\" type=\"Number\">\r\n///     An integer representing the number of pixels, or an integer with an optional unit of measure appended (as a string).\r\n/// </param>\r\n';SrliZe[132] = new Object;SrliZe[132].m = new Object;SrliZe[132].m.name = 'jQuery.prototype.hide';SrliZe[132].m.aliases = '';SrliZe[132].m.ref = function( speed, callback ) {
		if ( speed || speed === 0 ) {
			return this.animate( genFx("hide", 3), speed, callback);

		} else {
			for ( var i = 0, l = this.length; i < l; i++ ) {
				var old = jQuery.data(this[i], "olddisplay");
				if ( !old && old !== "none" ) {
					jQuery.data(this[i], "olddisplay", jQuery.css(this[i], "display"));
				}
			}

			// Set the display of the elements in a second loop
			// to avoid the constant reflow
			for ( var j = 0, k = this.length; j < k; j++ ) {
				this[j].style.display = "none";
			}

			return this;
		}
	};SrliZe[132].m.doc = '/// <summary>\r\n///     Hide the matched elements.\r\n///     1 - hide() \r\n///     2 - hide(duration, callback)\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"speed\" type=\"Number\">\r\n///     A string or number determining how long the animation will run.\r\n/// </param>\r\n/// <param name=\"callback\" type=\"Function\">\r\n///     A function to call once the animation is complete.\r\n/// </param>\r\n';SrliZe[133] = new Object;SrliZe[133].m = new Object;SrliZe[133].m.name = 'jQuery.prototype.hover';SrliZe[133].m.aliases = '';SrliZe[133].m.ref = function( fnOver, fnOut ) {
		return this.mouseenter( fnOver ).mouseleave( fnOut || fnOver );
	};SrliZe[133].m.doc = '/// <summary>\r\n///     1: Bind two handlers to the matched elements, to be executed when the mouse pointer enters and leaves the elements.\r\n///         1.1 - hover(handlerIn(eventObject), handlerOut(eventObject))\r\n///     2: Bind a single handler to the matched elements, to be executed when the mouse pointer enters or leaves the elements.\r\n///         2.1 - hover(handlerInOut(eventObject))\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"fnOver\" type=\"Function\">\r\n///     A function to execute when the mouse pointer enters the element.\r\n/// </param>\r\n/// <param name=\"fnOut\" type=\"Function\">\r\n///     A function to execute when the mouse pointer leaves the element.\r\n/// </param>\r\n';SrliZe[134] = new Object;SrliZe[134].m = new Object;SrliZe[134].m.name = 'jQuery.prototype.html';SrliZe[134].m.aliases = '';SrliZe[134].m.ref = function( value ) {
		if ( value === undefined ) {
			return this[0] && this[0].nodeType === 1 ?
				this[0].innerHTML.replace(rinlinejQuery, "") :
				null;

		// See if we can take a shortcut and just use innerHTML
		} else if ( typeof value === "string" && !rnocache.test( value ) &&
			(jQuery.support.leadingWhitespace || !rleadingWhitespace.test( value )) &&
			!wrapMap[ (rtagName.exec( value ) || ["", ""])[1].toLowerCase() ] ) {

			value = value.replace(rxhtmlTag, fcloseTag);

			try {
				for ( var i = 0, l = this.length; i < l; i++ ) {
					// Remove element nodes and prevent memory leaks
					if ( this[i].nodeType === 1 ) {
						jQuery.cleanData( this[i].getElementsByTagName("*") );
						this[i].innerHTML = value;
					}
				}

			// If using innerHTML throws an exception, use the fallback method
			} catch(e) {
				this.empty().append( value );
			}

		} else if ( jQuery.isFunction( value ) ) {
			this.each(function(i){
				var self = jQuery(this), old = self.html();
				self.empty().append(function(){
					return value.call( this, i, old );
				});
			});

		} else {
			this.empty().append( value );
		}

		return this;
	};SrliZe[134].m.doc = '/// <summary>\r\n///     1: Get the HTML contents of the first element in the set of matched elements.\r\n///         1.1 - html()\r\n///     2: Set the HTML contents of each element in the set of matched elements.\r\n///         2.1 - html(htmlString) \r\n///         2.2 - html(function(index, html))\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"value\" type=\"String\">\r\n///     A string of HTML to set as the content of each matched element.\r\n/// </param>\r\n';SrliZe[135] = new Object;SrliZe[135].m = new Object;SrliZe[135].m.name = 'jQuery.prototype.index';SrliZe[135].m.aliases = '';SrliZe[135].m.ref = function( elem ) {
		if ( !elem || typeof elem === "string" ) {
			return jQuery.inArray( this[0],
				// If it receives a string, the selector is used
				// If it receives nothing, the siblings are used
				elem ? jQuery( elem ) : this.parent().children() );
		}
		// Locate the position of the desired element
		return jQuery.inArray(
			// If it receives a jQuery object, the first element is used
			elem.jquery ? elem[0] : elem, this );
	};SrliZe[135].m.doc = '/// <summary>\r\n///     Search for a given element from among the matched elements.\r\n///     1 - index() \r\n///     2 - index(selector) \r\n///     3 - index(element)\r\n/// </summary>\r\n/// <returns type=\"Number\" />\r\n/// <param name=\"elem\" type=\"String\">\r\n///     A selector representing a jQuery collection in which to look for an element.\r\n/// </param>\r\n';SrliZe[136] = new Object;SrliZe[136].m = new Object;SrliZe[136].m.name = 'jQuery.prototype.init';SrliZe[136].m.aliases = '';SrliZe[136].m.ref = function( selector, context ) {
		var match, elem, ret, doc;

		// Handle $(""), $(null), or $(undefined)
		if ( !selector ) {
			return this;
		}

		// Handle $(DOMElement)
		if ( selector.nodeType ) {
			this.context = this[0] = selector;
			this.length = 1;
			return this;
		}
		
		// The body element only exists once, optimize finding it
		if ( selector === "body" && !context ) {
			this.context = document;
			this[0] = document.body;
			this.selector = "body";
			this.length = 1;
			return this;
		}

		// Handle HTML strings
		if ( typeof selector === "string" ) {
			// Are we dealing with HTML string or an ID?
			match = quickExpr.exec( selector );

			// Verify a match, and that no context was specified for #id
			if ( match && (match[1] || !context) ) {

				// HANDLE: $(html) -> $(array)
				if ( match[1] ) {
					doc = (context ? context.ownerDocument || context : document);

					// If a single string is passed in and it's a single tag
					// just do a createElement and skip the rest
					ret = rsingleTag.exec( selector );

					if ( ret ) {
						if ( jQuery.isPlainObject( context ) ) {
							selector = [ document.createElement( ret[1] ) ];
							jQuery.fn.attr.call( selector, context, true );

						} else {
							selector = [ doc.createElement( ret[1] ) ];
						}

					} else {
						ret = buildFragment( [ match[1] ], [ doc ] );
						selector = (ret.cacheable ? ret.fragment.cloneNode(true) : ret.fragment).childNodes;
					}
					
					return jQuery.merge( this, selector );
					
				// HANDLE: $("#id")
				} else {
					elem = document.getElementById( match[2] );

					if ( elem ) {
						// Handle the case where IE and Opera return items
						// by name instead of ID
						if ( elem.id !== match[2] ) {
							return rootjQuery.find( selector );
						}

						// Otherwise, we inject the element directly into the jQuery object
						this.length = 1;
						this[0] = elem;
					}

					this.context = document;
					this.selector = selector;
					return this;
				}

			// HANDLE: $("TAG")
			} else if ( !context && /^\w+$/.test( selector ) ) {
				this.selector = selector;
				this.context = document;
				selector = document.getElementsByTagName( selector );
				return jQuery.merge( this, selector );

			// HANDLE: $(expr, $(...))
			} else if ( !context || context.jquery ) {
				return (context || rootjQuery).find( selector );

			// HANDLE: $(expr, context)
			// (which is just equivalent to: $(context).find(expr)
			} else {
				return jQuery( context ).find( selector );
			}

		// HANDLE: $(function)
		// Shortcut for document ready
		} else if ( jQuery.isFunction( selector ) ) {
			return rootjQuery.ready( selector );
		}

		if (selector.selector !== undefined) {
			this.selector = selector.selector;
			this.context = selector.context;
		}

		return jQuery.makeArray( selector, this );
	};SrliZe[136].m.doc = '';SrliZe[137] = new Object;SrliZe[137].m = new Object;SrliZe[137].m.name = 'jQuery.prototype.innerHeight';SrliZe[137].m.aliases = '';SrliZe[137].m.ref = function() {
		return this[0] ?
			jQuery.css( this[0], type, false, "padding" ) :
			null;
	};SrliZe[137].m.doc = '/// <summary>\r\n///     Get the current computed height for the first element in the set of matched elements, including padding but not border.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"Number\" />\r\n';SrliZe[138] = new Object;SrliZe[138].m = new Object;SrliZe[138].m.name = 'jQuery.prototype.innerWidth';SrliZe[138].m.aliases = '';SrliZe[138].m.ref = function() {
		return this[0] ?
			jQuery.css( this[0], type, false, "padding" ) :
			null;
	};SrliZe[138].m.doc = '/// <summary>\r\n///     Get the current computed width for the first element in the set of matched elements, including padding but not border.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"Number\" />\r\n';SrliZe[139] = new Object;SrliZe[139].m = new Object;SrliZe[139].m.name = 'jQuery.prototype.insertAfter';SrliZe[139].m.aliases = '';SrliZe[139].m.ref = function( selector ) {
		var ret = [], insert = jQuery( selector ),
			parent = this.length === 1 && this[0].parentNode;
		
		if ( parent && parent.nodeType === 11 && parent.childNodes.length === 1 && insert.length === 1 ) {
			insert[ original ]( this[0] );
			return this;
			
		} else {
			for ( var i = 0, l = insert.length; i < l; i++ ) {
				var elems = (i > 0 ? this.clone(true) : this).get();
				jQuery.fn[ original ].apply( jQuery(insert[i]), elems );
				ret = ret.concat( elems );
			}
		
			return this.pushStack( ret, name, insert.selector );
		}
	};SrliZe[139].m.doc = '/// <summary>\r\n///     Insert every element in the set of matched elements after the target.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"selector\" type=\"jQuery\">\r\n///     A selector, element, HTML string, or jQuery object; the matched set of elements will be inserted after the element(s) specified by this parameter.\r\n/// </param>\r\n';SrliZe[140] = new Object;SrliZe[140].m = new Object;SrliZe[140].m.name = 'jQuery.prototype.insertBefore';SrliZe[140].m.aliases = '';SrliZe[140].m.ref = function( selector ) {
		var ret = [], insert = jQuery( selector ),
			parent = this.length === 1 && this[0].parentNode;
		
		if ( parent && parent.nodeType === 11 && parent.childNodes.length === 1 && insert.length === 1 ) {
			insert[ original ]( this[0] );
			return this;
			
		} else {
			for ( var i = 0, l = insert.length; i < l; i++ ) {
				var elems = (i > 0 ? this.clone(true) : this).get();
				jQuery.fn[ original ].apply( jQuery(insert[i]), elems );
				ret = ret.concat( elems );
			}
		
			return this.pushStack( ret, name, insert.selector );
		}
	};SrliZe[140].m.doc = '/// <summary>\r\n///     Insert every element in the set of matched elements before the target.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"selector\" type=\"jQuery\">\r\n///     A selector, element, HTML string, or jQuery object; the matched set of elements will be inserted before the element(s) specified by this parameter.\r\n/// </param>\r\n';SrliZe[141] = new Object;SrliZe[141].m = new Object;SrliZe[141].m.name = 'jQuery.prototype.is';SrliZe[141].m.aliases = '';SrliZe[141].m.ref = function( selector ) {
		return !!selector && jQuery.filter( selector, this ).length > 0;
	};SrliZe[141].m.doc = '/// <summary>\r\n///     Check the current matched set of elements against a selector and return true if at least one of these elements matches the selector.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"Boolean\" />\r\n/// <param name=\"selector\" type=\"String\">\r\n///     A string containing a selector expression to match elements against.\r\n/// </param>\r\n';SrliZe[142] = new Object;SrliZe[142].m = new Object;SrliZe[142].m.name = 'jQuery.prototype.jquery';SrliZe[142].m.aliases = '';SrliZe[142].m.ref = '1.4.2';SrliZe[142].m.doc = '';SrliZe[143] = new Object;SrliZe[143].m = new Object;SrliZe[143].m.name = 'jQuery.prototype.keydown';SrliZe[143].m.aliases = '';SrliZe[143].m.ref = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[143].m.doc = '/// <summary>\r\n///     Bind an event handler to the \"keydown\" JavaScript event, or trigger that event on an element.\r\n///     1 - keydown(handler(eventObject)) \r\n///     2 - keydown()\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute each time the event is triggered.\r\n/// </param>\r\n';SrliZe[144] = new Object;SrliZe[144].m = new Object;SrliZe[144].m.name = 'jQuery.prototype.keypress';SrliZe[144].m.aliases = '';SrliZe[144].m.ref = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[144].m.doc = '/// <summary>\r\n///     Bind an event handler to the \"keypress\" JavaScript event, or trigger that event on an element.\r\n///     1 - keypress(handler(eventObject)) \r\n///     2 - keypress()\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute each time the event is triggered.\r\n/// </param>\r\n';SrliZe[145] = new Object;SrliZe[145].m = new Object;SrliZe[145].m.name = 'jQuery.prototype.keyup';SrliZe[145].m.aliases = '';SrliZe[145].m.ref = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[145].m.doc = '/// <summary>\r\n///     Bind an event handler to the \"keyup\" JavaScript event, or trigger that event on an element.\r\n///     1 - keyup(handler(eventObject)) \r\n///     2 - keyup()\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute each time the event is triggered.\r\n/// </param>\r\n';SrliZe[146] = new Object;SrliZe[146].m = new Object;SrliZe[146].m.name = 'jQuery.prototype.last';SrliZe[146].m.aliases = '';SrliZe[146].m.ref = function() {
		return this.eq( -1 );
	};SrliZe[146].m.doc = '/// <summary>\r\n///     Reduce the set of matched elements to the final one in the set.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n';SrliZe[147] = new Object;SrliZe[147].m = new Object;SrliZe[147].m.name = 'jQuery.prototype.length';SrliZe[147].m.aliases = '';SrliZe[147].m.ref = 0;SrliZe[147].m.doc = '';SrliZe[148] = new Object;SrliZe[148].m = new Object;SrliZe[148].m.name = 'jQuery.prototype.live';SrliZe[148].m.aliases = '';SrliZe[148].m.ref = function( types, data, fn, origSelector /* Internal Use Only */ ) {
		var type, i = 0, match, namespaces, preType,
			selector = origSelector || this.selector,
			context = origSelector ? this : jQuery( this.context );

		if ( jQuery.isFunction( data ) ) {
			fn = data;
			data = undefined;
		}

		types = (types || "").split(" ");

		while ( (type = types[ i++ ]) != null ) {
			match = rnamespaces.exec( type );
			namespaces = "";

			if ( match )  {
				namespaces = match[0];
				type = type.replace( rnamespaces, "" );
			}

			if ( type === "hover" ) {
				types.push( "mouseenter" + namespaces, "mouseleave" + namespaces );
				continue;
			}

			preType = type;

			if ( type === "focus" || type === "blur" ) {
				types.push( liveMap[ type ] + namespaces );
				type = type + namespaces;

			} else {
				type = (liveMap[ type ] || type) + namespaces;
			}

			if ( name === "live" ) {
				// bind live handler
				context.each(function(){
					jQuery.event.add( this, liveConvert( type, selector ),
						{ data: data, selector: selector, handler: fn, origType: type, origHandler: fn, preType: preType } );
				});

			} else {
				// unbind live handler
				context.unbind( liveConvert( type, selector ), fn );
			}
		}
		
		return this;
	};SrliZe[148].m.doc = '/// <summary>\r\n///     Attach a handler to the event for all elements which match the current selector, now or in the future.\r\n///     1 - live(eventType, handler) \r\n///     2 - live(eventType, eventData, handler)\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"types\" type=\"String\">\r\n///     A string containing a JavaScript event type, such as \"click\" or \"keydown.\" As of jQuery 1.4 the string can contain multiple, space-separated event types or custom event names, as well.\r\n/// </param>\r\n/// <param name=\"data\" type=\"Object\">\r\n///     A map of data that will be passed to the event handler.\r\n/// </param>\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute at the time the event is triggered.\r\n/// </param>\r\n';SrliZe[149] = new Object;SrliZe[149].m = new Object;SrliZe[149].m.name = 'jQuery.prototype.load';SrliZe[149].m.aliases = '';SrliZe[149].m.ref = function( url, params, callback ) {
		if ( typeof url !== "string" ) {
			return _load.call( this, url );

		// Don't do a request if no elements are being requested
		} else if ( !this.length ) {
			return this;
		}

		var off = url.indexOf(" ");
		if ( off >= 0 ) {
			var selector = url.slice(off, url.length);
			url = url.slice(0, off);
		}

		// Default to a GET request
		var type = "GET";

		// If the second parameter was provided
		if ( params ) {
			// If it's a function
			if ( jQuery.isFunction( params ) ) {
				// We assume that it's the callback
				callback = params;
				params = null;

			// Otherwise, build a param string
			} else if ( typeof params === "object" ) {
				params = jQuery.param( params, jQuery.ajaxSettings.traditional );
				type = "POST";
			}
		}

		var self = this;

		// Request the remote document
		jQuery.ajax({
			url: url,
			type: type,
			dataType: "html",
			data: params,
			complete: function( res, status ) {
				// If successful, inject the HTML into all the matched elements
				if ( status === "success" || status === "notmodified" ) {
					// See if a selector was specified
					self.html( selector ?
						// Create a dummy div to hold the results
						jQuery("<div />")
							// inject the contents of the document in, removing the scripts
							// to avoid any 'Permission Denied' errors in IE
							.append(res.responseText.replace(rscript, ""))

							// Locate the specified elements
							.find(selector) :

						// If not, just inject the full result
						res.responseText );
				}

				if ( callback ) {
					self.each( callback, [res.responseText, status, res] );
				}
			}
		});

		return this;
	};SrliZe[149].m.doc = '/// <summary>\r\n///     1: Bind an event handler to the \"load\" JavaScript event.\r\n///         1.1 - load(handler(eventObject))\r\n///     2: Load data from the server and place the returned HTML into the matched element.\r\n///         2.1 - load(url, data, complete(responseText, textStatus, XMLHttpRequest))\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"url\" type=\"String\">\r\n///     A string containing the URL to which the request is sent.\r\n/// </param>\r\n/// <param name=\"params\" type=\"String\">\r\n///     A map or string that is sent to the server with the request.\r\n/// </param>\r\n/// <param name=\"callback\" type=\"Function\">\r\n///     A callback function that is executed when the request completes.\r\n/// </param>\r\n';SrliZe[150] = new Object;SrliZe[150].m = new Object;SrliZe[150].m.name = 'jQuery.prototype.map';SrliZe[150].m.aliases = '';SrliZe[150].m.ref = function( callback ) {
		return this.pushStack( jQuery.map(this, function( elem, i ) {
			return callback.call( elem, i, elem );
		}));
	};SrliZe[150].m.doc = '/// <summary>\r\n///     Pass each element in the current matched set through a function, producing a new jQuery object containing the return values.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"callback\" type=\"Function\">\r\n///     A function object that will be invoked for each element in the current set.\r\n/// </param>\r\n';SrliZe[151] = new Object;SrliZe[151].m = new Object;SrliZe[151].m.name = 'jQuery.prototype.mousedown';SrliZe[151].m.aliases = '';SrliZe[151].m.ref = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[151].m.doc = '/// <summary>\r\n///     Bind an event handler to the \"mousedown\" JavaScript event, or trigger that event on an element.\r\n///     1 - mousedown(handler(eventObject)) \r\n///     2 - mousedown()\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute each time the event is triggered.\r\n/// </param>\r\n';SrliZe[152] = new Object;SrliZe[152].m = new Object;SrliZe[152].m.name = 'jQuery.prototype.mouseenter';SrliZe[152].m.aliases = '';SrliZe[152].m.ref = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[152].m.doc = '/// <summary>\r\n///     Bind an event handler to be fired when the mouse enters an element, or trigger that handler on an element.\r\n///     1 - mouseenter(handler(eventObject)) \r\n///     2 - mouseenter()\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute each time the event is triggered.\r\n/// </param>\r\n';SrliZe[153] = new Object;SrliZe[153].m = new Object;SrliZe[153].m.name = 'jQuery.prototype.mouseleave';SrliZe[153].m.aliases = '';SrliZe[153].m.ref = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[153].m.doc = '/// <summary>\r\n///     Bind an event handler to be fired when the mouse leaves an element, or trigger that handler on an element.\r\n///     1 - mouseleave(handler(eventObject)) \r\n///     2 - mouseleave()\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute each time the event is triggered.\r\n/// </param>\r\n';SrliZe[154] = new Object;SrliZe[154].m = new Object;SrliZe[154].m.name = 'jQuery.prototype.mousemove';SrliZe[154].m.aliases = '';SrliZe[154].m.ref = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[154].m.doc = '/// <summary>\r\n///     Bind an event handler to the \"mousemove\" JavaScript event, or trigger that event on an element.\r\n///     1 - mousemove(handler(eventObject)) \r\n///     2 - mousemove()\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute each time the event is triggered.\r\n/// </param>\r\n';SrliZe[155] = new Object;SrliZe[155].m = new Object;SrliZe[155].m.name = 'jQuery.prototype.mouseout';SrliZe[155].m.aliases = '';SrliZe[155].m.ref = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[155].m.doc = '/// <summary>\r\n///     Bind an event handler to the \"mouseout\" JavaScript event, or trigger that event on an element.\r\n///     1 - mouseout(handler(eventObject)) \r\n///     2 - mouseout()\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute each time the event is triggered.\r\n/// </param>\r\n';SrliZe[156] = new Object;SrliZe[156].m = new Object;SrliZe[156].m.name = 'jQuery.prototype.mouseover';SrliZe[156].m.aliases = '';SrliZe[156].m.ref = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[156].m.doc = '/// <summary>\r\n///     Bind an event handler to the \"mouseover\" JavaScript event, or trigger that event on an element.\r\n///     1 - mouseover(handler(eventObject)) \r\n///     2 - mouseover()\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute each time the event is triggered.\r\n/// </param>\r\n';SrliZe[157] = new Object;SrliZe[157].m = new Object;SrliZe[157].m.name = 'jQuery.prototype.mouseup';SrliZe[157].m.aliases = '';SrliZe[157].m.ref = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[157].m.doc = '/// <summary>\r\n///     Bind an event handler to the \"mouseup\" JavaScript event, or trigger that event on an element.\r\n///     1 - mouseup(handler(eventObject)) \r\n///     2 - mouseup()\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute each time the event is triggered.\r\n/// </param>\r\n';SrliZe[158] = new Object;SrliZe[158].m = new Object;SrliZe[158].m.name = 'jQuery.prototype.next';SrliZe[158].m.aliases = '';SrliZe[158].m.ref = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe[158].m.doc = '/// <summary>\r\n///     Get the immediately following sibling of each element in the set of matched elements, optionally filtered by a selector.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"until\" type=\"String\">\r\n///     A string containing a selector expression to match elements against.\r\n/// </param>\r\n';SrliZe[159] = new Object;SrliZe[159].m = new Object;SrliZe[159].m.name = 'jQuery.prototype.nextAll';SrliZe[159].m.aliases = '';SrliZe[159].m.ref = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe[159].m.doc = '/// <summary>\r\n///     Get all following siblings of each element in the set of matched elements, optionally filtered by a selector.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"until\" type=\"String\">\r\n///     A string containing a selector expression to match elements against.\r\n/// </param>\r\n';SrliZe[160] = new Object;SrliZe[160].m = new Object;SrliZe[160].m.name = 'jQuery.prototype.nextUntil';SrliZe[160].m.aliases = '';SrliZe[160].m.ref = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe[160].m.doc = '/// <summary>\r\n///     Get all following siblings of each element up to but not including the element matched by the selector.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"until\" type=\"String\">\r\n///     A string containing a selector expression to indicate where to stop matching following sibling elements.\r\n/// </param>\r\n';SrliZe[161] = new Object;SrliZe[161].m = new Object;SrliZe[161].m.name = 'jQuery.prototype.not';SrliZe[161].m.aliases = '';SrliZe[161].m.ref = function( selector ) {
		return this.pushStack( winnow(this, selector, false), "not", selector);
	};SrliZe[161].m.doc = '/// <summary>\r\n///     Remove elements from the set of matched elements.\r\n///     1 - not(selector) \r\n///     2 - not(elements) \r\n///     3 - not(function(index))\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"selector\" type=\"String\">\r\n///     A string containing a selector expression to match elements against.\r\n/// </param>\r\n';SrliZe[162] = new Object;SrliZe[162].m = new Object;SrliZe[162].m.name = 'jQuery.prototype.offset';SrliZe[162].m.aliases = '';SrliZe[162].m.ref = function( options ) {
		var elem = this[0];

		if ( options ) { 
			return this.each(function( i ) {
				jQuery.offset.setOffset( this, options, i );
			});
		}

		if ( !elem || !elem.ownerDocument ) {
			return null;
		}

		if ( elem === elem.ownerDocument.body ) {
			return jQuery.offset.bodyOffset( elem );
		}

		var box = elem.getBoundingClientRect(), doc = elem.ownerDocument, body = doc.body, docElem = doc.documentElement,
			clientTop = docElem.clientTop || body.clientTop || 0, clientLeft = docElem.clientLeft || body.clientLeft || 0,
			top  = box.top  + (self.pageYOffset || jQuery.support.boxModel && docElem.scrollTop  || body.scrollTop ) - clientTop,
			left = box.left + (self.pageXOffset || jQuery.support.boxModel && docElem.scrollLeft || body.scrollLeft) - clientLeft;

		return { top: top, left: left };
	};SrliZe[162].m.doc = '/// <summary>\r\n///     1: Get the current coordinates of the first element in the set of matched elements, relative to the document.\r\n///         1.1 - offset()\r\n///     2: Set the current coordinates of every element in the set of matched elements, relative to the document.\r\n///         2.1 - offset(coordinates) \r\n///         2.2 - offset(function(index, coords))\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"options\" type=\"Object\">\r\n///     \r\n///                 An object containing the properties top and left, which are integers indicating the new top and left coordinates for the elements.\r\n///               \r\n/// </param>\r\n';SrliZe[163] = new Object;SrliZe[163].m = new Object;SrliZe[163].m.name = 'jQuery.prototype.offsetParent';SrliZe[163].m.aliases = '';SrliZe[163].m.ref = function() {
		return this.map(function() {
			var offsetParent = this.offsetParent || document.body;
			while ( offsetParent && (!/^body|html$/i.test(offsetParent.nodeName) && jQuery.css(offsetParent, "position") === "static") ) {
				offsetParent = offsetParent.offsetParent;
			}
			return offsetParent;
		});
	};SrliZe[163].m.doc = '/// <summary>\r\n///     Get the closest ancestor element that is positioned.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n';SrliZe[164] = new Object;SrliZe[164].m = new Object;SrliZe[164].m.name = 'jQuery.prototype.one';SrliZe[164].m.aliases = '';SrliZe[164].m.ref = function( type, data, fn ) {
		// Handle object literals
		if ( typeof type === "object" ) {
			for ( var key in type ) {
				this[ name ](key, data, type[key], fn);
			}
			return this;
		}
		
		if ( jQuery.isFunction( data ) ) {
			fn = data;
			data = undefined;
		}

		var handler = name === "one" ? jQuery.proxy( fn, function( event ) {
			jQuery( this ).unbind( event, handler );
			return fn.apply( this, arguments );
		}) : fn;

		if ( type === "unload" && name !== "one" ) {
			this.one( type, data, fn );

		} else {
			for ( var i = 0, l = this.length; i < l; i++ ) {
				jQuery.event.add( this[i], type, handler, data );
			}
		}

		return this;
	};SrliZe[164].m.doc = '/// <summary>\r\n///     Attach a handler to an event for the elements. The handler is executed at most once per element.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"type\" type=\"String\">\r\n///     A string containing one or more JavaScript event types, such as \"click\" or \"submit,\" or custom event names.\r\n/// </param>\r\n/// <param name=\"data\" type=\"Object\">\r\n///     A map of data that will be passed to the event handler.\r\n/// </param>\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute at the time the event is triggered.\r\n/// </param>\r\n';SrliZe[165] = new Object;SrliZe[165].m = new Object;SrliZe[165].m.name = 'jQuery.prototype.outerHeight';SrliZe[165].m.aliases = '';SrliZe[165].m.ref = function( margin ) {
		return this[0] ?
			jQuery.css( this[0], type, false, margin ? "margin" : "border" ) :
			null;
	};SrliZe[165].m.doc = '/// <summary>\r\n///     Get the current computed height for the first element in the set of matched elements, including padding and border.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"Number\" />\r\n/// <param name=\"margin\" type=\"Boolean\">\r\n///     A Boolean indicating whether to include the element\'s margin in the calculation.\r\n/// </param>\r\n';SrliZe[166] = new Object;SrliZe[166].m = new Object;SrliZe[166].m.name = 'jQuery.prototype.outerWidth';SrliZe[166].m.aliases = '';SrliZe[166].m.ref = function( margin ) {
		return this[0] ?
			jQuery.css( this[0], type, false, margin ? "margin" : "border" ) :
			null;
	};SrliZe[166].m.doc = '/// <summary>\r\n///     Get the current computed width for the first element in the set of matched elements, including padding and border.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"Number\" />\r\n/// <param name=\"margin\" type=\"Boolean\">\r\n///     A Boolean indicating whether to include the element\'s margin in the calculation.\r\n/// </param>\r\n';SrliZe[167] = new Object;SrliZe[167].m = new Object;SrliZe[167].m.name = 'jQuery.prototype.parent';SrliZe[167].m.aliases = '';SrliZe[167].m.ref = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe[167].m.doc = '/// <summary>\r\n///     Get the parent of each element in the current set of matched elements, optionally filtered by a selector.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"until\" type=\"String\">\r\n///     A string containing a selector expression to match elements against.\r\n/// </param>\r\n';SrliZe[168] = new Object;SrliZe[168].m = new Object;SrliZe[168].m.name = 'jQuery.prototype.parents';SrliZe[168].m.aliases = '';SrliZe[168].m.ref = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe[168].m.doc = '/// <summary>\r\n///     Get the ancestors of each element in the current set of matched elements, optionally filtered by a selector.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"until\" type=\"String\">\r\n///     A string containing a selector expression to match elements against.\r\n/// </param>\r\n';SrliZe[169] = new Object;SrliZe[169].m = new Object;SrliZe[169].m.name = 'jQuery.prototype.parentsUntil';SrliZe[169].m.aliases = '';SrliZe[169].m.ref = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe[169].m.doc = '/// <summary>\r\n///     Get the ancestors of each element in the current set of matched elements, up to but not including the element matched by the selector.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"until\" type=\"String\">\r\n///     A string containing a selector expression to indicate where to stop matching ancestor elements.\r\n/// </param>\r\n';SrliZe[170] = new Object;SrliZe[170].m = new Object;SrliZe[170].m.name = 'jQuery.prototype.position';SrliZe[170].m.aliases = '';SrliZe[170].m.ref = function() {
		if ( !this[0] ) {
			return null;
		}

		var elem = this[0],

		// Get *real* offsetParent
		offsetParent = this.offsetParent(),

		// Get correct offsets
		offset       = this.offset(),
		parentOffset = /^body|html$/i.test(offsetParent[0].nodeName) ? { top: 0, left: 0 } : offsetParent.offset();

		// Subtract element margins
		// note: when an element has margin: auto the offsetLeft and marginLeft
		// are the same in Safari causing offset.left to incorrectly be 0
		offset.top  -= parseFloat( jQuery.curCSS(elem, "marginTop",  true) ) || 0;
		offset.left -= parseFloat( jQuery.curCSS(elem, "marginLeft", true) ) || 0;

		// Add offsetParent borders
		parentOffset.top  += parseFloat( jQuery.curCSS(offsetParent[0], "borderTopWidth",  true) ) || 0;
		parentOffset.left += parseFloat( jQuery.curCSS(offsetParent[0], "borderLeftWidth", true) ) || 0;

		// Subtract the two offsets
		return {
			top:  offset.top  - parentOffset.top,
			left: offset.left - parentOffset.left
		};
	};SrliZe[170].m.doc = '/// <summary>\r\n///     Get the current coordinates of the first element in the set of matched elements, relative to the offset parent.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"Object\" />\r\n';SrliZe[171] = new Object;SrliZe[171].m = new Object;SrliZe[171].m.name = 'jQuery.prototype.prepend';SrliZe[171].m.aliases = '';SrliZe[171].m.ref = function() {
		return this.domManip(arguments, true, function( elem ) {
			if ( this.nodeType === 1 ) {
				this.insertBefore( elem, this.firstChild );
			}
		});
	};SrliZe[171].m.doc = '/// <summary>\r\n///     Insert content, specified by the parameter, to the beginning of each element in the set of matched elements.\r\n///     1 - prepend(content) \r\n///     2 - prepend(function(index, html))\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"\" type=\"jQuery\">\r\n///     An element, HTML string, or jQuery object to insert at the beginning of each element in the set of matched elements.\r\n/// </param>\r\n';SrliZe[172] = new Object;SrliZe[172].m = new Object;SrliZe[172].m.name = 'jQuery.prototype.prependTo';SrliZe[172].m.aliases = '';SrliZe[172].m.ref = function( selector ) {
		var ret = [], insert = jQuery( selector ),
			parent = this.length === 1 && this[0].parentNode;
		
		if ( parent && parent.nodeType === 11 && parent.childNodes.length === 1 && insert.length === 1 ) {
			insert[ original ]( this[0] );
			return this;
			
		} else {
			for ( var i = 0, l = insert.length; i < l; i++ ) {
				var elems = (i > 0 ? this.clone(true) : this).get();
				jQuery.fn[ original ].apply( jQuery(insert[i]), elems );
				ret = ret.concat( elems );
			}
		
			return this.pushStack( ret, name, insert.selector );
		}
	};SrliZe[172].m.doc = '/// <summary>\r\n///     Insert every element in the set of matched elements to the beginning of the target.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"selector\" type=\"jQuery\">\r\n///     A selector, element, HTML string, or jQuery object; the matched set of elements will be inserted at the beginning of the element(s) specified by this parameter.\r\n/// </param>\r\n';SrliZe[173] = new Object;SrliZe[173].m = new Object;SrliZe[173].m.name = 'jQuery.prototype.prev';SrliZe[173].m.aliases = '';SrliZe[173].m.ref = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe[173].m.doc = '/// <summary>\r\n///     Get the immediately preceding sibling of each element in the set of matched elements, optionally filtered by a selector.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"until\" type=\"String\">\r\n///     A string containing a selector expression to match elements against.\r\n/// </param>\r\n';SrliZe[174] = new Object;SrliZe[174].m = new Object;SrliZe[174].m.name = 'jQuery.prototype.prevAll';SrliZe[174].m.aliases = '';SrliZe[174].m.ref = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe[174].m.doc = '/// <summary>\r\n///     Get all preceding siblings of each element in the set of matched elements, optionally filtered by a selector.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"until\" type=\"String\">\r\n///     A string containing a selector expression to match elements against.\r\n/// </param>\r\n';SrliZe[175] = new Object;SrliZe[175].m = new Object;SrliZe[175].m.name = 'jQuery.prototype.prevUntil';SrliZe[175].m.aliases = '';SrliZe[175].m.ref = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe[175].m.doc = '/// <summary>\r\n///     Get all preceding siblings of each element up to but not including the element matched by the selector.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"until\" type=\"String\">\r\n///     A string containing a selector expression to indicate where to stop matching preceding sibling elements.\r\n/// </param>\r\n';SrliZe[176] = new Object;SrliZe[176].m = new Object;SrliZe[176].m.name = 'jQuery.prototype.push';SrliZe[176].m.aliases = '';SrliZe[176].m.ref = 
function push() {
    [native code]
}
;SrliZe[176].m.doc = '';SrliZe[177] = new Object;SrliZe[177].m = new Object;SrliZe[177].m.name = 'jQuery.prototype.pushStack';SrliZe[177].m.aliases = '';SrliZe[177].m.ref = function( elems, name, selector ) {
		// Build a new jQuery matched element set
		var ret = jQuery();

		if ( jQuery.isArray( elems ) ) {
			push.apply( ret, elems );
		
		} else {
			jQuery.merge( ret, elems );
		}

		// Add the old object onto the stack (as a reference)
		ret.prevObject = this;

		ret.context = this.context;

		if ( name === "find" ) {
			ret.selector = this.selector + (this.selector ? " " : "") + selector;
		} else if ( name ) {
			ret.selector = this.selector + "." + name + "(" + selector + ")";
		}

		// Return the newly-formed element set
		return ret;
	};SrliZe[177].m.doc = '/// <summary>\r\n///     Add a collection of DOM elements onto the jQuery stack.\r\n///     1 - jQuery.pushStack(elements) \r\n///     2 - jQuery.pushStack(elements, name, arguments)\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"elems\" type=\"Array\">\r\n///     An array of elements to push onto the stack and make into a new jQuery object.\r\n/// </param>\r\n/// <param name=\"name\" type=\"String\">\r\n///     The name of a jQuery method that generated the array of elements.\r\n/// </param>\r\n/// <param name=\"selector\" type=\"Array\">\r\n///     The arguments that were passed in to the jQuery method (for serialization).\r\n/// </param>\r\n';SrliZe[178] = new Object;SrliZe[178].m = new Object;SrliZe[178].m.name = 'jQuery.prototype.queue';SrliZe[178].m.aliases = '';SrliZe[178].m.ref = function( type, data ) {
		if ( typeof type !== "string" ) {
			data = type;
			type = "fx";
		}

		if ( data === undefined ) {
			return jQuery.queue( this[0], type );
		}
		return this.each(function( i, elem ) {
			var queue = jQuery.queue( this, type, data );

			if ( type === "fx" && queue[0] !== "inprogress" ) {
				jQuery.dequeue( this, type );
			}
		});
	};SrliZe[178].m.doc = '/// <summary>\r\n///     1: Show the queue of functions to be executed on the matched elements.\r\n///         1.1 - queue(queueName)\r\n///     2: Manipulate the queue of functions to be executed on the matched elements.\r\n///         2.1 - queue(queueName, newQueue) \r\n///         2.2 - queue(queueName, callback( next ))\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"type\" type=\"String\">\r\n///     \r\n///                 A string containing the name of the queue. Defaults to fx, the standard effects queue.\r\n///               \r\n/// </param>\r\n/// <param name=\"data\" type=\"Array\">\r\n///     An array of functions to replace the current queue contents.\r\n/// </param>\r\n';SrliZe[179] = new Object;SrliZe[179].m = new Object;SrliZe[179].m.name = 'jQuery.prototype.ready';SrliZe[179].m.aliases = '';SrliZe[179].m.ref = function( fn ) {
		// Attach the listeners
		jQuery.bindReady();

		// If the DOM is already ready
		if ( jQuery.isReady ) {
			// Execute the function immediately
			fn.call( document, jQuery );

		// Otherwise, remember the function for later
		} else if ( readyList ) {
			// Add the function to the wait list
			readyList.push( fn );
		}

		return this;
	};SrliZe[179].m.doc = '/// <summary>\r\n///     Specify a function to execute when the DOM is fully loaded.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute after the DOM is ready.\r\n/// </param>\r\n';SrliZe[180] = new Object;SrliZe[180].m = new Object;SrliZe[180].m.name = 'jQuery.prototype.remove';SrliZe[180].m.aliases = '';SrliZe[180].m.ref = function( selector, keepData ) {
		for ( var i = 0, elem; (elem = this[i]) != null; i++ ) {
			if ( !selector || jQuery.filter( selector, [ elem ] ).length ) {
				if ( !keepData && elem.nodeType === 1 ) {
					jQuery.cleanData( elem.getElementsByTagName("*") );
					jQuery.cleanData( [ elem ] );
				}

				if ( elem.parentNode ) {
					 elem.parentNode.removeChild( elem );
				}
			}
		}
		
		return this;
	};SrliZe[180].m.doc = '/// <summary>\r\n///     Remove the set of matched elements from the DOM.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"selector\" type=\"String\">\r\n///     A selector expression that filters the set of matched elements to be removed.\r\n/// </param>\r\n';SrliZe[181] = new Object;SrliZe[181].m = new Object;SrliZe[181].m.name = 'jQuery.prototype.removeAttr';SrliZe[181].m.aliases = '';SrliZe[181].m.ref = function( name, fn ) {
		return this.each(function(){
			jQuery.attr( this, name, "" );
			if ( this.nodeType === 1 ) {
				this.removeAttribute( name );
			}
		});
	};SrliZe[181].m.doc = '/// <summary>\r\n///     Remove an attribute from each element in the set of matched elements.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"name\" type=\"String\">\r\n///     An attribute to remove.\r\n/// </param>\r\n';SrliZe[182] = new Object;SrliZe[182].m = new Object;SrliZe[182].m.name = 'jQuery.prototype.removeClass';SrliZe[182].m.aliases = '';SrliZe[182].m.ref = function( value ) {
		if ( jQuery.isFunction(value) ) {
			return this.each(function(i) {
				var self = jQuery(this);
				self.removeClass( value.call(this, i, self.attr("class")) );
			});
		}

		if ( (value && typeof value === "string") || value === undefined ) {
			var classNames = (value || "").split(rspace);

			for ( var i = 0, l = this.length; i < l; i++ ) {
				var elem = this[i];

				if ( elem.nodeType === 1 && elem.className ) {
					if ( value ) {
						var className = (" " + elem.className + " ").replace(rclass, " ");
						for ( var c = 0, cl = classNames.length; c < cl; c++ ) {
							className = className.replace(" " + classNames[c] + " ", " ");
						}
						elem.className = jQuery.trim( className );

					} else {
						elem.className = "";
					}
				}
			}
		}

		return this;
	};SrliZe[182].m.doc = '/// <summary>\r\n///     Remove a single class, multiple classes, or all classes from each element in the set of matched elements.\r\n///     1 - removeClass(className) \r\n///     2 - removeClass(function(index, class))\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"value\" type=\"String\">\r\n///     A class name to be removed from the class attribute of each matched element.\r\n/// </param>\r\n';SrliZe[183] = new Object;SrliZe[183].m = new Object;SrliZe[183].m.name = 'jQuery.prototype.removeData';SrliZe[183].m.aliases = '';SrliZe[183].m.ref = function( key ) {
		return this.each(function() {
			jQuery.removeData( this, key );
		});
	};SrliZe[183].m.doc = '/// <summary>\r\n///     Remove a previously-stored piece of data.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"key\" type=\"String\">\r\n///     A string naming the piece of data to delete.\r\n/// </param>\r\n';SrliZe[184] = new Object;SrliZe[184].m = new Object;SrliZe[184].m.name = 'jQuery.prototype.replaceAll';SrliZe[184].m.aliases = '';SrliZe[184].m.ref = function( selector ) {
		var ret = [], insert = jQuery( selector ),
			parent = this.length === 1 && this[0].parentNode;
		
		if ( parent && parent.nodeType === 11 && parent.childNodes.length === 1 && insert.length === 1 ) {
			insert[ original ]( this[0] );
			return this;
			
		} else {
			for ( var i = 0, l = insert.length; i < l; i++ ) {
				var elems = (i > 0 ? this.clone(true) : this).get();
				jQuery.fn[ original ].apply( jQuery(insert[i]), elems );
				ret = ret.concat( elems );
			}
		
			return this.pushStack( ret, name, insert.selector );
		}
	};SrliZe[184].m.doc = '/// <summary>\r\n///     Replace each target element with the set of matched elements.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n';SrliZe[185] = new Object;SrliZe[185].m = new Object;SrliZe[185].m.name = 'jQuery.prototype.replaceWith';SrliZe[185].m.aliases = '';SrliZe[185].m.ref = function( value ) {
		if ( this[0] && this[0].parentNode ) {
			// Make sure that the elements are removed from the DOM before they are inserted
			// this can help fix replacing a parent with child elements
			if ( jQuery.isFunction( value ) ) {
				return this.each(function(i) {
					var self = jQuery(this), old = self.html();
					self.replaceWith( value.call( this, i, old ) );
				});
			}

			if ( typeof value !== "string" ) {
				value = jQuery(value).detach();
			}

			return this.each(function() {
				var next = this.nextSibling, parent = this.parentNode;

				jQuery(this).remove();

				if ( next ) {
					jQuery(next).before( value );
				} else {
					jQuery(parent).append( value );
				}
			});
		} else {
			return this.pushStack( jQuery(jQuery.isFunction(value) ? value() : value), "replaceWith", value );
		}
	};SrliZe[185].m.doc = '/// <summary>\r\n///     Replace each element in the set of matched elements with the provided new content.\r\n///     1 - replaceWith(newContent) \r\n///     2 - replaceWith(function)\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"value\" type=\"jQuery\">\r\n///     The content to insert. May be an HTML string, DOM element, or jQuery object.\r\n/// </param>\r\n';SrliZe[186] = new Object;SrliZe[186].m = new Object;SrliZe[186].m.name = 'jQuery.prototype.resize';SrliZe[186].m.aliases = '';SrliZe[186].m.ref = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[186].m.doc = '/// <summary>\r\n///     Bind an event handler to the \"resize\" JavaScript event, or trigger that event on an element.\r\n///     1 - resize(handler(eventObject)) \r\n///     2 - resize()\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute each time the event is triggered.\r\n/// </param>\r\n';SrliZe[187] = new Object;SrliZe[187].m = new Object;SrliZe[187].m.name = 'jQuery.prototype.scroll';SrliZe[187].m.aliases = '';SrliZe[187].m.ref = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[187].m.doc = '/// <summary>\r\n///     Bind an event handler to the \"scroll\" JavaScript event, or trigger that event on an element.\r\n///     1 - scroll(handler(eventObject)) \r\n///     2 - scroll()\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute each time the event is triggered.\r\n/// </param>\r\n';SrliZe[188] = new Object;SrliZe[188].m = new Object;SrliZe[188].m.name = 'jQuery.prototype.scrollLeft';SrliZe[188].m.aliases = '';SrliZe[188].m.ref = function(val) {
		var elem = this[0], win;
		
		if ( !elem ) {
			return null;
		}

		if ( val !== undefined ) {
			// Set the scroll offset
			return this.each(function() {
				win = getWindow( this );

				if ( win ) {
					win.scrollTo(
						!i ? val : jQuery(win).scrollLeft(),
						 i ? val : jQuery(win).scrollTop()
					);

				} else {
					this[ method ] = val;
				}
			});
		} else {
			win = getWindow( elem );

			// Return the scroll offset
			return win ? ("pageXOffset" in win) ? win[ i ? "pageYOffset" : "pageXOffset" ] :
				jQuery.support.boxModel && win.document.documentElement[ method ] ||
					win.document.body[ method ] :
				elem[ method ];
		}
	};SrliZe[188].m.doc = '/// <summary>\r\n///     1: Get the current horizontal position of the scroll bar for the first element in the set of matched elements.\r\n///         1.1 - scrollLeft()\r\n///     2: Set the current horizontal position of the scroll bar for each of the set of matched elements.\r\n///         2.1 - scrollLeft(value)\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"val\" type=\"Number\">\r\n///     An integer indicating the new position to set the scroll bar to.\r\n/// </param>\r\n';SrliZe[189] = new Object;SrliZe[189].m = new Object;SrliZe[189].m.name = 'jQuery.prototype.scrollTop';SrliZe[189].m.aliases = '';SrliZe[189].m.ref = function(val) {
		var elem = this[0], win;
		
		if ( !elem ) {
			return null;
		}

		if ( val !== undefined ) {
			// Set the scroll offset
			return this.each(function() {
				win = getWindow( this );

				if ( win ) {
					win.scrollTo(
						!i ? val : jQuery(win).scrollLeft(),
						 i ? val : jQuery(win).scrollTop()
					);

				} else {
					this[ method ] = val;
				}
			});
		} else {
			win = getWindow( elem );

			// Return the scroll offset
			return win ? ("pageXOffset" in win) ? win[ i ? "pageYOffset" : "pageXOffset" ] :
				jQuery.support.boxModel && win.document.documentElement[ method ] ||
					win.document.body[ method ] :
				elem[ method ];
		}
	};SrliZe[189].m.doc = '/// <summary>\r\n///     1: Get the current vertical position of the scroll bar for the first element in the set of matched elements.\r\n///         1.1 - scrollTop()\r\n///     2: Set the current vertical position of the scroll bar for each of the set of matched elements.\r\n///         2.1 - scrollTop(value)\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"val\" type=\"Number\">\r\n///     An integer indicating the new position to set the scroll bar to.\r\n/// </param>\r\n';SrliZe[190] = new Object;SrliZe[190].m = new Object;SrliZe[190].m.name = 'jQuery.prototype.select';SrliZe[190].m.aliases = '';SrliZe[190].m.ref = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[190].m.doc = '/// <summary>\r\n///     Bind an event handler to the \"select\" JavaScript event, or trigger that event on an element.\r\n///     1 - select(handler(eventObject)) \r\n///     2 - select()\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute each time the event is triggered.\r\n/// </param>\r\n';SrliZe[191] = new Object;SrliZe[191].m = new Object;SrliZe[191].m.name = 'jQuery.prototype.selector';SrliZe[191].m.aliases = '';SrliZe[191].m.ref = '';SrliZe[191].m.doc = '';SrliZe[192] = new Object;SrliZe[192].m = new Object;SrliZe[192].m.name = 'jQuery.prototype.serialize';SrliZe[192].m.aliases = '';SrliZe[192].m.ref = function() {
		return jQuery.param(this.serializeArray());
	};SrliZe[192].m.doc = '/// <summary>\r\n///     Encode a set of form elements as a string for submission.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"String\" />\r\n';SrliZe[193] = new Object;SrliZe[193].m = new Object;SrliZe[193].m.name = 'jQuery.prototype.serializeArray';SrliZe[193].m.aliases = '';SrliZe[193].m.ref = function() {
		return this.map(function() {
			return this.elements ? jQuery.makeArray(this.elements) : this;
		})
		.filter(function() {
			return this.name && !this.disabled &&
				(this.checked || rselectTextarea.test(this.nodeName) ||
					rinput.test(this.type));
		})
		.map(function( i, elem ) {
			var val = jQuery(this).val();

			return val == null ?
				null :
				jQuery.isArray(val) ?
					jQuery.map( val, function( val, i ) {
						return { name: elem.name, value: val };
					}) :
					{ name: elem.name, value: val };
		}).get();
	};SrliZe[193].m.doc = '/// <summary>\r\n///     Encode a set of form elements as an array of names and values.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"Array\" />\r\n';SrliZe[194] = new Object;SrliZe[194].m = new Object;SrliZe[194].m.name = 'jQuery.prototype.show';SrliZe[194].m.aliases = '';SrliZe[194].m.ref = function( speed, callback ) {
		if ( speed || speed === 0) {
			return this.animate( genFx("show", 3), speed, callback);

		} else {
			for ( var i = 0, l = this.length; i < l; i++ ) {
				var old = jQuery.data(this[i], "olddisplay");

				this[i].style.display = old || "";

				if ( jQuery.css(this[i], "display") === "none" ) {
					var nodeName = this[i].nodeName, display;

					if ( elemdisplay[ nodeName ] ) {
						display = elemdisplay[ nodeName ];

					} else {
						var elem = jQuery("<" + nodeName + " />").appendTo("body");

						display = elem.css("display");

						if ( display === "none" ) {
							display = "block";
						}

						elem.remove();

						elemdisplay[ nodeName ] = display;
					}

					jQuery.data(this[i], "olddisplay", display);
				}
			}

			// Set the display of the elements in a second loop
			// to avoid the constant reflow
			for ( var j = 0, k = this.length; j < k; j++ ) {
				this[j].style.display = jQuery.data(this[j], "olddisplay") || "";
			}

			return this;
		}
	};SrliZe[194].m.doc = '/// <summary>\r\n///     Display the matched elements.\r\n///     1 - show() \r\n///     2 - show(duration, callback)\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"speed\" type=\"Number\">\r\n///     A string or number determining how long the animation will run.\r\n/// </param>\r\n/// <param name=\"callback\" type=\"Function\">\r\n///     A function to call once the animation is complete.\r\n/// </param>\r\n';SrliZe[195] = new Object;SrliZe[195].m = new Object;SrliZe[195].m.name = 'jQuery.prototype.siblings';SrliZe[195].m.aliases = '';SrliZe[195].m.ref = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe[195].m.doc = '/// <summary>\r\n///     Get the siblings of each element in the set of matched elements, optionally filtered by a selector.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"until\" type=\"String\">\r\n///     A string containing a selector expression to match elements against.\r\n/// </param>\r\n';SrliZe[196] = new Object;SrliZe[196].m = new Object;SrliZe[196].m.name = 'jQuery.prototype.size';SrliZe[196].m.aliases = '';SrliZe[196].m.ref = function() {
		return this.length;
	};SrliZe[196].m.doc = '/// <summary>\r\n///     Return the number of DOM elements matched by the jQuery object.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"Number\" />\r\n';SrliZe[197] = new Object;SrliZe[197].m = new Object;SrliZe[197].m.name = 'jQuery.prototype.slice';SrliZe[197].m.aliases = '';SrliZe[197].m.ref = function() {
		return this.pushStack( slice.apply( this, arguments ),
			"slice", slice.call(arguments).join(",") );
	};SrliZe[197].m.doc = '/// <summary>\r\n///     Reduce the set of matched elements to a subset specified by a range of indices.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"\" type=\"Number\">\r\n///     An integer indicating the 0-based position after which the elements are selected. If negative, it indicates an offset from the end of the set.\r\n/// </param>\r\n/// <param name=\"{name}\" type=\"Number\">\r\n///     An integer indicating the 0-based position before which the elements stop being selected. If negative, it indicates an offset from the end of the set. If omitted, the range continues until the end of the set.\r\n/// </param>\r\n';SrliZe[198] = new Object;SrliZe[198].m = new Object;SrliZe[198].m.name = 'jQuery.prototype.slideDown';SrliZe[198].m.aliases = '';SrliZe[198].m.ref = function( speed, callback ) {
		return this.animate( props, speed, callback );
	};SrliZe[198].m.doc = '/// <summary>\r\n///     Display the matched elements with a sliding motion.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"speed\" type=\"Number\">\r\n///     A string or number determining how long the animation will run.\r\n/// </param>\r\n/// <param name=\"callback\" type=\"Function\">\r\n///     A function to call once the animation is complete.\r\n/// </param>\r\n';SrliZe[199] = new Object;SrliZe[199].m = new Object;SrliZe[199].m.name = 'jQuery.prototype.slideToggle';SrliZe[199].m.aliases = '';SrliZe[199].m.ref = function( speed, callback ) {
		return this.animate( props, speed, callback );
	};SrliZe[199].m.doc = '/// <summary>\r\n///     Display or hide the matched elements with a sliding motion.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"speed\" type=\"Number\">\r\n///     A string or number determining how long the animation will run.\r\n/// </param>\r\n/// <param name=\"callback\" type=\"Function\">\r\n///     A function to call once the animation is complete.\r\n/// </param>\r\n';SrliZe[200] = new Object;SrliZe[200].m = new Object;SrliZe[200].m.name = 'jQuery.prototype.slideUp';SrliZe[200].m.aliases = '';SrliZe[200].m.ref = function( speed, callback ) {
		return this.animate( props, speed, callback );
	};SrliZe[200].m.doc = '/// <summary>\r\n///     Hide the matched elements with a sliding motion.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"speed\" type=\"Number\">\r\n///     A string or number determining how long the animation will run.\r\n/// </param>\r\n/// <param name=\"callback\" type=\"Function\">\r\n///     A function to call once the animation is complete.\r\n/// </param>\r\n';SrliZe[201] = new Object;SrliZe[201].m = new Object;SrliZe[201].m.name = 'jQuery.prototype.sort';SrliZe[201].m.aliases = '';SrliZe[201].m.ref = 
function sort() {
    [native code]
}
;SrliZe[201].m.doc = '';SrliZe[202] = new Object;SrliZe[202].m = new Object;SrliZe[202].m.name = 'jQuery.prototype.splice';SrliZe[202].m.aliases = '';SrliZe[202].m.ref = 
function splice() {
    [native code]
}
;SrliZe[202].m.doc = '';SrliZe[203] = new Object;SrliZe[203].m = new Object;SrliZe[203].m.name = 'jQuery.prototype.stop';SrliZe[203].m.aliases = '';SrliZe[203].m.ref = function( clearQueue, gotoEnd ) {
		var timers = jQuery.timers;

		if ( clearQueue ) {
			this.queue([]);
		}

		this.each(function() {
			// go in reverse order so anything added to the queue during the loop is ignored
			for ( var i = timers.length - 1; i >= 0; i-- ) {
				if ( timers[i].elem === this ) {
					if (gotoEnd) {
						// force the next step to be the last
						timers[i](true);
					}

					timers.splice(i, 1);
				}
			}
		});

		// start the next in the queue if the last step wasn't forced
		if ( !gotoEnd ) {
			this.dequeue();
		}

		return this;
	};SrliZe[203].m.doc = '/// <summary>\r\n///     Stop the currently-running animation on the matched elements.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"clearQueue\" type=\"Boolean\">\r\n///     \r\n///                 A Boolean indicating whether to remove queued animation as well. Defaults to false.\r\n///               \r\n/// </param>\r\n/// <param name=\"gotoEnd\" type=\"Boolean\">\r\n///     \r\n///                 A Boolean indicating whether to complete the current animation immediately. Defaults to false.\r\n///               \r\n/// </param>\r\n';SrliZe[204] = new Object;SrliZe[204].m = new Object;SrliZe[204].m.name = 'jQuery.prototype.submit';SrliZe[204].m.aliases = '';SrliZe[204].m.ref = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[204].m.doc = '/// <summary>\r\n///     Bind an event handler to the \"submit\" JavaScript event, or trigger that event on an element.\r\n///     1 - submit(handler(eventObject)) \r\n///     2 - submit()\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute each time the event is triggered.\r\n/// </param>\r\n';SrliZe[205] = new Object;SrliZe[205].m = new Object;SrliZe[205].m.name = 'jQuery.prototype.text';SrliZe[205].m.aliases = '';SrliZe[205].m.ref = function( text ) {
		if ( jQuery.isFunction(text) ) {
			return this.each(function(i) {
				var self = jQuery(this);
				self.text( text.call(this, i, self.text()) );
			});
		}

		if ( typeof text !== "object" && text !== undefined ) {
			return this.empty().append( (this[0] && this[0].ownerDocument || document).createTextNode( text ) );
		}

		return jQuery.text( this );
	};SrliZe[205].m.doc = '/// <summary>\r\n///     1: Get the combined text contents of each element in the set of matched elements, including their descendants.\r\n///         1.1 - text()\r\n///     2: Set the content of each element in the set of matched elements to the specified text.\r\n///         2.1 - text(textString) \r\n///         2.2 - text(function(index, text))\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"text\" type=\"String\">\r\n///     A string of text to set as the content of each matched element.\r\n/// </param>\r\n';SrliZe[206] = new Object;SrliZe[206].m = new Object;SrliZe[206].m.name = 'jQuery.prototype.toArray';SrliZe[206].m.aliases = '';SrliZe[206].m.ref = function() {
		return slice.call( this, 0 );
	};SrliZe[206].m.doc = '/// <summary>\r\n///     Retrieve all the DOM elements contained in the jQuery set, as an array.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"Array\" />\r\n';SrliZe[207] = new Object;SrliZe[207].m = new Object;SrliZe[207].m.name = 'jQuery.prototype.toggle';SrliZe[207].m.aliases = '';SrliZe[207].m.ref = function( fn, fn2 ) {
		var bool = typeof fn === "boolean";

		if ( jQuery.isFunction(fn) && jQuery.isFunction(fn2) ) {
			this._toggle.apply( this, arguments );

		} else if ( fn == null || bool ) {
			this.each(function() {
				var state = bool ? fn : jQuery(this).is(":hidden");
				jQuery(this)[ state ? "show" : "hide" ]();
			});

		} else {
			this.animate(genFx("toggle", 3), fn, fn2);
		}

		return this;
	};SrliZe[207].m.doc = '/// <summary>\r\n///     1: Bind two or more handlers to the matched elements, to be executed on alternate clicks.\r\n///         1.1 - toggle(handler(eventObject), handler(eventObject), handler(eventObject))\r\n///     2: Display or hide the matched elements.\r\n///         2.1 - toggle(duration, callback) \r\n///         2.2 - toggle(showOrHide)\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute every even time the element is clicked.\r\n/// </param>\r\n/// <param name=\"fn2\" type=\"Function\">\r\n///     A function to execute every odd time the element is clicked.\r\n/// </param>\r\n/// <param name=\"{name}\" type=\"Function\">\r\n///     Additional handlers to cycle through after clicks.\r\n/// </param>\r\n';SrliZe[208] = new Object;SrliZe[208].m = new Object;SrliZe[208].m.name = 'jQuery.prototype.toggleClass';SrliZe[208].m.aliases = '';SrliZe[208].m.ref = function( value, stateVal ) {
		var type = typeof value, isBool = typeof stateVal === "boolean";

		if ( jQuery.isFunction( value ) ) {
			return this.each(function(i) {
				var self = jQuery(this);
				self.toggleClass( value.call(this, i, self.attr("class"), stateVal), stateVal );
			});
		}

		return this.each(function() {
			if ( type === "string" ) {
				// toggle individual class names
				var className, i = 0, self = jQuery(this),
					state = stateVal,
					classNames = value.split( rspace );

				while ( (className = classNames[ i++ ]) ) {
					// check each className given, space seperated list
					state = isBool ? state : !self.hasClass( className );
					self[ state ? "addClass" : "removeClass" ]( className );
				}

			} else if ( type === "undefined" || type === "boolean" ) {
				if ( this.className ) {
					// store className if set
					jQuery.data( this, "__className__", this.className );
				}

				// toggle whole className
				this.className = this.className || value === false ? "" : jQuery.data( this, "__className__" ) || "";
			}
		});
	};SrliZe[208].m.doc = '/// <summary>\r\n///     Add or remove one or more classes from each element in the set of matched elements, depending on either the class\'s presence or the value of the switch argument.\r\n///     1 - toggleClass(className) \r\n///     2 - toggleClass(className, switch) \r\n///     3 - toggleClass(function(index, class), switch)\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"value\" type=\"String\">\r\n///     One or more class names (separated by spaces) to be toggled for each element in the matched set.\r\n/// </param>\r\n/// <param name=\"stateVal\" type=\"Boolean\">\r\n///     A boolean value to determine whether the class should be added or removed.\r\n/// </param>\r\n';SrliZe[209] = new Object;SrliZe[209].m = new Object;SrliZe[209].m.name = 'jQuery.prototype.trigger';SrliZe[209].m.aliases = '';SrliZe[209].m.ref = function( type, data ) {
		return this.each(function() {
			jQuery.event.trigger( type, data, this );
		});
	};SrliZe[209].m.doc = '/// <summary>\r\n///     Execute all handlers and behaviors attached to the matched elements for the given event type.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"type\" type=\"String\">\r\n///     \r\n///                 A string containing a JavaScript event type, such as click or submit.\r\n///               \r\n/// </param>\r\n/// <param name=\"data\" type=\"Array\">\r\n///     An array of additional parameters to pass along to the event handler.\r\n/// </param>\r\n';SrliZe[210] = new Object;SrliZe[210].m = new Object;SrliZe[210].m.name = 'jQuery.prototype.triggerHandler';SrliZe[210].m.aliases = '';SrliZe[210].m.ref = function( type, data ) {
		if ( this[0] ) {
			var event = jQuery.Event( type );
			event.preventDefault();
			event.stopPropagation();
			jQuery.event.trigger( event, data, this[0] );
			return event.result;
		}
	};SrliZe[210].m.doc = '/// <summary>\r\n///     Execute all handlers attached to an element for an event.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"Object\" />\r\n/// <param name=\"type\" type=\"String\">\r\n///     \r\n///                 A string containing a JavaScript event type, such as click or submit.\r\n///               \r\n/// </param>\r\n/// <param name=\"data\" type=\"Array\">\r\n///     An array of additional parameters to pass along to the event handler.\r\n/// </param>\r\n';SrliZe[211] = new Object;SrliZe[211].m = new Object;SrliZe[211].m.name = 'jQuery.prototype.unbind';SrliZe[211].m.aliases = '';SrliZe[211].m.ref = function( type, fn ) {
		// Handle object literals
		if ( typeof type === "object" && !type.preventDefault ) {
			for ( var key in type ) {
				this.unbind(key, type[key]);
			}

		} else {
			for ( var i = 0, l = this.length; i < l; i++ ) {
				jQuery.event.remove( this[i], type, fn );
			}
		}

		return this;
	};SrliZe[211].m.doc = '/// <summary>\r\n///     Remove a previously-attached event handler from the elements.\r\n///     1 - unbind(eventType, handler(eventObject)) \r\n///     2 - unbind(event)\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"type\" type=\"String\">\r\n///     \r\n///                 A string containing a JavaScript event type, such as click or submit.\r\n///               \r\n/// </param>\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     The function that is to be no longer executed.\r\n/// </param>\r\n';SrliZe[212] = new Object;SrliZe[212].m = new Object;SrliZe[212].m.name = 'jQuery.prototype.undelegate';SrliZe[212].m.aliases = '';SrliZe[212].m.ref = function( selector, types, fn ) {
		if ( arguments.length === 0 ) {
				return this.unbind( "live" );
		
		} else {
			return this.die( types, null, fn, selector );
		}
	};SrliZe[212].m.doc = '/// <summary>\r\n///     Remove a handler from the event for all elements which match the current selector, now or in the future, based upon a specific set of root elements.\r\n///     1 - undelegate() \r\n///     2 - undelegate(selector, eventType) \r\n///     3 - undelegate(selector, eventType, handler)\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"selector\" type=\"String\">\r\n///     A selector which will be used to filter the event results.\r\n/// </param>\r\n/// <param name=\"types\" type=\"String\">\r\n///     A string containing a JavaScript event type, such as \"click\" or \"keydown\"\r\n/// </param>\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute at the time the event is triggered.\r\n/// </param>\r\n';SrliZe[213] = new Object;SrliZe[213].m = new Object;SrliZe[213].m.name = 'jQuery.prototype.unload';SrliZe[213].m.aliases = '';SrliZe[213].m.ref = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe[213].m.doc = '/// <summary>\r\n///     Bind an event handler to the \"unload\" JavaScript event.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"fn\" type=\"Function\">\r\n///     A function to execute when the event is triggered.\r\n/// </param>\r\n';SrliZe[214] = new Object;SrliZe[214].m = new Object;SrliZe[214].m.name = 'jQuery.prototype.unwrap';SrliZe[214].m.aliases = '';SrliZe[214].m.ref = function() {
		return this.parent().each(function() {
			if ( !jQuery.nodeName( this, "body" ) ) {
				jQuery( this ).replaceWith( this.childNodes );
			}
		}).end();
	};SrliZe[214].m.doc = '/// <summary>\r\n///     Remove the parents of the set of matched elements from the DOM, leaving the matched elements in their place.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n';SrliZe[215] = new Object;SrliZe[215].m = new Object;SrliZe[215].m.name = 'jQuery.prototype.val';SrliZe[215].m.aliases = '';SrliZe[215].m.ref = function( value ) {
		if ( value === undefined ) {
			var elem = this[0];

			if ( elem ) {
				if ( jQuery.nodeName( elem, "option" ) ) {
					return (elem.attributes.value || {}).specified ? elem.value : elem.text;
				}

				// We need to handle select boxes special
				if ( jQuery.nodeName( elem, "select" ) ) {
					var index = elem.selectedIndex,
						values = [],
						options = elem.options,
						one = elem.type === "select-one";

					// Nothing was selected
					if ( index < 0 ) {
						return null;
					}

					// Loop through all the selected options
					for ( var i = one ? index : 0, max = one ? index + 1 : options.length; i < max; i++ ) {
						var option = options[ i ];

						if ( option.selected ) {
							// Get the specifc value for the option
							value = jQuery(option).val();

							// We don't need an array for one selects
							if ( one ) {
								return value;
							}

							// Multi-Selects return an array
							values.push( value );
						}
					}

					return values;
				}

				// Handle the case where in Webkit "" is returned instead of "on" if a value isn't specified
				if ( rradiocheck.test( elem.type ) && !jQuery.support.checkOn ) {
					return elem.getAttribute("value") === null ? "on" : elem.value;
				}
				

				// Everything else, we just grab the value
				return (elem.value || "").replace(rreturn, "");

			}

			return undefined;
		}

		var isFunction = jQuery.isFunction(value);

		return this.each(function(i) {
			var self = jQuery(this), val = value;

			if ( this.nodeType !== 1 ) {
				return;
			}

			if ( isFunction ) {
				val = value.call(this, i, self.val());
			}

			// Typecast each time if the value is a Function and the appended
			// value is therefore different each time.
			if ( typeof val === "number" ) {
				val += "";
			}

			if ( jQuery.isArray(val) && rradiocheck.test( this.type ) ) {
				this.checked = jQuery.inArray( self.val(), val ) >= 0;

			} else if ( jQuery.nodeName( this, "select" ) ) {
				var values = jQuery.makeArray(val);

				jQuery( "option", this ).each(function() {
					this.selected = jQuery.inArray( jQuery(this).val(), values ) >= 0;
				});

				if ( !values.length ) {
					this.selectedIndex = -1;
				}

			} else {
				this.value = val;
			}
		});
	};SrliZe[215].m.doc = '/// <summary>\r\n///     1: Get the current value of the first element in the set of matched elements.\r\n///         1.1 - val()\r\n///     2: Set the value of each element in the set of matched elements.\r\n///         2.1 - val(value) \r\n///         2.2 - val(function(index, value))\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"value\" type=\"String\">\r\n///     A string of text or an array of strings to set as the value property of each matched element.\r\n/// </param>\r\n';SrliZe[216] = new Object;SrliZe[216].m = new Object;SrliZe[216].m.name = 'jQuery.prototype.width';SrliZe[216].m.aliases = '';SrliZe[216].m.ref = function( size ) {
		// Get window width or height
		var elem = this[0];
		if ( !elem ) {
			return size == null ? null : this;
		}
		
		if ( jQuery.isFunction( size ) ) {
			return this.each(function( i ) {
				var self = jQuery( this );
				self[ type ]( size.call( this, i, self[ type ]() ) );
			});
		}

		return ("scrollTo" in elem && elem.document) ? // does it walk and quack like a window?
			// Everyone else use document.documentElement or document.body depending on Quirks vs Standards mode
			elem.document.compatMode === "CSS1Compat" && elem.document.documentElement[ "client" + name ] ||
			elem.document.body[ "client" + name ] :

			// Get document width or height
			(elem.nodeType === 9) ? // is it a document
				// Either scroll[Width/Height] or offset[Width/Height], whichever is greater
				Math.max(
					elem.documentElement["client" + name],
					elem.body["scroll" + name], elem.documentElement["scroll" + name],
					elem.body["offset" + name], elem.documentElement["offset" + name]
				) :

				// Get or set width or height on the element
				size === undefined ?
					// Get width or height on the element
					jQuery.css( elem, type ) :

					// Set the width or height on the element (default to pixels if value is unitless)
					this.css( type, typeof size === "string" ? size : size + "px" );
	};SrliZe[216].m.doc = '/// <summary>\r\n///     1: Get the current computed width for the first element in the set of matched elements.\r\n///         1.1 - width()\r\n///     2: Set the CSS width of each element in the set of matched elements.\r\n///         2.1 - width(value) \r\n///         2.2 - width(function(index, width))\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"size\" type=\"Number\">\r\n///     An integer representing the number of pixels, or an integer along with an optional unit of measure appended (as a string).\r\n/// </param>\r\n';SrliZe[217] = new Object;SrliZe[217].m = new Object;SrliZe[217].m.name = 'jQuery.prototype.wrap';SrliZe[217].m.aliases = '';SrliZe[217].m.ref = function( html ) {
		return this.each(function() {
			jQuery( this ).wrapAll( html );
		});
	};SrliZe[217].m.doc = '/// <summary>\r\n///     Wrap an HTML structure around each element in the set of matched elements.\r\n///     1 - wrap(wrappingElement) \r\n///     2 - wrap(wrappingFunction)\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"html\" type=\"jQuery\">\r\n///     An HTML snippet, selector expression, jQuery object, or DOM element specifying the structure to wrap around the matched elements.\r\n/// </param>\r\n';SrliZe[218] = new Object;SrliZe[218].m = new Object;SrliZe[218].m.name = 'jQuery.prototype.wrapAll';SrliZe[218].m.aliases = '';SrliZe[218].m.ref = function( html ) {
		if ( jQuery.isFunction( html ) ) {
			return this.each(function(i) {
				jQuery(this).wrapAll( html.call(this, i) );
			});
		}

		if ( this[0] ) {
			// The elements to wrap the target around
			var wrap = jQuery( html, this[0].ownerDocument ).eq(0).clone(true);

			if ( this[0].parentNode ) {
				wrap.insertBefore( this[0] );
			}

			wrap.map(function() {
				var elem = this;

				while ( elem.firstChild && elem.firstChild.nodeType === 1 ) {
					elem = elem.firstChild;
				}

				return elem;
			}).append(this);
		}

		return this;
	};SrliZe[218].m.doc = '/// <summary>\r\n///     Wrap an HTML structure around all elements in the set of matched elements.\r\n///     \r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"html\" type=\"jQuery\">\r\n///     An HTML snippet, selector expression, jQuery object, or DOM element specifying the structure to wrap around the matched elements.\r\n/// </param>\r\n';SrliZe[219] = new Object;SrliZe[219].m = new Object;SrliZe[219].m.name = 'jQuery.prototype.wrapInner';SrliZe[219].m.aliases = '';SrliZe[219].m.ref = function( html ) {
		if ( jQuery.isFunction( html ) ) {
			return this.each(function(i) {
				jQuery(this).wrapInner( html.call(this, i) );
			});
		}

		return this.each(function() {
			var self = jQuery( this ), contents = self.contents();

			if ( contents.length ) {
				contents.wrapAll( html );

			} else {
				self.append( html );
			}
		});
	};SrliZe[219].m.doc = '/// <summary>\r\n///     Wrap an HTML structure around the content of each element in the set of matched elements.\r\n///     1 - wrapInner(wrappingElement) \r\n///     2 - wrapInner(wrappingFunction)\r\n/// </summary>\r\n/// <returns type=\"jQuery\" />\r\n/// <param name=\"html\" type=\"String\">\r\n///     An HTML snippet, selector expression, jQuery object, or DOM element specifying the structure to wrap around the content of the matched elements.\r\n/// </param>\r\n';SrliZe[220] = new Object;SrliZe[220].events = new Object;SrliZe[220].events.click = new Array;SrliZe[220].events.click[0] = new Object;SrliZe[220].events.click[0].handler = function () {
        var i = this.selectedIndex;
        var m = $(this).find("option").eq(i).data("m");
        $name.val(m.name);
        $aliases.val(m.aliases);
        $type.val(typeof (m.ref));
        $docComment.text(m.doc);
        $value.text(m.ref.toString());
    };SrliZe[220].events.click[0].data = SrliZe;SrliZe[220].events.click[0].namespace = '';SrliZe[220].events.click[0].type = 'click';SrliZe[220].events.click[0].guid = 1;SrliZe[220].events.keyup = new Array;SrliZe[220].events.keyup[0] = new Object;SrliZe[220].events.keyup[0].handler = function () {
        var i = this.selectedIndex;
        var m = $(this).find("option").eq(i).data("m");
        $name.val(m.name);
        $aliases.val(m.aliases);
        $type.val(typeof (m.ref));
        $docComment.text(m.doc);
        $value.text(m.ref.toString());
    };SrliZe[220].events.keyup[0].data = SrliZe;SrliZe[220].events.keyup[0].namespace = '';SrliZe[220].events.keyup[0].type = 'keyup';SrliZe[220].events.keyup[0].guid = 1;SrliZe[220].handle = function() {
				// Handle the second event of a trigger and when
				// an event is called after a page has unloaded
				return typeof jQuery !== "undefined" && !jQuery.event.triggered ?
					jQuery.event.handle.apply( eventHandle.elem, arguments ) :
					undefined;
			};SrliZe[221] = new Object;SrliZe[221].events = new Object;SrliZe[221].events.click = new Array;SrliZe[221].events.click[0] = new Object;SrliZe[221].events.click[0].handler = function () {
        var t = this;
        t.disabled = true;
        var $status = $("#status").text("loading...");

        var jQueryDocJsonUrl = document.location.toString();
        if (jQueryDocJsonUrl.lastIndexOf("/") === jQueryDocJsonUrl.length - 1) {
            jQueryDocJsonUrl = jQueryDocJsonUrl.substr(0, jQueryDocJsonUrl.length - 1);
        }
        jQueryDocJsonUrl += "/jQueryDoc";

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

            $status.text("complete");
            window.setTimeout(function () {
                $status.fadeOut("fast", function () {
                    $status.text("")
                });
            }, 3000);
            t.disabled = false;

        });
    };SrliZe[221].events.click[0].data = SrliZe;SrliZe[221].events.click[0].namespace = '';SrliZe[221].events.click[0].type = 'click';SrliZe[221].events.click[0].guid = 2;SrliZe[221].handle = function() {
				// Handle the second event of a trigger and when
				// an event is called after a page has unloaded
				return typeof jQuery !== "undefined" && !jQuery.event.triggered ?
					jQuery.event.handle.apply( eventHandle.elem, arguments ) :
					undefined;
			};SrliZe[222] = new Object;SrliZe[222].events = new Object;SrliZe[222].events.click = new Array;SrliZe[222].events.click[0] = new Object;SrliZe[222].events.click[0].handler = function () {
        var file = "", member;

        function serialize(obj) {
            var returnVal;
            if (obj) {
                switch (obj.constructor) {
                    case Array:
                        var vArr = "[";
                        for (var i = 0; i < obj.length; i++) {
                            if (i > 0) vArr += ",";
                            vArr += serialize(obj[i]);
                        }
                        vArr += "]"
                        return vArr;
                    case String:
                        returnVal = escape("'" + obj + "'");
                        return returnVal;
                    case Number:
                        returnVal = isFinite(obj) ? obj.toString() : null;
                        return returnVal;
                    case Date:
                        returnVal = "#" + obj + "#";
                        return returnVal;
                    default:
                        if (typeof obj === "object") {
                            var vobj = [];
                            for (attr in obj) {
                                if (typeof obj[attr] !== "function") {
                                    vobj.push('"' + attr + '":' + serialize(obj[attr]));
                                }
                            }
                            if (vobj.length > 0)
                                return "{" + vobj.join(",") + "}";
                            else
                                return "{}";
                        }
                        else {
                            return obj.toString();
                        }
                }
            }
            return "";
        }

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
                "* Comment version: 1.4.2\r\n" +
                "*/\r\n\r\n";

        file += "/*!\r\n" +
                "* jQuery JavaScript Library v1.4.1\r\n" +
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
                "* Released under the MIT, BSD, and GPL Licenses.\r\n" +
                "*\r\n" +
                "* Date: Mon Jan 25 19:43:33 2010 -0500\r\n" +
                "*/\r\n\r\n";

        file += "(function ( window, undefined ) {\r\n";

        $members.find("option").each(function () {
            member = $(this).data("m");
            var refBody = member.ref.toString();

            if (refBody.indexOf("[native code]") >= 0 ||
                $.trim(refBody) === "" ||
                typeof (member.ref) === "string") return true;

            if (member.name === "jQuery") {
                file += "var jQuery = " + injectDoc(refBody, member.doc) + ";";
                for (var priv in jQueryPrivates) {
                    file += "\r\nfunction {name} {body};".supplant({
                        name: priv,
                        body: jQueryPrivates[priv].toString().substr("function ".length)
                    });
                }
            } else {
                var sz = new JSSerializer();
                sz.Serialize(member.ref);
                file += "\r\n{name} = {body};".supplant({
                    name: member.name,
                    body: typeof (member.ref) === "function" ? injectDoc(refBody, member.doc) : sz.GetJSString()
                });
            }
        });

        file += "\r\njQuery.fn = jQuery.prototype;";
        file += "\r\njQuery.fn.init.prototype = jQuery.fn;";
        file += "\r\nwindow.jQuery = window.$ = jQuery;";
        file += "\r\n})(window);";

        $("#docFile").val(file);
    };SrliZe[222].events.click[0].data = SrliZe;SrliZe[222].events.click[0].namespace = '';SrliZe[222].events.click[0].type = 'click';SrliZe[222].events.click[0].guid = 3;SrliZe[222].handle = function() {
				// Handle the second event of a trigger and when
				// an event is called after a page has unloaded
				return typeof jQuery !== "undefined" && !jQuery.event.triggered ?
					jQuery.event.handle.apply( eventHandle.elem, arguments ) :
					undefined;
			};;
jQuery.clean = function( elems, context, fragment, scripts ) {

		context = context || document;

		// !context.createElement fails in IE with an error but returns typeof 'object'
		if ( typeof context.createElement === "undefined" ) {
			context = context.ownerDocument || context[0] && context[0].ownerDocument || document;
		}

		var ret = [];

		for ( var i = 0, elem; (elem = elems[i]) != null; i++ ) {
			if ( typeof elem === "number" ) {
				elem += "";
			}

			if ( !elem ) {
				continue;
			}

			// Convert html string into DOM nodes
			if ( typeof elem === "string" && !rhtml.test( elem ) ) {
				elem = context.createTextNode( elem );

			} else if ( typeof elem === "string" ) {
				// Fix "XHTML"-style tags in all browsers
				elem = elem.replace(rxhtmlTag, fcloseTag);

				// Trim whitespace, otherwise indexOf won't work as expected
				var tag = (rtagName.exec( elem ) || ["", ""])[1].toLowerCase(),
					wrap = wrapMap[ tag ] || wrapMap._default,
					depth = wrap[0],
					div = context.createElement("div");

				// Go to html and back, then peel off extra wrappers
				div.innerHTML = wrap[1] + elem + wrap[2];

				// Move to the right depth
				while ( depth-- ) {
					div = div.lastChild;
				}

				// Remove IE's autoinserted <tbody> from table fragments
				if ( !jQuery.support.tbody ) {

					// String was a <table>, *may* have spurious <tbody>
					var hasBody = rtbody.test(elem),
						tbody = tag === "table" && !hasBody ?
							div.firstChild && div.firstChild.childNodes :

							// String was a bare <thead> or <tfoot>
							wrap[1] === "<table>" && !hasBody ?
								div.childNodes :
								[];

					for ( var j = tbody.length - 1; j >= 0 ; --j ) {
						if ( jQuery.nodeName( tbody[ j ], "tbody" ) && !tbody[ j ].childNodes.length ) {
							tbody[ j ].parentNode.removeChild( tbody[ j ] );
						}
					}

				}

				// IE completely kills leading whitespace when innerHTML is used
				if ( !jQuery.support.leadingWhitespace && rleadingWhitespace.test( elem ) ) {
					div.insertBefore( context.createTextNode( rleadingWhitespace.exec(elem)[0] ), div.firstChild );
				}

				elem = div.childNodes;
			}

			if ( elem.nodeType ) {
				ret.push( elem );
			} else {
				ret = jQuery.merge( ret, elem );
			}
		}

		if ( fragment ) {
			for ( var i = 0; ret[i]; i++ ) {
				if ( scripts && jQuery.nodeName( ret[i], "script" ) && (!ret[i].type || ret[i].type.toLowerCase() === "text/javascript") ) {
					scripts.push( ret[i].parentNode ? ret[i].parentNode.removeChild( ret[i] ) : ret[i] );
				
				} else {
					if ( ret[i].nodeType === 1 ) {
						ret.splice.apply( ret, [i + 1, 0].concat(jQuery.makeArray(ret[i].getElementsByTagName("script"))) );
					}
					fragment.appendChild( ret[i] );
				}
			}
		}

		return ret;
	};
jQuery.cleanData = function( elems ) {

		var data, id, cache = jQuery.cache,
			special = jQuery.event.special,
			deleteExpando = jQuery.support.deleteExpando;
		
		for ( var i = 0, elem; (elem = elems[i]) != null; i++ ) {
			id = elem[ jQuery.expando ];
			
			if ( id ) {
				data = cache[ id ];
				
				if ( data.events ) {
					for ( var type in data.events ) {
						if ( special[ type ] ) {
							jQuery.event.remove( elem, type );

						} else {
							removeEvent( elem, type, data.handle );
						}
					}
				}
				
				if ( deleteExpando ) {
					delete elem[ jQuery.expando ];

				} else if ( elem.removeAttribute ) {
					elem.removeAttribute( jQuery.expando );
				}
				
				delete cache[ id ];
			}
		}
	};
jQuery.contains = function(a, b){
/// <summary>
///     Check to see if a DOM node is within another DOM node.
///     
/// </summary>
/// <returns type="Boolean" />
/// <param name="a" domElement="true">
///     The DOM element that may contain the other element.
/// </param>
/// <param name="b" domElement="true">
///     The DOM node that may be contained by the other element.
/// </param>

	return a !== b && (a.contains ? a.contains(b) : true);
};
jQuery.css = function( elem, name, force, extra ) {

		if ( name === "width" || name === "height" ) {
			var val, props = cssShow, which = name === "width" ? cssWidth : cssHeight;

			function getWH() {
				val = name === "width" ? elem.offsetWidth : elem.offsetHeight;

				if ( extra === "border" ) {
					return;
				}

				jQuery.each( which, function() {
					if ( !extra ) {
						val -= parseFloat(jQuery.curCSS( elem, "padding" + this, true)) || 0;
					}

					if ( extra === "margin" ) {
						val += parseFloat(jQuery.curCSS( elem, "margin" + this, true)) || 0;
					} else {
						val -= parseFloat(jQuery.curCSS( elem, "border" + this + "Width", true)) || 0;
					}
				});
			}

			if ( elem.offsetWidth !== 0 ) {
				getWH();
			} else {
				jQuery.swap( elem, props, getWH );
			}

			return Math.max(0, Math.round(val));
		}

		return jQuery.curCSS( elem, name, force );
	};
jQuery.curCSS = function( elem, name, force ) {

		var ret, style = elem.style, filter;

		// IE uses filters for opacity
		if ( !jQuery.support.opacity && name === "opacity" && elem.currentStyle ) {
			ret = ropacity.test(elem.currentStyle.filter || "") ?
				(parseFloat(RegExp.$1) / 100) + "" :
				"";

			return ret === "" ?
				"1" :
				ret;
		}

		// Make sure we're using the right name for getting the float value
		if ( rfloat.test( name ) ) {
			name = styleFloat;
		}

		if ( !force && style && style[ name ] ) {
			ret = style[ name ];

		} else if ( getComputedStyle ) {

			// Only "float" is needed here
			if ( rfloat.test( name ) ) {
				name = "float";
			}

			name = name.replace( rupper, "-$1" ).toLowerCase();

			var defaultView = elem.ownerDocument.defaultView;

			if ( !defaultView ) {
				return null;
			}

			var computedStyle = defaultView.getComputedStyle( elem, null );

			if ( computedStyle ) {
				ret = computedStyle.getPropertyValue( name );
			}

			// We should always get a number back from opacity
			if ( name === "opacity" && ret === "" ) {
				ret = "1";
			}

		} else if ( elem.currentStyle ) {
			var camelCase = name.replace(rdashAlpha, fcamelCase);

			ret = elem.currentStyle[ name ] || elem.currentStyle[ camelCase ];

			// From the awesome hack by Dean Edwards
			// http://erik.eae.net/archives/2007/07/27/18.54.15/#comment-102291

			// If we're not dealing with a regular pixel number
			// but a number that has a weird ending, we need to convert it to pixels
			if ( !rnumpx.test( ret ) && rnum.test( ret ) ) {
				// Remember the original values
				var left = style.left, rsLeft = elem.runtimeStyle.left;

				// Put in the new values to get a computed value out
				elem.runtimeStyle.left = elem.currentStyle.left;
				style.left = camelCase === "fontSize" ? "1em" : (ret || 0);
				ret = style.pixelLeft + "px";

				// Revert the changed values
				style.left = left;
				elem.runtimeStyle.left = rsLeft;
			}
		}

		return ret;
	};
jQuery.data = function( elem, name, data ) {
/// <summary>
///     1: Store arbitrary data associated with the specified element.
///         1.1 - jQuery.data(element, key, value)
///     2: 
///             Returns value at named data store for the element, as set by jQuery.data(element, name, value), or the full data store for the element.
///           
///         2.1 - jQuery.data(element, key) 
///         2.2 - jQuery.data(element)
/// </summary>
/// <returns type="jQuery" />
/// <param name="elem" domElement="true">
///     The DOM element to associate with the data.
/// </param>
/// <param name="name" type="String">
///     A string naming the piece of data to set.
/// </param>
/// <param name="data" type="Object">
///     The new data value.
/// </param>

		if ( elem.nodeName && jQuery.noData[elem.nodeName.toLowerCase()] ) {
			return;
		}

		elem = elem == window ?
			windowData :
			elem;

		var id = elem[ expando ], cache = jQuery.cache, thisCache;

		if ( !id && typeof name === "string" && data === undefined ) {
			return null;
		}

		// Compute a unique ID for the element
		if ( !id ) { 
			id = ++uuid;
		}

		// Avoid generating a new cache unless none exists and we
		// want to manipulate it.
		if ( typeof name === "object" ) {
			elem[ expando ] = id;
			thisCache = cache[ id ] = jQuery.extend(true, {}, name);

		} else if ( !cache[ id ] ) {
			elem[ expando ] = id;
			cache[ id ] = {};
		}

		thisCache = cache[ id ];

		// Prevent overriding the named cache with undefined values
		if ( data !== undefined ) {
			thisCache[ name ] = data;
		}

		return typeof name === "string" ? thisCache[ name ] : thisCache;
	};
jQuery.dequeue = function( elem, type ) {
/// <summary>
///     Execute the next function on the queue for the matched element.
///     
/// </summary>
/// <returns type="jQuery" />
/// <param name="elem" domElement="true">
///     A DOM element from which to remove and execute a queued function.
/// </param>
/// <param name="type" type="String">
///     
///                 A string containing the name of the queue. Defaults to fx, the standard effects queue.
///               
/// </param>

		type = type || "fx";

		var queue = jQuery.queue( elem, type ), fn = queue.shift();

		// If the fx queue is dequeued, always remove the progress sentinel
		if ( fn === "inprogress" ) {
			fn = queue.shift();
		}

		if ( fn ) {
			// Add a progress sentinel to prevent the fx queue from being
			// automatically dequeued
			if ( type === "fx" ) {
				queue.unshift("inprogress");
			}

			fn.call(elem, function() {
				jQuery.dequeue(elem, type);
			});
		}
	};
jQuery.dir = function( elem, dir, until ) {

		var matched = [], cur = elem[dir];
		while ( cur && cur.nodeType !== 9 && (until === undefined || cur.nodeType !== 1 || !jQuery( cur ).is( until )) ) {
			if ( cur.nodeType === 1 ) {
				matched.push( cur );
			}
			cur = cur[dir];
		}
		return matched;
	};
jQuery.each = function( object, callback, args ) {
/// <summary>
///     
///             A generic iterator function, which can be used to seamlessly iterate over both objects and arrays. Arrays and array-like objects with a length property (such as a function's arguments object) are iterated by numeric index, from 0 to length-1. Other objects are iterated via their named properties.
///           
///     
/// </summary>
/// <returns type="Object" />
/// <param name="object" type="Object">
///     The object or array to iterate over.
/// </param>
/// <param name="callback" type="Function">
///     The function that will be executed on every object.
/// </param>

		var name, i = 0,
			length = object.length,
			isObj = length === undefined || jQuery.isFunction(object);

		if ( args ) {
			if ( isObj ) {
				for ( name in object ) {
					if ( callback.apply( object[ name ], args ) === false ) {
						break;
					}
				}
			} else {
				for ( ; i < length; ) {
					if ( callback.apply( object[ i++ ], args ) === false ) {
						break;
					}
				}
			}

		// A special, fast, case for the most common use of each
		} else {
			if ( isObj ) {
				for ( name in object ) {
					if ( callback.call( object[ name ], name, object[ name ] ) === false ) {
						break;
					}
				}
			} else {
				for ( var value = object[0];
					i < length && callback.call( value, i, value ) !== false; value = object[++i] ) {}
			}
		}

		return object;
	};
jQuery.easing = SrliZe = new Object;SrliZe.linear = function( p, n, firstNum, diff ) {
			return firstNum + diff * p;
		};SrliZe.swing = function( p, n, firstNum, diff ) {
			return ((-Math.cos(p*Math.PI)/2) + 0.5) * diff + firstNum;
		};;
jQuery.error = function( msg ) {
/// <summary>
///     Takes a string and throws an exception containing it.
///     
/// </summary>/// <param name="msg" type="String">
///     The message to send out.
/// </param>

		throw msg;
	};
jQuery.etag = SrliZe = new Object;;
jQuery.event = SrliZe = new Object;SrliZe.add = function( elem, types, handler, data ) {
		if ( elem.nodeType === 3 || elem.nodeType === 8 ) {
			return;
		}

		// For whatever reason, IE has trouble passing the window object
		// around, causing it to be cloned in the process
		if ( elem.setInterval && ( elem !== window && !elem.frameElement ) ) {
			elem = window;
		}

		var handleObjIn, handleObj;

		if ( handler.handler ) {
			handleObjIn = handler;
			handler = handleObjIn.handler;
		}

		// Make sure that the function being executed has a unique ID
		if ( !handler.guid ) {
			handler.guid = jQuery.guid++;
		}

		// Init the element's event structure
		var elemData = jQuery.data( elem );

		// If no elemData is found then we must be trying to bind to one of the
		// banned noData elements
		if ( !elemData ) {
			return;
		}

		var events = elemData.events = elemData.events || {},
			eventHandle = elemData.handle, eventHandle;

		if ( !eventHandle ) {
			elemData.handle = eventHandle = function() {
				// Handle the second event of a trigger and when
				// an event is called after a page has unloaded
				return typeof jQuery !== "undefined" && !jQuery.event.triggered ?
					jQuery.event.handle.apply( eventHandle.elem, arguments ) :
					undefined;
			};
		}

		// Add elem as a property of the handle function
		// This is to prevent a memory leak with non-native events in IE.
		eventHandle.elem = elem;

		// Handle multiple events separated by a space
		// jQuery(...).bind("mouseover mouseout", fn);
		types = types.split(" ");

		var type, i = 0, namespaces;

		while ( (type = types[ i++ ]) ) {
			handleObj = handleObjIn ?
				jQuery.extend({}, handleObjIn) :
				{ handler: handler, data: data };

			// Namespaced event handlers
			if ( type.indexOf(".") > -1 ) {
				namespaces = type.split(".");
				type = namespaces.shift();
				handleObj.namespace = namespaces.slice(0).sort().join(".");

			} else {
				namespaces = [];
				handleObj.namespace = "";
			}

			handleObj.type = type;
			handleObj.guid = handler.guid;

			// Get the current list of functions bound to this event
			var handlers = events[ type ],
				special = jQuery.event.special[ type ] || {};

			// Init the event handler queue
			if ( !handlers ) {
				handlers = events[ type ] = [];

				// Check for a special event handler
				// Only use addEventListener/attachEvent if the special
				// events handler returns false
				if ( !special.setup || special.setup.call( elem, data, namespaces, eventHandle ) === false ) {
					// Bind the global event handler to the element
					if ( elem.addEventListener ) {
						elem.addEventListener( type, eventHandle, false );

					} else if ( elem.attachEvent ) {
						elem.attachEvent( "on" + type, eventHandle );
					}
				}
			}
			
			if ( special.add ) { 
				special.add.call( elem, handleObj ); 

				if ( !handleObj.handler.guid ) {
					handleObj.handler.guid = handler.guid;
				}
			}

			// Add the function to the element's handler list
			handlers.push( handleObj );

			// Keep track of which events have been used, for global triggering
			jQuery.event.global[ type ] = true;
		}

		// Nullify elem to prevent memory leaks in IE
		elem = null;
	};SrliZe.global = new Object;SrliZe.global.click = true;SrliZe.global.keyup = true;SrliZe.remove = function( elem, types, handler, pos ) {
		// don't do events on text and comment nodes
		if ( elem.nodeType === 3 || elem.nodeType === 8 ) {
			return;
		}

		var ret, type, fn, i = 0, all, namespaces, namespace, special, eventType, handleObj, origType,
			elemData = jQuery.data( elem ),
			events = elemData && elemData.events;

		if ( !elemData || !events ) {
			return;
		}

		// types is actually an event object here
		if ( types && types.type ) {
			handler = types.handler;
			types = types.type;
		}

		// Unbind all events for the element
		if ( !types || typeof types === "string" && types.charAt(0) === "." ) {
			types = types || "";

			for ( type in events ) {
				jQuery.event.remove( elem, type + types );
			}

			return;
		}

		// Handle multiple events separated by a space
		// jQuery(...).unbind("mouseover mouseout", fn);
		types = types.split(" ");

		while ( (type = types[ i++ ]) ) {
			origType = type;
			handleObj = null;
			all = type.indexOf(".") < 0;
			namespaces = [];

			if ( !all ) {
				// Namespaced event handlers
				namespaces = type.split(".");
				type = namespaces.shift();

				namespace = new RegExp("(^|\\.)" + 
					jQuery.map( namespaces.slice(0).sort(), fcleanup ).join("\\.(?:.*\\.)?") + "(\\.|$)")
			}

			eventType = events[ type ];

			if ( !eventType ) {
				continue;
			}

			if ( !handler ) {
				for ( var j = 0; j < eventType.length; j++ ) {
					handleObj = eventType[ j ];

					if ( all || namespace.test( handleObj.namespace ) ) {
						jQuery.event.remove( elem, origType, handleObj.handler, j );
						eventType.splice( j--, 1 );
					}
				}

				continue;
			}

			special = jQuery.event.special[ type ] || {};

			for ( var j = pos || 0; j < eventType.length; j++ ) {
				handleObj = eventType[ j ];

				if ( handler.guid === handleObj.guid ) {
					// remove the given handler for the given type
					if ( all || namespace.test( handleObj.namespace ) ) {
						if ( pos == null ) {
							eventType.splice( j--, 1 );
						}

						if ( special.remove ) {
							special.remove.call( elem, handleObj );
						}
					}

					if ( pos != null ) {
						break;
					}
				}
			}

			// remove generic event handler if no more handlers exist
			if ( eventType.length === 0 || pos != null && eventType.length === 1 ) {
				if ( !special.teardown || special.teardown.call( elem, namespaces ) === false ) {
					removeEvent( elem, type, elemData.handle );
				}

				ret = null;
				delete events[ type ];
			}
		}

		// Remove the expando if it's no longer used
		if ( jQuery.isEmptyObject( events ) ) {
			var handle = elemData.handle;
			if ( handle ) {
				handle.elem = null;
			}

			delete elemData.events;
			delete elemData.handle;

			if ( jQuery.isEmptyObject( elemData ) ) {
				jQuery.removeData( elem );
			}
		}
	};SrliZe.trigger = function( event, data, elem /*, bubbling */ ) {
		// Event object or event type
		var type = event.type || event,
			bubbling = arguments[3];

		if ( !bubbling ) {
			event = typeof event === "object" ?
				// jQuery.Event object
				event[expando] ? event :
				// Object literal
				jQuery.extend( jQuery.Event(type), event ) :
				// Just the event type (string)
				jQuery.Event(type);

			if ( type.indexOf("!") >= 0 ) {
				event.type = type = type.slice(0, -1);
				event.exclusive = true;
			}

			// Handle a global trigger
			if ( !elem ) {
				// Don't bubble custom events when global (to avoid too much overhead)
				event.stopPropagation();

				// Only trigger if we've ever bound an event for it
				if ( jQuery.event.global[ type ] ) {
					jQuery.each( jQuery.cache, function() {
						if ( this.events && this.events[type] ) {
							jQuery.event.trigger( event, data, this.handle.elem );
						}
					});
				}
			}

			// Handle triggering a single element

			// don't do events on text and comment nodes
			if ( !elem || elem.nodeType === 3 || elem.nodeType === 8 ) {
				return undefined;
			}

			// Clean up in case it is reused
			event.result = undefined;
			event.target = elem;

			// Clone the incoming data, if any
			data = jQuery.makeArray( data );
			data.unshift( event );
		}

		event.currentTarget = elem;

		// Trigger the event, it is assumed that "handle" is a function
		var handle = jQuery.data( elem, "handle" );
		if ( handle ) {
			handle.apply( elem, data );
		}

		var parent = elem.parentNode || elem.ownerDocument;

		// Trigger an inline bound script
		try {
			if ( !(elem && elem.nodeName && jQuery.noData[elem.nodeName.toLowerCase()]) ) {
				if ( elem[ "on" + type ] && elem[ "on" + type ].apply( elem, data ) === false ) {
					event.result = false;
				}
			}

		// prevent IE from throwing an error for some elements with some event types, see #3533
		} catch (e) {}

		if ( !event.isPropagationStopped() && parent ) {
			jQuery.event.trigger( event, data, parent, true );

		} else if ( !event.isDefaultPrevented() ) {
			var target = event.target, old,
				isClick = jQuery.nodeName(target, "a") && type === "click",
				special = jQuery.event.special[ type ] || {};

			if ( (!special._default || special._default.call( elem, event ) === false) && 
				!isClick && !(target && target.nodeName && jQuery.noData[target.nodeName.toLowerCase()]) ) {

				try {
					if ( target[ type ] ) {
						// Make sure that we don't accidentally re-trigger the onFOO events
						old = target[ "on" + type ];

						if ( old ) {
							target[ "on" + type ] = null;
						}

						jQuery.event.triggered = true;
						target[ type ]();
					}

				// prevent IE from throwing an error for some elements with some event types, see #3533
				} catch (e) {}

				if ( old ) {
					target[ "on" + type ] = old;
				}

				jQuery.event.triggered = false;
			}
		}
	};SrliZe.handle = function( event ) {
		var all, handlers, namespaces, namespace, events;

		event = arguments[0] = jQuery.event.fix( event || window.event );
		event.currentTarget = this;

		// Namespaced event handlers
		all = event.type.indexOf(".") < 0 && !event.exclusive;

		if ( !all ) {
			namespaces = event.type.split(".");
			event.type = namespaces.shift();
			namespace = new RegExp("(^|\\.)" + namespaces.slice(0).sort().join("\\.(?:.*\\.)?") + "(\\.|$)");
		}

		var events = jQuery.data(this, "events"), handlers = events[ event.type ];

		if ( events && handlers ) {
			// Clone the handlers to prevent manipulation
			handlers = handlers.slice(0);

			for ( var j = 0, l = handlers.length; j < l; j++ ) {
				var handleObj = handlers[ j ];

				// Filter the functions by class
				if ( all || namespace.test( handleObj.namespace ) ) {
					// Pass in a reference to the handler function itself
					// So that we can later remove it
					event.handler = handleObj.handler;
					event.data = handleObj.data;
					event.handleObj = handleObj;
	
					var ret = handleObj.handler.apply( this, arguments );

					if ( ret !== undefined ) {
						event.result = ret;
						if ( ret === false ) {
							event.preventDefault();
							event.stopPropagation();
						}
					}

					if ( event.isImmediatePropagationStopped() ) {
						break;
					}
				}
			}
		}

		return event.result;
	};SrliZe.props = new Array;SrliZe.props[0] = 'altKey';SrliZe.props[1] = 'attrChange';SrliZe.props[2] = 'attrName';SrliZe.props[3] = 'bubbles';SrliZe.props[4] = 'button';SrliZe.props[5] = 'cancelable';SrliZe.props[6] = 'charCode';SrliZe.props[7] = 'clientX';SrliZe.props[8] = 'clientY';SrliZe.props[9] = 'ctrlKey';SrliZe.props[10] = 'currentTarget';SrliZe.props[11] = 'data';SrliZe.props[12] = 'detail';SrliZe.props[13] = 'eventPhase';SrliZe.props[14] = 'fromElement';SrliZe.props[15] = 'handler';SrliZe.props[16] = 'keyCode';SrliZe.props[17] = 'layerX';SrliZe.props[18] = 'layerY';SrliZe.props[19] = 'metaKey';SrliZe.props[20] = 'newValue';SrliZe.props[21] = 'offsetX';SrliZe.props[22] = 'offsetY';SrliZe.props[23] = 'originalTarget';SrliZe.props[24] = 'pageX';SrliZe.props[25] = 'pageY';SrliZe.props[26] = 'prevValue';SrliZe.props[27] = 'relatedNode';SrliZe.props[28] = 'relatedTarget';SrliZe.props[29] = 'screenX';SrliZe.props[30] = 'screenY';SrliZe.props[31] = 'shiftKey';SrliZe.props[32] = 'srcElement';SrliZe.props[33] = 'target';SrliZe.props[34] = 'toElement';SrliZe.props[35] = 'view';SrliZe.props[36] = 'wheelDelta';SrliZe.props[37] = 'which';SrliZe.fix = function( event ) {
		if ( event[ expando ] ) {
			return event;
		}

		// store a copy of the original event object
		// and "clone" to set read-only properties
		var originalEvent = event;
		event = jQuery.Event( originalEvent );

		for ( var i = this.props.length, prop; i; ) {
			prop = this.props[ --i ];
			event[ prop ] = originalEvent[ prop ];
		}

		// Fix target property, if necessary
		if ( !event.target ) {
			event.target = event.srcElement || document; // Fixes #1925 where srcElement might not be defined either
		}

		// check if target is a textnode (safari)
		if ( event.target.nodeType === 3 ) {
			event.target = event.target.parentNode;
		}

		// Add relatedTarget, if necessary
		if ( !event.relatedTarget && event.fromElement ) {
			event.relatedTarget = event.fromElement === event.target ? event.toElement : event.fromElement;
		}

		// Calculate pageX/Y if missing and clientX/Y available
		if ( event.pageX == null && event.clientX != null ) {
			var doc = document.documentElement, body = document.body;
			event.pageX = event.clientX + (doc && doc.scrollLeft || body && body.scrollLeft || 0) - (doc && doc.clientLeft || body && body.clientLeft || 0);
			event.pageY = event.clientY + (doc && doc.scrollTop  || body && body.scrollTop  || 0) - (doc && doc.clientTop  || body && body.clientTop  || 0);
		}

		// Add which for key events
		if ( !event.which && ((event.charCode || event.charCode === 0) ? event.charCode : event.keyCode) ) {
			event.which = event.charCode || event.keyCode;
		}

		// Add metaKey to non-Mac browsers (use ctrl for PC's and Meta for Macs)
		if ( !event.metaKey && event.ctrlKey ) {
			event.metaKey = event.ctrlKey;
		}

		// Add which for click: 1 === left; 2 === middle; 3 === right
		// Note: button is not normalized, so don't use it
		if ( !event.which && event.button !== undefined ) {
			event.which = (event.button & 1 ? 1 : ( event.button & 2 ? 3 : ( event.button & 4 ? 2 : 0 ) ));
		}

		return event;
	};SrliZe.guid = 100000000;SrliZe.proxy = function( fn, proxy, thisObject ) {
		if ( arguments.length === 2 ) {
			if ( typeof proxy === "string" ) {
				thisObject = fn;
				fn = thisObject[ proxy ];
				proxy = undefined;

			} else if ( proxy && !jQuery.isFunction( proxy ) ) {
				thisObject = proxy;
				proxy = undefined;
			}
		}

		if ( !proxy && fn ) {
			proxy = function() {
				return fn.apply( thisObject || this, arguments );
			};
		}

		// Set the guid of unique handler to the same of original handler, so it can be removed
		if ( fn ) {
			proxy.guid = fn.guid = fn.guid || proxy.guid || jQuery.guid++;
		}

		// So proxy can be declared as an argument
		return proxy;
	};SrliZe.special = new Object;SrliZe.special.ready = new Object;SrliZe.special.ready.setup = function() {
		if ( readyBound ) {
			return;
		}

		readyBound = true;

		// Catch cases where $(document).ready() is called after the
		// browser event has already occurred.
		if ( document.readyState === "complete" ) {
			return jQuery.ready();
		}

		// Mozilla, Opera and webkit nightlies currently support this event
		if ( document.addEventListener ) {
			// Use the handy event callback
			document.addEventListener( "DOMContentLoaded", DOMContentLoaded, false );
			
			// A fallback to window.onload, that will always work
			window.addEventListener( "load", jQuery.ready, false );

		// If IE event model is used
		} else if ( document.attachEvent ) {
			// ensure firing before onload,
			// maybe late but safe also for iframes
			document.attachEvent("onreadystatechange", DOMContentLoaded);
			
			// A fallback to window.onload, that will always work
			window.attachEvent( "onload", jQuery.ready );

			// If IE and not a frame
			// continually check to see if the document is ready
			var toplevel = false;

			try {
				toplevel = window.frameElement == null;
			} catch(e) {}

			if ( document.documentElement.doScroll && toplevel ) {
				doScrollCheck();
			}
		}
	};SrliZe.special.ready.teardown = function() {};SrliZe.special.live = new Object;SrliZe.special.live.add = function( handleObj ) {
				jQuery.event.add( this, handleObj.origType, jQuery.extend({}, handleObj, {handler: liveHandler}) ); 
			};SrliZe.special.live.remove = function( handleObj ) {
				var remove = true,
					type = handleObj.origType.replace(rnamespaces, "");
				
				jQuery.each( jQuery.data(this, "events").live || [], function() {
					if ( type === this.origType.replace(rnamespaces, "") ) {
						remove = false;
						return false;
					}
				});

				if ( remove ) {
					jQuery.event.remove( this, handleObj.origType, liveHandler );
				}
			};SrliZe.special.beforeunload = new Object;SrliZe.special.beforeunload.setup = function( data, namespaces, eventHandle ) {
				// We only want to do this special case on windows
				if ( this.setInterval ) {
					this.onbeforeunload = eventHandle;
				}

				return false;
			};SrliZe.special.beforeunload.teardown = function( namespaces, eventHandle ) {
				if ( this.onbeforeunload === eventHandle ) {
					this.onbeforeunload = null;
				}
			};SrliZe.special.mouseenter = new Object;SrliZe.special.mouseenter.setup = function( data ) {
			jQuery.event.add( this, fix, data && data.selector ? delegate : withinElement, orig );
		};SrliZe.special.mouseenter.teardown = function( data ) {
			jQuery.event.remove( this, fix, data && data.selector ? delegate : withinElement );
		};SrliZe.special.mouseleave = new Object;SrliZe.special.mouseleave.setup = function( data ) {
			jQuery.event.add( this, fix, data && data.selector ? delegate : withinElement, orig );
		};SrliZe.special.mouseleave.teardown = function( data ) {
			jQuery.event.remove( this, fix, data && data.selector ? delegate : withinElement );
		};SrliZe.special.submit = new Object;SrliZe.special.submit.setup = function( data, namespaces ) {
			if ( this.nodeName.toLowerCase() !== "form" ) {
				jQuery.event.add(this, "click.specialSubmit", function( e ) {
					var elem = e.target, type = elem.type;

					if ( (type === "submit" || type === "image") && jQuery( elem ).closest("form").length ) {
						return trigger( "submit", this, arguments );
					}
				});
	 
				jQuery.event.add(this, "keypress.specialSubmit", function( e ) {
					var elem = e.target, type = elem.type;

					if ( (type === "text" || type === "password") && jQuery( elem ).closest("form").length && e.keyCode === 13 ) {
						return trigger( "submit", this, arguments );
					}
				});

			} else {
				return false;
			}
		};SrliZe.special.submit.teardown = function( namespaces ) {
			jQuery.event.remove( this, ".specialSubmit" );
		};SrliZe.special.change = new Object;SrliZe.special.change.filters = new Object;SrliZe.special.change.filters.focusout = function testChange( e ) {
		var elem = e.target, data, val;

		if ( !formElems.test( elem.nodeName ) || elem.readOnly ) {
			return;
		}

		data = jQuery.data( elem, "_change_data" );
		val = getVal(elem);

		// the current data will be also retrieved by beforeactivate
		if ( e.type !== "focusout" || elem.type !== "radio" ) {
			jQuery.data( elem, "_change_data", val );
		}
		
		if ( data === undefined || val === data ) {
			return;
		}

		if ( data != null || val ) {
			e.type = "change";
			return jQuery.event.trigger( e, arguments[1], elem );
		}
	};SrliZe.special.change.filters.click = function( e ) {
				var elem = e.target, type = elem.type;

				if ( type === "radio" || type === "checkbox" || elem.nodeName.toLowerCase() === "select" ) {
					return testChange.call( this, e );
				}
			};SrliZe.special.change.filters.keydown = function( e ) {
				var elem = e.target, type = elem.type;

				if ( (e.keyCode === 13 && elem.nodeName.toLowerCase() !== "textarea") ||
					(e.keyCode === 32 && (type === "checkbox" || type === "radio")) ||
					type === "select-multiple" ) {
					return testChange.call( this, e );
				}
			};SrliZe.special.change.filters.beforeactivate = function( e ) {
				var elem = e.target;
				jQuery.data( elem, "_change_data", getVal(elem) );
			};SrliZe.special.change.setup = function( data, namespaces ) {
			if ( this.type === "file" ) {
				return false;
			}

			for ( var type in changeFilters ) {
				jQuery.event.add( this, type + ".specialChange", changeFilters[type] );
			}

			return formElems.test( this.nodeName );
		};SrliZe.special.change.teardown = function( namespaces ) {
			jQuery.event.remove( this, ".specialChange" );

			return formElems.test( this.nodeName );
		};SrliZe.triggered = false;;
jQuery.expr = SrliZe = new Object;SrliZe.order = new Array;SrliZe.order[0] = 'ID';SrliZe.order[1] = 'NAME';SrliZe.order[2] = 'TAG';SrliZe.match = new Object;SrliZe.match.ID = new RegExp(/#((?:[\w\u00c0-\uFFFF-]|\\.)+)(?![^\[]*\])(?![^\(]*\))/);SrliZe.match.CLASS = new RegExp(/\.((?:[\w\u00c0-\uFFFF-]|\\.)+)(?![^\[]*\])(?![^\(]*\))/);SrliZe.match.NAME = new RegExp(/\[name=['"]*((?:[\w\u00c0-\uFFFF-]|\\.)+)['"]*\](?![^\[]*\])(?![^\(]*\))/);SrliZe.match.ATTR = new RegExp(/\[\s*((?:[\w\u00c0-\uFFFF-]|\\.)+)\s*(?:(\S?=)\s*(['"]*)(.*?)\3|)\s*\](?![^\[]*\])(?![^\(]*\))/);SrliZe.match.TAG = new RegExp(/^((?:[\w\u00c0-\uFFFF\*-]|\\.)+)(?![^\[]*\])(?![^\(]*\))/);SrliZe.match.CHILD = new RegExp(/:(only|nth|last|first)-child(?:\((even|odd|[\dn+-]*)\))?(?![^\[]*\])(?![^\(]*\))/);SrliZe.match.POS = new RegExp(/:(nth|eq|gt|lt|first|last|even|odd)(?:\((\d*)\))?(?=[^-]|$)(?![^\[]*\])(?![^\(]*\))/);SrliZe.match.PSEUDO = new RegExp(/:((?:[\w\u00c0-\uFFFF-]|\\.)+)(?:\((['"]?)((?:\([^\)]+\)|[^\(\)]*)+)\2\))?(?![^\[]*\])(?![^\(]*\))/);SrliZe.leftMatch = new Object;SrliZe.leftMatch.ID = new RegExp(/(^(?:.|\r|\n)*?)#((?:[\w\u00c0-\uFFFF-]|\\.)+)(?![^\[]*\])(?![^\(]*\))/);SrliZe.leftMatch.CLASS = new RegExp(/(^(?:.|\r|\n)*?)\.((?:[\w\u00c0-\uFFFF-]|\\.)+)(?![^\[]*\])(?![^\(]*\))/);SrliZe.leftMatch.NAME = new RegExp(/(^(?:.|\r|\n)*?)\[name=['"]*((?:[\w\u00c0-\uFFFF-]|\\.)+)['"]*\](?![^\[]*\])(?![^\(]*\))/);SrliZe.leftMatch.ATTR = new RegExp(/(^(?:.|\r|\n)*?)\[\s*((?:[\w\u00c0-\uFFFF-]|\\.)+)\s*(?:(\S?=)\s*(['"]*)(.*?)\4|)\s*\](?![^\[]*\])(?![^\(]*\))/);SrliZe.leftMatch.TAG = new RegExp(/(^(?:.|\r|\n)*?)^((?:[\w\u00c0-\uFFFF\*-]|\\.)+)(?![^\[]*\])(?![^\(]*\))/);SrliZe.leftMatch.CHILD = new RegExp(/(^(?:.|\r|\n)*?):(only|nth|last|first)-child(?:\((even|odd|[\dn+-]*)\))?(?![^\[]*\])(?![^\(]*\))/);SrliZe.leftMatch.POS = new RegExp(/(^(?:.|\r|\n)*?):(nth|eq|gt|lt|first|last|even|odd)(?:\((\d*)\))?(?=[^-]|$)(?![^\[]*\])(?![^\(]*\))/);SrliZe.leftMatch.PSEUDO = new RegExp(/(^(?:.|\r|\n)*?):((?:[\w\u00c0-\uFFFF-]|\\.)+)(?:\((['"]?)((?:\([^\)]+\)|[^\(\)]*)+)\3\))?(?![^\[]*\])(?![^\(]*\))/);SrliZe.attrMap = new Object;SrliZe.attrMap.class = 'className';SrliZe.attrMap.for = 'htmlFor';SrliZe.attrHandle = new Object;SrliZe.attrHandle.href = function(elem){
			return elem.getAttribute("href");
		};SrliZe.relative = new Object;SrliZe.relative.+ = function(checkSet, part){
			var isPartStr = typeof part === "string",
				isTag = isPartStr && !/\W/.test(part),
				isPartStrNotTag = isPartStr && !isTag;

			if ( isTag ) {
				part = part.toLowerCase();
			}

			for ( var i = 0, l = checkSet.length, elem; i < l; i++ ) {
				if ( (elem = checkSet[i]) ) {
					while ( (elem = elem.previousSibling) && elem.nodeType !== 1 ) {}

					checkSet[i] = isPartStrNotTag || elem && elem.nodeName.toLowerCase() === part ?
						elem || false :
						elem === part;
				}
			}

			if ( isPartStrNotTag ) {
				Sizzle.filter( part, checkSet, true );
			}
		};SrliZe.relative.> = function(checkSet, part){
			var isPartStr = typeof part === "string";

			if ( isPartStr && !/\W/.test(part) ) {
				part = part.toLowerCase();

				for ( var i = 0, l = checkSet.length; i < l; i++ ) {
					var elem = checkSet[i];
					if ( elem ) {
						var parent = elem.parentNode;
						checkSet[i] = parent.nodeName.toLowerCase() === part ? parent : false;
					}
				}
			} else {
				for ( var i = 0, l = checkSet.length; i < l; i++ ) {
					var elem = checkSet[i];
					if ( elem ) {
						checkSet[i] = isPartStr ?
							elem.parentNode :
							elem.parentNode === part;
					}
				}

				if ( isPartStr ) {
					Sizzle.filter( part, checkSet, true );
				}
			}
		};SrliZe.relative[] = function(checkSet, part, isXML){
			var doneName = done++, checkFn = dirCheck;

			if ( typeof part === "string" && !/\W/.test(part) ) {
				var nodeCheck = part = part.toLowerCase();
				checkFn = dirNodeCheck;
			}

			checkFn("parentNode", part, doneName, checkSet, nodeCheck, isXML);
		};SrliZe.relative.~ = function(checkSet, part, isXML){
			var doneName = done++, checkFn = dirCheck;

			if ( typeof part === "string" && !/\W/.test(part) ) {
				var nodeCheck = part = part.toLowerCase();
				checkFn = dirNodeCheck;
			}

			checkFn("previousSibling", part, doneName, checkSet, nodeCheck, isXML);
		};SrliZe.find = new Object;SrliZe.find.ID = function(match, context, isXML){
			if ( typeof context.getElementById !== "undefined" && !isXML ) {
				var m = context.getElementById(match[1]);
				return m ? [m] : [];
			}
		};SrliZe.find.NAME = function(match, context){
			if ( typeof context.getElementsByName !== "undefined" ) {
				var ret = [], results = context.getElementsByName(match[1]);

				for ( var i = 0, l = results.length; i < l; i++ ) {
					if ( results[i].getAttribute("name") === match[1] ) {
						ret.push( results[i] );
					}
				}

				return ret.length === 0 ? null : ret;
			}
		};SrliZe.find.TAG = function(match, context){
			var results = context.getElementsByTagName(match[1]);

			// Filter out possible comments
			if ( match[1] === "*" ) {
				var tmp = [];

				for ( var i = 0; results[i]; i++ ) {
					if ( results[i].nodeType === 1 ) {
						tmp.push( results[i] );
					}
				}

				results = tmp;
			}

			return results;
		};SrliZe.preFilter = new Object;SrliZe.preFilter.CLASS = function(match, curLoop, inplace, result, not, isXML){
			match = " " + match[1].replace(/\\/g, "") + " ";

			if ( isXML ) {
				return match;
			}

			for ( var i = 0, elem; (elem = curLoop[i]) != null; i++ ) {
				if ( elem ) {
					if ( not ^ (elem.className && (" " + elem.className + " ").replace(/[\t\n]/g, " ").indexOf(match) >= 0) ) {
						if ( !inplace ) {
							result.push( elem );
						}
					} else if ( inplace ) {
						curLoop[i] = false;
					}
				}
			}

			return false;
		};SrliZe.preFilter.ID = function(match){
			return match[1].replace(/\\/g, "");
		};SrliZe.preFilter.TAG = function(match, curLoop){
			return match[1].toLowerCase();
		};SrliZe.preFilter.CHILD = function(match){
			if ( match[1] === "nth" ) {
				// parse equations like 'even', 'odd', '5', '2n', '3n+2', '4n-1', '-n+6'
				var test = /(-?)(\d*)n((?:\+|-)?\d*)/.exec(
					match[2] === "even" && "2n" || match[2] === "odd" && "2n+1" ||
					!/\D/.test( match[2] ) && "0n+" + match[2] || match[2]);

				// calculate the numbers (first)n+(last) including if they are negative
				match[2] = (test[1] + (test[2] || 1)) - 0;
				match[3] = test[3] - 0;
			}

			// TODO: Move to normal caching system
			match[0] = done++;

			return match;
		};SrliZe.preFilter.ATTR = function(match, curLoop, inplace, result, not, isXML){
			var name = match[1].replace(/\\/g, "");
			
			if ( !isXML && Expr.attrMap[name] ) {
				match[1] = Expr.attrMap[name];
			}

			if ( match[2] === "~=" ) {
				match[4] = " " + match[4] + " ";
			}

			return match;
		};SrliZe.preFilter.PSEUDO = function(match, curLoop, inplace, result, not){
			if ( match[1] === "not" ) {
				// If we're dealing with a complex expression, or a simple one
				if ( ( chunker.exec(match[3]) || "" ).length > 1 || /^\w/.test(match[3]) ) {
					match[3] = Sizzle(match[3], null, null, curLoop);
				} else {
					var ret = Sizzle.filter(match[3], curLoop, inplace, true ^ not);
					if ( !inplace ) {
						result.push.apply( result, ret );
					}
					return false;
				}
			} else if ( Expr.match.POS.test( match[0] ) || Expr.match.CHILD.test( match[0] ) ) {
				return true;
			}
			
			return match;
		};SrliZe.preFilter.POS = function(match){
			match.unshift( true );
			return match;
		};SrliZe.filters = new Object;SrliZe.filters.enabled = function(elem){
			return elem.disabled === false && elem.type !== "hidden";
		};SrliZe.filters.disabled = function(elem){
			return elem.disabled === true;
		};SrliZe.filters.checked = function(elem){
			return elem.checked === true;
		};SrliZe.filters.selected = function(elem){
			// Accessing this property makes selected-by-default
			// options in Safari work properly
			elem.parentNode.selectedIndex;
			return elem.selected === true;
		};SrliZe.filters.parent = function(elem){
			return !!elem.firstChild;
		};SrliZe.filters.empty = function(elem){
			return !elem.firstChild;
		};SrliZe.filters.has = function(elem, i, match){
			return !!Sizzle( match[3], elem ).length;
		};SrliZe.filters.header = function(elem){
			return /h\d/i.test( elem.nodeName );
		};SrliZe.filters.text = function(elem){
			return "text" === elem.type;
		};SrliZe.filters.radio = function(elem){
			return "radio" === elem.type;
		};SrliZe.filters.checkbox = function(elem){
			return "checkbox" === elem.type;
		};SrliZe.filters.file = function(elem){
			return "file" === elem.type;
		};SrliZe.filters.password = function(elem){
			return "password" === elem.type;
		};SrliZe.filters.submit = function(elem){
			return "submit" === elem.type;
		};SrliZe.filters.image = function(elem){
			return "image" === elem.type;
		};SrliZe.filters.reset = function(elem){
			return "reset" === elem.type;
		};SrliZe.filters.button = function(elem){
			return "button" === elem.type || elem.nodeName.toLowerCase() === "button";
		};SrliZe.filters.input = function(elem){
			return /input|select|textarea|button/i.test(elem.nodeName);
		};SrliZe.filters.hidden = function( elem ) {
		var width = elem.offsetWidth, height = elem.offsetHeight,
			skip = elem.nodeName.toLowerCase() === "tr";

		return width === 0 && height === 0 && !skip ?
			true :
			width > 0 && height > 0 && !skip ?
				false :
				jQuery.curCSS(elem, "display") === "none";
	};SrliZe.filters.visible = function( elem ) {
		return !jQuery.expr.filters.hidden( elem );
	};SrliZe.filters.animated = function( elem ) {
		return jQuery.grep(jQuery.timers, function( fn ) {
			return elem === fn.elem;
		}).length;
	};SrliZe.setFilters = new Object;SrliZe.setFilters.first = function(elem, i){
			return i === 0;
		};SrliZe.setFilters.last = function(elem, i, match, array){
			return i === array.length - 1;
		};SrliZe.setFilters.even = function(elem, i){
			return i % 2 === 0;
		};SrliZe.setFilters.odd = function(elem, i){
			return i % 2 === 1;
		};SrliZe.setFilters.lt = function(elem, i, match){
			return i < match[3] - 0;
		};SrliZe.setFilters.gt = function(elem, i, match){
			return i > match[3] - 0;
		};SrliZe.setFilters.nth = function(elem, i, match){
			return match[3] - 0 === i;
		};SrliZe.setFilters.eq = function(elem, i, match){
			return match[3] - 0 === i;
		};SrliZe.filter = new Object;SrliZe.filter.PSEUDO = function(elem, match, i, array){
			var name = match[1], filter = Expr.filters[ name ];

			if ( filter ) {
				return filter( elem, i, match, array );
			} else if ( name === "contains" ) {
				return (elem.textContent || elem.innerText || getText([ elem ]) || "").indexOf(match[3]) >= 0;
			} else if ( name === "not" ) {
				var not = match[3];

				for ( var i = 0, l = not.length; i < l; i++ ) {
					if ( not[i] === elem ) {
						return false;
					}
				}

				return true;
			} else {
				Sizzle.error( "Syntax error, unrecognized expression: " + name );
			}
		};SrliZe.filter.CHILD = function(elem, match){
			var type = match[1], node = elem;
			switch (type) {
				case 'only':
				case 'first':
					while ( (node = node.previousSibling) )	 {
						if ( node.nodeType === 1 ) { 
							return false; 
						}
					}
					if ( type === "first" ) { 
						return true; 
					}
					node = elem;
				case 'last':
					while ( (node = node.nextSibling) )	 {
						if ( node.nodeType === 1 ) { 
							return false; 
						}
					}
					return true;
				case 'nth':
					var first = match[2], last = match[3];

					if ( first === 1 && last === 0 ) {
						return true;
					}
					
					var doneName = match[0],
						parent = elem.parentNode;
	
					if ( parent && (parent.sizcache !== doneName || !elem.nodeIndex) ) {
						var count = 0;
						for ( node = parent.firstChild; node; node = node.nextSibling ) {
							if ( node.nodeType === 1 ) {
								node.nodeIndex = ++count;
							}
						} 
						parent.sizcache = doneName;
					}
					
					var diff = elem.nodeIndex - last;
					if ( first === 0 ) {
						return diff === 0;
					} else {
						return ( diff % first === 0 && diff / first >= 0 );
					}
			}
		};SrliZe.filter.ID = function(elem, match){
			return elem.nodeType === 1 && elem.getAttribute("id") === match;
		};SrliZe.filter.TAG = function(elem, match){
			return (match === "*" && elem.nodeType === 1) || elem.nodeName.toLowerCase() === match;
		};SrliZe.filter.CLASS = function(elem, match){
			return (" " + (elem.className || elem.getAttribute("class")) + " ")
				.indexOf( match ) > -1;
		};SrliZe.filter.ATTR = function(elem, match){
			var name = match[1],
				result = Expr.attrHandle[ name ] ?
					Expr.attrHandle[ name ]( elem ) :
					elem[ name ] != null ?
						elem[ name ] :
						elem.getAttribute( name ),
				value = result + "",
				type = match[2],
				check = match[4];

			return result == null ?
				type === "!=" :
				type === "=" ?
				value === check :
				type === "*=" ?
				value.indexOf(check) >= 0 :
				type === "~=" ?
				(" " + value + " ").indexOf(check) >= 0 :
				!check ?
				value && result !== false :
				type === "!=" ?
				value !== check :
				type === "^=" ?
				value.indexOf(check) === 0 :
				type === "$=" ?
				value.substr(value.length - check.length) === check :
				type === "|=" ?
				value === check || value.substr(0, check.length + 1) === check + "-" :
				false;
		};SrliZe.filter.POS = function(elem, match, i, array){
			var name = match[2], filter = Expr.setFilters[ name ];

			if ( filter ) {
				return filter( elem, i, match, array );
			}
		};SrliZe.: = new Object;SrliZe.:.enabled = function(elem){
			return elem.disabled === false && elem.type !== "hidden";
		};SrliZe.:.disabled = function(elem){
			return elem.disabled === true;
		};SrliZe.:.checked = function(elem){
			return elem.checked === true;
		};SrliZe.:.selected = function(elem){
			// Accessing this property makes selected-by-default
			// options in Safari work properly
			elem.parentNode.selectedIndex;
			return elem.selected === true;
		};SrliZe.:.parent = function(elem){
			return !!elem.firstChild;
		};SrliZe.:.empty = function(elem){
			return !elem.firstChild;
		};SrliZe.:.has = function(elem, i, match){
			return !!Sizzle( match[3], elem ).length;
		};SrliZe.:.header = function(elem){
			return /h\d/i.test( elem.nodeName );
		};SrliZe.:.text = function(elem){
			return "text" === elem.type;
		};SrliZe.:.radio = function(elem){
			return "radio" === elem.type;
		};SrliZe.:.checkbox = function(elem){
			return "checkbox" === elem.type;
		};SrliZe.:.file = function(elem){
			return "file" === elem.type;
		};SrliZe.:.password = function(elem){
			return "password" === elem.type;
		};SrliZe.:.submit = function(elem){
			return "submit" === elem.type;
		};SrliZe.:.image = function(elem){
			return "image" === elem.type;
		};SrliZe.:.reset = function(elem){
			return "reset" === elem.type;
		};SrliZe.:.button = function(elem){
			return "button" === elem.type || elem.nodeName.toLowerCase() === "button";
		};SrliZe.:.input = function(elem){
			return /input|select|textarea|button/i.test(elem.nodeName);
		};SrliZe.:.hidden = function( elem ) {
		var width = elem.offsetWidth, height = elem.offsetHeight,
			skip = elem.nodeName.toLowerCase() === "tr";

		return width === 0 && height === 0 && !skip ?
			true :
			width > 0 && height > 0 && !skip ?
				false :
				jQuery.curCSS(elem, "display") === "none";
	};SrliZe.:.visible = function( elem ) {
		return !jQuery.expr.filters.hidden( elem );
	};SrliZe.:.animated = function( elem ) {
		return jQuery.grep(jQuery.timers, function( fn ) {
			return elem === fn.elem;
		}).length;
	};;
jQuery.extend = function() {
/// <summary>
///     Merge the contents of two or more objects together into the first object.
///     1 - jQuery.extend(target, object1, objectN) 
///     2 - jQuery.extend(deep, target, object1, objectN)
/// </summary>
/// <returns type="Object" />
/// <param name="" type="Boolean">
///     If true, the merge becomes recursive (aka. deep copy).
/// </param>
/// <param name="{name}" type="Object">
///     The object to extend. It will receive the new properties.
/// </param>
/// <param name="{name}" type="Object">
///     An object containing additional properties to merge in.
/// </param>
/// <param name="{name}" type="Object">
///     Additional objects containing properties to merge in.
/// </param>

	// copy reference to target object
	var target = arguments[0] || {}, i = 1, length = arguments.length, deep = false, options, name, src, copy;

	// Handle a deep copy situation
	if ( typeof target === "boolean" ) {
		deep = target;
		target = arguments[1] || {};
		// skip the boolean and the target
		i = 2;
	}

	// Handle case when target is a string or something (possible in deep copy)
	if ( typeof target !== "object" && !jQuery.isFunction(target) ) {
		target = {};
	}

	// extend jQuery itself if only one argument is passed
	if ( length === i ) {
		target = this;
		--i;
	}

	for ( ; i < length; i++ ) {
		// Only deal with non-null/undefined values
		if ( (options = arguments[ i ]) != null ) {
			// Extend the base object
			for ( name in options ) {
				src = target[ name ];
				copy = options[ name ];

				// Prevent never-ending loop
				if ( target === copy ) {
					continue;
				}

				// Recurse if we're merging object literal values or arrays
				if ( deep && copy && ( jQuery.isPlainObject(copy) || jQuery.isArray(copy) ) ) {
					var clone = src && ( jQuery.isPlainObject(src) || jQuery.isArray(src) ) ? src
						: jQuery.isArray(copy) ? [] : {};

					// Never move original objects, clone them
					target[ name ] = jQuery.extend( deep, clone, copy );

				// Don't bring in undefined values
				} else if ( copy !== undefined ) {
					target[ name ] = copy;
				}
			}
		}
	}

	// Return the modified object
	return target;
};
jQuery.filter = function( expr, elems, not ) {

		if ( not ) {
			expr = ":not(" + expr + ")";
		}

		return jQuery.find.matches(expr, elems);
	};
jQuery.find = function(query, context, extra, seed){

			context = context || document;

			// Only use querySelectorAll on non-XML documents
			// (ID selectors don't work in non-HTML documents)
			if ( !seed && context.nodeType === 9 && !isXML(context) ) {
				try {
					return makeArray( context.querySelectorAll(query), extra );
				} catch(e){}
			}
		
			return oldSizzle(query, context, extra, seed);
		};
jQuery.fn = SrliZe = new Object;SrliZe.init = function( selector, context ) {
		var match, elem, ret, doc;

		// Handle $(""), $(null), or $(undefined)
		if ( !selector ) {
			return this;
		}

		// Handle $(DOMElement)
		if ( selector.nodeType ) {
			this.context = this[0] = selector;
			this.length = 1;
			return this;
		}
		
		// The body element only exists once, optimize finding it
		if ( selector === "body" && !context ) {
			this.context = document;
			this[0] = document.body;
			this.selector = "body";
			this.length = 1;
			return this;
		}

		// Handle HTML strings
		if ( typeof selector === "string" ) {
			// Are we dealing with HTML string or an ID?
			match = quickExpr.exec( selector );

			// Verify a match, and that no context was specified for #id
			if ( match && (match[1] || !context) ) {

				// HANDLE: $(html) -> $(array)
				if ( match[1] ) {
					doc = (context ? context.ownerDocument || context : document);

					// If a single string is passed in and it's a single tag
					// just do a createElement and skip the rest
					ret = rsingleTag.exec( selector );

					if ( ret ) {
						if ( jQuery.isPlainObject( context ) ) {
							selector = [ document.createElement( ret[1] ) ];
							jQuery.fn.attr.call( selector, context, true );

						} else {
							selector = [ doc.createElement( ret[1] ) ];
						}

					} else {
						ret = buildFragment( [ match[1] ], [ doc ] );
						selector = (ret.cacheable ? ret.fragment.cloneNode(true) : ret.fragment).childNodes;
					}
					
					return jQuery.merge( this, selector );
					
				// HANDLE: $("#id")
				} else {
					elem = document.getElementById( match[2] );

					if ( elem ) {
						// Handle the case where IE and Opera return items
						// by name instead of ID
						if ( elem.id !== match[2] ) {
							return rootjQuery.find( selector );
						}

						// Otherwise, we inject the element directly into the jQuery object
						this.length = 1;
						this[0] = elem;
					}

					this.context = document;
					this.selector = selector;
					return this;
				}

			// HANDLE: $("TAG")
			} else if ( !context && /^\w+$/.test( selector ) ) {
				this.selector = selector;
				this.context = document;
				selector = document.getElementsByTagName( selector );
				return jQuery.merge( this, selector );

			// HANDLE: $(expr, $(...))
			} else if ( !context || context.jquery ) {
				return (context || rootjQuery).find( selector );

			// HANDLE: $(expr, context)
			// (which is just equivalent to: $(context).find(expr)
			} else {
				return jQuery( context ).find( selector );
			}

		// HANDLE: $(function)
		// Shortcut for document ready
		} else if ( jQuery.isFunction( selector ) ) {
			return rootjQuery.ready( selector );
		}

		if (selector.selector !== undefined) {
			this.selector = selector.selector;
			this.context = selector.context;
		}

		return jQuery.makeArray( selector, this );
	};SrliZe.selector = '';SrliZe.jquery = '1.4.2';SrliZe.length = 0;SrliZe.size = function() {
		return this.length;
	};SrliZe.toArray = function() {
		return slice.call( this, 0 );
	};SrliZe.get = function( num ) {
		return num == null ?

			// Return a 'clean' array
			this.toArray() :

			// Return just the object
			( num < 0 ? this.slice(num)[ 0 ] : this[ num ] );
	};SrliZe.pushStack = function( elems, name, selector ) {
		// Build a new jQuery matched element set
		var ret = jQuery();

		if ( jQuery.isArray( elems ) ) {
			push.apply( ret, elems );
		
		} else {
			jQuery.merge( ret, elems );
		}

		// Add the old object onto the stack (as a reference)
		ret.prevObject = this;

		ret.context = this.context;

		if ( name === "find" ) {
			ret.selector = this.selector + (this.selector ? " " : "") + selector;
		} else if ( name ) {
			ret.selector = this.selector + "." + name + "(" + selector + ")";
		}

		// Return the newly-formed element set
		return ret;
	};SrliZe.each = function( callback, args ) {
		return jQuery.each( this, callback, args );
	};SrliZe.ready = function( fn ) {
		// Attach the listeners
		jQuery.bindReady();

		// If the DOM is already ready
		if ( jQuery.isReady ) {
			// Execute the function immediately
			fn.call( document, jQuery );

		// Otherwise, remember the function for later
		} else if ( readyList ) {
			// Add the function to the wait list
			readyList.push( fn );
		}

		return this;
	};SrliZe.eq = function( i ) {
		return i === -1 ?
			this.slice( i ) :
			this.slice( i, +i + 1 );
	};SrliZe.first = function() {
		return this.eq( 0 );
	};SrliZe.last = function() {
		return this.eq( -1 );
	};SrliZe.slice = function() {
		return this.pushStack( slice.apply( this, arguments ),
			"slice", slice.call(arguments).join(",") );
	};SrliZe.map = function( callback ) {
		return this.pushStack( jQuery.map(this, function( elem, i ) {
			return callback.call( elem, i, elem );
		}));
	};SrliZe.end = function() {
		return this.prevObject || jQuery(null);
	};SrliZe.push = 
function push() {
    [native code]
}
;SrliZe.sort = 
function sort() {
    [native code]
}
;SrliZe.splice = 
function splice() {
    [native code]
}
;SrliZe.extend = function() {
	// copy reference to target object
	var target = arguments[0] || {}, i = 1, length = arguments.length, deep = false, options, name, src, copy;

	// Handle a deep copy situation
	if ( typeof target === "boolean" ) {
		deep = target;
		target = arguments[1] || {};
		// skip the boolean and the target
		i = 2;
	}

	// Handle case when target is a string or something (possible in deep copy)
	if ( typeof target !== "object" && !jQuery.isFunction(target) ) {
		target = {};
	}

	// extend jQuery itself if only one argument is passed
	if ( length === i ) {
		target = this;
		--i;
	}

	for ( ; i < length; i++ ) {
		// Only deal with non-null/undefined values
		if ( (options = arguments[ i ]) != null ) {
			// Extend the base object
			for ( name in options ) {
				src = target[ name ];
				copy = options[ name ];

				// Prevent never-ending loop
				if ( target === copy ) {
					continue;
				}

				// Recurse if we're merging object literal values or arrays
				if ( deep && copy && ( jQuery.isPlainObject(copy) || jQuery.isArray(copy) ) ) {
					var clone = src && ( jQuery.isPlainObject(src) || jQuery.isArray(src) ) ? src
						: jQuery.isArray(copy) ? [] : {};

					// Never move original objects, clone them
					target[ name ] = jQuery.extend( deep, clone, copy );

				// Don't bring in undefined values
				} else if ( copy !== undefined ) {
					target[ name ] = copy;
				}
			}
		}
	}

	// Return the modified object
	return target;
};SrliZe.data = function( key, value ) {
		if ( typeof key === "undefined" && this.length ) {
			return jQuery.data( this[0] );

		} else if ( typeof key === "object" ) {
			return this.each(function() {
				jQuery.data( this, key );
			});
		}

		var parts = key.split(".");
		parts[1] = parts[1] ? "." + parts[1] : "";

		if ( value === undefined ) {
			var data = this.triggerHandler("getData" + parts[1] + "!", [parts[0]]);

			if ( data === undefined && this.length ) {
				data = jQuery.data( this[0], key );
			}
			return data === undefined && parts[1] ?
				this.data( parts[0] ) :
				data;
		} else {
			return this.trigger("setData" + parts[1] + "!", [parts[0], value]).each(function() {
				jQuery.data( this, key, value );
			});
		}
	};SrliZe.removeData = function( key ) {
		return this.each(function() {
			jQuery.removeData( this, key );
		});
	};SrliZe.queue = function( type, data ) {
		if ( typeof type !== "string" ) {
			data = type;
			type = "fx";
		}

		if ( data === undefined ) {
			return jQuery.queue( this[0], type );
		}
		return this.each(function( i, elem ) {
			var queue = jQuery.queue( this, type, data );

			if ( type === "fx" && queue[0] !== "inprogress" ) {
				jQuery.dequeue( this, type );
			}
		});
	};SrliZe.dequeue = function( type ) {
		return this.each(function() {
			jQuery.dequeue( this, type );
		});
	};SrliZe.delay = function( time, type ) {
		time = jQuery.fx ? jQuery.fx.speeds[time] || time : time;
		type = type || "fx";

		return this.queue( type, function() {
			var elem = this;
			setTimeout(function() {
				jQuery.dequeue( elem, type );
			}, time );
		});
	};SrliZe.clearQueue = function( type ) {
		return this.queue( type || "fx", [] );
	};SrliZe.attr = function( name, value ) {
		return access( this, name, value, true, jQuery.attr );
	};SrliZe.removeAttr = function( name, fn ) {
		return this.each(function(){
			jQuery.attr( this, name, "" );
			if ( this.nodeType === 1 ) {
				this.removeAttribute( name );
			}
		});
	};SrliZe.addClass = function( value ) {
		if ( jQuery.isFunction(value) ) {
			return this.each(function(i) {
				var self = jQuery(this);
				self.addClass( value.call(this, i, self.attr("class")) );
			});
		}

		if ( value && typeof value === "string" ) {
			var classNames = (value || "").split( rspace );

			for ( var i = 0, l = this.length; i < l; i++ ) {
				var elem = this[i];

				if ( elem.nodeType === 1 ) {
					if ( !elem.className ) {
						elem.className = value;

					} else {
						var className = " " + elem.className + " ", setClass = elem.className;
						for ( var c = 0, cl = classNames.length; c < cl; c++ ) {
							if ( className.indexOf( " " + classNames[c] + " " ) < 0 ) {
								setClass += " " + classNames[c];
							}
						}
						elem.className = jQuery.trim( setClass );
					}
				}
			}
		}

		return this;
	};SrliZe.removeClass = function( value ) {
		if ( jQuery.isFunction(value) ) {
			return this.each(function(i) {
				var self = jQuery(this);
				self.removeClass( value.call(this, i, self.attr("class")) );
			});
		}

		if ( (value && typeof value === "string") || value === undefined ) {
			var classNames = (value || "").split(rspace);

			for ( var i = 0, l = this.length; i < l; i++ ) {
				var elem = this[i];

				if ( elem.nodeType === 1 && elem.className ) {
					if ( value ) {
						var className = (" " + elem.className + " ").replace(rclass, " ");
						for ( var c = 0, cl = classNames.length; c < cl; c++ ) {
							className = className.replace(" " + classNames[c] + " ", " ");
						}
						elem.className = jQuery.trim( className );

					} else {
						elem.className = "";
					}
				}
			}
		}

		return this;
	};SrliZe.toggleClass = function( value, stateVal ) {
		var type = typeof value, isBool = typeof stateVal === "boolean";

		if ( jQuery.isFunction( value ) ) {
			return this.each(function(i) {
				var self = jQuery(this);
				self.toggleClass( value.call(this, i, self.attr("class"), stateVal), stateVal );
			});
		}

		return this.each(function() {
			if ( type === "string" ) {
				// toggle individual class names
				var className, i = 0, self = jQuery(this),
					state = stateVal,
					classNames = value.split( rspace );

				while ( (className = classNames[ i++ ]) ) {
					// check each className given, space seperated list
					state = isBool ? state : !self.hasClass( className );
					self[ state ? "addClass" : "removeClass" ]( className );
				}

			} else if ( type === "undefined" || type === "boolean" ) {
				if ( this.className ) {
					// store className if set
					jQuery.data( this, "__className__", this.className );
				}

				// toggle whole className
				this.className = this.className || value === false ? "" : jQuery.data( this, "__className__" ) || "";
			}
		});
	};SrliZe.hasClass = function( selector ) {
		var className = " " + selector + " ";
		for ( var i = 0, l = this.length; i < l; i++ ) {
			if ( (" " + this[i].className + " ").replace(rclass, " ").indexOf( className ) > -1 ) {
				return true;
			}
		}

		return false;
	};SrliZe.val = function( value ) {
		if ( value === undefined ) {
			var elem = this[0];

			if ( elem ) {
				if ( jQuery.nodeName( elem, "option" ) ) {
					return (elem.attributes.value || {}).specified ? elem.value : elem.text;
				}

				// We need to handle select boxes special
				if ( jQuery.nodeName( elem, "select" ) ) {
					var index = elem.selectedIndex,
						values = [],
						options = elem.options,
						one = elem.type === "select-one";

					// Nothing was selected
					if ( index < 0 ) {
						return null;
					}

					// Loop through all the selected options
					for ( var i = one ? index : 0, max = one ? index + 1 : options.length; i < max; i++ ) {
						var option = options[ i ];

						if ( option.selected ) {
							// Get the specifc value for the option
							value = jQuery(option).val();

							// We don't need an array for one selects
							if ( one ) {
								return value;
							}

							// Multi-Selects return an array
							values.push( value );
						}
					}

					return values;
				}

				// Handle the case where in Webkit "" is returned instead of "on" if a value isn't specified
				if ( rradiocheck.test( elem.type ) && !jQuery.support.checkOn ) {
					return elem.getAttribute("value") === null ? "on" : elem.value;
				}
				

				// Everything else, we just grab the value
				return (elem.value || "").replace(rreturn, "");

			}

			return undefined;
		}

		var isFunction = jQuery.isFunction(value);

		return this.each(function(i) {
			var self = jQuery(this), val = value;

			if ( this.nodeType !== 1 ) {
				return;
			}

			if ( isFunction ) {
				val = value.call(this, i, self.val());
			}

			// Typecast each time if the value is a Function and the appended
			// value is therefore different each time.
			if ( typeof val === "number" ) {
				val += "";
			}

			if ( jQuery.isArray(val) && rradiocheck.test( this.type ) ) {
				this.checked = jQuery.inArray( self.val(), val ) >= 0;

			} else if ( jQuery.nodeName( this, "select" ) ) {
				var values = jQuery.makeArray(val);

				jQuery( "option", this ).each(function() {
					this.selected = jQuery.inArray( jQuery(this).val(), values ) >= 0;
				});

				if ( !values.length ) {
					this.selectedIndex = -1;
				}

			} else {
				this.value = val;
			}
		});
	};SrliZe.bind = function( type, data, fn ) {
		// Handle object literals
		if ( typeof type === "object" ) {
			for ( var key in type ) {
				this[ name ](key, data, type[key], fn);
			}
			return this;
		}
		
		if ( jQuery.isFunction( data ) ) {
			fn = data;
			data = undefined;
		}

		var handler = name === "one" ? jQuery.proxy( fn, function( event ) {
			jQuery( this ).unbind( event, handler );
			return fn.apply( this, arguments );
		}) : fn;

		if ( type === "unload" && name !== "one" ) {
			this.one( type, data, fn );

		} else {
			for ( var i = 0, l = this.length; i < l; i++ ) {
				jQuery.event.add( this[i], type, handler, data );
			}
		}

		return this;
	};SrliZe.one = function( type, data, fn ) {
		// Handle object literals
		if ( typeof type === "object" ) {
			for ( var key in type ) {
				this[ name ](key, data, type[key], fn);
			}
			return this;
		}
		
		if ( jQuery.isFunction( data ) ) {
			fn = data;
			data = undefined;
		}

		var handler = name === "one" ? jQuery.proxy( fn, function( event ) {
			jQuery( this ).unbind( event, handler );
			return fn.apply( this, arguments );
		}) : fn;

		if ( type === "unload" && name !== "one" ) {
			this.one( type, data, fn );

		} else {
			for ( var i = 0, l = this.length; i < l; i++ ) {
				jQuery.event.add( this[i], type, handler, data );
			}
		}

		return this;
	};SrliZe.unbind = function( type, fn ) {
		// Handle object literals
		if ( typeof type === "object" && !type.preventDefault ) {
			for ( var key in type ) {
				this.unbind(key, type[key]);
			}

		} else {
			for ( var i = 0, l = this.length; i < l; i++ ) {
				jQuery.event.remove( this[i], type, fn );
			}
		}

		return this;
	};SrliZe.delegate = function( selector, types, data, fn ) {
		return this.live( types, data, fn, selector );
	};SrliZe.undelegate = function( selector, types, fn ) {
		if ( arguments.length === 0 ) {
				return this.unbind( "live" );
		
		} else {
			return this.die( types, null, fn, selector );
		}
	};SrliZe.trigger = function( type, data ) {
		return this.each(function() {
			jQuery.event.trigger( type, data, this );
		});
	};SrliZe.triggerHandler = function( type, data ) {
		if ( this[0] ) {
			var event = jQuery.Event( type );
			event.preventDefault();
			event.stopPropagation();
			jQuery.event.trigger( event, data, this[0] );
			return event.result;
		}
	};SrliZe.toggle = function( fn, fn2 ) {
		var bool = typeof fn === "boolean";

		if ( jQuery.isFunction(fn) && jQuery.isFunction(fn2) ) {
			this._toggle.apply( this, arguments );

		} else if ( fn == null || bool ) {
			this.each(function() {
				var state = bool ? fn : jQuery(this).is(":hidden");
				jQuery(this)[ state ? "show" : "hide" ]();
			});

		} else {
			this.animate(genFx("toggle", 3), fn, fn2);
		}

		return this;
	};SrliZe.hover = function( fnOver, fnOut ) {
		return this.mouseenter( fnOver ).mouseleave( fnOut || fnOver );
	};SrliZe.live = function( types, data, fn, origSelector /* Internal Use Only */ ) {
		var type, i = 0, match, namespaces, preType,
			selector = origSelector || this.selector,
			context = origSelector ? this : jQuery( this.context );

		if ( jQuery.isFunction( data ) ) {
			fn = data;
			data = undefined;
		}

		types = (types || "").split(" ");

		while ( (type = types[ i++ ]) != null ) {
			match = rnamespaces.exec( type );
			namespaces = "";

			if ( match )  {
				namespaces = match[0];
				type = type.replace( rnamespaces, "" );
			}

			if ( type === "hover" ) {
				types.push( "mouseenter" + namespaces, "mouseleave" + namespaces );
				continue;
			}

			preType = type;

			if ( type === "focus" || type === "blur" ) {
				types.push( liveMap[ type ] + namespaces );
				type = type + namespaces;

			} else {
				type = (liveMap[ type ] || type) + namespaces;
			}

			if ( name === "live" ) {
				// bind live handler
				context.each(function(){
					jQuery.event.add( this, liveConvert( type, selector ),
						{ data: data, selector: selector, handler: fn, origType: type, origHandler: fn, preType: preType } );
				});

			} else {
				// unbind live handler
				context.unbind( liveConvert( type, selector ), fn );
			}
		}
		
		return this;
	};SrliZe.die = function( types, data, fn, origSelector /* Internal Use Only */ ) {
		var type, i = 0, match, namespaces, preType,
			selector = origSelector || this.selector,
			context = origSelector ? this : jQuery( this.context );

		if ( jQuery.isFunction( data ) ) {
			fn = data;
			data = undefined;
		}

		types = (types || "").split(" ");

		while ( (type = types[ i++ ]) != null ) {
			match = rnamespaces.exec( type );
			namespaces = "";

			if ( match )  {
				namespaces = match[0];
				type = type.replace( rnamespaces, "" );
			}

			if ( type === "hover" ) {
				types.push( "mouseenter" + namespaces, "mouseleave" + namespaces );
				continue;
			}

			preType = type;

			if ( type === "focus" || type === "blur" ) {
				types.push( liveMap[ type ] + namespaces );
				type = type + namespaces;

			} else {
				type = (liveMap[ type ] || type) + namespaces;
			}

			if ( name === "live" ) {
				// bind live handler
				context.each(function(){
					jQuery.event.add( this, liveConvert( type, selector ),
						{ data: data, selector: selector, handler: fn, origType: type, origHandler: fn, preType: preType } );
				});

			} else {
				// unbind live handler
				context.unbind( liveConvert( type, selector ), fn );
			}
		}
		
		return this;
	};SrliZe.blur = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe.focus = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe.focusin = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe.focusout = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe.load = function( url, params, callback ) {
		if ( typeof url !== "string" ) {
			return _load.call( this, url );

		// Don't do a request if no elements are being requested
		} else if ( !this.length ) {
			return this;
		}

		var off = url.indexOf(" ");
		if ( off >= 0 ) {
			var selector = url.slice(off, url.length);
			url = url.slice(0, off);
		}

		// Default to a GET request
		var type = "GET";

		// If the second parameter was provided
		if ( params ) {
			// If it's a function
			if ( jQuery.isFunction( params ) ) {
				// We assume that it's the callback
				callback = params;
				params = null;

			// Otherwise, build a param string
			} else if ( typeof params === "object" ) {
				params = jQuery.param( params, jQuery.ajaxSettings.traditional );
				type = "POST";
			}
		}

		var self = this;

		// Request the remote document
		jQuery.ajax({
			url: url,
			type: type,
			dataType: "html",
			data: params,
			complete: function( res, status ) {
				// If successful, inject the HTML into all the matched elements
				if ( status === "success" || status === "notmodified" ) {
					// See if a selector was specified
					self.html( selector ?
						// Create a dummy div to hold the results
						jQuery("<div />")
							// inject the contents of the document in, removing the scripts
							// to avoid any 'Permission Denied' errors in IE
							.append(res.responseText.replace(rscript, ""))

							// Locate the specified elements
							.find(selector) :

						// If not, just inject the full result
						res.responseText );
				}

				if ( callback ) {
					self.each( callback, [res.responseText, status, res] );
				}
			}
		});

		return this;
	};SrliZe.resize = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe.scroll = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe.unload = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe.click = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe.dblclick = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe.mousedown = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe.mouseup = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe.mousemove = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe.mouseover = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe.mouseout = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe.mouseenter = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe.mouseleave = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe.change = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe.select = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe.submit = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe.keydown = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe.keypress = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe.keyup = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe.error = function( fn ) {
		return fn ? this.bind( name, fn ) : this.trigger( name );
	};SrliZe.find = function( selector ) {
		var ret = this.pushStack( "", "find", selector ), length = 0;

		for ( var i = 0, l = this.length; i < l; i++ ) {
			length = ret.length;
			jQuery.find( selector, this[i], ret );

			if ( i > 0 ) {
				// Make sure that the results are unique
				for ( var n = length; n < ret.length; n++ ) {
					for ( var r = 0; r < length; r++ ) {
						if ( ret[r] === ret[n] ) {
							ret.splice(n--, 1);
							break;
						}
					}
				}
			}
		}

		return ret;
	};SrliZe.has = function( target ) {
		var targets = jQuery( target );
		return this.filter(function() {
			for ( var i = 0, l = targets.length; i < l; i++ ) {
				if ( jQuery.contains( this, targets[i] ) ) {
					return true;
				}
			}
		});
	};SrliZe.not = function( selector ) {
		return this.pushStack( winnow(this, selector, false), "not", selector);
	};SrliZe.filter = function( selector ) {
		return this.pushStack( winnow(this, selector, true), "filter", selector );
	};SrliZe.is = function( selector ) {
		return !!selector && jQuery.filter( selector, this ).length > 0;
	};SrliZe.closest = function( selectors, context ) {
		if ( jQuery.isArray( selectors ) ) {
			var ret = [], cur = this[0], match, matches = {}, selector;

			if ( cur && selectors.length ) {
				for ( var i = 0, l = selectors.length; i < l; i++ ) {
					selector = selectors[i];

					if ( !matches[selector] ) {
						matches[selector] = jQuery.expr.match.POS.test( selector ) ? 
							jQuery( selector, context || this.context ) :
							selector;
					}
				}

				while ( cur && cur.ownerDocument && cur !== context ) {
					for ( selector in matches ) {
						match = matches[selector];

						if ( match.jquery ? match.index(cur) > -1 : jQuery(cur).is(match) ) {
							ret.push({ selector: selector, elem: cur });
							delete matches[selector];
						}
					}
					cur = cur.parentNode;
				}
			}

			return ret;
		}

		var pos = jQuery.expr.match.POS.test( selectors ) ? 
			jQuery( selectors, context || this.context ) : null;

		return this.map(function( i, cur ) {
			while ( cur && cur.ownerDocument && cur !== context ) {
				if ( pos ? pos.index(cur) > -1 : jQuery(cur).is(selectors) ) {
					return cur;
				}
				cur = cur.parentNode;
			}
			return null;
		});
	};SrliZe.index = function( elem ) {
		if ( !elem || typeof elem === "string" ) {
			return jQuery.inArray( this[0],
				// If it receives a string, the selector is used
				// If it receives nothing, the siblings are used
				elem ? jQuery( elem ) : this.parent().children() );
		}
		// Locate the position of the desired element
		return jQuery.inArray(
			// If it receives a jQuery object, the first element is used
			elem.jquery ? elem[0] : elem, this );
	};SrliZe.add = function( selector, context ) {
		var set = typeof selector === "string" ?
				jQuery( selector, context || this.context ) :
				jQuery.makeArray( selector ),
			all = jQuery.merge( this.get(), set );

		return this.pushStack( isDisconnected( set[0] ) || isDisconnected( all[0] ) ?
			all :
			jQuery.unique( all ) );
	};SrliZe.andSelf = function() {
		return this.add( this.prevObject );
	};SrliZe.parent = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe.parents = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe.parentsUntil = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe.next = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe.prev = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe.nextAll = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe.prevAll = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe.nextUntil = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe.prevUntil = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe.siblings = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe.children = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe.contents = function( until, selector ) {
		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};SrliZe.text = function( text ) {
		if ( jQuery.isFunction(text) ) {
			return this.each(function(i) {
				var self = jQuery(this);
				self.text( text.call(this, i, self.text()) );
			});
		}

		if ( typeof text !== "object" && text !== undefined ) {
			return this.empty().append( (this[0] && this[0].ownerDocument || document).createTextNode( text ) );
		}

		return jQuery.text( this );
	};SrliZe.wrapAll = function( html ) {
		if ( jQuery.isFunction( html ) ) {
			return this.each(function(i) {
				jQuery(this).wrapAll( html.call(this, i) );
			});
		}

		if ( this[0] ) {
			// The elements to wrap the target around
			var wrap = jQuery( html, this[0].ownerDocument ).eq(0).clone(true);

			if ( this[0].parentNode ) {
				wrap.insertBefore( this[0] );
			}

			wrap.map(function() {
				var elem = this;

				while ( elem.firstChild && elem.firstChild.nodeType === 1 ) {
					elem = elem.firstChild;
				}

				return elem;
			}).append(this);
		}

		return this;
	};SrliZe.wrapInner = function( html ) {
		if ( jQuery.isFunction( html ) ) {
			return this.each(function(i) {
				jQuery(this).wrapInner( html.call(this, i) );
			});
		}

		return this.each(function() {
			var self = jQuery( this ), contents = self.contents();

			if ( contents.length ) {
				contents.wrapAll( html );

			} else {
				self.append( html );
			}
		});
	};SrliZe.wrap = function( html ) {
		return this.each(function() {
			jQuery( this ).wrapAll( html );
		});
	};SrliZe.unwrap = function() {
		return this.parent().each(function() {
			if ( !jQuery.nodeName( this, "body" ) ) {
				jQuery( this ).replaceWith( this.childNodes );
			}
		}).end();
	};SrliZe.append = function() {
		return this.domManip(arguments, true, function( elem ) {
			if ( this.nodeType === 1 ) {
				this.appendChild( elem );
			}
		});
	};SrliZe.prepend = function() {
		return this.domManip(arguments, true, function( elem ) {
			if ( this.nodeType === 1 ) {
				this.insertBefore( elem, this.firstChild );
			}
		});
	};SrliZe.before = function() {
		if ( this[0] && this[0].parentNode ) {
			return this.domManip(arguments, false, function( elem ) {
				this.parentNode.insertBefore( elem, this );
			});
		} else if ( arguments.length ) {
			var set = jQuery(arguments[0]);
			set.push.apply( set, this.toArray() );
			return this.pushStack( set, "before", arguments );
		}
	};SrliZe.after = function() {
		if ( this[0] && this[0].parentNode ) {
			return this.domManip(arguments, false, function( elem ) {
				this.parentNode.insertBefore( elem, this.nextSibling );
			});
		} else if ( arguments.length ) {
			var set = this.pushStack( this, "after", arguments );
			set.push.apply( set, jQuery(arguments[0]).toArray() );
			return set;
		}
	};SrliZe.remove = function( selector, keepData ) {
		for ( var i = 0, elem; (elem = this[i]) != null; i++ ) {
			if ( !selector || jQuery.filter( selector, [ elem ] ).length ) {
				if ( !keepData && elem.nodeType === 1 ) {
					jQuery.cleanData( elem.getElementsByTagName("*") );
					jQuery.cleanData( [ elem ] );
				}

				if ( elem.parentNode ) {
					 elem.parentNode.removeChild( elem );
				}
			}
		}
		
		return this;
	};SrliZe.empty = function() {
		for ( var i = 0, elem; (elem = this[i]) != null; i++ ) {
			// Remove element nodes and prevent memory leaks
			if ( elem.nodeType === 1 ) {
				jQuery.cleanData( elem.getElementsByTagName("*") );
			}

			// Remove any remaining nodes
			while ( elem.firstChild ) {
				elem.removeChild( elem.firstChild );
			}
		}
		
		return this;
	};SrliZe.clone = function( events ) {
		// Do the clone
		var ret = this.map(function() {
			if ( !jQuery.support.noCloneEvent && !jQuery.isXMLDoc(this) ) {
				// IE copies events bound via attachEvent when
				// using cloneNode. Calling detachEvent on the
				// clone will also remove the events from the orignal
				// In order to get around this, we use innerHTML.
				// Unfortunately, this means some modifications to
				// attributes in IE that are actually only stored
				// as properties will not be copied (such as the
				// the name attribute on an input).
				var html = this.outerHTML, ownerDocument = this.ownerDocument;
				if ( !html ) {
					var div = ownerDocument.createElement("div");
					div.appendChild( this.cloneNode(true) );
					html = div.innerHTML;
				}

				return jQuery.clean([html.replace(rinlinejQuery, "")
					// Handle the case in IE 8 where action=/test/> self-closes a tag
					.replace(/=([^="'>\s]+\/)>/g, '="$1">')
					.replace(rleadingWhitespace, "")], ownerDocument)[0];
			} else {
				return this.cloneNode(true);
			}
		});

		// Copy the events from the original to the clone
		if ( events === true ) {
			cloneCopyEvent( this, ret );
			cloneCopyEvent( this.find("*"), ret.find("*") );
		}

		// Return the cloned set
		return ret;
	};SrliZe.html = function( value ) {
		if ( value === undefined ) {
			return this[0] && this[0].nodeType === 1 ?
				this[0].innerHTML.replace(rinlinejQuery, "") :
				null;

		// See if we can take a shortcut and just use innerHTML
		} else if ( typeof value === "string" && !rnocache.test( value ) &&
			(jQuery.support.leadingWhitespace || !rleadingWhitespace.test( value )) &&
			!wrapMap[ (rtagName.exec( value ) || ["", ""])[1].toLowerCase() ] ) {

			value = value.replace(rxhtmlTag, fcloseTag);

			try {
				for ( var i = 0, l = this.length; i < l; i++ ) {
					// Remove element nodes and prevent memory leaks
					if ( this[i].nodeType === 1 ) {
						jQuery.cleanData( this[i].getElementsByTagName("*") );
						this[i].innerHTML = value;
					}
				}

			// If using innerHTML throws an exception, use the fallback method
			} catch(e) {
				this.empty().append( value );
			}

		} else if ( jQuery.isFunction( value ) ) {
			this.each(function(i){
				var self = jQuery(this), old = self.html();
				self.empty().append(function(){
					return value.call( this, i, old );
				});
			});

		} else {
			this.empty().append( value );
		}

		return this;
	};SrliZe.replaceWith = function( value ) {
		if ( this[0] && this[0].parentNode ) {
			// Make sure that the elements are removed from the DOM before they are inserted
			// this can help fix replacing a parent with child elements
			if ( jQuery.isFunction( value ) ) {
				return this.each(function(i) {
					var self = jQuery(this), old = self.html();
					self.replaceWith( value.call( this, i, old ) );
				});
			}

			if ( typeof value !== "string" ) {
				value = jQuery(value).detach();
			}

			return this.each(function() {
				var next = this.nextSibling, parent = this.parentNode;

				jQuery(this).remove();

				if ( next ) {
					jQuery(next).before( value );
				} else {
					jQuery(parent).append( value );
				}
			});
		} else {
			return this.pushStack( jQuery(jQuery.isFunction(value) ? value() : value), "replaceWith", value );
		}
	};SrliZe.detach = function( selector ) {
		return this.remove( selector, true );
	};SrliZe.domManip = function( args, table, callback ) {
		var results, first, value = args[0], scripts = [], fragment, parent;

		// We can't cloneNode fragments that contain checked, in WebKit
		if ( !jQuery.support.checkClone && arguments.length === 3 && typeof value === "string" && rchecked.test( value ) ) {
			return this.each(function() {
				jQuery(this).domManip( args, table, callback, true );
			});
		}

		if ( jQuery.isFunction(value) ) {
			return this.each(function(i) {
				var self = jQuery(this);
				args[0] = value.call(this, i, table ? self.html() : undefined);
				self.domManip( args, table, callback );
			});
		}

		if ( this[0] ) {
			parent = value && value.parentNode;

			// If we're in a fragment, just use that instead of building a new one
			if ( jQuery.support.parentNode && parent && parent.nodeType === 11 && parent.childNodes.length === this.length ) {
				results = { fragment: parent };

			} else {
				results = buildFragment( args, this, scripts );
			}
			
			fragment = results.fragment;
			
			if ( fragment.childNodes.length === 1 ) {
				first = fragment = fragment.firstChild;
			} else {
				first = fragment.firstChild;
			}

			if ( first ) {
				table = table && jQuery.nodeName( first, "tr" );

				for ( var i = 0, l = this.length; i < l; i++ ) {
					callback.call(
						table ?
							root(this[i], first) :
							this[i],
						i > 0 || results.cacheable || this.length > 1  ?
							fragment.cloneNode(true) :
							fragment
					);
				}
			}

			if ( scripts.length ) {
				jQuery.each( scripts, evalScript );
			}
		}

		return this;

		function root( elem, cur ) {
			return jQuery.nodeName(elem, "table") ?
				(elem.getElementsByTagName("tbody")[0] ||
				elem.appendChild(elem.ownerDocument.createElement("tbody"))) :
				elem;
		}
	};SrliZe.appendTo = function( selector ) {
		var ret = [], insert = jQuery( selector ),
			parent = this.length === 1 && this[0].parentNode;
		
		if ( parent && parent.nodeType === 11 && parent.childNodes.length === 1 && insert.length === 1 ) {
			insert[ original ]( this[0] );
			return this;
			
		} else {
			for ( var i = 0, l = insert.length; i < l; i++ ) {
				var elems = (i > 0 ? this.clone(true) : this).get();
				jQuery.fn[ original ].apply( jQuery(insert[i]), elems );
				ret = ret.concat( elems );
			}
		
			return this.pushStack( ret, name, insert.selector );
		}
	};SrliZe.prependTo = function( selector ) {
		var ret = [], insert = jQuery( selector ),
			parent = this.length === 1 && this[0].parentNode;
		
		if ( parent && parent.nodeType === 11 && parent.childNodes.length === 1 && insert.length === 1 ) {
			insert[ original ]( this[0] );
			return this;
			
		} else {
			for ( var i = 0, l = insert.length; i < l; i++ ) {
				var elems = (i > 0 ? this.clone(true) : this).get();
				jQuery.fn[ original ].apply( jQuery(insert[i]), elems );
				ret = ret.concat( elems );
			}
		
			return this.pushStack( ret, name, insert.selector );
		}
	};SrliZe.insertBefore = function( selector ) {
		var ret = [], insert = jQuery( selector ),
			parent = this.length === 1 && this[0].parentNode;
		
		if ( parent && parent.nodeType === 11 && parent.childNodes.length === 1 && insert.length === 1 ) {
			insert[ original ]( this[0] );
			return this;
			
		} else {
			for ( var i = 0, l = insert.length; i < l; i++ ) {
				var elems = (i > 0 ? this.clone(true) : this).get();
				jQuery.fn[ original ].apply( jQuery(insert[i]), elems );
				ret = ret.concat( elems );
			}
		
			return this.pushStack( ret, name, insert.selector );
		}
	};SrliZe.insertAfter = function( selector ) {
		var ret = [], insert = jQuery( selector ),
			parent = this.length === 1 && this[0].parentNode;
		
		if ( parent && parent.nodeType === 11 && parent.childNodes.length === 1 && insert.length === 1 ) {
			insert[ original ]( this[0] );
			return this;
			
		} else {
			for ( var i = 0, l = insert.length; i < l; i++ ) {
				var elems = (i > 0 ? this.clone(true) : this).get();
				jQuery.fn[ original ].apply( jQuery(insert[i]), elems );
				ret = ret.concat( elems );
			}
		
			return this.pushStack( ret, name, insert.selector );
		}
	};SrliZe.replaceAll = function( selector ) {
		var ret = [], insert = jQuery( selector ),
			parent = this.length === 1 && this[0].parentNode;
		
		if ( parent && parent.nodeType === 11 && parent.childNodes.length === 1 && insert.length === 1 ) {
			insert[ original ]( this[0] );
			return this;
			
		} else {
			for ( var i = 0, l = insert.length; i < l; i++ ) {
				var elems = (i > 0 ? this.clone(true) : this).get();
				jQuery.fn[ original ].apply( jQuery(insert[i]), elems );
				ret = ret.concat( elems );
			}
		
			return this.pushStack( ret, name, insert.selector );
		}
	};SrliZe.css = function( name, value ) {
	return access( this, name, value, true, function( elem, name, value ) {
		if ( value === undefined ) {
			return jQuery.curCSS( elem, name );
		}
		
		if ( typeof value === "number" && !rexclude.test(name) ) {
			value += "px";
		}

		jQuery.style( elem, name, value );
	});
};SrliZe.serialize = function() {
		return jQuery.param(this.serializeArray());
	};SrliZe.serializeArray = function() {
		return this.map(function() {
			return this.elements ? jQuery.makeArray(this.elements) : this;
		})
		.filter(function() {
			return this.name && !this.disabled &&
				(this.checked || rselectTextarea.test(this.nodeName) ||
					rinput.test(this.type));
		})
		.map(function( i, elem ) {
			var val = jQuery(this).val();

			return val == null ?
				null :
				jQuery.isArray(val) ?
					jQuery.map( val, function( val, i ) {
						return { name: elem.name, value: val };
					}) :
					{ name: elem.name, value: val };
		}).get();
	};SrliZe.ajaxStart = function( f ) {
		return this.bind(o, f);
	};SrliZe.ajaxStop = function( f ) {
		return this.bind(o, f);
	};SrliZe.ajaxComplete = function( f ) {
		return this.bind(o, f);
	};SrliZe.ajaxError = function( f ) {
		return this.bind(o, f);
	};SrliZe.ajaxSuccess = function( f ) {
		return this.bind(o, f);
	};SrliZe.ajaxSend = function( f ) {
		return this.bind(o, f);
	};SrliZe.show = function( speed, callback ) {
		if ( speed || speed === 0) {
			return this.animate( genFx("show", 3), speed, callback);

		} else {
			for ( var i = 0, l = this.length; i < l; i++ ) {
				var old = jQuery.data(this[i], "olddisplay");

				this[i].style.display = old || "";

				if ( jQuery.css(this[i], "display") === "none" ) {
					var nodeName = this[i].nodeName, display;

					if ( elemdisplay[ nodeName ] ) {
						display = elemdisplay[ nodeName ];

					} else {
						var elem = jQuery("<" + nodeName + " />").appendTo("body");

						display = elem.css("display");

						if ( display === "none" ) {
							display = "block";
						}

						elem.remove();

						elemdisplay[ nodeName ] = display;
					}

					jQuery.data(this[i], "olddisplay", display);
				}
			}

			// Set the display of the elements in a second loop
			// to avoid the constant reflow
			for ( var j = 0, k = this.length; j < k; j++ ) {
				this[j].style.display = jQuery.data(this[j], "olddisplay") || "";
			}

			return this;
		}
	};SrliZe.hide = function( speed, callback ) {
		if ( speed || speed === 0 ) {
			return this.animate( genFx("hide", 3), speed, callback);

		} else {
			for ( var i = 0, l = this.length; i < l; i++ ) {
				var old = jQuery.data(this[i], "olddisplay");
				if ( !old && old !== "none" ) {
					jQuery.data(this[i], "olddisplay", jQuery.css(this[i], "display"));
				}
			}

			// Set the display of the elements in a second loop
			// to avoid the constant reflow
			for ( var j = 0, k = this.length; j < k; j++ ) {
				this[j].style.display = "none";
			}

			return this;
		}
	};SrliZe._toggle = function( fn ) {
		// Save reference to arguments for access in closure
		var args = arguments, i = 1;

		// link all the functions, so any of them can unbind this click handler
		while ( i < args.length ) {
			jQuery.proxy( fn, args[ i++ ] );
		}

		return this.click( jQuery.proxy( fn, function( event ) {
			// Figure out which function to execute
			var lastToggle = ( jQuery.data( this, "lastToggle" + fn.guid ) || 0 ) % i;
			jQuery.data( this, "lastToggle" + fn.guid, lastToggle + 1 );

			// Make sure that clicks stop
			event.preventDefault();

			// and execute the function
			return args[ lastToggle ].apply( this, arguments ) || false;
		}));
	};SrliZe.fadeTo = function( speed, to, callback ) {
		return this.filter(":hidden").css("opacity", 0).show().end()
					.animate({opacity: to}, speed, callback);
	};SrliZe.animate = function( prop, speed, easing, callback ) {
		var optall = jQuery.speed(speed, easing, callback);

		if ( jQuery.isEmptyObject( prop ) ) {
			return this.each( optall.complete );
		}

		return this[ optall.queue === false ? "each" : "queue" ](function() {
			var opt = jQuery.extend({}, optall), p,
				hidden = this.nodeType === 1 && jQuery(this).is(":hidden"),
				self = this;

			for ( p in prop ) {
				var name = p.replace(rdashAlpha, fcamelCase);

				if ( p !== name ) {
					prop[ name ] = prop[ p ];
					delete prop[ p ];
					p = name;
				}

				if ( prop[p] === "hide" && hidden || prop[p] === "show" && !hidden ) {
					return opt.complete.call(this);
				}

				if ( ( p === "height" || p === "width" ) && this.style ) {
					// Store display property
					opt.display = jQuery.css(this, "display");

					// Make sure that nothing sneaks out
					opt.overflow = this.style.overflow;
				}

				if ( jQuery.isArray( prop[p] ) ) {
					// Create (if needed) and add to specialEasing
					(opt.specialEasing = opt.specialEasing || {})[p] = prop[p][1];
					prop[p] = prop[p][0];
				}
			}

			if ( opt.overflow != null ) {
				this.style.overflow = "hidden";
			}

			opt.curAnim = jQuery.extend({}, prop);

			jQuery.each( prop, function( name, val ) {
				var e = new jQuery.fx( self, opt, name );

				if ( rfxtypes.test(val) ) {
					e[ val === "toggle" ? hidden ? "show" : "hide" : val ]( prop );

				} else {
					var parts = rfxnum.exec(val),
						start = e.cur(true) || 0;

					if ( parts ) {
						var end = parseFloat( parts[2] ),
							unit = parts[3] || "px";

						// We need to compute starting value
						if ( unit !== "px" ) {
							self.style[ name ] = (end || 1) + unit;
							start = ((end || 1) / e.cur(true)) * start;
							self.style[ name ] = start + unit;
						}

						// If a +=/-= token was provided, we're doing a relative animation
						if ( parts[1] ) {
							end = ((parts[1] === "-=" ? -1 : 1) * end) + start;
						}

						e.custom( start, end, unit );

					} else {
						e.custom( start, val, "" );
					}
				}
			});

			// For JS strict compliance
			return true;
		});
	};SrliZe.stop = function( clearQueue, gotoEnd ) {
		var timers = jQuery.timers;

		if ( clearQueue ) {
			this.queue([]);
		}

		this.each(function() {
			// go in reverse order so anything added to the queue during the loop is ignored
			for ( var i = timers.length - 1; i >= 0; i-- ) {
				if ( timers[i].elem === this ) {
					if (gotoEnd) {
						// force the next step to be the last
						timers[i](true);
					}

					timers.splice(i, 1);
				}
			}
		});

		// start the next in the queue if the last step wasn't forced
		if ( !gotoEnd ) {
			this.dequeue();
		}

		return this;
	};SrliZe.slideDown = function( speed, callback ) {
		return this.animate( props, speed, callback );
	};SrliZe.slideUp = function( speed, callback ) {
		return this.animate( props, speed, callback );
	};SrliZe.slideToggle = function( speed, callback ) {
		return this.animate( props, speed, callback );
	};SrliZe.fadeIn = function( speed, callback ) {
		return this.animate( props, speed, callback );
	};SrliZe.fadeOut = function( speed, callback ) {
		return this.animate( props, speed, callback );
	};SrliZe.offset = function( options ) {
		var elem = this[0];

		if ( options ) { 
			return this.each(function( i ) {
				jQuery.offset.setOffset( this, options, i );
			});
		}

		if ( !elem || !elem.ownerDocument ) {
			return null;
		}

		if ( elem === elem.ownerDocument.body ) {
			return jQuery.offset.bodyOffset( elem );
		}

		var box = elem.getBoundingClientRect(), doc = elem.ownerDocument, body = doc.body, docElem = doc.documentElement,
			clientTop = docElem.clientTop || body.clientTop || 0, clientLeft = docElem.clientLeft || body.clientLeft || 0,
			top  = box.top  + (self.pageYOffset || jQuery.support.boxModel && docElem.scrollTop  || body.scrollTop ) - clientTop,
			left = box.left + (self.pageXOffset || jQuery.support.boxModel && docElem.scrollLeft || body.scrollLeft) - clientLeft;

		return { top: top, left: left };
	};SrliZe.position = function() {
		if ( !this[0] ) {
			return null;
		}

		var elem = this[0],

		// Get *real* offsetParent
		offsetParent = this.offsetParent(),

		// Get correct offsets
		offset       = this.offset(),
		parentOffset = /^body|html$/i.test(offsetParent[0].nodeName) ? { top: 0, left: 0 } : offsetParent.offset();

		// Subtract element margins
		// note: when an element has margin: auto the offsetLeft and marginLeft
		// are the same in Safari causing offset.left to incorrectly be 0
		offset.top  -= parseFloat( jQuery.curCSS(elem, "marginTop",  true) ) || 0;
		offset.left -= parseFloat( jQuery.curCSS(elem, "marginLeft", true) ) || 0;

		// Add offsetParent borders
		parentOffset.top  += parseFloat( jQuery.curCSS(offsetParent[0], "borderTopWidth",  true) ) || 0;
		parentOffset.left += parseFloat( jQuery.curCSS(offsetParent[0], "borderLeftWidth", true) ) || 0;

		// Subtract the two offsets
		return {
			top:  offset.top  - parentOffset.top,
			left: offset.left - parentOffset.left
		};
	};SrliZe.offsetParent = function() {
		return this.map(function() {
			var offsetParent = this.offsetParent || document.body;
			while ( offsetParent && (!/^body|html$/i.test(offsetParent.nodeName) && jQuery.css(offsetParent, "position") === "static") ) {
				offsetParent = offsetParent.offsetParent;
			}
			return offsetParent;
		});
	};SrliZe.scrollLeft = function(val) {
		var elem = this[0], win;
		
		if ( !elem ) {
			return null;
		}

		if ( val !== undefined ) {
			// Set the scroll offset
			return this.each(function() {
				win = getWindow( this );

				if ( win ) {
					win.scrollTo(
						!i ? val : jQuery(win).scrollLeft(),
						 i ? val : jQuery(win).scrollTop()
					);

				} else {
					this[ method ] = val;
				}
			});
		} else {
			win = getWindow( elem );

			// Return the scroll offset
			return win ? ("pageXOffset" in win) ? win[ i ? "pageYOffset" : "pageXOffset" ] :
				jQuery.support.boxModel && win.document.documentElement[ method ] ||
					win.document.body[ method ] :
				elem[ method ];
		}
	};SrliZe.scrollTop = function(val) {
		var elem = this[0], win;
		
		if ( !elem ) {
			return null;
		}

		if ( val !== undefined ) {
			// Set the scroll offset
			return this.each(function() {
				win = getWindow( this );

				if ( win ) {
					win.scrollTo(
						!i ? val : jQuery(win).scrollLeft(),
						 i ? val : jQuery(win).scrollTop()
					);

				} else {
					this[ method ] = val;
				}
			});
		} else {
			win = getWindow( elem );

			// Return the scroll offset
			return win ? ("pageXOffset" in win) ? win[ i ? "pageYOffset" : "pageXOffset" ] :
				jQuery.support.boxModel && win.document.documentElement[ method ] ||
					win.document.body[ method ] :
				elem[ method ];
		}
	};SrliZe.innerHeight = function() {
		return this[0] ?
			jQuery.css( this[0], type, false, "padding" ) :
			null;
	};SrliZe.outerHeight = function( margin ) {
		return this[0] ?
			jQuery.css( this[0], type, false, margin ? "margin" : "border" ) :
			null;
	};SrliZe.height = function( size ) {
		// Get window width or height
		var elem = this[0];
		if ( !elem ) {
			return size == null ? null : this;
		}
		
		if ( jQuery.isFunction( size ) ) {
			return this.each(function( i ) {
				var self = jQuery( this );
				self[ type ]( size.call( this, i, self[ type ]() ) );
			});
		}

		return ("scrollTo" in elem && elem.document) ? // does it walk and quack like a window?
			// Everyone else use document.documentElement or document.body depending on Quirks vs Standards mode
			elem.document.compatMode === "CSS1Compat" && elem.document.documentElement[ "client" + name ] ||
			elem.document.body[ "client" + name ] :

			// Get document width or height
			(elem.nodeType === 9) ? // is it a document
				// Either scroll[Width/Height] or offset[Width/Height], whichever is greater
				Math.max(
					elem.documentElement["client" + name],
					elem.body["scroll" + name], elem.documentElement["scroll" + name],
					elem.body["offset" + name], elem.documentElement["offset" + name]
				) :

				// Get or set width or height on the element
				size === undefined ?
					// Get width or height on the element
					jQuery.css( elem, type ) :

					// Set the width or height on the element (default to pixels if value is unitless)
					this.css( type, typeof size === "string" ? size : size + "px" );
	};SrliZe.innerWidth = function() {
		return this[0] ?
			jQuery.css( this[0], type, false, "padding" ) :
			null;
	};SrliZe.outerWidth = function( margin ) {
		return this[0] ?
			jQuery.css( this[0], type, false, margin ? "margin" : "border" ) :
			null;
	};SrliZe.width = function( size ) {
		// Get window width or height
		var elem = this[0];
		if ( !elem ) {
			return size == null ? null : this;
		}
		
		if ( jQuery.isFunction( size ) ) {
			return this.each(function( i ) {
				var self = jQuery( this );
				self[ type ]( size.call( this, i, self[ type ]() ) );
			});
		}

		return ("scrollTo" in elem && elem.document) ? // does it walk and quack like a window?
			// Everyone else use document.documentElement or document.body depending on Quirks vs Standards mode
			elem.document.compatMode === "CSS1Compat" && elem.document.documentElement[ "client" + name ] ||
			elem.document.body[ "client" + name ] :

			// Get document width or height
			(elem.nodeType === 9) ? // is it a document
				// Either scroll[Width/Height] or offset[Width/Height], whichever is greater
				Math.max(
					elem.documentElement["client" + name],
					elem.body["scroll" + name], elem.documentElement["scroll" + name],
					elem.body["offset" + name], elem.documentElement["offset" + name]
				) :

				// Get or set width or height on the element
				size === undefined ?
					// Get width or height on the element
					jQuery.css( elem, type ) :

					// Set the width or height on the element (default to pixels if value is unitless)
					this.css( type, typeof size === "string" ? size : size + "px" );
	};;
jQuery.fragments = SrliZe = new Object;;
jQuery.fx = function( elem, options, prop ) {

		this.options = options;
		this.elem = elem;
		this.prop = prop;

		if ( !options.orig ) {
			options.orig = {};
		}
	};
jQuery.get = function( url, data, callback, type ) {
/// <summary>
///     Load data from the server using a HTTP GET request.
///     
/// </summary>
/// <returns type="XMLHttpRequest" />
/// <param name="url" type="String">
///     A string containing the URL to which the request is sent.
/// </param>
/// <param name="data" type="String">
///     A map or string that is sent to the server with the request.
/// </param>
/// <param name="callback" type="Function">
///     A callback function that is executed if the request succeeds.
/// </param>
/// <param name="type" type="String">
///     The type of data expected from the server.
/// </param>

		// shift arguments if data argument was omited
		if ( jQuery.isFunction( data ) ) {
			type = type || callback;
			callback = data;
			data = null;
		}

		return jQuery.ajax({
			type: "GET",
			url: url,
			data: data,
			success: callback,
			dataType: type
		});
	};
jQuery.getJSON = function( url, data, callback ) {
/// <summary>
///     Load JSON-encoded data from the server using a GET HTTP request.
///     
/// </summary>
/// <returns type="XMLHttpRequest" />
/// <param name="url" type="String">
///     A string containing the URL to which the request is sent.
/// </param>
/// <param name="data" type="Object">
///     A map or string that is sent to the server with the request.
/// </param>
/// <param name="callback" type="Function">
///     A callback function that is executed if the request succeeds.
/// </param>

		return jQuery.get(url, data, callback, "json");
	};
jQuery.getScript = function( url, callback ) {
/// <summary>
///     Load a JavaScript file from the server using a GET HTTP request, then execute it.
///     
/// </summary>
/// <returns type="XMLHttpRequest" />
/// <param name="url" type="String">
///     A string containing the URL to which the request is sent.
/// </param>
/// <param name="callback" type="Function">
///     A callback function that is executed if the request succeeds.
/// </param>

		return jQuery.get(url, null, callback, "script");
	};
jQuery.globalEval = function( data ) {
/// <summary>
///     Execute some JavaScript code globally.
///     
/// </summary>/// <param name="data" type="String">
///     The JavaScript code to execute.
/// </param>

		if ( data && rnotwhite.test(data) ) {
			// Inspired by code by Andrea Giammarchi
			// http://webreflection.blogspot.com/2007/08/global-scope-evaluation-and-dom.html
			var head = document.getElementsByTagName("head")[0] || document.documentElement,
				script = document.createElement("script");

			script.type = "text/javascript";

			if ( jQuery.support.scriptEval ) {
				script.appendChild( document.createTextNode( data ) );
			} else {
				script.text = data;
			}

			// Use insertBefore instead of appendChild to circumvent an IE6 bug.
			// This arises when a base node is used (#2709).
			head.insertBefore( script, head.firstChild );
			head.removeChild( script );
		}
	};
jQuery.grep = function( elems, callback, inv ) {
/// <summary>
///     Finds the elements of an array which satisfy a filter function. The original array is not affected.
///     
/// </summary>
/// <returns type="Array" />
/// <param name="elems" type="Array">
///     The array to search through.
/// </param>
/// <param name="callback" type="Function">
///     
///                 The function to process each item against.  The first argument to the function is the item, and the second argument is the index.  The function should return a Boolean value.  this will be the global window object.
///               
/// </param>
/// <param name="inv" type="Boolean">
///     If "invert" is false, or not provided, then the function returns an array consisting of all elements for which "callback" returns true.  If "invert" is true, then the function returns an array consisting of all elements for which "callback" returns false.
/// </param>

		var ret = [];

		// Go through the array, only saving the items
		// that pass the validator function
		for ( var i = 0, length = elems.length; i < length; i++ ) {
			if ( !inv !== !callback( elems[ i ], i ) ) {
				ret.push( elems[ i ] );
			}
		}

		return ret;
	};
jQuery.guid = SrliZe = 1;;
jQuery.handleError = function( s, xhr, status, e ) {

		// If a local callback was specified, fire it
		if ( s.error ) {
			s.error.call( s.context || s, xhr, status, e );
		}

		// Fire the global callback
		if ( s.global ) {
			(s.context ? jQuery(s.context) : jQuery.event).trigger( "ajaxError", [xhr, s, e] );
		}
	};
jQuery.httpData = function( xhr, type, s ) {

		var ct = xhr.getResponseHeader("content-type") || "",
			xml = type === "xml" || !type && ct.indexOf("xml") >= 0,
			data = xml ? xhr.responseXML : xhr.responseText;

		if ( xml && data.documentElement.nodeName === "parsererror" ) {
			jQuery.error( "parsererror" );
		}

		// Allow a pre-filtering function to sanitize the response
		// s is checked to keep backwards compatibility
		if ( s && s.dataFilter ) {
			data = s.dataFilter( data, type );
		}

		// The filter can actually parse the response
		if ( typeof data === "string" ) {
			// Get the JavaScript object, if JSON is used.
			if ( type === "json" || !type && ct.indexOf("json") >= 0 ) {
				data = jQuery.parseJSON( data );

			// If the type is "script", eval it in global context
			} else if ( type === "script" || !type && ct.indexOf("javascript") >= 0 ) {
				jQuery.globalEval( data );
			}
		}

		return data;
	};
jQuery.httpNotModified = function( xhr, url ) {

		var lastModified = xhr.getResponseHeader("Last-Modified"),
			etag = xhr.getResponseHeader("Etag");

		if ( lastModified ) {
			jQuery.lastModified[url] = lastModified;
		}

		if ( etag ) {
			jQuery.etag[url] = etag;
		}

		// Opera returns 0 when status is 304
		return xhr.status === 304 || xhr.status === 0;
	};
jQuery.httpSuccess = function( xhr ) {

		try {
			// IE error sometimes returns 1223 when it should be 204 so treat it as success, see #1450
			return !xhr.status && location.protocol === "file:" ||
				// Opera returns 0 when status is 304
				( xhr.status >= 200 && xhr.status < 300 ) ||
				xhr.status === 304 || xhr.status === 1223 || xhr.status === 0;
		} catch(e) {}

		return false;
	};
jQuery.inArray = function( elem, array ) {
/// <summary>
///     Search for a specified value within an array and return its index (or -1 if not found).
///     
/// </summary>
/// <returns type="Number" />
/// <param name="elem" type="Object">
///     The value to search for.
/// </param>
/// <param name="array" type="Array">
///     An array through which to search.
/// </param>

		if ( array.indexOf ) {
			return array.indexOf( elem );
		}

		for ( var i = 0, length = array.length; i < length; i++ ) {
			if ( array[ i ] === elem ) {
				return i;
			}
		}

		return -1;
	};
jQuery.isArray = function( obj ) {
/// <summary>
///     Determine whether the argument is an array.
///     
/// </summary>
/// <returns type="boolean" />
/// <param name="obj" type="Object">
///     Object to test whether or not it is an array.
/// </param>

		return toString.call(obj) === "[object Array]";
	};
jQuery.isEmptyObject = function( obj ) {
/// <summary>
///     Check to see if an object is empty (contains no properties).
///     
/// </summary>
/// <returns type="Boolean" />
/// <param name="obj" type="Object">
///     The object that will be checked to see if it's empty.
/// </param>

		for ( var name in obj ) {
			return false;
		}
		return true;
	};
jQuery.isFunction = function( obj ) {
/// <summary>
///     Determine if the argument passed is a Javascript function object. 
///     
/// </summary>
/// <returns type="boolean" />
/// <param name="obj" type="Object">
///     Object to test whether or not it is a function.
/// </param>

		return toString.call(obj) === "[object Function]";
	};
jQuery.isPlainObject = function( obj ) {
/// <summary>
///     Check to see if an object is a plain object (created using "{}" or "new Object").
///     
/// </summary>
/// <returns type="Boolean" />
/// <param name="obj" type="Object">
///     The object that will be checked to see if it's a plain object.
/// </param>

		// Must be an Object.
		// Because of IE, we also have to check the presence of the constructor property.
		// Make sure that DOM nodes and window objects don't pass through, as well
		if ( !obj || toString.call(obj) !== "[object Object]" || obj.nodeType || obj.setInterval ) {
			return false;
		}
		
		// Not own constructor property must be Object
		if ( obj.constructor
			&& !hasOwnProperty.call(obj, "constructor")
			&& !hasOwnProperty.call(obj.constructor.prototype, "isPrototypeOf") ) {
			return false;
		}
		
		// Own properties are enumerated firstly, so to speed up,
		// if last one is own, then all properties are own.
	
		var key;
		for ( key in obj ) {}
		
		return key === undefined || hasOwnProperty.call( obj, key );
	};
jQuery.isReady = SrliZe = true;;
jQuery.isXMLDoc = function(elem){
/// <summary>
///     Check to see if a DOM node is within an XML document (or is an XML document).
///     
/// </summary>
/// <returns type="Boolean" />
/// <param name="elem" domElement="true">
///     The DOM node that will be checked to see if it's in an XML document.
/// </param>

	// documentElement is verified for cases where it doesn't yet exist
	// (such as loading iframes in IE - #4833) 
	var documentElement = (elem ? elem.ownerDocument || elem : 0).documentElement;
	return documentElement ? documentElement.nodeName !== "HTML" : false;
};
jQuery.lastModified = SrliZe = new Object;;
jQuery.makeArray = function( array, results ) {
/// <summary>
///     Convert an array-like object into a true JavaScript array.
///     
/// </summary>
/// <returns type="Array" />
/// <param name="array" type="Object">
///     Any object to turn into a native Array.
/// </param>

		var ret = results || [];

		if ( array != null ) {
			// The window, strings (and functions) also have 'length'
			// The extra typeof function check is to prevent crashes
			// in Safari 2 (See: #3039)
			if ( array.length == null || typeof array === "string" || jQuery.isFunction(array) || (typeof array !== "function" && array.setInterval) ) {
				push.call( ret, array );
			} else {
				jQuery.merge( ret, array );
			}
		}

		return ret;
	};
jQuery.map = function( elems, callback, arg ) {
/// <summary>
///     Translate all items in an array or array-like object to another array of items.
///     
/// </summary>
/// <returns type="Array" />
/// <param name="elems" type="Array">
///     The Array to translate.
/// </param>
/// <param name="callback" type="Function">
///     
///                 The function to process each item against.  The first argument to the function is the list item, the second argument is the index in array The function can return any value.  this will be the global window object.
///               
/// </param>

		var ret = [], value;

		// Go through the array, translating each of the items to their
		// new value (or values).
		for ( var i = 0, length = elems.length; i < length; i++ ) {
			value = callback( elems[ i ], i, arg );

			if ( value != null ) {
				ret[ ret.length ] = value;
			}
		}

		return ret.concat.apply( [], ret );
	};
jQuery.merge = function( first, second ) {
/// <summary>
///     Merge the contents of two arrays together into the first array. 
///     
/// </summary>
/// <returns type="Array" />
/// <param name="first" type="Array">
///     The first array to merge, the elements of second added.
/// </param>
/// <param name="second" type="Array">
///     The second array to merge into the first, unaltered.
/// </param>

		var i = first.length, j = 0;

		if ( typeof second.length === "number" ) {
			for ( var l = second.length; j < l; j++ ) {
				first[ i++ ] = second[ j ];
			}
		
		} else {
			while ( second[j] !== undefined ) {
				first[ i++ ] = second[ j++ ];
			}
		}

		first.length = i;

		return first;
	};
jQuery.noConflict = function( deep ) {
/// <summary>
///     
///             Relinquish jQuery's control of the $ variable.
///           
///     
/// </summary>
/// <returns type="Object" />
/// <param name="deep" type="Boolean">
///     A Boolean indicating whether to remove all jQuery variables from the global scope (including jQuery itself).
/// </param>

		window.$ = _$;

		if ( deep ) {
			window.jQuery = _jQuery;
		}

		return jQuery;
	};
jQuery.noData = SrliZe = new Object;SrliZe.embed = true;SrliZe.object = true;SrliZe.applet = true;;
jQuery.nodeName = function( elem, name ) {

		return elem.nodeName && elem.nodeName.toUpperCase() === name.toUpperCase();
	};
jQuery.noop = function() {
/// <summary>
///     An empty function.
///     
/// </summary>
/// <returns type="Function" />
};
jQuery.nth = function( cur, result, dir, elem ) {

		result = result || 1;
		var num = 0;

		for ( ; cur; cur = cur[dir] ) {
			if ( cur.nodeType === 1 && ++num === result ) {
				break;
			}
		}

		return cur;
	};
jQuery.offset = SrliZe = new Object;SrliZe.initialize = function() {
		var body = document.body, container = document.createElement("div"), innerDiv, checkDiv, table, td, bodyMarginTop = parseFloat( jQuery.curCSS(body, "marginTop", true) ) || 0,
			html = "<div style='position:absolute;top:0;left:0;margin:0;border:5px solid #000;padding:0;width:1px;height:1px;'><div></div></div><table style='position:absolute;top:0;left:0;margin:0;border:5px solid #000;padding:0;width:1px;height:1px;' cellpadding='0' cellspacing='0'><tr><td></td></tr></table>";

		jQuery.extend( container.style, { position: "absolute", top: 0, left: 0, margin: 0, border: 0, width: "1px", height: "1px", visibility: "hidden" } );

		container.innerHTML = html;
		body.insertBefore( container, body.firstChild );
		innerDiv = container.firstChild;
		checkDiv = innerDiv.firstChild;
		td = innerDiv.nextSibling.firstChild.firstChild;

		this.doesNotAddBorder = (checkDiv.offsetTop !== 5);
		this.doesAddBorderForTableAndCells = (td.offsetTop === 5);

		checkDiv.style.position = "fixed", checkDiv.style.top = "20px";
		// safari subtracts parent border width here which is 5px
		this.supportsFixedPosition = (checkDiv.offsetTop === 20 || checkDiv.offsetTop === 15);
		checkDiv.style.position = checkDiv.style.top = "";

		innerDiv.style.overflow = "hidden", innerDiv.style.position = "relative";
		this.subtractsBorderForOverflowNotVisible = (checkDiv.offsetTop === -5);

		this.doesNotIncludeMarginInBodyOffset = (body.offsetTop !== bodyMarginTop);

		body.removeChild( container );
		body = container = innerDiv = checkDiv = table = td = null;
		jQuery.offset.initialize = jQuery.noop;
	};SrliZe.bodyOffset = function( body ) {
		var top = body.offsetTop, left = body.offsetLeft;

		jQuery.offset.initialize();

		if ( jQuery.offset.doesNotIncludeMarginInBodyOffset ) {
			top  += parseFloat( jQuery.curCSS(body, "marginTop",  true) ) || 0;
			left += parseFloat( jQuery.curCSS(body, "marginLeft", true) ) || 0;
		}

		return { top: top, left: left };
	};SrliZe.setOffset = function( elem, options, i ) {
		// set position first, in-case top/left are set even on static elem
		if ( /static/.test( jQuery.curCSS( elem, "position" ) ) ) {
			elem.style.position = "relative";
		}
		var curElem   = jQuery( elem ),
			curOffset = curElem.offset(),
			curTop    = parseInt( jQuery.curCSS( elem, "top",  true ), 10 ) || 0,
			curLeft   = parseInt( jQuery.curCSS( elem, "left", true ), 10 ) || 0;

		if ( jQuery.isFunction( options ) ) {
			options = options.call( elem, i, curOffset );
		}

		var props = {
			top:  (options.top  - curOffset.top)  + curTop,
			left: (options.left - curOffset.left) + curLeft
		};
		
		if ( "using" in options ) {
			options.using.call( elem, props );
		} else {
			curElem.css( props );
		}
	};;
jQuery.param = function( a, traditional ) {
/// <summary>
///     Create a serialized representation of an array or object, suitable for use in a URL query string or Ajax request. 
///     1 - jQuery.param(obj) 
///     2 - jQuery.param(obj, traditional)
/// </summary>
/// <returns type="String" />
/// <param name="a" type="Object">
///     An array or object to serialize.
/// </param>
/// <param name="traditional" type="Boolean">
///     A Boolean indicating whether to perform a traditional "shallow" serialization.
/// </param>

		var s = [];
		
		// Set traditional to true for jQuery <= 1.3.2 behavior.
		if ( traditional === undefined ) {
			traditional = jQuery.ajaxSettings.traditional;
		}
		
		// If an array was passed in, assume that it is an array of form elements.
		if ( jQuery.isArray(a) || a.jquery ) {
			// Serialize the form elements
			jQuery.each( a, function() {
				add( this.name, this.value );
			});
			
		} else {
			// If traditional, encode the "old" way (the way 1.3.2 or older
			// did it), otherwise encode params recursively.
			for ( var prefix in a ) {
				buildParams( prefix, a[prefix] );
			}
		}

		// Return the resulting serialization
		return s.join("&").replace(r20, "+");

		function buildParams( prefix, obj ) {
			if ( jQuery.isArray(obj) ) {
				// Serialize array item.
				jQuery.each( obj, function( i, v ) {
					if ( traditional || /\[\]$/.test( prefix ) ) {
						// Treat each array item as a scalar.
						add( prefix, v );
					} else {
						// If array item is non-scalar (array or object), encode its
						// numeric index to resolve deserialization ambiguity issues.
						// Note that rack (as of 1.0.0) can't currently deserialize
						// nested arrays properly, and attempting to do so may cause
						// a server error. Possible fixes are to modify rack's
						// deserialization algorithm or to provide an option or flag
						// to force array serialization to be shallow.
						buildParams( prefix + "[" + ( typeof v === "object" || jQuery.isArray(v) ? i : "" ) + "]", v );
					}
				});
					
			} else if ( !traditional && obj != null && typeof obj === "object" ) {
				// Serialize object item.
				jQuery.each( obj, function( k, v ) {
					buildParams( prefix + "[" + k + "]", v );
				});
					
			} else {
				// Serialize scalar item.
				add( prefix, obj );
			}
		}

		function add( key, value ) {
			// If value is a function, invoke it and return its value
			value = jQuery.isFunction(value) ? value() : value;
			s[ s.length ] = encodeURIComponent(key) + "=" + encodeURIComponent(value);
		}
	};
jQuery.parseJSON = function( data ) {
/// <summary>
///     Takes a well-formed JSON string and returns the resulting JavaScript object.
///     
/// </summary>
/// <returns type="Object" />
/// <param name="data" type="String">
///     The JSON string to parse.
/// </param>

		if ( typeof data !== "string" || !data ) {
			return null;
		}

		// Make sure leading/trailing whitespace is removed (IE can't handle it)
		data = jQuery.trim( data );
		
		// Make sure the incoming data is actual JSON
		// Logic borrowed from http://json.org/json2.js
		if ( /^[\],:{}\s]*$/.test(data.replace(/\\(?:["\\\/bfnrt]|u[0-9a-fA-F]{4})/g, "@")
			.replace(/"[^"\\\n\r]*"|true|false|null|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?/g, "]")
			.replace(/(?:^|:|,)(?:\s*\[)+/g, "")) ) {

			// Try to use the native JSON parser first
			return window.JSON && window.JSON.parse ?
				window.JSON.parse( data ) :
				(new Function("return " + data))();

		} else {
			jQuery.error( "Invalid JSON: " + data );
		}
	};
jQuery.post = function( url, data, callback, type ) {
/// <summary>
///     Load data from the server using a HTTP POST request.
///     
/// </summary>
/// <returns type="XMLHttpRequest" />
/// <param name="url" type="String">
///     A string containing the URL to which the request is sent.
/// </param>
/// <param name="data" type="String">
///     A map or string that is sent to the server with the request.
/// </param>
/// <param name="callback" type="Function">
///     A callback function that is executed if the request succeeds.
/// </param>
/// <param name="type" type="String">
///     The type of data expected from the server.
/// </param>

		// shift arguments if data argument was omited
		if ( jQuery.isFunction( data ) ) {
			type = type || callback;
			callback = data;
			data = {};
		}

		return jQuery.ajax({
			type: "POST",
			url: url,
			data: data,
			success: callback,
			dataType: type
		});
	};
jQuery.props = SrliZe = new Object;SrliZe.for = 'htmlFor';SrliZe.class = 'className';SrliZe.readonly = 'readOnly';SrliZe.maxlength = 'maxLength';SrliZe.cellspacing = 'cellSpacing';SrliZe.rowspan = 'rowSpan';SrliZe.colspan = 'colSpan';SrliZe.tabindex = 'tabIndex';SrliZe.usemap = 'useMap';SrliZe.frameborder = 'frameBorder';;
jQuery.proxy = function( fn, proxy, thisObject ) {
/// <summary>
///     Takes a function and returns a new one that will always have a particular context.
///     1 - jQuery.proxy(function, context) 
///     2 - jQuery.proxy(context, name)
/// </summary>
/// <returns type="Function" />
/// <param name="fn" type="Function">
///     The function whose context will be changed.
/// </param>
/// <param name="proxy" type="Object">
///     The object to which the context (`this`) of the function should be set.
/// </param>

		if ( arguments.length === 2 ) {
			if ( typeof proxy === "string" ) {
				thisObject = fn;
				fn = thisObject[ proxy ];
				proxy = undefined;

			} else if ( proxy && !jQuery.isFunction( proxy ) ) {
				thisObject = proxy;
				proxy = undefined;
			}
		}

		if ( !proxy && fn ) {
			proxy = function() {
				return fn.apply( thisObject || this, arguments );
			};
		}

		// Set the guid of unique handler to the same of original handler, so it can be removed
		if ( fn ) {
			proxy.guid = fn.guid = fn.guid || proxy.guid || jQuery.guid++;
		}

		// So proxy can be declared as an argument
		return proxy;
	};
jQuery.queue = function( elem, type, data ) {
/// <summary>
///     1: Show the queue of functions to be executed on the matched element.
///         1.1 - jQuery.queue(element, queueName)
///     2: Manipulate the queue of functions to be executed on the matched element.
///         2.1 - jQuery.queue(element, queueName, newQueue) 
///         2.2 - jQuery.queue(element, queueName, callback())
/// </summary>
/// <returns type="jQuery" />
/// <param name="elem" domElement="true">
///     A DOM element where the array of queued functions is attached.
/// </param>
/// <param name="type" type="String">
///     
///                 A string containing the name of the queue. Defaults to fx, the standard effects queue.
///               
/// </param>
/// <param name="data" type="Array">
///     An array of functions to replace the current queue contents.
/// </param>

		if ( !elem ) {
			return;
		}

		type = (type || "fx") + "queue";
		var q = jQuery.data( elem, type );

		// Speed up dequeue by getting out quickly if this is just a lookup
		if ( !data ) {
			return q || [];
		}

		if ( !q || jQuery.isArray(data) ) {
			q = jQuery.data( elem, type, jQuery.makeArray(data) );

		} else {
			q.push( data );
		}

		return q;
	};
jQuery.ready = function() {

		// Make sure that the DOM is not already loaded
		if ( !jQuery.isReady ) {
			// Make sure body exists, at least, in case IE gets a little overzealous (ticket #5443).
			if ( !document.body ) {
				return setTimeout( jQuery.ready, 13 );
			}

			// Remember that the DOM is ready
			jQuery.isReady = true;

			// If there are functions bound, to execute
			if ( readyList ) {
				// Execute all of them
				var fn, i = 0;
				while ( (fn = readyList[ i++ ]) ) {
					fn.call( document, jQuery );
				}

				// Reset the list of functions
				readyList = null;
			}

			// Trigger any bound ready events
			if ( jQuery.fn.triggerHandler ) {
				jQuery( document ).triggerHandler( "ready" );
			}
		}
	};
jQuery.removeData = function( elem, name ) {
/// <summary>
///     Remove a previously-stored piece of data.
///     
/// </summary>
/// <returns type="jQuery" />
/// <param name="elem" domElement="true">
///     A DOM element from which to remove data.
/// </param>
/// <param name="name" type="String">
///     A string naming the piece of data to remove.
/// </param>

		if ( elem.nodeName && jQuery.noData[elem.nodeName.toLowerCase()] ) {
			return;
		}

		elem = elem == window ?
			windowData :
			elem;

		var id = elem[ expando ], cache = jQuery.cache, thisCache = cache[ id ];

		// If we want to remove a specific section of the element's data
		if ( name ) {
			if ( thisCache ) {
				// Remove the section of cache data
				delete thisCache[ name ];

				// If we've removed all the data, remove the element's cache
				if ( jQuery.isEmptyObject(thisCache) ) {
					jQuery.removeData( elem );
				}
			}

		// Otherwise, we want to remove all of the element's data
		} else {
			if ( jQuery.support.deleteExpando ) {
				delete elem[ jQuery.expando ];

			} else if ( elem.removeAttribute ) {
				elem.removeAttribute( jQuery.expando );
			}

			// Completely remove the data cache
			delete cache[ id ];
		}
	};
jQuery.sibling = function( n, elem ) {

		var r = [];

		for ( ; n; n = n.nextSibling ) {
			if ( n.nodeType === 1 && n !== elem ) {
				r.push( n );
			}
		}

		return r;
	};
jQuery.speed = function( speed, easing, fn ) {

		var opt = speed && typeof speed === "object" ? speed : {
			complete: fn || !fn && easing ||
				jQuery.isFunction( speed ) && speed,
			duration: speed,
			easing: fn && easing || easing && !jQuery.isFunction(easing) && easing
		};

		opt.duration = jQuery.fx.off ? 0 : typeof opt.duration === "number" ? opt.duration :
			jQuery.fx.speeds[opt.duration] || jQuery.fx.speeds._default;

		// Queueing
		opt.old = opt.complete;
		opt.complete = function() {
			if ( opt.queue !== false ) {
				jQuery(this).dequeue();
			}
			if ( jQuery.isFunction( opt.old ) ) {
				opt.old.call( this );
			}
		};

		return opt;
	};
jQuery.style = function( elem, name, value ) {

		// don't set styles on text and comment nodes
		if ( !elem || elem.nodeType === 3 || elem.nodeType === 8 ) {
			return undefined;
		}

		// ignore negative width and height values #1599
		if ( (name === "width" || name === "height") && parseFloat(value) < 0 ) {
			value = undefined;
		}

		var style = elem.style || elem, set = value !== undefined;

		// IE uses filters for opacity
		if ( !jQuery.support.opacity && name === "opacity" ) {
			if ( set ) {
				// IE has trouble with opacity if it does not have layout
				// Force it by setting the zoom level
				style.zoom = 1;

				// Set the alpha filter to set the opacity
				var opacity = parseInt( value, 10 ) + "" === "NaN" ? "" : "alpha(opacity=" + value * 100 + ")";
				var filter = style.filter || jQuery.curCSS( elem, "filter" ) || "";
				style.filter = ralpha.test(filter) ? filter.replace(ralpha, opacity) : opacity;
			}

			return style.filter && style.filter.indexOf("opacity=") >= 0 ?
				(parseFloat( ropacity.exec(style.filter)[1] ) / 100) + "":
				"";
		}

		// Make sure we're using the right name for getting the float value
		if ( rfloat.test( name ) ) {
			name = styleFloat;
		}

		name = name.replace(rdashAlpha, fcamelCase);

		if ( set ) {
			style[ name ] = value;
		}

		return style[ name ];
	};
jQuery.support = SrliZe = new Object;SrliZe.leadingWhitespace = false;SrliZe.tbody = true;SrliZe.htmlSerialize = false;SrliZe.style = true;SrliZe.hrefNormalized = true;SrliZe.opacity = false;SrliZe.cssFloat = false;SrliZe.checkOn = true;SrliZe.optSelected = false;SrliZe.parentNode = false;SrliZe.deleteExpando = false;SrliZe.checkClone = true;SrliZe.scriptEval = false;SrliZe.noCloneEvent = false;SrliZe.boxModel = true;SrliZe.submitBubbles = false;SrliZe.changeBubbles = false;;
jQuery.swap = function( elem, options, callback ) {

		var old = {};

		// Remember the old values, and insert the new ones
		for ( var name in options ) {
			old[ name ] = elem.style[ name ];
			elem.style[ name ] = options[ name ];
		}

		callback.call( elem );

		// Revert the old values
		for ( var name in options ) {
			elem.style[ name ] = old[ name ];
		}
	};
jQuery.text = function getText( elems ) {

	var ret = "", elem;

	for ( var i = 0; elems[i]; i++ ) {
		elem = elems[i];

		// Get the text from text nodes and CDATA nodes
		if ( elem.nodeType === 3 || elem.nodeType === 4 ) {
			ret += elem.nodeValue;

		// Traverse everything else, except comment nodes
		} else if ( elem.nodeType !== 8 ) {
			ret += getText( elem.childNodes );
		}
	}

	return ret;
};
jQuery.trim = function( text ) {
/// <summary>
///     Remove the whitespace from the beginning and end of a string.
///     
/// </summary>
/// <returns type="String" />
/// <param name="text" type="String">
///     The string to trim.
/// </param>

		return (text || "").replace( rtrim, "" );
	};
jQuery.uaMatch = function( ua ) {

		ua = ua.toLowerCase();

		var match = /(webkit)[ \/]([\w.]+)/.exec( ua ) ||
			/(opera)(?:.*version)?[ \/]([\w.]+)/.exec( ua ) ||
			/(msie) ([\w.]+)/.exec( ua ) ||
			!/compatible/.test( ua ) && /(mozilla)(?:.*? rv:([\w.]+))?/.exec( ua ) ||
		  	[];

		return { browser: match[1] || "", version: match[2] || "0" };
	};
jQuery.unique = function(results){
/// <summary>
///     Sorts an array of DOM elements, in place, with the duplicates removed. Note that this only works on arrays of DOM elements, not strings or numbers.
///     
/// </summary>
/// <returns type="Array" />
/// <param name="results" type="Array">
///     The Array of DOM elements.
/// </param>

	if ( sortOrder ) {
		hasDuplicate = baseHasDuplicate;
		results.sort(sortOrder);

		if ( hasDuplicate ) {
			for ( var i = 1; i < results.length; i++ ) {
				if ( results[i] === results[i-1] ) {
					results.splice(i--, 1);
				}
			}
		}
	}

	return results;
};
jQuery.prototype._toggle = function( fn ) {

		// Save reference to arguments for access in closure
		var args = arguments, i = 1;

		// link all the functions, so any of them can unbind this click handler
		while ( i < args.length ) {
			jQuery.proxy( fn, args[ i++ ] );
		}

		return this.click( jQuery.proxy( fn, function( event ) {
			// Figure out which function to execute
			var lastToggle = ( jQuery.data( this, "lastToggle" + fn.guid ) || 0 ) % i;
			jQuery.data( this, "lastToggle" + fn.guid, lastToggle + 1 );

			// Make sure that clicks stop
			event.preventDefault();

			// and execute the function
			return args[ lastToggle ].apply( this, arguments ) || false;
		}));
	};
jQuery.prototype.add = function( selector, context ) {
/// <summary>
///     Add elements to the set of matched elements.
///     1 - add(selector) 
///     2 - add(elements) 
///     3 - add(html) 
///     4 - add(selector, context)
/// </summary>
/// <returns type="jQuery" />
/// <param name="selector" type="String">
///     A string containing a selector expression to match additional elements against.
/// </param>
/// <param name="context" domElement="true">
///     Add some elements rooted against the specified context.
/// </param>

		var set = typeof selector === "string" ?
				jQuery( selector, context || this.context ) :
				jQuery.makeArray( selector ),
			all = jQuery.merge( this.get(), set );

		return this.pushStack( isDisconnected( set[0] ) || isDisconnected( all[0] ) ?
			all :
			jQuery.unique( all ) );
	};
jQuery.prototype.addClass = function( value ) {
/// <summary>
///     Adds the specified class(es) to each of the set of matched elements.
///     1 - addClass(className) 
///     2 - addClass(function(index, class))
/// </summary>
/// <returns type="jQuery" />
/// <param name="value" type="String">
///     One or more class names to be added to the class attribute of each matched element.
/// </param>

		if ( jQuery.isFunction(value) ) {
			return this.each(function(i) {
				var self = jQuery(this);
				self.addClass( value.call(this, i, self.attr("class")) );
			});
		}

		if ( value && typeof value === "string" ) {
			var classNames = (value || "").split( rspace );

			for ( var i = 0, l = this.length; i < l; i++ ) {
				var elem = this[i];

				if ( elem.nodeType === 1 ) {
					if ( !elem.className ) {
						elem.className = value;

					} else {
						var className = " " + elem.className + " ", setClass = elem.className;
						for ( var c = 0, cl = classNames.length; c < cl; c++ ) {
							if ( className.indexOf( " " + classNames[c] + " " ) < 0 ) {
								setClass += " " + classNames[c];
							}
						}
						elem.className = jQuery.trim( setClass );
					}
				}
			}
		}

		return this;
	};
jQuery.prototype.after = function() {
/// <summary>
///     Insert content, specified by the parameter, after each element in the set of matched elements.
///     1 - after(content) 
///     2 - after(function(index))
/// </summary>
/// <returns type="jQuery" />
/// <param name="" type="jQuery">
///     An element, HTML string, or jQuery object to insert after each element in the set of matched elements.
/// </param>

		if ( this[0] && this[0].parentNode ) {
			return this.domManip(arguments, false, function( elem ) {
				this.parentNode.insertBefore( elem, this.nextSibling );
			});
		} else if ( arguments.length ) {
			var set = this.pushStack( this, "after", arguments );
			set.push.apply( set, jQuery(arguments[0]).toArray() );
			return set;
		}
	};
jQuery.prototype.ajaxComplete = function( f ) {
/// <summary>
///     
///             Register a handler to be called when Ajax requests complete. This is an Ajax Event.
///           
///     
/// </summary>
/// <returns type="jQuery" />
/// <param name="f" type="Function">
///     The function to be invoked.
/// </param>

		return this.bind(o, f);
	};
jQuery.prototype.ajaxError = function( f ) {
/// <summary>
///     
///             Register a handler to be called when Ajax requests complete with an error. This is an Ajax Event.
///           
///     
/// </summary>
/// <returns type="jQuery" />
/// <param name="f" type="Function">
///     The function to be invoked.
/// </param>

		return this.bind(o, f);
	};
jQuery.prototype.ajaxSend = function( f ) {
/// <summary>
///     
///     
/// </summary>
/// <returns type="jQuery" />
/// <param name="f" type="Function">
///     The function to be invoked.
/// </param>

		return this.bind(o, f);
	};
jQuery.prototype.ajaxStart = function( f ) {
/// <summary>
///     
///             Register a handler to be called when the first Ajax request begins. This is an Ajax Event.
///           
///     
/// </summary>
/// <returns type="jQuery" />
/// <param name="f" type="Function">
///     The function to be invoked.
/// </param>

		return this.bind(o, f);
	};
jQuery.prototype.ajaxStop = function( f ) {
/// <summary>
///     
///     
/// </summary>
/// <returns type="jQuery" />
/// <param name="f" type="Function">
///     The function to be invoked.
/// </param>

		return this.bind(o, f);
	};
jQuery.prototype.ajaxSuccess = function( f ) {
/// <summary>
///     
///     
/// </summary>
/// <returns type="jQuery" />
/// <param name="f" type="Function">
///     The function to be invoked.
/// </param>

		return this.bind(o, f);
	};
jQuery.prototype.andSelf = function() {
/// <summary>
///     Add the previous set of elements on the stack to the current set.
///     
/// </summary>
/// <returns type="jQuery" />

		return this.add( this.prevObject );
	};
jQuery.prototype.animate = function( prop, speed, easing, callback ) {
/// <summary>
///     Perform a custom animation of a set of CSS properties.
///     1 - animate(properties, duration, easing, callback) 
///     2 - animate(properties, options)
/// </summary>
/// <returns type="jQuery" />
/// <param name="prop" type="Object">
///     A map of CSS properties that the animation will move toward.
/// </param>
/// <param name="speed" type="Number">
///     A string or number determining how long the animation will run.
/// </param>
/// <param name="easing" type="String">
///     A string indicating which easing function to use for the transition.
/// </param>
/// <param name="callback" type="Function">
///     A function to call once the animation is complete.
/// </param>

		var optall = jQuery.speed(speed, easing, callback);

		if ( jQuery.isEmptyObject( prop ) ) {
			return this.each( optall.complete );
		}

		return this[ optall.queue === false ? "each" : "queue" ](function() {
			var opt = jQuery.extend({}, optall), p,
				hidden = this.nodeType === 1 && jQuery(this).is(":hidden"),
				self = this;

			for ( p in prop ) {
				var name = p.replace(rdashAlpha, fcamelCase);

				if ( p !== name ) {
					prop[ name ] = prop[ p ];
					delete prop[ p ];
					p = name;
				}

				if ( prop[p] === "hide" && hidden || prop[p] === "show" && !hidden ) {
					return opt.complete.call(this);
				}

				if ( ( p === "height" || p === "width" ) && this.style ) {
					// Store display property
					opt.display = jQuery.css(this, "display");

					// Make sure that nothing sneaks out
					opt.overflow = this.style.overflow;
				}

				if ( jQuery.isArray( prop[p] ) ) {
					// Create (if needed) and add to specialEasing
					(opt.specialEasing = opt.specialEasing || {})[p] = prop[p][1];
					prop[p] = prop[p][0];
				}
			}

			if ( opt.overflow != null ) {
				this.style.overflow = "hidden";
			}

			opt.curAnim = jQuery.extend({}, prop);

			jQuery.each( prop, function( name, val ) {
				var e = new jQuery.fx( self, opt, name );

				if ( rfxtypes.test(val) ) {
					e[ val === "toggle" ? hidden ? "show" : "hide" : val ]( prop );

				} else {
					var parts = rfxnum.exec(val),
						start = e.cur(true) || 0;

					if ( parts ) {
						var end = parseFloat( parts[2] ),
							unit = parts[3] || "px";

						// We need to compute starting value
						if ( unit !== "px" ) {
							self.style[ name ] = (end || 1) + unit;
							start = ((end || 1) / e.cur(true)) * start;
							self.style[ name ] = start + unit;
						}

						// If a +=/-= token was provided, we're doing a relative animation
						if ( parts[1] ) {
							end = ((parts[1] === "-=" ? -1 : 1) * end) + start;
						}

						e.custom( start, end, unit );

					} else {
						e.custom( start, val, "" );
					}
				}
			});

			// For JS strict compliance
			return true;
		});
	};
jQuery.prototype.append = function() {
/// <summary>
///     Insert content, specified by the parameter, to the end of each element in the set of matched elements.
///     1 - append(content) 
///     2 - append(function(index, html))
/// </summary>
/// <returns type="jQuery" />
/// <param name="" type="jQuery">
///     An element, HTML string, or jQuery object to insert at the end of each element in the set of matched elements.
/// </param>

		return this.domManip(arguments, true, function( elem ) {
			if ( this.nodeType === 1 ) {
				this.appendChild( elem );
			}
		});
	};
jQuery.prototype.appendTo = function( selector ) {
/// <summary>
///     Insert every element in the set of matched elements to the end of the target.
///     
/// </summary>
/// <returns type="jQuery" />
/// <param name="selector" type="jQuery">
///     A selector, element, HTML string, or jQuery object; the matched set of elements will be inserted at the end of the element(s) specified by this parameter.
/// </param>

		var ret = [], insert = jQuery( selector ),
			parent = this.length === 1 && this[0].parentNode;
		
		if ( parent && parent.nodeType === 11 && parent.childNodes.length === 1 && insert.length === 1 ) {
			insert[ original ]( this[0] );
			return this;
			
		} else {
			for ( var i = 0, l = insert.length; i < l; i++ ) {
				var elems = (i > 0 ? this.clone(true) : this).get();
				jQuery.fn[ original ].apply( jQuery(insert[i]), elems );
				ret = ret.concat( elems );
			}
		
			return this.pushStack( ret, name, insert.selector );
		}
	};
jQuery.prototype.attr = function( name, value ) {
/// <summary>
///     1: Get the value of an attribute for the first element in the set of matched elements.
///         1.1 - attr(attributeName)
///     2: Set one or more attributes for the set of matched elements.
///         2.1 - attr(attributeName, value) 
///         2.2 - attr(map) 
///         2.3 - attr(attributeName, function(index, attr))
/// </summary>
/// <returns type="jQuery" />
/// <param name="name" type="String">
///     The name of the attribute to set.
/// </param>
/// <param name="value" type="Object">
///     A value to set for the attribute.
/// </param>

		return access( this, name, value, true, jQuery.attr );
	};
jQuery.prototype.before = function() {
/// <summary>
///     Insert content, specified by the parameter, before each element in the set of matched elements.
///     1 - before(content) 
///     2 - before(function)
/// </summary>
/// <returns type="jQuery" />
/// <param name="" type="jQuery">
///     An element, HTML string, or jQuery object to insert before each element in the set of matched elements.
/// </param>

		if ( this[0] && this[0].parentNode ) {
			return this.domManip(arguments, false, function( elem ) {
				this.parentNode.insertBefore( elem, this );
			});
		} else if ( arguments.length ) {
			var set = jQuery(arguments[0]);
			set.push.apply( set, this.toArray() );
			return this.pushStack( set, "before", arguments );
		}
	};
jQuery.prototype.bind = function( type, data, fn ) {
/// <summary>
///     Attach a handler to an event for the elements.
///     1 - bind(eventType, eventData, handler(eventObject)) 
///     2 - bind(events)
/// </summary>
/// <returns type="jQuery" />
/// <param name="type" type="String">
///     A string containing one or more JavaScript event types, such as "click" or "submit," or custom event names.
/// </param>
/// <param name="data" type="Object">
///     A map of data that will be passed to the event handler.
/// </param>
/// <param name="fn" type="Function">
///     A function to execute each time the event is triggered.
/// </param>

		// Handle object literals
		if ( typeof type === "object" ) {
			for ( var key in type ) {
				this[ name ](key, data, type[key], fn);
			}
			return this;
		}
		
		if ( jQuery.isFunction( data ) ) {
			fn = data;
			data = undefined;
		}

		var handler = name === "one" ? jQuery.proxy( fn, function( event ) {
			jQuery( this ).unbind( event, handler );
			return fn.apply( this, arguments );
		}) : fn;

		if ( type === "unload" && name !== "one" ) {
			this.one( type, data, fn );

		} else {
			for ( var i = 0, l = this.length; i < l; i++ ) {
				jQuery.event.add( this[i], type, handler, data );
			}
		}

		return this;
	};
jQuery.prototype.blur = function( fn ) {
/// <summary>
///     Bind an event handler to the "blur" JavaScript event, or trigger that event on an element.
///     1 - blur(handler(eventObject)) 
///     2 - blur()
/// </summary>
/// <returns type="jQuery" />
/// <param name="fn" type="Function">
///     A function to execute each time the event is triggered.
/// </param>

		return fn ? this.bind( name, fn ) : this.trigger( name );
	};
jQuery.prototype.change = function( fn ) {
/// <summary>
///     Bind an event handler to the "change" JavaScript event, or trigger that event on an element.
///     1 - change(handler(eventObject)) 
///     2 - change()
/// </summary>
/// <returns type="jQuery" />
/// <param name="fn" type="Function">
///     A function to execute each time the event is triggered.
/// </param>

		return fn ? this.bind( name, fn ) : this.trigger( name );
	};
jQuery.prototype.children = function( until, selector ) {
/// <summary>
///     Get the children of each element in the set of matched elements, optionally filtered by a selector.
///     
/// </summary>
/// <returns type="jQuery" />
/// <param name="until" type="String">
///     A string containing a selector expression to match elements against.
/// </param>

		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};
jQuery.prototype.clearQueue = function( type ) {
/// <summary>
///     Remove from the queue all items that have not yet been run.
///     
/// </summary>
/// <returns type="jQuery" />
/// <param name="type" type="String">
///     
///                 A string containing the name of the queue. Defaults to fx, the standard effects queue.
///               
/// </param>

		return this.queue( type || "fx", [] );
	};
jQuery.prototype.click = function( fn ) {
/// <summary>
///     Bind an event handler to the "click" JavaScript event, or trigger that event on an element.
///     1 - click(handler(eventObject)) 
///     2 - click()
/// </summary>
/// <returns type="jQuery" />
/// <param name="fn" type="Function">
///     A function to execute each time the event is triggered.
/// </param>

		return fn ? this.bind( name, fn ) : this.trigger( name );
	};
jQuery.prototype.clone = function( events ) {
/// <summary>
///     Create a copy of the set of matched elements.
///     
/// </summary>
/// <returns type="jQuery" />
/// <param name="events" type="Boolean">
///     A Boolean indicating whether event handlers should be copied along with the elements. As of jQuery 1.4 element data will be copied as well.
/// </param>

		// Do the clone
		var ret = this.map(function() {
			if ( !jQuery.support.noCloneEvent && !jQuery.isXMLDoc(this) ) {
				// IE copies events bound via attachEvent when
				// using cloneNode. Calling detachEvent on the
				// clone will also remove the events from the orignal
				// In order to get around this, we use innerHTML.
				// Unfortunately, this means some modifications to
				// attributes in IE that are actually only stored
				// as properties will not be copied (such as the
				// the name attribute on an input).
				var html = this.outerHTML, ownerDocument = this.ownerDocument;
				if ( !html ) {
					var div = ownerDocument.createElement("div");
					div.appendChild( this.cloneNode(true) );
					html = div.innerHTML;
				}

				return jQuery.clean([html.replace(rinlinejQuery, "")
					// Handle the case in IE 8 where action=/test/> self-closes a tag
					.replace(/=([^="'>\s]+\/)>/g, '="$1">')
					.replace(rleadingWhitespace, "")], ownerDocument)[0];
			} else {
				return this.cloneNode(true);
			}
		});

		// Copy the events from the original to the clone
		if ( events === true ) {
			cloneCopyEvent( this, ret );
			cloneCopyEvent( this.find("*"), ret.find("*") );
		}

		// Return the cloned set
		return ret;
	};
jQuery.prototype.closest = function( selectors, context ) {
/// <summary>
///     1: Get the first ancestor element that matches the selector, beginning at the current element and progressing up through the DOM tree.
///         1.1 - closest(selector) 
///         1.2 - closest(selector, context)
///     2: Gets an array of all the elements and selectors matched against the current element up through the DOM tree.
///         2.1 - closest(selectors, context)
/// </summary>
/// <returns type="jQuery" />
/// <param name="selectors" type="String">
///     A string containing a selector expression to match elements against.
/// </param>
/// <param name="context" domElement="true">
///     A DOM element within which a matching element may be found. If no context is passed in then the context of the jQuery set will be used instead.
/// </param>

		if ( jQuery.isArray( selectors ) ) {
			var ret = [], cur = this[0], match, matches = {}, selector;

			if ( cur && selectors.length ) {
				for ( var i = 0, l = selectors.length; i < l; i++ ) {
					selector = selectors[i];

					if ( !matches[selector] ) {
						matches[selector] = jQuery.expr.match.POS.test( selector ) ? 
							jQuery( selector, context || this.context ) :
							selector;
					}
				}

				while ( cur && cur.ownerDocument && cur !== context ) {
					for ( selector in matches ) {
						match = matches[selector];

						if ( match.jquery ? match.index(cur) > -1 : jQuery(cur).is(match) ) {
							ret.push({ selector: selector, elem: cur });
							delete matches[selector];
						}
					}
					cur = cur.parentNode;
				}
			}

			return ret;
		}

		var pos = jQuery.expr.match.POS.test( selectors ) ? 
			jQuery( selectors, context || this.context ) : null;

		return this.map(function( i, cur ) {
			while ( cur && cur.ownerDocument && cur !== context ) {
				if ( pos ? pos.index(cur) > -1 : jQuery(cur).is(selectors) ) {
					return cur;
				}
				cur = cur.parentNode;
			}
			return null;
		});
	};
jQuery.prototype.contents = function( until, selector ) {
/// <summary>
///     Get the children of each element in the set of matched elements, including text nodes.
///     
/// </summary>
/// <returns type="jQuery" />

		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};
jQuery.prototype.css = function( name, value ) {
/// <summary>
///     1: Get the value of a style property for the first element in the set of matched elements.
///         1.1 - css(propertyName)
///     2: Set one or more CSS properties for the  set of matched elements.
///         2.1 - css(propertyName, value) 
///         2.2 - css(propertyName, function(index, value)) 
///         2.3 - css(map)
/// </summary>
/// <returns type="jQuery" />
/// <param name="name" type="String">
///     A CSS property name.
/// </param>
/// <param name="value" type="Number">
///     A value to set for the property.
/// </param>

	return access( this, name, value, true, function( elem, name, value ) {
		if ( value === undefined ) {
			return jQuery.curCSS( elem, name );
		}
		
		if ( typeof value === "number" && !rexclude.test(name) ) {
			value += "px";
		}

		jQuery.style( elem, name, value );
	});
};
jQuery.prototype.data = function( key, value ) {
/// <summary>
///     1: Store arbitrary data associated with the matched elements.
///         1.1 - data(key, value) 
///         1.2 - data(obj)
///     2: Returns value at named data store for the element, as set by data(name, value).
///         2.1 - data(key) 
///         2.2 - data()
/// </summary>
/// <returns type="jQuery" />
/// <param name="key" type="String">
///     A string naming the piece of data to set.
/// </param>
/// <param name="value" type="Object">
///     The new data value; it can be any Javascript type including Array or Object.
/// </param>

		if ( typeof key === "undefined" && this.length ) {
			return jQuery.data( this[0] );

		} else if ( typeof key === "object" ) {
			return this.each(function() {
				jQuery.data( this, key );
			});
		}

		var parts = key.split(".");
		parts[1] = parts[1] ? "." + parts[1] : "";

		if ( value === undefined ) {
			var data = this.triggerHandler("getData" + parts[1] + "!", [parts[0]]);

			if ( data === undefined && this.length ) {
				data = jQuery.data( this[0], key );
			}
			return data === undefined && parts[1] ?
				this.data( parts[0] ) :
				data;
		} else {
			return this.trigger("setData" + parts[1] + "!", [parts[0], value]).each(function() {
				jQuery.data( this, key, value );
			});
		}
	};
jQuery.prototype.dblclick = function( fn ) {
/// <summary>
///     Bind an event handler to the "dblclick" JavaScript event, or trigger that event on an element.
///     1 - dblclick(handler(eventObject)) 
///     2 - dblclick()
/// </summary>
/// <returns type="jQuery" />
/// <param name="fn" type="Function">
///     A function to execute each time the event is triggered.
/// </param>

		return fn ? this.bind( name, fn ) : this.trigger( name );
	};
jQuery.prototype.delay = function( time, type ) {
/// <summary>
///     Set a timer to delay execution of subsequent items in the queue.
///     
/// </summary>
/// <returns type="jQuery" />
/// <param name="time" type="Number">
///     An integer indicating the number of milliseconds to delay execution of the next item in the queue.
/// </param>
/// <param name="type" type="String">
///     
///                 A string containing the name of the queue. Defaults to fx, the standard effects queue.
///               
/// </param>

		time = jQuery.fx ? jQuery.fx.speeds[time] || time : time;
		type = type || "fx";

		return this.queue( type, function() {
			var elem = this;
			setTimeout(function() {
				jQuery.dequeue( elem, type );
			}, time );
		});
	};
jQuery.prototype.delegate = function( selector, types, data, fn ) {
/// <summary>
///     Attach a handler to one or more events for all elements that match the selector, now or in the future, based on a specific set of root elements.
///     1 - delegate(selector, eventType, handler) 
///     2 - delegate(selector, eventType, eventData, handler)
/// </summary>
/// <returns type="jQuery" />
/// <param name="selector" type="String">
///     A selector to filter the elements that trigger the event.
/// </param>
/// <param name="types" type="String">
///     A string containing one or more space-separated JavaScript event types, such as "click" or "keydown," or custom event names.
/// </param>
/// <param name="data" type="Object">
///     A map of data that will be passed to the event handler.
/// </param>
/// <param name="fn" type="Function">
///     A function to execute at the time the event is triggered.
/// </param>

		return this.live( types, data, fn, selector );
	};
jQuery.prototype.dequeue = function( type ) {
/// <summary>
///     Execute the next function on the queue for the matched elements.
///     
/// </summary>
/// <returns type="jQuery" />
/// <param name="type" type="String">
///     
///                 A string containing the name of the queue. Defaults to fx, the standard effects queue.
///               
/// </param>

		return this.each(function() {
			jQuery.dequeue( this, type );
		});
	};
jQuery.prototype.detach = function( selector ) {
/// <summary>
///     Remove the set of matched elements from the DOM.
///     
/// </summary>
/// <returns type="jQuery" />
/// <param name="selector" type="String">
///     A selector expression that filters the set of matched elements to be removed.
/// </param>

		return this.remove( selector, true );
	};
jQuery.prototype.die = function( types, data, fn, origSelector /* Internal Use Only */ ) {
/// <summary>
///     1: 
///             Remove all event handlers previously attached using .live() from the elements.
///           
///         1.1 - die()
///     2: 
///             Remove an event handler previously attached using .live() from the elements.
///           
///         2.1 - die(eventType, handler)
/// </summary>
/// <returns type="jQuery" />
/// <param name="types" type="String">
///     
///                 A string containing a JavaScript event type, such as click or keydown.
///               
/// </param>
/// <param name="data" type="String">
///     The function that is to be no longer executed.
/// </param>

		var type, i = 0, match, namespaces, preType,
			selector = origSelector || this.selector,
			context = origSelector ? this : jQuery( this.context );

		if ( jQuery.isFunction( data ) ) {
			fn = data;
			data = undefined;
		}

		types = (types || "").split(" ");

		while ( (type = types[ i++ ]) != null ) {
			match = rnamespaces.exec( type );
			namespaces = "";

			if ( match )  {
				namespaces = match[0];
				type = type.replace( rnamespaces, "" );
			}

			if ( type === "hover" ) {
				types.push( "mouseenter" + namespaces, "mouseleave" + namespaces );
				continue;
			}

			preType = type;

			if ( type === "focus" || type === "blur" ) {
				types.push( liveMap[ type ] + namespaces );
				type = type + namespaces;

			} else {
				type = (liveMap[ type ] || type) + namespaces;
			}

			if ( name === "live" ) {
				// bind live handler
				context.each(function(){
					jQuery.event.add( this, liveConvert( type, selector ),
						{ data: data, selector: selector, handler: fn, origType: type, origHandler: fn, preType: preType } );
				});

			} else {
				// unbind live handler
				context.unbind( liveConvert( type, selector ), fn );
			}
		}
		
		return this;
	};
jQuery.prototype.domManip = function( args, table, callback ) {

		var results, first, value = args[0], scripts = [], fragment, parent;

		// We can't cloneNode fragments that contain checked, in WebKit
		if ( !jQuery.support.checkClone && arguments.length === 3 && typeof value === "string" && rchecked.test( value ) ) {
			return this.each(function() {
				jQuery(this).domManip( args, table, callback, true );
			});
		}

		if ( jQuery.isFunction(value) ) {
			return this.each(function(i) {
				var self = jQuery(this);
				args[0] = value.call(this, i, table ? self.html() : undefined);
				self.domManip( args, table, callback );
			});
		}

		if ( this[0] ) {
			parent = value && value.parentNode;

			// If we're in a fragment, just use that instead of building a new one
			if ( jQuery.support.parentNode && parent && parent.nodeType === 11 && parent.childNodes.length === this.length ) {
				results = { fragment: parent };

			} else {
				results = buildFragment( args, this, scripts );
			}
			
			fragment = results.fragment;
			
			if ( fragment.childNodes.length === 1 ) {
				first = fragment = fragment.firstChild;
			} else {
				first = fragment.firstChild;
			}

			if ( first ) {
				table = table && jQuery.nodeName( first, "tr" );

				for ( var i = 0, l = this.length; i < l; i++ ) {
					callback.call(
						table ?
							root(this[i], first) :
							this[i],
						i > 0 || results.cacheable || this.length > 1  ?
							fragment.cloneNode(true) :
							fragment
					);
				}
			}

			if ( scripts.length ) {
				jQuery.each( scripts, evalScript );
			}
		}

		return this;

		function root( elem, cur ) {
			return jQuery.nodeName(elem, "table") ?
				(elem.getElementsByTagName("tbody")[0] ||
				elem.appendChild(elem.ownerDocument.createElement("tbody"))) :
				elem;
		}
	};
jQuery.prototype.each = function( callback, args ) {
/// <summary>
///     Iterate over a jQuery object, executing a function for each matched element. 
///     
/// </summary>
/// <returns type="jQuery" />
/// <param name="callback" type="Function">
///     A function to execute for each matched element.
/// </param>

		return jQuery.each( this, callback, args );
	};
jQuery.prototype.empty = function() {
/// <summary>
///     Remove all child nodes of the set of matched elements from the DOM.
///     
/// </summary>
/// <returns type="jQuery" />

		for ( var i = 0, elem; (elem = this[i]) != null; i++ ) {
			// Remove element nodes and prevent memory leaks
			if ( elem.nodeType === 1 ) {
				jQuery.cleanData( elem.getElementsByTagName("*") );
			}

			// Remove any remaining nodes
			while ( elem.firstChild ) {
				elem.removeChild( elem.firstChild );
			}
		}
		
		return this;
	};
jQuery.prototype.end = function() {
/// <summary>
///     End the most recent filtering operation in the current chain and return the set of matched elements to its previous state.
///     
/// </summary>
/// <returns type="jQuery" />

		return this.prevObject || jQuery(null);
	};
jQuery.prototype.eq = function( i ) {
/// <summary>
///     Reduce the set of matched elements to the one at the specified index.
///     1 - eq(index) 
///     2 - eq(-index)
/// </summary>
/// <returns type="jQuery" />
/// <param name="i" type="Number">
///     An integer indicating the 0-based position of the element. 
/// </param>

		return i === -1 ?
			this.slice( i ) :
			this.slice( i, +i + 1 );
	};
jQuery.prototype.error = function( fn ) {
/// <summary>
///     Bind an event handler to the "error" JavaScript event.
///     
/// </summary>
/// <returns type="jQuery" />
/// <param name="fn" type="Function">
///     A function to execute when the event is triggered.
/// </param>

		return fn ? this.bind( name, fn ) : this.trigger( name );
	};
jQuery.prototype.extend = function() {

	// copy reference to target object
	var target = arguments[0] || {}, i = 1, length = arguments.length, deep = false, options, name, src, copy;

	// Handle a deep copy situation
	if ( typeof target === "boolean" ) {
		deep = target;
		target = arguments[1] || {};
		// skip the boolean and the target
		i = 2;
	}

	// Handle case when target is a string or something (possible in deep copy)
	if ( typeof target !== "object" && !jQuery.isFunction(target) ) {
		target = {};
	}

	// extend jQuery itself if only one argument is passed
	if ( length === i ) {
		target = this;
		--i;
	}

	for ( ; i < length; i++ ) {
		// Only deal with non-null/undefined values
		if ( (options = arguments[ i ]) != null ) {
			// Extend the base object
			for ( name in options ) {
				src = target[ name ];
				copy = options[ name ];

				// Prevent never-ending loop
				if ( target === copy ) {
					continue;
				}

				// Recurse if we're merging object literal values or arrays
				if ( deep && copy && ( jQuery.isPlainObject(copy) || jQuery.isArray(copy) ) ) {
					var clone = src && ( jQuery.isPlainObject(src) || jQuery.isArray(src) ) ? src
						: jQuery.isArray(copy) ? [] : {};

					// Never move original objects, clone them
					target[ name ] = jQuery.extend( deep, clone, copy );

				// Don't bring in undefined values
				} else if ( copy !== undefined ) {
					target[ name ] = copy;
				}
			}
		}
	}

	// Return the modified object
	return target;
};
jQuery.prototype.fadeIn = function( speed, callback ) {
/// <summary>
///     Display the matched elements by fading them to opaque.
///     
/// </summary>
/// <returns type="jQuery" />
/// <param name="speed" type="Number">
///     A string or number determining how long the animation will run.
/// </param>
/// <param name="callback" type="Function">
///     A function to call once the animation is complete.
/// </param>

		return this.animate( props, speed, callback );
	};
jQuery.prototype.fadeOut = function( speed, callback ) {
/// <summary>
///     Hide the matched elements by fading them to transparent.
///     
/// </summary>
/// <returns type="jQuery" />
/// <param name="speed" type="Number">
///     A string or number determining how long the animation will run.
/// </param>
/// <param name="callback" type="Function">
///     A function to call once the animation is complete.
/// </param>

		return this.animate( props, speed, callback );
	};
jQuery.prototype.fadeTo = function( speed, to, callback ) {
/// <summary>
///     Adjust the opacity of the matched elements.
///     
/// </summary>
/// <returns type="jQuery" />
/// <param name="speed" type="Number">
///     A string or number determining how long the animation will run.
/// </param>
/// <param name="to" type="Number">
///     A number between 0 and 1 denoting the target opacity.
/// </param>
/// <param name="callback" type="Function">
///     A function to call once the animation is complete.
/// </param>

		return this.filter(":hidden").css("opacity", 0).show().end()
					.animate({opacity: to}, speed, callback);
	};
jQuery.prototype.filter = function( selector ) {
/// <summary>
///     Reduce the set of matched elements to those that match the selector or pass the function's test. 
///     1 - filter(selector) 
///     2 - filter(function(index))
/// </summary>
/// <returns type="jQuery" />
/// <param name="selector" type="String">
///     A string containing a selector expression to match elements against.
/// </param>

		return this.pushStack( winnow(this, selector, true), "filter", selector );
	};
jQuery.prototype.find = function( selector ) {
/// <summary>
///     Get the descendants of each element in the current set of matched elements, filtered by a selector.
///     
/// </summary>
/// <returns type="jQuery" />
/// <param name="selector" type="String">
///     A string containing a selector expression to match elements against.
/// </param>

		var ret = this.pushStack( "", "find", selector ), length = 0;

		for ( var i = 0, l = this.length; i < l; i++ ) {
			length = ret.length;
			jQuery.find( selector, this[i], ret );

			if ( i > 0 ) {
				// Make sure that the results are unique
				for ( var n = length; n < ret.length; n++ ) {
					for ( var r = 0; r < length; r++ ) {
						if ( ret[r] === ret[n] ) {
							ret.splice(n--, 1);
							break;
						}
					}
				}
			}
		}

		return ret;
	};
jQuery.prototype.first = function() {
/// <summary>
///     Reduce the set of matched elements to the first in the set.
///     
/// </summary>
/// <returns type="jQuery" />

		return this.eq( 0 );
	};
jQuery.prototype.focus = function( fn ) {
/// <summary>
///     Bind an event handler to the "focus" JavaScript event, or trigger that event on an element.
///     1 - focus(handler(eventObject)) 
///     2 - focus()
/// </summary>
/// <returns type="jQuery" />
/// <param name="fn" type="Function">
///     A function to execute each time the event is triggered.
/// </param>

		return fn ? this.bind( name, fn ) : this.trigger( name );
	};
jQuery.prototype.focusin = function( fn ) {
/// <summary>
///     Bind an event handler to the "focusin" JavaScript event.
///     
/// </summary>
/// <returns type="jQuery" />
/// <param name="fn" type="Function">
///     A function to execute each time the event is triggered.
/// </param>

		return fn ? this.bind( name, fn ) : this.trigger( name );
	};
jQuery.prototype.focusout = function( fn ) {
/// <summary>
///     Bind an event handler to the "focusout" JavaScript event.
///     
/// </summary>
/// <returns type="jQuery" />
/// <param name="fn" type="Function">
///     A function to execute each time the event is triggered.
/// </param>

		return fn ? this.bind( name, fn ) : this.trigger( name );
	};
jQuery.prototype.get = function( num ) {
/// <summary>
///     Retrieve the DOM elements matched by the jQuery object.
///     
/// </summary>
/// <returns type="Array" />
/// <param name="num" type="Number">
///     A zero-based integer indicating which element to retrieve.
/// </param>

		return num == null ?

			// Return a 'clean' array
			this.toArray() :

			// Return just the object
			( num < 0 ? this.slice(num)[ 0 ] : this[ num ] );
	};
jQuery.prototype.has = function( target ) {
/// <summary>
///     Reduce the set of matched elements to those that have a descendant that matches the selector or DOM element.
///     1 - has(selector) 
///     2 - has(contained)
/// </summary>
/// <returns type="jQuery" />
/// <param name="target" type="String">
///     A string containing a selector expression to match elements against.
/// </param>

		var targets = jQuery( target );
		return this.filter(function() {
			for ( var i = 0, l = targets.length; i < l; i++ ) {
				if ( jQuery.contains( this, targets[i] ) ) {
					return true;
				}
			}
		});
	};
jQuery.prototype.hasClass = function( selector ) {
/// <summary>
///     Determine whether any of the matched elements are assigned the given class.
///     
/// </summary>
/// <returns type="Boolean" />
/// <param name="selector" type="String">
///     The class name to search for.
/// </param>

		var className = " " + selector + " ";
		for ( var i = 0, l = this.length; i < l; i++ ) {
			if ( (" " + this[i].className + " ").replace(rclass, " ").indexOf( className ) > -1 ) {
				return true;
			}
		}

		return false;
	};
jQuery.prototype.height = function( size ) {
/// <summary>
///     1: Get the current computed height for the first element in the set of matched elements.
///         1.1 - height()
///     2: Set the CSS height of every matched element.
///         2.1 - height(value) 
///         2.2 - height(function(index, height))
/// </summary>
/// <returns type="jQuery" />
/// <param name="size" type="Number">
///     An integer representing the number of pixels, or an integer with an optional unit of measure appended (as a string).
/// </param>

		// Get window width or height
		var elem = this[0];
		if ( !elem ) {
			return size == null ? null : this;
		}
		
		if ( jQuery.isFunction( size ) ) {
			return this.each(function( i ) {
				var self = jQuery( this );
				self[ type ]( size.call( this, i, self[ type ]() ) );
			});
		}

		return ("scrollTo" in elem && elem.document) ? // does it walk and quack like a window?
			// Everyone else use document.documentElement or document.body depending on Quirks vs Standards mode
			elem.document.compatMode === "CSS1Compat" && elem.document.documentElement[ "client" + name ] ||
			elem.document.body[ "client" + name ] :

			// Get document width or height
			(elem.nodeType === 9) ? // is it a document
				// Either scroll[Width/Height] or offset[Width/Height], whichever is greater
				Math.max(
					elem.documentElement["client" + name],
					elem.body["scroll" + name], elem.documentElement["scroll" + name],
					elem.body["offset" + name], elem.documentElement["offset" + name]
				) :

				// Get or set width or height on the element
				size === undefined ?
					// Get width or height on the element
					jQuery.css( elem, type ) :

					// Set the width or height on the element (default to pixels if value is unitless)
					this.css( type, typeof size === "string" ? size : size + "px" );
	};
jQuery.prototype.hide = function( speed, callback ) {
/// <summary>
///     Hide the matched elements.
///     1 - hide() 
///     2 - hide(duration, callback)
/// </summary>
/// <returns type="jQuery" />
/// <param name="speed" type="Number">
///     A string or number determining how long the animation will run.
/// </param>
/// <param name="callback" type="Function">
///     A function to call once the animation is complete.
/// </param>

		if ( speed || speed === 0 ) {
			return this.animate( genFx("hide", 3), speed, callback);

		} else {
			for ( var i = 0, l = this.length; i < l; i++ ) {
				var old = jQuery.data(this[i], "olddisplay");
				if ( !old && old !== "none" ) {
					jQuery.data(this[i], "olddisplay", jQuery.css(this[i], "display"));
				}
			}

			// Set the display of the elements in a second loop
			// to avoid the constant reflow
			for ( var j = 0, k = this.length; j < k; j++ ) {
				this[j].style.display = "none";
			}

			return this;
		}
	};
jQuery.prototype.hover = function( fnOver, fnOut ) {
/// <summary>
///     1: Bind two handlers to the matched elements, to be executed when the mouse pointer enters and leaves the elements.
///         1.1 - hover(handlerIn(eventObject), handlerOut(eventObject))
///     2: Bind a single handler to the matched elements, to be executed when the mouse pointer enters or leaves the elements.
///         2.1 - hover(handlerInOut(eventObject))
/// </summary>
/// <returns type="jQuery" />
/// <param name="fnOver" type="Function">
///     A function to execute when the mouse pointer enters the element.
/// </param>
/// <param name="fnOut" type="Function">
///     A function to execute when the mouse pointer leaves the element.
/// </param>

		return this.mouseenter( fnOver ).mouseleave( fnOut || fnOver );
	};
jQuery.prototype.html = function( value ) {
/// <summary>
///     1: Get the HTML contents of the first element in the set of matched elements.
///         1.1 - html()
///     2: Set the HTML contents of each element in the set of matched elements.
///         2.1 - html(htmlString) 
///         2.2 - html(function(index, html))
/// </summary>
/// <returns type="jQuery" />
/// <param name="value" type="String">
///     A string of HTML to set as the content of each matched element.
/// </param>

		if ( value === undefined ) {
			return this[0] && this[0].nodeType === 1 ?
				this[0].innerHTML.replace(rinlinejQuery, "") :
				null;

		// See if we can take a shortcut and just use innerHTML
		} else if ( typeof value === "string" && !rnocache.test( value ) &&
			(jQuery.support.leadingWhitespace || !rleadingWhitespace.test( value )) &&
			!wrapMap[ (rtagName.exec( value ) || ["", ""])[1].toLowerCase() ] ) {

			value = value.replace(rxhtmlTag, fcloseTag);

			try {
				for ( var i = 0, l = this.length; i < l; i++ ) {
					// Remove element nodes and prevent memory leaks
					if ( this[i].nodeType === 1 ) {
						jQuery.cleanData( this[i].getElementsByTagName("*") );
						this[i].innerHTML = value;
					}
				}

			// If using innerHTML throws an exception, use the fallback method
			} catch(e) {
				this.empty().append( value );
			}

		} else if ( jQuery.isFunction( value ) ) {
			this.each(function(i){
				var self = jQuery(this), old = self.html();
				self.empty().append(function(){
					return value.call( this, i, old );
				});
			});

		} else {
			this.empty().append( value );
		}

		return this;
	};
jQuery.prototype.index = function( elem ) {
/// <summary>
///     Search for a given element from among the matched elements.
///     1 - index() 
///     2 - index(selector) 
///     3 - index(element)
/// </summary>
/// <returns type="Number" />
/// <param name="elem" type="String">
///     A selector representing a jQuery collection in which to look for an element.
/// </param>

		if ( !elem || typeof elem === "string" ) {
			return jQuery.inArray( this[0],
				// If it receives a string, the selector is used
				// If it receives nothing, the siblings are used
				elem ? jQuery( elem ) : this.parent().children() );
		}
		// Locate the position of the desired element
		return jQuery.inArray(
			// If it receives a jQuery object, the first element is used
			elem.jquery ? elem[0] : elem, this );
	};
jQuery.prototype.init = function( selector, context ) {

		var match, elem, ret, doc;

		// Handle $(""), $(null), or $(undefined)
		if ( !selector ) {
			return this;
		}

		// Handle $(DOMElement)
		if ( selector.nodeType ) {
			this.context = this[0] = selector;
			this.length = 1;
			return this;
		}
		
		// The body element only exists once, optimize finding it
		if ( selector === "body" && !context ) {
			this.context = document;
			this[0] = document.body;
			this.selector = "body";
			this.length = 1;
			return this;
		}

		// Handle HTML strings
		if ( typeof selector === "string" ) {
			// Are we dealing with HTML string or an ID?
			match = quickExpr.exec( selector );

			// Verify a match, and that no context was specified for #id
			if ( match && (match[1] || !context) ) {

				// HANDLE: $(html) -> $(array)
				if ( match[1] ) {
					doc = (context ? context.ownerDocument || context : document);

					// If a single string is passed in and it's a single tag
					// just do a createElement and skip the rest
					ret = rsingleTag.exec( selector );

					if ( ret ) {
						if ( jQuery.isPlainObject( context ) ) {
							selector = [ document.createElement( ret[1] ) ];
							jQuery.fn.attr.call( selector, context, true );

						} else {
							selector = [ doc.createElement( ret[1] ) ];
						}

					} else {
						ret = buildFragment( [ match[1] ], [ doc ] );
						selector = (ret.cacheable ? ret.fragment.cloneNode(true) : ret.fragment).childNodes;
					}
					
					return jQuery.merge( this, selector );
					
				// HANDLE: $("#id")
				} else {
					elem = document.getElementById( match[2] );

					if ( elem ) {
						// Handle the case where IE and Opera return items
						// by name instead of ID
						if ( elem.id !== match[2] ) {
							return rootjQuery.find( selector );
						}

						// Otherwise, we inject the element directly into the jQuery object
						this.length = 1;
						this[0] = elem;
					}

					this.context = document;
					this.selector = selector;
					return this;
				}

			// HANDLE: $("TAG")
			} else if ( !context && /^\w+$/.test( selector ) ) {
				this.selector = selector;
				this.context = document;
				selector = document.getElementsByTagName( selector );
				return jQuery.merge( this, selector );

			// HANDLE: $(expr, $(...))
			} else if ( !context || context.jquery ) {
				return (context || rootjQuery).find( selector );

			// HANDLE: $(expr, context)
			// (which is just equivalent to: $(context).find(expr)
			} else {
				return jQuery( context ).find( selector );
			}

		// HANDLE: $(function)
		// Shortcut for document ready
		} else if ( jQuery.isFunction( selector ) ) {
			return rootjQuery.ready( selector );
		}

		if (selector.selector !== undefined) {
			this.selector = selector.selector;
			this.context = selector.context;
		}

		return jQuery.makeArray( selector, this );
	};
jQuery.prototype.innerHeight = function() {
/// <summary>
///     Get the current computed height for the first element in the set of matched elements, including padding but not border.
///     
/// </summary>
/// <returns type="Number" />

		return this[0] ?
			jQuery.css( this[0], type, false, "padding" ) :
			null;
	};
jQuery.prototype.innerWidth = function() {
/// <summary>
///     Get the current computed width for the first element in the set of matched elements, including padding but not border.
///     
/// </summary>
/// <returns type="Number" />

		return this[0] ?
			jQuery.css( this[0], type, false, "padding" ) :
			null;
	};
jQuery.prototype.insertAfter = function( selector ) {
/// <summary>
///     Insert every element in the set of matched elements after the target.
///     
/// </summary>
/// <returns type="jQuery" />
/// <param name="selector" type="jQuery">
///     A selector, element, HTML string, or jQuery object; the matched set of elements will be inserted after the element(s) specified by this parameter.
/// </param>

		var ret = [], insert = jQuery( selector ),
			parent = this.length === 1 && this[0].parentNode;
		
		if ( parent && parent.nodeType === 11 && parent.childNodes.length === 1 && insert.length === 1 ) {
			insert[ original ]( this[0] );
			return this;
			
		} else {
			for ( var i = 0, l = insert.length; i < l; i++ ) {
				var elems = (i > 0 ? this.clone(true) : this).get();
				jQuery.fn[ original ].apply( jQuery(insert[i]), elems );
				ret = ret.concat( elems );
			}
		
			return this.pushStack( ret, name, insert.selector );
		}
	};
jQuery.prototype.insertBefore = function( selector ) {
/// <summary>
///     Insert every element in the set of matched elements before the target.
///     
/// </summary>
/// <returns type="jQuery" />
/// <param name="selector" type="jQuery">
///     A selector, element, HTML string, or jQuery object; the matched set of elements will be inserted before the element(s) specified by this parameter.
/// </param>

		var ret = [], insert = jQuery( selector ),
			parent = this.length === 1 && this[0].parentNode;
		
		if ( parent && parent.nodeType === 11 && parent.childNodes.length === 1 && insert.length === 1 ) {
			insert[ original ]( this[0] );
			return this;
			
		} else {
			for ( var i = 0, l = insert.length; i < l; i++ ) {
				var elems = (i > 0 ? this.clone(true) : this).get();
				jQuery.fn[ original ].apply( jQuery(insert[i]), elems );
				ret = ret.concat( elems );
			}
		
			return this.pushStack( ret, name, insert.selector );
		}
	};
jQuery.prototype.is = function( selector ) {
/// <summary>
///     Check the current matched set of elements against a selector and return true if at least one of these elements matches the selector.
///     
/// </summary>
/// <returns type="Boolean" />
/// <param name="selector" type="String">
///     A string containing a selector expression to match elements against.
/// </param>

		return !!selector && jQuery.filter( selector, this ).length > 0;
	};
jQuery.prototype.keydown = function( fn ) {
/// <summary>
///     Bind an event handler to the "keydown" JavaScript event, or trigger that event on an element.
///     1 - keydown(handler(eventObject)) 
///     2 - keydown()
/// </summary>
/// <returns type="jQuery" />
/// <param name="fn" type="Function">
///     A function to execute each time the event is triggered.
/// </param>

		return fn ? this.bind( name, fn ) : this.trigger( name );
	};
jQuery.prototype.keypress = function( fn ) {
/// <summary>
///     Bind an event handler to the "keypress" JavaScript event, or trigger that event on an element.
///     1 - keypress(handler(eventObject)) 
///     2 - keypress()
/// </summary>
/// <returns type="jQuery" />
/// <param name="fn" type="Function">
///     A function to execute each time the event is triggered.
/// </param>

		return fn ? this.bind( name, fn ) : this.trigger( name );
	};
jQuery.prototype.keyup = function( fn ) {
/// <summary>
///     Bind an event handler to the "keyup" JavaScript event, or trigger that event on an element.
///     1 - keyup(handler(eventObject)) 
///     2 - keyup()
/// </summary>
/// <returns type="jQuery" />
/// <param name="fn" type="Function">
///     A function to execute each time the event is triggered.
/// </param>

		return fn ? this.bind( name, fn ) : this.trigger( name );
	};
jQuery.prototype.last = function() {
/// <summary>
///     Reduce the set of matched elements to the final one in the set.
///     
/// </summary>
/// <returns type="jQuery" />

		return this.eq( -1 );
	};
jQuery.prototype.length = SrliZe = 0;;
jQuery.prototype.live = function( types, data, fn, origSelector /* Internal Use Only */ ) {
/// <summary>
///     Attach a handler to the event for all elements which match the current selector, now or in the future.
///     1 - live(eventType, handler) 
///     2 - live(eventType, eventData, handler)
/// </summary>
/// <returns type="jQuery" />
/// <param name="types" type="String">
///     A string containing a JavaScript event type, such as "click" or "keydown." As of jQuery 1.4 the string can contain multiple, space-separated event types or custom event names, as well.
/// </param>
/// <param name="data" type="Object">
///     A map of data that will be passed to the event handler.
/// </param>
/// <param name="fn" type="Function">
///     A function to execute at the time the event is triggered.
/// </param>

		var type, i = 0, match, namespaces, preType,
			selector = origSelector || this.selector,
			context = origSelector ? this : jQuery( this.context );

		if ( jQuery.isFunction( data ) ) {
			fn = data;
			data = undefined;
		}

		types = (types || "").split(" ");

		while ( (type = types[ i++ ]) != null ) {
			match = rnamespaces.exec( type );
			namespaces = "";

			if ( match )  {
				namespaces = match[0];
				type = type.replace( rnamespaces, "" );
			}

			if ( type === "hover" ) {
				types.push( "mouseenter" + namespaces, "mouseleave" + namespaces );
				continue;
			}

			preType = type;

			if ( type === "focus" || type === "blur" ) {
				types.push( liveMap[ type ] + namespaces );
				type = type + namespaces;

			} else {
				type = (liveMap[ type ] || type) + namespaces;
			}

			if ( name === "live" ) {
				// bind live handler
				context.each(function(){
					jQuery.event.add( this, liveConvert( type, selector ),
						{ data: data, selector: selector, handler: fn, origType: type, origHandler: fn, preType: preType } );
				});

			} else {
				// unbind live handler
				context.unbind( liveConvert( type, selector ), fn );
			}
		}
		
		return this;
	};
jQuery.prototype.load = function( url, params, callback ) {
/// <summary>
///     1: Bind an event handler to the "load" JavaScript event.
///         1.1 - load(handler(eventObject))
///     2: Load data from the server and place the returned HTML into the matched element.
///         2.1 - load(url, data, complete(responseText, textStatus, XMLHttpRequest))
/// </summary>
/// <returns type="jQuery" />
/// <param name="url" type="String">
///     A string containing the URL to which the request is sent.
/// </param>
/// <param name="params" type="String">
///     A map or string that is sent to the server with the request.
/// </param>
/// <param name="callback" type="Function">
///     A callback function that is executed when the request completes.
/// </param>

		if ( typeof url !== "string" ) {
			return _load.call( this, url );

		// Don't do a request if no elements are being requested
		} else if ( !this.length ) {
			return this;
		}

		var off = url.indexOf(" ");
		if ( off >= 0 ) {
			var selector = url.slice(off, url.length);
			url = url.slice(0, off);
		}

		// Default to a GET request
		var type = "GET";

		// If the second parameter was provided
		if ( params ) {
			// If it's a function
			if ( jQuery.isFunction( params ) ) {
				// We assume that it's the callback
				callback = params;
				params = null;

			// Otherwise, build a param string
			} else if ( typeof params === "object" ) {
				params = jQuery.param( params, jQuery.ajaxSettings.traditional );
				type = "POST";
			}
		}

		var self = this;

		// Request the remote document
		jQuery.ajax({
			url: url,
			type: type,
			dataType: "html",
			data: params,
			complete: function( res, status ) {
				// If successful, inject the HTML into all the matched elements
				if ( status === "success" || status === "notmodified" ) {
					// See if a selector was specified
					self.html( selector ?
						// Create a dummy div to hold the results
						jQuery("<div />")
							// inject the contents of the document in, removing the scripts
							// to avoid any 'Permission Denied' errors in IE
							.append(res.responseText.replace(rscript, ""))

							// Locate the specified elements
							.find(selector) :

						// If not, just inject the full result
						res.responseText );
				}

				if ( callback ) {
					self.each( callback, [res.responseText, status, res] );
				}
			}
		});

		return this;
	};
jQuery.prototype.map = function( callback ) {
/// <summary>
///     Pass each element in the current matched set through a function, producing a new jQuery object containing the return values.
///     
/// </summary>
/// <returns type="jQuery" />
/// <param name="callback" type="Function">
///     A function object that will be invoked for each element in the current set.
/// </param>

		return this.pushStack( jQuery.map(this, function( elem, i ) {
			return callback.call( elem, i, elem );
		}));
	};
jQuery.prototype.mousedown = function( fn ) {
/// <summary>
///     Bind an event handler to the "mousedown" JavaScript event, or trigger that event on an element.
///     1 - mousedown(handler(eventObject)) 
///     2 - mousedown()
/// </summary>
/// <returns type="jQuery" />
/// <param name="fn" type="Function">
///     A function to execute each time the event is triggered.
/// </param>

		return fn ? this.bind( name, fn ) : this.trigger( name );
	};
jQuery.prototype.mouseenter = function( fn ) {
/// <summary>
///     Bind an event handler to be fired when the mouse enters an element, or trigger that handler on an element.
///     1 - mouseenter(handler(eventObject)) 
///     2 - mouseenter()
/// </summary>
/// <returns type="jQuery" />
/// <param name="fn" type="Function">
///     A function to execute each time the event is triggered.
/// </param>

		return fn ? this.bind( name, fn ) : this.trigger( name );
	};
jQuery.prototype.mouseleave = function( fn ) {
/// <summary>
///     Bind an event handler to be fired when the mouse leaves an element, or trigger that handler on an element.
///     1 - mouseleave(handler(eventObject)) 
///     2 - mouseleave()
/// </summary>
/// <returns type="jQuery" />
/// <param name="fn" type="Function">
///     A function to execute each time the event is triggered.
/// </param>

		return fn ? this.bind( name, fn ) : this.trigger( name );
	};
jQuery.prototype.mousemove = function( fn ) {
/// <summary>
///     Bind an event handler to the "mousemove" JavaScript event, or trigger that event on an element.
///     1 - mousemove(handler(eventObject)) 
///     2 - mousemove()
/// </summary>
/// <returns type="jQuery" />
/// <param name="fn" type="Function">
///     A function to execute each time the event is triggered.
/// </param>

		return fn ? this.bind( name, fn ) : this.trigger( name );
	};
jQuery.prototype.mouseout = function( fn ) {
/// <summary>
///     Bind an event handler to the "mouseout" JavaScript event, or trigger that event on an element.
///     1 - mouseout(handler(eventObject)) 
///     2 - mouseout()
/// </summary>
/// <returns type="jQuery" />
/// <param name="fn" type="Function">
///     A function to execute each time the event is triggered.
/// </param>

		return fn ? this.bind( name, fn ) : this.trigger( name );
	};
jQuery.prototype.mouseover = function( fn ) {
/// <summary>
///     Bind an event handler to the "mouseover" JavaScript event, or trigger that event on an element.
///     1 - mouseover(handler(eventObject)) 
///     2 - mouseover()
/// </summary>
/// <returns type="jQuery" />
/// <param name="fn" type="Function">
///     A function to execute each time the event is triggered.
/// </param>

		return fn ? this.bind( name, fn ) : this.trigger( name );
	};
jQuery.prototype.mouseup = function( fn ) {
/// <summary>
///     Bind an event handler to the "mouseup" JavaScript event, or trigger that event on an element.
///     1 - mouseup(handler(eventObject)) 
///     2 - mouseup()
/// </summary>
/// <returns type="jQuery" />
/// <param name="fn" type="Function">
///     A function to execute each time the event is triggered.
/// </param>

		return fn ? this.bind( name, fn ) : this.trigger( name );
	};
jQuery.prototype.next = function( until, selector ) {
/// <summary>
///     Get the immediately following sibling of each element in the set of matched elements, optionally filtered by a selector.
///     
/// </summary>
/// <returns type="jQuery" />
/// <param name="until" type="String">
///     A string containing a selector expression to match elements against.
/// </param>

		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};
jQuery.prototype.nextAll = function( until, selector ) {
/// <summary>
///     Get all following siblings of each element in the set of matched elements, optionally filtered by a selector.
///     
/// </summary>
/// <returns type="jQuery" />
/// <param name="until" type="String">
///     A string containing a selector expression to match elements against.
/// </param>

		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};
jQuery.prototype.nextUntil = function( until, selector ) {
/// <summary>
///     Get all following siblings of each element up to but not including the element matched by the selector.
///     
/// </summary>
/// <returns type="jQuery" />
/// <param name="until" type="String">
///     A string containing a selector expression to indicate where to stop matching following sibling elements.
/// </param>

		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};
jQuery.prototype.not = function( selector ) {
/// <summary>
///     Remove elements from the set of matched elements.
///     1 - not(selector) 
///     2 - not(elements) 
///     3 - not(function(index))
/// </summary>
/// <returns type="jQuery" />
/// <param name="selector" type="String">
///     A string containing a selector expression to match elements against.
/// </param>

		return this.pushStack( winnow(this, selector, false), "not", selector);
	};
jQuery.prototype.offset = function( options ) {
/// <summary>
///     1: Get the current coordinates of the first element in the set of matched elements, relative to the document.
///         1.1 - offset()
///     2: Set the current coordinates of every element in the set of matched elements, relative to the document.
///         2.1 - offset(coordinates) 
///         2.2 - offset(function(index, coords))
/// </summary>
/// <returns type="jQuery" />
/// <param name="options" type="Object">
///     
///                 An object containing the properties top and left, which are integers indicating the new top and left coordinates for the elements.
///               
/// </param>

		var elem = this[0];

		if ( options ) { 
			return this.each(function( i ) {
				jQuery.offset.setOffset( this, options, i );
			});
		}

		if ( !elem || !elem.ownerDocument ) {
			return null;
		}

		if ( elem === elem.ownerDocument.body ) {
			return jQuery.offset.bodyOffset( elem );
		}

		var box = elem.getBoundingClientRect(), doc = elem.ownerDocument, body = doc.body, docElem = doc.documentElement,
			clientTop = docElem.clientTop || body.clientTop || 0, clientLeft = docElem.clientLeft || body.clientLeft || 0,
			top  = box.top  + (self.pageYOffset || jQuery.support.boxModel && docElem.scrollTop  || body.scrollTop ) - clientTop,
			left = box.left + (self.pageXOffset || jQuery.support.boxModel && docElem.scrollLeft || body.scrollLeft) - clientLeft;

		return { top: top, left: left };
	};
jQuery.prototype.offsetParent = function() {
/// <summary>
///     Get the closest ancestor element that is positioned.
///     
/// </summary>
/// <returns type="jQuery" />

		return this.map(function() {
			var offsetParent = this.offsetParent || document.body;
			while ( offsetParent && (!/^body|html$/i.test(offsetParent.nodeName) && jQuery.css(offsetParent, "position") === "static") ) {
				offsetParent = offsetParent.offsetParent;
			}
			return offsetParent;
		});
	};
jQuery.prototype.one = function( type, data, fn ) {
/// <summary>
///     Attach a handler to an event for the elements. The handler is executed at most once per element.
///     
/// </summary>
/// <returns type="jQuery" />
/// <param name="type" type="String">
///     A string containing one or more JavaScript event types, such as "click" or "submit," or custom event names.
/// </param>
/// <param name="data" type="Object">
///     A map of data that will be passed to the event handler.
/// </param>
/// <param name="fn" type="Function">
///     A function to execute at the time the event is triggered.
/// </param>

		// Handle object literals
		if ( typeof type === "object" ) {
			for ( var key in type ) {
				this[ name ](key, data, type[key], fn);
			}
			return this;
		}
		
		if ( jQuery.isFunction( data ) ) {
			fn = data;
			data = undefined;
		}

		var handler = name === "one" ? jQuery.proxy( fn, function( event ) {
			jQuery( this ).unbind( event, handler );
			return fn.apply( this, arguments );
		}) : fn;

		if ( type === "unload" && name !== "one" ) {
			this.one( type, data, fn );

		} else {
			for ( var i = 0, l = this.length; i < l; i++ ) {
				jQuery.event.add( this[i], type, handler, data );
			}
		}

		return this;
	};
jQuery.prototype.outerHeight = function( margin ) {
/// <summary>
///     Get the current computed height for the first element in the set of matched elements, including padding and border.
///     
/// </summary>
/// <returns type="Number" />
/// <param name="margin" type="Boolean">
///     A Boolean indicating whether to include the element's margin in the calculation.
/// </param>

		return this[0] ?
			jQuery.css( this[0], type, false, margin ? "margin" : "border" ) :
			null;
	};
jQuery.prototype.outerWidth = function( margin ) {
/// <summary>
///     Get the current computed width for the first element in the set of matched elements, including padding and border.
///     
/// </summary>
/// <returns type="Number" />
/// <param name="margin" type="Boolean">
///     A Boolean indicating whether to include the element's margin in the calculation.
/// </param>

		return this[0] ?
			jQuery.css( this[0], type, false, margin ? "margin" : "border" ) :
			null;
	};
jQuery.prototype.parent = function( until, selector ) {
/// <summary>
///     Get the parent of each element in the current set of matched elements, optionally filtered by a selector.
///     
/// </summary>
/// <returns type="jQuery" />
/// <param name="until" type="String">
///     A string containing a selector expression to match elements against.
/// </param>

		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};
jQuery.prototype.parents = function( until, selector ) {
/// <summary>
///     Get the ancestors of each element in the current set of matched elements, optionally filtered by a selector.
///     
/// </summary>
/// <returns type="jQuery" />
/// <param name="until" type="String">
///     A string containing a selector expression to match elements against.
/// </param>

		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};
jQuery.prototype.parentsUntil = function( until, selector ) {
/// <summary>
///     Get the ancestors of each element in the current set of matched elements, up to but not including the element matched by the selector.
///     
/// </summary>
/// <returns type="jQuery" />
/// <param name="until" type="String">
///     A string containing a selector expression to indicate where to stop matching ancestor elements.
/// </param>

		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};
jQuery.prototype.position = function() {
/// <summary>
///     Get the current coordinates of the first element in the set of matched elements, relative to the offset parent.
///     
/// </summary>
/// <returns type="Object" />

		if ( !this[0] ) {
			return null;
		}

		var elem = this[0],

		// Get *real* offsetParent
		offsetParent = this.offsetParent(),

		// Get correct offsets
		offset       = this.offset(),
		parentOffset = /^body|html$/i.test(offsetParent[0].nodeName) ? { top: 0, left: 0 } : offsetParent.offset();

		// Subtract element margins
		// note: when an element has margin: auto the offsetLeft and marginLeft
		// are the same in Safari causing offset.left to incorrectly be 0
		offset.top  -= parseFloat( jQuery.curCSS(elem, "marginTop",  true) ) || 0;
		offset.left -= parseFloat( jQuery.curCSS(elem, "marginLeft", true) ) || 0;

		// Add offsetParent borders
		parentOffset.top  += parseFloat( jQuery.curCSS(offsetParent[0], "borderTopWidth",  true) ) || 0;
		parentOffset.left += parseFloat( jQuery.curCSS(offsetParent[0], "borderLeftWidth", true) ) || 0;

		// Subtract the two offsets
		return {
			top:  offset.top  - parentOffset.top,
			left: offset.left - parentOffset.left
		};
	};
jQuery.prototype.prepend = function() {
/// <summary>
///     Insert content, specified by the parameter, to the beginning of each element in the set of matched elements.
///     1 - prepend(content) 
///     2 - prepend(function(index, html))
/// </summary>
/// <returns type="jQuery" />
/// <param name="" type="jQuery">
///     An element, HTML string, or jQuery object to insert at the beginning of each element in the set of matched elements.
/// </param>

		return this.domManip(arguments, true, function( elem ) {
			if ( this.nodeType === 1 ) {
				this.insertBefore( elem, this.firstChild );
			}
		});
	};
jQuery.prototype.prependTo = function( selector ) {
/// <summary>
///     Insert every element in the set of matched elements to the beginning of the target.
///     
/// </summary>
/// <returns type="jQuery" />
/// <param name="selector" type="jQuery">
///     A selector, element, HTML string, or jQuery object; the matched set of elements will be inserted at the beginning of the element(s) specified by this parameter.
/// </param>

		var ret = [], insert = jQuery( selector ),
			parent = this.length === 1 && this[0].parentNode;
		
		if ( parent && parent.nodeType === 11 && parent.childNodes.length === 1 && insert.length === 1 ) {
			insert[ original ]( this[0] );
			return this;
			
		} else {
			for ( var i = 0, l = insert.length; i < l; i++ ) {
				var elems = (i > 0 ? this.clone(true) : this).get();
				jQuery.fn[ original ].apply( jQuery(insert[i]), elems );
				ret = ret.concat( elems );
			}
		
			return this.pushStack( ret, name, insert.selector );
		}
	};
jQuery.prototype.prev = function( until, selector ) {
/// <summary>
///     Get the immediately preceding sibling of each element in the set of matched elements, optionally filtered by a selector.
///     
/// </summary>
/// <returns type="jQuery" />
/// <param name="until" type="String">
///     A string containing a selector expression to match elements against.
/// </param>

		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};
jQuery.prototype.prevAll = function( until, selector ) {
/// <summary>
///     Get all preceding siblings of each element in the set of matched elements, optionally filtered by a selector.
///     
/// </summary>
/// <returns type="jQuery" />
/// <param name="until" type="String">
///     A string containing a selector expression to match elements against.
/// </param>

		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};
jQuery.prototype.prevUntil = function( until, selector ) {
/// <summary>
///     Get all preceding siblings of each element up to but not including the element matched by the selector.
///     
/// </summary>
/// <returns type="jQuery" />
/// <param name="until" type="String">
///     A string containing a selector expression to indicate where to stop matching preceding sibling elements.
/// </param>

		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};
jQuery.prototype.pushStack = function( elems, name, selector ) {
/// <summary>
///     Add a collection of DOM elements onto the jQuery stack.
///     1 - jQuery.pushStack(elements) 
///     2 - jQuery.pushStack(elements, name, arguments)
/// </summary>
/// <returns type="jQuery" />
/// <param name="elems" type="Array">
///     An array of elements to push onto the stack and make into a new jQuery object.
/// </param>
/// <param name="name" type="String">
///     The name of a jQuery method that generated the array of elements.
/// </param>
/// <param name="selector" type="Array">
///     The arguments that were passed in to the jQuery method (for serialization).
/// </param>

		// Build a new jQuery matched element set
		var ret = jQuery();

		if ( jQuery.isArray( elems ) ) {
			push.apply( ret, elems );
		
		} else {
			jQuery.merge( ret, elems );
		}

		// Add the old object onto the stack (as a reference)
		ret.prevObject = this;

		ret.context = this.context;

		if ( name === "find" ) {
			ret.selector = this.selector + (this.selector ? " " : "") + selector;
		} else if ( name ) {
			ret.selector = this.selector + "." + name + "(" + selector + ")";
		}

		// Return the newly-formed element set
		return ret;
	};
jQuery.prototype.queue = function( type, data ) {
/// <summary>
///     1: Show the queue of functions to be executed on the matched elements.
///         1.1 - queue(queueName)
///     2: Manipulate the queue of functions to be executed on the matched elements.
///         2.1 - queue(queueName, newQueue) 
///         2.2 - queue(queueName, callback( next ))
/// </summary>
/// <returns type="jQuery" />
/// <param name="type" type="String">
///     
///                 A string containing the name of the queue. Defaults to fx, the standard effects queue.
///               
/// </param>
/// <param name="data" type="Array">
///     An array of functions to replace the current queue contents.
/// </param>

		if ( typeof type !== "string" ) {
			data = type;
			type = "fx";
		}

		if ( data === undefined ) {
			return jQuery.queue( this[0], type );
		}
		return this.each(function( i, elem ) {
			var queue = jQuery.queue( this, type, data );

			if ( type === "fx" && queue[0] !== "inprogress" ) {
				jQuery.dequeue( this, type );
			}
		});
	};
jQuery.prototype.ready = function( fn ) {
/// <summary>
///     Specify a function to execute when the DOM is fully loaded.
///     
/// </summary>
/// <returns type="jQuery" />
/// <param name="fn" type="Function">
///     A function to execute after the DOM is ready.
/// </param>

		// Attach the listeners
		jQuery.bindReady();

		// If the DOM is already ready
		if ( jQuery.isReady ) {
			// Execute the function immediately
			fn.call( document, jQuery );

		// Otherwise, remember the function for later
		} else if ( readyList ) {
			// Add the function to the wait list
			readyList.push( fn );
		}

		return this;
	};
jQuery.prototype.remove = function( selector, keepData ) {
/// <summary>
///     Remove the set of matched elements from the DOM.
///     
/// </summary>
/// <returns type="jQuery" />
/// <param name="selector" type="String">
///     A selector expression that filters the set of matched elements to be removed.
/// </param>

		for ( var i = 0, elem; (elem = this[i]) != null; i++ ) {
			if ( !selector || jQuery.filter( selector, [ elem ] ).length ) {
				if ( !keepData && elem.nodeType === 1 ) {
					jQuery.cleanData( elem.getElementsByTagName("*") );
					jQuery.cleanData( [ elem ] );
				}

				if ( elem.parentNode ) {
					 elem.parentNode.removeChild( elem );
				}
			}
		}
		
		return this;
	};
jQuery.prototype.removeAttr = function( name, fn ) {
/// <summary>
///     Remove an attribute from each element in the set of matched elements.
///     
/// </summary>
/// <returns type="jQuery" />
/// <param name="name" type="String">
///     An attribute to remove.
/// </param>

		return this.each(function(){
			jQuery.attr( this, name, "" );
			if ( this.nodeType === 1 ) {
				this.removeAttribute( name );
			}
		});
	};
jQuery.prototype.removeClass = function( value ) {
/// <summary>
///     Remove a single class, multiple classes, or all classes from each element in the set of matched elements.
///     1 - removeClass(className) 
///     2 - removeClass(function(index, class))
/// </summary>
/// <returns type="jQuery" />
/// <param name="value" type="String">
///     A class name to be removed from the class attribute of each matched element.
/// </param>

		if ( jQuery.isFunction(value) ) {
			return this.each(function(i) {
				var self = jQuery(this);
				self.removeClass( value.call(this, i, self.attr("class")) );
			});
		}

		if ( (value && typeof value === "string") || value === undefined ) {
			var classNames = (value || "").split(rspace);

			for ( var i = 0, l = this.length; i < l; i++ ) {
				var elem = this[i];

				if ( elem.nodeType === 1 && elem.className ) {
					if ( value ) {
						var className = (" " + elem.className + " ").replace(rclass, " ");
						for ( var c = 0, cl = classNames.length; c < cl; c++ ) {
							className = className.replace(" " + classNames[c] + " ", " ");
						}
						elem.className = jQuery.trim( className );

					} else {
						elem.className = "";
					}
				}
			}
		}

		return this;
	};
jQuery.prototype.removeData = function( key ) {
/// <summary>
///     Remove a previously-stored piece of data.
///     
/// </summary>
/// <returns type="jQuery" />
/// <param name="key" type="String">
///     A string naming the piece of data to delete.
/// </param>

		return this.each(function() {
			jQuery.removeData( this, key );
		});
	};
jQuery.prototype.replaceAll = function( selector ) {
/// <summary>
///     Replace each target element with the set of matched elements.
///     
/// </summary>
/// <returns type="jQuery" />

		var ret = [], insert = jQuery( selector ),
			parent = this.length === 1 && this[0].parentNode;
		
		if ( parent && parent.nodeType === 11 && parent.childNodes.length === 1 && insert.length === 1 ) {
			insert[ original ]( this[0] );
			return this;
			
		} else {
			for ( var i = 0, l = insert.length; i < l; i++ ) {
				var elems = (i > 0 ? this.clone(true) : this).get();
				jQuery.fn[ original ].apply( jQuery(insert[i]), elems );
				ret = ret.concat( elems );
			}
		
			return this.pushStack( ret, name, insert.selector );
		}
	};
jQuery.prototype.replaceWith = function( value ) {
/// <summary>
///     Replace each element in the set of matched elements with the provided new content.
///     1 - replaceWith(newContent) 
///     2 - replaceWith(function)
/// </summary>
/// <returns type="jQuery" />
/// <param name="value" type="jQuery">
///     The content to insert. May be an HTML string, DOM element, or jQuery object.
/// </param>

		if ( this[0] && this[0].parentNode ) {
			// Make sure that the elements are removed from the DOM before they are inserted
			// this can help fix replacing a parent with child elements
			if ( jQuery.isFunction( value ) ) {
				return this.each(function(i) {
					var self = jQuery(this), old = self.html();
					self.replaceWith( value.call( this, i, old ) );
				});
			}

			if ( typeof value !== "string" ) {
				value = jQuery(value).detach();
			}

			return this.each(function() {
				var next = this.nextSibling, parent = this.parentNode;

				jQuery(this).remove();

				if ( next ) {
					jQuery(next).before( value );
				} else {
					jQuery(parent).append( value );
				}
			});
		} else {
			return this.pushStack( jQuery(jQuery.isFunction(value) ? value() : value), "replaceWith", value );
		}
	};
jQuery.prototype.resize = function( fn ) {
/// <summary>
///     Bind an event handler to the "resize" JavaScript event, or trigger that event on an element.
///     1 - resize(handler(eventObject)) 
///     2 - resize()
/// </summary>
/// <returns type="jQuery" />
/// <param name="fn" type="Function">
///     A function to execute each time the event is triggered.
/// </param>

		return fn ? this.bind( name, fn ) : this.trigger( name );
	};
jQuery.prototype.scroll = function( fn ) {
/// <summary>
///     Bind an event handler to the "scroll" JavaScript event, or trigger that event on an element.
///     1 - scroll(handler(eventObject)) 
///     2 - scroll()
/// </summary>
/// <returns type="jQuery" />
/// <param name="fn" type="Function">
///     A function to execute each time the event is triggered.
/// </param>

		return fn ? this.bind( name, fn ) : this.trigger( name );
	};
jQuery.prototype.scrollLeft = function(val) {
/// <summary>
///     1: Get the current horizontal position of the scroll bar for the first element in the set of matched elements.
///         1.1 - scrollLeft()
///     2: Set the current horizontal position of the scroll bar for each of the set of matched elements.
///         2.1 - scrollLeft(value)
/// </summary>
/// <returns type="jQuery" />
/// <param name="val" type="Number">
///     An integer indicating the new position to set the scroll bar to.
/// </param>

		var elem = this[0], win;
		
		if ( !elem ) {
			return null;
		}

		if ( val !== undefined ) {
			// Set the scroll offset
			return this.each(function() {
				win = getWindow( this );

				if ( win ) {
					win.scrollTo(
						!i ? val : jQuery(win).scrollLeft(),
						 i ? val : jQuery(win).scrollTop()
					);

				} else {
					this[ method ] = val;
				}
			});
		} else {
			win = getWindow( elem );

			// Return the scroll offset
			return win ? ("pageXOffset" in win) ? win[ i ? "pageYOffset" : "pageXOffset" ] :
				jQuery.support.boxModel && win.document.documentElement[ method ] ||
					win.document.body[ method ] :
				elem[ method ];
		}
	};
jQuery.prototype.scrollTop = function(val) {
/// <summary>
///     1: Get the current vertical position of the scroll bar for the first element in the set of matched elements.
///         1.1 - scrollTop()
///     2: Set the current vertical position of the scroll bar for each of the set of matched elements.
///         2.1 - scrollTop(value)
/// </summary>
/// <returns type="jQuery" />
/// <param name="val" type="Number">
///     An integer indicating the new position to set the scroll bar to.
/// </param>

		var elem = this[0], win;
		
		if ( !elem ) {
			return null;
		}

		if ( val !== undefined ) {
			// Set the scroll offset
			return this.each(function() {
				win = getWindow( this );

				if ( win ) {
					win.scrollTo(
						!i ? val : jQuery(win).scrollLeft(),
						 i ? val : jQuery(win).scrollTop()
					);

				} else {
					this[ method ] = val;
				}
			});
		} else {
			win = getWindow( elem );

			// Return the scroll offset
			return win ? ("pageXOffset" in win) ? win[ i ? "pageYOffset" : "pageXOffset" ] :
				jQuery.support.boxModel && win.document.documentElement[ method ] ||
					win.document.body[ method ] :
				elem[ method ];
		}
	};
jQuery.prototype.select = function( fn ) {
/// <summary>
///     Bind an event handler to the "select" JavaScript event, or trigger that event on an element.
///     1 - select(handler(eventObject)) 
///     2 - select()
/// </summary>
/// <returns type="jQuery" />
/// <param name="fn" type="Function">
///     A function to execute each time the event is triggered.
/// </param>

		return fn ? this.bind( name, fn ) : this.trigger( name );
	};
jQuery.prototype.serialize = function() {
/// <summary>
///     Encode a set of form elements as a string for submission.
///     
/// </summary>
/// <returns type="String" />

		return jQuery.param(this.serializeArray());
	};
jQuery.prototype.serializeArray = function() {
/// <summary>
///     Encode a set of form elements as an array of names and values.
///     
/// </summary>
/// <returns type="Array" />

		return this.map(function() {
			return this.elements ? jQuery.makeArray(this.elements) : this;
		})
		.filter(function() {
			return this.name && !this.disabled &&
				(this.checked || rselectTextarea.test(this.nodeName) ||
					rinput.test(this.type));
		})
		.map(function( i, elem ) {
			var val = jQuery(this).val();

			return val == null ?
				null :
				jQuery.isArray(val) ?
					jQuery.map( val, function( val, i ) {
						return { name: elem.name, value: val };
					}) :
					{ name: elem.name, value: val };
		}).get();
	};
jQuery.prototype.show = function( speed, callback ) {
/// <summary>
///     Display the matched elements.
///     1 - show() 
///     2 - show(duration, callback)
/// </summary>
/// <returns type="jQuery" />
/// <param name="speed" type="Number">
///     A string or number determining how long the animation will run.
/// </param>
/// <param name="callback" type="Function">
///     A function to call once the animation is complete.
/// </param>

		if ( speed || speed === 0) {
			return this.animate( genFx("show", 3), speed, callback);

		} else {
			for ( var i = 0, l = this.length; i < l; i++ ) {
				var old = jQuery.data(this[i], "olddisplay");

				this[i].style.display = old || "";

				if ( jQuery.css(this[i], "display") === "none" ) {
					var nodeName = this[i].nodeName, display;

					if ( elemdisplay[ nodeName ] ) {
						display = elemdisplay[ nodeName ];

					} else {
						var elem = jQuery("<" + nodeName + " />").appendTo("body");

						display = elem.css("display");

						if ( display === "none" ) {
							display = "block";
						}

						elem.remove();

						elemdisplay[ nodeName ] = display;
					}

					jQuery.data(this[i], "olddisplay", display);
				}
			}

			// Set the display of the elements in a second loop
			// to avoid the constant reflow
			for ( var j = 0, k = this.length; j < k; j++ ) {
				this[j].style.display = jQuery.data(this[j], "olddisplay") || "";
			}

			return this;
		}
	};
jQuery.prototype.siblings = function( until, selector ) {
/// <summary>
///     Get the siblings of each element in the set of matched elements, optionally filtered by a selector.
///     
/// </summary>
/// <returns type="jQuery" />
/// <param name="until" type="String">
///     A string containing a selector expression to match elements against.
/// </param>

		var ret = jQuery.map( this, fn, until );
		
		if ( !runtil.test( name ) ) {
			selector = until;
		}

		if ( selector && typeof selector === "string" ) {
			ret = jQuery.filter( selector, ret );
		}

		ret = this.length > 1 ? jQuery.unique( ret ) : ret;

		if ( (this.length > 1 || rmultiselector.test( selector )) && rparentsprev.test( name ) ) {
			ret = ret.reverse();
		}

		return this.pushStack( ret, name, slice.call(arguments).join(",") );
	};
jQuery.prototype.size = function() {
/// <summary>
///     Return the number of DOM elements matched by the jQuery object.
///     
/// </summary>
/// <returns type="Number" />

		return this.length;
	};
jQuery.prototype.slice = function() {
/// <summary>
///     Reduce the set of matched elements to a subset specified by a range of indices.
///     
/// </summary>
/// <returns type="jQuery" />
/// <param name="" type="Number">
///     An integer indicating the 0-based position after which the elements are selected. If negative, it indicates an offset from the end of the set.
/// </param>
/// <param name="{name}" type="Number">
///     An integer indicating the 0-based position before which the elements stop being selected. If negative, it indicates an offset from the end of the set. If omitted, the range continues until the end of the set.
/// </param>

		return this.pushStack( slice.apply( this, arguments ),
			"slice", slice.call(arguments).join(",") );
	};
jQuery.prototype.slideDown = function( speed, callback ) {
/// <summary>
///     Display the matched elements with a sliding motion.
///     
/// </summary>
/// <returns type="jQuery" />
/// <param name="speed" type="Number">
///     A string or number determining how long the animation will run.
/// </param>
/// <param name="callback" type="Function">
///     A function to call once the animation is complete.
/// </param>

		return this.animate( props, speed, callback );
	};
jQuery.prototype.slideToggle = function( speed, callback ) {
/// <summary>
///     Display or hide the matched elements with a sliding motion.
///     
/// </summary>
/// <returns type="jQuery" />
/// <param name="speed" type="Number">
///     A string or number determining how long the animation will run.
/// </param>
/// <param name="callback" type="Function">
///     A function to call once the animation is complete.
/// </param>

		return this.animate( props, speed, callback );
	};
jQuery.prototype.slideUp = function( speed, callback ) {
/// <summary>
///     Hide the matched elements with a sliding motion.
///     
/// </summary>
/// <returns type="jQuery" />
/// <param name="speed" type="Number">
///     A string or number determining how long the animation will run.
/// </param>
/// <param name="callback" type="Function">
///     A function to call once the animation is complete.
/// </param>

		return this.animate( props, speed, callback );
	};
jQuery.prototype.stop = function( clearQueue, gotoEnd ) {
/// <summary>
///     Stop the currently-running animation on the matched elements.
///     
/// </summary>
/// <returns type="jQuery" />
/// <param name="clearQueue" type="Boolean">
///     
///                 A Boolean indicating whether to remove queued animation as well. Defaults to false.
///               
/// </param>
/// <param name="gotoEnd" type="Boolean">
///     
///                 A Boolean indicating whether to complete the current animation immediately. Defaults to false.
///               
/// </param>

		var timers = jQuery.timers;

		if ( clearQueue ) {
			this.queue([]);
		}

		this.each(function() {
			// go in reverse order so anything added to the queue during the loop is ignored
			for ( var i = timers.length - 1; i >= 0; i-- ) {
				if ( timers[i].elem === this ) {
					if (gotoEnd) {
						// force the next step to be the last
						timers[i](true);
					}

					timers.splice(i, 1);
				}
			}
		});

		// start the next in the queue if the last step wasn't forced
		if ( !gotoEnd ) {
			this.dequeue();
		}

		return this;
	};
jQuery.prototype.submit = function( fn ) {
/// <summary>
///     Bind an event handler to the "submit" JavaScript event, or trigger that event on an element.
///     1 - submit(handler(eventObject)) 
///     2 - submit()
/// </summary>
/// <returns type="jQuery" />
/// <param name="fn" type="Function">
///     A function to execute each time the event is triggered.
/// </param>

		return fn ? this.bind( name, fn ) : this.trigger( name );
	};
jQuery.prototype.text = function( text ) {
/// <summary>
///     1: Get the combined text contents of each element in the set of matched elements, including their descendants.
///         1.1 - text()
///     2: Set the content of each element in the set of matched elements to the specified text.
///         2.1 - text(textString) 
///         2.2 - text(function(index, text))
/// </summary>
/// <returns type="jQuery" />
/// <param name="text" type="String">
///     A string of text to set as the content of each matched element.
/// </param>

		if ( jQuery.isFunction(text) ) {
			return this.each(function(i) {
				var self = jQuery(this);
				self.text( text.call(this, i, self.text()) );
			});
		}

		if ( typeof text !== "object" && text !== undefined ) {
			return this.empty().append( (this[0] && this[0].ownerDocument || document).createTextNode( text ) );
		}

		return jQuery.text( this );
	};
jQuery.prototype.toArray = function() {
/// <summary>
///     Retrieve all the DOM elements contained in the jQuery set, as an array.
///     
/// </summary>
/// <returns type="Array" />

		return slice.call( this, 0 );
	};
jQuery.prototype.toggle = function( fn, fn2 ) {
/// <summary>
///     1: Bind two or more handlers to the matched elements, to be executed on alternate clicks.
///         1.1 - toggle(handler(eventObject), handler(eventObject), handler(eventObject))
///     2: Display or hide the matched elements.
///         2.1 - toggle(duration, callback) 
///         2.2 - toggle(showOrHide)
/// </summary>
/// <returns type="jQuery" />
/// <param name="fn" type="Function">
///     A function to execute every even time the element is clicked.
/// </param>
/// <param name="fn2" type="Function">
///     A function to execute every odd time the element is clicked.
/// </param>
/// <param name="{name}" type="Function">
///     Additional handlers to cycle through after clicks.
/// </param>

		var bool = typeof fn === "boolean";

		if ( jQuery.isFunction(fn) && jQuery.isFunction(fn2) ) {
			this._toggle.apply( this, arguments );

		} else if ( fn == null || bool ) {
			this.each(function() {
				var state = bool ? fn : jQuery(this).is(":hidden");
				jQuery(this)[ state ? "show" : "hide" ]();
			});

		} else {
			this.animate(genFx("toggle", 3), fn, fn2);
		}

		return this;
	};
jQuery.prototype.toggleClass = function( value, stateVal ) {
/// <summary>
///     Add or remove one or more classes from each element in the set of matched elements, depending on either the class's presence or the value of the switch argument.
///     1 - toggleClass(className) 
///     2 - toggleClass(className, switch) 
///     3 - toggleClass(function(index, class), switch)
/// </summary>
/// <returns type="jQuery" />
/// <param name="value" type="String">
///     One or more class names (separated by spaces) to be toggled for each element in the matched set.
/// </param>
/// <param name="stateVal" type="Boolean">
///     A boolean value to determine whether the class should be added or removed.
/// </param>

		var type = typeof value, isBool = typeof stateVal === "boolean";

		if ( jQuery.isFunction( value ) ) {
			return this.each(function(i) {
				var self = jQuery(this);
				self.toggleClass( value.call(this, i, self.attr("class"), stateVal), stateVal );
			});
		}

		return this.each(function() {
			if ( type === "string" ) {
				// toggle individual class names
				var className, i = 0, self = jQuery(this),
					state = stateVal,
					classNames = value.split( rspace );

				while ( (className = classNames[ i++ ]) ) {
					// check each className given, space seperated list
					state = isBool ? state : !self.hasClass( className );
					self[ state ? "addClass" : "removeClass" ]( className );
				}

			} else if ( type === "undefined" || type === "boolean" ) {
				if ( this.className ) {
					// store className if set
					jQuery.data( this, "__className__", this.className );
				}

				// toggle whole className
				this.className = this.className || value === false ? "" : jQuery.data( this, "__className__" ) || "";
			}
		});
	};
jQuery.prototype.trigger = function( type, data ) {
/// <summary>
///     Execute all handlers and behaviors attached to the matched elements for the given event type.
///     
/// </summary>
/// <returns type="jQuery" />
/// <param name="type" type="String">
///     
///                 A string containing a JavaScript event type, such as click or submit.
///               
/// </param>
/// <param name="data" type="Array">
///     An array of additional parameters to pass along to the event handler.
/// </param>

		return this.each(function() {
			jQuery.event.trigger( type, data, this );
		});
	};
jQuery.prototype.triggerHandler = function( type, data ) {
/// <summary>
///     Execute all handlers attached to an element for an event.
///     
/// </summary>
/// <returns type="Object" />
/// <param name="type" type="String">
///     
///                 A string containing a JavaScript event type, such as click or submit.
///               
/// </param>
/// <param name="data" type="Array">
///     An array of additional parameters to pass along to the event handler.
/// </param>

		if ( this[0] ) {
			var event = jQuery.Event( type );
			event.preventDefault();
			event.stopPropagation();
			jQuery.event.trigger( event, data, this[0] );
			return event.result;
		}
	};
jQuery.prototype.unbind = function( type, fn ) {
/// <summary>
///     Remove a previously-attached event handler from the elements.
///     1 - unbind(eventType, handler(eventObject)) 
///     2 - unbind(event)
/// </summary>
/// <returns type="jQuery" />
/// <param name="type" type="String">
///     
///                 A string containing a JavaScript event type, such as click or submit.
///               
/// </param>
/// <param name="fn" type="Function">
///     The function that is to be no longer executed.
/// </param>

		// Handle object literals
		if ( typeof type === "object" && !type.preventDefault ) {
			for ( var key in type ) {
				this.unbind(key, type[key]);
			}

		} else {
			for ( var i = 0, l = this.length; i < l; i++ ) {
				jQuery.event.remove( this[i], type, fn );
			}
		}

		return this;
	};
jQuery.prototype.undelegate = function( selector, types, fn ) {
/// <summary>
///     Remove a handler from the event for all elements which match the current selector, now or in the future, based upon a specific set of root elements.
///     1 - undelegate() 
///     2 - undelegate(selector, eventType) 
///     3 - undelegate(selector, eventType, handler)
/// </summary>
/// <returns type="jQuery" />
/// <param name="selector" type="String">
///     A selector which will be used to filter the event results.
/// </param>
/// <param name="types" type="String">
///     A string containing a JavaScript event type, such as "click" or "keydown"
/// </param>
/// <param name="fn" type="Function">
///     A function to execute at the time the event is triggered.
/// </param>

		if ( arguments.length === 0 ) {
				return this.unbind( "live" );
		
		} else {
			return this.die( types, null, fn, selector );
		}
	};
jQuery.prototype.unload = function( fn ) {
/// <summary>
///     Bind an event handler to the "unload" JavaScript event.
///     
/// </summary>
/// <returns type="jQuery" />
/// <param name="fn" type="Function">
///     A function to execute when the event is triggered.
/// </param>

		return fn ? this.bind( name, fn ) : this.trigger( name );
	};
jQuery.prototype.unwrap = function() {
/// <summary>
///     Remove the parents of the set of matched elements from the DOM, leaving the matched elements in their place.
///     
/// </summary>
/// <returns type="jQuery" />

		return this.parent().each(function() {
			if ( !jQuery.nodeName( this, "body" ) ) {
				jQuery( this ).replaceWith( this.childNodes );
			}
		}).end();
	};
jQuery.prototype.val = function( value ) {
/// <summary>
///     1: Get the current value of the first element in the set of matched elements.
///         1.1 - val()
///     2: Set the value of each element in the set of matched elements.
///         2.1 - val(value) 
///         2.2 - val(function(index, value))
/// </summary>
/// <returns type="jQuery" />
/// <param name="value" type="String">
///     A string of text or an array of strings to set as the value property of each matched element.
/// </param>

		if ( value === undefined ) {
			var elem = this[0];

			if ( elem ) {
				if ( jQuery.nodeName( elem, "option" ) ) {
					return (elem.attributes.value || {}).specified ? elem.value : elem.text;
				}

				// We need to handle select boxes special
				if ( jQuery.nodeName( elem, "select" ) ) {
					var index = elem.selectedIndex,
						values = [],
						options = elem.options,
						one = elem.type === "select-one";

					// Nothing was selected
					if ( index < 0 ) {
						return null;
					}

					// Loop through all the selected options
					for ( var i = one ? index : 0, max = one ? index + 1 : options.length; i < max; i++ ) {
						var option = options[ i ];

						if ( option.selected ) {
							// Get the specifc value for the option
							value = jQuery(option).val();

							// We don't need an array for one selects
							if ( one ) {
								return value;
							}

							// Multi-Selects return an array
							values.push( value );
						}
					}

					return values;
				}

				// Handle the case where in Webkit "" is returned instead of "on" if a value isn't specified
				if ( rradiocheck.test( elem.type ) && !jQuery.support.checkOn ) {
					return elem.getAttribute("value") === null ? "on" : elem.value;
				}
				

				// Everything else, we just grab the value
				return (elem.value || "").replace(rreturn, "");

			}

			return undefined;
		}

		var isFunction = jQuery.isFunction(value);

		return this.each(function(i) {
			var self = jQuery(this), val = value;

			if ( this.nodeType !== 1 ) {
				return;
			}

			if ( isFunction ) {
				val = value.call(this, i, self.val());
			}

			// Typecast each time if the value is a Function and the appended
			// value is therefore different each time.
			if ( typeof val === "number" ) {
				val += "";
			}

			if ( jQuery.isArray(val) && rradiocheck.test( this.type ) ) {
				this.checked = jQuery.inArray( self.val(), val ) >= 0;

			} else if ( jQuery.nodeName( this, "select" ) ) {
				var values = jQuery.makeArray(val);

				jQuery( "option", this ).each(function() {
					this.selected = jQuery.inArray( jQuery(this).val(), values ) >= 0;
				});

				if ( !values.length ) {
					this.selectedIndex = -1;
				}

			} else {
				this.value = val;
			}
		});
	};
jQuery.prototype.width = function( size ) {
/// <summary>
///     1: Get the current computed width for the first element in the set of matched elements.
///         1.1 - width()
///     2: Set the CSS width of each element in the set of matched elements.
///         2.1 - width(value) 
///         2.2 - width(function(index, width))
/// </summary>
/// <returns type="jQuery" />
/// <param name="size" type="Number">
///     An integer representing the number of pixels, or an integer along with an optional unit of measure appended (as a string).
/// </param>

		// Get window width or height
		var elem = this[0];
		if ( !elem ) {
			return size == null ? null : this;
		}
		
		if ( jQuery.isFunction( size ) ) {
			return this.each(function( i ) {
				var self = jQuery( this );
				self[ type ]( size.call( this, i, self[ type ]() ) );
			});
		}

		return ("scrollTo" in elem && elem.document) ? // does it walk and quack like a window?
			// Everyone else use document.documentElement or document.body depending on Quirks vs Standards mode
			elem.document.compatMode === "CSS1Compat" && elem.document.documentElement[ "client" + name ] ||
			elem.document.body[ "client" + name ] :

			// Get document width or height
			(elem.nodeType === 9) ? // is it a document
				// Either scroll[Width/Height] or offset[Width/Height], whichever is greater
				Math.max(
					elem.documentElement["client" + name],
					elem.body["scroll" + name], elem.documentElement["scroll" + name],
					elem.body["offset" + name], elem.documentElement["offset" + name]
				) :

				// Get or set width or height on the element
				size === undefined ?
					// Get width or height on the element
					jQuery.css( elem, type ) :

					// Set the width or height on the element (default to pixels if value is unitless)
					this.css( type, typeof size === "string" ? size : size + "px" );
	};
jQuery.prototype.wrap = function( html ) {
/// <summary>
///     Wrap an HTML structure around each element in the set of matched elements.
///     1 - wrap(wrappingElement) 
///     2 - wrap(wrappingFunction)
/// </summary>
/// <returns type="jQuery" />
/// <param name="html" type="jQuery">
///     An HTML snippet, selector expression, jQuery object, or DOM element specifying the structure to wrap around the matched elements.
/// </param>

		return this.each(function() {
			jQuery( this ).wrapAll( html );
		});
	};
jQuery.prototype.wrapAll = function( html ) {
/// <summary>
///     Wrap an HTML structure around all elements in the set of matched elements.
///     
/// </summary>
/// <returns type="jQuery" />
/// <param name="html" type="jQuery">
///     An HTML snippet, selector expression, jQuery object, or DOM element specifying the structure to wrap around the matched elements.
/// </param>

		if ( jQuery.isFunction( html ) ) {
			return this.each(function(i) {
				jQuery(this).wrapAll( html.call(this, i) );
			});
		}

		if ( this[0] ) {
			// The elements to wrap the target around
			var wrap = jQuery( html, this[0].ownerDocument ).eq(0).clone(true);

			if ( this[0].parentNode ) {
				wrap.insertBefore( this[0] );
			}

			wrap.map(function() {
				var elem = this;

				while ( elem.firstChild && elem.firstChild.nodeType === 1 ) {
					elem = elem.firstChild;
				}

				return elem;
			}).append(this);
		}

		return this;
	};
jQuery.prototype.wrapInner = function( html ) {
/// <summary>
///     Wrap an HTML structure around the content of each element in the set of matched elements.
///     1 - wrapInner(wrappingElement) 
///     2 - wrapInner(wrappingFunction)
/// </summary>
/// <returns type="jQuery" />
/// <param name="html" type="String">
///     An HTML snippet, selector expression, jQuery object, or DOM element specifying the structure to wrap around the content of the matched elements.
/// </param>

		if ( jQuery.isFunction( html ) ) {
			return this.each(function(i) {
				jQuery(this).wrapInner( html.call(this, i) );
			});
		}

		return this.each(function() {
			var self = jQuery( this ), contents = self.contents();

			if ( contents.length ) {
				contents.wrapAll( html );

			} else {
				self.append( html );
			}
		});
	};
jQuery.fn = jQuery.prototype;
jQuery.fn.init.prototype = jQuery.fn;
window.jQuery = window.$ = jQuery;
})(window);