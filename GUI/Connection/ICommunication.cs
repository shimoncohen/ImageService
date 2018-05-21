using System;
using Infrastructure.Modal.Event;

namespace GUI.Connection
{
    interface ICommunication
    {
        // need to hold appConfig info
        // needs to hols logs
        event EventHandler<InfoEventArgs> InfoRecieved;
        
        /// <summary>
        /// Start a new tcp client
        /// </summary>
        void Start();

        /// <summary>
        /// Write information to the server with a streamer.
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">The information we write</param>
        void StartSenderChannel(object sender, CommandRecievedEventArgs e);

        /// <summary>
        /// The function calls the close of the tcp client.
        /// </summary>
        void Stop();
    }
}
