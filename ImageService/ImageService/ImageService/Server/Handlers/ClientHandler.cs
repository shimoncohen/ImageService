using ImageService.Controller;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Modal;
using ImageService.Logging.Modal.Event;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace ImageService.Server.Handlers
{
    class ClientHandler : IClientHandler
    {
        #region Members
        private IImageController controller;
        private ILoggingService logging;
        NetworkStream stream;
        BinaryReader reader;
        BinaryWriter writer;
        #endregion

        #region Properties
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;
        #endregion

        public ClientHandler(IImageController c, ILoggingService logger)
        {
            controller = c;
            logging = logger;
        }

        public void HandleClient(TcpClient client, Mutex send)
        {
            new Task(() =>
            {
                stream = client.GetStream();
                reader = new BinaryReader(stream);
                writer = new BinaryWriter(stream);
                while (client.Connected)
                {
                    send.WaitOne();
                    string commandLine;
                    try
                    {
                        commandLine = reader.ReadString();
                    } catch(Exception e)
                    {
                        send.ReleaseMutex();
                        return;
                    }
                    bool result;
                    CommandRecievedEventArgs args = JsonConvert.DeserializeObject<CommandRecievedEventArgs>(commandLine);
                    if (args.RequestDirPath == "Empty")
                    {
                        string sendString = controller.ExecuteCommand(args.CommandID, args.Args, out result);
                        if (result)
                        {
                            logging.Log("Got command: " + args.CommandID + ", with arguments: " +
                                args.Args, MessageTypeEnum.INFO);
                            if(client.Connected)
                            {
                                writer.Write(sendString);
                            } else
                            {
                                logging.Log("Client dissconnected", MessageTypeEnum.INFO);
                                send.ReleaseMutex();
                                return;
                            }
                            // TODO: write to log that command was sent
                            logging.Log("Sent " + args.CommandID.ToString(), MessageTypeEnum.INFO);
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
                    send.ReleaseMutex();
                }
            }).Start();
        }

        private void CloseResources(Stream stream, BinaryReader reader = null, BinaryWriter writer = null)
        {
            stream.Dispose();
            reader.Close();
            writer.Close();
        }
    }
}
