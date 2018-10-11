using System.Web;
using System.Web.Mvc;

namespace IndustryTower.Helpers
{
    public static class AbstractTextHelper
    {
        public static IHtmlString  DisplayAbstractFor(this HtmlHelper html, string text, int characters = 300)
        {
            if(text.Length > characters)
            {
                return html.Raw(text.Substring(0, characters));
            }
            else return html.Raw(text);
        }
    }
}