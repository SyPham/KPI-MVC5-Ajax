using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Configuration;
using KPI.Model.helpers;

namespace KPI.Web.WindowService
{
    public class JobScheduler
    {
        public static async Task StartAsync()
        {
            var hh = ConfigurationManager.AppSettings["hh"].ToInt();
            var mm = ConfigurationManager.AppSettings["mm"].ToInt();
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler().ConfigureAwait(true);

          await  scheduler.Start();

            IJobDetail job = JobBuilder.Create<EmailJob>().Build();

            ITrigger trigger = TriggerBuilder.Create()

                .WithDailyTimeIntervalSchedule

                  (s =>

                     s.WithIntervalInHours(24)

                    .OnEveryDay()

                    .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(hh, mm))
                  )

                .Build();

           await scheduler.ScheduleJob(job, trigger);

        }
    }
}