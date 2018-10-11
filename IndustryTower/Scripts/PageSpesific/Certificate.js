$(document).ready(function () {

    //**********************Create Certificate**********************\\
    ModalAjaxCall("a.create-Certificate",
        function (data) {
            CallDatePicker();
            $('input, textarea').placeholder();
        }, 
        function (data) {
            $("#UPCertificate .panel-body").append(data.Result);
            modalMessage(data.Message);
        },
        null)
    //**********************Edit Certificate**********************\\
    ModalAjaxCall("a.edit-Certificate",
        function (data) {
            CallDatePicker();
            $('input, textarea').placeholder();
        },
        function (data) {
            $thisLink.parents("div#certificateItem").replaceWith(data.Result);
            modalMessage(data.Message);
        },
        null)

    //**********************Delete Certificate**********************\\
    ModalAjaxCall("a.delete-Certificate",
        function (data) {

        },
        function (data) {
            $thisLink.parents('div#certificateItem').remove();
            modalMessage(data.Message);
        },
        null)

});