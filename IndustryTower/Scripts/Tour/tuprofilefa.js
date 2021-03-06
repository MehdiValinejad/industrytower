﻿(function () {
    $(function () {

        var $demo, duration, remaining, tour;
        //$demo = $(".glyphicon-question-sign");
        duration = 5000;
        remaining = duration;
        tour = new Tour({
            template: "<div class='popover tour'><div class='arrow'></div><h3 class='popover-title'></h3><div class='popover-content'></div><div class='popover-navigation'><button class='btn btn-default btn-xs' data-role='prev'>« قبلی</button><span data-role='separator'>|</span><button class='btn btn-default btn-xs' data-role='next'>بعدی »</button><button class='btn btn-default btn-xs' data-role='end'>پایان</button></div></nav></div>",
            //onStart: function () {
            //    return $demo.addClass("disabled", true);
            //},
            //onEnd: function () {
            //    return $demo.removeClass("disabled", true);
            //},
            //debug: true,
            steps: [
              {
                  placement: "bottom",
                  title: "به گاه نمای خود خوش آمدید!",
                  content: "این صفحه گاه نمای شما نام دارد، سوابق کاری، تحصیلات، اختراعات و پست های شما در اینجا قرار می گیرد.",
                  orphan: true
              }, {
                  element: ".changeprofilepic-link",
                  placement: "bottom",
                  title: "ویرایش عکس",
                  content: "با کلیک بر روی علامت مداد در تمامی قسمت های سایت، می توانید آن قسمت را ویرایش نمائید. با کلیک بر روی این علامت مداد، می توانید عکس خود را آپلود نمائید.",
              }, {
                  element: ".editUserInfo-link",
                  placement: "bottom",
                  title: "ویرایش اطلاعات کاربری و تخصص ها",
                  content: "در این قسمت می توانید اطلاعات کاربری خود را ویرایش نمائید، همچنین می توانید تخصص های خود را از لیست تخصص ها انتخاب نمائید، در صورتی که این تخصص های خود را انتخاب ننمائید با دیگر متخصصین ارتباط نمی یابید."
              }, {
                  element: "#userinfo",
                  placement: "bottom",
                  title: "اطلاعات فعالیت در وبسایت",
                  content: "با کلیک بر روی این دکمه به صفحه اطلاعات فعالیت، از جمله امتیازات، چرخ دنده های طلایی، نقره ای و برنز و دیگر اطلاعات مرتبط با فعالیت شما در سایت منتقل می شوید. این بخش یکی از مهمترین بخش ها برای در معرض دید قرار دادن تخصص های واقعی شما به بازدید کنندگان می باشد."
              }, {
                  element: ".create-Experience",
                  placement: "bottom",
                  title: "تجربه کاری",
                  content: "تجربیات کاری شما، اعم از شغل ها و مسئولیت هایی که تا کنون داشته اید، در این بخش قرار می گیرد. توجه داشته باشید که برای بهبود جستجو حتما تجربیات کاری خود را در موارد جدا با کلیک بر روی علامت مثبت وارد نمائید."
              }, {
                  element: ".create-Education",
                  placement: "bottom",
                  title: "مدارج علمی و تحصیلات",
                  content: "تصحیلات و مدارج علمی شما در این بخش قرار می گیرد، این بخش را نیز به صورت مجزا وارد نمائید."
              }, {
                  element: ".create-Certificate",
                  placement: "bottom",
                  title: "گواهی نامه های شما",
                  content: "گواهی نامه های شما در این بخش قرار می گیرد، مانند گواهی نامه های ISO و PMBOK و...، این موارد را نیز به صورت مجزا وارد نمائید."
              }, {
                  element: ".create-Patent",
                  placement: "bottom",
                  title: "حق ثبت اختراع (Patent)",
                  content: "اختراعات شما در این بخش قرار می گیرد، این بخش ها را نیز مجزا وارد نمائید. "
              }, {
                  element: ".create-Event",
                  placement: "bottom",
                  title: "رویداد های شما",
                  content: "رویداد هایی که شما از طریق صفحه اصلی هریک از رویداد های درج شده از طرف شما یا دیگر اعضا اعلام شرکت نموده اید در اینجا مشخص است."
              }, {
                  element: "#UPQuestions",
                  placement: "top",
                  title: "سوالات مرتبط با شما",
                  content: "بنا بر تخصص های که برای خود بر می گزینید، با امکانات مختلف سایت در ارتباط هستید، سوالاتی که مرتبط با تخصص های شما هستند در این بخش قرار می گیرد.",
                  backdrop: true
              }, {
                  element: "#postsContainer",
                  placement: "top",
                  title: "پست های گاه نما",
                  content: "پست هایی که شما روی گاه نمای خود می گذارید و دیگر پست هایی که دوستان یا شرکت ها یاواحد های تجاری آنها روی گاه نمای شما می گذارند در این قسمت قابل نمایش است.",
                  backdrop: true
              }
            ]
        }).init();
        //if (tour.ended()) {
        //    $('<div class="alert alert-info alert-dismissable"><button class="close" data-dismiss="alert" aria-hidden="true">&times;</button>You ended the demo tour. <a href="#" data-demo>Restart the demo tour.</a></div>').prependTo(".content").alert();
        //}
        $(document).ready(function () {
            tour.restart();
        });

    });

}).call(this);
