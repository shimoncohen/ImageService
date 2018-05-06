using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using GUI;

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
        { //get; set;
           get { return this.m_LogsInfoList; }
           set
           {
               this.m_LogsInfoList = value;
               OnPropertyChanged("LogsInfoList");
           }
        }
        
        public void AddToList(MessageRecievedEventArgs e)
        {
            this.m_LogsInfoList.Add(e);
            OnPropertyChanged("AddToList");
        }
        
        public LogsModel()
        {
            this.m_LogsInfoList = new ObservableCollection<MessageRecievedEventArgs>();

            this.m_LogsInfoList.Add(new MessageRecievedEventArgs() { Status = MessageTypeEnum.INFO, Message = "Test Message" });
            this.m_LogsInfoList.Add(new MessageRecievedEventArgs() { Status = MessageTypeEnum.WARNING, Message = "Test Message2" });
            this.m_LogsInfoList.Add(new MessageRecievedEventArgs() { Status = MessageTypeEnum.FAIL, Message = "Test Message3" });
        }
    }
}
