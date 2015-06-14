using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;

namespace CertificatesWatcher
{
    public partial class CertificatesWatcherService : ServiceBase
    {
        public CertificatesWatcherService()
        {
            InitializeComponent();
        }

        private readonly ScheduleSettings _scheduleSettings = new ScheduleSettings();

        protected override void OnStart(string[] args)
        {
            _scheduleSettings.RegisterSchedulers();

            new WatcherJob().Execute(null);
        }

        protected override void OnStop()
        {
        }
    }
}
