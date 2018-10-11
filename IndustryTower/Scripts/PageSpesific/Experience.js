$(document).ready(function () {

    //**********************Create Experience**********************\\
    ModalAjaxCall("a.create-Experience",
        function (data) {
            //modalApear();
            countryLoad();
            CallDatePicker();
            $('input, textarea').placeholder();
        },
        function (data) { 
            $("#experienceContainer .panel-body").append(data.Result);
            modalMessage(data.Message);
        },
        null)

    //**********************Create Experience**********************\\
    ModalAjaxCall("a.edit-Experience",
        function (data) {
            //modalApear();
            countryLoad();
            CallDatePicker();
            $('input, textarea').placeholder();
        },
        function (data) {
            $thisLink.parents("div#expItem").replaceWith(data.Result);
            modalMessage(data.Message);
        },
        null)

    //**********************Delete Experience**********************\\
    ModalAjaxCall("a.delete-Experience",
        function (data) {
            //modalApear(data);
        },
        function (data) {
            $thisLink.parents('div#expItem').remove();
            modalMessage(data.Message);
        },
        null)


    //**********************Search Company Loader Experience**********************\\
    var ROOT = $("#ROOT").val();

    $(document).delegate("div#experienceCreate .search-mini-item, div#experienceCreate .search-nano-item", "click", function (e) {
        $ThisParent = $(this).parents("#experienceCreate");
        var dataId = $(this).data("id");
        var text = $(this).find(".us-text").data("name");
        $ThisParent.find(".company-exp").after("<div id='expCo' class='col-md-12 margin-top-bot-md'><button id='expCoClose' type='button' class='close'><span aria-hidden='true'>&times;</span><span class='sr-only'>Close</span></button><div class='inline-block margin-lf-r8-md'>" + text + "<input type='hidden' id='CoId' name='CoId' value='" + dataId + "' /></div></div>");
        $ThisParent.find(".company-exp").hide();
    });

    $(document).delegate("#expCoClose", "click", function () {
        $ThisParent = $(this).parents("div#experienceCreate");
        $(this).parents("div#expCo").remove();
        $ThisParent.find(".company-exp").show();
    });
})