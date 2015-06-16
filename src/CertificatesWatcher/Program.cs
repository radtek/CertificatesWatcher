using System;
using System.Threading;

namespace CertificatesWatcher
{
    internal static class Program
    {
        private static readonly ScheduleSettings ScheduleSettings = new ScheduleSettings();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static int Main()
        {
            if (new WatcherJob().Execute())
            {
                ScheduleSettings.RegisterSchedulers();

                Console.Out.WriteLine("Certificates Watcher is running.");

                while (ScheduleSettings.Scheduler.IsStarted)
                {
                    Thread.Sleep(10000);
                }
            }
            else
            {
                Console.Out.WriteLine("Certificates Watcher can not be started at first watching.");

                Environment.Exit(-1);
            }

            return 0;
        }
    }
}
