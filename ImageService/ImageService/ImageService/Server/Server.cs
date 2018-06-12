using ImageService.Logging;
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
    abstract class Server : IServer
    {
        #region Members
        protected ILoggingService logging;
        protected const int serverPort = 8000;
        protected TcpListener listener;
        protected List<TcpClient> clients;
        #endregion

        public void Start(string[] str)
        {
            // create the client list
            clients = new List<TcpClient>();
            // connect to the port
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), serverPort);
            listener = new TcpListener(ep);
            listener.Start();
            new Task(() => {
                while (true)
                {
                    try
                    {
                        // accept new clients
                        TcpClient client = listener.AcceptTcpClient();
                        clients.Add(client);
                        // handle client
                        communicate(client);
                    }
                    catch (Exception e)
                    {
                        logging.Log(e.Message, MessageTypeEnum.FAIL);
                        continue;
                    }
                }
            }).Start();
        }

        /// <summary>
        /// creates a new handler for each clients and starts to handle them
        /// </summary>
        /// <param name= client> the client to handle </param>
        protected abstract void communicate(TcpClient client);

        public void Stop()
        {
            clients.Clear();
            listener.Stop();
        }
    }
}
