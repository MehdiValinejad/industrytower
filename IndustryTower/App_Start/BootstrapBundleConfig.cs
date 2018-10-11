using System.Web.Optimization;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(IndustryTower.App_Start.BootstrapBundleConfig), "RegisterBundles")]

namespace IndustryTower.App_Start
{
	public class BootstrapBundleConfig
	{
		public static void RegisterBundles()
		{
			// Add @Styles.Render("~/Content/bootstrap") in the <head/> of your _Layout.cshtml view
			// For Bootstrap theme add @Styles.Render("~/Content/bootstrap-theme") in the <head/> of your _Layout.cshtml view
			// Add @Scripts.Render("~/bundles/bootstrap") after jQuery in your _Layout.cshtml view
			// When <compilation debug="true" />, MVC4 will render the full readable version. When set to <compilation debug="false" />, the minified version will be rendered automatically



            var bootstrapjsbundle = new ScriptBundle("~/bundles/bootstrap", "//netdna.bootstrapcdn.com/bootstrap/3.2.0/js/bootstrap.min.js")
            {
                CdnFallbackExpression = "$.fn.modal"
            };
            bootstrapjsbundle.Include("~/Scripts/bootstrap.js");
            BundleTable.Bundles.Add(bootstrapjsbundle);


            BundleTable.Bundles.Add(new StyleBundle("~/Content/bootstrap",
                "//netdna.bootstrapcdn.com/bootstrap/3.2.0/css/bootstrap.min.css")
                .IncludeFallback("~/Content/bootstrap.min.css", "sr-only", "width", "1px"));


            //BundleTable.Bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap.js"));
            //BundleTable.Bundles.Add(new StyleBundle("~/Content/bootstrap").Include("~/Content/bootstrap.css"));
            //BundleTable.Bundles.Add(new StyleBundle("~/Content/bootstrap-theme").Include("~/Content/bootstrap-theme.css"));
		}
	}
}
