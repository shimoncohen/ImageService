using ImageService.Logging.Modal.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageService.Server.Handlers
{
    public interface IClientHandler
    {
        event EventHandler<CommandRecievedEventArgs> CommandRecieved;

        void HandleClient(TcpClient client, Mutex send);
    }
}
