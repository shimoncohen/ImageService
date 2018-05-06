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

        private ObservableCollection<LogInfo> m_LogInfoList;
        public ObservableCollection<LogInfo> LogInfoList
        { get; set;
           /* get { return this.m_LogInfoList; }
            set
            {
                this.m_LogInfoList = value;
                OnPropertyChanged("LogInfoList");
            }*/
        }

        /*private string m_Color;
        public string ColorList
        {
            get
            {
                return;
            }
        }*/
        
        public LogsModel()
        {
            this.m_LogInfoList = new ObservableCollection<LogInfo>();
            this.m_LogInfoList.Add(new LogInfo() { Type = "INFO", Message = "Test Message" });
        }
    }
}
