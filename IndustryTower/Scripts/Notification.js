$(document).ready(function () {
    var ROOT = $("#ROOT").val();
    if ($("#allNotif").length > 0)
    {
        $.ajax({
            url: ROOT + "Notification/NotifCounter",
            type: "GET",
            cache: false,
            dataType: 'json',
            success: function (data) {
                if (data.FR > 0) {
                    $("#FR").show().text(data.FR);
                }
                if (data.NF > 0) {
                    $("#NF").show().text(data.NF);
                }
            }
        });
    }
     
    
    //****************************NotifICON**************************\\
    $(".notif-icon a").each(function (index, item) {
        var address = $(item).attr("href");
        if (window.location.pathname == address) {
            $(item).parent(".notif-icon").addClass("active")
        }
        
    });
    if (window.location.pathname == "/") {
        $("#homeFeedNotif").addClass("active")
    }

    //****************************Friendship**************************\\




    AjaxCall("form.friendRequest-notif-form",
       function (data) {
           $thisForm.parents(".fr-Notif").remove();
           if (parseInt($("#FR").text(), 10) - 1 > 0) {
               $("#FR").text(parseInt($("#FR").text(), 10) - 1)
           }
           else { $("#FR").hide() }
           //$ThisLoadingElement.remove();
           modalApear();
           modalMessage(data.Message);
       });



    //****************************Notifications**************************\\
    $(".notif-popover").popover({
        trigger: 'click',
        html: true,
        container: '.navbar-header',
        content: function () {
            $this = $(this);
            if ($this.find(".loaded").is(':empty')) {
                "<div id='loading'></div>"
                $.ajax({
                    url: $this.data("url"),
                    type: "GET",
                    cache: false,
                    success: function (data) {
                        $this.find(".loaded").html(data);
                        $this.popover("show");
                    }
                });
                return "<div id='loading'></div>";
            }
            else {
                return $this.find(".loaded").html();
            }
        }
    });

    $('body').on('click', function (e) {
        $('[data-toggle="popover"]').each(function () {
            //the 'is' for buttons that trigger popups
            //the 'has' for icons within a button that triggers a popup
            if (!$(this).is(e.target) && !$(this).is(".sch-pr-avatar") && $(this).has(e.target).length === 0 && $('.popover').has(e.target).length === 0) {
                $(this).popover('hide');
            }
        });
    });

    
    $(document).delegate(".notif", "click", function () {
        var URL = $(this).data("read")
        $.ajax({
            url: URL,
            type: "POST",
            cache: false,
            data: {},
            success: function (data) {
                //$(data).hide().appendTo($("#allNotif")).slideDown();
                //$this.append(data)
            }
        });
        window.location.replace($(this).data("link"))
    });

    
    $(document).delegate("a.markAllRead", "click", function (e) {
        $.ajax({
            url: ROOT + "Notification/AllRead",
            type: "POST",
            cache: false,
            data: {},
            success: function (data) {
                if(data.Success == true){
                    $("div.notif").addClass("read_notif");
                    $("#NF").hide();
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                var errorText = $.parseJSON(XMLHttpRequest.responseText).errorMessage;
                errorApear(errorText);

            }
        });
    });
})


//function notifMenuScrollSize(containerSelector,elemSelector,count) {
//    $thisNF = $(containerSelector);
//    $thisNF.append('<div class="scrollbar"><div class="track"><div class="thumb"><div class="end"></div></div></div></div>');

//    if ($thisNF.find(elemSelector).length > count) {
//        var outerHeight = 0;
//        $(elemSelector).each(function (index, item) {
//            if (index < count) {
//                outerHeight += $(this).outerHeight();
//            }
//        });
//        var height = outerHeight;
//        //+ parseInt($thisNF.css("padding-top"))
//        //+ parseInt($thisNF.css("padding-bottom"));
//        $thisNF.find(".viewport").css("height", height + "px");
//    }
//    else {
//        var outerHeight = 0;
//        $(elemSelector).each(function () {
//            outerHeight += $(this).outerHeight();
//        });
//        var height = outerHeight
//                     + parseInt($thisNF.css("padding-top"))
//                     + parseInt($thisNF.css("padding-bottom"));
//        $thisNF.find(".viewport").css("height", height + "px");
//    }
//        $thisNF.tinyscrollbar();
    
//}