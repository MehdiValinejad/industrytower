$(document).ready(function () {
    $(document).delegate("#FollowButton", "click", function (e) {
        $(this).find("form").submit();

    });
    //**********************Add Following**********************\\
    AjaxCall("form.follow-form",
       function (data) {
           $thisForm.parents("li#followContainer").replaceWith(data.Result);
       },
       null,
       function () { 
           loadingBox($thisForm.parents("div#followContainer"));
           $ThisLoadingElement = $thisForm.parents("div#followContainer").find("div#loading"); //overridesDefault
       })

    AjaxGetCall("#FollowCount a",
        function (data) {
            messageApear(data)
        })
  
});