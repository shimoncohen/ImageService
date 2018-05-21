using ImageService.Controller;
using ImageService.Server.Handlers;
using ImageService.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;
using Infrastructure.Enums;
using ImageService.Logging.Modal;
using System.Diagnostics;
using Infrastructure.Modal.Event;

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
        private NetworkStream stream;
        private BinaryWriter writer;
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
                        logging.Log(e.Message, MessageTypeEnum.FAIL);
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

        public void NewLog(object sender, MessageRecievedEventArgs e)
        {
            string[] args = { e.Status.ToString(), e.Message };
            InfoEventArgs info = new InfoEventArgs((int)InfoEnums.LogInfo, args);
            NotifyClients(this, info);
        }

        private void SendToClient(TcpClient client, string info)
        {
            if (client.Connected)
            {
                stream = client.GetStream();
                writer = new BinaryWriter(stream);
                send.WaitOne();
                writer.Write(info);
                send.ReleaseMutex();
            }
            else
            {
                clients.Remove(client);
            }
        }

        public void NotifyClients(object sender, InfoEventArgs e)
        {
            new Task(() =>
            {
                string info = JsonConvert.SerializeObject(e);
                List<TcpClient> temp = new List<TcpClient>();
                foreach (TcpClient client in clients) temp.Add(client);
                foreach (TcpClient client in temp)
                {
                    try
                    {
                        SendToClient(client, info);
                    } catch(Exception e1)
                    {
                        Debug.WriteLine("In TcpServer, failed send to client, Error: " + e1.ToString());
                        continue;
                    }
                }
            }).Start();
        }

        public void Stop()
        {
            clients.Clear();
            listener.Stop();
        }
    }
}
