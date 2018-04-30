using ImageService.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    class LogCommand : ICommand
    {
        ILoggingService log;

        public LogCommand()
        {

        }

        public string Execute(string[] args, out bool result)
        {
            throw new NotImplementedException();
        }
    }
}
