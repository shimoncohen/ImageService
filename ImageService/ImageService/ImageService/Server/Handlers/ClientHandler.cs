using ImageService.Controller;
using ImageService.Logging;
using ImageService.Logging.Modal;
using Infrastructure.Enums;
using Infrastructure.Modal.Event;
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
                    string commandLine;
                    try
                    {
                        //send.WaitOne();
                        commandLine = reader.ReadString();
                        //send.ReleaseMutex();
                    } catch(Exception e)
                    {
                        //send.ReleaseMutex();
                        break;
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
                            try
                            {
                                send.WaitOne();
                                writer.Write(sendString);
                                send.ReleaseMutex();
                            } catch(Exception e)
                            {
                                send.ReleaseMutex();
                                logging.Log("Client disconnected", MessageTypeEnum.INFO);
                                break;
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
                }
                //CloseResources(stream, reader, writer);
            }).Start();
        }

        private void CloseResources(Stream stream, BinaryReader reader = null, BinaryWriter writer = null)
        {
            stream.Dispose();
            if (reader != null)
            {
                reader.Close();
            }
            if (writer != null)
            {
                writer.Close();
            }
        }
    }
}
