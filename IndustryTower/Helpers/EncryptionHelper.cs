using System;
using System.Text;
using System.Web;
using System.Web.Security;

namespace IndustryTower.Helpers
{
    public class EncryptionHelper
    {
        private const string Purpose = "Authentication Token";
        private const string Secret1 = "I5T";
        private const string Secret2 = "2I3";

        public static string Protect(object unprotectedText)
        {
            var unprotectedBytes = Encoding.UTF8.GetBytes(Secret1 + unprotectedText as string + Secret2);
            var protectedBytes = MachineKey.Protect(unprotectedBytes, Purpose);
            var protectedText = HttpServerUtility.UrlTokenEncode(protectedBytes);
            //var protectedText = Convert.ToBase64String(protectedBytes);
            return protectedText;
        }

        public static int? Unprotect(string protectedText)
        {
            if(!String.IsNullOrEmpty(protectedText))
            {
                var protectedBytes = HttpServerUtility.UrlTokenDecode(protectedText);
                //var protectedBytes = Convert.FromBase64String(protectedText);
                var unprotectedBytes = MachineKey.Unprotect(protectedBytes, Purpose);
                var unprotectedText = Encoding.UTF8.GetString(unprotectedBytes);
                var finalString = unprotectedText.Substring(Secret1.Length, unprotectedText.Length - Secret2.Length - Secret1.Length);
                return int.Parse(finalString);
            }
            return null;
        }

        //public static string Encrypt(string plaintextValue)
        //{
        //    var plaintextBytes = Encoding.UTF8.GetBytes(plaintextValue);
        //    return MachineKey.Encode(plaintextBytes, MachineKeyProtection.All);
        //}

        //public static string Decrypt(string encryptedValue)
        //{
        //    try
        //    {
        //        var decryptedBytes = MachineKey.Decode(encryptedValue, MachineKeyProtection.All);
        //        return Encoding.UTF8.GetString(decryptedBytes);
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}
    }
}