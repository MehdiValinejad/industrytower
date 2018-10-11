function TabChanger(parent, attribute, time) {
    function firstRun(pparent) {
        pparent.find("div.tab-content:first").addClass("activeTab").fadeIn(function () {
            pparent.find("div.tab:first").addClass("activeTabCaption").css("opacity", "1");
        });
        //pparent.find("div.tab-content:first").addClass("activeTab").effect("slide", { direction: 'left', mode: 'show' }, 300, function () {
        //    pparent.find("div.tab:first").addClass("activeTabCaption").css("opacity","1");
        //});
    }
    firstRun(parent);

    function autoSlide(PPP) {

        $ThisActiveCaption = PPP.find("div.activeTabCaption")
        if ($ThisActiveCaption.length === 0)
        {
            firstRun(PPP); 
        }
        else if ($ThisActiveCaption.is(":last-child")) {
            $ThisActiveCaption.removeClass("activeTabCaption").css("opacity", "0.5");
            PPP.find("div.activeTab").removeClass("activeTab").fadeOut(function () {
                firstRun(PPP);
            })
            //PPP.find("div.activeTab").removeClass("activeTab").effect("slide", { direction: 'right', mode: 'hide' }, 300, function () {
            //    firstRun(PPP);
            //});
        }
        else {
            $ThisActiveCaption.removeClass("activeTabCaption").css("opacity", "0.5");
            $ThisActiveCaption.next().addClass("activeTabCaption").css("opacity", "1");
            $thisactive = PPP.find("div.activeTab");
            $nextactive = $thisactive.next();
            $thisactive.removeClass("activeTab").fadeOut(function () {
                $nextactive.fadeIn().addClass("activeTab");
            })
            //$thisactive.removeClass("activeTab").effect("slide", { direction: 'right', mode: 'hide' }, 300, function () {
            //    $nextactive.effect("slide", { direction: 'left', mode: 'show' }, 300).addClass("activeTab");
            //});
        }
    }

    if (parent.find(".tab").length > 1)
    {
        var unidades = {};
        unidades[attribute] = setInterval(function () {
            autoSlide(parent)
        }, time);

        parent.hover(function () {
            clearInterval(unidades[attribute]);
        }, function () {
            unidades[attribute] = setInterval(function () {
                autoSlide(parent)
            }, time);
        });
    }
    

    $(parent).delegate('div.tab', 'click', function (e) {
        if ($(e.target).is('a, img')){
            e.preventDefault();
        }
        $This = $(this);
        var index = $This.index();
        $ThisContainer = $This.parents("div#tabChangerContainer");

        $ThisTab = $ThisContainer.find("div.tab-content:eq(" + index + ")");
        if (!$ThisTab.hasClass("activeTab")) {
            $ThisContainer.find(".activeTabCaption").removeClass("activeTabCaption").css("opacity", "0.5");
            $ThisContainer.find("div.activeTab").removeClass("activeTab").fadeOut(function () {
                $ThisTab.fadeIn().addClass("activeTab");
            })
            //$ThisContainer.find("div.activeTab").removeClass("activeTab").effect("slide", { direction: 'right', mode: 'hide' }, 300, function () {
            //    $ThisTab.effect("slide", { direction: 'left', mode: 'show' }).addClass("activeTab");
            //});
            $This.addClass("activeTabCaption").css("opacity", "1");
        }
    });


};