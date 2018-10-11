/// <reference path="JQueryold/jquery-1.11.0.min.js" />

function PartialContentLoader(parentElement) {
    $all = $(parentElement).find("div#partialContents").each(function (i, item) {
        var isCache = false;
        if ($(item).attr("cache").toString().toLowerCase() == "true") {
            isCache = true
        }
        setTimeout(function () {
            var elemTo = $(item);
            var parent = elemTo.parent();
            var url = elemTo.data("url");
            if (url && url.length > 0) {
                $.ajax({
                    url: url,
                    cache: isCache,
                    success: function (data) { 
                        elemTo.replaceWith(data)
                    },
                    complete: function () {
                        if (typeof SlideShow !== 'undefined' && $.isFunction(SlideShow)) {
                            SlideShow();
                        }
                        $(parent).find('input, textarea').placeholder();
                        $(parent).find(".expandable").expander();
                        $("[title]").tooltip();
                        if (typeof PopulateCatTags !== 'undefined' && $.isFunction(PopulateCatTags) && $("div#SelectedCategories .catTags").length === 0) {
                            PopulateCatTags();
                        }

                        PartialContentLoader(parent);
                    }
                })
            };
        }, 0)
    });
}


//function PartialContentLoader(parentElement) {
//    $(parentElement).find("div#partialContents").each(function (index, item) {
//        var parent = $(item).parent();
//            var url = $(item).data("url");
//            if (url && url.length > 0) {
//                $.ajax({
//                    url: url,
//                    cache: false,
//                    success: function (data) {
//                        $(item).replaceWith(data)
//                    }, 
//                    complete: function () {
//                        if (typeof SlideShow !== 'undefined' && $.isFunction(SlideShow)) {
//                            SlideShow();
//                        }
                        
//                        $(parent).find('input, textarea').placeholder();
//                        $(parent).find(".expandable").expander();
//                        if (typeof PopulateCatTags == 'function') {
//                            PopulateCatTags();
//                        }
                        
//                        PartialContentLoader(parent);
//                    }
//                })
//            };
//        })

//}
