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
            stream = client.GetStream();
            // as long as the client is connected
            while (client.Connected)
            {
                while (true) { 

                    byte[] thisByte = new byte[1] { 0 };
                    List<byte> currBytes = new List<byte>();
                
                    try
                    {
                        while (thisByte[0] != (byte) '\n')
                        {
                            this.stream.Read(thisByte, 0, 1);
                            if (thisByte[0] != (byte) '\n')
                            {
                                currBytes.Add(thisByte[0]);
                            }
                        }

                        // convert to the size of the picture to int
                        string picStr = Encoding.ASCII.GetString(currBytes.ToArray(), 0, currBytes.ToArray().Length);
                        int picSize;
                        bool successful = int.TryParse(picStr, out picSize);
                        if(!successful)
                        {
                            continue;
                        }

                        // if the string is End\n we reached the end of the current picture
                        if(picStr.Equals("End\n")) { break; }

                        // get the name of the picture
                        thisByte[0] = 0;
                        currBytes = new List<byte>();
                        while (!this.stream.DataAvailable) { }
                        while (thisByte[0] != (byte)'\n')
                        {
                            this.stream.Read(thisByte, 0, 1);
                            if (thisByte[0] != (byte)'\n' &&
                                thisByte[0] != 0)
                            {
                                currBytes.Add(thisByte[0]);
                            }
                        }
                        // convert to string
                        string picName = Encoding.ASCII.GetString(currBytes.ToArray(), 0, currBytes.ToArray().Length);

                        // get the picture
                        byte[] bytes = new byte[picSize];
                        int bytesReadFirst = stream.Read(bytes, 0, bytes.Length);
                        int tempBytes = bytesReadFirst;
                        while(tempBytes < bytes.Length)
                        {
                            tempBytes += stream.Read(bytes, tempBytes, bytes.Length - tempBytes);
                        }
                        

                        ServiceInfo info = ServiceInfo.CreateServiceInfo();
                        // save the image
                        string directory = info.Handlers[0];
                        File.WriteAllBytes(Path.Combine(directory, picName), bytes);
                        logging.Log("Saved image from Application client", MessageTypeEnum.INFO);
                    }
                    catch (Exception e)
                    {
                        logging.Log("Error reading from Application client. Exiting client handler",
                            MessageTypeEnum.FAIL);
                        break;
                    }
                }
            }
            stream.Close();
            client.Close();
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
