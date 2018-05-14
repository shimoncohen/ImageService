using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUI.Modal.Event;
using GUI.Models;

namespace GUI.VMs
{
    interface ConnectionInterface
    {
        event EventHandler<CommandRecievedEventArgs> sendInfo;
        void sendToServer();
        void getInfoFromServer(object sender, InfoEventArgs e);
    }
}
