//**********************Post**********************//
$(document).ready(function () {

    //**********************New Post**********************\\
    AjaxCall("form.newpost-form",
           function (data) {
               $thisForm.find("textarea").val("");
               $thisForm.find("#filesToUpload").val("");
               $thisForm.parents("div#newPost").find("#uploadedFiles .thumbnail").parent("li").remove();
               $ThisLoadingElement.remove();
               if ($('div#postsContainer > div').length > 0) {
                   $('div#postsContainer > div:first').before(data.Result);
               } 
               else {
                   $('div#postsContainer').append(data.Result);
               }
               $thisForm.find("input[type='submit']").prop("disabled", false).removeClass("disabled");
               PartialContentLoader($("div#postContainer:first"));
               SlideShow();
           },
           null,
           function () {
               loadingBox($thisForm.parents("#newPost"));
               $ThisLoadingElement = $thisForm.parents("#newPost").find('div#loading');
           })


    //**********************Edit Post**********************\\
    AjaxGetCall("a.edit-Post",
       function (data) {
           var pos = $thisLink.parents('#postDetails').offset().top - 75;
           $thisLink.parents('#postDetails').replaceWith(data);
           $('html, body').animate({
               scrollTop: pos
           }, 1000)
           $textarea = $("form.editpost-form textarea");
           $textarea.height($textarea[0].scrollHeight + 10);
           CallPostUpload();
       })
    AjaxCall("form.editpost-form",
       function (data) {
           $thisForm.parents("div#postEditorContainer").find("div#loading").remove();
           $thisForm.parents('div#postContainer').replaceWith(data.Result);
           modalApear();
           modalMessage(data.Message);
           SlideShow();
           PartialContentLoader($("div#postContainer"));
       },
       function () {
           loadingBox($thisForm.parents('div#postEditorContainer'));
       })



    //**********************Delete Answer**********************\\
    ModalAjaxCall("a.delete-Post",
        null,
        function (data) {
            $thisLink.parents('div#postContainer').remove();
            modalMessage(data.Message);
        },
        null)


    //**********************Next Post**********************\\
    AjaxGetCall("a.next-posts-link",
        function (data) {
            var thisParent = $thisLink.parent('#nextPosts').parent();
            $thisLink.parent('#nextPosts').replaceWith(data);
            //$('html, body').animate({
            //    scrollTop: $pos
            //}, 2000)
            PartialContentLoader(thisParent);
            thisParent.find(".expandable").expander();
            thisParent.find('input, textarea').placeholder();
        },
        function () {
            //$pos = $thisLink.offset().top;
            loadingBox($thisLink);
        })

    AutoScroll("a.next-posts-link", 1000);




    //$(window).scroll(function () {

        //if ($("a.next-posts-link").length > 0) {
        //    var target = $("a.next-posts-link").offset().top - 1000
        //    if ($(window).scrollTop() >= target) {
        //        $("a.next-posts-link").trigger("click");
        //    }
        //}
    //});


    
});