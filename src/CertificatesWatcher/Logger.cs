using System;
using System.Diagnostics;

namespace CertificatesWatcher
{
    public static class Logger
    {
        private static readonly EventLog EventLog;

        public const string LogName = "CertificatesWatcher";

        static Logger()
        {
            if (!EventLog.SourceExists(LogName))
            {
                EventLog.CreateEventSource(LogName, LogName);
            }
            EventLog = new EventLog(LogName, Environment.MachineName, LogName);
        }

        public static void Write(string text, EventLogEntryType type)
        {
            EventLog.WriteEntry(text, type);

            var message = String.Format("{1}: {0}", text, type);

            if (type == EventLogEntryType.Error)
            {
                Console.Error.WriteLine(message);
            }
            else
            {
                Console.Out.WriteLine(message);
            }
        }
    }
}
