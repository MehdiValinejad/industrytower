
function SlideShow() {
    $("div#SlideShow:not([data-ld='true'])").each(function (index, item) {
        $this = $(item).find("ul#Slides li:first");
        var url = $this.data("url");
        if (url !== "no") {
            $this.fadeIn().addClass("active").html("<img class='center-image' src='" + url + "' />");
        } else $this.fadeIn().addClass("active");
        $nav = $this.parents('div#SlideShow').find('ol#slideNavigator')
        $(item).find("ul#Slides li").each(function () {
            $nav.append("<li id='navItem' class='inline-block'></li>")
        })
        $nav.find('li:first').addClass("active");
        //(item).attr("class", "sld-loaded");
        $(item).attr("data-ld","true")
    });
} 
function SlideShowArrowKeys() {
    $(document).delegate("a#arrowleft", 'click', function () {
        $thisActive = $(this).parent().find("ul#Slides li.active");
        $thisNav = $(this).parent().find("ol#slideNavigator");
        $previous = $thisActive.prev('li');
        if ($previous.length > 0) {
            var ImgUrl = $previous.data("url");
            if (!(ImgUrl === "no") && $previous.children('img').length === 0) {
                $previous.html("<img class='center-image' src=" + ImgUrl + " />")
            }
            var inx = $previous.index();
            var previnx = inx + 1;
            $thisNav.find('li:eq(' + inx + ')').addClass("active", 1000, "easeInOutExpo");
            $previous.fadeIn().addClass("active");
            $thisNav.find('li:eq(' + previnx + ')').removeClass("active");
            $thisActive.fadeOut().removeClass("active");
        } else {
            $par = $thisActive.parents("#SlideShow");
            $thisActive.fadeOut().removeClass("active");
            $thisNav.find("li.active").removeClass("active");
            $par.find("ul#Slides li:last").fadeIn().addClass("active");
            $thisNav.find("li:last").addClass("active");
        }
    });
    $(document).delegate("a#arrowright", 'click', function () {
        $thisActive = $(this).parent().find("ul#Slides li.active");
        $thisNav = $(this).parent().find("ol#slideNavigator");
        $next = $thisActive.next('li');

        if ($next.length > 0) {
            var ImgUrl = $next.data("url");
            if (!(ImgUrl === "no") && $next.children('img').length === 0) {
                $next.html("<img class='center-image' src=" + ImgUrl + " />")
            }
            var inx = $next.index();
            var nextinx = inx - 1;
            $thisNav.find('li:eq(' + inx + ')').addClass("active", 1000, "easeInOutExpo");
            $next.fadeIn().addClass("active")
            $thisNav.find('li:eq(' + nextinx + ')').removeClass("active");
            $thisActive.fadeOut().removeClass("active");
        } else {
            $par = $thisActive.parents("#SlideShow");
            $thisActive.fadeOut().removeClass("active");
            $thisNav.find("li.active").removeClass("active");
            $par.find("ul#Slides li:first").fadeIn().addClass("active");
            $thisNav.find("li:first").addClass("active");
        }
    });
}
    
$(document).ready(function () {
    setInterval(function () {
        $(".proserv-sld").each(function (ind, item) {
            if (!$(item).is(":hover")) {
                $(item).find("#arrowright").trigger("click");
            }
        })
        
    },5000)
});

