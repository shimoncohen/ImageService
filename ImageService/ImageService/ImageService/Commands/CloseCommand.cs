using ImageService.Logging.Modal.Event;
using System;

namespace ImageService.Commands
{
    class CloseCommand : ICommand
    {
        public event EventHandler<DirectoryCloseEventArgs> Closed;

        public string Execute(string[] args, out bool result)
        {
            // create event arguments
            DirectoryCloseEventArgs e = new DirectoryCloseEventArgs(args[0], "Closing handler");
            // invoke event notifying that a handler needs to close
            Closed?.Invoke(this, e);
            result = true;
            return "Executed Close Command with arguments: " + args[0];
        }
    }
}
