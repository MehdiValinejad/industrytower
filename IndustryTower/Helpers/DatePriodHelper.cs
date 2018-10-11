using System;
using System.Web.Mvc;

namespace IndustryTower.Helpers
{
    public static class DatePriodHelper
    {
        public static string DatePeriod(this HtmlHelper helper, DateTime firstDate, DateTime? secondDate)
        {
            string first = null;
            string second = "- " + Resource.Resource.present;
            var culture = System.Threading.Thread.CurrentThread.CurrentUICulture.Name.ToLowerInvariant();

            first = firstDate.ToString("MMMM,yyyy");
            second = secondDate != null ? secondDate.Value.ToString("MMMM,yyyy") : second;


            return String.Concat(first, " ", Resource.Resource.until, " ", second);
            
        }

    }
}