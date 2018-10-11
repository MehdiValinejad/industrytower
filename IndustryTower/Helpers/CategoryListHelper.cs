using IndustryTower.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace IndustryTower.Helpers
{
    public static class CategoryListHelper
    {
        public static MvcHtmlString CategoryListAdmin(this HtmlHelper helper, Category category)
        {
            UrlHelper Url = new UrlHelper(HttpContext.Current.Request.RequestContext);
            TagBuilder divtag = new TagBuilder("div");

            TagBuilder createChildLink = new TagBuilder("a");

            if (category.parent4ID == null)
            {
                createChildLink.Attributes["href"] = Url.Action("Create", "Category", new { parentId = EncryptionHelper.Protect(category.catID) });
                createChildLink.InnerHtml = Resource.Resource.creatChild;
                createChildLink.AddCssClass("admin-create-child");
            }

            TagBuilder span = new TagBuilder("span");
            span.AddCssClass("admin-cat");
            span.SetInnerText(category.CultureCatName);

            TagBuilder editLink = new TagBuilder("a");
            editLink.Attributes["href"] = Url.Action("Edit", "Category", new { catId = EncryptionHelper.Protect(category.catID) });
            editLink.InnerHtml = Resource.Resource.edit;
            editLink.AddCssClass("admin-edit-child");

            TagBuilder detailsLink = new TagBuilder("a");
            detailsLink.Attributes["href"] = Url.Action("Detail", "Category", new { catId = EncryptionHelper.Protect(category.catID) });
            detailsLink.InnerHtml = Resource.Resource.details;
            detailsLink.AddCssClass("admin-edit-child");


            TagBuilder inputTag =  new TagBuilder("input");
            inputTag.Attributes["type"] = "submit";
            inputTag.Attributes["value"] = Resource.Resource.delete;
            inputTag.Attributes["onclick"] = "return confirm('Are you sure?')";
            inputTag.AddCssClass("delete-cat-button");

            divtag.InnerHtml = String.Concat(span,
                                             category.parent4ID == null ? createChildLink : null,
                                             editLink,
                                             detailsLink);
            return MvcHtmlString.Create(divtag.ToString());
        }
    }
}