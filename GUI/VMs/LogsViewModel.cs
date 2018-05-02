using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace GUI.VMs
{
    class LogsViewModel : INotifyPropertyChanged
    {
        public string Type { get; set; }
        public string Message { get; set; }

        public LogsViewModel()
        {
            this.Type = "none";
            this.Message = "nothing";
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
