using System;

namespace CertificatesWatcher
{
    internal static class Program
    {
        private static readonly ScheduleSettings ScheduleSettings = new ScheduleSettings();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            if (new WatcherJob().Execute())
            {
                ScheduleSettings.RegisterSchedulers();

                Console.Out.WriteLine("Certificates Watcher is running.");
            }
            else
            {
                Console.Out.WriteLine("Certificates Watcher enabled to start at first watching.");

                Environment.Exit(-1);
            }
        }
    }
}
