$(document).ready(function () {

    $(document).delegate(".voteup,.votedn", "click", function (e) {
        $(this).find("form").submit();
    });
    //****************************LIKE**************************\\
    AjaxCall(".voteup form, .votedn form",
       function (data) {
           var res = parseInt(data.Result);
           if (res == 0) {
               $thisForm.parent().popover({
                   trigger: 'manual',
                   content: $thisForm.parent().data("ctn")
               });
               $thisForm.parent().popover("show");
               $thisForm.parents(".votebtn").find("div#loading").remove();
           }
           else if (res == -1000) {
               modalApear();
               modalError($thisForm.parent().data("lowscore"));
               $thisForm.parents(".votebtn").find("div#loading").remove();
           }
           else {
               var fin = res + parseInt($thisForm.parents(".vote").find(".voteconter").text());
               $thisForm.parents(".vote").find(".voteconter").text(fin);
               $thisForm.parents(".votebtn").find("div#loading").remove();
           }
       },
       null,
       function () {
           loadingBox($thisForm.parents(".votebtn"));
           $ThisLoadingElement = $thisForm.parents(".votebtn").find("div#loading"); //overridesDefault
       });

    $('body').on('click', function (e) {
        $('[data-ctn]').each(function () {
            //the 'is' for buttons that trigger popups
            //the 'has' for icons within a button that triggers a popup
            if (!$(this).is(e.target) && $(this).has(e.target).length === 0 && $('.popover').has(e.target).length === 0) {
                $(this).popover('destroy');
            }
        });
    });
});