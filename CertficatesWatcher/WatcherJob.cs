﻿using System;
using System.Diagnostics;
using System.Linq;
using Quartz;

namespace CertficatesWatcher
{
    public class WatcherJob : IJob
    {
        public static readonly CertificateWatcher CertificateWatcher = new CertificateWatcher();

        public void Execute(IJobExecutionContext context)
        {
            var certificates = CertificateWatcher.GetExpiringCertificates();

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
                    Logger.Write(ex.ToString(), EventLogEntryType.Error);
                }
            }
            else
            {
                Logger.Write("All certificates are OK.", EventLogEntryType.Information);
            }
        }
    }
}

