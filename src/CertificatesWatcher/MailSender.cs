using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net.Mime;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using CertificatesWatcher.Configuration;

namespace CertificatesWatcher
{
    internal class MailSender
    {
        public void SendExpiringCertificates(ICollection<X509Certificate2> certificates)
        {
            if (String.IsNullOrWhiteSpace(Config.Current.Mails))
            {
                throw new Exception("Could not send any mails. Target mail address is empty.");
            }
            
            MailMessage message = GetMessage(certificates);

            SetToAddress(message, Config.Current.Mails);

            using (var smtp = new SmtpClient())
            {
                smtp.Send(message);
            }
        }

        private void SetToAddress(MailMessage message, string emails)
        {
            var emailList = emails.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            foreach (var email in emailList)
            {
                message.To.Add(email.Trim());
            }
        }


        public MailMessage GetMessage(ICollection<X509Certificate2> certificates)
        {
            var template = new ExpiringCertificatesTemplate
                {
                    ServerName = Environment.MachineName,
                    ExpiringCertificates = certificates
                };

            var message = new MailMessage {Subject = template.Title};
            var msg = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(template.TransformText()));
            var htmlView = AlternateView.CreateAlternateViewFromString(msg, null, MediaTypeNames.Text.Html);
            message.AlternateViews.Add(htmlView);

            return message;
        }
    }
}
