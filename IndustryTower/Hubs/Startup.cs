using IndustryTower.Hubs;
using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

[assembly: OwinStartup(typeof(IndustryTower.Startup))]
namespace IndustryTower
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Any connection or hub wire up and configuration should go here
            app.MapSignalR<WebinarConnection>("/echo");
            app.MapSignalR();
        }
    }
}