using ImageService.Logging.Modal.Event;
using System;

namespace ImageService.Commands
{
    class CloseCommand : ICommand
    {
        public event EventHandler<DirectoryCloseEventArgs> Closed;

        public string Execute(string[] args, out bool result)
        {
            DirectoryCloseEventArgs e = new DirectoryCloseEventArgs(args[0], "Closing handler");
            Closed?.Invoke(this, e);
            result = true;
            return "Executed Close Command with arguments: " + args[0];
        }
    }
}
