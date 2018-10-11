using IndustryTower.DAL;
using IndustryTower.Filters;
using IndustryTower.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IndustryTower.Controllers
{
    [ITTAuthorizeAttribute(Roles = "ITAdmin")]
    public class AdminController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        public ActionResult MainMenu()
        {
            AdminViewModel viewmodel = new AdminViewModel();
            viewmodel.Posts = unitOfWork.PostRepository.Get().Count();
            viewmodel.Users = unitOfWork.ActiveUserRepository.Get().Count();
            viewmodel.NotExpiredCompanies = unitOfWork.NotExpiredCompanyRepository.Get().Count();
            viewmodel.ExpiredCompanies = unitOfWork.ExpiredCompanyRepository.Get().Count();
            viewmodel.NotExpiredStores = unitOfWork.StoreNotExpiredRepository.Get().Count();
            viewmodel.ExpiredStores = unitOfWork.StoreExpiredRepository.Get().Count();
            viewmodel.Products = unitOfWork.ProductRepository.Get().Count();
            viewmodel.Services = unitOfWork.ServiceRepository.Get().Count();
            viewmodel.ExpiredEvents = unitOfWork.EventRepository.Get(e => e.untilDate < DateTime.UtcNow).Count();
            viewmodel.UpcomingEvents = unitOfWork.EventRepository.Get(e => e.untilDate > DateTime.UtcNow).Count();
            viewmodel.Questions = unitOfWork.QuestionRepository.Get().Count();

            return View(viewmodel);
        }
	}
}