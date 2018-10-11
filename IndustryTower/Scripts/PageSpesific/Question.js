$(document).ready(function () {
    //**********************Next Questions**********************\\
    $("body").delegate("a.next-questions-link", 'click', function () {
        $thisNextLink = $(this);
        loadingBox($thisNextLink);
        $.get($thisNextLink.attr("href"),
            null, 
            function (data) {
                var thisParent = $thisNextLink.parents('#nextQuestions').parent();
                $thisNextLink.parents('#nextQuestions').replaceWith(data);
                PartialContentLoader(thisParent);
                thisParent.find(".expandable").expander();
                thisParent.find('input, textarea').placeholder();
            });
    });

    //**********************Create Question**********************\\
    AjaxCall("div#questionCreate form",
       function (data) {
           location.replace(data.URL)
       },
       null)

    //**********************Edit Question**********************\\
    AjaxCall("div#questionEdit form",
       function (data) {
           location.replace(data.URL)
       },
       null)

    //**********************Delete Question**********************\\
    ModalAjaxCall("a.delete-Question",
        function (data) {
            modalApear(data);
        },
        function (data) {
            location.replace(data.RedirectURL);
        },
        null)
});



