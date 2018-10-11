function AjaxInterval(elem) {
    $ThisContainer = $("div#tabChangerContainer");
    //function firstRun() {
    //    $ThisContainer.find("div.tab-content:first").effect("slide", { direction: 'left', mode: 'show' }).addClass("activeTab");
    //    $ThisContainer.find("div.tab:first").addClass("activeTabCaption");
    //}

    var autoSlide = function () {
        $ThisActiveCaption = $ThisContainer.find("div.activeTabCaption")
        if ($ThisActiveCaption.length === 0 || $ThisActiveCaption.is(":last-child")) {
            $ThisActiveCaption.removeClass("activeTabCaption");
            $ThisContainer.find("div.activeTab").effect("slide", { direction: 'right', mode: 'hide' }).removeClass("activeTab");
            firstRun();
        }
        else {
            $ThisActiveCaption.removeClass("activeTabCaption");
            $ThisActiveCaption.next().addClass("activeTabCaption");
            $ThisContainer.find("div.activeTab").effect("slide", { direction: 'right', mode: 'hide' }).removeClass("activeTab");
            $ThisContainer.find("div.activeTab").next().effect("slide", { direction: 'right', mode: 'hide' }).removeClass("activeTab");
        }
    }



    firstRun();
    var timer = setInterval(autoSlide, 3000);


    $ThisContainer.hover(function () {
        clearInterval(timer);
    }, function () {
        timer = setInterval(autoSlide, 3000);
    });
}