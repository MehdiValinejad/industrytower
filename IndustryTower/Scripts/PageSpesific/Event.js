$(document).ready(function () {
    //**********************Create Event**********************\\
    try  {
        CallDatePicker();
    }catch(e){ false;}
    
    $('input, textarea').placeholder();
    $(".expandable").expander();


    //**********************Create Edit Event**********************\\
    AjaxCall("#EventCreatEdit form", 
       function (data) {
           location.replace(data.URL)
       },
       null)


    //**********************Delete Event**********************\\
    ModalAjaxCall("a.delete-Event",
        function (data) {
            modalApear(data);
        },
        function (data) {
            messageApear(data.Message);
            window.location.replace(data.RedirectURL);
        },
        null)


    //****************************Attend**************************\\
    AjaxCall("form.attend-form",
       function (data) {
           $thisForm.parents("#attend").replaceWith(data.Result);
       },
       null)

});