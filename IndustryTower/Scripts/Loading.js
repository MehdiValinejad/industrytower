function loadingBox(target,Class) {
    $("<div id='loading' style='display:none' class='" + Class + "' ></div>").appendTo(target).fadeIn();
    //target.append("<div id='loading' style='display:none' ></div>").fadeIn();
}

$(document).ready(function () {
    //$(document).tooltip({ position: { my: "left+5 center", at: "right center" } });
    //PartialLoader
    PartialContentLoader(document);
    //PartialLoader

    $('input, textarea').placeholder();
    $(".expandable").expander();

    (function ($) {
        $("input[name = 'culture'] + label").click(function () {
            $(this).prev('input').attr('checked', 'checked');
            $(this).parents("form").submit(); // post form
        });
        $("input[name = 'culture']:checked").next("label").hide();
    })(jQuery);
     
    $(".user-menu li:not('.nored')").click(function () {
        window.location.replace($(this).find('a').attr('href'));
    });

    $("[title]").tooltip();

    $(document).delegate(".clear-input", "click", function () {
        $(this).prev().find("input").val("");
    });
    //var carrheight=0;
    //$(".main-carr .row").each(function (item,index) {
    //    if (carrheight > $(item).outerHeight()) carrheight = $(item).outerHeight();
    //    alert(carrheight)
    //    $(".main-carr").height(carrheight);
    //})

    //var ua = navigator.userAgent;
    //if (ua.indexOf("Android") >= 0) {
    //    var androidversion = parseFloat(ua.slice(ua.indexOf("Android") + 8));
    //    if (androidversion < 4.4) {
    //        document.write('<link href="//docs.google.com/uc?id=0B4od40H5_B_dbmZXRHNBdGktRWs&export=download"rel="stylesheet" type="text/css"><\/script>');
    //    }
    //}

    var tourname = $("#toururl").val();
    if (tourname != 'undefined' && tourname != null) {
        $("#thispagetour").parent().removeClass("display-none");
        $("#helpSign").css("color", "rgb(122, 105, 182)");
        $("#thispagetour").click(function (e) {
            e.preventDefault();
            loadingBox($("body"));
            $.getScript(location.protocol + '//' + location.host + "/Scripts/Tour/bootstrap-tour.min.js", function () {
                $.getScript(location.protocol + '//' + location.host + "/Scripts/Tour/" + tourname + ".min.js", function () {
                    $("body > #loading").remove();
                });
            });
        });
    }
});




function textAreaAdjust(o) {
    o.style.height = "1px";
    o.style.height = (5 + o.scrollHeight) + "px";
}




function findBootstrapEnvironment() {
    var envs = ['xs', 'sm', 'md', 'lg'];

    $el = $('<div>');
    $el.appendTo($('body'));

    for (var i = envs.length - 1; i >= 0; i--) {
        var env = envs[i];

        $el.addClass('hidden-' + env);
        if ($el.is(':hidden')) {
            $el.remove();
            return env
        }
    };
}
