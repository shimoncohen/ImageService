using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace ImageService
{
    /*
     * A singelton holding the services creation information
     */
    class ServiceInfo
    {
        //private readonly string[] handlers;
        //private readonly string outputDir;
        //private readonly string sourceName;
        //private readonly string logName;
        //private readonly int thumbnailSize;

        public string[] Handlers { get; }
        public string OutputDir { get; }
        public string SourceName { get; }
        public string LogName { get; }
        public int ThumbnailSize { get; }

        private static ServiceInfo serviceInfo;

        // constructor
        private ServiceInfo()
        {
            int result;
            // extract info from app config file
            string handlerName = ConfigurationManager.AppSettings["Handler"];
            this.Handlers = handlerName.Split(';');
            this.OutputDir = ConfigurationManager.AppSettings["OutputDir"];
            this.LogName = ConfigurationManager.AppSettings["LogName"];
            this.SourceName = ConfigurationManager.AppSettings["SourceName"];
            int.TryParse(ConfigurationManager.AppSettings["ThumbnailSize"], out result);
            // if parse was succesfull
            if (result != 0)
            {
                this.ThumbnailSize = int.Parse(ConfigurationManager.AppSettings["ThumbnailSize"]);
            }
        }

        // request a new instance of the calss
        public static ServiceInfo CreateServiceInfo()
        {
            // if not already created
            if(serviceInfo == null)
            {
                serviceInfo = new ServiceInfo();
            }
            // otherwise create new instance
            return serviceInfo;
        }
    }
}
