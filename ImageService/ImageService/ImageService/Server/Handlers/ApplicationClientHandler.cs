using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ImageService.Logging;
using ImageService.Modal;
using Infrastructure.Modal.Event;

namespace ImageService.Server.Handlers
{
    class ApplicationClientHandler : IClientHandler
    {
        #region Members
        private IImageServiceModal myModel;
        protected ILoggingService logging;
        private object locker;
        #endregion

        #region Properties
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;
        #endregion

        public ApplicationClientHandler(IImageServiceModal model, ILoggingService logger)
        {
            this.myModel = model;
            this.logging = logger;
            locker = new object();
        }

        public void HandleClient(TcpClient client, object locker)
        {
            new Task(() =>
            {
                //stream = client.GetStream();
                //reader = new BinaryReader(stream);
                //writer = new BinaryWriter(stream);

                // as long as the client is connected
                while (client.Connected)
                {
                    byte[] commandLine;
                    try
                    {
                        // read a message from the client
                        //commandLine = reader.ReadString();


                    }
                    catch (Exception e)
                    {
                        break;
                    }
                    bool result;
                }
            }).Start();
        }
    }
}
