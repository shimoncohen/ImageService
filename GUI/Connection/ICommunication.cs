using System;
using Infrastructure.Modal.Event;

namespace GUI.Connection
{
    interface ICommunication
    {
        // need to hold appConfig info
        // needs to hols logs
        event EventHandler<InfoEventArgs> InfoRecieved;

        void start();
        void StartSenderChannel(object sender, CommandRecievedEventArgs e);
        void stop();
    }
}
