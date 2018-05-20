using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUI.Models;
using GUI.Connection;
using GUI;
using System.Windows.Input;
using Prism.Commands;

namespace GUI.VMs
{
    class MainWindowViewModel
    {
        private MainWindowModel MainWindowModel;
        public ICommand CloseCommand { get; private set; }

        public MainWindowViewModel()
        {
            this.MainWindowModel = new MainWindowModel();
            this.CloseCommand = new DelegateCommand<object>(this.OnWindowClosing);
        }

        public string ConnectionStatus {
            get { return this.MainWindowModel.ConnectionStatus; }
        }

        private void OnWindowClosing(object obj)
        {
            Communication communication = Communication.CreateConnectionChannel();
            communication.stop();
        }
    }
}
