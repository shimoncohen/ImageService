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

    /// <summary>
    /// A model for the main window.
    /// in charge of the logic of the main window
    /// </summary>
    class MainWindowModel
    {
        private Communication communication;
        private bool connection;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindowModel()
        {
            communication = Communication.CreateConnectionChannel();
            // get connection status
            connection = communication.IsConnected();
        }


        /// <summary>
        /// The function returns the proper color of background as a string.
        /// The background color is determinded according to the connection with the server.
        /// if the client is connected the color will be white, otherwise we return drak-gray
        /// </summary>
        public string ConnectionStatus
        {
            get { return this.BackgroundChooser(this.connection); }
        }


        /// <summary>
        /// The function detemines the background color of the window.
        /// if the client is connected the color will be white, otherwise we return drak-gray
        /// </summary>
        /// <returns>The background color as a string</returns>
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
