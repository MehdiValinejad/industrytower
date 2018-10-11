$(document).ready(function () {

    //**********************Create Patent**********************\\
    ModalAjaxCall("a.create-Patent",
        function (data) {
            
            CallDatePicker();
            $('input, textarea').placeholder();
        },
        function (data) {
            $("#UPPatent .panel-body").append(data.Result);
            modalMessage(data.Message);
        },
        null)

    //**********************Edit Patent**********************\\
    ModalAjaxCall("a.edit-Patent",
        function (data) {
            
            CallDatePicker(); 
            $('input, textarea').placeholder();
        },
        function (data) {
            $thisLink.parents("div#patItem").replaceWith(data.Result);
            modalMessage(data.Message);
        },
        null)

    //**********************Delete Patent**********************\\
    ModalAjaxCall("a.delete-Patent",
        function (data) {
            
        },
        function (data) {
            $thisLink.parents("div#patItem").remove();
            modalMessage(data.Message);
        },
        null)
   
});