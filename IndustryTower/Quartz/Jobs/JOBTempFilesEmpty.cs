using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using System.Diagnostics;
using System.IO;

namespace IndustryTower.Quartz.Jobs
{
    public class JOBTempFilesEmpty : IJob
    {
        public void Execute(IJobExecutionContext context) 
        {
            var dirurl = System.Web.Hosting.HostingEnvironment.MapPath("~/Uploads/Temporary/");
            DirectoryInfo dirInfo = new DirectoryInfo(dirurl);
            foreach (var f in dirInfo.GetFiles())
            {
                if (f.LastWriteTime.AddMinutes(30) < DateTime.Now)
                {
                    f.Delete();
                }
            }
        }
    }
}