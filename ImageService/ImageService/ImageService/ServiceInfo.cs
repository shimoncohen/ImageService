using System.Collections;
using System.Collections.Generic;
using System.Configuration;

namespace ImageService
{
     /// <summary>
     /// A singelton holding the services creation information.
     /// gets information from AppConfig and parse it.
     /// </summary>
    public class ServiceInfo
    {
        //private readonly List<string> handlers;
        public List<string> Handlers { get; }
        //private readonly string outputDir;
        public string OutputDir { get; }
        //private readonly string sourceName;
        public string SourceName { get; }
        //private readonly string logName;
        public string LogName { get; }
        //private readonly int thumbnailSize;
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
            Handlers = new List<string>();
            // add all of the handler paths to the handler list
            foreach (string handler in handlerName.Split(';'))
            {
                Handlers.Add(handler);
            }
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

        /// <summary>
        /// request a new instance of the class.
        /// </summary>
        /// <param name= path> the path of the handler to remove. </param>
        /// <return> removes a handler from the handler list </return>
        public void RemoveHandler(string path)
        {
            Handlers.Remove(path);
            string handlerName = ConfigurationManager.AppSettings["Handler"];
            List<string> temp = new List<string>();
            // add all of the handler paths to the handler list
            foreach (string handler in handlerName.Split(';'))
            {
                temp.Add(handler);
            }
            temp.Remove(path);
        }
    }
}