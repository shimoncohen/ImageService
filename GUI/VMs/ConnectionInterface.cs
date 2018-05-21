using System;
using Infrastructure.Modal.Event;

namespace GUI.VMs
{
    interface ConnectionInterface
    {
        event EventHandler<CommandRecievedEventArgs> sendInfo;
        void sendToServer();
        void getInfoFromServer(object sender, InfoEventArgs e);
    }
}
