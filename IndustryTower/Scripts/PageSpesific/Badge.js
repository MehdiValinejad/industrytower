
$(document).ready(function () {

    AjaxCall("#BadgeUserChoose form",
       function (data) {
           window.location.replace(data.Url)
       },
       null,
       function () {
           //overridesDefault
       });

});

