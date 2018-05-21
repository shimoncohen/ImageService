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
    /// <summary>
    /// A View-Model for the main window
    /// </summary>
    class MainWindowViewModel
    {
        private MainWindowModel MainWindowModel;
        public ICommand CloseCommand { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindowViewModel()
        {
            this.MainWindowModel = new MainWindowModel();
            this.CloseCommand = new DelegateCommand<object>(this.OnWindowClosing);
        }

        /// <summary>
        /// The function returns the status of the connection with the server.
        /// </summary>
        /// <return>the connection status as a string</return>
        public string ConnectionStatus {
            get { return this.MainWindowModel.ConnectionStatus; }
        }

        /// <summary>
        /// When closing the window we stop the communication with the server
        /// </summary>
        private void OnWindowClosing(object obj)
        {
            Communication communication = Communication.CreateConnectionChannel();
            communication.Stop();
        }
    }
}
