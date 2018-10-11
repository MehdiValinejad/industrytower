using System.Web;
using System.Web.Mvc;

namespace IndustryTower.Helpers
{
    public static class NextItemsHelper
    {
        public static MvcHtmlString NextItems(this HtmlHelper helper, string Action, string controller, object routValues, string linkclass = "", string imgclass = "", string caption = null)
        {
            var nextLinkTag = new TagBuilder("a");
            var nextImgTag = new TagBuilder("div");
            var context = new UrlHelper(HttpContext.Current.Request.RequestContext);

            nextLinkTag.Attributes["href"] = context.Action(Action, controller, routValues);
            nextLinkTag.Attributes["title"] = caption != null ? caption : Resource.Resource.nextItems;
            nextLinkTag.AddCssClass("next-items-link btn btn-default btn-xs col-md-12 " + linkclass + " next-" + controller);
            nextLinkTag.Attributes["data-ajax"] = "true";
            //nextImgTag.Attributes["src"] = context.Content("~/Images/More.png");
            nextImgTag.AddCssClass("glyphicon glyphicon-chevron-down");

            nextLinkTag.InnerHtml = nextImgTag.ToString(); //helper.Sprite("I_More", new { @class = imgclass }).ToString(); //nextImgTag.ToString(TagRenderMode.SelfClosing);

            return MvcHtmlString.Create(nextLinkTag.ToString(TagRenderMode.Normal));
        }
    }
}