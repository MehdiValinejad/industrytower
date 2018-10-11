$(document).ready(function () {

    //**********************Next Comments**********************\\
    $("body").delegate("a.next-comments-link", 'click', function () {
        $thisNextLink = $(this);
        loadingBox($thisNextLink);
        $.ajax({
            url: $thisNextLink.attr("href"),
            cache: false,
            type: "GET",
            success: function (data) { 
                var thisParent = $thisNextLink.parent('li#nextComments').parent()
                $thisNextLink.parent('li#nextComments').replaceWith(data);
                PartialContentLoader(thisParent);
            }
        })
    })

    //**********************New Comment**********************\\
    AjaxCall("form.new-comment-form",
           function (data) {
               $thisForm.find("textarea").val("");
               $thisForm.parent().prev('ul#newComment').append(data.Result);
               $thisForm.parent().prev('ul#newComment').find('.comment-label').show();
               $submitbut.button("reset");
           },
           null)


    //**********************Delete Comment**********************\\
    ModalAjaxCall("a.delete-Comment",
        null,
        function (data) {
            $thisLink.parents('li#comment').remove();
            messageApear(data.Message);
        },
        null)
});