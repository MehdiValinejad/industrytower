using System.Web;
using System.Web.Mvc;

namespace IndustryTower.Helpers
{
    public static class ModifyIconsHelper
    {
        public static MvcHtmlString ModifyIcons(this HtmlHelper helper, string editAction, string deletAction, string controller, object routValues ,
                                                string editClass = "", string deleteClass = "", bool editAjax = true, bool deleteAjax = true, bool deletevis = true, bool editvis = true)
        {
            var context = new UrlHelper(HttpContext.Current.Request.RequestContext);
            var editLinkTag = new TagBuilder("a");
            var editImgTag = new TagBuilder("span");
            var deletLinkTag = new TagBuilder("a");
            var deletImgTag = new TagBuilder("span");

            if (editvis)
            {
                editLinkTag.Attributes["href"] = context.Action(editAction, controller, routValues);
                editLinkTag.AddCssClass("inline-block " + editClass + " edit-" + controller);
                editLinkTag.Attributes["data-ajax"] = editAjax.ToString().ToLower();
                editLinkTag.Attributes["title"] = Resource.Resource.edit;
                editLinkTag.Attributes["data-placement"] = "bottom";
                //editImgTag.Attributes["src"] = context.Content("~/Images/Pencil.png");
                //editImgTag.AddCssClass("deleteEditIMG");
                //helper.Sprite("I_Pencil", new { @class = "deleteEditIMG" });
                editImgTag.AddCssClass("glyphicon glyphicon-pencil ");
                editLinkTag.InnerHtml = editImgTag.ToString();//helper.Sprite("I_Pencil", new { @class = "deleteEditIMG" }).ToString();//editImgTag.ToString(TagRenderMode.SelfClosing);
            }

            if (deletevis)
            {
                deletLinkTag.Attributes["href"] = context.Action(deletAction, controller, routValues);
                deletLinkTag.AddCssClass("inline-block " + deleteClass + " delete-" + controller);
                deletLinkTag.Attributes["data-ajax"] = deleteAjax.ToString().ToLower();
                deletLinkTag.Attributes["title"] = Resource.Resource.delete;
                deletLinkTag.Attributes["data-placement"] = "bottom";
                //deletImgTag.Attributes["src"] = context.Content("~/Images/RecycleBin.png");
                //deletImgTag.AddCssClass("deleteEditIMG");

                deletImgTag.AddCssClass("glyphicon glyphicon-trash");
                deletLinkTag.InnerHtml = deletImgTag.ToString();//helper.Sprite("I_RecycleBin", new { @class = "deleteEditIMG" }).ToString();//deletImgTag.ToString(TagRenderMode.SelfClosing);
            }
            

            return MvcHtmlString.Create(editLinkTag.ToString(TagRenderMode.Normal) + deletLinkTag.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString CreateIcon(this HtmlHelper helper, string action, string controller, object routValues = null, string Class = "",bool dataAjax = true)
        {
            var CreateLinkTag = new TagBuilder("a");
            var CreateImgTag = new TagBuilder("span");
            var context = new UrlHelper(HttpContext.Current.Request.RequestContext);

            CreateLinkTag.Attributes["href"] = context.Action(action, controller, routValues);
            CreateLinkTag.AddCssClass("float-left " + Class + " create-" + controller);
            CreateLinkTag.Attributes["data-ajax"] = dataAjax.ToString().ToLower();
            CreateLinkTag.Attributes["title"] = Resource.Resource.create;
            CreateLinkTag.Attributes["data-placement"] = "bottom";
            //CreateImgTag.Attributes["src"] = context.Content("~/Images/Add.png");
            //CreateImgTag.AddCssClass("deleteEditIMG");
            CreateImgTag.AddCssClass("glyphicon glyphicon-plus");
            CreateLinkTag.InnerHtml = CreateImgTag.ToString();//helper.Sprite("I_Add", new { @class = "deleteEditIMG" }).ToString();//CreateImgTag.ToString(TagRenderMode.SelfClosing);

            return MvcHtmlString.Create(CreateLinkTag.ToString(TagRenderMode.Normal));

        }
    }
}