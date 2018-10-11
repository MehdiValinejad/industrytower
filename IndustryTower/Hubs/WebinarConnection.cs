using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace IndustryTower.Hubs
{
    public class WebinarConnection : PersistentConnection 
    {
        protected override Task OnReceived(IRequest request, string connectionId, string message)
        {
            
            // Broadcast data to all clients
            return Connection.Broadcast(message);
        }
        //protected override bool AuthorizeRequest(IRequest request)
        //{
        //    return request.User.Identity.IsAuthenticated;
        //}
    }

}