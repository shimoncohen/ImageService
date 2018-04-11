using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace ImageService
{
     /// <summary>
     /// A singelton holding the services creation information.
     /// gets information from AppConfig and parse it.
     /// </summary>
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

        /// <summary>
        /// construtor
        /// </summary>
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
        
         /// <summary>
         /// request a new instance of the class.
         /// </summary>
         /// <return> returns the instance of the object (singleton) </return>
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
