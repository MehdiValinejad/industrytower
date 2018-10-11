$(document).ready(function () {
    //**********************Create Answer**********************\\
    AjaxCall("form.newanswer-form",
       function (data) {
           //$ThisLoadingElement.remove();
           $('div#questionAnswers').append(data.Result);
           $submitbut.button("reset");
       },
       null)

    //**********************Edit Answer**********************\\
    AjaxGetCall("a.edit-Answer",
       function (data) {
           $thisLink.parents('#answerDetail').html(data); 
           $textarea = $("form.editAnswer-form textarea")
           $textarea.height($textarea[0].scrollHeight);
       })
    AjaxCall("form.editAnswer-form",
       function (data) {
           $thisForm.parents('div#answerDetail').replaceWith(data.Result);
       },
       null)

    //**********************Delete Answer**********************\\
    ModalAjaxCall("a.delete-Answer",
        null,
        function (data) {
            $thisLink.parents('div#answerBodyDisplay').remove();
            //messageApear(data.Message);
        },
        null)


    //**********************Accept Answer**********************\\

    $(document).delegate("button#ansacc", "click", function () {
        $(this).parents("form.accept-nswer").submit();
    })
    AjaxCall("form.accept-nswer",
       function (data) {
           $thisContainer = $thisForm.parents(".panel")
           $accButton = $thisForm.find("button#ansacc");

           if (data.Result) {
               //$thisNotMarked.effect("drop", {}, 500, function () {
               //    $thisMarked.effect("slide", {}, 500, function () { });
               //});
               $accButton.find("i").text($accButton.data("marked"));
               $accButton.removeClass("btn-default");
               $accButton.addClass("btn-success");

               $thisContainer.removeClass("notacc-ans-box")
               $thisContainer.addClass("acc-ans-box")
           }
           else {
               //$thisMarked.effect("drop", {}, 500, function () {
               //    $thisNotMarked.effect("slide", {}, 500, function () { });
               //});
               $accButton.find("i").text($accButton.data("notmarked"));
               $accButton.removeClass("btn-success");
               $accButton.addClass("btn-default");
               $thisContainer.removeClass("acc-ans-box")
               $thisContainer.addClass("notacc-ans-box")

           }
       },
       null)
});
