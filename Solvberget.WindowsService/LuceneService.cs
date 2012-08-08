using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Solvberget.Domain.Implementation;

namespace Solvberget.WindowsService
{
    public class LuceneService : ServiceBase
    {
        private System.Timers.Timer timer;
        private bool IsRunning;
        private Thread thread;

        public LuceneService()
        {
            InitializeComponent();
            timer = new System.Timers.Timer();
            timer.Elapsed += Listen;
            timer.Interval = 120000;
            IsRunning = false;
        }

        protected override void OnStart(string[] args)
        {
            WriteLogEntry("Dictionary building has started", EventLogEntryType.SuccessAudit);
            timer.Enabled = true;
        }

        protected override void OnStop()
        {
            if (thread.IsAlive)
            {
                thread.Join();
            }
            timer.Dispose();
        }

        private void Listen(object source, ElapsedEventArgs args) {
            if (!IsRunning)
            {
                IsRunning = true;
                thread = new Thread(BuildDictionary); 
                thread.Start();
            }
        }

        private void BuildDictionary() {
            try
            {
                var indexPath = Properties.Settings.Default.IndexPath;
                var sugggestionPath = Properties.Settings.Default.SuggestionPath;
                var suggestioncopy = sugggestionPath.Substring(0, sugggestionPath.Length - 4) + "copy.txt";
                File.Copy(sugggestionPath, suggestioncopy);
                using (var repository = new LuceneRepository(indexPath, suggestioncopy))
                {
                    repository.SuggestionListBuildDictionary();
                }
                File.Delete(suggestioncopy);
            }
            catch (Exception e)
            {
                WriteLogEntry("Error occured" + e.Message + " - " + DateTime.Now, EventLogEntryType.Error);
            }
            finally
            {
                IsRunning = false;
                WriteLogEntry("Dictionary building has completed", EventLogEntryType.SuccessAudit);
            }
        }

        private void WriteLogEntry(string message, EventLogEntryType type = EventLogEntryType.Information)
        {
            if (!EventLog.SourceExists("LuceneService"))
                EventLog.CreateEventSource("LuceneService", "Application");
            EventLog.WriteEntry("LuceneService", message, type);
        }

        private void InitializeComponent()
        {
            // 
            // LuceneService
            // 
            this.ServiceName = "LuceneService";

        }
    }
}
