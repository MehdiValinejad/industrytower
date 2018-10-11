using IndustryTower.DAL;
using IndustryTower.ViewModels;
using System.Linq;
using System.Web.Mvc;

namespace IndustryTower.Controllers
{

    public class MessageController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        public ActionResult MessageBox(int UId)
        {
            var rr = unitOfWork.MessageRepository.Get(m=>m.ReceiverUsers.Select(t=>t.UserId).Contains(UId) || m.senderUserID == UId);
            var GG = from h in rr
                     orderby h.messageDate descending
                     group h by new
                     {
                         h.SenderUser,
                         h.SenderCompany,
                         h.SenderStore,

                     } into g
                     select new MessageViewModel
                     {
                         user = g.Key.SenderUser,
                         company = g.Key.SenderCompany,
                         store = g.Key.SenderStore,

                         message = g.ToList()
                     };
            return PartialView(GG);
        }

    }
    
}
