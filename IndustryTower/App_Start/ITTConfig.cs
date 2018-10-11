using System.Globalization;
using System.Web;
using System.Web.Configuration;

namespace IndustryTower.App_Start
{
    public static class ITTConfig
    {
        static public string BaseURL { get; set; }

        static public string CurrentCulture 
        { 
            get 
            {
                return HttpContext.Current.Request.RequestContext.RouteData.Values["culture"].ToString().ToLowerInvariant();//System.Threading.Thread.CurrentThread.CurrentUICulture.Name.ToLowerInvariant(); 
            } 
        }
        static public string CurrentCultureShotDateFormat { get { return CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern.ToLower(); } }
        static public bool CurrentCultureIsNotEN
        {
            get
            {
                var req = HttpContext.Current.Request;
                return req.RequestContext.RouteData.Values["culture"].ToString().ToUpper() == "FA" ? true : false;
            }
        }

        static public int CompanExpirationDays { get; set; }
        static public int FileSizeLimit { get; set; }
        static public int FileSizeLimitBook { get; set; }
        static public int MaxProfessionTagsLimit { get; set; }
        static public int MaxCategoryTagsLimit { get; set; }

        static public int MaxAdminsLimit { get; set; }

        static public int ImageHeightPost { get; set; }
        static public int ImageHeightProfile { get; set; }
        static public int ImageHeightProServ { get; set; }
        static public int ImageHeightBook { get; set; }

        static public int MaxFilesCountPost { get; set; }
        static public int MaxFilesCountProfile { get; set; }
        static public int MaxFilesCountProServ { get; set; }
        static public int MaxFilesCountBook { get; set; }

        static public int MaxNotifications { get; set; }

        static ITTConfig()
        {
            BaseURL = WebConfigurationManager.AppSettings["BaseURL"];
            CompanExpirationDays = int.Parse(WebConfigurationManager.AppSettings["CompanyExpirationDays"]);
            FileSizeLimit = int.Parse(WebConfigurationManager.AppSettings["FileSizeLimit"]); //change resource Too
            FileSizeLimitBook = int.Parse(WebConfigurationManager.AppSettings["FileSizeLimitBook"]); //change resource Too
            MaxProfessionTagsLimit = int.Parse(WebConfigurationManager.AppSettings["ProfessionTagsLimit"]);
            MaxCategoryTagsLimit = int.Parse(WebConfigurationManager.AppSettings["CategoryTagsLimit"]);
            ImageHeightPost = int.Parse(WebConfigurationManager.AppSettings["ImageHeightPost"]);
            ImageHeightProServ = int.Parse(WebConfigurationManager.AppSettings["ProServImageHeight"]);
            ImageHeightProfile = int.Parse(WebConfigurationManager.AppSettings["ImageHeightProfile"]);
            ImageHeightBook = int.Parse(WebConfigurationManager.AppSettings["ImageHeightBook"]);
            MaxFilesCountPost = int.Parse(WebConfigurationManager.AppSettings["MaxFilesCountPost"]);
            MaxFilesCountProfile = int.Parse(WebConfigurationManager.AppSettings["MaxFilesCountProfile"]);
            MaxFilesCountProServ = int.Parse(WebConfigurationManager.AppSettings["MaxFilesCountProServ"]);
            MaxFilesCountBook = int.Parse(WebConfigurationManager.AppSettings["MaxFilesCountBook"]);
            MaxNotifications = int.Parse(WebConfigurationManager.AppSettings["MaxNotifications"]);
            MaxAdminsLimit = int.Parse(WebConfigurationManager.AppSettings["MaxAdminsLimit"]);
        }

    }

    
}