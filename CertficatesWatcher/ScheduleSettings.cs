using System;
using System.Collections.Specialized;
using System.Diagnostics;
using Quartz;
using Quartz.Impl;

namespace CertficatesWatcher
{
    public class ScheduleSettings
    {
        public readonly IScheduler Scheduler;

        public ScheduleSettings()
        {
            var properties = new NameValueCollection();
            properties["quartz.scheduler.instanceName"] = "RemoteServer";

            // set thread pool info
            properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
            properties["quartz.threadPool.threadCount"] = "5";
            properties["quartz.threadPool.threadPriority"] = "Normal";

            ISchedulerFactory schedulerFactory = new StdSchedulerFactory(properties);
            Scheduler = schedulerFactory.GetScheduler();
        }

        public bool IsRegistered { get; private set; }


        public void RegisterSchedulers()
        {
            if (!IsRegistered)
            {
                lock (Scheduler)
                {
                    if (!IsRegistered)
                    {
                        Scheduler.ScheduleJob(CreateJobDetail<WatcherJob>(), EveryDayOneTimeAt(TimeOfDay.HourAndMinuteOfDay(0, 5)));

                        Scheduler.Start();

                        IsRegistered = true;

                        Logger.Write("Watcher Job was created.", EventLogEntryType.Information);
                    }
                }
            }
        }

        private IJobDetail CreateJobDetail<TJob>() where TJob : IJob
        {
            JobBuilder jobBuilder = JobBuilder.Create();

            jobBuilder.OfType<TJob>();

            return jobBuilder.Build();
        }

        private ITrigger EveryDayOneTimeAt(TimeOfDay timeOfDay)
        {
            TriggerBuilder triggerBuilder = TriggerBuilder.Create();

            triggerBuilder.WithCronSchedule(String.Format("0 {0} {1} ? * *", timeOfDay.Minute, timeOfDay.Hour));

            return triggerBuilder.Build();
        }
    }
}
