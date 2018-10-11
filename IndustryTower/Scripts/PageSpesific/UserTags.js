$(document).ready(function () {

    populateChooseUsers();

    
     
    $(document).delegate(".hasUserChoose div.search-mini-item, .hasUserChoose div.search-nano-item", "click", function (e) {

        $parentBox = $(this).parents("div.hasUserChoose");
        var currentID = $(this).data("id");
        var currentText = $(this).find(".us-text").data("name");
        var inilial = [];
        $.each($parentBox.find("#choosedUsers .us-txt"), function (index, item) {
            inilial.push($(item).data("name"))
        });
        if ($.inArray(currentText, inilial) > -1) return;

        var content = "<button type='button' class='close pull-right'><span aria-hidden='true'>&times;</span><span class='sr-only'>Close</span></button>" + $(this).html();
        $parentBox.find("#choosedUsers").append("<li class='btn btn-default' data-id='" + currentID + "'>" + content + "</li>");
        var currentArray = []
        $.each($parentBox.find("#choosedUsers li"), function (index, item) {
            currentArray.push($(item).data("id"));
        });
        if ($parentBox.find("input#UserTags").length > 0) {
            $parentBox.find("input#UserTags").val(currentArray.join(","));
        } else {
            if (currentArray.length === 1) {
                $parentBox.find("input#UserTag").val(currentArray.join(","));
            }
        }
        
    });

    $(document).delegate("#choosedUsers button", "click", function () {
        $parentBox = $(this).parents("ul#choosedUsers");
        $(this).parent("li").remove();
        var finalVal = []
        $.each($parentBox.find("li"), function (index, item) {
            finalVal.push($(item).data("id"));
        });
        //$parentBox.find("input#UserTags").val(finalVal.join(","));
        if ($parentBox.find("input#UserTags").length > 0) {
            $parentBox.find("input#UserTags").val(finalVal.join(","));
        } else {
            if (finalVal.length == 0) {
                $parentBox.find("input#UserTag").val(finalVal.join(","));
            }
        }
    });
});

function populateChooseUsers() {
    $this = $("ul#choosedUsers");
    var init = []
    $.each($this.find("li"), function (index, item) {
        init.push($(item).data("id"));
    });

        $this.find("input#UserTags").val(init.join(","));
}