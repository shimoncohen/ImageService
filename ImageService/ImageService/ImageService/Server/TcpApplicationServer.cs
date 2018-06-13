using ImageService.Logging;
using ImageService.Modal;
using ImageService.Server.Handlers;
using Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Server
{
    class TcpApplicationServer : Server
    {
        #region Members
        private object locker;
        #endregion

        public TcpApplicationServer(ILoggingService logger)
        {
            this.serverPort = 8001;
            this.logging = logger;
        }

        protected override void communicate(TcpClient client)
        {
            new Task(() =>
            {
                IClientHandler handler = new ApplicationClientHandler(logging);
                handler.HandleClient(client, locker);
            }).Start();
        }
    }
}
