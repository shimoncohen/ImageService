using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    class GetConfigCommand : ICommand
    {
        public GetConfigCommand()
        {

        }

        public string Execute(string[] args, out bool result)
        {
            ServiceInfo info = ServiceInfo.CreateServiceInfo();
            result = true;
            string handlers = String.Join(",", info.Handlers);
            return info.OutputDir + "," + info.SourceName + "," + info.LogName + "," + info.ThumbnailSize + "," + handlers;
        }
    }
}
