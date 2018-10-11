using IndustryTower.ViewModels;
using Mvc.Mailer;
using System.Collections.Generic;
using System.Net.Mail;

namespace IndustryTower.Mailers
{ 
    public class UserMailer : MailerBase, IUserMailer 	
	{

		public UserMailer()
		{
			MasterName="_Layout";
		}
		
		public virtual MvcMailMessage Welcome()
		{
			//ViewBag.Data = someObject;
			return Populate(x =>
			{
				x.Subject = "Welcome";
				x.ViewName = "Welcome";
				x.To.Add("some-email@example.com");
                x.IsBodyHtml = true;
			});
		}
        
        public virtual MvcMailMessage ConfirmResend(MailResendConfirmationViewModel model)
		{
            ViewData.Model = model;
            //var resources = new Dictionary<string, string>();
            //resources["logo"] = CurrentHttpContext.Server.MapPath(model.image);
			//ViewBag.Data = someObject;
			return Populate(x =>
			{
				x.Subject = Resource.Resource.confirmation;
                x.ViewName = "ConfirmResend";
                x.From = new MailAddress(address: "noreply@industrytower.com", displayName: Resource.Resource.title);
                x.To.Add(new MailAddress(address: model.EmailAddress, displayName: model.DisplayName));
                x.IsBodyHtml = true;
			});
		}

        public virtual MvcMailMessage Registraion(MailRgisterConfirmationViewModel model)
        {
            ViewData.Model = model;
            //var resources = new Dictionary<string, string>();
            //resources["logo"] = CurrentHttpContext.Server.MapPath(model.image);
            //ViewBag.Data = someObject;
            return Populate(x =>
            {
                x.Subject = Resource.Resource.confirmation;
                x.ViewName = "Registration";
                x.From = new MailAddress(address: "noreply@industrytower.com", displayName: Resource.Resource.title);
                x.To.Add(new MailAddress(address:model.EmailAddress, displayName: model.DisplayName));
                x.IsBodyHtml = true;
            });
        }

        public virtual MvcMailMessage PasswordReset(MailPassResetConfirmationViewModel model)
        {
            ViewData.Model = model;
            //var resources = new Dictionary<string, string>();
            //resources["logo"] = CurrentHttpContext.Server.MapPath(model.image);
            //ViewBag.Data = someObject;
            return Populate(x =>
            {
                x.Subject = Resource.Resource.passReset;
                x.ViewName = "PasswordReset";
                x.From = new MailAddress(address: "noreply@industrytower.com", displayName: Resource.Resource.title);
                x.To.Add(new MailAddress(address: model.EmailAddress, displayName: model.DisplayName));
                x.IsBodyHtml = true;
            });
        }

        public virtual MvcMailMessage NewPass(MailNewPassConfirmationViewModel model)
        {
            ViewData.Model = model;
            //var resources = new Dictionary<string, string>();
            //resources["logo"] = CurrentHttpContext.Server.MapPath(model.image);
            //ViewBag.Data = someObject;
            return Populate(x =>
            {
                x.Subject = Resource.Resource.passReset;
                x.ViewName = "NewPass";
                x.From = new MailAddress(address: "noreply@industrytower.com", displayName: Resource.Resource.title);
                x.To.Add(new MailAddress(address: model.EmailAddress, displayName: model.DisplayName));
                x.IsBodyHtml = true;
            });
        }
 	}
}