using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;

namespace CertficatesWatcher
{
    public partial class CertficatesWatcherService : ServiceBase
    {
        public CertficatesWatcherService()
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
