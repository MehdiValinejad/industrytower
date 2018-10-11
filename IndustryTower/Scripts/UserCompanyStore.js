$(document).ready(function () {
    //$("div.has-choose-CoStore").hover(function () {
    //    $(this).children("div.select-user-comStore:not(:first)").toggle();
    //});
    

    $(document).delegate(".select-user-comStore", "click", function () {
            
        $curr = $(".curr-costor");
        $this = $(this).find("a");
        $par = $(this).parents("#senderCount")
        $par.find("#PrC").val("");
        $par.find("#PrS").val("");
        if ($this.data("sender") == "co") {
            $par.find("#PrC").val($this.data("id"));
        }
        else if ($this.data("sender") == "st") {
            $par.find("#PrS").val($this.data("id"));
        } 
        $this.removeClass("select-user-comStore").addClass("curr-costor dropdown-toggle glyphicon glyphicon-chevron-down inline-block thumbnail").attr({ "data-toggle": "dropdown" }).insertBefore(".user-costre");
        $curr.removeClass("curr-costor dropdown-toggle glyphicon glyphicon-chevron-down inline-block thumbnail").addClass("inline-block thumbnail").removeAttr("data-toggle").appendTo(this);

       
    });

    $("div.has-choose-CoStore a").on("click", function (e) {
        e.preventDefault();
        //$ThisOption = $(this).parent("div.select-user-comStore");
        //if (!$ThisOption.is("div.select-user-comStore:first")) {
        //    $("input[name='PrC']").val("");
        //    $("input[name='PrS']").val("");
        //    $ThisOption.detach().insertBefore("div.select-user-comStore:first");
        //    if ($ThisOption.attr("sender") == "co") {
        //        $("input[name='PrC']").val($ThisOption.data("ajax"));
        //    }
        //    else if ($ThisOption.attr("sender") == "st") {
        //        $("input[name='PrS']").val($ThisOption.data("ajax"));
        //    }
        //}

    });
    $("[title]").tooltip();

});