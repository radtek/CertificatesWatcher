using System;
using System.Diagnostics;

namespace CertficatesWatcher
{
    public static class Logger
    {
        private static readonly EventLog EventLog;

        public const string LogName = "CertficatesWatcher";

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
        }
    }
}
