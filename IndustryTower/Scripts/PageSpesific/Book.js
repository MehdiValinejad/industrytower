$(document).ready(function () {
    //**********************Create Book**********************\\
    AjaxCall("div#bookCreate form",
       function (data) {
           location.replace(data.URL)
       },
       null);

    AjaxCall("div#BookReviewCreate form",
       function (data) {
           location.replace(data.URL)
       },
       null);

    $("#loadChooseUser").click(function () {
        $(this).parent().find(".display-none").slideDown();
        $(this).slideUp();
    });
});