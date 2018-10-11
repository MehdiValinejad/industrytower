using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IndustryTower.ViewModels
{
    public class MailResendConfirmationViewModel
    {
        public string EmailAddress { get; set; }
        public string DisplayName { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
    }

    public class MailRgisterConfirmationViewModel
    {
        public string EmailAddress { get; set; }
        public string DisplayName { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
    }

    public class MailPassResetConfirmationViewModel
    {
        public string EmailAddress { get; set; }
        public string DisplayName { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
    }

    public class MailNewPassConfirmationViewModel
    {
        public string EmailAddress { get; set; }
        public string DisplayName { get; set; }
        public string UserName { get; set; }
        public string NewPass { get; set; }
    }


}