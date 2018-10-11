using IndustryTower.ViewModels;
using Mvc.Mailer;

namespace IndustryTower.Mailers
{ 
    public interface IUserMailer
    {
			MvcMailMessage Welcome();
            MvcMailMessage ConfirmResend(MailResendConfirmationViewModel model);
            MvcMailMessage Registraion(MailRgisterConfirmationViewModel model);
            MvcMailMessage PasswordReset(MailPassResetConfirmationViewModel model);
            MvcMailMessage NewPass(MailNewPassConfirmationViewModel model);
	}
}