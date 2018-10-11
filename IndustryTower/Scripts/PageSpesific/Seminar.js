$(document).ready(function () {

    AjaxCall("#semCreateEdit form",
       function (data) {
           window.location.replace(data.Url);
       },
       null, 
       function () {
           //overridesDefault
       });

    $(document).delegate("input#fileInsert", "keyup", function () {
        if ($(this).next("input").length == 0) {
            $(this).after("<input type='text' id='fileInsert' class='form-control margin-top-bot-xs' name='fileInsert' placeholder=" + $(this).attr("placeholder") + " />")
        }
    });

    //AjaxGetCall("#seminarAudiences",
    //function (data) {
    //    modalApear(data)
    //});

    AjaxCall("#SeminarAudienceEdit form",
       function (data) {
           window.location.replace(data.Url)
       },
       null,
       function () {
           //overridesDefault
       });
   
});