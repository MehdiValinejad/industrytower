using IndustryTower.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IndustryTower.Controllers
{
    [ITTAuthorizeAttribute]
    public class DownloadController : Controller
    {
        public FileResult GetBook(string name, string title = "download")
        {
            
            var ext = name.Substring(name.ToLower().LastIndexOf(".") + 1);
            //return File("~/Uploads/" + ext + "/Book/" + name, System.Net.Mime.MediaTypeNames.Application.Octet);
            //return new FilePathResult("~/Uploads/"+ext+"/Book/" + name, System.Net.Mime.MediaTypeNames.Application.Octet);
            byte[] fileBytes = System.IO.File.ReadAllBytes(Request.MapPath("~/Uploads/" + ext + "/Book/" + name));
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, title + "." + ext);
        }
    }
}