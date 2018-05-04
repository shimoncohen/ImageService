using ImageService.Controller.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    class CloseCommand : ICommand
    {
        Func<List<IDirectoryHandler>> func;

        public CloseCommand(Func<List<IDirectoryHandler>> function)
        {
            func = function;
        }

        public string Execute(string[] args, out bool result)
        {
            List<IDirectoryHandler> list = func();
            foreach(IDirectoryHandler handler in list)
            {
                handler.StopHandleDirectory(args[0]);
            }
            result = true;
            return "Executed Close Command with arguments: " + args[0];
        }
    }
}
