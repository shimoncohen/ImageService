using ImageService.Controller;
using ImageService.Logging;
using ImageService.Logging.Modal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ImageService.Server
{
    class TcpServer
    {
        #region Members
        private IImageController controller;
        private ILoggingService logging;
        private const int serverPort = 8000;
        private TcpListener listener;
        private List<TcpClient> clients;
        #endregion

        #region Properties
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;
        #endregion

        public TcpServer(IImageController c, ILoggingService logger)
        {
            controller = c;
            logging = logger;
        }

        private void Start()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), serverPort);
            listener = new TcpListener(ep);
            listener.Start();
            new Task(() => {
                while (true)
                {
                    try
                    {
                        TcpClient client = listener.AcceptTcpClient();
                        clients.Add(client);
                        communicate(client);
                    }
                    catch (Exception e)
                    {
                        break;
                    }
                }
            }).Start();
        }

        private void communicate(TcpClient client)
        {
            new Task(() =>
            {
                while (true)
                {
                    using (NetworkStream stream = client.GetStream())
                    using (StreamReader reader = new StreamReader(stream))
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        string commandLine = reader.ReadLine();
                        bool result;
                        CommandRecievedEventArgs args = JsonConvert.DeserializeObject<CommandRecievedEventArgs>(commandLine);
                        if (args.RequestDirPath == "Empty")
                        {
                            string send = controller.ExecuteCommand(args.CommandID, args.Args, out result);
                            if (result)
                            {
                                writer.Write(send);
                                logging.Log("Got command: " + args.CommandID + ", with arguments: " +
                                    args.Args + ", to directory: " + args.RequestDirPath, MessageTypeEnum.INFO);
                            }
                            else
                            {
                                logging.Log("Failed to execute command: " + args.CommandID, MessageTypeEnum.FAIL);
                            }
                        }
                        else
                        {
                            this.CommandRecieved?.Invoke(this, args);
                        }
                    }
                }
            }).Start();
        }

        public void NotifyClients(object sender, InfoEventArgs e)
        {
            new Task(() =>
            {
                string info = JsonConvert.SerializeObject(e);
                foreach (TcpClient client in clients)
                {
                    using (NetworkStream stream = client.GetStream())
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(info);
                    }
                }
            }).Start();
        }

        public void Stop()
        {
            listener.Stop();
        }
    }
}
