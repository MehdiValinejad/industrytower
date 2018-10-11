using IndustryTower.DAL;
using System.Linq;
using System.Web.Mvc;

namespace IndustryTower.Controllers
{
    public class ProjectController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        public ActionResult Index(int UId)
        {
            var userProjects = unitOfWork.ProjectRepository.Get(filter: p => p.Offers.Where(of => of.accepted).Select(o => o.offererID).Contains(UId));
            return PartialView(userProjects);
        }

    }
}
