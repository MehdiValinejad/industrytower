using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace IndustryTower.Helpers
{
    public static class CultureHelper
    {


        public enum Culture
        {
            fa = 1,
            en = 2
        }

        public class MultiCultureMvcRouteHandler : MvcRouteHandler
        {
            protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
            {
                var culture = requestContext.RouteData.Values["culture"].ToString();
                var ci = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = ci;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
                return base.GetHttpHandler(requestContext);
            }
        }

        public class SingleCultureMvcRouteHandler : MvcRouteHandler { }

        

        public class CultureConstraint : IRouteConstraint
        {
            private string[] _values;
            public CultureConstraint(params string[] values)
            {
                this._values = values;
            }

            public bool Match(HttpContextBase httpContext, Route route, string parameterName,
                                RouteValueDictionary values, RouteDirection routeDirection)
            {
                // Get the value called "parameterName" from the 
                // RouteValueDictionary called "value"
                string value = values[parameterName].ToString();
                // Return true is the list of allowed values contains 
                // this value.
                return _values.Contains(value);
            }
        }


        public static Culture DefaultCulture()
        {
            IndustryTower.Helpers.CultureHelper.Culture defaultC;
            var Request = HttpContext.Current.Request;
            if (Request.Cookies["_ITCul"] == null)
            {
                var userlang = Request.UserLanguages != null && Request.UserLanguages.Length > 0 ?
                    Request.UserLanguages[0] :  // obtain it from HTTP header AcceptLanguages
                    IndustryTower.Helpers.CultureHelper.Culture.fa.ToString();
                bool isFa = userlang.ToUpper().StartsWith("FA");

                HttpCookie culCookie = new HttpCookie("_ITCul");
                culCookie.Value = isFa.ToString();
                HttpContext.Current.Response.Cookies.Add(culCookie);

                defaultC = isFa ? IndustryTower.Helpers.CultureHelper.Culture.fa : IndustryTower.Helpers.CultureHelper.Culture.en;

            }
            else
            {
                defaultC = Request.Cookies["_ITCul"].Value.ToUpper() == "TRUE" ? IndustryTower.Helpers.CultureHelper.Culture.fa : IndustryTower.Helpers.CultureHelper.Culture.en;
            }

            return defaultC;
        }
        // Valid cultures
        private static readonly List<string> _validCultures = new List<string> { "af", "af-ZA", "sq", "sq-AL", "gsw-FR", "am-ET", "ar", "ar-DZ", "ar-BH", "ar-EG", "ar-IQ", "ar-JO", "ar-KW", "ar-LB", "ar-LY", "ar-MA", "ar-OM", "ar-QA", "ar-SA", "ar-SY", "ar-TN", "ar-AE", "ar-YE", "hy", "hy-AM", "as-IN", "az", "az-Cyrl-AZ", "az-Latn-AZ", "ba-RU", "eu", "eu-ES", "be", "be-BY", "bn-BD", "bn-IN", "bs-Cyrl-BA", "bs-Latn-BA", "br-FR", "bg", "bg-BG", "ca", "ca-ES", "zh-HK", "zh-MO", "zh-CN", "zh-Hans", "zh-SG", "zh-TW", "zh-Hant", "co-FR", "hr", "hr-HR", "hr-BA", "cs", "cs-CZ", "da", "da-DK", "prs-AF", "div", "div-MV", "nl", "nl-BE", "nl-NL", "en", "en-AU", "en-BZ", "en-CA", "en-029", "en-IN", "en-IE", "en-JM", "en-MY", "en-NZ", "en-PH", "en-SG", "en-ZA", "en-TT", "en-GB", "en-US", "en-ZW", "et", "et-EE", "fo", "fo-FO", "fil-PH", "fi", "fi-FI", "fr", "fr-BE", "fr-CA", "fr-FR", "fr-LU", "fr-MC", "fr-CH", "fy-NL", "gl", "gl-ES", "ka", "ka-GE", "de", "de-AT", "de-DE", "de-LI", "de-LU", "de-CH", "el", "el-GR", "kl-GL", "gu", "gu-IN", "ha-Latn-NG", "he", "he-IL", "hi", "hi-IN", "hu", "hu-HU", "is", "is-IS", "ig-NG", "id", "id-ID", "iu-Latn-CA", "iu-Cans-CA", "ga-IE", "xh-ZA", "zu-ZA", "it", "it-IT", "it-CH", "ja", "ja-JP", "kn", "kn-IN", "kk", "kk-KZ", "km-KH", "qut-GT", "rw-RW", "sw", "sw-KE", "kok", "kok-IN", "ko", "ko-KR", "ky", "ky-KG", "lo-LA", "lv", "lv-LV", "lt", "lt-LT", "wee-DE", "lb-LU", "mk", "mk-MK", "ms", "ms-BN", "ms-MY", "ml-IN", "mt-MT", "mi-NZ", "arn-CL", "mr", "mr-IN", "moh-CA", "mn", "mn-MN", "mn-Mong-CN", "ne-NP", "no", "nb-NO", "nn-NO", "oc-FR", "or-IN", "ps-AF", "fa", "fa-IR", "pl", "pl-PL", "pt", "pt-BR", "pt-PT", "pa", "pa-IN", "quz-BO", "quz-EC", "quz-PE", "ro", "ro-RO", "rm-CH", "ru", "ru-RU", "smn-FI", "smj-NO", "smj-SE", "se-FI", "se-NO", "se-SE", "sms-FI", "sma-NO", "sma-SE", "sa", "sa-IN", "sr", "sr-Cyrl-BA", "sr-Cyrl-SP", "sr-Latn-BA", "sr-Latn-SP", "nso-ZA", "tn-ZA", "si-LK", "sk", "sk-SK", "sl", "sl-SI", "es", "es-AR", "es-BO", "es-CL", "es-CO", "es-CR", "es-DO", "es-EC", "es-SV", "es-GT", "es-HN", "es-MX", "es-NI", "es-PA", "es-PY", "es-PE", "es-PR", "es-ES", "es-US", "es-UY", "es-VE", "sv", "sv-FI", "sv-SE", "syr", "syr-SY", "tg-Cyrl-TJ", "tzm-Latn-DZ", "ta", "ta-IN", "tt", "tt-RU", "te", "te-IN", "th", "th-TH", "bo-CN", "tr", "tr-TR", "tk-TM", "ug-CN", "uk", "uk-UA", "wen-DE", "ur", "ur-PK", "uz", "uz-Cyrl-UZ", "uz-Latn-UZ", "vi", "vi-VN", "cy-GB", "wo-SN", "sah-RU", "ii-CN", "yo-NG" };
        // Include ONLY cultures you are implementing
        private static readonly List<string> _cultures = new List<string> {
        "en-US",  // first culture is the DEFAULT
        "fa-IR" // Spanish NEUTRAL culture
          // Arabic NEUTRAL culture
        };

        /// <summary>
        /// Returns true if the language is a right-to-left language. Otherwise, false.
        /// </summary>
        public static bool IsRighToLeft()
        {
            return System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.IsRightToLeft;

        }
    
        public static string GetCurrentCulture()
        {
            return Thread.CurrentThread.CurrentCulture.Name;
        }
        public static string GetCurrentNeutralCulture()
        {
            return GetNeutralCulture(Thread.CurrentThread.CurrentCulture.Name);
        }

        public static string GetNeutralCulture(string name)
        {
            if (name.Length < 2)
                return name;
            return name.Substring(0, 2); // Read first two chars only. E.g. "en", "es"
        }


        public static string TrueFalseCulture(bool value)
        {
                return value ? Resource.Resource.yes : Resource.Resource.no;
        }

       

    }
}