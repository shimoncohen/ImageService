using System;
using Infrastructure.Modal.Event;

namespace GUI.VMs
{
    /// <summary>
    /// Interaface for interaction with the server
    /// </summary>
    interface ConnectionInterface
    {

        event EventHandler<CommandRecievedEventArgs> SendInfo;
        
        /// <summary>
        /// Receive information from the server.
        /// </summary>
        /// <param name="sender">The Object that sent the info</param>
        /// <param name="e">The received arguments</param>
        void GetInfoFromServer(object sender, InfoEventArgs e);
    }
}
