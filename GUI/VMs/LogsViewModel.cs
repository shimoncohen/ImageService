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

        public ObservableCollection<LogInfo> VM_LogsInfoList
        {
            get { return this.LogsModel.LogInfoList; }
            set
            {
                this.LogsModel.LogInfoList = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public LogsViewModel()
        {
            LogsModel = new LogsModel();
            LogsModel.PropertyChanged +=
               delegate (Object sender, PropertyChangedEventArgs e) {
                   NotifyPropertyChanged("VM_" + e.PropertyName);
               };
        }

        protected void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
