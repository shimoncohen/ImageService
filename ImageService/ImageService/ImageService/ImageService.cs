﻿using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Runtime.InteropServices;
using ImageService.Logging;
using ImageService.Server;
using ImageService.Modal;
using ImageService.Controller;
using Infrastructure.Enums;
using Infrastructure.Modal.Event;

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
        //private TcpServer tcpServer;
        private TcpApplicationServer tcpApplicationServer;
        private LogHistory history;

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
            // log history class (singelton)
            history = LogHistory.CreateLogHistory();
            // create the service model
            IImageServiceModal model = new ImageServiceModal(info.OutputDir, info.ThumbnailSize);
            // create the services servers
            ImageController controller = new ImageController(model);
            // create image server
            server = new ImageServer(controller, logger);
            // create tcp server
            //tcpServer = new TcpServer(controller, logger);
            // create TcpApplicationServer
            tcpApplicationServer = new TcpApplicationServer(logger);
            // start the tcp server
            string[] str = { };
            //tcpServer.Start(str);
            // start the image server
            server.Start(info.Handlers.ToArray());
            // start the application server
            tcpApplicationServer.Start(str);
            controller.HandlerClosedEvent += server.CloseHandler;
            //logger.NotifyClients += tcpServer.NotifyClients;
            //server.NotifyClients += tcpServer.NotifyClients;
            //tcpServer.CommandRecieved += server.NewCommand;
            logger.MessageRecieved += ImageServiceMessage;
            logger.MessageRecieved += history.UpdateLog;
            //logger.MessageRecieved += tcpServer.NewLog;


            // Update the service state to Start Pending.
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            logger.Log("In OnStart", MessageTypeEnum.INFO);
            // Set up a timer to trigger every minute.  
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 60000; // 60 seconds  
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            timer.Start();

            // Update the service state to Running.  
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }

        /// <summary>
        /// the function that runs when the service stops
        /// </summary>
        protected override void OnStop()
        {
            // write to the log
            logger.Log("In onStop", MessageTypeEnum.INFO);
            LogHistory logHistory = LogHistory.CreateLogHistory();
            // close the image server
            server.Stop();
            // close the tcp server
            //tcpServer.Stop();
            // close the application server
            tcpApplicationServer.Stop();
            logger.MessageRecieved -= ImageServiceMessage;
            //logger.NotifyClients -= tcpServer.NotifyClients;
            logger.MessageRecieved -= logHistory.UpdateLog;
            logger.MessageRecieved -= history.UpdateLog;
            //logger.MessageRecieved += tcpServer.NewLog;
            //server.NotifyClients -= tcpServer.NotifyClients;
            logHistory.ResetLog();
            logger.Log("In onStop", MessageTypeEnum.INFO);
        }

        /// <summary>
        /// the function that runs when monitoring the directories
        /// </summary>
        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            // TODO: Insert monitoring activities here.
            logger.Log("Monitoring the System", MessageTypeEnum.INFO);
            //eventLog1.WriteEntry("Monitoring the System", EventLogEntryType.Information, eventId++);
        }

        /// <summary>
        /// the function writes messages to the logger
        /// </summary>
        /// <param>
        /// the sender object, and the event that occured
        /// </param>
        private void ImageServiceMessage(object sender, MessageRecievedEventArgs e)
        {
            switch(e.Status)
            {
                case MessageTypeEnum.FAIL:
                    this.eventLog1.WriteEntry(e.Message, EventLogEntryType.Error);
                    break;
                case MessageTypeEnum.INFO:
                    this.eventLog1.WriteEntry(e.Message, EventLogEntryType.Information);
                    break;
                case MessageTypeEnum.WARNING:
                    this.eventLog1.WriteEntry(e.Message, EventLogEntryType.Warning);
                    break;
            }
        }
    }
}