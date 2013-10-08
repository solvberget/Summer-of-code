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
        private readonly System.Timers.Timer _timer;
        private bool _isRunning;
        private Thread _thread;

        public LuceneService()
        {
            InitializeComponent();
            _timer = new System.Timers.Timer();
            _timer.Elapsed += Listen;
            _timer.Interval = 30000;
            _isRunning = false;
        }

        protected override void OnStart(string[] args)
        {
            WriteLogEntry("Dictionary building has started", EventLogEntryType.SuccessAudit);
            _timer.Enabled = true;
        }

        protected override void OnStop()
        {
            _timer.Dispose();
        }

        private void Listen(object source, ElapsedEventArgs args) {
            if (!_isRunning)
            {
                _isRunning = true;
                Task.Factory.StartNew(BuildDictionary);
            }
        }

        private void BuildDictionary() {
            var indexPath = Properties.Settings.Default.IndexPath;
            var sugggestionPath = Properties.Settings.Default.SuggestionPath;
            var suggestionCopy = sugggestionPath.Substring(0, sugggestionPath.Length - 4) + "copy.txt";
            try
            {
                File.Copy(sugggestionPath, suggestionCopy, true);
                DictionaryBuilder.Build(suggestionCopy, indexPath);
                File.Delete(suggestionCopy);
            }
            catch (Exception e)
            {
                WriteLogEntry("Error occured" + e.Message + " - " + DateTime.Now, EventLogEntryType.Error);
            }
            finally
            {
                _isRunning = false;;
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
