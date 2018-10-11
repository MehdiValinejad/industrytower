$(document).ready(function () {

    //********************** Edit Company**********************\\
    AjaxCall("#storeEdit form",
       function (data) {
           location.replace(data.URL)
       },
       null)

     

    ModalAjaxCall("a.deleteStLogo-link",
        null,
        function (data) {
            location.reload();
        },
        null)

    ModalAjaxCall("a.changeStLogo-link",
        null,
        function (data) {
            if (data.Success) {
                location.reload();
            }
        },
        null)


});