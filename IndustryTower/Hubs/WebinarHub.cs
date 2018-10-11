using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace IndustryTower.Hubs
{
    public class WebinarHub : Hub
    {
        public void Send(string message)
        {
            Clients.All.onMessageReceived(message);
        }
    }
}