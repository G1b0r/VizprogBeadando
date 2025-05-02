using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Windows;

namespace project_library
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const string EventSourceName = "LibraryApp";
        private const string EventLogName = "Application";

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Ensure the event source exists
            if (!EventLog.SourceExists(EventSourceName))
            {
                EventLog.CreateEventSource(EventSourceName, EventLogName);
            }
        }
    }

}
