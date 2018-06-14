using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ImageService.Logging;
using ImageService.Modal;
using Infrastructure.Enums;
using Infrastructure.Modal.Event;

namespace ImageService.Server.Handlers
{
    public class ApplicationClientHandler : IClientHandler
    {
        #region Members
        protected ILoggingService logging;
        private NetworkStream stream;
        private const int serverPort = 8001;
        private const int MAXREAD = 50000000;
        #endregion

        #region Properties
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;
        #endregion

        public ApplicationClientHandler(ILoggingService logger)
        {
            this.logging = logger;
        }

        public void HandleClient(TcpClient client, object locker)
        {
            new Task(() =>
            {
                stream = client.GetStream();

                // as long as the client is connected
                while (client.Connected)
                {
                    byte[] picture = new byte[MAXREAD];
                    int numBytes;
                    try
                    {
                        // read a message from the client
                        numBytes = stream.Read(picture, 0, MAXREAD);
                        if (numBytes > 0)
                        {
                            // convert the stream of bytes to an image
                            Image img = (Bitmap)((new ImageConverter()).ConvertFrom(picture));
                            logging.Log("Recieved image from Application client", MessageTypeEnum.INFO);
                            ServiceInfo info = ServiceInfo.CreateServiceInfo();
                            // save the image
                            img.Save(info.Handlers[0]);
                            logging.Log("Saved image from Application client", MessageTypeEnum.INFO);
                        }
                        // initialize byte array
                        picture.Initialize();
                    }
                    catch (Exception e)
                    {
                        logging.Log("Error reading from Application client. Exiting client handler",
                            MessageTypeEnum.FAIL);
                        break;
                    }
                }
            }).Start();
        }

        /*public void HandleClientTest(byte[] image)
        {
            try
            {
                Image img = (Bitmap)((new ImageConverter()).ConvertFrom(image));
                logging.Log("Recieved image from Application client", MessageTypeEnum.INFO);
                // save the image
                img.Save("C:\\Users\\Larry\\Desktop\\test\\from\\download.jpg");
                logging.Log("Saved image from Application client", MessageTypeEnum.INFO);
            }
            catch (Exception e)
            {
                logging.Log("Error reading from Application client. Exiting client handler",
                    MessageTypeEnum.FAIL);
            }
        }*/
    }
}
