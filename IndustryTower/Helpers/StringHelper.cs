using IndustryTower.App_Start;
using System;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace IndustryTower.Helpers
{
    public static class StringHelper
    {
        public static string URLName(string parameter)
        {
            var trimed = parameter.Trim();
            trimed = trimed.Length > 50 ? trimed.Substring(0, 50) : trimed;
            try
            {
                return Regex.Replace(trimed, @"[^\w]", "-",
                                     RegexOptions.None, TimeSpan.FromSeconds(1.5));
            }
            catch (RegexMatchTimeoutException)
            {
                return "-";
            }
        }

        

    }
}