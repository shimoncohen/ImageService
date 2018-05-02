using ImageService.Controller;
using ImageService.Controller.Handlers;
using Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Modal;
using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using ImageService.Logging.Modal;
using System.IO;
using Newtonsoft.Json;
using Infrastructure.Modal.Event;
using System.Collections.Generic;

namespace ImageService.Server
{
    /// <summary>
    /// the class of the server. holds a controller to execute commands and a logger to write operations that occured.
    /// </summary>
    public class ImageServer
    {
        #region Members
        private IImageController controller;
        private ILoggingService logging;
        private const int serverPort = 8000;
        private TcpListener listener;
        private List<TcpClient> clients;
        #endregion

        #region Properties
        // The event that notifies about a new Command being recieved
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;
        //public event EventHandler<DirectoryCloseEventArgs> Closing;
        #endregion

        /// <summary>
        /// constuctor
        /// </summary>
        /// <param name= model> the image modal we have, to create the proper controller. </param>
        /// <param name= handler> the paths to all the directories we want to monitor </param>
        /// <param name= logger> a logger to follow action and operations that occured </param>
        public ImageServer(ImageController imageController, string[] handlers, ILoggingService logger)
        {
            // create controller
            this.controller = imageController;
            this.logging = logger;
            // create handler for each given directory
            foreach (string directory in handlers)
            {
                CreateHandler(directory);
            }
        }

        private void Start()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), serverPort);
            listener = new TcpListener(ep);
            listener.Start();
            new Task(() => {
                while(true) {
                    try {
                        TcpClient client = listener.AcceptTcpClient();
                        clients.Add(client);
                        communicate(client);
                    } catch(Exception e) {
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

        public void SendLog(object sender, MessageRecievedEventArgs e)
        {
            string[] args = new string[2];
            args[0] = e.Status.ToString();
            args[1] = e.Message;
            InfoEventArgs info = new InfoEventArgs((int)InfoEnums.LogInfo, args);
            NotifyClients(info);
        }

        private void NotifyClients(InfoEventArgs e)
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

        public void Stop() {
            listener.Stop();
        }
 
        /// <summary>
        /// creates a new handler for a given directory
        /// </summary>
        /// <param name= directory> the path to the directory we want to monitor </param>
        private void CreateHandler(string directory)
        {
            // create handler for given directory
            IDirectoryHandler directoryHandler = new DirectoyHandler(this.controller, this.logging);
            directoryHandler.DirectoryClose += new EventHandler<DirectoryCloseEventArgs>(CloseHandler);
            this.CommandRecieved += directoryHandler.OnCommandRecieved;
            directoryHandler.StartHandleDirectory(directory);
        }

        /// <summary>
        /// the function stops the handling of a specific directiory
        /// </summary>
        /// <param name= sender> the object that sent the request </param>
        /// <param name= e> the event that occured </param>
        private void CloseHandler(object sender, DirectoryCloseEventArgs e)
        {
            IDirectoryHandler handlerToClose = (IDirectoryHandler)sender;
            this.CommandRecieved -= handlerToClose.OnCommandRecieved;
            this.logging.Log("Closed handler for " + e.DirectoryPath, MessageTypeEnum.INFO);
            
            //TODO: check if handler is deleted
            //handlerToClose.DirectoryClose -= CloseHandler;
            // delete handler

            string[] args = { e.DirectoryPath };
            // create info args
            InfoEventArgs info = new InfoEventArgs((int)InfoEnums.CloseHandlerInfo, args);
            // remove the handler from the app config handler list
            ServiceInfo serviceInfo = ServiceInfo.CreateServiceInfo();
            serviceInfo.RemoveHandler(e.DirectoryPath);
            // notify all of the clients that the handler was closed
            NotifyClients(info);
        }

        /// <summary>
        /// the function closes the server. invoke closing operation and writes to log.
        /// </summary>
        public void CloseServer()
        {
            string[] args = { };
            CommandRecievedEventArgs e = new CommandRecievedEventArgs((int)CommandEnum.CloseCommand, args, "*");
            Stop();
            this.CommandRecieved?.Invoke(this, e);
            this.logging.Log("Server closing", MessageTypeEnum.INFO);
        }
    }
}