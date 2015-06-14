using System;
using System.Diagnostics;
using System.Linq;
using CertificatesWatcher.Configuration;
using Quartz;

namespace CertificatesWatcher
{
    public class WatcherJob : IJob
    {
        public static readonly CertificateWatcher CertificateWatcher = new CertificateWatcher();

        void IJob.Execute(IJobExecutionContext context)
        {
            Execute();
        }

        public bool Execute()
        {
            var certificates = CertificateWatcher.GetExpiringCertificates(TimeSpan.FromDays(Config.Current.DaysToExpiration));

            if (certificates.Any())
            {
                try
                {
                    var mailSender = new MailSender();
                    mailSender.SendExpiringCertificates(certificates);
                    
                    Logger.Write("Mail about expiring certificates was created and sent.",
                        EventLogEntryType.Information);
                }
                catch (Exception ex)
                {
                    Logger.Write(ex.Message, EventLogEntryType.Error);
                    return false;
                }
            }
            else
            {
                Logger.Write("All certificates are OK.", EventLogEntryType.Information);
            }
            return true;
        }
    }
}

