using System.Web.Mvc;

namespace IndustryTower.Helpers
{
    public static class SpriteHelper
    {
        public static MvcHtmlString Sprite(this HtmlHelper urlHelper, string ImageName, object htmlAttributes = null, string InnerHtml = null)
        {
            TagBuilder divTag = new TagBuilder("div");

            var htmlattrs = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            divTag.MergeAttributes(htmlattrs);
            divTag.AddCssClass(ImageName);
            divTag.AddCssClass("IMGDIV");
            divTag.InnerHtml = InnerHtml;
            return MvcHtmlString.Create(divTag.ToString(TagRenderMode.Normal));
        }
    }
}