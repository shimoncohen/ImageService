using ImageService.Controller;
using ImageService.Logging;
using ImageService.Modal;
using ImageService.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService
{
    class mainClass
    {
        public static void Main(string[] args)
        {
            ServiceInfo info = ServiceInfo.CreateServiceInfo();
            IImageServiceModal model = new ImageServiceModal(info.OutputDir, info.ThumbnailSize);
            ImageController controller = new ImageController(model);
            ILoggingService logger = new LoggingService();
            ImageServer s = new ImageServer(controller, info.Handlers.ToArray(), logger);
            TcpServer tcpServer = new TcpServer(controller, logger);
            tcpServer.Start();
        }
    }
}
