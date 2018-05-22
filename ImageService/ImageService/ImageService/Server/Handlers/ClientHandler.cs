using ImageService.Controller;
using ImageService.Logging;
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
        Mutex mutex;
        #endregion

        #region Properties
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;
        #endregion

        public ClientHandler(IImageController c, ILoggingService logger)
        {
            controller = c;
            logging = logger;
        }

        public void HandleClient(TcpClient client, object locker)
        {
            new Task(() =>
            {
                // initiate connection
                //mutex = send;
                stream = client.GetStream();
                reader = new BinaryReader(stream);
                writer = new BinaryWriter(stream);
                // as long as the client is connected
                while (client.Connected)
                {
                    string commandLine;
                    try
                    {
                        // read a message from the client
                        commandLine = reader.ReadString();
                    } catch(Exception e)
                    {
                        break;
                    }
                    bool result;
                    CommandRecievedEventArgs args = JsonConvert.DeserializeObject<CommandRecievedEventArgs>(commandLine);
                    // if the requested path is empty indicating it is ment to execute right away
                    if (args.RequestDirPath == "Empty")
                    {
                        string sendString = controller.ExecuteCommand(args.CommandID, args.Args, out result);
                        // if the command executed correctly
                        if (result)
                        {
                            logging.Log("Got command: " + EnumTranslator.CommandToString(args.CommandID) + ", with arguments: " +
                                args.Args, MessageTypeEnum.INFO);
                            try
                            {
                                // let the client know the operation's results
                               // mutex.WaitOne();
                                lock(locker)
                                {
                                    writer.Write(sendString);
                                }
                                //writer.Write(sendString);
                                //mutex.ReleaseMutex();
                            } catch(Exception e)
                            {
                                //mutex.ReleaseMutex();
                                logging.Log("Client disconnected", MessageTypeEnum.INFO);
                                break;
                            }
                            logging.Log("Executed " + EnumTranslator.CommandToString(args.CommandID) +
                                " command and sent the info to the client", MessageTypeEnum.INFO);
                        }
                        else
                        {
                            logging.Log("Failed to execute command: " + EnumTranslator.CommandToString(args.CommandID),
                                MessageTypeEnum.FAIL);
                        }
                    }
                    else
                    {
                        logging.Log("Got command: " + EnumTranslator.CommandToString(args.CommandID) +
                            ", with arguments: " + args.Args + ", to directory: " +
                            args.RequestDirPath, MessageTypeEnum.INFO);
                        // let the handlers know that a command was recieved
                        this.CommandRecieved?.Invoke(this, args);
                    }
                }
            }).Start();
        }
    }
}
