using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUI.Models;
using GUI.Views;
using GUI;

namespace GUI.VMs
{
    class MainWindowViewModel
    {
        private MainWindowModel MainWindowModel;

        public MainWindowViewModel()
        {
            this.MainWindowModel = new MainWindowModel();
        }

        public string ConnectionStatus {
            get { return this.MainWindowModel.ConnectionStatus; }
        }
    }
}
