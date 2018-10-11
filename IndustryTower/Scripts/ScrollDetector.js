$(document).ready(function () {
    /////////////////SMooth Scrolling\\\\\\\\\\\\\\\\
    //$(function () {

    //    var top = 0,
    //        step = 55,
    //        viewport = $(window).height(),
    //        body = /webkit/.test(navigator.userAgent.toLowerCase()) ? $('body') : $('html'),
    //        wheel = false;


    //    $('body').bind("mousewheel", function (event) {
    //        wheel = true;

    //        if (event.originalEvent.wheelDelta < 0) { 
    //            //scroll down
    //            top = (top + viewport) >= $(document).height() ? top : top += step;

    //            body.stop().animate({ scrollTop: top }, 400, function () {
    //                wheel = false;
    //            });
    //        } else {
    //            //scroll up
    //            top = top <= 0 ? 0 : top -= step;

    //            body.stop().animate({ scrollTop: top }, 400, function () {
    //                wheel = false;
    //            });
    //        }

    //    });

    //    $(window).on('resize', function (e) {
    //        viewport = $(this).height();
    //    });

    //    $(window).on('scroll', function (e) {
    //        if (!wheel)
    //            top = $(this).scrollTop();
    //    });

    //});


    /////////////////header Scrolling\\\\\\\\\\\\\\\\
    $("textarea").each(function () {
        if (this.scrollHeight > 5) {
            this.style.height = (this.scrollHeight - 5) + 'px';
        }
    });

    var didScroll;
    var lastScrollTop = 0;
    var delta = 5;
    var navbarHeight = $('div#header').outerHeight();

    $(window).scroll(function (event) {
        didScroll = true;
    });

    setInterval(function () {
        if (didScroll) {
            hasScrolled();
            didScroll = false;
        }
    }, 250);

    function hasScrolled() {
        var st = $(this).scrollTop();

        // Make sure they scroll more than delta
        if (Math.abs(lastScrollTop - st) <= delta)
            return;

        // If they scrolled down and are past the navbar, add class .nav-up.
        // This is necessary so you never see what is "behind" the navbar.
        if (st > lastScrollTop && st > navbarHeight) {
            // Scroll Down
            $('div#header').animate({ "top": "-90px" })
            //$('div#header').removeClass('nav-down').addClass('nav-up');
        } else {
            // Scroll Up
            if (st + $(window).height() < $(document).height()) {
                $('div#header').animate({ "top": "0px" })
                //$('div#header').removeClass('nav-up').addClass('nav-down');
            }
        }

        lastScrollTop = st;
    }

});


function AutoScroll(elemselector, distance) {
    $(window).scroll(function () {
        if ($(elemselector).length > 0 && $(elemselector).find("#loading").length == 0 && $(".mainsearchLoading").length == 0) { //$(".mainsearchLoading").length == 0) {
            var target = $(elemselector).offset().top - distance
            if ($(window).scrollTop() >= target) {
                $(elemselector).trigger("click");
            }
        }
    });
}