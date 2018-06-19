using ImageService.Logging;
using ImageService.Server.Handlers;
using System;
using System.Drawing;

namespace ImageService.ImageService.ImageService.Server.Handlers
{
    class TestHandler
    {

        static void Main(string[] args)
        {
            ILoggingService logger = new LoggingService();
            Image image = Image.FromFile("C:\\Users\\NadavSpitzer\\Desktop\\test\\to\\2018\\6\\name(1).png");
            ApplicationClientHandler handler = new ApplicationClientHandler(logger);
            ImageConverter _imageConverter = new ImageConverter();
            byte[] xByte = (byte[])_imageConverter.ConvertTo(image, typeof(byte[]));
            Console.WriteLine("I hate this");
            //handler.HandleClientTest(xByte);
        }
    }
}
