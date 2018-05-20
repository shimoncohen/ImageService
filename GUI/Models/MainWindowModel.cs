using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUI.VMs;
using GUI.Views;
using GUI.Connection;
using GUI;

namespace GUI.Models
{
    class MainWindowModel
    {
        private Communication communication;
        private bool connection;

        public MainWindowModel()
        {
            communication = Communication.CreateConnectionChannel();
            // get connection status
            connection = communication.IsConnected();
        }

        public string ConnectionStatus
        {
            get { return this.BackgroundChooser(this.connection); }
        }

        private string BackgroundChooser(bool connection)
        {
            switch (connection)
            {
                case true:
                    return "White";
                case false:
                    return "DarkGray";
                default:
                    return "Transparent";
            }
        }
    }
}
