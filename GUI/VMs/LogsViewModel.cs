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
    class LogsViewModel : INotifyPropertyChanged
    {
        private LogsModel LogsModel;

        // the list of the logs.
        public ObservableCollection<LogInfo> VM_LogsInfoList
        {
            get { return LogsModel.LogsInfoList; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

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
        }

        protected void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
