function GategoryTags(MaxTagsChoose,errorMaxChoose,errorhasParent) {
    //$(document).delegate('li#childCat', 'mouseenter', function () {
    //    $This = $(this);
    //    var url = $This.data("url");
    //    if (url && url.length > 0) {
    //        $.ajax({
    //            url: url,
    //            type: "GET",
    //            success: function (data) {
    //                $This.append(data);
    //            }
    //        }) 
    //    }
    //    $(this).mouseleave(function () {
    //        $(this).children('ul').empty();
    //    });
    //});
    
    //PopulateCatTags();
    

    $(document).delegate('li#childCat', 'mouseenter', function () {
        $This = $(this); 
        
        $This.find('ul').addClass("CatColoractive");
        $This.children('ul').slideDown('fast');

        var parents_array = $This.parents("li#childCat").map(function () {
            return $(this).data("value");
        });
        var childs_array = $This.find("li#childCat").map(function () {
            return $(this).data("value");
        });
        var selected_array = $("div#SelectedCategories .catTags").map(function () {
            return $(this).data("value");
        });
        var ParentsAre = Array();
        var childrenAre = Array();
        jQuery.grep(selected_array, function (el) {
            if (jQuery.inArray(el, parents_array) != -1) ParentsAre.push(el);
        });
        jQuery.grep(selected_array, function (el) {
            if (jQuery.inArray(el, childs_array) != -1) childrenAre.push(el);
        });
        

        if (ParentsAre.length != 0) {
            $(this).parent().addClass("hasParent");
            $(this).parents(':eq(1)').find('li').addClass("hasParent");
            //$(this).addClass("hasParent");
            $(this).find('ul').addClass("hasParent");
        }
        else {
            $This.addClass("CatColoractive");
        }

        $(this).mouseleave(function () {
            $(this).children('ul').slideUp('fast');
            $(this).removeClass("CatColoractive");
            $(this).children('ul').removeClass("CatColoractive");
            
        });
    });

    $(document).delegate('li#childCat span', 'click', function () {
        if ($("div#SelectedCategories .catTags").length >=  parseInt(MaxTagsChoose))
        {
            modalApear();
            modalError(errorMaxChoose)
            return;
        }
        var parents_array = $(this).parents("li#childCat").map(function () {
            return $(this).data("value");
        });
        var childs_array = $(this).parent().find("li#childCat").map(function () {
            return $(this).data("value");
        });
        var selected_array = $("div#SelectedCategories .catTags").map(function () {
            return $(this).data("value");
        });
        var ParentsAre = Array();
        var childrenAre = Array();
        jQuery.grep(selected_array, function (el) {
            if (jQuery.inArray(el, parents_array) != -1) ParentsAre.push(el);
        });
        jQuery.grep(selected_array, function (el) {
            if (jQuery.inArray(el, childs_array) != -1) childrenAre.push(el);
        });

        if (ParentsAre.length === 0) {
            $this = $(this);
            if (childrenAre.length === 0) {
                var id = $this.parent('li').data("value");
                var content = $this.text();
                $("div#SelectedCategories .panel-body").append("<div class='catTags green-tag inline-block selectedtag-marg' data-value ='" + id + "'>" + content + "<button type='button' class='close'><span aria-hidden='true'>×</span><span class='sr-only'>Close</span></button></div>");
            }
            else {
                $.each(childrenAre, function (index, item) {
                    $("div#SelectedCategories .panel-body").children('div[data-value=' + item + ']').remove();
                    var id = $this.parent('li').data("value");
                    var content = $this.text();
                    $("div#SelectedCategories").append("<div class='catTags green-tag inline-block selectedtag-marg' data-value ='" + id + "'>" + content + "<button type='button' class='close'><span aria-hidden='true'>×</span><span class='sr-only'>Close</span></button></div>");
                })
            }
        }
        else {
            modalApear();
            modalError(errorhasParent)
        }
        CatBeforeSubmit();

    });
    $(document).delegate('div.catTags button', 'click', function () {
        $(".hasParent").removeClass("hasParent");
        $(this).parent().remove();
        CatBeforeSubmit();
    });
}

function CatBeforeSubmit() {
    var arrayToPass = [];
    var Cat_array = $("div#SelectedCategories .catTags").map(function () {
        arrayToPass.push($(this).data("value"))
        //return $(this).data("value");
    });
    var finalArray = arrayToPass.join(",");
    $("#Cats").val(finalArray);
}

function PopulateCatTags() {
    var categories = $("#Cats").val();
    if (categories != null) {
        var cts = {};
        cts = categories.split(",");
        $.each(cts, function (index, item) {
            if (item.length > 0) {
                var content = $("ul#childsContainer li[data-value='" + item + "']").children('span').text();
                $("div#SelectedCategories .panel-body").append("<div class='catTags green-tag inline-block selectedtag-marg' data-value ='" + item + "'>" + content + "<button type='button' class='close'><span aria-hidden='true'>×</span><span class='sr-only'>Close</span></button></div>");
            }
        });
    }
}

function CatsColor() {
    $(document).delegate("span.admin-cat", "click", function () {
        $(this).parent().parent().find("> ul.categories-container").toggle();
        var randomColor = Math.floor(Math.random() * 16777215).toString(16);
        $(this).parent().parent().find("> ul.categories-container").css("background-color", "#" + randomColor)
    })
    $("ul.categories-container ul").each(function (inxed, item) {
        var randomColor = Math.floor(Math.random() * 16777215).toString(16);
        $(item).css("background-color", "#" + randomColor)
    });
}