using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;

namespace GUI.VMs
{
    class SettingsViewModel : INotifyPropertyChanged
    {
        // intialize Settings model
        public string OutputDirectory { get; set; }
        public string SourceName { get; set; }
        public string LogName { get; set; }
        public string ThumbSize { get; set; }

        public SettingsViewModel()
        {
            this.OutputDirectory = "out";
            this.SourceName = "source";
            this.LogName = "log";
            this.ThumbSize = "10";
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
