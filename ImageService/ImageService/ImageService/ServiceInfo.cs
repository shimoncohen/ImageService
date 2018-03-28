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
        private  string[] handlers;
        private  string outputDir;
        private  string sourceName;
        private  string logName;
        private  int thumbnailSize;

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
            this.handlers = handlerName.Split(';');
            this.outputDir = ConfigurationManager.AppSettings["OutputDir"];
            this.logName = ConfigurationManager.AppSettings["LogName"];
            this.sourceName = ConfigurationManager.AppSettings["SourceName"];
            int.TryParse(ConfigurationManager.AppSettings["ThumbnailSize"], out result);
            // if parse was succesfull
            if (result != 0)
            {
                this.thumbnailSize = int.Parse(ConfigurationManager.AppSettings["ThumbnailSize"]);
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
