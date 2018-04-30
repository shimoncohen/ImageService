using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Runtime.InteropServices;
using ImageService.Logging;
using ImageService.Logging.Modal;
using ImageService.Server;
using ImageService.Modal;

namespace ImageService
{
    /// <summary>
    /// the main class of the service
    /// </summary>
    public partial class ImageService : ServiceBase
    {
        private EventLog eventLog1;
        private int eventId = 1;
        private ILoggingService logger;
        private ImageServer server;

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);

        public enum ServiceState
        {
            SERVICE_STOPPED = 0x00000001,
            SERVICE_START_PENDING = 0x00000002,
            SERVICE_STOP_PENDING = 0x00000003,
            SERVICE_RUNNING = 0x00000004,
            SERVICE_CONTINUE_PENDING = 0x00000005,
            SERVICE_PAUSE_PENDING = 0x00000006,
            SERVICE_PAUSED = 0x00000007,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ServiceStatus
        {
            public int dwServiceType;
            public ServiceState dwCurrentState;
            public int dwControlsAccepted;
            public int dwWin32ExitCode;
            public int dwServiceSpecificExitCode;
            public int dwCheckPoint;
            public int dwWaitHint;
        };

        /// <summary>
        /// the constructor of the service
        /// </summary>
        public ImageService(string[] args)
        {
            InitializeComponent();
            // service info class (singelton)
            ServiceInfo info = ServiceInfo.CreateServiceInfo();
            
            string eventSourceName = info.SourceName;
            string logName = info.LogName;

            eventLog1 = new EventLog();
            // create event logs source if dosent exist yet
            if (!EventLog.SourceExists(eventSourceName))
            {
                EventLog.CreateEventSource(eventSourceName, logName);
            }
            eventLog1.Source = eventSourceName;
            eventLog1.Log = logName;
            // create logger
            this.logger = new LoggingService();
        }

        /// <summary>
        /// the function that runs when the service starts
        /// </summary>
        protected override void OnStart(string[] args)
        {
            // service info class (singelton)
            ServiceInfo info = ServiceInfo.CreateServiceInfo();
            // create the service model
            IImageServiceModal model = new ImageServiceModal(info.OutputDir, info.ThumbnailSize);
            // create the services server
            ImageServer server = new ImageServer(model, info.Handlers, this.logger);
            this.server = server;
            this.logger.MessageRecieved += this.ImageServiceMessage;

            // Update the service state to Start Pending.
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            eventLog1.WriteEntry("In OnStart");
            // Set up a timer to trigger every minute.  
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 60000; // 60 seconds  
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            timer.Start();

            // Update the service state to Running.  
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }

        /*protected override void OnContinue()
        {
            eventLog1.WriteEntry("In OnContinue.");
        }*/

        /// <summary>
        /// the function that runs when the service stops
        /// </summary>
        protected override void OnStop()
        {
            this.server.CloseServer();
            this.logger.MessageRecieved -= this.ImageServiceMessage;
            eventLog1.WriteEntry("In onStop.");
        }

        /// <summary>
        /// the function that runs when monitoring the directories
        /// </summary>
        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            // TODO: Insert monitoring activities here.  
            eventLog1.WriteEntry("Monitoring the System", EventLogEntryType.Information, eventId++);
        }

        /// <summary>
        /// the function writes messages to the logger
        /// </summary>
        /// <param>
        /// the sender object, and the event that occured
        /// </param>
        private void ImageServiceMessage(object sender, MessageRecievedEventArgs e)
        {
            this.eventLog1.WriteEntry(e.Message);
            EventLogEntryCollection logs = eventLog1.Entries;
            foreach (EventLogEntry entry in logs) {
                //
            }
        }
    }
}