(function () {
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
                    title: "به خبر خوان خود خوش آمدید!",
                    content: "با زدن دکمه های بعدی و قبلی همین کادر می توانید مراحل معرفی را طی نمائید، برای دیدن مرحله اول، بعدی را کلیک نمائید.",
                    orphan:true
              }, {
                  element: ".top-menu",
                  placement: "bottom",
                  title: "دسترسی سریع به جستجو",
                  content: "با کلیک بر روی هریک از موارد این لیست، به صورت سریع به جستجو در تمامی موارد موجود در آن عنوان دسترسی می یابید، به عنوان مثال با کلیک بر روی «سوالات» به صفحه جستجوی تمامی سوالات دسترسی می یابید. ",
                  backdrop :true
              }, {
                  element: "#user-setting .dropdown-toggle",
                  placement: "bottom",
                  title: "منو کاربری",
                  content: "با کلیک بر روی این قسمت می توانید به منو کاربری و تنظیمات حساب خود دسترسی یابید."
              }, {
                  element: "#homeFeedNotif",
                  placement: "bottom",
                  title: "خبرخوان",
                  content: "در هر قسمتی از سایت، می توانید با کلیک بر روی این قسمت به همین صفحه یعنی خبر خوان خود مراجعه نمائید."
              }, {
                  element: "#allNotif a",
                  placement: "bottom",
                  title: "اطلاع رسانی ها (Notifications)",
                  content: "اطلاع رسانی ها یا همان Notifications اوی آخرین اتفاقاتی است که در ارتباط مستقیم باشماست مانند پذیرفته شدن پاسخ های شما به سوالات، درج دیدگاه های دیگر اعضا"
              }, {
                  element: "#addFriendNotif a",
                  placement: "bottom",
                  title: "درخواست های دوستی",
                  content: "در این قسمت، درخواست های دوستی که برای شما ارسال شده است درج می گردد، شما می توانید بپذیرید یا آن را رد نمائید."
              }, {
                  element: "#allGroup a",
                  placement: "bottom",
                  title: "گروه های شما",
                  content: "با کلیک بر روی این قسمت، شما به لیست گروه هایی که در آن عضویت دارید دسترسی می یابید."
              }, {
                  element: "#addEvent a",
                  placement: "bottom",
                  title: "ایجاد رویداد",
                  content: "شما با کلیک بر روی این قسمت، به صفحه ایجاد رویداد منتقل می شوید، در آن صفحه شما امکان ایجاد یک رویداد جدید مانند نمایشگاه ها، جشنواره ها و ... را خواهید داشت."
              }, {
                  element: "#askQuestion a",
                  placement: "bottom",
                  title: "پرسیدن سوال",
                  content: "اگر سوال تخصصی دارید، با کلیک بر روی این قسمت به صفحه ایجاد سوال منتقل می گردید، سوال شما بنا بر تخصص هایی که انتخاب می نمائید، به متخصصین همان تخصص ها نمایش داده می شود."
              }, {
                  element: "#postsContainer",
                  placement: "top",
                  title: "خبر های روزانه",
                  content: "در این قسمت تمامی فعالیت های دوستان، شرکت هایی که با انها در ارتباط هستید به صورت لحظه ای نمایش داده می شود.",
                  backdrop: true
              }, {
                  element: "#newPost",
                  placement: "bottom",
                  title: "پست جدید",
                  content: "شما در این قسمت می توانید بحث های روزانه را مطرح نموده، فایل و تصاویر را برای دوستان خود به اشتراک بگذارید. پست وارد شده توسط شما در گاه نمای شما قابل رویت است.",
                  backdrop: true
              }, {
                  element: "#uploadButton",
                  placement: "bottom",
                  title: "آپلود فایل و عکس",
                  content: "با کلیک بر روی این قسمت می توانیدفایل و عکس های مورد نظر خود را به پست روزانه خود پیوست نمائید."
              }, {
                  element: "#senderCount",
                  placement: "bottom",
                  title: "فرستنده",
                  content: "با کلیک بر روی این قسمت می توانید به شرکت ها و واحد های تجاری که مدیریت آن ها را بر عهده دارید دسترسی یافته و از طرف آنها پست خود را به اشتراک بگذارید."
              }, {
                  element: ".search-type-choose",
                  placement: "bottom",
                  title: "انتخاب نوع جستجو",
                  content: "با کلیک بر روی این قسمت می توانید نوع جستجو را انتخاب نمائید، افراد، گروه ها و ...  و سپس عبارت مورد نظر خود را وارد نموده و روی علامت ذره بین کلیک نمائید. این قسمت نیز در تمامی صفحات سایت قابل دسترس است.  "
              }, {
                  element: "#helpSign",
                  placement: "bottom",
                  title: "دسترسی به گاه نما",
                  content: "با کلیک بر روی این قسمت، به منو راهنما دسترسی می یابید، در هر قسمتی از سایت، اولین مورد این لیست مربوط به راهنمایی همان صفحه است و بقیه موارد مربوط به راهنمایی های کلی و یا دیگر صفحات می باشد.",
              }, {
                  element: ".navbar-pic",
                  placement: "bottom",
                  title: "دسترسی به گاه نما",
                  content: "با کلیک بر روی این قسمت به گاه نمای (Profile) خود دسترسی می یابید. این بخش از راهنمایی پایان یافت، روی پایان کلیک نموده تا به گاه نمای خود منتقل شوید.",
                  onHidden: function () {
                      return window.location.assign("/");
                  }
              }
            ]
        }).init();
        //if (tour.ended()) {
        //    $('<div class="alert alert-info alert-dismissable"><button class="close" data-dismiss="alert" aria-hidden="true">&times;</button>You ended the demo tour. <a href="#" data-demo>Restart the demo tour.</a></div>').prependTo(".content").alert();
        //}
        $(document).ready(function () {
            tour.restart();
        });
        //$(document).on("click", ".glyphicon-question-sign", function (e) {
        //    e.preventDefault();
        //    if ($(this).hasClass("disabled")) {
        //        return;
        //    }
        //    tour.restart();
        //    return $(".alert").alert("close");
        //});
        //$("html").smoothScroll();
    });

}).call(this);
