$(document).ready(function () {

    $(document).delegate("#likeButton", "click", function (e) {
        $(this).find("form").submit();
        
    });
    //****************************LIKE**************************\\
    AjaxCall("form.like-form",
       function (data) {
           $thisForm.parents("li#likeContainer").replaceWith(data.Result);
       },
       null,
       function () { 
           loadingBox($thisForm.parents("li#likeContainer"));
           $ThisLoadingElement = $thisForm.parents("div#likeContainer").find("div#loading"); //overridesDefault
       })

    AjaxGetCall("#likeCount a",
        function (data) {
            modalApear(data)

        })

});