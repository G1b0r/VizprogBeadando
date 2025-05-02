using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace project_library.Utilities
{
    public static class EventLogger
    {
        private const string EventSourceName = "LibraryApp";
        private const string EventLogName = "Application";

        static EventLogger()
        {
            // Ensure the event source exists
            if (!EventLog.SourceExists(EventSourceName))
            {
                EventLog.CreateEventSource(EventSourceName, EventLogName);
            }
        }

        public static void LogEvent(string message, EventLogEntryType type)
        {
            EventLog.WriteEntry(EventSourceName, message, type);
        }
    }
}
