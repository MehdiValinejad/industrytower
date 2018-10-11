using System.Web.Optimization;

namespace IndustryTower
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {

            //BundleTable.EnableOptimizations = true;
            bundles.UseCdn = true;


            //var jquerybundle = new ScriptBundle("~/bundles/jquery", "//ajax.googleapis.com/ajax/libs/jquery/2.1.0/jquery.min.js")
            //{
            //    CdnFallbackExpression = "window.jquery"
            //};
            //jquerybundle.Include("~/scripts/jquery-{version}.js");
            //bundles.Add(jquerybundle);

            var jqueryOldbundle = new ScriptBundle("~/bundles/jqueryOld", "//ajax.googleapis.com/ajax/libs/jquery/1.11.0/jquery.min.js")
            {
                CdnFallbackExpression = "window.jQuery"
            };
            jqueryOldbundle.Include("~/scripts/JQueryold/jquery-1.11.0.min.js");
            bundles.Add(jqueryOldbundle);

            //var jqueryUIbundle = new ScriptBundle("~/bundles/jqueryui", "//ajax.googleapis.com/ajax/libs/jqueryui/1.10.4/jquery-ui.min.js")
            //{
            //    CdnFallbackExpression = "window.jquery"
            //};
            //jqueryUIbundle.Include("~/Scripts/jquery-ui-{version}.js");
            //bundles.Add(jqueryUIbundle);

            //var jqueryMobilebundle = new ScriptBundle("~/bundles/jqueryui", "//ajax.googleapis.com/ajax/libs/jquerymobile/1.4.2/jquery.mobile.min.js")
            //{
            //    CdnFallbackExpression = "window.jquery"
            //};
            //jqueryMobilebundle.Include("~/Scripts/jquery-ui-{version}.jds");//////////////////////////////ATTENTION
            //bundles.Add(jqueryMobilebundle);





            bundles.Add(new ScriptBundle("~/bundles/Total").Include(
                        "~/Scripts/GoogleAnalytic.js",
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/AJAX.js",
                        "~/Scripts/PartialLoader.js",
                        "~/Scripts/jquery.placeholder.min.js",
                        "~/Scripts/Modal.js",
                        "~/Scripts/Loading.js",
                        "~/Scripts/jquery.expander.min.js",
                        "~/Scripts/Search.js",
                        "~/Scripts/ScrollDetector.js",
                        "~/Scripts/Notification.js",
                        "~/Scripts/Cookie.js"));

            bundles.Add(new ScriptBundle("~/bundles/Upload").Include(
                        "~/Scripts/FileUpload/vendor/jquery-ui-widget.js",
                        "~/Scripts/FileUpload/jquery.fileupload.js",
                        "~/Scripts/FileUpload/jquery.iframe-transport.js",
                        "~/Scripts/Upload.js"));

            bundles.Add(new ScriptBundle("~/bundles/Social").Include(
                        "~/Scripts/PageSpesific/Post.js",
                        "~/Scripts/PageSpesific/Comment.js",
                        "~/Scripts/PageSpesific/Like.js",
                        "~/Scripts/PageSpesific/Share.js"));

            bundles.Add(new ScriptBundle("~/bundles/Crop").Include(
                        "~/Scripts/jquery-cropit.min.js"));

            //bundles.Add(new ScriptBundle("~/bundles/Notification").Include(
            //            "~/Scripts/Notification*"));

            bundles.Add(new ScriptBundle("~/bundles/DatePicker").Include(
                        "~/Scripts/Datepicker/bootstrap-datepicker.js",
                        "~/Scripts/Datepicker/pDatepicker.min.js",
                        "~/Scripts/Datepicker/DPMgr.js"));

            bundles.Add(new ScriptBundle("~/bundles/TimePicker").Include(
                        "~/Scripts/Datepicker/jquery-clockpicker.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/Profession").Include(
                        "~/Scripts/PageSpesific/ProfessionTags.js"));

            bundles.Add(new ScriptBundle("~/bundles/Category").Include(
                        "~/Scripts/PageSpesific/Category.js"));

            bundles.Add(new ScriptBundle("~/bundles/CountryDropDown").Include(
                        "~/Scripts/CountryDropDown.js"));

            bundles.Add(new ScriptBundle("~/bundles/UserCompanyStore").Include(
                        "~/Scripts/UserCompanyStore.js"));

            bundles.Add(new ScriptBundle("~/bundles/SlideShow").Include(
                        "~/Scripts/SlideShow.js"));

            bundles.Add(new ScriptBundle("~/bundles/TabChanger").Include(
                        "~/Scripts/TabChanger.js"));

            bundles.Add(new ScriptBundle("~/bundles/Following").Include(
                        "~/Scripts/PageSpesific/Following.js"));

            bundles.Add(new ScriptBundle("~/bundles/FriendRequest").Include(
                        "~/Scripts/PageSpesific/FriendRequest.js"));

            bundles.Add(new ScriptBundle("~/bundles/UserProfile").Include(
                        "~/Scripts/PageSpesific/UserProfile.js",
                        "~/Scripts/PageSpesific/Certificate.js",
                        "~/Scripts/PageSpesific/Education.js",
                        "~/Scripts/PageSpesific/Experience.js",
                        "~/Scripts/PageSpesific/Patent.js"));

            bundles.Add(new ScriptBundle("~/bundles/Company").Include(
                        "~/Scripts/PageSpesific/Company.js"));

            bundles.Add(new ScriptBundle("~/bundles/Product").Include(
                       "~/Scripts/PageSpesific/Product.js"));

            bundles.Add(new ScriptBundle("~/bundles/Service").Include(
                       "~/Scripts/PageSpesific/Service.js"));

            bundles.Add(new ScriptBundle("~/bundles/Company").Include(
                       "~/Scripts/PageSpesific/Company.js"));

            bundles.Add(new ScriptBundle("~/bundles/Store").Include(
                        "~/Scripts/PageSpesific/Store.js"));

            bundles.Add(new ScriptBundle("~/bundles/UserTags").Include(
                       "~/Scripts/PageSpesific/UserTags.js"));

            bundles.Add(new ScriptBundle("~/bundles/Question").Include(
                        "~/Scripts/PageSpesific/Question.js"));

            bundles.Add(new ScriptBundle("~/bundles/Answer").Include(
                        "~/Scripts/PageSpesific/Answer.js"));

            bundles.Add(new ScriptBundle("~/bundles/Event").Include(
                        "~/Scripts/PageSpesific/Event.js"));

            bundles.Add(new ScriptBundle("~/bundles/Score").Include(
                        "~/Scripts/PageSpesific/Score.js"));

            bundles.Add(new ScriptBundle("~/bundles/Badge").Include(
                        "~/Scripts/PageSpesific/Badge.js"));

            bundles.Add(new ScriptBundle("~/bundles/Book").Include(
                        "~/Scripts/PageSpesific/Book.js"));

            //bundles.Add(new ScriptBundle("~/bundles/Certificate").Include(
            //            "~/Scripts/PageSpesific/Certificate.js"));

            //bundles.Add(new ScriptBundle("~/bundles/Education").Include(
            //            "~/Scripts/PageSpesific/Education.js"));

            //bundles.Add(new ScriptBundle("~/bundles/Experience").Include(
            //            "~/Scripts/PageSpesific/Experience.js"));

            //bundles.Add(new ScriptBundle("~/bundles/Patent").Include(
            //            "~/Scripts/PageSpesific/Patent.js"));

            bundles.Add(new ScriptBundle("~/bundles/PlanRequest").Include(
                        "~/Scripts/PageSpesific/PlanRequest.js"));

            bundles.Add(new ScriptBundle("~/bundles/Group").Include(
                        "~/Scripts/PageSpesific/Group.js"));

            bundles.Add(new ScriptBundle("~/bundles/Dict").Include(
                        "~/Scripts/PageSpesific/Dict.js"));

            bundles.Add(new ScriptBundle("~/bundles/Seminar").Include(
                        "~/Scripts/PageSpesific/Seminar.js"));


            ///////////////////////////////////Webinar\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
            bundles.Add(new ScriptBundle("~/bundles/Webinar").Include(
                        "~/Scripts/Webinar/Webinar.js"));

            bundles.Add(new ScriptBundle("~/bundles/Moderator").Include(
                        "~/Scripts/Webinar/Webinar.js",
                        "~/Scripts/Webinar/lzstring133.min.js",
                        "~/Scripts/Webinar/ITRTC.js",
                        //"~/Scripts/Webinar/RTCMulticonnection.js",
                        //"~/Scripts/Webinar/RecordRTC.js",
                        "~/Scripts/jquery.signalR-{version}.js",
                        "~/Scripts/Webinar/Moderator.js"));

            bundles.Add(new ScriptBundle("~/bundles/Broadcaster").Include(
                        "~/Scripts/Webinar/Webinar.js",
                        "~/Scripts/Webinar/lzstring133.min.js",
                        "~/Scripts/Webinar/ITRTC.js",
                        //"~/Scripts/Webinar/RTCMulticonnection.js",
                        "~/Scripts/jquery.signalR-{version}.js",
                        "~/Scripts/Webinar/Broadcaster.js"));

            bundles.Add(new ScriptBundle("~/bundles/Audience").Include(
                        "~/Scripts/Webinar/Webinar.js",
                        "~/Scripts/Webinar/lzstring133.min.js",
                        "~/Scripts/Webinar/ITRTC.js",
                        //"~/Scripts/Webinar/RTCMulticonnection.js",
                        "~/Scripts/jquery.signalR-{version}.js",
                        "~/Scripts/Webinar/Audience.js"));



            bundles.Add(new ScriptBundle("~/bundles/Workshop").Include(
                        "~/Scripts/Webinar/Workshop.js"));

            bundles.Add(new ScriptBundle("~/bundles/WSModerator").Include(
                        "~/Scripts/Workshop/Workshop.js",
                        "~/Scripts/Webinar/FileBufferReader.js",
                        "~/Scripts/Webinar/lzstring133.min.js",
                        //"~/Scripts/Workshop/ITRTC.js",
                        "~/Scripts/Workshop/RTCMulticonnection.js",
                        //"~/Scripts/Webinar/RecordRTC.js",
                        "~/Scripts/jquery.signalR-{version}.js",
                        "~/Scripts/Workshop/Moderator.js"));

            bundles.Add(new ScriptBundle("~/bundles/Attendee").Include(
                        "~/Scripts/Workshop/Workshop.js",
                        "~/Scripts/Webinar/FileBufferReader.js",
                        "~/Scripts/Webinar/lzstring133.min.js",
                        //"~/Scripts/Workshop/ITRTC.js",
                        "~/Scripts/Workshop/RTCMulticonnection.js",
                        "~/Scripts/jquery.signalR-{version}.js",
                        "~/Scripts/Workshop/Attendee.js"));




            

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            
            
            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap-theme.css",
                "~/Content/CSS.css",
                "~/Content/Sprite.css",
                "~/Content/Datepicker/bootstrap-datepicker.css",
                "~/Content/icn.css",
                //"~/Content/themes/base/jquery.ui.tooltip.css",
                //"~/Content/themes/base/jquery.ui.datepicker(Custom)*",
                "~/Content/themes/base/jquery-clockpicker.min.css"
                ));
            bundles.Add(new StyleBundle("~/Content/css-rtl").Include(
                "~/Content/bootstrap-theme.css",
                "~/Content/CSS.css",
                "~/Content/rtl-CSS.css",
                "~/Content/Datepicker/pDpicker.css",
                "~/Content/Sprite.css",
                "~/Content/icn.css",
                //"~/Content/themes/base/jquery.ui.tooltip.css",
                //"~/Content/themes/base/jquery.ui.datepicker(Custom)*",
                "~/Content/themes/base/jquery-clockpicker.min.css"
                ));

            bundles.Add(new StyleBundle("~/Content/Webinar").Include(
                "~/Content/Webinar.css"
                ));

            //bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
            //            //"~/Content/themes/base/jquery.ui.core.css",
            //            //"~/Content/themes/base/jquery.ui.resizable.css",
            //            //"~/Content/themes/base/jquery.ui.selectable.css",
            //            //"~/Content/themes/base/jquery.ui.accordion.css",
            //            //"~/Content/themes/base/jquery.ui.autocomplete.css",
            //            //"~/Content/themes/base/jquery.ui.button.css",
            //            //"~/Content/themes/base/jquery.ui.dialog.css",
            //            //"~/Content/themes/base/jquery.ui.slider.css",
            //            //"~/Content/themes/base/jquery.ui.tabs.css",
            //            //"~/Content/themes/base/jquery.ui.datepicker.css",
            //            //"~/Content/themes/base/jquery.ui.progressbar.css",
            //            //"~/Content/themes/base/jquery.ui.theme.css"
            //            "~/Content/themes/base/jquery.ui.tooltip.css",
            //            "~/Content/themes/base/jquery.ui.datepicker(Custom)*"
            //            ));

            //bundles.Add(new StyleBundle("~/Content/crop/css").Include("~/Content/Crop/cropper*"));
        }
    }
}