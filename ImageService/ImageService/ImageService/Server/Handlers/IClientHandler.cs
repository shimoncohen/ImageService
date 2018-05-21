using Infrastructure.Modal.Event;
using System;
using System.Net.Sockets;
using System.Threading;

namespace ImageService.Server.Handlers
{
    public interface IClientHandler
    {
        event EventHandler<CommandRecievedEventArgs> CommandRecieved;

        void HandleClient(TcpClient client, Mutex send);
    }
}
