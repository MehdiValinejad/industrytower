$(document).ready(function () {
    //*************************search Form Icon Initial *****************************\\
    var Current = $("input#searchType").val();
    var relevant = $(".search-type-choose").find("div[data-search='" + Current + "']");
    $("input#SearchString").attr("placeholder", relevant.data("ph"));
    relevant.parents("li:eq(0)").addClass("search-active-class");
    var content  =  relevant.clone();
    $(".active-search-img div").replaceWith(content.addClass("active-search-img"));

    $("div.search-menu a").each(function (index, item) {
        var linkpram = $(item).data("searchtype").toLowerCase();
        var urlparam = $("input#searchType").val().toLowerCase();
        //var linkpram = getUrlVar("searchType", $(item).attr("href")).toLowerCase();
        //var urlparam = getUrlVar("searchType").toLowerCase();
        if ((linkpram !== "" && linkpram === urlparam) || (window.location.pathname === "/" && linkpram === "")) {
            $(item).parents("li").addClass("active");
            $(".search-create[data-searchtype='" + $(item).data("searchtype") + "']").show();
        }
    });

    //*************************Menu Initial Item*****************************\\
    $(".top-menu div").each(function (index, item) {
        //var linkpram = getUrlVar("searchType",$(item).find("a").attr("href")).toLowerCase();
        //var urlparam = getUrlVar("searchType").toLowerCase();
        var linkpram = $(item).data("searchtype").toLowerCase();
        var urlparam = $("input#searchType").val().toLowerCase();
        if ((linkpram !== "" && linkpram === urlparam) || (window.location.pathname === "/" && linkpram === "")) {
            //$(item).css("background", "#21a1b9")
            if (linkpram === "all" && linkpram === urlparam && window.location.pathname.indexOf("TotalSearch") === -1) {
                return false;
            }
            $(item).addClass("active");
        }
    });

    $(".active-search").click(function () {
        $(".search-type-choose").toggle();
    });
    
    $(".search-option").click(function () {
        $(".search-top li.active").removeClass("active");
        //$(".search-active-class").removeClass("search-active-class");
        $(this).parent("li").addClass("active");
        var tt = $(this).find(".IMGDIV").clone();
        $(".active-search-img div").replaceWith(tt.addClass("active-search-img"));
        var searchType = $(this).find(".IMGDIV").data("search");
        $("input#SearchString").attr("placeholder", $(this).find(".IMGDIV").data("ph"));
        $("input#searchType").val(searchType);
        //$(".search-type-choose").hide();
    });

    //**********************Search Submit**********************\\
    $("input.total-search-box").keypress(function (event) {
        if (event.which === 13) {
            event.preventDefault();
            $(this).parents(".main-search").find("form").submit();
        }
    });

    $(".navbar-form").submit(function (e) {
        var stype = $("#searchType").val();
        if (stype == "ALL") {
            var curr = $(this).find(".total-search-box").val();
            $(this).find(".total-search-box").css("box-shadow", " 0 0 5px red");
            if (curr.trim().length == 0) e.preventDefault();
        }
        
    });

    $(document).delegate(".navbar-fixed-top .search-mini .row", "click", function () {
        location.replace($(this).data("link"));
    });

    //$(".search-btn").click(function () {
    //    $(this).parents("div.mainSearchBoxContainer").find("form").submit();
    //});

    //**********************Search ContentLoader**********************\\
    //var currentFilterInitial = $("input#currentFilter").val();
    //$("input#SearchString").val(currentFilterInitial);
    $this = $("#searchPartialContent");
    $parent = $this.parent();
    var url = $this.data("url");
    if (url && url.length > 0) {
        $.ajax({
            url: url,
            cache: false,
            type: "GET",
            dataType: 'json',
            success: function (data) {
                $this.replaceWith(data.Result);
                $(".search-result-count span").html(data.ResultCount);
                $(".search-menu").append(data.SearchPanel);
                $('input, textarea').placeholder();
                if (findBootstrapEnvironment() === "xs") {
                    $(".search-menu").remove().clone().appendTo(".navbar-collapse");
                }
            },
            complete: function () {
                $parent.find(".expandable").expander();
                PartialContentLoader($parent);
            }
        });
    }


    //***********************Quick Search************************\\
    $(document).delegate(".has-mini-search", "keyup", function () {
        $thisSearchBox = $(this);
        var searchValue = $thisSearchBox.val();
        var width = $thisSearchBox.outerWidth();
        var cls = '';
        
        if ($.trim(searchValue) !== '') {
            var URL = $thisSearchBox.data("minisearch");
            $(".search-mini").remove();
            var styl = " style='width:" + width + "px' "
            var cls = "";
            if ($thisSearchBox.hasClass("main-search")) {
                $thisSearchBox = $(".navbar-fixed-top .container");
                styl = '';
                cls = "col-md-10 col-md-offset-1"
            } 

            if (searchValue.length > 2) {
                $thisSearchBox.after("<div class='search-mini panel panel-default " + cls + "' " + styl + "><div id='loading'></div></div>");
                $.ajax({
                    url: URL,
                    cache: false,
                    type: "GET",
                    data: { searchString: searchValue },
                    success: function (Result) {
                        $thisSearchBox.next(".search-mini").html(Result);
                        //$(".search-mini").remove();
                        //$thisSearchBox.after("<div class='search-mini panel panel-default' style='width:" + width + "px'>" + Result + "</div>")
                    }
                });
            }
            
            $(document).click(function (e) {
                if (!$(e.target).is(".search-mini")) {
                    $(".search-mini").remove();
                }
            });
        }
        else $(".search-mini").remove();
    });

    $(document).delegate("div.search-mini-item a, div.search-nano-item a", "click", function (e) {
        e.preventDefault();
    });
    

    //**********************Sort Item Click*********************\\
    $(document).delegate("div.sort-item", "click", function () {
        var sortParm = $(this).data("order");
        $("input#sortOrder").val(sortParm);
        submitSearch(1);
    });

    //**********************Criteria Item Click*********************\\
    $(document).delegate("div.criteria-item", "click", function (e) {
        if ($(e.target).is("input[type='checkbox']"))
        {
            submitSearch(1);
            $("html, body").animate({ scrollTop: 0 }, "slow");
            return;
        }
        $ThisCheckbox = $(this).find("input[type='checkbox']");
        if (!$ThisCheckbox.is(":checked")) {
            $ThisCheckbox.prop("checked", true);
        } else {
            $ThisCheckbox.prop("checked", false);
        }
        submitSearch(1);
        $("html, body").animate({ scrollTop: 0 }, "slow");
    });

    $(document).delegate(".search-menu-criteria div.search-mini-item, .search-menu-criteria div.search-nano-item", "click", function () {
        var dataId = $(this).data("id");
        $parentBox = $(this).parents("div.criteria-box");
        $thistextbox = $parentBox.find("input[type='text']");
        var newcheckboxName = $thistextbox.attr("checkboxName");

        $thistextbox.before("<input type='checkbox' name='" + newcheckboxName + "' id='" + newcheckboxName + "' value='" + dataId + "' style='display:none' checked='checked' />");
        submitSearch(1);
    });


    //**********************Next Results**********************\\
    $(document).delegate("div#nextSearchResults", "click", function () {
        $thisNext = $(this);
        //var pos = $thisNext.offset().top;
        var nextpage = $(this).data("nextpage");
        submitSearch(nextpage, function (data) {
            $thisNext.replaceWith(data.Result);
            //$('html, body').animate({
            //    scrollTop: pos
            //},2000)
        });
    });

    AutoScroll("div#nextSearchResults", 1000);

    //**********************Search Navigation Product**********************\\
    //$(document).delegate(".search-product-pic", "mouseenter", function () {
    //    $this = $(this);
    //    $this.parents(".search-grid-product").find(".curren-product-icon").removeClass("curren-product-icon");
    //    $this.addClass("curren-product-icon");
    //    $thisCont = $(this).parents(".search-grid-product").next(".search-grid-desc");
    //    $thisCont.fadeOut("fast", function () {
    //        var content = $this.next(".search-product-desc").html();
    //        $thisCont.html(content).fadeIn();
    //        $thisCont.fadeIn();
    //    });
    //});


    //$(document).delegate(".sch-pr-avatar a", "click", function (e) {
    //    e.preventDefault();

    //});

    //var popOverSettings = {
    //    placement: 'bottom',
    //    container: 'body',
    //    html: true,
    //    selector: '.sch-pr-avatar', //Sepcify the selector here
    //    content: function () {
    //        var html = "<div class=' dir'><div class='pr-img-popover'>" + $(this).html() + "</div>" + $(this).find(".desc").html() + "</div>";
    //        return html;
    //    }
    //};

    //$('body').popover(popOverSettings);
});


//function getUrlVar(key,url) {
//    var result = new RegExp(key + "=([^&]*)", "i").exec(url !== null ? url : window.location.search);
//    return result && unescape(result[1]) || "";
//}

function submitSearch(pageNumber, successFunc) {
    var ROOT = $("#ROOT").val();

    var searchType = $("input#searchType").val();
    var currentFilter = $("input#currentFilter").val();
    var sortOrder = $("input#sortOrder").val();
    stateArray = [];
    categoryArray = [];
    professionArray = [];
    coSizeArray = [];
    expCosArray = [];

    $("input[type='checkbox']#searchPanelState:checked").each(function (index, item) {
        stateArray.push($(item).val());
    });
    $("input[type='checkbox']#searchPanelCats:checked").each(function (index, item) {
        categoryArray.push($(item).val());
    });
    $("input[type='checkbox']#searchPanelProf:checked").each(function (index, item) {
        professionArray.push($(item).val());
    });
    $("input[type='checkbox']#searchPanelCoSize:checked").each(function (index, item) {
        coSizeArray.push($(item).val());
    });
    $("input[type='checkbox']#searchPanelCos:checked").each(function (index, item) {
        expCosArray.push($(item).val());
    });

    loadingBox($("html"),"mainsearchLoading");
    $.ajax({
        url: ROOT + "Home/TotalSearch",
        cache: false,
        type: "GET",
        dataType: 'json',
        traditional: true,
        data: {
            searchType: searchType,
            currentFilter: currentFilter,
            sortOrder: sortOrder,
            page: pageNumber,
            searchStates: stateArray,
            searchCats: categoryArray,
            searchProfessions: professionArray,
            searchCoSize: coSizeArray,
            searchExpCo: expCosArray
        },
        success: function (data) {
            $(".mainsearchLoading").fadeOut("slow", function () { $(this).remove(); });
            if (successFunc != null) {
                successFunc(data);
            } else {
                $(".search-results").html(data.Result);
                $(".search-result-count span").html(data.ResultCount);
                $(".search-menu-criteria").replaceWith(data.SearchPanel);
                $("input,textarea").placeholder();
            }
        },
        complete: function () {
            $parent.find(".expandable").expander();
            PartialContentLoader($parent);
            $("div.sort-item").each(function () {
                var content = $(this).data("order").toLowerCase();
                if (sortOrder != "") {
                    if (content.match("^" + sortOrder.toLowerCase()) || sortOrder.match("^" + content.toLowerCase())) {
                        $(this).addClass("active-sort");
                    }
                }
            });
        }
    });
}

