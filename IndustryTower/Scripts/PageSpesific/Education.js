$(document).ready(function () {

    //**********************Create Education**********************\\
    ModalAjaxCall("a.create-Education",
        function (data) {
            //modalApear(data);
            CallDatePicker();
            $('input, textarea').placeholder();
        },
        function (data) {
            $("#UPEducation .panel-body").append(data.Result);
            modalMessage(data.Message); 
        },
        null)

    //**********************Create Education**********************\\
    ModalAjaxCall("a.edit-Education",
        function (data) {
            //modalApear(data);
            CallDatePicker();
            $('input, textarea').placeholder();
        },
        function (data) {
            $thisLink.parents("div#eduItem").replaceWith(data.Result);
            modalMessage(data.Message);
        },
        null)

    //**********************Delete Education**********************\\
    ModalAjaxCall("a.delete-Education",
        function (data) {
            //modalApear(data);
        },
        function (data) {
            $thisLink.parents('div#eduItem').remove();
            modalMessage(data.Message);
        },
        null)
});