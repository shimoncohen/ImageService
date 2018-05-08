﻿using ImageService.Controller;
using ImageService.Server.Handlers;
using ImageService.Logging;
using ImageService.Logging.Modal.Event;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;

namespace ImageService.Server
{
    public class TcpServer
    {
        #region Members
        private IImageController controller;
        private ILoggingService logging;
        private const int serverPort = 8000;
        private TcpListener listener;
        private List<TcpClient> clients;
        private Mutex send;
        #endregion

        #region Properties
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;
        #endregion

        public TcpServer(IImageController c, ILoggingService logger)
        {
            controller = c;
            logging = logger;
            send = new Mutex();
        }

        public void Start()
        {
            clients = new List<TcpClient>();
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
                        logging.Log(e.Message, Logging.Modal.MessageTypeEnum.FAIL);
                        continue;
                    }
                }
            }).Start();
        }

        private void communicate(TcpClient client)
        {
            new Task(() =>
            {
                IClientHandler handler = new ClientHandler(controller, logging);
                handler.CommandRecieved += NewCommand;
                handler.HandleClient(client, send);
            }).Start();
        }

        public void NewCommand(object sender, CommandRecievedEventArgs e)
        {
            CommandRecieved?.Invoke(this, e);
        }

        public void NotifyClients(object sender, InfoEventArgs e)
        {
            new Task(() =>
            {
                string info = JsonConvert.SerializeObject(e);
                send.WaitOne();
                foreach (TcpClient client in clients)
                {
                    using (NetworkStream stream = client.GetStream())
                    using (BinaryWriter writer = new BinaryWriter(stream))
                    {
                        writer.Write(info);
                    }
                }
                send.ReleaseMutex();
            }).Start();
        }

        public void Stop()
        {
            clients.Clear();
            listener.Stop();
        }
    }
}
