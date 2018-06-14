using ImageService.Logging;
using ImageService.Server.Handlers;
using System.Drawing;

namespace ImageService.ImageService.ImageService.Server.Handlers
{
    class TestHandler
    {

        static void Main(string[] args)
        {
            ILoggingService logger = new LoggingService();
            Image image = Image.FromFile("C:\\Users\\Larry\\Desktop\\download.jpg");
            ApplicationClientHandler handler = new ApplicationClientHandler(logger);
            ImageConverter _imageConverter = new ImageConverter();
            byte[] xByte = (byte[])_imageConverter.ConvertTo(image, typeof(byte[]));
            //handler.HandleClientTest(xByte);
        }
    }
}
