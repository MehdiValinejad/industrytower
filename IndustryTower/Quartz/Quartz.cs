using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using Quartz.Impl;
using IndustryTower.Quartz.Jobs;


namespace IndustryTower.Quartz
{
    public class Quartz
    {
        public static void ConfigureQuartzJobs()
        {
            // construct a scheduler factory    
            ISchedulerFactory schedFact = new StdSchedulerFactory();
            // get a scheduler    
            IScheduler sched = schedFact.GetScheduler(); 
            sched.Start();
            // construct job info    
            IJobDetail jobDetail = JobBuilder.Create<JOBTempFilesEmpty>()
                                   .WithIdentity("TempFileDeletion", "Important")
                                   .Build();
            //created trigger which will fire every minute starting immediately    
            ITrigger trigger = TriggerBuilder.Create()
                                .WithIdentity("TempFileDeletion", "Important")
                                .WithSimpleSchedule(x => x
                                    .WithIntervalInMinutes(30)
                                    .RepeatForever())
                                .StartAt(DateTime.Now.AddMinutes(5))
                                .Build();

            sched.ScheduleJob(jobDetail, trigger);
        }
    }
    
}