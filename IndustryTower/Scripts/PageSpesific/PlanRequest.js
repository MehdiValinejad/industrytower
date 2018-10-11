$(document).ready(function () {

    //**********************Create PlanReuest**********************\\
    AjaxCall("#planreuqest form",
       function (data) {
           location.replace(data.URL)
       },
       null)
    //**********************Create PlanReuest**********************\\
    AjaxCall("#RevivalForm form",
       function (data) {
           location.replace(data.URL)
       }, 
       null)
    //**********************PayForPlan**********************\\
    AjaxCall("#bankPay form",
       function (data) {
           location.replace(data.URL)
       },
       null)
    AjaxCall("#cashPay form",
       function (data) {
           location.replace(data.URL)
       },
       null)

 
});

