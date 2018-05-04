using ImageService.Controller;
using ImageService.Logging;
using ImageService.Logging.Modal;
using ImageService.Logging.Modal.Event;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ImageService.Server.Handlers
{
    class ClientHandler : IClientHandler
    {
        #region Members
        private IImageController controller;
        private ILoggingService logging;
        #endregion

        #region Properties
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;
        #endregion

        public ClientHandler(IImageController c, ILoggingService logger)
        {
            controller = c;
            logging = logger;
        }

        public void HandleClient(TcpClient client)
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
                            }
                            else
                            {
                                logging.Log("Failed to execute command: " + args.CommandID, MessageTypeEnum.FAIL);
                            }
                        }
                        else
                        {
                            logging.Log("Got command: " + args.CommandID + ", with arguments: " +
                                    args.Args + ", to directory: " + args.RequestDirPath, MessageTypeEnum.INFO);
                            this.CommandRecieved?.Invoke(this, args);
                        }
                    }
                }
            }).Start();
        }
    }
}
