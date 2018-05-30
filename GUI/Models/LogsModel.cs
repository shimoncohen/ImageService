using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using Infrastructure.Modal.Event;
using Infrastructure.Enums;
using Infrastructure.Modal;
using GUI.VMs;
using GUI.Connection;

namespace GUI.Models
{
    /// <summary>
    /// A model to the logs window.
    /// in charge of the logic of the logs
    /// </summary>
    class LogsModel : INotifyPropertyChanged, ConnectionInterface
    {
        // an event that raises when a property is being changed
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        // the list of the logs
        private ObservableCollection<LogInfo> m_LogsInfoList;
        public ObservableCollection<LogInfo> LogsInfoList
        {
           get { return this.m_LogsInfoList; }
           set
           {
               this.m_LogsInfoList = value;
               OnPropertyChanged("LogsInfoList");
           }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public LogsModel()
        {
            this.m_LogsInfoList = new ObservableCollection<LogInfo>();
            Object locker = new object();
            BindingOperations.EnableCollectionSynchronization(m_LogsInfoList, locker);
            m_Connection = Communication.CreateConnectionChannel();

            SendInfo += m_Connection.StartSenderChannel;
            // sign to the event of getting the info from the server
            m_Connection.InfoRecieved += GetInfoFromServer;
            System.Threading.Thread.Sleep(50);
            SendToServer();
        }

        /// <summary>
        /// The function adds all the history of the log to the logs list.
        /// </summary>
        /// <param name="e">The logs from the logger as an array of strings</param>
        public void SetLogHistory(InfoEventArgs e)
        {
            string[] answer = e.Args;
            for (int i = 0; i < answer.Length; i++)
            {
                // split the received log by ",".
                string[] log = answer[i].Split(',');
                MessageTypeEnum type;
                string message;
                // log[0] is the message type
                type = ParseTypeFromString(log[0]);
                // log[1] is the message 
                message = log[1];
                LogInfo m = new LogInfo() { Status = type, Message = message };
                // add to the logs list
                Application.Current.Dispatcher.Invoke(new Action(() => { m_LogsInfoList.Add(m); }));
            }
            OnPropertyChanged("LogsInfoList");
        }

        /// <summary>
        /// The function adds a log to the list of logs.
        /// </summary>
        /// <param name="e">The log we add to the list</param>
        public void AddNewLog(InfoEventArgs e)
        {
            string[] answer = e.Args;
            // split the received log by ",".
            // log[1] is the message 
            string message = answer[1];
            // log[0] is the message type
            MessageTypeEnum type = ParseTypeFromString(answer[0]);
            LogInfo m = new LogInfo() { Status = type, Message = message };
            // add to the logs list
            Application.Current.Dispatcher.Invoke(new Action(() => { m_LogsInfoList.Add(m); }));
            OnPropertyChanged("LogsInfoList");
        }

        /// <summary>
        /// The function parse from string to MessageTypeEnum.
        /// </summary>
        /// <param name="s">The message type as a string</param>
        /// <returns>The message type as an Enum</returns>
        public MessageTypeEnum ParseTypeFromString(string s)
        {
            switch(s)
            {
                case "INFO":
                    return MessageTypeEnum.INFO;
                case "WARNING":
                    return MessageTypeEnum.WARNING;
                case "FAIL":
                    return MessageTypeEnum.FAIL;
                default:
                    return MessageTypeEnum.FAIL;
            }
        }

        private Communication m_Connection;

        public event EventHandler<CommandRecievedEventArgs> SendInfo;

        /// <summary>
        /// The function sends a log command to the server.
        /// </summary>
        public void SendToServer()
        {
            string[] args = { };
            CommandRecievedEventArgs e = new CommandRecievedEventArgs((int)CommandEnum.LogCommand, args, "Empty");
            SendInfo?.Invoke(this, e);
        }

        /// <summary>
        /// The function gets the information from the server.
        /// </summary>
        public void GetInfoFromServer(object sender, InfoEventArgs e)
        {
            int infoType = InfoReceivedParser.parseInfoType(e.InfoId);
            // if infoType is 2 it is an information for the logs model
            if (infoType == 2)
            {
                // 1 is to get a new log from the server
                if (e.InfoId == 1)
                {
                    this.AddNewLog(e);
                }
                else if (e.InfoId == 2)  // 2 is to get log history
                {
                    this.SetLogHistory(e);
                }
            }
        }
    }
}
