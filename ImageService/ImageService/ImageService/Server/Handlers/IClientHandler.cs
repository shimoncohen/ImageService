using Infrastructure.Modal.Event;
using System;
using System.Net.Sockets;
using System.Threading;

namespace ImageService.Server.Handlers
{
    public interface IClientHandler
    {
        #region Properties
        event EventHandler<CommandRecievedEventArgs> CommandRecieved;
        #endregion

        /// <summary>
        /// Handles a new client of the server
        /// </summary>
        /// <param name= client> the client to handle </param>
        /// <param name= send> the shared mutex for all of the communication </param>
        void HandleClient(TcpClient client, object locker);
    }
}
