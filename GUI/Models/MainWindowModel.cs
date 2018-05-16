using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUI.VMs;
using GUI.Views;
using GUI;

namespace GUI.Models
{
    class MainWindowModel
    {
        private Model model;
        private bool connection;

        public MainWindowModel()
        {
            model = Model.CreateConnectionChannel();
            // get connection status
            connection = model.IsConnected();
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
