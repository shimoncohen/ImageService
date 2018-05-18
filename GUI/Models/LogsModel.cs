using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using GUI;
using GUI.Modal.Event;
using GUI.Enums;
using System.Windows;

namespace GUI.Models
{
    class LogsModel : INotifyPropertyChanged
    {
        // an event that raises when a property is being changed
        public event PropertyChangedEventHandler PropertyChanged;
        
        protected void OnPropertyChanged(string name)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private ObservableCollection<MessageRecievedEventArgs> m_LogsInfoList;
        public ObservableCollection<MessageRecievedEventArgs> LogsInfoList
        {
           get { return this.m_LogsInfoList; }
           set
           {
               this.m_LogsInfoList = value;
               OnPropertyChanged("LogsInfoList");
           }
        }
        
        public void AddToList(MessageRecievedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                this.m_LogsInfoList.Add(e);
            }));
            OnPropertyChanged("LogsInfoList");
        }
        
        public LogsModel()
        {
            this.m_LogsInfoList = new ObservableCollection<MessageRecievedEventArgs>();
            // test messages
            this.m_LogsInfoList.Add(new MessageRecievedEventArgs() { Status = MessageTypeEnum.INFO, Message = "Test Message" });
            this.m_LogsInfoList.Add(new MessageRecievedEventArgs() { Status = MessageTypeEnum.WARNING, Message = "Test Message2" });
            this.m_LogsInfoList.Add(new MessageRecievedEventArgs() { Status = MessageTypeEnum.FAIL, Message = "Test Message3" });
        }

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
                MessageRecievedEventArgs m = new MessageRecievedEventArgs() { Status = type, Message = message };
                // add to the logs list
                Application.Current.Dispatcher.Invoke(new Action(() => { m_LogsInfoList.Add(m); }));
            }
            OnPropertyChanged("LogsInfoList");
        }

        public void AddNewLog(InfoEventArgs e)
        {
            string[] answer = e.Args;
            // split the received log by ",".
            // log[1] is the message 
            string message = answer[1];
            // log[0] is the message type
            MessageTypeEnum type = ParseTypeFromString(answer[0]);
            MessageRecievedEventArgs m = new MessageRecievedEventArgs() { Status = type, Message = message };
            // add to the logs list
            Application.Current.Dispatcher.Invoke(new Action(() => { m_LogsInfoList.Add(m); }));
            OnPropertyChanged("LogsInfoList");
        }

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
    }
}
