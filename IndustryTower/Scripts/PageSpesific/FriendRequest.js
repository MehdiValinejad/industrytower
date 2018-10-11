$(document).ready(function () {

    //****************************Friendship**************************\\
    AjaxCall("form.friendRequest-form",
       function (data) {
           $('div#friendRequest').replaceWith(data.Result);
           //$ThisLoadingElement.remove();
           //messageApear(data.Message);
           modalApear();
           modalMessage(data.Message);
       })

    
    //$("body").delegate("#addFriendBottun", "click", function () {
    //    $("#addFriendContainer").fadeToggle();
    //    $(this).find('input, textarea').placeholder();
    //});
});