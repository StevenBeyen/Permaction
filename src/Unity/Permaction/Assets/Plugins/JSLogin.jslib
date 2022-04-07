mergeInto(LibraryManager.library, {

    login : function(url, formdata)
    {
        var jsURL = Pointer_stringify(url);
        var jsFormData = Pointer_stringify(formdata);

        var xhttp = new XMLHttpRequest();
        xhttp.onreadystatechange = function(){
            if(this.readyState == 4 && this.status == 200){
                //console.log(xhttp.getResponseHeader('Set-Cookie'));
                //console.log(document.cookie);
            }
        };

        xhttp.open("POST", jsURL, true);
        xhttp.withCredentials = true;
        xhttp.setRequestHeader("Content-type", "application/json");
        xhttp.send(jsFormData);
    },
    
    /*getCookies: function()
    {
	    var cookies = document.cookie;
	    var length = lengthBytesUTF8(cookies) + 1;
	    var buffer = _malloc(length);
	    stringToUTF8(cookies, buffer, length);
	    return buffer;
    },*/

    getCookie : function(nameArg)
    {
	    var name = UTF8ToString(nameArg);
	    var cookie = document.cookie;
	    var search = name + "=";
	    var setStr = "";
	    var offset = 0;
	    var end = 0;
	    if (cookie.length > 0)
	    {
		    offset = cookie.indexOf(search);
		    if (offset != -1)
		    {
			    offset += search.length;
			    end = cookie.indexOf(";", offset);
			    if (end == -1)
			    {
				    end = cookie.length;
			    }
			    setStr = unescape(cookie.substring(offset, end));
		    }
	    }

	    var length = lengthBytesUTF8(setStr) + 1;
	    var buffer = _malloc(length);
	    stringToUTF8(setStr, buffer, length);
	    return buffer;
    },
    
});

