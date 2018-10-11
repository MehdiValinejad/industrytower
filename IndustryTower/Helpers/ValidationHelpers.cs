using Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace IndustryTower.Helpers
{
    public static class ValidationHelpers
    {
        public static class WhiteSpace
        {
            public static ValidationResult WhitSpaceCheck(object value)
            {
                if (value == null)
                {
                    return (ValidationResult.Success);
                }
                if (!String.IsNullOrWhiteSpace(value.ToString()))
                {
                    return (ValidationResult.Success);
                }
                else return (new ValidationResult(ModelValidation.whitSpaceNotAllowed));

            }
        }

        public static class URL
        {
            public static ValidationResult URLCheck(object value)
            {
                if (value == null)
                {
                    return (ValidationResult.Success);
                }
                string urlToTest = value.ToString();
                if (!String.IsNullOrWhiteSpace(urlToTest))
                {
                    Uri uriResult;
                    if (Uri.TryCreate(urlToTest, UriKind.Absolute, out uriResult) 
                        && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
                    {
                        return (ValidationResult.Success);
                    }
                    else return (new ValidationResult(ModelValidation.urlField));
                }
                else return (new ValidationResult(ModelValidation.urlField));
            }
        }


        public static class EMAIL
        {
            public static ValidationResult EmailCheck(object value)
            {
                if (value == null)
                {
                    return (ValidationResult.Success);
                }
                string emailToTest = value.ToString();
                if (!String.IsNullOrWhiteSpace(emailToTest))
                {
                    try
                    {
                        var addr = new System.Net.Mail.MailAddress(emailToTest);
                        return (ValidationResult.Success);
                    }
                    catch
                    {
                        return (new ValidationResult(ModelValidation.emailField));
                    }
                    
                }
                else return (new ValidationResult(ModelValidation.emailField));
            }
        }


        public static class MelliCode
        {
            public static ValidationResult MelliCheck(object value)
            {
                if (value == null)
                {
                    return (ValidationResult.Success);
                }
                string codeToTest = value.ToString();
                if (!String.IsNullOrWhiteSpace(codeToTest))
                {
                    //در صورتی که کد ملی وارد شده طولش کمتر از 10 رقم باشد
                    //if (codeToTest.Length != 10)
                    //    return (new ValidationResult(ModelValidation.melliCode));

                    ////در صورتی که کد ملی ده رقم عددی نباشد
                    //var regex = new Regex(@"\d{10}");
                    //if (!regex.IsMatch(codeToTest))
                    //    return (new ValidationResult("کد ملی تشکیل شده از ده رقم عددی می‌باشد؛ لطفا کد ملی را صحیح وارد نمایید"));

                    //در صورتی که رقم‌های کد ملی وارد شده یکسان باشد
                    //var allDigitEqual = new [] { "0000000000", "1111111111", "2222222222", "3333333333", "4444444444", "5555555555", "6666666666", "7777777777", "8888888888", "9999999999" };
                    //if (allDigitEqual.Contains(codeToTest)) return (new ValidationResult(ModelValidation.melliCode));


                    //عملیات شرح داده شده در بالا
                    var chArray = codeToTest.ToCharArray();
                    var num0 = Convert.ToInt32(chArray[0].ToString()) * 10;
                    var num2 = Convert.ToInt32(chArray[1].ToString()) * 9;
                    var num3 = Convert.ToInt32(chArray[2].ToString()) * 8;
                    var num4 = Convert.ToInt32(chArray[3].ToString()) * 7;
                    var num5 = Convert.ToInt32(chArray[4].ToString()) * 6;
                    var num6 = Convert.ToInt32(chArray[5].ToString()) * 5;
                    var num7 = Convert.ToInt32(chArray[6].ToString()) * 4;
                    var num8 = Convert.ToInt32(chArray[7].ToString()) * 3;
                    var num9 = Convert.ToInt32(chArray[8].ToString()) * 2;
                    var a = Convert.ToInt32(chArray[9].ToString());

                    var b = (((((((num0 + num2) + num3) + num4) + num5) + num6) + num7) + num8) + num9;
                    var c = b % 11;

                    if (((c < 2) && (a == c)) || ((c >= 2) && ((11 - c) == a))) 
                    {
                        return (ValidationResult.Success);
                    }
                    else return (new ValidationResult(ModelValidation.melliCode));
                }
                else return (new ValidationResult(ModelValidation.melliCode));
            }
        }

        //public class DateValidation
        //{
        //    public static ValidationResult ValidateDate(string value)
        //    {
        //        var culture = System.Threading.Thread.CurrentThread.CurrentUICulture.Name.ToLowerInvariant();
        //        if (culture.StartsWith("fa"))
        //        {
        //            if (Regex.Match(value, @"^(13|14)\d\d[- /.](0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])$ ").Success)
        //            {
        //                return (ValidationResult.Success);
        //            }
        //            else return (new ValidationResult("Value is not even"));
        //        }
        //        else
        //        {
        //            if (Regex.Match(value, @"^(13|14)\d\d[- /.](0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])$ ").Success)
        //            {
        //                return (ValidationResult.Success);
        //            }
        //            else return (new ValidationResult("Value is not even"));
        //        }

        //        //string[] formats = { "MM/dd/yyyy", "M/d/yyyy", "M/dd/yyyy", "MM/d/yyyy" };
        //        //DateTime expectedDate;
        //        //if (DateTime.TryParseExact(value.ToString(), formats, CultureInfo.InvariantCulture,
        //        //                            DateTimeStyles.None, out expectedDate))
        //        //{
        //        //    return (ValidationResult.Success);
        //        //}
        //        //else
        //        //{
        //        //    return (new ValidationResult("Value is not even"));
        //        //}
        //    }

        //}
    }
}