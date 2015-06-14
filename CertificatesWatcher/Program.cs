using System.ServiceProcess;

namespace CertificatesWatcher
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            var servicesToRun = new ServiceBase[]
            {
                new CertificatesWatcherService()
            };
            ServiceBase.Run(servicesToRun);
        }
    }
}
