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
using System.Diagnostics;
using Infrastructure.Modal.Event;
using Infrastructure.Enums;

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
        private object locker;
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
            locker = new object();
        }

        /// <summary>
        /// starting the server
        /// </summary>
        public void Start()
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
        private void communicate(TcpClient client)
        {
            new Task(() =>
            {
                IClientHandler handler = new ClientHandler(controller, logging);
                handler.CommandRecieved += NewCommand;
                //handler.HandleClient(client, send);
                handler.HandleClient(client, locker);
            }).Start();
        }

        /// <summary>
        /// invokes the CommandRecieved event to indicate a command was recieved
        /// </summary>
        public void NewCommand(object sender, CommandRecievedEventArgs e)
        {
            CommandRecieved?.Invoke(this, e);
        }

        /// <summary>
        /// translates a new message to info and sends it to the function responsible to forward it to the clients
        /// </summary>
        /// <param name= sender> the sending class </param>
        /// <param name= e> the message event arguments </param>
        public void NewLog(object sender, MessageRecievedEventArgs e)
        {
            string[] args = { e.Status.ToString(), e.Message };
            InfoEventArgs info = new InfoEventArgs((int)InfoEnums.LogInfo, args);
            NotifyClients(this, info);
        }

        /// <summary>
        /// sends givent information to the given client
        /// </summary>
        /// <param name= client> the client to send the info to </param>
        /// <param name= info> the information to send to the client </param>
        private void SendToClient(TcpClient client, string info)
        {
            if (client.Connected)
            {
                stream = client.GetStream();
                writer = new BinaryWriter(stream);
                //send.WaitOne();
                lock(locker)
                {
                    writer.Write(info);
                }
                //send.ReleaseMutex();
            }
            else
            {
                // if the client is not connected then remove it from the list of clients
                clients.Remove(client);
            }
        }

        /// <summary>
        /// the function sends given info to all of the clients
        /// </summary>
        /// <param name= sender> the class that invoked the event </param>
        /// <param name= info> the information to send to the client </param>
        public void NotifyClients(object sender, InfoEventArgs e)
        {
            new Task(() =>
            {
                // serealize the object
                string info = JsonConvert.SerializeObject(e);
                List<TcpClient> temp = new List<TcpClient>();
                // copy client list to a new list
                foreach (TcpClient client in clients) temp.Add(client);
                // for each client, send him the information
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

        /// <summary>
        /// stop the server from running
        /// </summary>
        public void Stop()
        {
            clients.Clear();
            listener.Stop();
        }
    }
}
