$(document).ready(function () {

    //*********************************TagMenu**********************\\
    $(document).delegate("div.tag-options-icon", "mouseenter", function (e) {
        $elem = $(this).parent().next('div')
        $elem.slideDown();

        $(this).mouseleave(function () {
            var hidetimer = setTimeout(function () {
                $elem.slideUp();
            }, 1000)

            $elem.hover(function () {
                clearTimeout(hidetimer);
            })
        })
         
        $elem.mouseleave(function () {
            setTimeout(function () {
                $elem.slideUp();
            }, 1000)
        })
    })
})