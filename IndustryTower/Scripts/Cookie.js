
//$(document).ready(function () {
//    var isTour = readCookie("isTour");
//    if (isTour == null) {
//        //$("#isTour").show();
//        setTimeout(function () {
//            $(".navbar-pic").popover("show");
//        },5000)
//    }
//    //else { 
//    //    eraseCookie("isTour");
//    //}

    
//    $(".navbar-pic").click(function () {
//        $(".navbar-pic").popover("hide");
//        createCookie("isTour", "true", 1)
//    })
//});

function createCookie(name, value, days) {
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        var expires = "; expires=" + date.toGMTString();
    }
    else var expires = "";
    document.cookie = name + "=" + value + expires + "; path=/";
}

function readCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
}

function eraseCookie(name) {
    createCookie(name, "", -1);
}