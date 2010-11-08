/// <reference path="jquery-1.4.2.js"/>



(function() {
    // ( ) { }
    var a = " ( ";
    var x = " ) ";

    var o = {
        z : 1,
        y : "test"
    };
})()



var i = /te\/st/; function a() {
    // {
    // {

    // }
    var x = "}";
}

function b() /*/*/ {
    var w = /*comment*//\/\/\\\\\/\\/;var a = { x : /}/ }; // "\/\"
    var x = /\\\/{\/a/;
    var y = /\/a/;
    var z = /\\\/}/;
    var eep = (1 / 2) + (3 * 0.5);
    var bee = ("foo" / "foo");
    return fff({ a: /test/, b: (1
    / 2) });
}

var result = (1 / 2) / (3 * 0.5);

foo(/blah/);

var x = 1
/ 2;

var firstLine = 1;
/test/.match("hello");

var o = { foo : /test/ };

// first char before slash [')'] has no classification, hence is regex literal <= FAIL
var z1 = (1 + 2) / (3 - 2);

function foo(p1, p2) {
    /// <summary>
    ///   Description
    ///   <para>This is a new paragraph</para> 
    ///   <para>   This paragraph is indented with spaces</para>
    ///   <para>This has a child paragraph
    ///       <para>   This is a nested paragraph</para>
    ///   </para>
    /// </summary>
    /// <param name="p1" type="String">
    ///   This is p1
    ///   <para>This is more info. about p1</para>
    /// </param>
    /// <param name="p2" type="Number">This is p2</param>
    /// <returns type="String" />

    return "";
}


function sample(name) {
    /// <summary>Description
    ///  <para>This is a new paragraph</para></summary>
    var o = {

    };
    return ""
}
sample(




function hello(name) {
    /// <param name="name" type="String" />
    return name + "Hello ";
}
var name = helloWorld("John");





var o = {
    alpha: {
        beta: 1,
        gamma: function (p1) {
            return this.beta;
        }
    },
    delta_one: {
        foo: 2
    },
    gamma$man: {
        alpha_ray: function () {},
        beta$ray: 1
    },
    _blah: "test",
    $foo: "test"
};



function bar() {
    /// <summary>Great summary</summary>
}

function missingParam(p1, ) {
    /// <summary>This is a bad function</summary>
    /// <param name="p1" type="String">Description</param>
}

function badParamName(-p1, p2) {
    /// <summary>This has a bad parameter name</summary>
    /// <param name="-p1" type="String">Description</param>
    /// <param name="p2" type="String">Description</param>
}

function tooMany(p1, p2) {
    /// <summary>This function declares more parms in doc comments than the signature</summary>
    /// <param name="p1" type="String">Description</param>
    /// <param name="p2" type="String">Description</param>
    /// <param name="p3" type="String">Description</param>
    
}

var obj = {





























































































};
































function fffff() {
    if ( true ) {
	    if ( match && (match[1] || !context) ) {
    	
        
        
        
        
        
            if ( match[1] ) {
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

	    } else if ( true ) {
		    this.selector = selector;
		    this.context = document;
		    selector = document.getElementsByTagName( selector );
		    return jQuery.merge( this, selector );
	    } else if ( !context || context.jquery ) {
		    return (context || rootjQuery).find( selector );
	    } else {
		    return jQuery( context ).find( selector );
	    }
    } else if ( jQuery.isFunction( selector ) ) {
	    return rootjQuery.ready( selector );
    }
}

var f1 = foo("", "");