using System;
using System.ComponentModel;
using GUI.Models;
using GUI.Connection;
using System.Collections.ObjectModel;
using Infrastructure.Modal.Event;
using Infrastructure.Enums;
using Infrastructure.Modal;

namespace GUI.VMs
{
    /// <summary>
    /// A View-Model for the logs window
    /// </summary>
    class LogsViewModel : INotifyPropertyChanged, ConnectionInterface
    {
        private LogsModel LogsModel;
        private Communication m_Connection;

        // the list of the logs.
        public ObservableCollection<LogInfo> VM_LogsInfoList
        {
            get { return LogsModel.LogsInfoList; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<CommandRecievedEventArgs> SendInfo;

        // the logs model
        public LogsModel LogModel
        {
            get { return this.LogsModel; }
            set
            {
                this.LogsModel = value;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public LogsViewModel()
        {
            LogsModel = new LogsModel();
            // sign to the event of property changed
            LogsModel.PropertyChanged +=
               delegate (Object sender, PropertyChangedEventArgs e) {
                   NotifyPropertyChanged("VM_" + e.PropertyName);
               };
            m_Connection = Communication.CreateConnectionChannel();
            SendInfo += m_Connection.StartSenderChannel;
            // sign to the event of getting the info from the server
            m_Connection.InfoRecieved += GetInfoFromServer;
            System.Threading.Thread.Sleep(50);
            SendToServer();
        }

        protected void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

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
                    LogModel.AddNewLog(e);
                }
                else if (e.InfoId == 2)  // 2 is to get log history
                {
                    LogsModel.SetLogHistory(e);
                }
            }
        }
    }
}
