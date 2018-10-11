using System.Web;
using System.Web.Mvc;

namespace IndustryTower.Helpers
{
    public static class AjaxPrtialHelper
    {
        public static MvcHtmlString AjaxPrtialLoader(this HtmlHelper helper, string url, bool isCache = false)
        {
            var context = new UrlHelper(HttpContext.Current.Request.RequestContext);
            var tagBuilder = new TagBuilder("div");
            tagBuilder.Attributes["id"] = "partialContents";
            tagBuilder.Attributes["data-url"] = url;
            tagBuilder.Attributes["cache"] = isCache.ToString();
            tagBuilder.InnerHtml = "<div id='loadingHor'></div>";
            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.Normal));
        }
    }
}