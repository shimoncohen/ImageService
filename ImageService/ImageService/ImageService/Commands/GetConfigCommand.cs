using ImageService.Infrastructure.Enums;
using ImageService.Logging.Modal.Event;
using Newtonsoft.Json;
using System;

namespace ImageService.Commands
{
    class GetConfigCommand : ICommand
    {

        /// <summary>
        /// the function executes the get config command
        /// </summary>
        /// <param name= args> the commands arguments </param>
        /// <param name= result> the result of the function, if succeeded or not </param>
        /// <return> returns the app config info as a string </return>
        public string Execute(string[] args, out bool result)
        {
            // getsa the instance of the app config info
            ServiceInfo info = ServiceInfo.CreateServiceInfo();
            result = true;
            string handlers = String.Join(",", info.Handlers);
            string[] answer = (info.OutputDir + "," + info.SourceName + "," + info.LogName + "," + info.ThumbnailSize + "," + handlers).Split(',');
            // return the info converted to Json ready to be sent to client
            InfoEventArgs infoArgs = new InfoEventArgs((int)EnumTranslator.CommandToInfo((int)CommandEnum.GetConfigCommand), answer);
            return JsonConvert.SerializeObject(infoArgs);
        }
    }
}
