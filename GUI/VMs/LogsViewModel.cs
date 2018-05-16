using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using GUI.Models;
using System.Collections.ObjectModel;
using GUI.Modal.Event;
using GUI.Enums;

namespace GUI.VMs
{
    class LogsViewModel : INotifyPropertyChanged, ConnectionInterface
    {
        private LogsModel LogsModel;
        private Model m_ConnectionModel;

        public ObservableCollection<MessageRecievedEventArgs> VM_LogsInfoList
        {
            get { return LogsModel.LogsInfoList; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<CommandRecievedEventArgs> sendInfo;

        public LogsModel LogModel
        {
            get { return this.LogsModel; }
            set
            {
                this.LogsModel = value;
            }
        }

        public LogsViewModel()
        {
            LogsModel = new LogsModel();
            LogsModel.PropertyChanged +=
               delegate (Object sender, PropertyChangedEventArgs e) {
                   NotifyPropertyChanged("VM_" + e.PropertyName);
               };
            m_ConnectionModel = Model.CreateConnectionChannel();
            sendInfo += m_ConnectionModel.StartSenderChannel;
            // getting the initialize info from the server
            m_ConnectionModel.InfoRecieved += getInfoFromServer;
            m_ConnectionModel.start();
            System.Threading.Thread.Sleep(50);
            this.m_ConnectionModel.StartRecieverChannel();
            sendToServer();
        }

        protected void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public void sendToServer()
        {
            string[] args = { };
            CommandRecievedEventArgs e = new CommandRecievedEventArgs((int)CommandEnum.LogCommand, args, "Empty");
            sendInfo?.Invoke(this, e);
        }

        public void getInfoFromServer(object sender, InfoEventArgs e)
        {
            int infoType = InfoReceivedParser.parseInfoType(e.InfoId);
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
