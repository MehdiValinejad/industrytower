using IndustryTower.Models;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IndustryTower.Helpers
{
    public static class ProfilePicHelper
    {
        public static MvcHtmlString ProfilePic(this HtmlHelper helper, UserProfile user = null, Company company = null, Store store = null, Product product = null, Service service = null, Book book = null, object linkhtmlAttributes = null, object imghtmlAttributes = null)
        {
            var linkTag = new TagBuilder("a");
            var imgTag = new TagBuilder("img");
            var context = new UrlHelper(HttpContext.Current.Request.RequestContext);
            MvcHtmlString final = MvcHtmlString.Empty;
            if (user != null)
            {
                linkTag.Attributes["href"] = context.Action("UProfile", "UserProfile", new { UId = user.UserId, UName = StringHelper.URLName(user.CultureFullName) });
                linkTag.Attributes["title"] = user.CultureFullName;
                if (user.image != null)
                {
                    var pic = user.image;
                    imgTag.Attributes["src"] = context.Content("~/Uploads/" + pic.Substring(pic.LastIndexOf('.') + 1) + "/Profile/" + pic);
                }
                else
                {
                    imgTag.Attributes["src"] = context.Content("~/Images/Icons/People.png");
                }
                imgTag.Attributes["alt"] = user.CultureFullName;
            }
            else if (company != null)
            {
                linkTag.Attributes["href"] = context.Action("CProfile", "Company", new { CoId = company.coID, CoName = StringHelper.URLName(company.CultureCoName) });
                linkTag.Attributes["title"] = company.CultureCoName;
                if (company.logo != null)
                {
                    var pic = company.logo;
                    imgTag.Attributes["src"] = context.Content("~/Uploads/" + pic.Substring(pic.LastIndexOf('.') + 1) + "/Company/" + pic);

                }
                else
                {
                    imgTag.Attributes["src"] = context.Content("~/Images/Icons/Company.png");
                }
                imgTag.Attributes["alt"] = company.CultureCoName;
            }
            else if (store != null)
            {
                linkTag.Attributes["href"] = context.Action("SProfile", "Store", new { StId = store.storeID, StName = StringHelper.URLName(store.CultureStoreName) });
                linkTag.Attributes["title"] = store.CultureStoreName;
                if (store.logo != null)
                {
                    var pic = store.logo;
                    imgTag.Attributes["src"] = context.Content("~/Uploads/" + pic.Substring(pic.LastIndexOf('.') + 1) + "/Store/" + pic);

                }
                else
                {
                    imgTag.Attributes["src"] = context.Content("~/Images/Icons/Store.png");
                }
                imgTag.Attributes["alt"] = store.CultureStoreName;
            }
            else if (product != null)
            {
                linkTag.Attributes["href"] = context.Action("Detail", "Product", new { PrId = product.productID, PrName = StringHelper.URLName(product.CultureProductName) });
                linkTag.Attributes["title"] = product.CultureProductName;
                if (product.image != null)
                {
                    var pic = product.image.Split(new char[] { ',' }).OrderBy(c => Guid.NewGuid()).First();
                    imgTag.Attributes["src"] = context.Content("~/Uploads/" + pic.Substring(pic.LastIndexOf('.') + 1) + "/Product/" + pic);

                }
                else
                {
                    imgTag.Attributes["src"] = context.Content("~/Images/Icons/Product.png");
                }
                imgTag.Attributes["alt"] = product.CultureProductName;
            }
            else if (service != null)
            {
                linkTag.Attributes["href"] = context.Action("Detail", "Service", new { SrId = service.serviceID, SrName = StringHelper.URLName(service.CultureServiceName) });
                linkTag.Attributes["title"] = service.CultureServiceName;
                if (service.image != null)
                {
                    var pic = service.image.Split(new char[] { ',' }).OrderBy(c => Guid.NewGuid()).First();
                    imgTag.Attributes["src"] = context.Content("~/Uploads/" + pic.Substring(pic.LastIndexOf('.') + 1) + "/Service/" + pic);

                }
                else
                {
                    imgTag.Attributes["src"] = context.Content("~/Images/Icons/Service.png");
                }
                imgTag.Attributes["alt"] = service.CultureServiceName;
            }
            else if (book != null)
            {
                linkTag.Attributes["href"] = context.Action("Detail", "Book", new { BId = book.BookId, BName = StringHelper.URLName(book.title) });
                linkTag.Attributes["title"] = book.title;
                if (book.image != null)
                {
                    var pic = book.image.Split(new char[] { ',' }).OrderBy(c => Guid.NewGuid()).First();
                    imgTag.Attributes["src"] = context.Content("~/Uploads/" + pic.Substring(pic.LastIndexOf('.') + 1) + "/Book/" + pic);
                }
                else
                {
                    imgTag.Attributes["src"] = context.Content("~/Images/Icons/Book.png");
                }
                imgTag.Attributes["alt"] = book.title;
            }

            var linkhtmlattrs = HtmlHelper.AnonymousObjectToHtmlAttributes(linkhtmlAttributes);
            linkTag.MergeAttributes(linkhtmlattrs);
            var htmlattrsimg = HtmlHelper.AnonymousObjectToHtmlAttributes(imghtmlAttributes);
            imgTag.MergeAttributes(htmlattrsimg);

            linkTag.AddCssClass("thumbnail");
            imgTag.Attributes["itemprop"] = "image";
            linkTag.InnerHtml = imgTag.ToString(TagRenderMode.SelfClosing);
            return MvcHtmlString.Create(linkTag.ToString(TagRenderMode.Normal));
        }
    }
}