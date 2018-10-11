$(document).ready(function () {


    ModalAjaxCall("a.sharepost-link",
        function (data) {
            //modalApear(data);
        },
        function (data) {
            messageApear(data.Message);
        },
        null)
     
    //**********************Edit Share**********************\\
    ModalAjaxCall("a.edit-Share",
        null,
        function (data) {
            modalApear();
            modalMessage(data.Message);
            $thisLink.parents("#postDesc").find("#shareN").html(data.Note)
        },
        null)


    //**********************Delete Share**********************\\
    ModalAjaxCall("a.delete-Share",
        null,
        function (data) {
            modalApear();
            modalMessage(data.Message);
            $thisLink.parents("#postDesc").remove();
        },
        null)

});