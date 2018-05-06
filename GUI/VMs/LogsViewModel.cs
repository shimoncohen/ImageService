using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using GUI.Models;
using System.Collections.ObjectModel;

namespace GUI.VMs
{
    class LogsViewModel : INotifyPropertyChanged
    {
        private LogsModel LogsModel;

        public ObservableCollection<MessageRecievedEventArgs> LogsInfoList
        {
            get { return LogsModel.LogsInfoList; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

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
                   NotifyPropertyChanged(e.PropertyName);
               };
        }

        protected void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
